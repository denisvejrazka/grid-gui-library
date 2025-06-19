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
public class AvaloniaSquareRenderer : BaseAvaloniaRenderer
{
    public AvaloniaSquareRenderer(Canvas canvas, GridEngine engine, IMap mapper, SquareHelper helper, int cellSize,
            EventHandler<CellClickEventArgs>? onClick = null,
            EventHandler<CellHoverEventArgs>? onHover = null)
            : base(canvas, engine, mapper, helper, cellSize, onClick, onHover) { }


    public override void RenderCell(int row, int col, Cell cell, int cellSize, (double x, double y) position)
    {
        var rect = new Rectangle
        {
            Width = cellSize,
            Height = cellSize,
            Fill = new SolidColorBrush(Color.Parse(cell.Fill)),
            Stroke = Brushes.Black,
            StrokeThickness = 0.5,
        };

        var panel = new Panel { Width = cellSize, Height = cellSize };
        panel.Children.Add(rect);

        if (!string.IsNullOrEmpty(cell.Raster))
        {
            var uri = new Uri(cell.Raster);
            double imageScale = 0.6; // např. 60 % velikosti buňky
            double imageSize = cellSize * imageScale;

            var bitmapImage = new Image
            {
                Source = new Bitmap(AssetLoader.Open(uri)),
                Width = imageSize,
                Height = imageSize,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.Uniform,
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