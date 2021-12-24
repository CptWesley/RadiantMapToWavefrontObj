#pragma warning disable CS1591
#pragma warning disable CS1573
#pragma warning disable CS1572

namespace RadiantMapToObj.Quake.Hammer
{
    /// <summary>
    /// A data holder for displacement info.
    /// </summary>
    /// <param name="Dimensions">The dimensions of the displacement.</param>
    /// <param name="StartingPosition">The bottom left corner coordinates.</param>
    /// <param name="Elevation">Universal displacement added to vertex normal added to all points.</param>
    /// <param name="Normals">The direction of each vertex.</param>
    /// <param name="Distances">The distance each vertex is moved towards the normal.</param>
    /// <param name="Offsets">The position offset for each vertex.</param>
    /// <param name="OffsetNormals">The offset towards the direction of each vertex.</param>
    public record DisplacementInfo(int Dimensions, Vector StartingPosition, double Elevation, Grid<Vector> Normals, Grid<double> Distances, Grid<Vector> Offsets, Grid<Vector> OffsetNormals);
}

#pragma warning restore CS1591
#pragma warning restore CS1573
#pragma warning restore CS1572