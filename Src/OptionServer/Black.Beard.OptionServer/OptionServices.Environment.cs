using Bb.OptionServer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb
{

    public partial class OptionServices
    {


        public List<EnvironmentEntity> GetEnvironments(string username, string groupName)
        {

            var group = CheckGroup(username, groupName, AccessEntityEnum.Read, objectKingEnum.Environment);

            return Environments.ReadAll(group.ApplicationGroupId).ToList();

        }

        public EnvironmentEntity AddEnvironment(string username, string groupName, string name)
        {

            var group = CheckGroup(username, groupName, AccessEntityEnum.Add, objectKingEnum.Environment);

            var env = new EnvironmentEntity()
            {
                Id = Guid.NewGuid(),
                GroupId = group.ApplicationGroupId,
                Name = name,
            };

            Environments.Save(env);

            return env;

        }

        public EnvironmentEntity Environment(Guid groupId, string name)
        {
            var environment = Environments.Read(groupId, name);
            return environment;
        }

        private EnvironmentRepository Environments => _environments ?? (_environments = new EnvironmentRepository(_manager));

        private EnvironmentRepository _environments;


    }
}
