using System;
using System.Collections.Generic;
using System.Linq;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Internal
{
    /// <summary>
    /// Contains logic for triangulation.
    /// </summary>
    internal static class Triangulation
    {
        /// <summary>
        /// Apply Bowyer-Watson algorithm to triangulate all the points in a plane.
        /// Pseudo code taken from related wikipedia page and provided on the side in comments.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="texture">The texture of the faces.</param>
        /// <returns>The faces creates by the bowyer watson algorithm.</returns>
        public static IEnumerable<Face> BowyerWatson(IEnumerable<Vector> vertices, string texture)
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
    }
}
