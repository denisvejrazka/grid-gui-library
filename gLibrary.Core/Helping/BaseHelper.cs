using System.Collections.Generic;
using gLibrary.Core.Engine;

namespace gLibrary.Core.Helping
{
    public abstract class BaseHelper
    {
        protected GridEngine Engine;
        protected BaseHelper(GridEngine engine){Engine = engine;}
        public abstract (double x, double y) GetPosition(int row, int col, int cellSize);
        public abstract (int row, int col)? GetCellCoordinatesFromPixel(double x, double y, int cellSize);
        public abstract List<(int row, int col)> GetNeighbors(int row, int col);
    }
}
