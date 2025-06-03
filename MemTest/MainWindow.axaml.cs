using Avalonia.Controls;
using gLibrary.Engine;
using gLibrary.Helping;
using gLibrary.Rendering.AvaloniaRenderers;
using MemTest.Game.Mapping;
using MemTest;

namespace MemTest.Views
{
    public partial class MainWindow : Window
    {
        private MemTest _game;
        private const int CellSize = 70;

        public MainWindow()
        {
            InitializeComponent();

            // 1) Vytvoøíme „Core“ komponenty:
            var engine = new GridEngine(rows: 3, columns: 3);
            engine.GenerateGrid();

            var mapper = new MemMapper();
            var helper = new SquareHelper(engine);

            // 2) Vytvoøíme Avalonia renderer pro ètvercovou møížku:
            var avaloniaRenderer = new AvaloniaSquareRenderer(
                canvas: MemBackground,
                engine: engine,
                mapper: mapper,
                helper: helper,
                cellSize: CellSize);

            // 3) Vytvoøíme instanci hry a pøedáme mu závislosti vèetnì rendereru:
            _game = new MemTestGame(engine, mapper, helper, avaloniaRenderer, CellSize);

            // 4) Propojíme kliknutí s herní logikou:
            avaloniaRenderer.CellClicked += _game.HandleCellClick;

            // 5) Spustíme inicializaci (vykreslení a zaèátek hry):
            _game.Initialize();
        }
    }
}