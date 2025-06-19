using Avalonia.Controls;
using gLibrary.Core.Engine;
using gLibrary.Core.Helping;
using gLibrary.Rendering.Ava;
using MemTest.Game.Mapping;
using MemTest.Game;
using gLibrary.Core.Mapping;
namespace MemTest.Views
{
    public partial class MainWindow : Window
    {
        private MemTestLogic _game;
        private const int CellSize = 70;

        public MainWindow()
        {
            InitializeComponent();
            GridEngine engine = new GridEngine(3, 3);
            engine.GenerateGrid();
            IMap mapper = new MemMapper();
            HexagonHelper helper = new HexagonHelper(engine);
            AvaloniaHexagonRenderer avaloniaRenderer = new AvaloniaHexagonRenderer(MemBackground, engine, mapper, helper, CellSize);
            _game = new MemTestLogic(engine, mapper, helper, avaloniaRenderer, CellSize);
            avaloniaRenderer.CellClicked += _game.HandleCellClick;
            _game.Initialize();
        }
    }
}
