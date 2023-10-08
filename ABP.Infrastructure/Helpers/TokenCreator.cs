using System;
using System.Security.Cryptography;

namespace ABP.Infrastructure.Helpers
{
    public static class TokenCreator
    {
        public static string HashPassword(string key)
        {
            if (key != null)
            {
                var bytes = new Rfc2898DeriveBytes(key, 0x10, 0x3e8);

                var salt = bytes.Salt;
                var buffer = bytes.GetBytes(0x20);

                var dst = new byte[0x31];

                Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
                Buffer.BlockCopy(buffer, 0, dst, 0x11, 0x20);

                return Convert.ToBase64String(dst);
            }
            return null;
        }
        public static bool VerifyHashedPassword(string hashedKey, string key)
        {
            if (key != null && hashedKey != null)
            {
                var src = Convert.FromBase64String(hashedKey);

                if ((src.Length != 0x31) || (src[0] != 0))
                {
                    return false;
                }

                var dst = new byte[0x10];

                var buffer = new byte[0x20];

                Buffer.BlockCopy(src, 1, dst, 0, 0x10);
                Buffer.BlockCopy(src, 0x11, buffer, 0, 0x20);

                var bytes = new Rfc2898DeriveBytes(key, dst, 0x3e8);

                var buffer2 = bytes.GetBytes(0x20);

                return AreHashesEqual(buffer, buffer2);
            }

            return false;
        }

        private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            var _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;

            var xor = firstHash.Length ^ secondHash.Length;

            for (int i = 0; i < _minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];

            return 0 == xor;
        }
    }
}
