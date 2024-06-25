using System;
using System.Linq;
using TED.Utilities;
using Scripts.Simulator;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.Unity {
    using static GraphVisualizer;
    using static GUI;
    using static GUIManager;
    using static Input;
    using static SaveManager;
    using static Benchmark;

    // ReSharper disable once UnusedMember.Global
    /// <summary>Handles running the simulation and interfacing with the GUI and Tile Managers.</summary>
    public class SimulationController : MonoBehaviour {
        // ReSharper disable MemberCanBePrivate.Global, FieldCanBeMadeReadOnly.Global, ConvertToConstant.Global, UnassignedField.Global
        public bool RecordPerformanceData;
        public bool AlphabeticalTables = true;
        public bool PrettyNamesOnly = true;
        public Vector2Int TownCenter;
        public Tilemap Tilemap;
        public Tile OccupiedLot;
        public Component GraphComponent;
        // ReSharper restore UnassignedField.Global, ConvertToConstant.Global, FieldCanBeMadeReadOnly.Global, MemberCanBePrivate.Global

        private TileManager _tileManager;
        private GraphVisualizer _graphVisualizer;
        private Vector2 _graphCenterPoint;
        private Vector2 _graphMaxDimensions;
        private bool _simulationRunning = true; // make public for unity inspector control of start
        private bool _simulationSingleStep;
        private bool _profileRuleExecutionTime;
        private bool _guiRunOnce;

        // ReSharper disable once UnusedMember.Global
        internal void Start() {
            TED.Comparer<Vector2Int>.Default = new GridComparer();
            RecordingPerformance = RecordPerformanceData;
            AlphabetizeTables = AlphabeticalTables;
            _tileManager = new TileManager(Tilemap, TownCenter, OccupiedLot);
            _graphVisualizer = GraphComponent.GetComponent<GraphVisualizer>();
            GraphScreenCoordinates();
            GraphBoundRect = REPLContainer;
            InitSimulator();
            AvailableTables((PrettyNamesOnly ? 
                                 Simulation.Tables.Where(t => !t.Name.Contains("_")) :
                                 Simulation.Tables).Append(Simulation.Exceptions).Append(Simulation.Problems));
            ActiveTables(new[] { "WhereTheyAre", "InteractedWith", "Affinity", "" });
            LoadFromSave(Simulation);
        }

        // ReSharper disable once UnusedMember.Global
        internal void Update() {
            if (GetKeyDown(KeyCode.Escape)) {
                _simulationRunning = !_simulationRunning;
                SavingWithName = false;
            }
            if (!_simulationRunning && GetKeyDown(KeyCode.Space)) _simulationSingleStep = true;
            if (GetKeyDown(KeyCode.BackQuote)) _profileRuleExecutionTime = !_profileRuleExecutionTime;
            if (GetKeyDown(KeyCode.F1)) ToggleShowTables();
            if (GetKeyDown(KeyCode.F2)) {
                ToggleREPLTable();
                _simulationRunning = !ShowREPLTable;
            }
            if (GetKeyDown(KeyCode.F4)) Save(Simulation);
            if (GetKeyDown(KeyCode.F5)) {
                _simulationRunning = false;
                SavingWithName = true;
            }
            if (_simulationRunning || _simulationSingleStep) {
                try { UpdateSimulator(); } catch (Exception e) {
                    Debug.LogException(e);
                    _simulationRunning = false;
                    throw;
                }
                _simulationSingleStep = false;
                if (PoppedTable & _simulationRunning) {
                    _simulationRunning = false;
                    PoppedTable = false;
                }
            }
            if (!ShowREPLTable) _tileManager.UpdateSelectedTile();
        }

        // ReSharper disable once UnusedMember.Global
        internal void OnGUI() {
            if (!_guiRunOnce) {
                CustomSkins();
                InitAllTables();
                _guiRunOnce = true;
            }
            ShowStrings();
            ShowActiveTables();
            ChangeActiveTables();
            ShowFlowButtons();
            ShowREPL();
            SaveNameText();
            _tileManager.SetVisibility(ShowTilemap);
            if (_profileRuleExecutionTime) RuleExecutionTimes();
            if (!_simulationRunning && !ChangeTable && !ShowREPLTable && !SavingWithName) ShowPaused();
        }

        // ************************************ Button Layouts ************************************

        private static void ShowFlowButtons() {
            if (!GraphVisible) return;
            if (Button(DataFlowButton(true), "Show dataflow"))
                ShowGraph(DataflowVisualizer.MakeGraph(Simulation));
            if (Button(DataFlowButton(false), "Update graph"))
                ShowGraph(UpdateFlowVisualizer.MakeGraph(Simulation));
        }

        // ************************************* REPL Layout **************************************
        // ReSharper disable InconsistentNaming

        private void GraphScreenCoordinates() {
            var screenCenter = new Vector2(Screen.width, Screen.height) / 2;
            var right = screenCenter.x + _graphVisualizer.RightBorder;
            var left = screenCenter.x + _graphVisualizer.LeftBorder;
            var width = right - left;
            var top = screenCenter.y - _graphVisualizer.TopBorder;
            var bottom = screenCenter.y - _graphVisualizer.BottomBorder;
            var height = bottom - top;
            _graphMaxDimensions = new Vector2(width, height);
            var center = _graphMaxDimensions / 2;
            _graphCenterPoint = new Vector2(left + center.x, top + center.y);
        }

        private Vector2 ClampREPLDimensions(int width, int height) =>
            new(width > _graphMaxDimensions.x ? _graphMaxDimensions.x : width,
                height > _graphMaxDimensions.y ? _graphMaxDimensions.y : height);
        private Rect REPLContainer(int width, int height, int offset) {
            var dimensions = ClampREPLDimensions(width, height);
            var centerOffsets = dimensions / 2;
            var center = _graphCenterPoint - centerOffsets;
            return new Rect(center.x, center.y - offset, dimensions.x, dimensions.y);
        }
    }
}
