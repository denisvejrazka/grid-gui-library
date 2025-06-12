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
    private readonly AvaloniaSquareRenderer renderer;
    private readonly BaseRenderer hexRenderer;

    public MainWindow()
    {
        InitializeComponent();

        const int cellSize = 50;
        engine = new GridEngine(4, 4);
        engine.GenerateGrid();

        // Nastavení hodnot podle matice A
        int[,] values = new int[,]
        {
    { 4, 1, 2, 3 },
    { 1, 3, 1, 2 },
    { 0, 0, 2, 1 },
    { 0, 2, 0, 0 }
        };

        for (int i = 0; i < values.GetLength(0); i++)
        {
            for (int j = 0; j < values.GetLength(1); j++)
            {
                engine.SetCellValue(i, j, values[i, j]);
            }
        }

        Mapper mapper = new Mapper();
        SquareHelper helper = new SquareHelper(engine);

        renderer = new AvaloniaSquareRenderer(Testik, engine, mapper, helper, cellSize, OnCellClicked /*, OnCellHovered*/);
        hexRenderer = new SquareRenderer(renderer, engine, mapper, helper, cellSize);

        hexRenderer.RenderGrid();

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