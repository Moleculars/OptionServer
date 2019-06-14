using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bb.OptionServer.Entities
{

    [DebuggerDisplay("{GroupName}")]
    public class GroupEntity
    {

        public GroupEntity(OwnerEntity owner)
        {
            Owner = owner;
            _environmentsById = new Dictionary<Guid, EnvironmentEntity>();
            _environmentsByName = new Dictionary<string, EnvironmentEntity>();
            Applications = new Dictionary<string, ApplicationEntity>();
            _typesByName = new Dictionary<string, TypeEntity>();
            _typesById = new Dictionary<Guid, TypeEntity>();
        }

        public Guid GroupId { get; set; }

        public string GroupName { get; set; }

        public AccessEntityEnum ApplicationAccess { get; internal set; }

        public AccessEntityEnum EnvironmentAccess { get; internal set; }

        public AccessEntityEnum TypeAccess { get; internal set; }

        public FieldValue<Guid> SecurityCoherence { get; internal set; }

        public OwnerEntity Owner { get; }

        public Dictionary<string, ApplicationEntity> Applications { get; }

        public bool IsOwner { get; internal set; }

        internal void AddEnvironments(params EnvironmentEntity[] envs)
        {
            foreach (var env in envs)
            {
                _environmentsById.Add(env.EnvironmentId, env);
                _environmentsByName.Add(env.EnvironmentName, env);
            }
        }

        public IEnumerable<EnvironmentEntity> GetEnvironments()
        {
            return _environmentsByName.Values;
        }

        public EnvironmentEntity GetEnvironment(string name)
        {
            _environmentsByName.TryGetValue(name, out EnvironmentEntity env);
            return env;
        }

        public EnvironmentEntity GetEnvironment(Guid id)
        {
            _environmentsById.TryGetValue(id, out EnvironmentEntity env);
            return env;
        }

        public bool TypesAreLoaded => _typesById.Count > 0;

        public void AddType(TypeEntity type)
        {
            _typesById.Add(type.TypeId, type);
            _typesByName.Add(type.TypeName, type);
        }

        public IEnumerable<TypeEntity> GetTypes()
        {
            return _typesByName.Values;
        }

        public TypeEntity GetType(string nameType)
        {
            _typesByName.TryGetValue(nameType, out TypeEntity type);
            return type;
        }

        public TypeEntity GetType(Guid id)
        {
            _typesById.TryGetValue(id, out TypeEntity type);
            return type;
        }

        internal TypeEntity TypeByExtension(string extension)
        {
            return _typesByName.Values.FirstOrDefault(c => c.Extension == extension);
        }

        internal ApplicationEntity Application(string name)
        {
            Applications.TryGetValue(name, out ApplicationEntity application);
            return application;
        }

        private readonly Dictionary<Guid, EnvironmentEntity> _environmentsById;
        private readonly Dictionary<string, EnvironmentEntity> _environmentsByName;

        private Dictionary<Guid, TypeEntity> _typesById { get; }
        private Dictionary<string, TypeEntity> _typesByName { get; }

        public string FullName => string.Concat(Owner.Pseudo, Constants.SeparatorPath, GroupName);

    }

}
