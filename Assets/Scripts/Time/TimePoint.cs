using System;
using Scripts.Utilities;

namespace Scripts.Time {
    using static Enum;

    /// <summary>
    /// Record of a point in time at the finest possible temporal resolution.
    /// </summary>
    public readonly struct TimePoint : IComparable<TimePoint>, IEquatable<TimePoint>, ISerializableValue<TimePoint> {
        /// <summary>Tick value within the simulation</summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly uint Clock;

        /// <param name="clock">Tick value</param>
        public TimePoint(uint clock) => Clock = clock;

        // ReSharper disable once InconsistentNaming
        public static readonly TimePoint Eschaton = new(uint.MaxValue);
        private bool IsEschaton => Clock == uint.MaxValue;

        // *************************** Compare and Equality interfacing ***************************

        public int CompareTo(TimePoint other) => Clock.CompareTo(other.Clock);
        public bool Equals(TimePoint other) => Clock == other.Clock;
        public override bool Equals(object obj) => obj is TimePoint other && Equals(other);
        public override int GetHashCode() => Clock.GetHashCode();

        public static bool operator ==(TimePoint t1, TimePoint t2) => t1.Equals(t2);
        public static bool operator !=(TimePoint t1, TimePoint t2) => !(t1 == t2);

        // ****************************************************************************************

        public override string ToString() => IsEschaton ? "Has not occurred" : Clock.ToString();
        
        public static TimePoint FromString(string timePointString) => timePointString == "Has not occurred" ? Eschaton : 
                                                                          new TimePoint(uint.Parse(timePointString));
    }
}
