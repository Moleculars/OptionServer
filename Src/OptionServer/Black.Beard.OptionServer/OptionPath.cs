using Bb.OptionServer.Entities;
using System;
using System.Collections.Generic;

namespace Bb.OptionServer
{

    public enum KindPathEnum
    {
        User,
        Group,
        Application,
        Environment,
        Configuration,
    }

    public class OptionPath
    {


        public static OptionPath GetUser(string path)
        {
            return new OptionPath(path, KindPathEnum.User);
        }

        public static OptionPath GetGroup(string path)
        {
            var result = new OptionPath(path, KindPathEnum.Group);
            return result;
        }

        public static OptionPath GetApplication(string path)
        {
            return new OptionPath(path, KindPathEnum.Application);
        }

        public static OptionPath GetEnvironment(string path)
        {
            return new OptionPath(path, KindPathEnum.Environment);
        }

        public static OptionPath GetConfiguration(string path)
        {
            return new OptionPath(path, KindPathEnum.Configuration);
        }

        public OptionPath(string fullPath, KindPathEnum kind) : this(fullPath.Split(Constants.SeparatorPathChar), kind)
        {

        }

        public OptionPath(string[] path, KindPathEnum kind)
        {

            Owner = new UserPath();
            Group = new ApplicationGroupPath(Owner);
            Application = new ApplicationPath(Group);

            switch (kind)
            {

                case KindPathEnum.User:
                    Owner.OwnerPseudo = path[0];
                    break;

                case KindPathEnum.Group:
                    if (path.Length == 1)
                        Group.Name = path[0];

                    else if (path.Length == 2)
                    {
                        Owner.OwnerPseudo = path[0];
                        Group.Name = path[1];
                    }
                    break;

                case KindPathEnum.Application:
                    if (path.Length == 1)
                        Application.Name = path[0];

                    else if (path.Length == 2)
                    {
                        Group.Name = path[0];
                        Application.Name = path[1];
                    }
                    else if (path.Length == 3)
                    {
                        Owner.OwnerPseudo = path[0];
                        Group.Name = path[1];
                        Application.Name = path[2];
                    }
                    break;

                case KindPathEnum.Environment:
                    if (path.Length == 1)
                        Environment = path[0];

                    else if (path.Length == 2)
                    {
                        Application.Name = path[0];
                        Environment = path[1];
                    }
                    else if (path.Length == 3)
                    {
                        Group.Name = path[0];
                        Application.Name = path[1];
                        Environment = path[2];
                    }
                    else if (path.Length == 4)
                    {
                        Owner.OwnerPseudo = path[0];
                        Group.Name = path[1];
                        Application.Name = path[2];
                        Environment = path[3];
                    }
                    break;

                case KindPathEnum.Configuration:
                    if (path.Length == 1)
                        Configuration = path[0];

                    else if (path.Length == 2)
                    {
                        Environment = path[0];
                        Configuration = path[1];
                    }
                    else if (path.Length == 3)
                    {
                        Application.Name = path[0];
                        Environment = path[1];
                        Configuration = path[2];
                    }
                    else if (path.Length == 4)
                    {
                        Group.Name = path[0];
                        Application.Name = path[1];
                        Environment = path[2];
                        Configuration = path[3];
                    }
                    else if (path.Length == 5)
                    {
                        Owner.OwnerPseudo = path[0];
                        Group.Name = path[1];
                        Application.Name = path[2];
                        Environment = path[3];
                        Configuration = path[4];
                    }
                    break;

                default:
                    break;
            }

        }

        public UserPath Owner { get; }

        public ApplicationGroupPath Group { get; private set; }

        public ApplicationPath Application { get; }

        public string Environment { get; }

        public string Configuration { get; }

        public OptionPath Map(UserEntity user)
        {

            if (!string.IsNullOrEmpty(Owner.OwnerPseudo))
                MapUser(user);

            if (!string.IsNullOrEmpty(Group.Name) && Group.Infos == null)
                MapGroup(user);

            if (!string.IsNullOrEmpty(Application.Name) && Application.Infos == null)
                MapApplication(user);

            return this;

        }

        private void MapApplication(UserEntity user)
        {

            if (Group.Infos != null)
                Application.Infos = Group.Infos.Application(Application.Name);

            else
            {

                List<ApplicationEntity> lst3 = user.Applications(Application.Name);

                if (lst3.Count == 1)
                {
                    Application.Infos = lst3[0];
                    Group.Infos = Application.Infos.Group;
                    Owner.Infos = Group.Infos.Owner;
                    Owner.OwnerUserId = Group.Infos.Owner.Id;
                    Owner.OwnerPseudo = Group.Infos.Owner.Pseudo;
                }

                else if (lst3.Count > 1)
                    throw new Exceptions.AmbigiousNameException(Application.Name);

            }

            if (Application.Infos == null)
                throw new Exceptions.InvalidNameException(Application.Name);

        }

        private void MapGroup(UserEntity user)
        {
            if (Owner.Infos != null)
                Group.Infos = Owner.Infos.Group(Group.Name);

            else
            {

                List<GroupEntity> lst = user.Groups(Group.Name);

                if (lst.Count == 1)
                {
                    Group.Infos = lst[0];
                    Owner.Infos = Group.Infos.Owner;
                    Owner.OwnerUserId = Group.Infos.Owner.Id;
                    Owner.OwnerPseudo = Group.Infos.Owner.Pseudo;
                }

                else if (lst.Count > 1)
                    throw new Exceptions.AmbigiousNameException(Group.Name);

            }

            if (Group.Infos == null)
                throw new Exceptions.InvalidNameException(Group.Name);
        }

        private void MapUser(UserEntity user)
        {
            if (user.OwnerAccess.TryGetValue(Owner.OwnerPseudo, out OwnerEntity owner))
                Owner.Infos = owner;

            else
                throw new Exceptions.AmbigiousNameException(Owner.OwnerPseudo);
        }
    }


    public class ApplicationPath
    {

        public ApplicationPath(ApplicationGroupPath group)
        {
            Group = group;
        }

        public ApplicationGroupPath Group { get; }

        public string Name { get; internal set; }

        public ApplicationEntity Infos { get; internal set; }

    }

    public class ApplicationGroupPath
    {

        public ApplicationGroupPath(UserPath owner)
        {
            Owner = owner;
        }

        public string Name { get; internal set; }

        public GroupEntity Infos { get; internal set; }

        public UserPath Owner { get; }
    }

    public class UserPath
    {

        public UserPath()
        {

        }

        public Guid OwnerUserId { get; internal set; }

        public OwnerEntity Infos { get; internal set; }
        public string OwnerPseudo { get; internal set; }
    }


}





// Pseudo/Group/Application/Environment/Configuration/Version