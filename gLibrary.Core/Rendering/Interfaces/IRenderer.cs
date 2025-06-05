using gLibrary.Core.Engine.Models;

namespace gLibrary.Core.Rendering
{
    public interface IRenderer
    {
        void RenderCell(int row, int col, Cell cell, int cellSize, (double x, double y) position);
        void Clear();
    }
}