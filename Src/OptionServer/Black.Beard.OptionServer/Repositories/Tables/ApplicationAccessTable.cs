using System;
using cm = System.ComponentModel.DataAnnotations;

namespace Bb.OptionServer.Repositories.Tables
{
    [cm.Schema.Table("ApplicationAccess", Schema = "dbo")]
    public class ApplicationAccessTable
    {

        public ApplicationAccessTable(Guid applicationId, Guid userId, int accessApplication, Guid? securityCoherence) : this()
        {
            ApplicationId.Value = applicationId;
            UserId.Value = userId;
            AccessApplication.Value = accessApplication;
            if (securityCoherence.HasValue && securityCoherence.Value != Guid.Empty)
                SecurityCoherence.Value = securityCoherence.Value;
        }

        public ApplicationAccessTable()
        {
            _mapping = DtoSqlManager.GetMapping(GetType());
            ApplicationId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(ApplicationId)] };
            UserId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(UserId)] };
            AccessApplication = new FieldValue<int>() { Parent = this, Properties = _mapping.IndexByName[nameof(AccessApplication)] };
            LastUpdate = new FieldValue<DateTimeOffset>() { Parent = this, Properties = _mapping.IndexByName[nameof(LastUpdate)] };
            SecurityCoherence = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(SecurityCoherence)] };
        }

        [cm.Key]
        [cm.Required]
        public FieldValue<Guid> ApplicationId { get; }

        [cm.Key]
        [cm.Required]
        public FieldValue<Guid> UserId { get; }

        [cm.Required]
        public FieldValue<int> AccessApplication { get; }

        [LastChangeDate]
        public FieldValue<DateTimeOffset> LastUpdate { get; }

        [SecurityCoherence]
        public FieldValue<Guid> SecurityCoherence { get; }

        private ObjectMapping _mapping;

    }

}
