using System;
using System.Collections.Generic;
using System.Linq;
using gLibrary.Core.Engine;
using gLibrary.Core.Helping;

namespace Tri.Game
{
    public class TriLogic
    {
        private readonly GridEngine _engine;
        private readonly TriangleHelper _helper;
        private readonly Random _random = new();
        private readonly List<(int, int)> _blacklist = new();
        private readonly List<(int, int)> _notPrimes = new();

        public int Score { get; private set; }
        public bool IsGameOver { get; private set; }

        public TriLogic(GridEngine engine, TriangleHelper helper)
        {
            _engine = engine;
            _helper = helper;
        }

        public void InitializeGrid()
        {
            for (int i = 0; i < _engine.Rows; i++)
                for (int j = 0; j < _engine.Columns; j++)
                    _engine.SetCellValue(i, j, 0);
        }

        public List<(int row, int col)> HandleClick(int row, int col, out List<(int row, int col, int newValue)> changedCells)
        {
            changedCells = new();
            List<(int, int)> affected = new List<(int, int)>();

            if (IsGameOver) return affected;

            int current = _engine.GetCellValue(row, col);
            if (IsPrime(current) && !_blacklist.Contains((row, col)))
            {
                _engine.SetCellValue(row, col, 1);
                affected.Add((row, col));
                _blacklist.Add((row, col));
                Score++;
            }
            else if (current == 0)
            {
                _engine.SetCellValue(row, col, 1);
                affected.Add((row, col));
                _blacklist.Add((row, col));
            }
            else
            {
                _engine.SetCellValue(row, col, 99);
                affected.Add((row, col));
                _blacklist.Add((row, col));
                _notPrimes.Add((row, col));
            }

            var newlyChanged = GenerateNeighborValues(row, col);
            changedCells.AddRange(newlyChanged);
            affected.AddRange(newlyChanged.Select(c => (c.row, c.col)));

            if (!AnyPrimesLeft())
                IsGameOver = true;

            return affected;
        }

        private List<(int row, int col, int newValue)> GenerateNeighborValues(int row, int col)
        {
            List<(int, int, int)> updated = new List<(int, int, int)>();

            List<(int, int)> neighbors = _helper.GetNeighbors(row, col)
                .Where(n => !_blacklist.Contains(n) && _engine.GetCellValue(n.Item1, n.Item2) == 0)
                .ToList();

            if (!neighbors.Any()) return updated;

            var prime = neighbors[_random.Next(neighbors.Count)];
            int primeVal = GenerateRandomPrime();
            _engine.SetCellValue(prime.Item1, prime.Item2, primeVal);
            updated.Add((prime.Item1, prime.Item2, primeVal));

            foreach (var (nRow, nCol) in neighbors)
            {
                if ((nRow, nCol) == prime) continue;

                int val;
                do { val = _random.Next(2, 97); } while (IsPrime(val));
                _engine.SetCellValue(nRow, nCol, val);
                updated.Add((nRow, nCol, val));
            }

            return updated;
        }

        private bool AnyPrimesLeft()
        {
            for (int i = 0; i < _engine.Rows; i++)
                for (int j = 0; j < _engine.Columns; j++)
                    if (IsPrime(_engine.GetCellValue(i, j)))
                        return true;
            return false;
        }

        private int GenerateRandomPrime()
        {
            int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
            return primes[_random.Next(primes.Length)];
        }

        private static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;
            int boundary = (int)Math.Sqrt(number);
            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;
            return true;
        }
    }
}