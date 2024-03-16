using Exam_Dictionardle.DAL;
using Exam_Dictionardle.Modules;
using System.Text.Json;
using System.Windows;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Drawing.Drawing2D;
using System.Drawing;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.ObjectModel;
using Exam_Dictionardle.DAL.Modules;
using System.Windows.Data;

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
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += UpdateUi;
            _timer.Start();
            SetupGrid();

        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if(wordBox.Text.Length == 0) 
            {
                toolTipBlock.Text = "Write something!";
                return;
            }
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
            }
            if (!_gameController.IsPlaying)
            {
                hintBtn.Visibility = Visibility.Collapsed;
                if (_gameController.Tries == 0)
                {
                    toolTipBlock.Text = $"You lost. The correct word was {_gameController.TheWord.Word}. Better luck next time!";
                }
                if (_gameController.Tries > 0)
                {
                    toolTipBlock.Text = "You won. Horray!";
                }
                guessContainer.Children.Clear();
            }
            if (_gameController.Tries == 5)
            {
                HintOption();
            }


            wordBox.Text = "";
            UpdateGrid();
        }


        private void HintOption()
        {
            toolTipBlock.Text = "You lost half of your lives so you get a hint! It does cost a life and 1800 points..";
            hintBtn.Visibility = Visibility.Visible; 

        }

        private void UpdateGrid()
        {
            if(gameGrid.Tag == "Highscores")
            {
                gameGrid.ItemsSource = _gameController.GetHighscores();
            }
            else if(gameGrid.Tag == "My games")
            {
                gameGrid.ItemsSource = _gameController.GetGamesByThisPlayer();
            }
        }

        private void suggestion_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            wordBox.Text = thisButton.Content.ToString();

            toolTipBlock.Text = "Here you go!";
            suggestContainer.Children.Clear();

        }

        void SetupGrid()
        {
            List<Game> games = _gameController.GetHighscores();



            gameGrid.AutoGenerateColumns = false;
            gameGrid.ItemsSource = games;

            DataGridTextColumn playerColumn = new DataGridTextColumn
            {
                Header = "Player",
                Binding = new Binding("Player.UserName")
            };
            gameGrid.Columns.Add(playerColumn);

            DataGridTextColumn wordColumn = new DataGridTextColumn
            {
                Header = "Word",
                Binding = new Binding("Word.Word")
            };
            gameGrid.Columns.Add(wordColumn);

            DataGridTextColumn scoreColumn = new DataGridTextColumn
            {
                Header = "Score",
                Binding = new Binding("Score")
            };
            gameGrid.Columns.Add(scoreColumn);
            DataGridTextColumn triesColumn = new DataGridTextColumn
            {
                Header = "Lives",
                Binding = new Binding("Tries")
            };
            gameGrid.Columns.Add(triesColumn);
            Binding binding = new Binding("Miliseconds");
            binding.Converter = new MilisecondsConverter();
            DataGridTextColumn timeColumn = new DataGridTextColumn
            {
                Header = "Time",
                Binding = binding,
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
            if(_gameController.PlayerController.Register(regUserNameBox.Text, regLoginBox.Text, regPasswordBox.Password))
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
            if (_gameController.PlayerController.Login(regLoginBox.Text, regPasswordBox.Password))
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gameGrid.Tag = "Highscores";
            UpdateGrid();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(_gameController.PlayerController.CurrentPlayer.UserName == "Anonymous")
            {
                toolTipBlock.Text = "You need to login first!";
            }
            else
            {
                gameGrid.Tag = "My games";
                UpdateGrid();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            hintBtn.Visibility = Visibility.Collapsed;
            toolTipBlock.Text = _gameController.TheWord.Description;
            _gameController.Tries--;
            _gameController.Score -= 1800;
        }
    }
}