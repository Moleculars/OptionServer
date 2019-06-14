using System;
using cm = System.ComponentModel.DataAnnotations;

namespace Bb.OptionServer.Repositories.Tables
{
    [cm.Schema.Table("ApplicationGroup", Schema = "dbo")]
    public class ApplicationGroupTable : IPrimaryTable
    {

        public ApplicationGroupTable()
        {
            _mapping = DtoSqlManager.GetMapping(GetType());
            Id = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(Id)] };
            Name = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Name)] };
            OwnerUserId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(OwnerUserId)] };
            LastUpdate = new FieldValue<DateTimeOffset>() { Parent = this, Properties = _mapping.IndexByName[nameof(LastUpdate)] };
            SecurityCoherence = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(SecurityCoherence)] };
        }

        [cm.Key]
        [cm.Required]
        public FieldValue<Guid> Id { get; }

        [cm.Required]
        [cm.MaxLength(100)]
        public FieldValue<string> Name { get; }

        [cm.Required]
        public FieldValue<Guid> OwnerUserId { get; }


        [LastChangeDate]
        public FieldValue<DateTimeOffset> LastUpdate { get; }

        [SecurityCoherence]
        public FieldValue<Guid> SecurityCoherence { get; }

        private readonly ObjectMapping _mapping;

    }

}
