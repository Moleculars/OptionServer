using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bb.OptionServer.Entities
{

    [DebuggerDisplay("{TypeName} : {Extension}")]
    public class TypeEntity : OptionServer.IMapperDbDataReader
    {

        public TypeEntity()
        {
            Group = new GroupEntity(null);
            this.Versions = new Dictionary<Guid, TypeVersionEntity>();
        }

        public Guid TypeId { get; set; }

        public string TypeName { get; set; }

        public string Extension { get; set; }

        public Guid SecurityCoherence { get; set; }

        public TypeVersionEntity CurrentVersion { get; private set; }

        public GroupEntity Group { get; internal set; }

        public Dictionary<Guid, TypeVersionEntity> Versions { get; }

        public void Map(DbDataReaderContext item)
        {

            TypeId = item.GetGuid(nameof(TypeId)).Value;
            TypeName = item.GetString(nameof(TypeName));
            Extension = item.GetString(nameof(Extension));
            SecurityCoherence = item.GetGuid("TypeSecurityCoherence").Value;

            CurrentVersion = new TypeVersionEntity();
            CurrentVersion.Map(item);
            Versions.Add(CurrentVersion.Id, CurrentVersion);

            Group.GroupId = item.GetGuid(nameof(Group.GroupId)).Value;

        }

    }

}
