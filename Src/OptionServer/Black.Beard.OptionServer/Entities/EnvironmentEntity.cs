using System;

namespace Bb.OptionServer.Entities
{
    public class EnvironmentEntity
    {

        
        public Guid EnvironmentId { get; set; }

        public string EnvironmentName { get; set; }
        public GroupEntity Group { get; internal set; }
    }

}
