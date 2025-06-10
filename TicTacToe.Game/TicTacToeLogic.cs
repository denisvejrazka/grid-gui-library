using gLibrary.Core.Engine;
using gLibrary.Core.Helping;
using gLibrary.Core.Rendering;
using gLibrary.Core.Saving;
using gLibrary.Core.Communication;
using TicTacToe.Game.Mapping;
using gLibrary.Core.Mapping;

namespace TicTacToe.Game
{
    public class TicTacToeLogic: ISaveableGame
    {
        private GridEngine _engine;
        private Player _player1;
        private Player _player2;
        private Player _currentPlayer;
        private int _movesMade;
        private bool _gameOver;

        public TicTacToeLogic(GridEngine engine)
        {
            _engine = engine;
            _player1 = new Player(1);
            _player2 = new Player(2);
            _currentPlayer = _player1;
            _movesMade = 0;
            _gameOver = false;
        }

        public bool MakeMove(int row, int col)
        {
            if (_gameOver)
                return false;

            if (_engine.GetCellValue(row, col) == 0)
            {
                _engine.SetCellValue(row, col, _currentPlayer.PlayerValue);
                _movesMade++;

                if (CheckWin(row, col, _currentPlayer.PlayerValue))
                {
                    _gameOver = true;
                    return true;
                }

                if (_movesMade == _engine.Rows * _engine.Columns)
                {
                    _gameOver = true;
                    return true;
                }

                _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
            }

            return false;
        }

        private bool CheckWin(int row, int col, int player)
        {
            return CheckLine(row, 0, 0, 1, player) || CheckLine(0, col, 1, 0, player) || (row == col && CheckLine(0, 0, 1, 1, player)) || //  \
                   (row + col == _engine.Rows - 1 && CheckLine(0, _engine.Columns - 1, 1, -1, player)); //  /
        }

        private bool CheckLine(int startRow, int startCol, int dRow, int dCol, int player)
        {
            for (int i = 0; i < _engine.Rows; i++)
            {
                int r = startRow + i * dRow;
                int c = startCol + i * dCol;
                if (_engine.GetCellValue(r, c) != player)
                    return false;
            }
            return true;
        }

        // Saving
        public GridState ToGameState()
        {
            var state = GridStateConverter.FromMatrix(_engine.ExportGrid());
            state.MovesMade = _movesMade;
            state.CurrentPlayer = _currentPlayer.PlayerValue;
            return state;
        }

        public void FromGameState(GridState state)
        {
            var matrix = GridStateConverter.ToMatrix(state);
            _engine.SetGrid(matrix);

            _movesMade = state.MovesMade;
            _currentPlayer = (state.CurrentPlayer == _player1.PlayerValue) ? _player1 : _player2;
            _gameOver = false;
        }
    }
}
