using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bb.OptionServer.Entities
{

    [DebuggerDisplay("{Pseudo}")]
    public class OwnerEntity
    {

        public OwnerEntity()
        {
            Groups = new Dictionary<string, GroupEntity>();
        }

        public Guid Id { get; set; }

        public string Pseudo { get; set; }

        public Dictionary<string, GroupEntity> Groups { get; }

        public GroupEntity Group(string groupName)
        {
            Groups.TryGetValue(groupName, out GroupEntity group);
            return group;
        }

    }

}
