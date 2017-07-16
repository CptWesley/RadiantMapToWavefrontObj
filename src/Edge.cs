
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
    }
}
