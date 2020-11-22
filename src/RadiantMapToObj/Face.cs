using System;
using System.Linq;

namespace RadiantMapToWavefrontObj
{
    /// <summary>
    /// Class for Faces.
    /// </summary>
    public class Face
    {
        // TODO: refactor vertices to 3 variables.
        private Vertex[] vertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="Face"/> class.
        /// </summary>
        public Face()
        {
            vertices = new Vertex[3];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Face"/> class.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        public Face(Vertex[] vertices)
        {
            this.vertices = vertices;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Face"/> class.
        /// </summary>
        /// <param name="a">One of the vertices.</param>
        /// <param name="b">Another of the vertices.</param>
        /// <param name="c">The last one of the vertices.</param>
        public Face(Vertex a, Vertex b, Vertex c)
        {
            vertices = new Vertex[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
        }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        public string? Texture { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="Vertex"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Vertex"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns>Vertices in face.</returns>
        public Vertex this[int index]
        {
            get => vertices[index];
            set => vertices[index] = value;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the double equals operator.
        /// </returns>
        public static bool operator ==(Face a, Face b)
        {
            if (a is null)
            {
                if (b is null)
                {
                    return true;
                }

                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Face a, Face b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Sets the texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        public void SetTexture(string texture)
        {
            Texture = texture;
        }

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        /// <returns>Returns the list of vertices.</returns>
        public Vertex[] GetVertices()
        {
            return vertices;
        }

        /// <summary>
        /// Sets the vertices.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        public void SetVertices(Vertex[] vertices)
        {
            if (vertices is null)
            {
                throw new ArgumentNullException(nameof(vertices));
            }

            this.vertices[0] = vertices[0];
            this.vertices[1] = vertices[1];
            this.vertices[2] = vertices[2];
        }

        /// <summary>
        /// Sets a vertex at an index to another vertex.
        /// </summary>
        /// <param name="index">The index of the original vertex.</param>
        /// <param name="vertex">The new vertex.</param>
        /// <exception cref="ArgumentNullException">vertices.</exception>
        public void SetVertex(int index, Vertex vertex)
        {
            if (vertex is null)
            {
                throw new ArgumentNullException(nameof(vertex));
            }

            if (index < vertices.Length)
            {
                vertices[index] = vertex;
            }
        }

        /// <summary>
        /// Gets a vertex with a certain index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The vertex with a certain intex.</returns>
        public Vertex? Vertex(int index)
        {
            if (index < 0 || index > 2)
            {
                return null;
            }

            return vertices[index];
        }

        /// <summary>
        /// Swaps the specified vertices.
        /// </summary>
        /// <param name="first">The first vertex.</param>
        /// <param name="second">The second vertex.</param>
        public void Swap(int first, int second)
        {
            if (first < 0 || first >= 3 || second < 0 || second >= 3)
            {
                return;
            }

            Vertex temp = vertices[first];
            vertices[first] = vertices[second];
            vertices[second] = temp;
        }

        /// <summary>
        /// Gets the normal.
        /// </summary>
        /// <returns>Vector normal for the face.</returns>
        public Vector GetNormal()
        {
            Vector v1 = (Vector)vertices[1] - (Vector)vertices[0];
            Vector v2 = (Vector)vertices[2] - (Vector)vertices[0];

            return Vector.CrossProduct(v1, v2) * -1;
        }

        /// <summary>
        /// Gets the circumsphere of this triangle.
        /// </summary>
        /// <returns>Circumsphere of the triangle.</returns>
        public Tuple<Vertex, double> GetCircumsphere()
        {
            Vector v0 = (Vector)vertices[1] - (Vector)vertices[0];
            Vector v1 = (Vector)vertices[2] - (Vector)vertices[0];

            Vector vx = Vector.CrossProduct(v0, v1);

            Vector centerVector = ((Vector.CrossProduct(vx, v0) * v1.SquareLength()) + (Vector.CrossProduct(v1, vx) * v0.SquareLength())) / (2 * vx.SquareLength());
            Vertex center = vertices[0] + centerVector;

            double radius = centerVector.Length();

            return new Tuple<Vertex, double>(center, radius);
        }

        /// <summary>
        /// Gets the edges.
        /// </summary>
        /// <returns> Edges of the face.</returns>
        public Edge[] GetEdges()
        {
            Edge[] edges = new Edge[3];
            edges[0] = new Edge(vertices[0], vertices[1]);
            edges[1] = new Edge(vertices[1], vertices[2]);
            edges[2] = new Edge(vertices[2], vertices[0]);
            return edges;
        }

        /// <summary>
        /// Determines whether this face contains a vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified vertex]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Vertex vertex)
        {
            return vertices.Contains(vertex);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "(" + vertices[0] + ", " + vertices[1] + ", " + vertices[2] + ")";
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Face)
            {
                Face that = (Face)obj;
                if (vertices[0] == that.vertices[0] && vertices[1] == that.vertices[1] && vertices[2] == that.vertices[2])
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return (vertices[0].GetHashCode() * 2) + (vertices[1].GetHashCode() * 4) + (vertices[2].GetHashCode() * 8);
        }
    }
}
