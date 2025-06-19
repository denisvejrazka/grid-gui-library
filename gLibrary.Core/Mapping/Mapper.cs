using gLibrary.Core.Engine.Models;
using gLibrary.Core.Mapping;

namespace gLibrary.Core.Mapping
{
    public class Mapper : IMap
    {   
        public Cell GetMap(int value, int row, int col) 
        {
            return value switch
            {
                0 => new Cell(row, col, "#FFFFFF"), 
                1 => new Cell(row, col, "#FFFFFF", "a"),
                2 => new Cell(row, col, "#FFFFFF", "b"),
                3 => new Cell(row, col, "#FFFFFF", "c"),
                4 => new Cell(row, col, "#FFFFFF"),
                _ => new Cell(row, col, "#CCCCCC")
            };
        }
    }
}