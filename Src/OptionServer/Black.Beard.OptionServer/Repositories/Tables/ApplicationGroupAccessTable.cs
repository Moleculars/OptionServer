using System;
using cm = System.ComponentModel.DataAnnotations;

namespace Bb.OptionServer.Repositories.Tables
{
    [cm.Schema.Table("ApplicationGroupAccess", Schema = "dbo")]
    public class ApplicationGroupAccessTable
    {

        public ApplicationGroupAccessTable()
        {
            _mapping = DtoSqlManager.GetMapping(GetType());
            ApplicationGroupId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(ApplicationGroupId)] };
            UserId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(UserId)] };
            AccessApplication = new FieldValue<int>() { Parent = this, Properties = _mapping.IndexByName[nameof(AccessApplication)] };
            AccessType = new FieldValue<int>() { Parent = this, Properties = _mapping.IndexByName[nameof(AccessType)] };
            AccessEnvironment = new FieldValue<int>() { Parent = this, Properties = _mapping.IndexByName[nameof(AccessEnvironment)] };
            LastUpdate = new FieldValue<DateTimeOffset>() { Parent = this, Properties = _mapping.IndexByName[nameof(LastUpdate)] };
            SecurityCoherence = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(SecurityCoherence)] };
        }

        [cm.Key]
        [cm.Required]
        public FieldValue<Guid> ApplicationGroupId { get; }

        [cm.Key]
        [cm.Required]
        public FieldValue<Guid> UserId { get; }


        [cm.Required]
        public FieldValue<int> AccessApplication { get; }

        [cm.Required]
        public FieldValue<int> AccessType { get; }

        [cm.Required]
        public FieldValue<int> AccessEnvironment { get; }

        [LastChangeDate]
        public FieldValue<DateTimeOffset> LastUpdate { get; }

        [SecurityCoherence]
        public FieldValue<Guid> SecurityCoherence { get; }

        private readonly ObjectMapping _mapping;

    }

}
