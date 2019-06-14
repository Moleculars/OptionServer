using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.OptionServer.Entities
{

    public class UserEntity
    {
        private List<GroupEntity> _groups;
        private List<ApplicationEntity> _applications;
        private ILookup<string, GroupEntity> _groupLookupByName;
        private ILookup<string, ApplicationEntity> _applicationLookupByName;
        private Dictionary<Guid, GroupEntity> _groupLookupById;

        public UserEntity()
        {
            OwnerAccess = new Dictionary<string, OwnerEntity>();
            _groups = new List<GroupEntity>();
            _applications = new List<ApplicationEntity>();

        }

        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Pseudo { get; set; }

        public string Email { get; set; }

        public string HashPassword { get; set; }

        public UserProfileEnum AccessProfile { get; set; }

        public DateTimeOffset LastUpdate { get; set; }

        public Guid SecurityCoherence { get; set; }


        public Dictionary<string, OwnerEntity> OwnerAccess { get; set; }

        internal void Append(GroupEntity group)
        {
            _groups.Add(group);
        }

        internal void Append(ApplicationEntity application)
        {
            _applications.Add(application);
        }

        public List<GroupEntity> Groups()
        {

            return _groups;

        }

        public IEnumerable<ApplicationEntity> Applications(string groupName, string applicationName)
        {

            if (_groupLookupByName == null)
                _groupLookupByName = _groups.ToLookup(c => c.GroupName);

            var groups = _groupLookupByName[groupName];

            foreach (GroupEntity group in groups)
                if (group.Applications.TryGetValue(applicationName, out ApplicationEntity application))
                    yield return application;

        }

        public List<GroupEntity> Groups(string groupName)
        {

            if (_groupLookupByName == null)
                _groupLookupByName = _groups.ToLookup(c => c.GroupName);

            return _groupLookupByName[groupName].ToList();

        }

        public GroupEntity Group(Guid id)
        {

            if (_groupLookupById == null)
                _groupLookupById = _groups.ToDictionary(c => c.GroupId);

            return _groupLookupById[id];

        }


        public OptionPath ResolveGroup(string fullname)
        {
            return OptionPath
                .GetGroup(fullname)
                .Map(this);
        }

        public OptionPath ResolveApplication(string fullname)
        {
            return OptionPath
                .GetApplication(fullname)
                .Map(this);
        }

        public List<ApplicationEntity> Applications(string applicationName)
        {

            if (_applicationLookupByName == null)
                _applicationLookupByName = _applications.ToLookup(c => c.Name);

            return _applicationLookupByName[applicationName].ToList();

        }

        internal GroupEntity CheckGroup(OptionPath path, AccessEntityEnum read, objectKingEnum environment)
        {
            return CheckGroup(path.Group?.Infos, read, environment);
        }

        public GroupEntity CheckGroup(GroupEntity group, AccessEntityEnum access, objectKingEnum objectKind)
        {

            if (group == null)
                throw new Exceptions.NotEnoughtRightException("groupName");

            AccessEntityEnum _access = AccessEntityEnum.None;

            switch (objectKind)
            {

                case objectKingEnum.Environment:
                    _access = group.EnvironmentAccess;
                    break;

                case objectKingEnum.Type:
                    _access = group.TypeAccess;
                    break;

                case objectKingEnum.Application:
                    _access = group.ApplicationAccess;
                    break;

                default:
                    _access = AccessEntityEnum.None;
                    break;

            }

            if (!_access.CanDoIt(access))
                throw new Exceptions.NotEnoughtRightException($"{nameof(Username)} have not enought right for {access} {objectKind}");

            return group;

        }

        //public void Map(DbDataReaderContext item)
        //{
        //    Id = item.GetGuid(nameof(Id));
        //    Username = item.GetString(nameof(Username));
        //    Pseudo = item.GetString(nameof(Pseudo));
        //    Email = item.GetString(nameof(Email));
        //    AccessProfile = (UserProfileEnum)(object)item.GetInt32(nameof(AccessProfile));
        //    HashPassword = item.GetString(nameof(HashPassword));
        //    LastUpdate = item.GetDateTime(nameof(LastUpdate));
        //    SecurityCoherence = item.GetGuid(nameof(SecurityCoherence));
        //}

    }

}
