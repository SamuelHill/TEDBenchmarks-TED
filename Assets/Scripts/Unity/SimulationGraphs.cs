using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TED.Interpreter;
using TED;
using TED.Tables;
using TED.Utilities;
using Scripts.Simulator;
using Scripts.Unity.GraphVisualization;
using Scripts.Utilities;
using Scripts.ValueTypes;
using Random = System.Random;

namespace Scripts.Unity {
    using static Randomize;
    using static GraphViz<object>;
    using static GraphVisualizer;
    using static GUIManager;
    using static Variables;
    using static Benchmark;

    public static class SimulationGraphs {
        //private static readonly Random Rng = MakeRng();

        // ********************************** Visualize Functions *********************************
        
        private static GraphViz<TGraph> TraceToDepth<TGraph, T>(int maxDepth, T start, 
            Func<T, IEnumerable<(T neighbor, string label, string color)>> edges) where T : TGraph {
            var g = new GraphViz<TGraph>();

            void Walk(T node, int depth) {
                if (depth > maxDepth || g.Nodes.Contains(node)) return;
                g.AddNode(node);
                foreach (var edge in edges(node)) {
                    Walk(edge.neighbor, depth + 1);
                    g.AddEdge(new GraphViz<TGraph>.Edge(node, edge.neighbor, true, edge.label,
                                                        new Dictionary<string, object> { { "color", edge.color } }));
                }
            }

            Walk(start, 0);
            return g;
        }

        // ************************************* Descriptions *************************************

        public static void SetDescriptionMethods() => 
            Graph.SetDescriptionMethod<TablePredicate>(TableDescription);

        private static string TableDescription(TablePredicate p) {
            var b = new StringBuilder();
            b.Append("<b>");
            b.AppendLine(p.DefaultGoal.ToString().Replace("[", "</b>["));
            b.AppendFormat("{0} rows\n", p.Length);
            b.Append("<size=16>");
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (p.UpdateMode) {
                case UpdateMode.BaseTable:
                    b.Append("Base table");
                    break;
                case UpdateMode.Operator:
                    b.Append("Operator result");
                    break;
                default:
                    if (p.Rules != null)
                        foreach (var r in p.Rules) b.AppendLine(r.ToString());
                    break;
            }
            return b.ToString();
        }
    }
}
