using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Layout;
using Avalonia.Platform;
using gLibrary.Core.Engine;
using gLibrary.Core.Rendering;
using gLibrary.Core.Engine.Models;
using gLibrary.Core.Helping;
using gLibrary.Core.Mapping;
using gLibrary.Core.Events;


namespace gLibrary.Rendering.Ava;

// add UpdateCell(int row, int col)
public class AvaloniaTriangleRenderer : BaseAvaloniaRenderer
{
    public AvaloniaTriangleRenderer(Canvas canvas, GridEngine engine, IMap mapper, TriangleHelper helper, int cellSize,
            EventHandler<CellClickEventArgs>? onClick = null,
            EventHandler<CellHoverEventArgs>? onHover = null)
            : base(canvas, engine, mapper, helper, cellSize, onClick, onHover) { }

    public override void RenderCell(int row, int col, Cell cell, int cellSize, (double x, double y) position)
    {
        var points = new List<Point>();
        double height = Math.Sqrt(3) / 2 * cellSize;
        bool isUpward = (row + col) % 2 == 0;

        if (isUpward)
        {
            points.Add(new Point(0, height));
            points.Add(new Point(cellSize / 2, 0));
            points.Add(new Point(cellSize, height));
        }
        else
        {
            points.Add(new Point(0, 0));
            points.Add(new Point(cellSize, 0));
            points.Add(new Point(cellSize / 2, height));
        }

        var triangle = new Polygon
        {
            Points = points,
            Fill = new SolidColorBrush(Color.Parse(cell.Fill)),
            Stroke = Brushes.Black,
            StrokeThickness = 0.3
        };

        var panel = new Panel { Width = cellSize, Height = cellSize };
        panel.Children.Add(triangle);

        if (!string.IsNullOrEmpty(cell.Raster))
        {
            var uri = new Uri(cell.Raster);
            var bitmapImage = new Image
            {
                Source = new Bitmap(AssetLoader.Open(uri)),
                Width = cellSize,
                Height = cellSize,
            };
            panel.Children.Add(bitmapImage);
        }

        var textBlock = new TextBlock
        {
            Text = cell.Text,
            Foreground = Brushes.Black,
            FontSize = cellSize * 0.4,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        panel.Children.Add(textBlock);

        Canvas.SetLeft(panel, position.x);
        Canvas.SetTop(panel, position.y);
        _canvas.Children.Add(panel);

        // Uložení panelu pro možnost pozdější aktualizace
        _cellVisuals[(row, col)] = panel;
    }
}