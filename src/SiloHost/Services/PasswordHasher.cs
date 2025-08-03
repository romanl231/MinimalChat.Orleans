﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SiloHost.Services
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password, 
                salt, 
                Iterations, 
                HashAlgorithmName.SHA256,
                HashSize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string password, string storedHash)
        {
            var parts = storedHash.Split('.', 3);
            if (parts.Length != 3) return false;

            if (!int.TryParse(parts[0], out int iterations)) return false;
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] hash = Convert.FromBase64String(parts[2]);

            byte[] attempted = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt, 
                iterations, 
                HashAlgorithmName.SHA256, 
                hash.Length);

            return CryptographicOperations.FixedTimeEquals(attempted, hash);
        }
    }
}
