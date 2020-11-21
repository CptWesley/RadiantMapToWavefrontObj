
namespace RadiantMapToWavefrontObj
{
    public class LineSegment                                                                // NB: This class exists solely for legacy reasons and is not being used.
    {
        public readonly Vertex V1, V2;

        // Constructor of a line segment.
        public LineSegment(Vertex begin, Vertex end)
        {
            V1 = begin;
            V2 = end;
        }

        // Check if 2 line segments intersect in 2D space where Z=0.
        public bool SegmentIntersects(LineSegment other)
        {
            return IntersectsLine(other.V1, other.V2) && other.IntersectsLine(V1, V2);
        }

        // Check if 2 lines intersect in 2D space where Z=0.
        private bool IntersectsLine(Vertex v1, Vertex v2)
        {
            return !((OnSide(v1) && OnSide(v2)) || (!OnSide(v1) && !OnSide(v2)));
        }

        // Check on what side of a line in 2D space where Z=0 a vertex is on.
        private bool OnSide(Vertex c)
        {
            double resXY = (V2.X - V1.X) * (c.Y - V1.Y) - (c.X - V1.X) * (V2.Y - V1.Y);
            return resXY > 0;
        }
    }
}
