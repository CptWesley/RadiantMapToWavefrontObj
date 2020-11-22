﻿using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Warpstone;
using static Warpstone.Parsers.BasicParsers;

namespace RadiantMapToObj.Internal.Parsing
{
    /// <summary>
    /// Provides parsers for common things.
    /// </summary>
    [SuppressMessage("Ordering Rules", "SA1202", Justification = "Order is important for instantiation.")]
    internal static class CommonParsingHelper
    {
        private static readonly IParser<string> Whitespace = Regex(@"\s+");
        private static readonly IParser<string> Comment = Regex(@"//.*");

        /// <summary>
        /// Parses mandatory layout.
        /// </summary>
        internal static readonly IParser<string> Layout = OneOrMore(Or(Comment, Whitespace)).Transform(x => string.Join(string.Empty, x)).WithName("layout");

        /// <summary>
        /// Parses optional layout.
        /// </summary>
        internal static readonly IParser<string> OptionalLayout = Or(Layout, Create(string.Empty));

        /// <summary>
        /// Parses texture names.
        /// </summary>
        internal static readonly IParser<string> Texture = Regex(@"[\w\/]+");

        /// <summary>
        /// Parses a single double.
        /// </summary>
        internal static readonly IParser<double> Double = Regex(@"-?((\.[0-9]+)|(([1-9][0-9]*|0)(\.[0-9]+)?))").WithName("double").Transform(x => double.Parse(x, CultureInfo.InvariantCulture));

        /// <summary>
        /// Parses a vector.
        /// </summary>
        internal static readonly IParser<Vector> Vertex
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(Double)
            .ThenSkip(Layout)
            .ThenAdd(Double)
            .ThenSkip(Layout)
            .ThenAdd(Double)
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"))
            .Transform((x, y, z) => new Vector(x, y, z));
    }
}