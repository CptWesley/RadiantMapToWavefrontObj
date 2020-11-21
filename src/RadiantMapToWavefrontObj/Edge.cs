
using System;

namespace RadiantMapToWavefrontObj
{
    public struct Edge
    {
        public Vertex A { get; set; }
        public Vertex B { get; set; }

        // Constructor for an edge between two vertices.
        public Edge(Vertex a, Vertex b)
        {
            A = a;
            B = b;
        }

        // Returns the vector given by this edge.
        public Vector GetVector()
        {
            return (Vector) B - (Vector) A;
        }

        // Returns the length of this edge.
        public double Length()
        {
            return GetVector().Length();
        }

        // Returns the inverse edge of this edge.
        public Edge GetInverse()
        {
            return new Edge(B, A);
        }

        // Returns a stringified version of this object.
        public override string ToString()
        {
            return "<" + A + ", " + B + ">";
        }

        // Checks if two edges are equal.
        public override bool Equals(object obj)
        {
            if (obj is Edge)
            {
                Edge that = (Edge)obj;
                if (A == that.A && B == that.B)
                    return true;
            }
            return false;
        }

        // Returns a hascode for the object.
        public override int GetHashCode()
        {
            return A.GetHashCode() + 2 * B.GetHashCode();
        }

        // Override == operator.
        public static bool operator ==(Edge a, Edge b)
        {
            if (ReferenceEquals(a, null))
            {
                if (ReferenceEquals(b, null))
                    return true;
                return false;
            }
            return a.Equals(b);
        }

        // Override != operator.
        public static bool operator !=(Edge a, Edge b)
        {
            return !(a == b);
        }
    }
}
