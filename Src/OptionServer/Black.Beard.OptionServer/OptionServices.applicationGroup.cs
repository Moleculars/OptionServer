//using Bb.OptionServer;

using Bb.Exceptions;
using Bb.OptionServer;
using System.Collections.Generic;
using System.Linq;

namespace Bb
{

    public partial class OptionServices
    {


        public ApplicationGroupAccessEntity SetAccess(string usernameOnwer, string usernameToGrant, string fullGroupname, AccessEntityEnum accessApplication, AccessEntityEnum accessType, AccessEntityEnum accessEnvironment)
        {

            if (!fullGroupname.StartsWith(usernameOnwer + "."))
                fullGroupname = $"{usernameOnwer}.{fullGroupname}";

            var userOwner = Users.Read(usernameOnwer) ?? throw new MissingUserException(usernameOnwer);
            var userToGrant = Users.Read(usernameToGrant) ?? throw new MissingUserException(usernameToGrant);

            var repGroup = ApplicationGroups;

            repGroup.SetAccess(userOwner.Id, userToGrant.Id, fullGroupname, accessApplication, accessType, accessEnvironment);
            var grp4 = repGroup.GetAccessByGoupApplicationName(userToGrant.Id, fullGroupname).FirstOrDefault();

            return grp4;

        }

        /// <summary>
        /// Groups the applications.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        public List<ApplicationGroupAccessEntity> GroupApplication(string username, string groupName)
        {

            if (!groupName.StartsWith(username + "."))
                groupName = $"{username}.{groupName}";

            var user = Users.Read(username) ?? throw new MissingUserException(username);
            var repository = ApplicationGroups;

            var result = repository.GetAccessByGoupApplicationName(user.Id, groupName);
            return result;
        }

        public List<ApplicationGroupAccessEntity> GetGroupApplicationsForUser(string username)
        {
            var user = User(username) ?? throw new MissingUserException(username);
            var repository = ApplicationGroups;
            var result = repository.GetAccessesByForUser(user.Id);
            return result;
        }

        public List<ApplicationGroupAccessEntity> CreateGroupApplication(string username, string groupName)
        {

            if (!groupName.StartsWith(username + "."))
                groupName = $"{username}.{groupName}";

            var repository = ApplicationGroups;

            var user = User(username) ?? throw new MissingUserException(username);

            var group1 = repository.GetByname(username, groupName);
            if (group1 != null && group1.Count > 1)
                throw new Exceptions.AllreadyExistException($"{username}.{groupName}");

            var result = repository.Create(user.Id, groupName);
            if (result)
            {
                var access = repository.GetAccessByGoupApplicationName(user.Id, groupName);
                return access;
            }

            return null;

        }

        private ApplicationGroupRepository ApplicationGroups => _applicationGroups ?? (_applicationGroups = new ApplicationGroupRepository(_manager));
        private ApplicationGroupRepository _applicationGroups;

    }


}
