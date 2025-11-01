using System;
using System.Security.Cryptography;

namespace VLDRINKS.CORE
{
    public static class PasswordHasher
    {

        private const int SaltSize = 32;       
        private const int HashSize = 20;       
        private const int Iterations = 10000;

        public static (byte[] Hash, byte[] Salt) HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);


                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    byte[] hash = pbkdf2.GetBytes(HashSize);
                    return (hash, salt);
                }
            }
        }

        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            try
            {

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, Iterations))
                {
                    byte[] testHash = pbkdf2.GetBytes(HashSize); 


                    int diff = storedHash.Length ^ testHash.Length;
                    for (int i = 0; i < storedHash.Length && i < testHash.Length; i++)
                    {
                        diff |= storedHash[i] ^ testHash[i];
                    }
                    return diff == 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}