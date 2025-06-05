using gLibrary.Core.Engine.Models;

namespace gLibrary.Core.Mapping
{
    public interface IMap
    {
        Cell GetMap(int value, int row, int col);
    }   
}
