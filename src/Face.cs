using System;

namespace RadiantMapToWavefrontObj
{
    public class Face
    {
        private Vertex[] _vertices;

        // Empty contructor that creates an empty face.
        public Face()
        {
            _vertices = new Vertex[3];
        }

        // Empty contructor that creates a new face with 3 vertices.
        public Face(Vertex[] vertices)
        {
            _vertices = vertices;
        }

        // Returns the vertices of the face.
        public Vertex[] GetVertices()
        {
            return _vertices;
        }

        // Set a new set of vertices.
        public void SetVertices(Vertex[] vertices)
        {
            _vertices[0] = vertices[0];
            _vertices[1] = vertices[1];
            _vertices[2] = vertices[2];
        }

        // Set a certain vertex to another vertex.
        public void SetVertex(int index, Vertex vertex)
        {
            if (index < _vertices.Length)
                _vertices[index] = vertex;
        }

        // Get a certain vertex.
        public Vertex Vertex(int index)
        {
            if (index < 0 || index > 2)
                return null;

            return _vertices[index];
        }

        // Swap around two vertices.
        public void Swap(int first, int second)
        {
            if (first < 0 || first >= 3 || second < 0 || second >= 3)
                return;
            Vertex temp = _vertices[first];
            _vertices[first] = _vertices[second];
            _vertices[second] = temp;
        }

        // Calculate and return the normal of the face.
        public Vector GetNormal()
        {
            Vector v1 = (Vector)_vertices[1] - (Vector)_vertices[0];
            Vector v2 = (Vector)_vertices[2] - (Vector)_vertices[0];

            return Vector.CrossProduct(v1, v2) * -1;
        }

        // Finds the center and radius of a circumcircle of this triangle.
        public Tuple<Vertex, double> FindCircumcircle()
        {
            // Find center.
            Vector v0 = (Vector)_vertices[1] - (Vector)_vertices[0];
            Vector v1 = (Vector)_vertices[2] - (Vector)_vertices[0];

            Vector vx = Vector.CrossProduct(v0, v1);

            Vector centerVector = (Vector.CrossProduct(vx, v0) * v1.SquareLength() + Vector.CrossProduct(v1, vx) * v0.SquareLength()) / (2 * vx.SquareLength());
            Vertex center = _vertices[0] + centerVector;

            // Find radius.
            double radius = centerVector.Length();

            return new Tuple<Vertex, double>(center, radius);
        }

        // Returns a stringified version of the object.
        public override string ToString()
        {
            return "(" + _vertices[0] + ", " + _vertices[1] + ", " + _vertices[2] + ")";
        }

        // Checks if two faces are equal.
        public override bool Equals(object obj)
        {
            if (obj is Face)
            {
                Face that = (Face)obj;
                if (_vertices[0] == that._vertices[0] && _vertices[1] == that._vertices[1] && _vertices[2] == that._vertices[2])
                    return true;
            }
            return false;
        }

        // Gets the hashcode of a face.
        public override int GetHashCode()
        {
            return _vertices[0].GetHashCode() * 2 + _vertices[1].GetHashCode() * 4 + _vertices[2].GetHashCode() * 8;
        }

        // Override == operator.
        public static bool operator ==(Face a, Face b)
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
        public static bool operator !=(Face a, Face b)
        {
            return !(a == b);
        }
    }
}
