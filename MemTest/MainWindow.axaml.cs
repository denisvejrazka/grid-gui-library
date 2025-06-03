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

            // 1) Vytvo��me �Core� komponenty:
            var engine = new GridEngine(rows: 3, columns: 3);
            engine.GenerateGrid();

            var mapper = new MemMapper();
            var helper = new SquareHelper(engine);

            // 2) Vytvo��me Avalonia renderer pro �tvercovou m��ku:
            var avaloniaRenderer = new AvaloniaSquareRenderer(
                canvas: MemBackground,
                engine: engine,
                mapper: mapper,
                helper: helper,
                cellSize: CellSize);

            // 3) Vytvo��me instanci hry a p�ed�me mu z�vislosti v�etn� rendereru:
            _game = new MemTestGame(engine, mapper, helper, avaloniaRenderer, CellSize);

            // 4) Propoj�me kliknut� s hern� logikou:
            avaloniaRenderer.CellClicked += _game.HandleCellClick;

            // 5) Spust�me inicializaci (vykreslen� a za��tek hry):
            _game.Initialize();
        }
    }
}