using Exam_Dictionardle.Modules;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace Exam_Dictionardle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string json = ApiController.GetWord("tower").Result;
            JsonDocument wordInfoJson = JsonDocument.Parse(json);
            WordInfo theWord = new WordInfo(wordInfoJson.RootElement);



        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            string json = ApiController.GetWord(wordBox.Text.ToLower()).Result;
            JsonDocument wordInfoJson = JsonDocument.Parse(json);
            WordInfo wordInfo = new WordInfo(wordInfoJson.RootElement);
            MessageBox.Show($"{wordInfo.Word}\n{wordInfo.Pronunciation}\n{wordInfo.AudioUrl}\n{wordInfo.Description}");
        }
    }
}