
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
    }
}
