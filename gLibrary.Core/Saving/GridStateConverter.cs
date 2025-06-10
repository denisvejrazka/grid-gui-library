using System;

namespace gLibrary.Core.Saving;

public static class GridStateConverter
{
    public static GridState FromMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        var values = new List<List<int>>();

        for (int i = 0; i < rows; i++)
        {
            var row = new List<int>();
            for (int j = 0; j < cols; j++)
                row.Add(matrix[i, j]);
            values.Add(row);
        }

        return new GridState
        {
            Rows = rows,
            Columns = cols,
            GridValues = values
        };
    }

    public static int[,] ToMatrix(GridState state)
    {
        int[,] matrix = new int[state.Rows, state.Columns];
        for (int i = 0; i < state.Rows; i++)
            for (int j = 0; j < state.Columns; j++)
                matrix[i, j] = state.GridValues[i][j];
        return matrix;
    }
}
