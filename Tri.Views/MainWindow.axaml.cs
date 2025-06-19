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
    private GridEngine _engine;

    public MainWindow()
    {
        InitializeComponent();

        const int size = 50;
        _engine = new GridEngine(5, 5);
        _engine.GenerateGrid();

        TriMapper mapper = new TriMapper();
        TriangleHelper helper = new TriangleHelper(_engine);
        _renderer = new AvaloniaTriangleRenderer(TriBackground, _engine, mapper, helper, size, OnClick);

        TriBackground.Width = _engine.Columns * size;
        TriBackground.Height = _engine.Rows * size;
        this.Width = TriBackground.Width + 80;
        this.Height = TriBackground.Height + 120;
        //shift
        TriBackground.Width = _engine.Columns * 0.5 * 50 + 15;
        _logic = new TriLogic(_engine, helper);
        _logic.InitializeGrid();

        TriangleRenderer triangleRenderer = new TriangleRenderer(_renderer, _engine, mapper, helper, size);
        triangleRenderer.RenderGrid();
    }

    private void OnClick(object? sender, CellClickEventArgs args)
    {
        int row = args.Cell.Row;
        int col = args.Cell.Column;

        // Pokud je buňka už modrá (1), klik ignoruj
        if (_engine.GetCellValue(row, col) == 1)
            return;

        var updated = _logic.HandleClick(row, col, out var changed);
        foreach (var (r, c) in updated)
            _renderer.UpdateCell(r, c);

        ScoreText.Text = $"{_logic.Score}";

        if (_logic.IsGameOver)
            EndGameMessage.IsVisible = true;
    }
}