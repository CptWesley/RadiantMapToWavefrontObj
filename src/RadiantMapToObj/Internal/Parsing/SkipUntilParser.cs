using System;
using Warpstone;

namespace RadiantMapToObj.Internal.Parsing
{
    /// <summary>
    /// Parser which parser a given regular expression pattern.
    /// </summary>
    /// <seealso cref="Parser{T}" />
    internal class SkipUntilParser : Parser<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkipUntilParser"/> class.
        /// </summary>
        /// <param name="str">The string to search for.</param>
        internal SkipUntilParser(string str)
        {
            String = str;
        }

        /// <summary>
        /// Gets the regular expression.
        /// </summary>
        internal string String { get; }

        /// <inheritdoc/>
        public override IParseResult<string> TryParse(string input, int position)
        {
            int index = input.IndexOf(String, position, StringComparison.Ordinal);

            if (index == -1)
            {
                return new ParseResult<string>(position, position, new UnexpectedTokenError(new string[] { $"'{String}'" }, GetFound(input, position)));
            }

            return new ParseResult<string>(string.Empty, index, index);
        }
    }
}
