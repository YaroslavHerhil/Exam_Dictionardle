using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Dictionardle.Modules
{
    public class Game
    {
        public int ID {  get; set; }
        public int Tries {  get; set; }
        public int Score { get; set; }

        public int Miliseconds { get; set; }
        public WordInfo Word { get; set; }

        public string PlayerName { get; set; }

    }
}
