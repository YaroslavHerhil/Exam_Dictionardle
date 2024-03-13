using Exam_Dictionardle.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Formats.Tar;
using Exam_Dictionardle.DAL.Modules;

namespace Exam_Dictionardle.DAL
{
    public class Repository
    {
        private Context _context = new Context();

        public Repository() { }

        public void AddGame(Game game)
        {
            _context.Players.Attach(game.Player);
            _context.Games.Add(game);
            _context.SaveChanges();
        }
        public void AddWordInfo(WordInfo word)
        {
            _context.WordsInfo.Add(word);
            _context.SaveChanges();
        }
        public void AddPlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        public DbSet<WordInfo> GetWords()
        {
            return _context.WordsInfo;
        }
        public DbSet<Player> GetPlayers()
        {
            return _context.Players;
        }
        public DbSet<Game> GetGame()
        {
            return _context.Games;
        }


    }
}
