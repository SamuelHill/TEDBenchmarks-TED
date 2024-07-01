using System;
using TED;
using Scripts.Time;
using Scripts.ValueTypes;
using UnityEngine;

namespace Scripts.Simulator {
    /// <summary>Commonly used TED.Vars for types used in this simulator.</summary>
    /// <remarks>Variables will be lowercase for style/identification purposes.</remarks>
    public static class Variables {
        // ReSharper disable InconsistentNaming
        // NOTE: These are not used in the benchmark
        public static readonly Var<string> firstName = (Var<string>)"firstName";
        public static readonly Var<string> lastName = (Var<string>)"lastName";
        
        // These are
        public static readonly Var<Location> location = (Var<Location>)"location";
        public static readonly Var<Person> person = (Var<Person>)"person";
        public static readonly Var<Person> other = (Var<Person>)"other";
        public static readonly Var<float> affinity = (Var<float>)"affinity";
        public static readonly Var<float> otherAffinity = (Var<float>)"otherAffinity";
        public static readonly Var<Fingerprint> mood = (Var<Fingerprint>)"mood";
        public static readonly Var<Interactions.Outcome> outcome = (Var<Interactions.Outcome>)"outcome";

    }
}
