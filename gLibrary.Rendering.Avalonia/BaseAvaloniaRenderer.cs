using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Rendering;
using gLibrary.Core.Engine;
using gLibrary.Core.Engine.Models;
using gLibrary.Core.Events;
using gLibrary.Core.Helping;
using gLibrary.Core.Mapping;
using gLibrary.Core.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gLibrary.Rendering.Ava
{
    public abstract class BaseAvaloniaRenderer: Control, Core.Rendering.IRenderer
    {
        protected readonly Canvas _canvas;
        protected readonly GridEngine _engine;
        protected readonly IMap _mapper;
        protected readonly BaseHelper _helper;
        protected readonly int _cellSize;
        protected readonly Dictionary<(int row, int col), Panel> _cellVisuals = new();

        //events
        public event EventHandler<CellClickEventArgs>? CellClicked;
        public event EventHandler<CellHoverEventArgs>? CellHovered;

        public BaseAvaloniaRenderer(Canvas canvas, GridEngine engine, IMap mapper, BaseHelper helper, int cellSize, EventHandler<CellClickEventArgs>? OnClick = null, EventHandler<CellHoverEventArgs>? OnHover = null)
        {
            _canvas = canvas;
            //events
            _canvas.PointerPressed += OnPointerPressed;
            //_canvas.PointerEntered += OnPointerMoved;
            CellClicked = OnClick;
            CellHovered = OnHover;
            _engine = engine;
            _mapper = mapper;
            _helper = helper;
            _cellSize = cellSize;
        }

        public void Clear() => _canvas.Children.Clear();

        public abstract void RenderCell(int row, int col, Cell cell, int cellSize, (double x, double y) position);

        public void UpdateCell(int row, int col)
        {
            int value = _engine.GetCellValue(row, col);

            Cell cell = _mapper.GetMap(value, row, col);

            var position = _helper.GetPosition(row, col, _cellSize);

            if (_cellVisuals.TryGetValue((row, col), out var oldPanel))
            {
                _canvas.Children.Remove(oldPanel);
                _cellVisuals.Remove((row, col));
            }

            RenderCell(row, col, cell, _cellSize, position);
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var point = e.GetPosition(_canvas);
            var cellCoords = _helper.GetCellCoordinatesFromPixel(point.X, point.Y, _cellSize);
            Cell cell = _mapper.GetMap(
                _engine.GetCellValue(cellCoords.Value.row, cellCoords.Value.col),
                cellCoords.Value.row,
                cellCoords.Value.col
            );

            if (cell != null)
            {
                var properties = e.GetCurrentPoint(_canvas).Properties;
                MouseButtonType button = properties.IsLeftButtonPressed
                                         ? MouseButtonType.Left
                                         : properties.IsRightButtonPressed
                                            ? MouseButtonType.Right
                                            : MouseButtonType.Other;

                CellClicked?.Invoke(this, new CellClickEventArgs(cell, button));
            }
        }

        //private void OnPointerMoved(object? sender, PointerEventArgs e)
        //{
        //    var point = e.GetPosition(_canvas);
        //    var cellCoords = _squareHelper.GetCellCoordinatesFromPixel(point.X, point.Y, _cellSize);
        //    Cell cell = _mapper.GetMap(_engine.GetCellValue(cellCoords.Value.row, cellCoords.Value.col), cellCoords.Value.row, cellCoords.Value.col);

        //    if (cell != null)
        //    {
        //        CellHovered?.Invoke(this, new CellHoverEventArgs(cell, e));
        //    }
        //}
    }
}
