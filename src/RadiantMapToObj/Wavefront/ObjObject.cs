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
        /// Gets the texture coordinates.
        /// </summary>
        public IEnumerable<TextureCoordinate> TextureCoordinates { get; private set; }

        /// <summary>
        /// Converts to .obj file content.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="faceVectorOffset">The face vector offset.</param>
        /// <param name="faceTextureOffset">The face texture coordinate offset.</param>
        /// <returns>The .obj file content.</returns>
        public string ToCode(string name, double scale, int faceVectorOffset, int faceTextureOffset)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("o ").AppendLine(name);

            // Write vertices.
            foreach (Vector vertex in Vertices)
            {
                string x = ToCoordinateString(-vertex.X * scale);
                string y = ToCoordinateString(-vertex.Z * scale);
                string z = ToCoordinateString(vertex.Y * scale);
                sb.Append("v ").Append(x).Append(' ').Append(y).Append(' ').AppendLine(z);
            }

            // Write texture coordinates.
            foreach (TextureCoordinate uv in TextureCoordinates)
            {
                string u = ToCoordinateString(uv.U);
                string v = ToCoordinateString(uv.V);

                sb.Append("vt ").Append(u).Append(' ').AppendLine(v);
            }

            // Write faces.
            foreach (Face face in Faces)
            {
                string v1 = GetVertexString(face.A, faceVectorOffset, faceTextureOffset);
                string v2 = GetVertexString(face.B, faceVectorOffset, faceTextureOffset);
                string v3 = GetVertexString(face.C, faceVectorOffset, faceTextureOffset);
                sb.Append("usemtl ").AppendLine(face.Texture);
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

        private static string ToCoordinateString(double x)
        {
            string result = x.ToString("0.000000", CultureInfo.InvariantCulture);

            if (x == 0 && result[0] == '-')
            {
                return result.Substring(1);
            }

            return result;
        }

        private string GetVertexString(Vector v, int faceVectorOffset, int faceTextureOffset)
        {
            int vi = Vertices.IndexOf(v) + 1 + faceVectorOffset;
            string result = vi.ToString(CultureInfo.InvariantCulture);

            if (v is Vertex vrt)
            {
                TextureCoordinate uv = new TextureCoordinate(vrt.U, vrt.V);
                int vti = TextureCoordinates.IndexOf(uv) + 1 + faceTextureOffset;
                result += "/" + vti.ToString(CultureInfo.InvariantCulture);
            }

            return result;
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
            TextureCoordinates = Faces.SelectMany(x => x.Vertices).Where(x => x is Vertex).Select(x => new TextureCoordinate(((Vertex)x).U, ((Vertex)x).V)).Distinct().ToList();
        }
    }
}
