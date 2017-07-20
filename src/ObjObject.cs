using System;
using System.Collections.Generic;

namespace RadiantMapToWavefrontObj
{
    public class ObjObject
    {
        public readonly Vertex[] Vertices;
        public readonly Face[] Faces;
        private string _name;

        // Constructor for an .obj object.
        public ObjObject(string name, Vertex[] vertices, Face[] faces)
        {
            _name = name;
            Vertices = vertices;
            Faces = faces;
        }

        // Returns the name of the object.
        public string GetName()
        {
            return _name;
        }

        // Sets the name of the object.
        public void SetName(string name)
        {
            _name = name;
        }

        // Returns .obj formatted code version of this object.
        public string ToCode(double scale, int faceOffset)
        {
            string res = "o " + _name + "\n";

            // Write vertices.
            foreach (Vertex vertex in Vertices)
            {
                string x = (vertex.X * scale).ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                string y = (-vertex.Z * scale).ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                string z = (-vertex.Y * scale).ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                res += "v " + x + " " + y + " " + z + "\n";
            }
            /*
            // Write vertex normals. WIP
            foreach (Vertex vertex in Vertices)
            {
                string x = vertex.GetNormal().X.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                string y = vertex.GetNormal().Y.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                string z = vertex.GetNormal().Z.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                res += "vn " + x + " " + y + " " + z + "\n";
            }
            */

            if (Faces == null)
            {
                Console.WriteLine("NULL FACES: " + _name);
                return res;
            }

            // Write faces.
            foreach (Face face in Faces)
            {
                int v1 = Vertices.IndexOf(face.Vertex(0)) + 1 + faceOffset;
                int v2 = Vertices.IndexOf(face.Vertex(1)) + 1 + faceOffset;
                int v3 = Vertices.IndexOf(face.Vertex(2)) + 1 + faceOffset;
                //res += "f " + v1 + "//" + v1 + " " + v2 + "//" + v2 + " " + v3 + "//" + v3 + "\n";    //WIP with vertex normals. Broken and not even sure if we want this :s
                res += "f " + v1 + " " + v2 + " " + v3 + "\n";
            }

            return res;
        }

        // Converts a radiant brush to an obj object.
        public static ObjObject CreateFromBrush(string name, Brush brush)
        {
            Vertex[] vertices = FindIntersections(brush.ClippingPlanes);
            Face[] faces = CreateFaces(vertices, brush.ClippingPlanes);
            //Vector[] normals = CreateNormals(brush.ClippingPlanes);                           // WIP

            return new ObjObject(name, vertices, faces);
        }

        // Create normals of the object based on the clipping plane intersections.
        private static Vector[] CreateNormals(ClippingPlane[] planes)
        {
            List<Vector> normals = new List<Vector>();

            foreach (ClippingPlane plane in planes)
                normals.Add(plane.Normal);

            return normals.ToArray();
        }

        // Create a list of all intersection points of each set of three clipping planes.
        private static Vertex[] FindIntersections(ClippingPlane[] planes)
        {
            List<Vertex> intersections = new List<Vertex>();

            // Check every unique combination of three clipping planes and see if we can find an intersection point.
            for (int i = 0; i < planes.Length; ++i)
            {
                for (int j = i; j < planes.Length; ++j)
                {
                    if (i == j)
                        continue;
                    for (int k = j; k < planes.Length; ++k)
                    {
                        if (i == k || j == k)
                            continue;
                        Vertex intersection;
                        if (ClippingPlane.FindIntersection(planes[i], planes[j], planes[k], out intersection))
                        {
                            // Checks if there does not exist a clipping plane with which we are in front.
                            // Would result in vertices being added outside of our object.
                            bool rightSide = true;
                            for (int l = 0; l < planes.Length; ++l)
                            {
                                if (planes[l] != planes[i] && planes[l] != planes[j] && planes[l] != planes[k])
                                {
                                    double dot = Vector.DotProduct((Vector)intersection, planes[l].Normal) - planes[l].D;
                                    if (dot > 0)
                                    {
                                        rightSide = false;
                                        break;
                                    }
                                }
                            }
                            if (rightSide && !intersections.Contains(intersection))
                                intersections.Add(intersection);
                        }
                    }
                }
            }

            return intersections.ToArray();
        }

        // Use all vertices to create triangle faces of the object.
        private static Face[] CreateFaces(Vertex[] vertices, ClippingPlane[] planes)
        {
            List<Face> faces = new List<Face>();

            for (int i = 0; i < planes.Length; ++i)
            {
                Vertex[] verts = planes[i].FindVerticesInPlane(vertices);

                // Abort when there are no vertices anyway. Something went wrong...
                if (verts.Length < 3)
                    return null;

                foreach (Face face in BowyerWatson(verts))
                    faces.Add(face);
            }

            return faces.ToArray();
        }

        // Apply Bowyer-Watson algorithm to triangulate all the points in a plane.
        // Pseudo code taken from related wikipedia page and provided on the side in comments.
        public static Face[] BowyerWatson(Vertex[] vertices)
        {
            List<Face> triangles = new List<Face>();

            // Add super triangle to list.
            Face superTriangle = FindSuperTriangle(vertices);
            Vertex[] superVertices = superTriangle.GetVertices();
            triangles.Add(superTriangle);

            // Add points.
            foreach (Vertex v in vertices)                                                              // for each point in pointList do
            {
                List<Face> badTriangles = new List<Face>();                                             // badTriangles := empty set
                foreach (Face triangle in triangles)                                                    // for each triangle in triangulation do
                {
                    if (InCircumsphere(v, triangle))                                                    // if point is inside circumcircle of triangle
                        badTriangles.Add(triangle);                                                     // add triangle to badTriangles
                }

                List<Edge> polygon = new List<Edge>();                                                  // polygon := empty set

                foreach (Face triangle in badTriangles)                                                 // for each triangle in badTriangles do
                {
                    foreach (Edge edge in triangle.GetEdges())                                          // for each edge in triangle do
                    {
                        bool shared = false;
                        foreach (Face otherTriangle in badTriangles)                                    // if edge is not shared by any other triangles in badTriangles
                        {
                            if (triangle == otherTriangle)
                                continue;
                            if (otherTriangle.GetEdges().Contains(edge) || otherTriangle.GetEdges().Contains(edge.GetInverse()))
                            {
                                shared = true;
                                break;
                            }
                        }
                        if (!shared)
                            polygon.Add(edge);                                                          // add edge to polygon
                    }
                }

                foreach (Face triangle in badTriangles)                                                 // for each triangle in badTriangles do
                    triangles.Remove(triangle);                                                         // remove triangle from triangulation

                foreach (Edge e in polygon)                                                             // for each edge in polygon do
                    triangles.Add(new Face(e.A, e.B, v));                                               // newTri := form a triangle from edge to point + add newTri to triangulation
            }

            List<Face> result = new List<Face>();

            foreach (Face t in triangles)                                                               // for each triangle in triangulation
            {
                Vertex[] curVertices = t.GetVertices();
                if (!curVertices.Contains(superVertices[0])                                             // if triangle contains a vertex from original super-triangle
                    && !curVertices.Contains(superVertices[1])
                    && !curVertices.Contains(superVertices[2]))
                    result.Add(t);                                                                      // remove triangle from triangulation
            }

            return result.ToArray();                                                                    // return triangulation
        }

        // Finds the Bowyer-Watson super triangle of a set of vertices.
        private static Face FindSuperTriangle(Vertex[] vertices)
        {
            // Setup super triangle.
            double minX, maxX, minY, maxY, minZ, maxZ;

            minX = minY = minZ = Double.MaxValue;
            maxX = maxY = maxZ = Double.MinValue;

            foreach (Vertex v in vertices)
            {
                if (v.X < minX)
                    minX = v.X;
                if (v.X > maxX)
                    maxX = v.X;

                if (v.Y < minY)
                    minY = v.Y;
                if (v.Y > maxY)
                    maxY = v.Y;

                if (v.Z < minZ)
                    minZ = v.Z;
                if (v.Z > maxZ)
                    maxZ = v.Z;
            }

            ClippingPlane plane = new ClippingPlane(vertices[0], vertices[1], vertices[2]);

            Vertex a = new Vertex(minX, minY, minZ);
            Vertex b = new Vertex(maxX, maxY, maxZ);

            Vector ab = (Vector)(b - a);
            a -= 10*ab;
            b += 10*ab;

            Vector triBase = Vector.CrossProduct(ab, plane.Normal).Unit();

            double length = ((Vector)(b - a)).Length();

            Vertex c = a + triBase * length;
            Vertex d = a - triBase * length;

            return new Face(b, c, d);
        }

        // Checks if a point lies in the circumsphere of a face.
        private static bool InCircumsphere(Vertex point, Face face)
        {
            Tuple<Vertex, double> cs = face.GetCircumsphere();
            if (point.Distance(cs.Item1) < cs.Item2)
                return true;
            return false;
        }

        // Fix normals of faces pointing in the wrong direction.
        private static void FixNormal(Face face, Vector normal)
        {
            // Check if the normal is correct, if not, invert the face.
            if (VerifyNormal(face, normal))
                return;
            face.Swap(0, 2);
        }

        // Checks if a normal of a face is equal to a certain normal.
        private static bool VerifyNormal(Face face, Vector normal)
        {
            Vector v1 = (Vector)face.Vertex(1) - (Vector)face.Vertex(0);
            Vector v2 = (Vector)face.Vertex(2) - (Vector)face.Vertex(0);

            Vector faceNormal = Vector.CrossProduct(v1, v2)*-1;
            if (normal.DirectionEquals(faceNormal))
                return true;
            return false;
        }
    }
}
