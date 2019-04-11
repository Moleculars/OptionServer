using Bb.OptionServer;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bb.Entities
{


    //                 sql = @"INSERT INTO [dbo].[TypeVersion] ([Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence]) 
    //VALUES(@id, @version, @typeId, @contract, @sha256, CURRENT_TIMESTAMP, @securityCoherence)";


    public class TypeVersionEntity : IMapperDbDataReader
    {

        public Guid Id { get; set; }

        public int Version { get; set; }

        public Guid TypeId { get; set; }

        public string Contract { get; set; }

        public string Sha256 { get; set; }

        public DateTimeOffset LastUpdate { get; set; }

        public Guid SecurityCoherence { get; set; }


        public void Map(DbDataReaderContext item)
        {
            Id = item.GetGuid(nameof(Id));
            Version = item.GetInt32(nameof(Version));
            TypeId = item.GetGuid(nameof(TypeId));
            Contract = item.GetString(nameof(Contract));
            Sha256 = item.GetString(nameof(Sha256));
            LastUpdate = item.GetDateTime(nameof(LastUpdate));
            SecurityCoherence = item.GetGuid(nameof(SecurityCoherence));
        }

        public void GenerateSave(DbUpdateContext item)
        {
            item.CreateParameter(nameof(Id), System.Data.DbType.Guid, Id);
            item.CreateParameter(nameof(Version), System.Data.DbType.Int32, Version);
            item.CreateParameter(nameof(TypeId), System.Data.DbType.Guid, TypeId);
            item.CreateParameter(nameof(Contract), System.Data.DbType.String, Contract);
            item.CreateParameter(nameof(Sha256), System.Data.DbType.String, Sha256);
            item.CreateParameter(nameof(SecurityCoherence), System.Data.DbType.Guid, SecurityCoherence);
        }

        internal static string Sha256_hash(string value)
        {
            using (SHA256 hash = SHA256Managed.Create())
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(value)).Select(item => item.ToString("x2")));
        }


    }

}