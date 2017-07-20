using System.Collections.Generic;

namespace RadiantMapToWavefrontObj
{
    public class Plane
    {
        public readonly double D;
        public readonly Vector Normal;

        public double A => Normal.X;
        public double B => Normal.Y;
        public double C => Normal.Z;

        // Constructor for a plane.
        public Plane(Vertex v1, Vertex v2, Vertex v3)
        {
            Vector vector1 = new Vector(v2.X - v1.X, v2.Y - v1.Y, v2.Z - v1.Z).Unit();
            Vector vector2 = new Vector(v3.X - v1.X, v3.Y - v1.Y, v3.Z - v1.Z).Unit();

            Normal = Vector.CrossProduct(vector1, vector2).Unit();
            D = -(A * v2.X + B * v2.Y + C * v2.Z);
        }

        // Returns the object as a string.
        public override string ToString()
        {
            return "<" + A + ", " + B + ", " + C + ", " + D + ">";
        }

        // Creates an array of all vertices that lie on this plane.
        public Vertex[] FindVerticesInPlane(Vertex[] vertices)
        {
            List<Vertex> res = new List<Vertex>();
            foreach (Vertex v in vertices)
            {
                if (v.OnPlane(this))
                    res.Add(v);
            }
            return res.ToArray();
        }
    }
}
