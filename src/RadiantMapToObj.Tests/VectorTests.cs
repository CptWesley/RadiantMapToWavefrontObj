using System;
using Xunit;

using static AssertNet.Assertions;

namespace RadiantMapToObj.Tests
{
    /// <summary>
    /// Test class for the <see cref="Vector"/> class.
    /// </summary>
    public static class VectorTests
    {
        /// <summary>
        /// Checks that the constructor functions correctly.
        /// </summary>
        [Fact]
        public static void ConstructorTest()
        {
            Vector v = new Vector(1, 2, 3);
            AssertThat(v.X).IsEqualTo(1);
            AssertThat(v.Y).IsEqualTo(2);
            AssertThat(v.Z).IsEqualTo(3);
        }

        /// <summary>
        /// Checks that the length functions correctly.
        /// </summary>
        [Fact]
        public static void LengthTest()
        {
            Vector v = new Vector(1, 2, 3);
            AssertThat(v.SquareLength).IsEqualTo(14);
            AssertThat(v.Length).IsEqualTo(Math.Sqrt(14));
        }

        /// <summary>
        /// Checks that the length functions correctly.
        /// </summary>
        [Fact]
        public static void UnitTest()
        {
            Vector v = new Vector(1, 2, 3);
            Vector u = v.Unit;
            AssertThat(u.Length).IsEqualTo(1);
            AssertThat(u.DirectionEquals(v)).IsTrue();
        }

        /// <summary>
        /// Checks that the direction functions correctly.
        /// </summary>
        [Fact]
        public static void DirectionTest()
        {
            Vector v1 = new Vector(1, 0, 0);
            Vector v2 = new Vector(2, 0, 0);
            Vector v3 = new Vector(0, 1, 0);
            AssertThat(v1.DirectionEquals(v2)).IsTrue();
            AssertThat(v1.DirectionEquals(v3)).IsFalse();
        }

        /// <summary>
        /// Checks that the addition functions correctly.
        /// </summary>
        [Fact]
        public static void AddTest()
            => AssertThat(new Vector(10, 20, 30) + new Vector(1, 2, 3)).IsEqualTo(new Vector(11, 22, 33));

        /// <summary>
        /// Checks that the subtraction functions correctly.
        /// </summary>
        [Fact]
        public static void SubtractTest()
            => AssertThat(new Vector(10, 20, 30) - new Vector(1, 2, 3)).IsEqualTo(new Vector(9, 18, 27));
    }
}
