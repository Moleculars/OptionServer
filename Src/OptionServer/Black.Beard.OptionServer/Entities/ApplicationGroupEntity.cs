using Bb.OptionServer;
using System;

namespace Bb.Entities
{

    public class ApplicationGroupEntity : OptionServer.IMapperDbDataReader
    {

        public ApplicationGroupEntity()
        {
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid OwnerUserId { get; set; }
        
        public Guid SecurityCoherence { get; set; }

        public void Map(DbDataReaderContext item)
        {
            Id = item.GetGuid(nameof(Id));
            Name = item.GetString(nameof(Name));
            OwnerUserId = item.GetGuid(nameof(OwnerUserId));
            SecurityCoherence = item.GetGuid(nameof(SecurityCoherence));
        }

        public void GenerateSave(DbUpdateContext item)
        {
        }

    }

    //    public class ApplicationGroupAccessEntity : OptionServer.IMapperDbDataReader
    //{


    //    public void Map(DbDataReaderContext item)
    //    {


    //    }

    //    public void GenerateSave(DbUpdateContext item)
    //    {

    //    }

    //}

}
