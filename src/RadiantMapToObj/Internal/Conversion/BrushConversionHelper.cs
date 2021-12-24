using System;
using System.Collections.Generic;
using System.Linq;
using RadiantMapToObj.Configuration;
using RadiantMapToObj.Quake;
using RadiantMapToObj.Quake.Hammer;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Internal.Conversion
{
    /// <summary>
    /// Provides helper functions for converting brushes to objects.
    /// </summary>
    internal static class BrushConversionHelper
    {
        /// <summary>
        /// Converts a radiant brush to an obj object.
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <param name="settings">The conversion settings.</param>
        /// <returns>A newly created obj object.</returns>
        public static ObjObject Convert(Brush brush, ConversionSettings settings)
        {
            IEnumerable<Vector> vertices = FindIntersections(brush.ClippingPlanes);

            DisplacementClippingPlane? displacement = brush.ClippingPlanes.FirstOrDefault(x => x is DisplacementClippingPlane) as DisplacementClippingPlane;
            if (displacement != null)
            {
                return DisplacementConversionHelper.Convert(displacement, vertices);
            }

            IEnumerable<Face> faces = CreateFaces(vertices, brush.ClippingPlanes, settings);
            return new ObjObject(vertices, faces);
        }

        /// <summary>
        /// Create a list of all intersection points of each set of three clipping planes.
        /// </summary>
        /// <param name="planes">The planes.</param>
        /// <returns>Gets all intersections of the given planes.</returns>
        private static IEnumerable<Vector> FindIntersections(IEnumerable<ClippingPlane> planes)
        {
            List<Vector> intersections = new List<Vector>();

            // Check every unique combination of three clipping planes and see if we can find an intersection point.
            int i = 0;
            foreach (ClippingPlane planeI in planes)
            {
                int j = ++i;
                foreach (ClippingPlane planeJ in planes.Skip(j))
                {
                    foreach (ClippingPlane planeK in planes.Skip(++j))
                    {
                        if (ClippingPlane.FindIntersection(planeI, planeJ, planeK, out Vector? intersection))
                        {
                            // Checks if there does not exist a clipping plane with which we are in front.
                            // Would result in vertices being added outside of our object.
                            bool rightSide = true;
                            foreach (ClippingPlane planeL in planes)
                            {
                                if (planeL != planeI && planeL != planeJ && planeL != planeK)
                                {
                                    double dot = Vector.DotProduct(intersection!, planeL.Normal) - planeL.D;
                                    if (dot > 0)
                                    {
                                        rightSide = false;
                                        break;
                                    }
                                }
                            }

                            if (rightSide && !intersections.Contains(intersection!))
                            {
                                intersections.Add(intersection!);
                            }
                        }
                    }
                }
            }

            return intersections;
        }

        /// <summary>
        /// Use all vertices to create triangle faces of the object.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="planes">The planes.</param>
        /// <param name="settings">The conversion settings.</param>
        /// <returns>The faces of the object.</returns>
        private static IEnumerable<Face> CreateFaces(IEnumerable<Vector> vertices, IEnumerable<ClippingPlane> planes, ConversionSettings settings)
        {
            List<Face> faces = new List<Face>();

            TextureFinder tf = new TextureFinder(settings.Textures);

            foreach (ClippingPlane plane in planes)
            {
                IEnumerable<Vector> verts = plane.FindVerticesInPlane(vertices);

                foreach (Face face in BowyerWatson(verts, plane.Texture.Name))
                {
                    Face fixedFace = FixNormal(face, plane.Normal);
                    Face texturedFace = ApplyTextures(fixedFace, plane, tf);
                    faces.Add(texturedFace);
                }
            }

            return faces;
        }

        /// <summary>
        /// Apply Bowyer-Watson algorithm to triangulate all the points in a plane.
        /// Pseudo code taken from related wikipedia page and provided on the side in comments.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="texture">The texture of the faces.</param>
        /// <returns>The faces creates by the bowyer watson algorithm.</returns>
        private static IEnumerable<Face> BowyerWatson(IEnumerable<Vector> vertices, string texture)
        {
            if (vertices is null)
            {
                throw new ArgumentNullException(nameof(vertices));
            }

            if (!vertices.CountAtLeast(3))
            {
                Console.WriteLine("WARNING found plane with less than 3 vertices.");
                return Array.Empty<Face>();

                // TODO Handle this better
                // throw new ArgumentException("Requires at least 3 vertices.", nameof(vertices));
            }

            HashSet<Face> triangles = new HashSet<Face>();

            // Add super triangle to list.
            Face superTriangle = FindSuperTriangle(vertices, texture);
            triangles.Add(superTriangle);

            // for each point in pointList do
            foreach (Vector v in vertices)
            {
                // badTriangles := empty set
                HashSet<Face> badTriangles = new HashSet<Face>();

                // for each triangle in triangulation do
                foreach (Face triangle in triangles)
                {
                    // if point is inside circumcircle of triangle
                    if (InCircumsphere(v, triangle))
                    {
                        // add triangle to badTriangles
                        badTriangles.Add(triangle);
                    }
                }

                // polygon := empty set
                HashSet<Edge> polygon = new HashSet<Edge>();

                // for each triangle in badTriangles do
                foreach (Face triangle in badTriangles)
                {
                    // for each edge in triangle do
                    foreach (Edge edge in triangle.GetEdges())
                    {
                        bool shared = false;

                        // if edge is not shared by any other triangles in badTriangles
                        foreach (Face otherTriangle in badTriangles)
                        {
                            if (triangle == otherTriangle)
                            {
                                continue;
                            }

                            if (otherTriangle.GetEdges().Contains(edge) || otherTriangle.GetEdges().Contains(edge.Inverse))
                            {
                                shared = true;
                                break;
                            }
                        }

                        if (!shared)
                        {
                            // add edge to polygon
                            polygon.Add(edge);
                        }
                    }
                }

                // for each triangle in badTriangles do
                foreach (Face triangle in badTriangles)
                {
                    // remove triangle from triangulation
                    triangles.Remove(triangle);
                }

                // for each edge in polygon do
                foreach (Edge e in polygon)
                {
                    // newTri := form a triangle from edge to point + add newTri to triangulation
                    triangles.Add(new Face(e.A, e.B, v, texture));
                }
            }

            HashSet<Face> result = new HashSet<Face>();

            // for each triangle in triangulation
            foreach (Face t in triangles)
            {
                IEnumerable<Vector> curVertices = t.Vertices;

                // if triangle contains a vertex from original super-triangle
                if (!curVertices.Contains(superTriangle.A)
                    && !curVertices.Contains(superTriangle.B)
                    && !curVertices.Contains(superTriangle.C))
                {
                    // remove triangle from triangulation
                    result.Add(t);
                }
            }

            // return triangulation
            return result;
        }

        /// <summary>
        /// Finds the Bowyer-Watson super triangle of a set of vertices.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="texture">The texture of the faces.</param>
        /// <returns>The found super triangle.</returns>
        private static Face FindSuperTriangle(IEnumerable<Vector> vertices, string texture)
        {
            // Setup super triangle.
            double minX, maxX, minY, maxY, minZ, maxZ;

            minX = minY = minZ = double.MaxValue;
            maxX = maxY = maxZ = double.MinValue;

            foreach (Vector v in vertices)
            {
                if (v.X < minX)
                {
                    minX = v.X;
                }

                if (v.X > maxX)
                {
                    maxX = v.X;
                }

                if (v.Y < minY)
                {
                    minY = v.Y;
                }

                if (v.Y > maxY)
                {
                    maxY = v.Y;
                }

                if (v.Z < minZ)
                {
                    minZ = v.Z;
                }

                if (v.Z > maxZ)
                {
                    maxZ = v.Z;
                }
            }

            Plane plane = new Plane(vertices.Get(0), vertices.Get(1), vertices.Get(2));

            Vector a = new Vector(minX, minY, minZ);
            Vector b = new Vector(maxX, maxY, maxZ);

            Vector ab = b - a;
            a -= 10 * ab;
            b += 10 * ab;

            Vector triBase = Vector.CrossProduct(ab, plane.Normal).Unit;

            double length = (b - a).Length;

            Vector c = a + (triBase * length);
            Vector d = a - (triBase * length);

            return new Face(b, c, d, texture);
        }

        /// <summary>
        /// Checks if a point lies in the circumsphere of a face.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="face">The face.</param>
        /// <returns>True if the point is in the circumsphere, false otherwise.</returns>
        private static bool InCircumsphere(Vector point, Face face)
        {
            Tuple<Vector, double> cs = face.GetCircumsphere();
            if (point.Distance(cs.Item1) < cs.Item2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Fix normals of faces pointing in the wrong direction.
        /// </summary>
        /// <param name="face">The face.</param>
        /// <param name="normal">The normal.</param>
        private static Face FixNormal(Face face, Vector normal)
        {
            // Check if the normal is correct, if not, invert the face.
            if (VerifyNormal(face, normal))
            {
                return face;
            }

            return new Face(face.C, face.B, face.A, face.Texture);
        }

        /// <summary>
        /// Checks if a normal of a face is equal to a certain normal.
        /// </summary>
        /// <param name="face">The face.</param>
        /// <param name="normal">The normal.</param>
        /// <returns>True if the normal is equal, false otherwise.</returns>
        private static bool VerifyNormal(Face face, Vector normal)
        {
            Vector v1 = face.B - face.A;
            Vector v2 = face.C - face.A;

            Vector faceNormal = -Vector.CrossProduct(v1, v2);
            if (normal.DirectionEquals(faceNormal))
            {
                return true;
            }

            return false;
        }

        private static Face ApplyTextures(Face face, ClippingPlane plane, TextureFinder tf)
        {
            (int width, int height) = tf.FindSize(face.Texture);

            if (width == 0 || height == 0)
            {
                return face;
            }

            Vector[] vectors = face.Vertices.ToArray();

            Vertex v1 = CreateTexturedVertex(plane, vectors[0], width, height);
            Vertex v2 = CreateTexturedVertex(plane, vectors[1], width, height);
            Vertex v3 = CreateTexturedVertex(plane, vectors[2], width, height);

            return new Face(v1, v2, v3, face.Texture);
        }

        private static Vertex CreateTexturedVertex(ClippingPlane plane, Vector point, int width, int height)
        {
            double yzr = Vector.DotProduct(new Vector(1, 0, 0), plane.Normal);
            double xzr = Vector.DotProduct(new Vector(0, 1, 0), plane.Normal);
            double xyr = Vector.DotProduct(new Vector(0, 0, 1), plane.Normal);
            double yz = Math.Abs(yzr);
            double xz = Math.Abs(xzr);
            double xy = Math.Abs(xyr);

            double rad = Math.PI / 180 * -plane.Texture.Rotation;
            double crot = Math.Cos(rad);
            double srot = Math.Sin(rad);

            double xOffset = plane.Texture.OffsetX;
            double yOffset = plane.Texture.OffsetY;

            double x;
            double y;
            if (yz >= xz && yz >= xy)
            {
                x = -point.Y;
                y = point.Z;
            }
            else if (xz >= xy)
            {
                x = -point.X;
                y = point.Z;
            }
            else
            {
                x = point.X;
                y = point.Y;
                xOffset = -xOffset;
            }

            x /= -plane.Texture.ScaleX;
            y /= -plane.Texture.ScaleY;

            x = Math.Round(x);
            y = Math.Round(y);

            double a = x;
            double b = y;

            x = (crot * a) + (srot * b);
            y = -(srot * a) + (crot * b);

            x -= xOffset;
            y -= yOffset;

            double u = x / width;
            double v = y / height;

            return new Vertex(point.X, point.Y, point.Z, u, v);
        }
    }
}
