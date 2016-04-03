﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HackerRepo
{
    public static class PasswordHelper
    {
        public static string GenerateRandomSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[10];
            provider.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static string HashPassword(string password, string salt)
        {
            SHA256Managed crypt = new SHA256Managed();

            string combinedString = password + salt;
            byte[] combined = Encoding.Unicode.GetBytes(combinedString);

            byte[] hash = crypt.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        public static bool PasswordMatch(string passwordInput, string passwordHash, string salt)
        {
            string userInputHash = HashPassword(passwordInput, salt);
            return passwordHash == userInputHash;
        }
    }
}
