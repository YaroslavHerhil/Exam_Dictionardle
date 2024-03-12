using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Exam_Dictionardle.Modules
{
    public class WordInfo
    {
        public string Word { get; set; }
        public string Pronunciation { get; set; }

        public string Type { get; set; }

        public string AudioUrl { get; set; }
        public string Description { get; set; }

        public WordInfo(JsonElement json)
        {
            var word_json = json[0];
            if (word_json.TryGetProperty("meta", out JsonElement meta))
            {
                Word = meta.GetProperty("stems")[0].GetString();
                Pronunciation = word_json.GetProperty("hwi").GetProperty("prs")[0].GetProperty("ipa").GetString();
                Type = word_json.GetProperty("fl").GetString();
                if (word_json.GetProperty("hwi").GetProperty("prs")[0].GetProperty("sound").TryGetProperty("audio", out JsonElement audio))
                {
                    ////https://media.merriam-webster.com/audio/prons/[language_code]/[country_code]/[format]/[subdirectory]/[base filename].[format]
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

                    AudioUrl = $@"https://media.merriam-webster.com/audio/prons/en/us/wav/{subDirStr}/{audioStr}.wav";
                }
                Description = word_json.GetProperty("shortdef").EnumerateArray().First().GetString();
            }
            else
            {

            }
        }
    }
}
