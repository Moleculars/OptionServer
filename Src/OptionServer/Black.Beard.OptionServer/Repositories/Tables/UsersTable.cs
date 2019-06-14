using System;
using cm = System.ComponentModel.DataAnnotations;

namespace Bb.OptionServer.Repositories.Tables
{
    [cm.Schema.Table("Users", Schema = "dbo")]
    public class UsersTable : IPrimaryTable
    {

        public UsersTable()
        {
            _mapping = DtoSqlManager.GetMapping(GetType());
            Id = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(Id)] };
            Username = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Username)] };
            Pseudo = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Pseudo)] };
            Email = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(Email)] };
            HashPassword = new FieldValue<string>() { Parent = this, Properties = _mapping.IndexByName[nameof(HashPassword)] };
            LastUpdate = new FieldValue<DateTimeOffset>() { Parent = this, Properties = _mapping.IndexByName[nameof(LastUpdate)] };
            SecurityCoherence = new FieldValue<Guid>() { Parent = this, Properties = _mapping.IndexByName[nameof(SecurityCoherence)] };
            AccessProfile = new FieldValue<int>() { Parent = this, Properties = _mapping.IndexByName[nameof(AccessProfile)] };
        }

        [cm.Key]
        [cm.Required]
        public FieldValue<Guid> Id { get; }

        [cm.Required]
        [cm.MaxLength(250)]
        public FieldValue<string> Username { get; }

        [cm.Required]
        [cm.MaxLength(250)]
        public FieldValue<string> Pseudo { get; }

        [cm.Required]
        [cm.MaxLength(500)]
        [cm.EmailAddress]
        public FieldValue<string> Email { get; }

        [cm.Required]
        [cm.MaxLength(250)]
        public FieldValue<string> HashPassword { get; }

        [LastChangeDate]
        public FieldValue<DateTimeOffset> LastUpdate { get; }

        [SecurityCoherence]
        public FieldValue<Guid> SecurityCoherence { get; }

        [cm.Required]
        public FieldValue<int> AccessProfile { get; }

        private readonly ObjectMapping _mapping;

    }

}
