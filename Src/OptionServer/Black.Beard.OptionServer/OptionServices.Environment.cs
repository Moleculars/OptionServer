using Bb.OptionServer;
using Bb.OptionServer.Entities;
using Bb.OptionServer.Repositories.Tables;
using System;
using System.Linq;

namespace Bb.OptionServer
{

    public partial class OptionServices
    {


        public GroupEntity LoadEnvironments(UserEntity user, string groupName)
        {

            var path = user.ResolveGroup(groupName);
            var group = user.CheckGroup(path, AccessEntityEnum.Reader, objectKingEnum.Environment);

            var i = Environments.ReadAllForGroup(group.GroupId).ToList();

            path.Group.Infos.AddEnvironments(i.Select(c => new EnvironmentEntity()
            {
                Group = path.Group.Infos,
                EnvironmentId = c.Id,
                EnvironmentName = c.Name,
            }).ToArray());

            return group;

        }

        public EnvironmentEntity AddEnvironment(UserEntity user, string groupName, string name)
        {

            var path = user.ResolveGroup(groupName);
            var group = user.CheckGroup(path, AccessEntityEnum.Operator, objectKingEnum.Environment);

            var env = new EnvironmentTable();
            env.Id.Value = Guid.NewGuid();
            env.GroupId.Value = path.Group.Infos.GroupId;
            env.Name.Value = name;

            Environments.Insert(env);

            var e = new EnvironmentEntity()
            {
                Group = group,
                EnvironmentId = env.Id,
                EnvironmentName = env.Name,
            };

            path.Group.Infos.AddEnvironments(e);

            return e;

        }

        public EnvironmentTable Environment(Guid groupId, string name)
        {
            var environment = Environments.Read(groupId, name);
            return environment;
        }

    }
}
