using System;
using System.Collections.Generic;

namespace Scripts.ValueTypes {
    /// <summary>
    /// Fields of Location specific to the C# implementation of the benchmark
    /// </summary>
    public partial class Location : IComparable<Location>, IEquatable<Location> {
        public readonly HashSet<Person> Occupants = new HashSet<Person>();

        public static readonly List<Location> Everywhere = new List<Location>();

        public static Location Random() => Scripts.Utilities.Randomize.RandomElement(Everywhere, Scripts.Utilities.Randomize.RngForInitialization);

        /// <summary>
        /// Make all the Location objects.
        /// </summary>
        /// <param name="count">How many to make</param>
        public static void CreateAll(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Everywhere.Add(new Location(i.ToString()));
            }
        }
    }
}