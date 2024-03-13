using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Exam_Dictionardle.Modules
{
    public class WordInfo
    {
        public int ID {  get; set; }
        public string Word { get; set; }
        public string Pronunciation { get; set; }

        public string Type { get; set; }

        public string AudioUrl { get; set; }
        public string Description { get; set; }

        public WordInfo()
        {

        }

        
    }
}
