using Exam_Dictionardle.DAL;
using Exam_Dictionardle.Modules;
using Microsoft.EntityFrameworkCore;
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
        public PlayerController PlayerController;

        public int Score { get; set; }
        public int Tries { get; set; }
        public Stopwatch Stopwatch { get; set; }
        public WordInfo TheWord { get; set; }

        public bool IsPlaying { get; set; }

        private Repository _repository;
        

        public GameController()
        {
            PlayerController = new PlayerController();
            _repository = new Repository();
            if (!_repository.GetWords().Any())
            {
                loadWords();
            }



            GameStart();
        }




        public void GameStart()
        {
            TheWord = getRandomWord();
            Score = 9000;
            Tries = 9;

            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            IsPlaying = true;
        }

        private WordInfo getRandomWord()
        {
            return _repository.GetWords().OrderBy(r => Guid.NewGuid()).Take(1).Single();
        }


        private void loadWords()
        {
            List<string> randomWords = new List<string>
            {
                "Dog", "Cat", "Book", "Tree", "House", "Car", "Sun", "Moon", "Rain", "Snow",
                "Chair", "Table", "Pen", "Phone", "Computer", "Door", "Window", "Flower", "Bird",
                "Fish", "Rabbit", "Horse", "Bike", "Ocean", "River", "Mountain", "Desert", "Forest",
                "Turtle", "Elephant", "Giraffe", "Lion", "Tiger", "Leopard", "Zebra", "Kangaroo",
                "Octopus", "Dolphin", "Penguin", "Eagle", "Butterfly", "Dragonfly", "Bee", "Ant",
                "Spider", "Snake", "Lizard", "Squirrel", "Beaver", "Koala", "Ostrich", "Raccoon",
                "Otter", "Seal", "Polar bear", "Grizzly bear", "Panda", "Chimpanzee", "Gorilla",
                "Wolverine", "Jackal", "Coyote", "Lynx", "Cougar", "Cheetah", "Hyena", "Bison",
                "Elk", "Moose", "Manatee", "Beluga", "Orca", "Whale", "Shark", "Octopus",
                "Squid", "Starfish", "Crab", "Lobster", "Shrimp", "Oyster", "Clam", "Snail",
                "Butterfly", "Dragonfly", "Ladybug", "Beetle", "Caterpillar", "Mosquito",
                "Frog", "Toad", "Salamander", "Newt", "Turtle", "Tortoise", "Crocodile",
                "Alligator", "Iguana", "Gecko", "Chameleon", "Komodo dragon"
            };

            foreach(string word in randomWords)
            {
                WordInfo newWord = getWordInfo(word);
                _repository.AddWordInfo(newWord);
            }
        }

        public WordInfo CheckWord(string word)
        {
            if(!IsPlaying) { GameStart(); }

            Tries--;

            WordInfo guessWord = getWordInfo(word);

            if (TheWord.Word == guessWord.Word) 
            {
                GameEnd();
            }
            else
            {
                Score -= 900;
            }

            if(Tries <= 0) { GameEnd(); }

            return guessWord;
        }


        public List<string> GetSuggestions(string word)
        {
            try
            {

                string json = ApiController.GetWord(word.ToLower()).Result;
                JsonDocument suggestionsJson = JsonDocument.Parse(json);
                List<string> suggestions = new List<string>();

                if (suggestionsJson.RootElement.GetArrayLength() > 0 && suggestionsJson.RootElement[0].ValueKind == JsonValueKind.String) 
                {
                    suggestions = JsonSerializer.Deserialize<List<string>>(json);
                }

                return suggestions;
            }
            catch
            {
                return new List<string> { };
            }
        }

        public string GetTimeString()
        {
            return Stopwatch.Elapsed.ToString(@"mm\:ss");
        }

        private WordInfo getWordInfo(string word)
        {
            try
            {
                string json = ApiController.GetWord(word.ToLower()).Result;
                JsonDocument wordInfoJson = JsonDocument.Parse(json);

                WordInfo wordInfo = new WordInfo();

                if (wordInfoJson.RootElement.GetArrayLength() > 0 && wordInfoJson.RootElement[0].ValueKind == JsonValueKind.Object)
                {
                    wordInfo.Word = wordInfoJson.RootElement[0].GetProperty("meta").GetProperty("stems")[0].GetString();
                    if (wordInfoJson.RootElement[0].GetProperty("hwi").TryGetProperty("prs", out JsonElement pronc))
                    {
                        wordInfo.Pronunciation = pronc[0].GetProperty("ipa").GetString();



                        if (pronc[0].TryGetProperty("sound", out JsonElement audio))
                        {
                            string audioStr = audio.GetProperty("audio").GetString();
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
                        else
                        {
                            wordInfo.AudioUrl = "-";
                        }

                    }
                    else
                    {
                        wordInfo.AudioUrl = "-";
                        wordInfo.Pronunciation = "-";
                    }
                    wordInfo.Type = wordInfoJson.RootElement[0].GetProperty("fl").GetString();

                    wordInfo.Description = wordInfoJson.RootElement[0].GetProperty("shortdef").EnumerateArray().First().GetString();

                    if (!_repository.GetWords().ToList().Contains(wordInfo))
                    {
                        _repository.AddWordInfo(wordInfo);
                    }
                }


                return wordInfo;
            }
            catch { return new WordInfo(); }

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
                Player = PlayerController.CurrentPlayer
            };

            IsPlaying = false;
            Game newGame = game;
            if (_repository.GetGames().Any(g => g.Player.Login == game.Player.Login))
            {
                newGame.Player = _repository.GetGames().First(g => g.Player.Login == game.Player.Login).Player;
            }
            _repository.AddGame(newGame);
        }


        public List<Game> GetHighscores()
        {
            return _repository.GetGames().Include(g => g.Player).Include(g => g.Word).OrderByDescending(g => g.Score).ToList();
        }
        public List<Game> GetGamesByThisPlayer()
        {
            return _repository.GetGames().Include(g => g.Player).Include(g => g.Word).Where(g => g.Player.Login == PlayerController.CurrentPlayer.Login).OrderByDescending(g => g.Score).ToList();
        }




    }
}
