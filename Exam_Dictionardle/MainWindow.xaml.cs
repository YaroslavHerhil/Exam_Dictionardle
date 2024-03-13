using Exam_Dictionardle.DAL;
using Exam_Dictionardle.Modules;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace Exam_Dictionardle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameController _gameController;

        public MainWindow()
        {
            _gameController = new GameController();
            InitializeComponent();
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



    }
}