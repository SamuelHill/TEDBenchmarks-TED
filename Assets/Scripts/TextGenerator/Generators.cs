using System.Collections.Generic;
using System.Linq;
using TED;
using Scripts.Utilities;
using Scripts.ValueTypes;
using Vector2Int = UnityEngine.Vector2Int;

namespace Scripts.TextGenerator {
    using static IntExtensions;
    using static Randomize;

    // ReSharper disable InconsistentNaming
    public static class Generators {
        private static TextGenerator Choice(params Choice.Option[] options) => new Choice(options);
        private static TextGenerator OneOf(params OneOf.Choice[] choices) => new OneOf(choices);
        private static TextGenerator Sequence(params TextGenerator[] segments) => new Sequence(segments);
        
        // ADD Generators here
    }
}
