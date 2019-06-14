using Bb.OptionServer;
using System;

namespace Bb.OptionServer.Entities
{

    public class ApplicationUserGroupEntity : OptionServer.IMapperDbDataReader
    {

        public ApplicationUserGroupEntity()
        {

        }


        public Guid? Id { get; set; }
        public string Name { get; set; }



        public Guid? OwnerUserId { get; set; }

        public string OwnerPseudo { get; set; }



        public AccessEntityEnum ApplicationAccess { get; set; }

        public AccessEntityEnum EnvironmentAccess { get; set; }

        public AccessEntityEnum TypeAccess { get; set; }



        public Guid? SecurityCoherence { get; set; }


        public Guid? ApplicationId { get; set; }
        public string ApplicationName { get; set; }

        public Guid? SecurityCoherenceApplication { get; set; }

        public void Map(DbDataReaderContext item)
        {

            Id = item.GetGuid(nameof(Id));
            Name = item.GetString(nameof(Name));

            OwnerUserId = item.GetGuid(nameof(OwnerUserId));
            OwnerPseudo = item.GetString(nameof(OwnerPseudo));

            ApplicationAccess = (AccessEntityEnum)item.GetInt32(nameof(ApplicationAccess));
            EnvironmentAccess = (AccessEntityEnum)item.GetInt32(nameof(EnvironmentAccess));
            TypeAccess = (AccessEntityEnum)item.GetInt32(nameof(TypeAccess));

            SecurityCoherence = item.GetGuid(nameof(SecurityCoherence));

            ApplicationId = item.GetGuid(nameof(ApplicationId));
            ApplicationName = item.GetString(nameof(ApplicationName));
            
            SecurityCoherenceApplication = item.GetGuid(nameof(SecurityCoherenceApplication));

        }


    }


    public class ApplicationGroupAccessEntity : OptionServer.IMapperDbDataReader
    {

        public ApplicationGroupAccessEntity()
        {
        }


        public Guid? Id { get; set; }
        public string Name { get; set; }

        public Guid? OwnerUserId { get; set; }
        public string OwnerPseudo { get; set; }


        public Guid? UserId { get; set; }
        public string Pseudo { get; set; }

        public AccessEntityEnum ApplicationAccess { get; set; }

        public AccessEntityEnum EnvironmentAccess { get; set; }

        public AccessEntityEnum TypeAccess { get; set; }


        public Guid? SecurityCoherence { get; set; }

        public void Map(DbDataReaderContext item)
        {

            Id = item.GetGuid(nameof(Id));
            Name = item.GetString(nameof(Name));

            OwnerUserId = item.GetGuid(nameof(OwnerUserId));
            OwnerPseudo = item.GetString(nameof(OwnerPseudo));

            UserId = item.GetGuid(nameof(UserId));
            Pseudo = item.GetString(nameof(Pseudo));

            ApplicationAccess = (AccessEntityEnum)item.GetInt32(nameof(ApplicationAccess)).Value;
            EnvironmentAccess = (AccessEntityEnum)item.GetInt32(nameof(EnvironmentAccess)).Value;
            TypeAccess = (AccessEntityEnum)item.GetInt32(nameof(TypeAccess)).Value;

            SecurityCoherence = item.GetGuid(nameof(SecurityCoherence));

        }

    }

}
