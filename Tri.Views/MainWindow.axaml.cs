using Avalonia.Controls;
using gLibrary.Core.Engine;
using gLibrary.Core.Helping;
using gLibrary.Core.Rendering;
using gLibrary.Rendering.Ava;
using Tri.Game.Mapping;
using Tri.Game;
using gLibrary.Core.Events;
namespace Tri.Views;
public partial class MainWindow : Window
    {
        private TriLogic _logic;
        private AvaloniaTriangleRenderer _renderer;

        public MainWindow()
        {
            InitializeComponent();

            const int size = 50;
            var engine = new GridEngine(5, 5);
            engine.GenerateGrid();

            var mapper = new TriMapper();
            var helper = new TriangleHelper(engine);
            _renderer = new AvaloniaTriangleRenderer(TriBackground, engine, mapper, helper, size, OnClick);

            TriBackground.Width = engine.Columns * size;
            TriBackground.Height = engine.Rows * size;
            this.Width = TriBackground.Width + 80;
            this.Height = TriBackground.Height + 120;

            _logic = new TriLogic(engine, helper);
            _logic.InitializeGrid();

            var triangleRenderer = new TriangleRenderer(_renderer, engine, mapper, helper, size);
            triangleRenderer.RenderGrid();
        }

        private void OnClick(object? sender, CellClickEventArgs args)
        {
            var updated = _logic.HandleClick(args.Cell.Row, args.Cell.Column, out var changed);
            foreach (var (row, col) in updated)
                _renderer.UpdateCell(row, col);

            ScoreText.Text = $"Skóre: {_logic.Score}";

            if (_logic.IsGameOver)
                EndGameMessage.IsVisible = true;
        }
    }