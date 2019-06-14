using Bb.OptionServer;
using System;

namespace Bb.Entities
{

    public class Entity : OptionServer.IMapperDbDataReader
    {

        public Guid Id { get; set; }

        public DateTimeOffset LastUpdate { get; set; }

        public Guid SecurityCoherence { get; set; }

        public virtual void GenerateSave(DbUpdateContext item)
        {
            item.CreateParameter(nameof(Id), System.Data.DbType.Guid, Id);
            item.CreateParameter(nameof(SecurityCoherence), System.Data.DbType.Guid, SecurityCoherence);
            item.CreateParameter(nameof(LastUpdate), System.Data.DbType.DateTimeOffset, LastUpdate);
        }

        public virtual void Map(DbDataReaderContext item)
        {
            Id = item.GetGuid(nameof(Id));
            LastUpdate = item.GetDateTime(nameof(LastUpdate));
            SecurityCoherence = item.GetGuid(nameof(SecurityCoherence));
        }

    }

}