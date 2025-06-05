using gLibrary.Core.Mapping;
using gLibrary.Core.Engine.Models;

namespace Tri.Game.Mapping;

public class TriMapper: IMap
    {
        public Cell GetMap(int value, int row, int col) 
        {
            return value switch
            {
                0 => new Cell(row, col, "#FFFFFF", ""),
                1 => new Cell(row, col, "#deffe7"),
                99 => new Cell(row, col, "#ff0019"),
                _ => new Cell(row, col, "#FFFFFF", value.ToString())
            };
        }
    }
