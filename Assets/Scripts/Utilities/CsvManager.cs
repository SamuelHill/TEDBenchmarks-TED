using System;
using System.Numerics;
using TED.Utilities;
using Scripts.Time;
using Scripts.ValueTypes;
using UnityEngine;

namespace Scripts.Utilities {
    using static ColorUtility;
    using static CsvReader;
    using static CsvWriter;
    using static Randomize;
    using static SaveManager;
    using static StringProcessing;

    /// <summary>
    /// Handles declaring CsvReader parsers for all relevant ValueTypes and gives a function
    /// for passing in the name of a CSV file and mapping to that file in the Data folder.
    /// </summary>
    public static class CsvManager {
        private static readonly System.Random PersonalityRng;
        static CsvManager() => PersonalityRng = MakeRng();

        public static string CsvDataFile(string filename) => $"Assets/Data/{filename}.csv";

        public static void DeclareParsers() {
            DeclareParser(typeof(Person), ParsePerson);
            DeclareParser(typeof(ValueTuple<Person, Person>), ParsePersonTuple);
            DeclareParser(typeof(TimePoint), ParseTimePoint);
            DeclareParser(typeof(Vector2Int), ParseVector2Int);
            DeclareParser(typeof(Color), ParseColor);
        }

        private static Person ParsePerson(string personString) => (Person)DeserializeIfId(personString, Person.FromString, PersonalityRng);

        // TODO: Automatically generate SymmetricTuples and ValueTuples parse functions for any potential pairings OR
        // TODO: Generate these as they are used (parse the program for all column types and declare parsers as needed)
        private static object ParsePersonTuple(string personTupleString) {
            var temp = CommaSeparated(personTupleString, ParsePerson);
            return temp is { Length: 2 } ? new ValueTuple<Person, Person>(temp[0], temp[1]) :
                       throw new ArgumentException($"Couldn't convert string {personTupleString} to a ValueTuple<Person, Person>");
        }

        // TODO: Use the ISerializableValue interface that all of these implement...
        private static object ParseTimePoint(string dateString) => TimePoint.FromString(dateString);
        
        private static object ParseVector2Int(string vector2String) {
            var ints = CommaSeparatedInts(vector2String);
            return ints.Length == 2 ? new Vector2Int(ints[0], ints[1]) :
                       throw new ArgumentOutOfRangeException(
                           $"Expecting 2 comma separated ints for Vector2Int, {ints.Length} provided");
        }

        private static object ParseColor(string htmlColorString) =>
            TryParseHtmlString(htmlColorString, out var color) ? color : Color.white;

        public static void DeclareWriters() {
            DeclareWriter(typeof(Person), WritePerson);
            DeclareWriter(typeof(ValueTuple<Person, Person>), WritePersonTuple);
            DeclareWriter(typeof(Vector2Int), WriteVector2Int);
            DeclareWriter(typeof(Color), WriteColor);
        }

        private static string WritePerson(object personObject) => SerializedId(personObject);

        private static string WritePersonTuple(object personTupleObject) {
            var pair = (ValueTuple<Person, Person>)personTupleObject;
            return QuoteString($"{WritePerson(pair.Item1)}, {WritePerson(pair.Item2)}");
        }

        // These are not needed - the CsvWriter falls back on ToStrings
        private static string WriteTimePoint(object dateObject) => ((TimePoint)dateObject).ToString();
        
        private static string WriteVector2Int(object vector2Object) {
            var vec = (Vector2Int)vector2Object;
            return QuoteString($"{vec.x}, {vec.y}");
        } 

        private static string WriteColor(object colorObject) => $"#{ToHtmlStringRGB((Color)colorObject)}";
    }
}
