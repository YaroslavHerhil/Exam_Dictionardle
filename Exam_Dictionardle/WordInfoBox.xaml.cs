using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Exam_Dictionardle.Modules;
    
namespace Exam_Dictionardle
{
    /// <summary>
    /// Interaction logic for WordInfoBox.xaml
    /// </summary>
    public partial class WordInfoBox : UserControl
    {

        
        public WordInfoBox(WordInfo theWord, WordInfo compareWord )
        {
            InitializeComponent();

            int matches = 0;
            for (int i = 0; i < compareWord.Word.Length; i++)
            {
                char symbol = compareWord.Word[i];
                if (theWord.Word.Contains(symbol) && symbol != ' ')
                {
                    List<int> foundIndexes = new List<int>();
                    for (int j = theWord.Word.IndexOf(symbol); j> -1; j = theWord.Word.IndexOf(symbol, j + 1))
                    {
                        // for loop end when i=-1 ('a' not found)
                        foundIndexes.Add(j);
                    }


                    if (foundIndexes.Contains(i))
                    {
                        wordTextBlock.Inlines.Add(new Run(symbol.ToString()) { Foreground = Brushes.DarkGreen });
                    }
                    else
                    {
                        wordTextBlock.Inlines.Add(new Run(symbol.ToString()) { Foreground = Brushes.Gold });
                    }
                    wordBorder.BorderBrush = Brushes.Gold;
                    matches++;
                }
                else
                {
                    wordTextBlock.Inlines.Add(new Run(symbol.ToString()) { Foreground = Brushes.Gray });
                }
            }
            if(matches == 0) 
            {
                wordTextBlock.Foreground = Brushes.Maroon;
                wordBorder.BorderBrush = Brushes.Maroon;
            }
            else if (matches == compareWord.Pronunciation.Length)
            {
                wordTextBlock.Foreground = Brushes.DarkGreen;
                wordBorder.BorderBrush = Brushes.DarkGreen;
            }

            matches = 0;
            for (int i = 0; i < compareWord.Pronunciation.Length; i++)
            {
                char symbol = compareWord.Pronunciation[i];
                if (theWord.Pronunciation.Contains(symbol) &&( symbol != ' ' || symbol != '/'))
                {
                    if (i == theWord.Word.IndexOf(symbol))
                    {
                        pronunciationTextBlock.Inlines.Add(new Run(symbol.ToString()) { Foreground = Brushes.DarkGreen });
                    }
                    else
                    {
                        pronunciationTextBlock.Inlines.Add(new Run(symbol.ToString()) { Foreground = Brushes.Gold });
                    }
                    pronBorder.BorderBrush = Brushes.Gold;
                    matches++;
                }
                else
                {
                    pronunciationTextBlock.Inlines.Add(new Run(symbol.ToString()) { Foreground = Brushes.Gray });
                }
            }
            if (matches == 0)
            {
                pronunciationTextBlock.Foreground = Brushes.Maroon;
                pronBorder.BorderBrush = Brushes.Maroon;
            }
            else if(matches == compareWord.Pronunciation.Length) 
            {
                pronunciationTextBlock.Foreground = Brushes.DarkGreen;
                pronBorder.BorderBrush = Brushes.DarkGreen;
            }

            if (theWord.Type == compareWord.Type) 
            {
                typeTextBlock.Foreground = Brushes.DarkGreen;
                typeBorder.BorderBrush = Brushes.DarkGreen;
            }
            else
            {
                typeTextBlock.Foreground = Brushes.Maroon;
                typeBorder.BorderBrush = Brushes.Maroon;
            }
            typeTextBlock.Text = compareWord.Type;


            countTextBlock.Text = compareWord.Word.Length.ToString();
            if (theWord.Word.Length == compareWord.Word.Length) 
            {
                countTextBlock.Foreground = Brushes.DarkGreen;
                countBorder.BorderBrush = Brushes.DarkGreen;
            }
            else if(theWord.Word.Length >= compareWord.Word.Length)
            {
                countTextBlock.Text += "↑";
                countTextBlock.Foreground = Brushes.Maroon;
                countBorder.BorderBrush = Brushes.Maroon;
            }
            else if (theWord.Word.Length <= compareWord.Word.Length)
            {
                countTextBlock.Text += "↓";
                countTextBlock.Foreground = Brushes.Maroon;
                countBorder.BorderBrush = Brushes.Maroon;
            }


        }
       
    }
}
