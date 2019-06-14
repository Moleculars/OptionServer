using Bb.OptionServer;
using System;
using cm = System.ComponentModel.DataAnnotations;

namespace Bb.OptionServer.Repositories.Tables
{

    [cm.Schema.Table("Type", Schema = "dbo")]
    public class TypeTable : IPrimaryTable
    {

        public TypeTable(Guid id, string name, string extension, Guid groupId, Guid? currentVersionId, Guid securityCoherence) : this()
        {
            Id = id;
            Name.Value = name;
            Extension.Value = extension;
            GroupId = groupId;
            if (currentVersionId.HasValue)
                CurrentVersionId.Value = currentVersionId.Value;
            SecurityCoherence.Value = securityCoherence;
            this.Reset();
        }

        public TypeTable()
        {
            _mapping = DtoSqlManager.GetMapping(GetType());
            Id = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(Id)] };
            Name = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Name)] };
            Extension = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Extension)] };
            GroupId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(GroupId)] };
            CurrentVersionId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(CurrentVersionId)] };
            LastUpdate = new FieldValue<DateTimeOffset>() { Parent = this, Properties = _mapping.IndexByName[nameof(LastUpdate)] };
            SecurityCoherence = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(SecurityCoherence)] };
        }

        [cm.Key]
        [cm.Required]
        public FieldValue<Guid> Id { get; }

        [cm.Required]
        [cm.MaxLength(50)]
        public FieldValue<string> Name { get; }

        [cm.Required]
        [cm.MaxLength(10)]
        public FieldValue<string> Extension { get; }

        [cm.Required]
        public FieldValue<Guid> GroupId { get; }

        public FieldValue<Guid> CurrentVersionId { get; }

        [LastChangeDate]
        public FieldValue<DateTimeOffset> LastUpdate { get; }

        [SecurityCoherence]
        public FieldValue<Guid> SecurityCoherence { get; }

        private readonly ObjectMapping _mapping;

    }

}
