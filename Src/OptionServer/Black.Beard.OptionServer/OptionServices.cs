using Bb.OptionServer;
using Bb.OptionServer.Repositories;
using System;
using System.Linq;

namespace Bb.OptionServer
{

    public partial class OptionServices
    {

        public OptionServices(DtoSqlManager dto)
        {

            _dto = dto;
            //_manager = manager;
        }



        ///// <summary>
        ///// Groups the applications.
        ///// </summary>
        ///// <param name="username">The username.</param>
        ///// <param name="groupName">Name of the group.</param>
        ///// <returns></returns>
        //public ApplicationAccessEntity AccessApplication(string username, string groupName)
        //{

        //    if (!groupName.StartsWith(username + "."))
        //        groupName = $"{username}.{groupName}";

        //    var user = Users.Read(username) ?? throw new MissingUserException(username);
        //    var result = Applications.GetApplicationAccesses(user.Id, groupName);

        //    return result.FirstOrDefault();

        //}


        private UserRepository Users => _users ?? (_users = new UserRepository(_dto));
        private ApplicationRepository Applications => _applications ?? (_applications = new ApplicationRepository(_dto));
        private ApplicationGroupRepository ApplicationGroups => _applicationGroups ?? (_applicationGroups = new ApplicationGroupRepository(_dto));
        private EnvironmentRepository Environments => _environments ?? (_environments = new EnvironmentRepository(_dto));
        private TypeRepository Types => _types ?? (_types = new TypeRepository(_dto));
        private TypeVersionRepository TypeVersions => _typeVersions ?? (_typeVersions = new TypeVersionRepository(_dto));
        private ConfigurationRepository Configurations => _configurations ?? (_configurations = new ConfigurationRepository(_dto));
        private ApplicationGroupAccessRepository ApplicationGroupAccess => _applicationGroupAccess ?? (_applicationGroupAccess = new ApplicationGroupAccessRepository(_dto));
        private ApplicationAccessRepository ApplicationAccess => _applicationAccess ?? (_applicationAccess = new ApplicationAccessRepository(_dto));


        private ApplicationAccessRepository _applicationAccess;
        private ApplicationGroupAccessRepository _applicationGroupAccess;
        private ConfigurationRepository _configurations;
        private TypeVersionRepository _typeVersions;
        private TypeRepository _types;
        private EnvironmentRepository _environments;
        private ApplicationGroupRepository _applicationGroups;
        private UserRepository _users;
        private ApplicationRepository _applications;
        private readonly DtoSqlManager _dto;

    }

}
