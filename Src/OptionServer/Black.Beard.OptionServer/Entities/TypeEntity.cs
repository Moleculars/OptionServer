using Bb.Entities;
using Bb.OptionServer;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bb
{

    public class TypeEntity : OptionServer.IMapperDbDataReader
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public Guid GroupId { get; set; }

        public Guid CurrentVersionId { get; set; }

        public DateTimeOffset LastUpdate { get; set; }

        public Guid SecurityCoherence { get; set; }


        public TypeVersionEntity Version { get; set; }

        public void Map(DbDataReaderContext item)
        {
            Id = item.GetGuid(nameof(Id));
            Name = item.GetString(nameof(Name));
            Extension = item.GetString(nameof(Extension));
            GroupId = item.GetGuid(nameof(GroupId));
            CurrentVersionId = item.GetGuid(nameof(CurrentVersionId));
            LastUpdate = item.GetDateTime(nameof(LastUpdate));
            SecurityCoherence = item.GetGuid(nameof(SecurityCoherence));
        }

        public void GenerateSave(DbUpdateContext item)
        {
            item.CreateParameter(nameof(Id), System.Data.DbType.Guid, Id);
            item.CreateParameter(nameof(Name), System.Data.DbType.String, Name);
            item.CreateParameter(nameof(Extension), System.Data.DbType.String, Extension);
            item.CreateParameter(nameof(CurrentVersionId), System.Data.DbType.Guid, CurrentVersionId);
            item.CreateParameter(nameof(GroupId), System.Data.DbType.Guid, GroupId);
            item.CreateParameter(nameof(SecurityCoherence), System.Data.DbType.Guid, SecurityCoherence);
        }

    }

}
