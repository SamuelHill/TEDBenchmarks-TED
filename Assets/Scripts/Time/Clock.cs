﻿using System;
using TED;
using TED.Primitives;
using Scripts.Utilities;

namespace Scripts.Time {
    /// <summary>
    /// Internal clock, keeps time during a simulation by ticking along with Simulator.Update
    /// </summary>
    public static class Clock {
        private const ushort NumTicks = 672;
        private const ushort PrimordialOffset = 100; // years before start that TimePoints can exist at
        internal const uint InitialClockTick = NumTicks * PrimordialOffset;

        public static uint ClockTick;
        public static void Tick() => ClockTick++;

        static Clock() => OffsetInitialClockTick();
        private static void OffsetInitialClockTick(uint offset = 0) => ClockTick = InitialClockTick + offset;

        public static TimePoint TimePoint() => new(ClockTick);
        public static readonly Function<TimePoint> CurrentTimePoint = new($"CurrentTimePoint", TimePoint, false);
    }
}
