using System;
using cm = System.ComponentModel.DataAnnotations;

namespace Bb.OptionServer.Repositories.Tables
{
    [cm.Schema.Table("Application", Schema = "dbo")]
    public class ApplicationTable : IPrimaryTable
    {


        public ApplicationTable(Guid id, string name, Guid groupId, Guid? securityCoherence = null) : this()
        {
            Id.Value = id;
            Name = name;
            GroupId.Value = groupId;
            if (securityCoherence.HasValue && securityCoherence != Guid.Empty)
                SecurityCoherence.Value = securityCoherence.Value;
            this.Reset();
        }

        public ApplicationTable()
        {
            _mapping = DtoSqlManager.GetMapping(GetType());
            Id = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(Id)] };
            LastUpdate = new FieldValue<DateTimeOffset>() { Parent = this, Properties = _mapping.IndexByName[nameof(LastUpdate)] };
            Name = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Name)] };
            GroupId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(GroupId)] };
            SecurityCoherence = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(SecurityCoherence)] };
        }


        [cm.Key]
        [cm.Required]
        public FieldValue<Guid> Id { get; }

        [LastChangeDate]
        public FieldValue<DateTimeOffset> LastUpdate { get; }

        [cm.Required]
        [cm.MaxLength(100)]
        public FieldValue<string> Name { get; }

        [cm.Required]
        public FieldValue<Guid> GroupId { get; }

        [SecurityCoherence]
        public FieldValue<Guid> SecurityCoherence { get; }

        private ObjectMapping _mapping;

    }

}
