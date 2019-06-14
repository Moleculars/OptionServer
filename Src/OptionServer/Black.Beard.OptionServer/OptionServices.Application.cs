using Bb.OptionServer.Entities;
using Bb.OptionServer.Repositories.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.OptionServer
{

    public partial class OptionServices
    {

        public ApplicationEntity AddApplication(UserEntity user, string groupName, string name)
        {

            var path = user.ResolveGroup(groupName);
            var group = user.CheckGroup(path, AccessEntityEnum.Manager, objectKingEnum.Application);

            if (group.Applications.ContainsKey(name))
                throw new Exceptions.AllreadyExistException(name);

            ApplicationTable application = new ApplicationTable(Guid.NewGuid(), name, group.GroupId, Guid.NewGuid());

            var a = Applications.Insert(application);

            var e = new ApplicationEntity()
            {
                Id = application.Id.Value,
                Name = application.Name.Value,
                SecurityCoherence = application.SecurityCoherence.Value,
                Group = group,
            };
            group.Applications.Add(e.Name, e);
            user.Append(e);

            return e;

        }

        public ApplicationEntity ApplicationByName(UserEntity user, string groupName, string name)
        {

            var path = user.ResolveGroup(groupName);
            var group = user.CheckGroup(path, AccessEntityEnum.Manager, objectKingEnum.Application);

            if (!group.Applications.TryGetValue(name, out ApplicationEntity appli))
            {

                Applications.GetForGroup(group.GroupId);

                if (!group.Applications.TryGetValue(name, out appli))
                    throw new Exceptions.AllreadyExistException(name);

            }

            return appli;

        }

        public List<ApplicationEntity> GetApplications(UserEntity user, string groupName)
        {
            var path = user.ResolveGroup(groupName);
            var group = user.CheckGroup(path, AccessEntityEnum.Reader, objectKingEnum.Application);
            var items = Applications.GetForGroup(group.GroupId);

            var result = items.Select(c => 
                new ApplicationEntity()
                {
                    Id = c.Id.Value,
                    Name = c.Name.Value,
                    SecurityCoherence = c.SecurityCoherence.Value,
                    Group = group,
                }
                ).ToList();

            return result;
        }

    }
}
