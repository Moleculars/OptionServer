using Bb.OptionServer.Entities;
using Bb.OptionServer.Exceptions;
using Bb.OptionServer.Repositories.Tables;
using System;
using System.Linq;

namespace Bb.OptionServer
{

    public partial class OptionServices
    {

        public UserEntity CreateGroupApplication(UserEntity user, string groupName)
        {

            var group1 = ApplicationGroups.GetByName(user.Id, groupName);
            if (group1 != null)
                throw new AllreadyExistException($"{user.Pseudo}.{groupName}");

            var group = new ApplicationGroupTable();
            group.Id.Value = Guid.NewGuid();
            group.Name.Value = groupName;
            group.OwnerUserId.Value = user.Id;
            group.SecurityCoherence.Value = Guid.NewGuid();
            group.LastUpdate.Value = DateTimeOffset.Now;

            var groupAccess = new ApplicationGroupAccessTable();
            groupAccess.AccessApplication.Value = (int)AccessEntityEnum.Owner;
            groupAccess.AccessEnvironment.Value = (int)AccessEntityEnum.Owner;
            groupAccess.AccessType.Value = (int)AccessEntityEnum.Owner;
            groupAccess.ApplicationGroupId.Value = group.Id;
            groupAccess.UserId.Value = user.Id;
            groupAccess.SecurityCoherence.Value = Guid.NewGuid();
            groupAccess.LastUpdate.Value = DateTimeOffset.Now;

            using (var trans = ApplicationGroups.Dto.Sql.GetTransaction())
            {

                var r = ApplicationGroups.Insert(group)
                        ? ApplicationGroupAccess.Insert(groupAccess)
                        : false
                        ;

                trans.Commit();

            }

            var grp = User(user.Id);

            return grp;

        }

        public UserEntity SetAccess(UserEntity user, UserEntity userToGrant, string fullGroupname, AccessEntityEnum accessApplication, AccessEntityEnum accessType, AccessEntityEnum accessEnvironment)
        {

            var path = user.ResolveGroup(fullGroupname)
                ;

            if (path.Owner.OwnerUserId != Guid.Empty && path.Owner.OwnerUserId != user.Id)
                throw new NotEnoughtRightException(fullGroupname);

            var g1 = userToGrant.Groups(path.Group.Name).FirstOrDefault();
            if (g1 != null)
            {

                var g2 = new ApplicationGroupAccessTable();
                g2.ApplicationGroupId.Value = g1.GroupId;
                g2.UserId.Value = userToGrant.Id;
                g2.SecurityCoherence.Value = g1.SecurityCoherence;
                g2.AccessApplication.Value = (int)g1.ApplicationAccess;
                g2.AccessEnvironment.Value = (int)g1.EnvironmentAccess;
                g2.AccessType.Value = (int)g1.TypeAccess;
                g2.Reset();

                g2.AccessApplication.Value = (int)accessApplication;
                g2.AccessEnvironment.Value = (int)accessEnvironment;
                g2.AccessType.Value = (int)accessType;

                ApplicationGroupAccess.Update(g2);
            }
            else
            {

                var g3 = new ApplicationGroupAccessTable();
                g3.ApplicationGroupId.Value = path.Group.Infos.GroupId;
                g3.UserId.Value = userToGrant.Id;

                g3.SecurityCoherence.Value = Guid.NewGuid();

                g3.AccessApplication.Value = (int)accessApplication;
                g3.AccessEnvironment.Value = (int)accessEnvironment;
                g3.AccessType.Value = (int)accessType;


                ApplicationGroupAccess.Insert(g3);

            }

            var grp = User(userToGrant.Id);

            return grp;

        }

    }


}




