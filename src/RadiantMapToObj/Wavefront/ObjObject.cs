using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using RadiantMapToObj.Configuration;
using RadiantMapToObj.Internal;

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
        /// <param name="vertices">The vertices.</param>
        /// <param name="faces">The faces.</param>
        public ObjObject(IEnumerable<Vector> vertices, IEnumerable<Face> faces)
        {
            Vertices = vertices.ToList();
            Faces = faces.ToList();
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
        /// Converts to .obj file content.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="faceOffset">The face offset.</param>
        /// <returns>The .obj file content.</returns>
        public string ToCode(string name, double scale, int faceOffset)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("o ").AppendLine(name);

            // Write vertices.
            foreach (Vector vertex in Vertices)
            {
                string x = (-vertex.X * scale).ToString("0.000000", CultureInfo.InvariantCulture);
                string y = (-vertex.Z * scale).ToString("0.000000", CultureInfo.InvariantCulture);
                string z = (vertex.Y * scale).ToString("0.000000", CultureInfo.InvariantCulture);
                sb.Append("v ").Append(x).Append(' ').Append(y).Append(' ').AppendLine(z);
            }

            // Write faces.
            foreach (Face face in Faces)
            {
                int v1 = Vertices.IndexOf(face.A) + 1 + faceOffset;
                int v2 = Vertices.IndexOf(face.B) + 1 + faceOffset;
                int v3 = Vertices.IndexOf(face.C) + 1 + faceOffset;
                sb.Append("f ").Append(v1).Append(' ').Append(v2).Append(' ').Append(v3).AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes all textures that are in the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void FilterTextures(Filter filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            List<Face> newFaces = new List<Face>();

            foreach (Face face in Faces)
            {
                if (!filter.Contains(face.Texture))
                {
                    newFaces.Add(face);
                }
            }

            Faces = newFaces;

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

            Vertices = newVertices;
        }
    }
}
