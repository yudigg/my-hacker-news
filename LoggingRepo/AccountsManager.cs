using LoggingRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerRepo
{
    public class AccountsManager
    {
        private readonly string _connectionString;
        public AccountsManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddUser(string username, string password)
        {
            User user = new User();
            user.Username = username;
            string salt = PasswordHelper.GenerateRandomSalt();
            string hash = PasswordHelper.HashPassword(password, salt);
            user.Salt = salt;
            user.Hash = hash;
            using (var dc = new HackerNewsDataContext(_connectionString))
            {
                dc.Users.InsertOnSubmit(user);
                dc.SubmitChanges();
            }
        }
        public User GetUser(string username,string password)
        {
            using(var dc = new HackerNewsDataContext(_connectionString))
            {
              var user = dc.Users.FirstOrDefault(u => u.Username == username);
              if (user == null)
              {
                  return null;
              }
               
              bool success = PasswordHelper.PasswordMatch(password, user.Hash, user.Salt);
              return success ? user : null;
            }
        }
        public User GetUserByUserName(string username)
        {
            using (var dc = new HackerNewsDataContext(_connectionString))
            {
                var user = dc.Users.FirstOrDefault(a => a.Username == username);
                if (user == null)
                {
                    return null;
                }
                return user;
            }
        }
    }
}
