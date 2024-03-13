using Exam_Dictionardle.DAL;
using Exam_Dictionardle.Modules;
using System.Text.Json;
using System.Windows;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Exam_Dictionardle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameController _gameController;
        private System.Timers.Timer _timer;
        public MainWindow()
        {
            
            InitializeComponent();
            _gameController = new GameController();
            CreateGrid();
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += UpdateUi;
            _timer.Start();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            WordInfo guessWord = _gameController.CheckWord(wordBox.Text);

            if(guessWord.Word == null) 
            {
                toolTipBlock.Text = "Did you mean...";

                List<string> suggestions = _gameController.GetSuggestions(wordBox.Text);
                suggestContainer.Children.Clear();
                foreach(string suggestion in suggestions) 
                { 
                    Button newSuggestion = new Button();
                    newSuggestion.Content = suggestion;
                    suggestContainer.Children.Add(newSuggestion);
                }
            }
            else
            {
                guessContainer.Children.Insert(0, new WordInfoBox(_gameController.TheWord, guessWord));

                if (!_gameController.IsPlaying)
                {
                    if (_gameController.Tries == 0)
                    {
                        MessageBox.Show("You lost. Sad!");
                    }
                    if (_gameController.Tries > 0)
                    {
                        MessageBox.Show("You Won. Nice.");
                    }
                    guessContainer.Children.Clear();
                }
            }


        }
        private void suggestion_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            wordBox.Text = thisButton.Content.ToString();

            toolTipBlock.Text = "Here you go!";
            suggestContainer.Children.Clear();

        }

        void CreateGrid()
        {
            List<Game> games = _gameController.GetAllGames();



            gameGrid.AutoGenerateColumns = false;
            gameGrid.ItemsSource = games;

            DataGridTextColumn playerColumn = new DataGridTextColumn
            {
                Header = "Player",
                Binding = new System.Windows.Data.Binding("Player.UserName")
            };
            gameGrid.Columns.Add(playerColumn);

            DataGridTextColumn wordColumn = new DataGridTextColumn
            {
                Header = "Word",
                Binding = new System.Windows.Data.Binding("Word.Word")
            };
            gameGrid.Columns.Add(wordColumn);

            DataGridTextColumn scoreColumn = new DataGridTextColumn
            {
                Header = "Score",
                Binding = new System.Windows.Data.Binding("Score")
            };
            gameGrid.Columns.Add(scoreColumn);
            DataGridTextColumn triesColumn = new DataGridTextColumn
            {
                Header = "Tries",
                Binding = new System.Windows.Data.Binding("Tries")
            };
            gameGrid.Columns.Add(triesColumn);
            DataGridTextColumn timeColumn = new DataGridTextColumn
            {
                Header = "Time",
                Binding = new System.Windows.Data.Binding("Miliseconds")
            };
            gameGrid.Columns.Add(timeColumn);
        }

        void UpdateUi(object sender, ElapsedEventArgs e)
        {

            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                timeHud.Text = $"{_gameController.GetTimeString()}";
                triesHud.Text = $"{_gameController.Tries}/9";
                scoreHud.Text = $"{_gameController.Score}";
            }));
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            if(_gameController.PlayerController.Register(regUserNameBox.Text, regLoginBox.Text, regPasswordBox.Text))
            {
                toolTipBlock.Text = $"Greetings, {_gameController.PlayerController.CurrentPlayer.UserName}! Welcome to Dictionardle.";
                loggedOnPanel.Visibility = Visibility.Visible;
                loggedOutPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Error, Maybe try different login or password?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void regDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            loginPanel.Visibility = Visibility.Collapsed;
            registerPanel.Visibility = Visibility.Visible;
        }

        private void logDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            loginPanel.Visibility = Visibility.Visible;
            registerPanel.Visibility = Visibility.Collapsed;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_gameController.PlayerController.Login(regLoginBox.Text, regPasswordBox.Text))
            {
                toolTipBlock.Text = $"Hi there, {_gameController.PlayerController.CurrentPlayer.UserName}. Welcome back!";
                loggedOnPanel.Visibility = Visibility.Visible;
                loggedOutPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Error, Maybe try different login or password?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void signOutBtn_Click(object sender, RoutedEventArgs e)
        {
            _gameController.PlayerController.SetAnonymous();
            toolTipBlock.Text = "GoodBye!";
            loggedOnPanel.Visibility = Visibility.Collapsed;
            loggedOutPanel.Visibility = Visibility.Visible;
        }
    }
}