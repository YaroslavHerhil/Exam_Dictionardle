using Exam_Dictionardle.DAL;
using Exam_Dictionardle.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Exam_Dictionardle
{
    public class GameController
    {
        public int Score { get; set; }
        public int Tries { get; set; }
        public Stopwatch Stopwatch { get; set; }
        public WordInfo TheWord { get; set; }

        public bool IsPlaying { get; set; }

        private Repository _repository;
        

        public GameController()
        {
            _repository = new Repository();
            GameStart();
        }

        public void GameStart()
        {
            TheWord = getRandomWord();
            Score = 10000;
            Tries = 9;

            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            IsPlaying = true;
        }

        private WordInfo getRandomWord()
        {
            return _repository.GetWords().OrderBy(r => Guid.NewGuid()).Take(1).Single();
        }




        public WordInfo CheckWord(string word)
        {
            if(!IsPlaying) { GameStart(); }

            Tries--;

            WordInfo guessWord = GetWordInfo(word);

            if (TheWord.Word == guessWord.Word) 
            {
                GameEnd();
            }
            else
            {
                Score -= 750;
            }

            if(Tries == 0) { GameEnd(); }

            return guessWord;
        }


        public List<string> GetSuggestions(string word)
        {
            string json = ApiController.GetWord(word.ToLower()).Result;
            JsonDocument suggestionsJson = JsonDocument.Parse(json);
            List<string> suggestions = new List<string>();

            if (suggestionsJson.RootElement[0].ValueKind == JsonValueKind.String) 
            {
                suggestions = JsonSerializer.Deserialize<List<string>>(json);
            }

            return suggestions;
        }

        public WordInfo GetWordInfo(string word)
        {
            string json = ApiController.GetWord(word.ToLower()).Result;
            JsonDocument wordInfoJson = JsonDocument.Parse(json);

            WordInfo wordInfo = new WordInfo();

            if (wordInfoJson.RootElement[0].ValueKind == JsonValueKind.Object)
            {
                wordInfo.Word = wordInfoJson.RootElement[0].GetProperty("meta").GetProperty("stems")[0].GetString();
                wordInfo.Pronunciation = wordInfoJson.RootElement[0].GetProperty("hwi").GetProperty("prs")[0].GetProperty("ipa").GetString();
                wordInfo.Type = wordInfoJson.RootElement[0].GetProperty("fl").GetString();
                if (wordInfoJson.RootElement[0].GetProperty("hwi").GetProperty("prs")[0].GetProperty("sound").TryGetProperty("audio", out JsonElement audio))
                {
                    string audioStr = audio.GetString();
                    string subDirStr = audioStr[0].ToString();
                    if (!char.IsLetter(audioStr[0]))
                    {
                        subDirStr = "number";
                    }
                    else if (audioStr.Substring(0, 3) == "bix")
                    {
                        subDirStr = "bix";
                    }
                    else if (audioStr.Substring(0, 2) == "gg")
                    {
                        subDirStr = "gg";
                    }

                    wordInfo.AudioUrl = $@"https://media.merriam-webster.com/audio/prons/en/us/wav/{subDirStr}/{audioStr}.wav";
                }
                wordInfo.Description = wordInfoJson.RootElement[0].GetProperty("shortdef").EnumerateArray().First().GetString();

                if (!_repository.GetWords().ToList().Contains(wordInfo))
                {
                    _repository.AddWordInfo(wordInfo);
                }
            }
            


            return wordInfo;
        }


        public void GameEnd()
        {
            Stopwatch.Stop();
            Game game = new Game() 
            {
                Tries = Tries,
                Score = Score,
                Miliseconds = (int)Stopwatch.ElapsedMilliseconds,
                Word = TheWord,
                PlayerName = "Placeholder"
            };

            IsPlaying = false;
            _repository.AddGame(game);
        }

        
        


    }
}
