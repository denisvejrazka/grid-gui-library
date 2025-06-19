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
    private readonly AvaloniaTriangleRenderer renderer;
    private readonly BaseRenderer hexRenderer;

    public MainWindow()
    {
        InitializeComponent();

        const int cellSize = 50;
        engine = new GridEngine(4, 4);
        engine.GenerateGrid();

        engine.SetCellValue(0, 0, 1);
        engine.SetCellValue(3, 3, 2);
        engine.SetCellValue(2, 2, 3);

        Mapper mapper = new Mapper();
        TriangleHelper helper = new TriangleHelper(engine);

        renderer = new AvaloniaTriangleRenderer(Testik, engine, mapper, helper, cellSize, OnCellClicked /*, OnCellHovered*/);
        hexRenderer = new TriangleRenderer(renderer, engine, mapper, helper, cellSize);
        hexRenderer.RenderGrid();
        renderer.Clear();
    }

    private void OnCellClicked(object? sender, CellClickEventArgs args)
    {
        if (args.Cell == null) return;

        engine.SetCellValue(args.Cell.Row, args.Cell.Column, 1);
        renderer.UpdateCell(args.Cell.Row, args.Cell.Column);
    }

//    private void OnCellHovered(object? sender, CellHoverEventArgs args)
//{
//    if (engine != null && renderer != null && args.Cell != null)
//        {
//            engine.SetCellValue(args.Cell.Row, args.Cell.Column, 2);
//            renderer.UpdateCell(args.Cell.Row, args.Cell.Column);
//        }
//}

}