using gLibrary.Core.Engine;

namespace gLibrary.Core.Helping
{
    public class TriangleHelper : BaseHelper
    {
        public TriangleHelper(GridEngine engine) : base(engine) { }

        public override (double x, double y) GetPosition(int row, int col, int cellSize)
        {
            double height = cellSize * Math.Sqrt(3) / 2.0;
            double xOffset = col * (cellSize * 0.5);
            double yOffset = row * height;
            return (xOffset, yOffset);
        }

        public override (int row, int col)? GetCellCoordinatesFromPixel(double x, double y, int cellSize)
        {
            double triHeight = cellSize * Math.Sqrt(3) / 2.0;

            for (int r = 0; r < Engine.Rows; r++)
            {
                for (int c = 0; c < Engine.Columns; c++)
                {
                    var (cellX, cellY) = GetPosition(r, c, cellSize);

                    bool isUpward = ((r + c) % 2) == 0;

                    (double X, double Y) A, B, C;
                    if (isUpward)
                    {
                        A = (cellX, cellY + triHeight);
                        B = (cellX + cellSize / 2.0, cellY);
                        C = (cellX + cellSize, cellY + triHeight);
                    }
                    else
                    {
                        A = (cellX, cellY);
                        B = (cellX + cellSize, cellY);
                        C = (cellX + cellSize / 2.0, cellY + triHeight);
                    }

                    if (IsPointInTriangle((x, y), A, B, C))
                    {
                        return (r, c);
                    }
                }
            }

            return null;
        }

        private bool IsPointInTriangle((double X, double Y) P, (double X, double Y) A, (double X, double Y) B, (double X, double Y) C)
        {
            double cross1 = (B.X - A.X) * (P.Y - A.Y) - (B.Y - A.Y) * (P.X - A.X);
            double cross2 = (C.X - B.X) * (P.Y - B.Y) - (C.Y - B.Y) * (P.X - B.X);
            double cross3 = (A.X - C.X) * (P.Y - C.Y) - (A.Y - C.Y) * (P.X - C.X);

            bool hasNeg = (cross1 < 0) || (cross2 < 0) || (cross3 < 0);
            bool hasPos = (cross1 > 0) || (cross2 > 0) || (cross3 > 0);

            return !(hasNeg && hasPos);
        }

        public override List<(int row, int col)> GetNeighbors(int row, int col)
        {
            List<(int row, int col)> neighbors = new List<(int row, int col)>();

            var directions = new (int dRow, int dCol)[]
            {
                (0, -1),
                (0, 1),
                ((row + col) % 2 == 0 ? (1, 0) : (-1, 0))
            };

            foreach (var (dr, dc) in directions)
            {
                int nr = row + dr;
                int nc = col + dc;
                if (nr >= 0 && nr < Engine.Rows && nc >= 0 && nc < Engine.Columns)
                {
                    neighbors.Add((nr, nc));
                }
            }

            return neighbors;
        }
    }
}
