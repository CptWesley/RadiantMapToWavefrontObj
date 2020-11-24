using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Added to allow for forwards compatibility with records in .net standard.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Needed for records.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit
    {
    }
}
