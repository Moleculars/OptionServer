using Bb.OptionServer;
using System;

namespace Bb
{

    public class ApplicationGroupAccessEntity : OptionServer.IMapperDbDataReader
    {

        public ApplicationGroupAccessEntity()
        {
            //UserIds = new Dictionary<Guid, ApplicationGroupAccessEntity>();
            //UserNames = new Dictionary<string, ApplicationGroupAccessEntity>();
        }

        public Guid ApplicationGroupId { get; set; }

        public string ApplicationGroupName { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; }



        public AccessEntityEnum ApplicationAccess { get; set; }

        public AccessEntityEnum EnvironmentAccess { get; set; }

        public AccessEntityEnum TypeAccess { get; set; }

        public Guid SecurityCoherence { get; set; }



        public void Map(DbDataReaderContext item)
        {

            ApplicationGroupId = item.GetGuid(nameof(ApplicationGroupId));
            ApplicationGroupName = item.GetString(nameof(ApplicationGroupName));

            UserId = item.GetGuid(nameof(UserId));
            Username = item.GetString(nameof(Username));
            ApplicationAccess = (AccessEntityEnum)item.GetInt32(nameof(ApplicationAccess));
            EnvironmentAccess = (AccessEntityEnum)item.GetInt32(nameof(EnvironmentAccess));
            TypeAccess = (AccessEntityEnum)item.GetInt32(nameof(TypeAccess));
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
