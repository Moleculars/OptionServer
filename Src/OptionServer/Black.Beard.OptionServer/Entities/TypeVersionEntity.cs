using Bb.OptionServer;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bb.OptionServer.Entities
{

    public class TypeVersionEntity : IMapperDbDataReader
    {

        public TypeVersionEntity()
        {

        }

        public Guid Id { get; set; }

        public int Version { get; set; }

        public string Contract { get; set; }
        public string Sha256 { get; set; }

        public Guid SecurityCoherence { get; set; }


        public void Map(DbDataReaderContext item)
        {
            Id = item.GetGuid("CurrentVersionId").Value;
            Version = item.GetInt32(nameof(Version)).Value;
            Contract = item.GetString(nameof(Contract));
            Sha256 = item.GetString(nameof(Sha256));
            SecurityCoherence = item.GetGuid("VersionSecurityCoherence").Value;
        }

    }

}