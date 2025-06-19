using gLibrary.Core.Rendering;
using gLibrary.Core.Engine;
using gLibrary.Core.Events;
using gLibrary.Core.Helping;
using gLibrary.Core.Mapping;
using System.Threading.Tasks;

namespace MemTest.Game
{
    public class MemTestLogic
    {
        private readonly GridEngine _engine;
        private readonly IMap _mapper;
        private readonly BaseHelper _helper;
        private readonly IRenderer _renderer;
        private readonly int _cellSize;
        private readonly Random _random = new();

        private int _score;
        private List<(int Row, int Col)> _sequence = new();
        private List<(int Row, int Col)> _playerInput = new();
        private int _currentStep = 0;
        private bool _isPlayerTurn = false;
        private bool _gameStarted = false;

        public MemTestLogic(GridEngine engine, IMap mapper, BaseHelper helper, IRenderer renderer, int cellSize)
        {
            _engine = engine;
            _mapper = mapper;
            _helper = helper;
            _renderer = renderer;
            _cellSize = cellSize;
        }

        public void Initialize()
        {
            for (int i = 0; i < _engine.Rows; i++)
                for (int j = 0; j < _engine.Columns; j++)
                    _engine.SetCellValue(i, j, 0);

            _renderer.Clear();

            new SquareRenderer(_renderer, _engine, _mapper, _helper, _cellSize)
                .RenderGrid();

            _score = 0;
            _sequence.Clear();
            _playerInput.Clear();
            _currentStep = 0;
            _isPlayerTurn = false;
            _gameStarted = false;
        }

        private async Task StartGameAsync()
        {
            _score = 0;
            _sequence.Clear();
            _playerInput.Clear();
            _currentStep = 0;
            _gameStarted = true;

            await AddRandomCellToSequence();
            await ShowSequenceAsync();
        }

        public async void HandleCellClick(object? sender, CellClickEventArgs args)
        {
            int row = args.Cell.Row;
            int col = args.Cell.Column;

            if (!_gameStarted)
            {
                await StartGameAsync();
                return;
            }

            if (!_isPlayerTurn)
                return;

            _playerInput.Add((row, col));

            _engine.SetCellValue(row, col, 1);
            _renderer.RenderCell(row, col, _mapper.GetMap(1, row, col), _cellSize, _helper.GetPosition(row, col, _cellSize));
            await Task.Delay(200);
            _engine.SetCellValue(row, col, 0);
            _renderer.RenderCell(row, col, _mapper.GetMap(0, row, col), _cellSize, _helper.GetPosition(row, col, _cellSize));

            if (_sequence[_currentStep] != (row, col))
            {
                EndGame();
                return;
            }

            _currentStep++;
            if (_currentStep >= _sequence.Count)
            {
                _score++;
                await AddRandomCellToSequence();
                await Task.Delay(300);
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

        private async Task AddRandomCellToSequence()
        {
            int row = _random.Next(0, _engine.Rows);
            int col = _random.Next(0, _engine.Columns);
            _sequence.Add((row, col));
        }

        private void EndGame()
        {
            _isPlayerTurn = false;
            _gameStarted = false;
        }
    }
}