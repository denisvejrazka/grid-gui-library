using gLibrary.Core.Engine;
using gLibrary.Core.Helping;
using gLibrary.Core.Mapping;

namespace gLibrary.Core.Rendering
{
    public class HexagonRenderer : BaseRenderer
    {
        public HexagonRenderer(IRenderer renderer, GridEngine engine, IMap mapper, BaseHelper helper, int cellSize) : base(renderer, engine, mapper, helper, cellSize) { }
    }
}
