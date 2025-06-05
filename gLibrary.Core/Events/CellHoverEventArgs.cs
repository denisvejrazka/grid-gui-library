using gLibrary.Core.Engine.Models;

namespace gLibrary.Core.Events
{
    public class CellHoverEventArgs : EventArgs
    {
        public Cell Cell { get; }

        public CellHoverEventArgs(Cell cell)
        {
            Cell = cell;
        }
    }
}
