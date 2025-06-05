using gLibrary.Core.Engine;
using gLibrary.Core.Mapping;
using gLibrary.Core.Helping;

namespace gLibrary.Core.Rendering
{
    public abstract class BaseRenderer
    {
        protected readonly IRenderer _renderer;
        protected readonly GridEngine _engine;
        protected readonly IMap _mapper;
        protected readonly BaseHelper _helper;
        protected readonly int _cellSize;

        protected BaseRenderer(IRenderer renderer, GridEngine engine, IMap mapper, BaseHelper helper, int cellSize)
        {
            _renderer = renderer;
            _engine = engine;
            _mapper = mapper;
            _helper = helper;
            _cellSize = cellSize;
        }

        public virtual void RenderGrid()
        {
            _renderer.Clear();
            for (int row = 0; row < _engine.Rows; row++)
            {
                for (int col = 0; col < _engine.Columns; col++)
                {
                    var value = _engine.GetCellValue(row, col);
                    var cell = _mapper.GetMap(value, row, col);
                    var position = _helper.GetPosition(row, col, _cellSize);
                    _renderer.RenderCell(row, col, cell, _cellSize, position);
                }
            }
        }
    }
}
