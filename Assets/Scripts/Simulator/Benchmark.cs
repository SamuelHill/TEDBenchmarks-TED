using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TED;
using TED.Interpreter;
using TED.Tables;
using TED.Utilities;
using Scripts.TextGenerator;
using Scripts.Time;
using Scripts.Unity;
using Scripts.Utilities;
using Scripts.ValueTypes;
using TMPro;
using UnityEngine;
using static TED.Language;

namespace Scripts.Simulator {
    // "Utils" (Utilities and Time)
    using static Clock;          // All current time functions
    using static CsvManager;     // DeclareParsers
    using static SaveManager;    // Save and Load simulation
    using static File;           // Performance CSV output
    using static Randomize;      // Seed and RandomElement
    // GUI/graphics
    using static Color;
    using static GUIManager;       // Colorizers and Pop tables
    using static SimulationGraphs; // Visualize___ functions
    // The following offload static components of the TED code...
    using static StaticTables; // non dynamic tables - classic datalog EDB
    using static Variables;
    using static Wrappers;

    public static class Benchmark {
        private const int Seed = 349571286;
        public static Simulation Simulation = null!;
        public static bool RecordingPerformance;
        private static TablePredicate<Person, Location> _whereTheyAre;
        private static TablePredicate<Person, Person, float, float, Interactions.Outcome> _interactedWith;
        private static TablePredicate<Person, Person, float> _affinity;
        private static readonly List<(uint, double)> PerformanceData = new();

        static Benchmark() {
            DeclareParsers(); // Parsers used in the FromCsv calls in InitStaticTables
            DeclareWriters();
            Seed(Seed);
            SetDefaultColorizers();
            SetDescriptionMethods();
            if (RecordingPerformance) {
                using var file = CreateText("PerformanceData.csv");
            }
            ReadSaveData = reader => ClockTick = uint.Parse(reader.ReadToEnd());
            WriteSaveData = writer => writer.Write(ClockTick.ToString());
        }

        // ReSharper disable InconsistentNaming
        // Tables, despite being local or private variables, will be capitalized for style/identification purposes.

        public static void InitSimulator() {
            Simulation = new Simulation("Benchmark");
            Simulation.BeginPredicates();
            var Person = Predicate("Person", person);
            var Location = Predicate("Location", location);
            var WorkLocation = Predicate("WorkLocation", person.Key, location);

            _affinity = Predicate("Affinity", person, other, affinity).JointKey(person, other);
            _affinity.Overwrite = true;
            var Affinity = Definition("Affinity", person, other, affinity).Is(FirstOf[_affinity, PersonPersonAffinity[person, other, affinity]]);

            _whereTheyAre = Predicate("WhereTheyAre", person, location.Indexed)
               .If(IsDaytime[true], Person, RandomMood[mood],
                   Maximal(location, affinity, Location & PersonLocationAffinity[person, mood, location, affinity]))
               .If(IsDaytime[false], WorkLocation);
            _interactedWith = Predicate("InteractedWith", person, other, affinity, otherAffinity, outcome)
               .If(_whereTheyAre, Maximal(other, affinity, _whereTheyAre[other, location] & Affinity[person, other, affinity]),
                   Affinity[other, person, otherAffinity], Interact[person, other, affinity, otherAffinity, outcome]);
            
            _affinity.Add[person, other, affinity + ActorOutcome[outcome]].If(_interactedWith);
            _affinity.Add[person, other, affinity + OtherOutcome[outcome]].If(_interactedWith[other, person, __, affinity, outcome]);
            Simulation.EndPredicates();

            Simulation.Compile("Scripts.Simulator",
                additionalDeclarations: new [] { "using Scripts.ValueTypes;", "using static Scripts.ValueTypes.Interactions;", "using static Scripts.Simulator.Wrappers;" });
            TED.Compiler.Compiler.Link(Simulation, true);
            
            Person.AddRows(Enumerable.Range(0, 2000).Select(s => new Person("Bob", $"Mc{s}", RngForInitialization)));
            Location.AddRows(Enumerable.Range(0, 100).Select(s => new Location($"{s}")));
            var locationList = Location.ToArray();
            WorkLocation.AddRows(Person.Select(p => (p, locationList.RandomElement(RngForInitialization))));
        }

        public static readonly Stopwatch Stopwatch = new Stopwatch();
        public static double UpdateTime;
        
        public static void UpdateSimulator() {
            Tick();
            Stopwatch.Reset();
            Stopwatch.Start();
            Simulation.Update();
            // Simulation.UpdateAsync().Wait();
            Stopwatch.Stop();
            UpdateTime = (15*UpdateTime+Stopwatch.Elapsed.TotalMilliseconds)/16;

            if (!RecordingPerformance) return;
            PerformanceData.Add((ClockTick - InitialClockTick, Stopwatch.Elapsed.TotalMilliseconds));
            if (ClockTick % 100 == 1) {
                using var file = AppendText("PerformanceData.csv");
                foreach ((var clock, var execution) in PerformanceData)
                    file.WriteLine($"{clock}, {execution}");
                PerformanceData.Clear();
            }
        }
    }
}
