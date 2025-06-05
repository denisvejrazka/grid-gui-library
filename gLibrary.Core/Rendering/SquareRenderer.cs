using gLibrary.Core.Engine;
using gLibrary.Core.Helping;
using gLibrary.Core.Mapping;

//abstract renderer
namespace gLibrary.Core.Rendering
{
    public class SquareRenderer : BaseRenderer
    {
        public SquareRenderer(IRenderer renderer, GridEngine engine, IMap mapper, BaseHelper helper, int cellSize) : base(renderer, engine, mapper, helper, cellSize) { }
    }
}
