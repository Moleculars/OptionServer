using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bb.OptionServer
{
    public static class Sha
    {

        public static string HashPassword(string value)
        {
            return Sha256_hash(Sha256_hash(value));
        }

        public static string Sha256_hash(string value)
        {
            using (SHA256 hash = SHA256Managed.Create())
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(value)).Select(item => item.ToString("x2")));
        }

    }

}
