using Accessibility;
using Exam_Dictionardle.DAL;
using Exam_Dictionardle.DAL.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Exam_Dictionardle
{
    public class PlayerController
    {
        private Repository _repository = new Repository();
        public Player CurrentPlayer { get; set; }

        public PlayerController() 
        {
            SetAnonymous();
        }

        public void SetAnonymous()
        {
            CurrentPlayer = _repository.GetPlayers().First(p => p.UserName == "Anonymous");
        }

        public bool Register(string userName, string login, string password)
        {
            if(userName.Length < 4) { return false; }
            if(login.Length < 8) { return false; }
            if(password.Length < 8) { return false; }
            if(_repository.GetPlayers().Any(p => p.Login == login)) {  return false; }


            byte[] salt = GenerateSalt();

            byte[] hashedPassword = HashPassword(password, salt);

            string hashedPasswordBase64 = Convert.ToBase64String(hashedPassword);



            Player newPlayer = new Player()
            {
                UserName = userName,
                Login = login,
                Password = hashedPasswordBase64
            };

            _repository.AddPlayer(newPlayer);


            CurrentPlayer = newPlayer;
            return true;
        }
        public bool Login(string login, string password)
        {
            byte[] salt = GenerateSalt();
            byte[] hashedPassword = HashPassword(password, salt);
            string hashedPasswordBase64 = Convert.ToBase64String(hashedPassword);

            Player newPlayer = _repository.GetPlayers().FirstOrDefault(p => p.Login == login && p.Password == hashedPasswordBase64);
            if(newPlayer == null)
            {
                return false;
            }
            CurrentPlayer = newPlayer;
            return true;
        }



        static byte[] GenerateSalt()
        {
            // Generate a random salt using RNGCryptoServiceProvider
            byte[] salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        static byte[] HashPassword(string password, byte[] salt)
        {
            // Combine password and salt bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

            // Compute SHA256 hash
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(combinedBytes);
            }
        }

    }
}
