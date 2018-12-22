using SimplePassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberManagement.Models
{
    public class PasswordHashModel
    {
        public static string Hash(string password) {
            var saltedPasswordHash = new SaltedPasswordHash(password, 20);
            return saltedPasswordHash.Hash + ":" + saltedPasswordHash.Salt;
        }
        public static bool Verify(string password, string passwordHash) {
            string[] passwordHashs = passwordHash.Split(':');
            if (passwordHashs.Length != 2) return false;

            var saltedPasswordHash = new SaltedPasswordHash(passwordHashs[0], passwordHashs[1]);
            return saltedPasswordHash.Verify(password);

        }
    }
}