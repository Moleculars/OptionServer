using Bb.OptionServer;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bb
{

    public class UserEntity : OptionServer.IMapperDbDataReader
    {

        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Pseudo { get; set; }

        public string Email { get; set; }

        public string HashPassword { get; set; }

        public UserProfileEnum AccessProfile { get; set; }


        public DateTimeOffset LastUpdate { get; set; }

        public Guid SecurityCoherence { get; set; }



        public void Map(DbDataReaderContext item)
        {
            Id = item.GetGuid(nameof(Id));
            Username = item.GetString(nameof(Username));
            Pseudo = item.GetString(nameof(Pseudo));
            Email = item.GetString(nameof(Email));
            AccessProfile = (UserProfileEnum)(object)item.GetInt32(nameof(AccessProfile));
            HashPassword = item.GetString(nameof(HashPassword));
            LastUpdate = item.GetDateTime(nameof(LastUpdate));
            SecurityCoherence = item.GetGuid(nameof(SecurityCoherence));
        }

        public void GenerateSave(DbUpdateContext item)
        {
            item.CreateParameter(nameof(Id), System.Data.DbType.Guid, Id);
            item.CreateParameter(nameof(SecurityCoherence), System.Data.DbType.Guid, SecurityCoherence);
            item.CreateParameter(nameof(LastUpdate), System.Data.DbType.DateTimeOffset, LastUpdate);
            item.CreateParameter(nameof(Username), System.Data.DbType.String, Username);
            item.CreateParameter(nameof(Pseudo), System.Data.DbType.String, Pseudo);
            item.CreateParameter(nameof(Email), System.Data.DbType.String, Email);
            item.CreateParameter(nameof(HashPassword), System.Data.DbType.String, HashPassword);
            item.CreateParameter(nameof(AccessProfile), System.Data.DbType.Int32, (int)AccessProfile);
        }


        public static string Hash(string value)
        {
            return Sha256_hash(Sha256_hash(value));
        }

        private static string Sha256_hash(string value)
        {
            using (SHA256 hash = SHA256Managed.Create())
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(value)).Select(item => item.ToString("x2")));

        }


    }

    public enum UserProfileEnum
    {
        Classical = 0,
        Premium = 8,
        Operator = 256,
        Administrator = 4096 | Premium | Operator,
    }


}
