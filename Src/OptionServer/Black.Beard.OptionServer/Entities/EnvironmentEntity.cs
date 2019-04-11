using Bb.OptionServer;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bb
{

    public class EnvironmentEntity : OptionServer.IMapperDbDataReader
    {


        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid GroupId { get; set; }

        public DateTimeOffset LastUpdate { get; set; }

        public Guid SecurityCoherence { get; set; }



        public void Map(DbDataReaderContext item)
        {
            Id = item.GetGuid(nameof(Id));
            Name = item.GetString(nameof(Name));
            GroupId = item.GetGuid(nameof(GroupId));
            LastUpdate = item.GetDateTime(nameof(LastUpdate));
            SecurityCoherence = item.GetGuid(nameof(SecurityCoherence));
        }

        public void GenerateSave(DbUpdateContext item)
        {
            item.CreateParameter(nameof(Id), System.Data.DbType.Guid, Id);
            item.CreateParameter(nameof(GroupId), System.Data.DbType.Guid, GroupId);
            item.CreateParameter(nameof(LastUpdate), System.Data.DbType.DateTimeOffset, LastUpdate);
            item.CreateParameter(nameof(Name), System.Data.DbType.String, Name);
            item.CreateParameter(nameof(SecurityCoherence), System.Data.DbType.Guid, SecurityCoherence);
        }

    }

}
