using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using gLibrary.Core.Engine;
using gLibrary.Core.Helping;
using gLibrary.Core.Mapping;
using gLibrary.Rendering.Ava;
using System;
using TicTacToe.Game;
using TicTacToe.Game.Mapping;
using gLibrary.Core.Events;
using gLibrary.Core.Saving;
using gLibrary.Core.Communication;
using gLibrary.Core.Rendering;

namespace TicTacToe.Views
{
    public partial class MainWindow : Window
    {
        private GridEngine _engine;
        private AvaloniaSquareRenderer _avaloniaSquareRenderer;
        private SquareHelper _squareHelper;
        private SquareRenderer _squareRenderer;
        private TicTacToeMapper _mapper;
        private TicTacToeLogic _TicTacToeLogic;
        private int _size;
        private GameStateManager _gameStateManager;
        private WebSocketManager _webSocketManager;

        public MainWindow()
        {
            InitializeComponent();
            _size = 55;
            _engine = new GridEngine(3, 3);
            _engine.GenerateGrid();

            int canvasWidth = _engine.Columns * _size;
            int canvasHeight = _engine.Rows * _size;
            TicTacToeBackground.Width = canvasWidth;
            TicTacToeBackground.Height = canvasHeight;
            this.Width = canvasWidth + 80;
            this.Height = canvasHeight + 120;

            InitializeGrid();
            InitWebSocket();
        }

        private void InitializeGrid()
        {
            _mapper = new TicTacToeMapper();
            _squareHelper = new SquareHelper(_engine);
            _avaloniaSquareRenderer = new AvaloniaSquareRenderer(TicTacToeBackground, _engine, _mapper, _squareHelper, _size, OnClick);
            _squareRenderer = new SquareRenderer(_avaloniaSquareRenderer, _engine, _mapper, _squareHelper, _size);
            _squareRenderer.RenderGrid();

            _TicTacToeLogic = new TicTacToeLogic(_engine);
            _gameStateManager = new GameStateManager();
        }

        private async void InitWebSocket()
        {
            _webSocketManager = new WebSocketManager();
            _webSocketManager.OnMessageReceived += (row, col) =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _TicTacToeLogic.MakeMove(row, col);
                    _avaloniaSquareRenderer.UpdateCell(row, col);
                });
            };

            await _webSocketManager.InitializeAsync("ws://:/ws/");
        }

        public void SaveGame()
        {
            _gameStateManager.SaveGame(_TicTacToeLogic, "gLibrary.Core/Saving/tictactoe_save.json");
        }

        public void LoadGame()
        {
            if (_gameStateManager.LoadGame(_TicTacToeLogic, "gLibrary.Core/Saving/tictactoe_save.json"))
            {
                _squareRenderer.RenderGrid();
            }
        }

        public void OnClick(object? sender, CellClickEventArgs args)
        {
            int row = args.Cell.Row;
            int col = args.Cell.Column;

            if (_webSocketManager?.IsConnected == true)
            {
                _webSocketManager.Send(row, col);
            }

            _TicTacToeLogic.MakeMove(row, col);
            _squareRenderer.RenderGrid();
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            SaveGame();
        }

        private void LoadButton_Click(object? sender, RoutedEventArgs e)
        {
            LoadGame();
        }
    }
}