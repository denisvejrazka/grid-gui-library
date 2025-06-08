using Avalonia.Controls;
using gLibrary.Core.Engine;
using gLibrary.Core.Engine.Models;
using gLibrary.Core.Events;
using gLibrary.Core.Helping;
using gLibrary.Core.Mapping;
using gLibrary.Core.Rendering;
using gLibrary.Rendering.Ava;

namespace Test;

public partial class MainWindow : Window
{
    private readonly GridEngine engine;
    private readonly AvaloniaHexagonRenderer renderer;
    private readonly BaseRenderer hexRenderer;

    public MainWindow()
    {
        InitializeComponent();

        const int cellSize = 50;
        engine = new GridEngine(5, 5); // správně inicializuj třídní pole
        engine.GenerateGrid();

        Mapper mapper = new Mapper();
        HexagonHelper helper = new HexagonHelper(engine);

        renderer = new AvaloniaHexagonRenderer(Testik, engine, mapper, helper, cellSize, OnCellClicked, OnCellHovered);
        hexRenderer = new HexagonRenderer(renderer, engine, mapper, helper, cellSize);

        hexRenderer.RenderGrid();
    }

    private void OnCellClicked(object? sender, CellClickEventArgs args)
    {
        if (engine != null && renderer != null && args.Cell != null)
        {
            engine.SetCellValue(args.Cell.Row, args.Cell.Column, 1);
            renderer.UpdateCell(args.Cell.Row, args.Cell.Column);
        }
    }

    private void OnCellHovered(object? sender, CellHoverEventArgs args)
    {
        Cell cell = args.Cell;
        cell.Fill = "#DDDDDD"; // Jemné zvýraznění
        renderer.UpdateCell(cell.Row, cell.Column);
    }
}