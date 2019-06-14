using System;
using cm = System.ComponentModel.DataAnnotations;

namespace Bb.OptionServer.Repositories.Tables
{
    [cm.Schema.Table("DocumentVersion", Schema = "dbo")]
    public class DocumentVersionTable : IPrimaryTable
    {

        public DocumentVersionTable(Guid id, string name, Guid applicatinId, Guid environmentId, Guid typeId, Guid typeVersionId, string payload, int version, string sha256, Guid securityCoherence) : this()
        {
            Id.Value = id;
            ApplicationId.Value = applicatinId;
            Name.Value = name;
            EnvironmentId.Value = environmentId;
            TypeId.Value = typeId;
            TypeVersionId.Value = typeVersionId;
            Payload.Value = payload;
            Version.Value = version;
            Sha256.Value = sha256;
            Deleted = false;
            SecurityCoherence.Value = securityCoherence;
            this.Reset();
        }

        public DocumentVersionTable()
        {
            _mapping = DtoSqlManager.GetMapping(GetType());
            Id = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(Id)] };
            ApplicationId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(ApplicationId)] };
            Name = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Name)] };
            LastUpdate = new FieldValue<DateTimeOffset>() { Parent = this, Properties = _mapping.IndexByName[nameof(LastUpdate)] };

            EnvironmentId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(EnvironmentId)] };
            TypeId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(TypeId)] };
            TypeVersionId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(TypeVersionId)] };

            Payload = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Payload)] };
            Version = new FieldValue<int>() { Parent = this, Properties = _mapping.IndexByName[nameof(Version)] };

            Sha256 = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Sha256)] };
            Deleted = new FieldValue<bool>() { Parent = this, Properties = _mapping.IndexByName[nameof(Deleted)] };
            SecurityCoherence = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(SecurityCoherence)] };

        }

        [cm.Key]
        [cm.Required]
        public FieldValue<Guid> Id { get; }

        [cm.Required]
        public FieldValue<Guid> ApplicationId { get; }

        [cm.Required]
        [cm.MaxLength(100)]
        public FieldValue<string> Name { get; }

        [LastChangeDate]
        public FieldValue<DateTimeOffset> LastUpdate { get; }

        [cm.Required]
        public FieldValue<Guid> EnvironmentId { get; }

        [cm.Required]
        public FieldValue<Guid> TypeId { get; }

        [cm.Required]
        public FieldValue<Guid> TypeVersionId { get; }

        [cm.Required]
        public FieldValue<string> Payload { get; }

        [cm.Required]
        public FieldValue<int> Version { get; }

        [cm.Required]
        public FieldValue<string> Sha256 { get; }

        [cm.Required]
        public FieldValue<bool> Deleted { get; }

        [SecurityCoherence]
        public FieldValue<Guid> SecurityCoherence { get; }

        private readonly ObjectMapping _mapping;

    }

}
