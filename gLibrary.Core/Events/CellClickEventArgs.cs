using gLibrary.Core.Engine.Models;

namespace gLibrary.Core.Events
{
    public enum MouseButtonType
    {
        Left,
        Right,
        Middle,
        Other
    }

    public class CellClickEventArgs : EventArgs
    {
        public Cell Cell { get; }
        public MouseButtonType Button { get; }

        public CellClickEventArgs(Cell cell, MouseButtonType button)
        {
            Cell = cell;
            Button = button;
        }
    }
}
