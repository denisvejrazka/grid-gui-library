using gLibrary.Core.Engine;
using gLibrary.Core.Helping;
using gLibrary.Core.Rendering;
using gLibrary.Core.Saving;
using gLibrary.Core.Communication;
using TicTacToe.Game.Mapping;
using gLibrary.Core.Mapping;

namespace TicTacToe.Game
{
    public class TicTacToeLogic
    {
        private readonly GridEngine _engine;
        private readonly Player _player1;
        private readonly Player _player2;
        private Player _currentPlayer;
        private int _movesMade;
        private bool _gameOver;
        private readonly IMap _mapper;
        private readonly BaseHelper _helper;
        private readonly IRenderer _renderer;
        private readonly SquareRenderer _squareRenderer;
        private readonly GameStateManager _gameStateManager;
        private readonly WebSocketManager _webSocketManager;
        private readonly int _cellSize;

        public event Action<int, int>? RemoteMoveArrived;

        public TicTacToeLogic(int rows, int cols, int cellSize, IRenderer renderer)
        {
            _cellSize = cellSize;
            _engine = new GridEngine(rows, cols);
            _player1 = new Player(1);
            _player2 = new Player(2);
            _currentPlayer = _player1;
            _movesMade = 0;
            _gameOver = false;
            _mapper = new TicTacToeMapper();
            _helper = new SquareHelper(_engine);
            _renderer = renderer;
            _squareRenderer = new SquareRenderer(_renderer, _engine, _mapper, (SquareHelper)_helper, _cellSize);
            _gameStateManager = new GameStateManager();
            _webSocketManager = new WebSocketManager();
            _webSocketManager.OnMessageReceived += (row, col) => HandleRemoteMove(row, col);
        }

        public void InitializeGrid()
        {
            _engine.GenerateGrid();
            _renderer.Clear();
            _squareRenderer.RenderGrid();
        }

        public void HandleLocalMove(int row, int col)
        {
            if (_webSocketManager.IsConnected)
            {
                _webSocketManager.Send(row, col);
            }
            MakeMove(row, col);
            _renderer.Clear();
            _squareRenderer.RenderGrid();
        }

        private void HandleRemoteMove(int row, int col)
        {
            MakeMove(row, col);
            _renderer.Clear();
            _squareRenderer.RenderGrid();
            RemoteMoveArrived?.Invoke(row, col);
        }

        private bool MakeMove(int row, int col)
        {
            if (_gameOver) return false;
            if (_engine.GetCellValue(row, col) != 0) return false;
            _engine.SetCellValue(row, col, _currentPlayer.PlayerValue);
            _movesMade++;
            if (CheckWin(row, col, _currentPlayer.PlayerValue) || _movesMade == _engine.Rows * _engine.Columns)
            {
                _gameOver = true;
                return true;
            }
            _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
            return false;
        }

        private bool CheckWin(int row, int col, int player)
        {
            return CheckLine(row, 0, 0, 1, player)
                || CheckLine(0, col, 1, 0, player)
                || (row == col && CheckLine(0, 0, 1, 1, player))
                || (row + col == _engine.Rows - 1 && CheckLine(0, _engine.Columns - 1, 1, -1, player));
        }

        private bool CheckLine(int startRow, int startCol, int dRow, int dCol, int player)
        {
            for (int i = 0; i < _engine.Rows; i++)
            {
                int r = startRow + i * dRow;
                int c = startCol + i * dCol;
                if (_engine.GetCellValue(r, c) != player) return false;
            }
            return true;
        }

        public void SaveGame(string path)
        {
            var grid = _engine.ExportGrid();
            var list = new List<List<int>>();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                var row = new List<int>();
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    row.Add(grid[i, j]);
                }
                list.Add(row);
            }
            var state = new GridState { GridValues = list };
            _gameStateManager.SaveGame(state, path);
        }

        public void LoadGame(string path)
        {
            var state = _gameStateManager.LoadGame(path);
            if (state == null) return;
            int rows = state.GridValues.Count;
            int cols = state.GridValues[0].Count;
            var newGrid = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    newGrid[i, j] = state.GridValues[i][j];
                }
            }
            _engine.SetGrid(newGrid);
            _movesMade = CountMoves(newGrid);
            _currentPlayer = (_movesMade % 2 == 0) ? _player1 : _player2;
            _gameOver = false;
            _renderer.Clear();
            _squareRenderer.RenderGrid();
        }

        private int CountMoves(int[,] grid)
        {
            int count = 0;
            foreach (var value in grid)
            {
                if (value != 0) count++;
            }
            return count;
        }

        public async Task InitializeWebSocketAsync(string uri)
        {
            await _webSocketManager.InitializeAsync(uri);
        }

        public bool IsSocketConnected => _webSocketManager.IsConnected;
    }
}
