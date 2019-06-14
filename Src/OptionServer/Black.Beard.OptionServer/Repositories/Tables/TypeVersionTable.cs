using System;
using cm = System.ComponentModel.DataAnnotations;

namespace Bb.OptionServer.Repositories.Tables
{
    [cm.Schema.Table("TypeVersion", Schema = "dbo")]
    public class TypeVersionTable : IPrimaryTable
    {

        public TypeVersionTable(Guid id, int version, Guid typeId, string contract, string sha256, Guid securityCoherence) : this()
        {
            Id.Value = id;
            Version.Value = version;
            TypeId.Value = typeId;
            Contract.Value = contract;
            Sha256.Value = sha256;
            SecurityCoherence.Value = securityCoherence;
            this.Reset();
        }

        public TypeVersionTable()
        {
            _mapping = DtoSqlManager.GetMapping(GetType());
            Id = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(Id)] };
            LastUpdate = new FieldValue<DateTimeOffset>() { Parent = this, Properties = _mapping.IndexByName[nameof(LastUpdate)] };
            Version = new FieldValue<int>() { Parent = this, Properties = _mapping.IndexByName[nameof(Version)] };
            TypeId = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(TypeId)] };
            Contract = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Contract)] };
            Sha256 = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Sha256)] };
            SecurityCoherence = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(SecurityCoherence)] };
        }

        [cm.Key]
        [cm.Required]
        public FieldValue<Guid> Id { get; }

        [LastChangeDate]
        public FieldValue<DateTimeOffset> LastUpdate { get; }

        [cm.Required]
        public FieldValue<int> Version { get; }

        [cm.Required]
        public FieldValue<Guid> TypeId { get; }

        [cm.Required]
        public FieldValue<string> Contract { get; }

        [cm.Required]
        [cm.MaxLength(70)]
        public FieldValue<string> Sha256 { get; }

        [SecurityCoherence]
        public FieldValue<Guid> SecurityCoherence { get; }

        private readonly ObjectMapping _mapping;

    }

}
