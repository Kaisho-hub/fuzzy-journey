using System.Windows;

namespace TicTacToe
{
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow gameWindow = new MainWindow();
            gameWindow.Show();
            this.Close();
        }
    }
}