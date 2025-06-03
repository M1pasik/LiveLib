using System;
using System.Security.Cryptography;
using LiveLib.Application.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LiveLib.PasswordHasher
{
    public class PasswordHasher : IPassowrdHasher
    {
        private const int Iterations = 10000;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const char Delimiter = ':';

        public string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            byte[] hash = GenerateByteHash(salt, password);

            return $"{Convert.ToBase64String(salt)}{Delimiter}{Convert.ToBase64String(hash)}";
        }

        public bool Verify(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            string[] parts = hashedPassword.Split(Delimiter);
            if (parts.Length != 2)
                return false;

            try
            {
                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] hash = Convert.FromBase64String(parts[1]);

                if (salt.Length != SaltSize || hash.Length != HashSize)
                    return false;

                byte[] newHash = GenerateByteHash(salt, password);
                return CryptographicOperations.FixedTimeEquals(hash, newHash);
            }
            catch
            {
                return false;
            }
        }

        private byte[] GenerateByteHash(byte[] salt, string password)
        {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize);
        }
    }
}