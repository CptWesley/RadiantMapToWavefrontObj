namespace RadiantMapToObj.Quake
{
    /// <summary>
    /// Class for representing plane texture information.
    /// </summary>
    public class PlaneTexture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneTexture"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="offsetX">The x offset.</param>
        /// <param name="offsetY">The y offset.</param>
        /// <param name="rotation">The rotation in degrees.</param>
        /// <param name="scaleX">The x scale.</param>
        /// <param name="scaleY">The y scale.</param>
        public PlaneTexture(string name, double offsetX, double offsetY, double rotation, double scaleX, double scaleY)
            => (Name, OffsetX, OffsetY, Rotation, ScaleX, ScaleY) = (name, offsetX, offsetY, rotation, scaleX, scaleY);

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the x offset.
        /// </summary>
        public double OffsetX { get; }

        /// <summary>
        /// Gets the y offset.
        /// </summary>
        public double OffsetY { get; }

        /// <summary>
        /// Gets the rotation.
        /// </summary>
        public double Rotation { get; }

        /// <summary>
        /// Gets the x scale.
        /// </summary>
        public double ScaleX { get; }

        /// <summary>
        /// Gets the x scale.
        /// </summary>
        public double ScaleY { get; }
    }
}
