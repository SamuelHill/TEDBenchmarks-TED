using System;
using System.Linq;
using Scripts.TextGenerator;
using TED;
using TED.Interpreter;
using Scripts.Time;
using Scripts.Utilities;
using Scripts.ValueTypes;
using UnityEngine;
using static TED.Language;

namespace Scripts.Simulator {
    using static CsvManager;
    using static Generators;
    using static LocationType;
    using static InteractionType;
    using static SaveManager;
    using static Variables;

    /// <summary>
    /// All static tables (Datalog style EDBs - in TED these can be extensional or intensional).
    /// </summary>
    public static class StaticTables {
        public static TablePredicate<string> FirstNames;
        private static TablePredicate<string> _femaleNames;
        private static TablePredicate<string> _maleNames;
        public static TablePredicate<string> Surnames;

        public static void InitStaticTables() {
            FirstNames = Predicate("FirstNames", firstName);
            _femaleNames = FromCsv("FemaleNames", CsvDataFile("female_names"), firstName);
            _maleNames = FromCsv("MaleNames", CsvDataFile("male_names"), firstName);
            FirstNames.Initially.Where(_femaleNames[firstName] | _maleNames[firstName]);
            Surnames = FromCsv("Surnames", CsvDataFile("english_surnames"), lastName);
        }

        private static TablePredicate<T> EnumTable<T>(string name, IColumnSpec<T> column) where T : Enum {
            var table = Predicate(name, column);
            table.AddRows(Enum.GetValues(typeof(T)).Cast<T>());
            return table;
        }
    }
}
