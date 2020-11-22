using System.Collections.Generic;
using System.Linq;

namespace RadiantMapToObj.Wavefront
{
    /// <summary>
    /// Represents Wavefront Obj objects.
    /// </summary>
    public class ObjObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjObject"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="vertices">The vertices.</param>
        /// <param name="faces">The faces.</param>
        public ObjObject(string name, IEnumerable<Vector> vertices, IEnumerable<Face> faces)
        {
            Name = name;
            Vertices = vertices;
            Faces = faces;
            Cleanup();
        }

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        public IEnumerable<Vector> Vertices { get; private set; }

        /// <summary>
        /// Gets the faces.
        /// </summary>
        public IEnumerable<Face> Faces { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Converts to .obj file content.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <param name="faceOffset">The face offset.</param>
        /// <returns>The .obj file content.</returns>
        public string ToCode(double scale, int faceOffset)
        {
            string res = "o " + Name + "\n";

            // Write vertices.
            foreach (Vector vertex in Vertices)
            {
                string x = (vertex.X * scale).ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                string y = (-vertex.Z * scale).ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                string z = (-vertex.Y * scale).ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                res += "v " + x + " " + y + " " + z + "\n";
            }

            // Write faces.
            foreach (Face face in Faces)
            {
                int v1 = Vertices.IndexOf(face.A) + 1 + faceOffset;
                int v2 = Vertices.IndexOf(face.B) + 1 + faceOffset;
                int v3 = Vertices.IndexOf(face.C) + 1 + faceOffset;
                res += "f " + v1 + " " + v2 + " " + v3 + "\n";
            }

            return res;
        }

        /// <summary>
        /// Removes all textures that are in the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void FilterTextures(string[] filter)
        {
            List<Face> newFaces = new List<Face>();

            foreach (Face face in Faces)
            {
                if (!filter.Contains(face.Texture))
                {
                    newFaces.Add(face);
                }
            }

            Faces = newFaces.ToArray();
            Cleanup();
        }

        /// <summary>
        /// Removes all vertices without faces.
        /// </summary>
        private void Cleanup()
        {
            if (Faces == null)
            {
                return;
            }

            List<Vector> newVertices = new List<Vector>();

            foreach (Vector vertex in Vertices)
            {
                bool contained = false;

                foreach (Face face in Faces)
                {
                    if (face.Contains(vertex))
                    {
                        contained = true;
                        break;
                    }
                }

                if (contained)
                {
                    newVertices.Add(vertex);
                }
            }

            Vertices = newVertices.ToArray();
        }
    }
}
