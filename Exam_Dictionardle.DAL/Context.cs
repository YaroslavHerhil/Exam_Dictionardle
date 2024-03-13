using Exam_Dictionardle.DAL.Modules;
using Exam_Dictionardle.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Numerics;

namespace Exam_Dictionardle.DAL
{
    public class Context : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<WordInfo> WordsInfo { get; set; }
        public DbSet<Player> Players { get; set; }

        public Context() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Dictionardle;Integrated Security=True;Connect Timeout=30;";

                optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
