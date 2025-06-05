using Avalonia.Controls;
using gLibrary.Core.Engine;
using gLibrary.Core.Helping;
using gLibrary.Rendering.AvaloniaRenderers;
using MemTest.Game.Mapping;
using MemTest.Game;
namespace MemTest.Views
{
    public partial class MainWindow : Window
    {
        private MemTestLogic _game;
        private const int CellSize = 70;

        public MainWindow()
        {
            InitializeComponent();
            var engine = new GridEngine(3, 3);
            engine.GenerateGrid();
            var mapper = new MemMapper();
            var helper = new SquareHelper(engine);
            var avaloniaRenderer = new AvaloniaSquareRenderer(MemBackground, engine, mapper, helper, CellSize);
            _game = new MemTestLogic(engine, mapper, helper, avaloniaRenderer, CellSize);
            avaloniaRenderer.CellClicked += _game.HandleCellClick;
            _game.Initialize();
        }
    }
}
