//using Bb.OptionServer;
//using System;

//namespace Bb.Entities
//{
//    public class ApplicationConfigurationAccessEntity : OptionServer.IMapperDbDataReader
//    {

//        public ApplicationConfigurationAccessEntity()
//        {

//        }

//        public Guid UserId { get; set; }
//        public string Username { get; set; }


//        public Guid GroupId { get; set; }
//        public string GroupName { get; set; }


//        public Guid ApplicationId { get; set; }
//        public string ApplicationNme { get; set; }


//        public Guid SecurityCoherence { get; set; }
//        public int AccessApplication { get; set; }

//        public void Map(DbDataReaderContext item)
//        {

//            UserId = item.GetGuid(nameof(UserId));
//            Username = item.GetString(nameof(Username));

//            GroupId = item.GetGuid(nameof(GroupId));
//            GroupName = item.GetString(nameof(GroupName));

//            AccessApplication = item.GetInt32(nameof(AccessApplication));
//            SecurityCoherence = item.GetGuid(nameof(SecurityCoherence));

//        }

//        public void GenerateSave(DbUpdateContext item)
//        {
//        }


//    }

//}
