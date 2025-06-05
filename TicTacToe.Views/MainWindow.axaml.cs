using Avalonia.Controls;
using Avalonia.Threading;
using gLibrary.Rendering.Avalonia;
using System;
using TicTacToe.Game;

namespace TicTacToe.Views
{
    public partial class MainWindow : Window
    {
        private TicTacToeLogic _game;
        private AvaloniaSquareRenderer _avaloniaRenderer;
        private const int CellSize = 100;
        private const int Rows = 3;
        private const int Cols = 3;

        public MainWindow()
        {
            InitializeComponent();
            _avaloniaRenderer = new AvaloniaSquareRenderer(
                TicTacToeBackground,
                OnCellClicked,
                null,
                CellSize
            );

            _game = new TicTacToeLogic(Rows, Cols, CellSize, _avaloniaRenderer);

            TicTacToeBackground.Width = Cols * CellSize;
            TicTacToeBackground.Height = Rows * CellSize;
            this.Width = Cols * CellSize + 40;
            this.Height = Rows * CellSize + 100;

            _game.InitializeGrid();

            _game.RemoteMoveArrived += (r, c) =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                });
            };

            InitWebSocket();

            SaveButton.Click += SaveButton_Click;
            LoadButton.Click += LoadButton_Click;
        }

        private async void InitWebSocket()
        {
            string uri = "ws://localhost:5000/ws/";
            try
            {
                await _game.InitializeWebSocketAsync(uri);
            }
            catch (Exception ex)
            {
                Console.WriteLine("WebSocket init error: " + ex.Message);
            }
        }

        private void OnCellClicked(object? sender, gLibrary.Events.CellClickEventArgs args)
        {
            int row = args.Cell.Row;
            int col = args.Cell.Column;
            _game.HandleLocalMove(row, col);
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            _game.SaveGame("Saves/tictactoe.json");
        }

        private void LoadButton_Click(object? sender, RoutedEventArgs e)
        {
            _game.LoadGame("Saves/tictactoe.json");
        }
    }
}