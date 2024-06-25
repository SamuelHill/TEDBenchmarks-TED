using System;
using System.Collections.Generic;
using Scripts.Utilities;

namespace Scripts.ValueTypes {
    /// <summary>
    /// Members of Person that are specific to the C# implementation of the benchmark
    /// </summary>
    public partial class Person : IComparable<Person>, IEquatable<Person> {
    }
}