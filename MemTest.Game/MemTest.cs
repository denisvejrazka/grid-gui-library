using gLibrary.core.Rendering;
using gLibrary.Engine;
using gLibrary.Events;
using gLibrary.Helping;
using gLibrary.Mapping;
using gLibrary.Rendering;

namespace MemTest.Game
{
    public class MemTest
    {
        private readonly GridEngine _engine;
        private readonly IMap _mapper;
        private readonly SquareHelper _helper;
        private readonly IRenderer _renderer;
        private readonly int _cellSize;

        private readonly Random _random = new();
        private int _score;
        private List<(int Row, int Col)> _sequence = new();
        private List<(int Row, int Col)> _playerInput = new();
        private int _currentStep = 0;
        private bool _isPlayerTurn = false;

        public MemTest(
            GridEngine engine,
            IMap mapper,
            SquareHelper helper,
            IRenderer renderer,
            int cellSize)
        {
            _engine = engine;
            _mapper = mapper;
            _helper = helper;
            _renderer = renderer;
            _cellSize = cellSize;
        }

        public void Initialize()
        {
            // Vynulovat matici
            for (int i = 0; i < _engine.Rows; i++)
                for (int j = 0; j < _engine.Columns; j++)
                    _engine.SetCellValue(i, j, 0);

            // Vyčistit renderer (Canvas v Avalonii)
            _renderer.Clear();

            // Vykreslit úplně poprvé celou mřížku (bílé buňky apod.)
            var gridRenderer = new SquareRenderer(_renderer, _engine, _mapper, _helper, _cellSize);
            gridRenderer.RenderGrid();

            // Reset skóre a posloupnost
            _score = 0;
            _sequence.Clear();
            _playerInput.Clear();
            _currentStep = 0;
            _isPlayerTurn = false;

            // Začít hru
            StartGame();
        }

        private void StartGame()
        {
            _sequence.Clear();
            _score = 0;
            AddRandomCellToSequence();
            _ = ShowSequenceAsync();
        }

        /// <summary>
        /// Ošetří kliknutí hráče (zavolá ho Avalonia renderer přes CellClicked event).
        /// </summary>
        public async void HandleCellClick(object? sender, CellClickEventArgs args)
        {
            if (!_isPlayerTurn)
                return;

            int row = args.Cell.Row;
            int col = args.Cell.Column;

            _playerInput.Add((row, col));
            _engine.SetCellValue(row, col, 1);
            _renderer.RenderCell(row, col, _mapper.GetMap(1, row, col), _cellSize, _helper.GetPosition(row, col, _cellSize));
            await Task.Delay(200);
            _engine.SetCellValue(row, col, 0);
            _renderer.RenderCell(row, col, _mapper.GetMap(0, row, col), _cellSize, _helper.GetPosition(row, col, _cellSize));

            if (_sequence[_currentStep] != (row, col))
            {
                EndGame($"Špatné kliknutí! Skóre: {_score}");
                return;
            }

            _currentStep++;
            if (_currentStep >= _sequence.Count)
            {
                _score++;
                AddRandomCellToSequence();
                await Task.Delay(500);
                await ShowSequenceAsync();
            }
        }

        private async Task ShowSequenceAsync()
        {
            _isPlayerTurn = false;
            foreach (var (row, col) in _sequence)
            {
                _engine.SetCellValue(row, col, 1);
                _renderer.RenderCell(row, col, _mapper.GetMap(1, row, col), _cellSize, _helper.GetPosition(row, col, _cellSize));
                await Task.Delay(600);

                _engine.SetCellValue(row, col, 0);
                _renderer.RenderCell(row, col, _mapper.GetMap(0, row, col), _cellSize, _helper.GetPosition(row, col, _cellSize));
                await Task.Delay(200);
            }

            _playerInput.Clear();
            _currentStep = 0;
            _isPlayerTurn = true;
        }

        private void AddRandomCellToSequence()
        {
            int row = _random.Next(0, _engine.Rows);
            int col = _random.Next(0, _engine.Columns);
            _sequence.Add((row, col));
        }

        private void EndGame(string message)
        {
            _isPlayerTurn = false;
            Console.WriteLine(message);
        }
    }
}
