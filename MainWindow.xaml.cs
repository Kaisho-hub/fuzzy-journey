using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        private bool isPlayerXTurn = true;
        private int movesCount = 0;
        Random rnd = new Random();
        public static string[,] Board = new string[3, 3]; //vytvářím si virtuální 2D pole
        private static int WinCount = 0; //počítá kolik výher hráč získal kvůli levelů

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button.Content != null)
                return;

            button.Content = "X";
            UpdateBoard();
            movesCount++;

            if (CheckForWin())
            {
                StatusText.Text = "You won!";
                WinCount++;
                DisableButtons();
                return;
            }
            else if (movesCount == 9)
            {
                StatusText.Text = "It's a draw!";
                return;
            }

            isPlayerXTurn = !isPlayerXTurn;

           

            if (WinCount < 3)
            {
                EasyBotMove();
                
                
            }
            else 
            {
                
                MediumBotMove();
                

            }
            
        }

        private void MediumBotMove()
        {
            if (movesCount >= 9)
                return;

            // Prohlížení zda na dalším tahu nemůže bot vyhrát nebo pokazit shodu
            if (TryToWinOrBlock("O") || TryToWinOrBlock("X"))
                return;

            // Pokud se nic takového nenajde, provde jednoduchý tah
            EasyBotMove();

            if (CheckForWin()) 
            {
                StatusText.Text = "You lost!";
                WinCount = 0;
                DisableButtons();
                return;
            }
            else if (movesCount == 9)
            {
                StatusText.Text = "It's a draw!";
                return;
            }

            isPlayerXTurn = !isPlayerXTurn;
        }

        private bool TryToWinOrBlock(string player)
        {
            
            return CheckLines(player) || CheckDiagonals(player);
        }

        private bool CheckLines(string player)
        {
            for (int i = 0; i < 3; i++)
            {
                // Řádky
                if (CheckAndPlace(player, i, 0, i, 1, i, 2)) return true;
                // Sloupce
                if (CheckAndPlace(player, 0, i, 1, i, 2, i)) return true;
            }
            return false;
        }

        private bool CheckDiagonals(string player)
        {
            // diagonaly
            if (CheckAndPlace(player, 0, 0, 1, 1, 2, 2)) return true;
            
            if (CheckAndPlace(player, 0, 2, 1, 1, 2, 0)) return true;
            return false;
        }

        private bool CheckAndPlace(string player, int x1, int y1, int x2, int y2, int x3, int y3)
        {
            var positions = new[] { (x1, y1), (x2, y2), (x3, y3) };
            int playerCount = positions.Count(pos => Board[pos.Item1, pos.Item2] == player);
            int emptyCount = positions.Count(pos => Board[pos.Item1, pos.Item2] == null);

            if (playerCount == 2 && emptyCount == 1)
            {
                var emptyPos = positions.First(pos => Board[pos.Item1, pos.Item2] == null);
                Button button = (Button)BoardGrid.FindName($"Button{emptyPos.Item1}{emptyPos.Item2}");

                button.Content = "O";
                UpdateBoard();
                movesCount++;
                if (CheckForWin())
                {
                    StatusText.Text = "You lost!";
                    WinCount = 0;
                    DisableButtons();
                }
                return true;
            }

            return false;
        }

        private void EasyBotMove()
        {
            if (movesCount >= 9) return;

            int x, y;
            Button button = null;
            do
            {
                x = rnd.Next(0, 3);
                y = rnd.Next(0, 3);

                string buttonName = $"Button{x}{y}"; //vyhledávání podle jména a souřadnic
                button = (Button)BoardGrid.FindName(buttonName);
            } while (button == null || button.Content != null);

            button.Content = "O";
            
            UpdateBoard();
            movesCount++;

            if (CheckForWin())
            {
                StatusText.Text = "You lost!";
                DisableButtons();
                return;
            }
            else if (movesCount == 9)
            {
                StatusText.Text = "It's a draw!";
                return;
            }

            isPlayerXTurn = !isPlayerXTurn;
        }

        private bool CheckForWin()
        {
            // řádky a sloupce
            for (int i = 0; i < 3; i++)
            {
                if (Board[i, 0] != null && Board[i, 0] == Board[i, 1] && Board[i, 1] == Board[i, 2])
                    return true;
                if (Board[0, i] != null && Board[0, i] == Board[1, i] && Board[1, i] == Board[2, i])
                    return true;
            }

            // diagonály
            if (Board[0, 0] != null && Board[0, 0] == Board[1, 1] && Board[1, 1] == Board[2, 2])
                return true;
            if (Board[0, 2] != null && Board[0, 2] == Board[1, 1] && Board[1, 1] == Board[2, 0])
                return true;

            return false;
        }

        private void UpdateBoard()
        {
            foreach (UIElement element in BoardGrid.Children)
            {
                if (element is Button button && button.Content != null)
                {
                    string name = button.Name;
                    int row = int.Parse(name.Substring(6, 1));
                    int col = int.Parse(name.Substring(7, 1));
                    Board[row, col] = button.Content.ToString();
                }
            }
        }

        private void DisableButtons() 
        {
            foreach (UIElement element in BoardGrid.Children)
            {
                if (element is Button button)
                {
                    button.IsEnabled = false;
                }
            }
        }

        private void Reser_Click(object sender, RoutedEventArgs e)
        {
            
            ResetBoard();
            
            
        }

        private void ResetBoard()
        {
            foreach (UIElement element in BoardGrid.Children)
            {
                if (element is Button button)
                {
                    button.Content = null;
                    button.IsEnabled = true;
                }
            }

            movesCount = 0;
            isPlayerXTurn = true;
            Array.Clear(Board, 0, Board.Length);
            StatusText.Text = "";
        }

        private void MenuBack_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            
            menuWindow.Show();
            this.Close();
        }
    }
}
