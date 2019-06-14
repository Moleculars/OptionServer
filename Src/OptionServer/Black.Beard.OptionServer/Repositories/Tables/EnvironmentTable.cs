using System;
using cm = System.ComponentModel.DataAnnotations;

namespace Bb.OptionServer.Repositories.Tables
{
    [cm.Schema.Table("Environment", Schema = "dbo")]
    public class EnvironmentTable : IPrimaryTable
    {

        public EnvironmentTable()
        {
            _mapping = DtoSqlManager.GetMapping(GetType());
            Id = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(Id)] };
            Name = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Name)] };
            GroupId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(GroupId)] };
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
        public FieldValue<Guid> GroupId { get; }

        [LastChangeDate]
        public FieldValue<DateTimeOffset> LastUpdate { get; }

        [SecurityCoherence]
        public FieldValue<Guid> SecurityCoherence { get; }

        private readonly ObjectMapping _mapping;

    }

}
