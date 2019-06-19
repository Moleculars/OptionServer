using Bb.OptionServer.Entities;
using Bb.OptionServer;
using Bb.OptionServer.Repositories.Tables;
using System;

namespace Bb.OptionServer
{

    public partial class OptionServices
    {

        public TypeEntity AddType(UserEntity user, string groupName, string name, string extension, string contract)
        {

            if (!extension.StartsWith("."))
                extension = "." + extension;

            if (string.IsNullOrEmpty(contract))
                contract = "_";

            var path = user.ResolveGroup(groupName);
            var group = user.CheckGroup(path, AccessEntityEnum.Operator, objectKingEnum.Type);

            var typeId = Guid.NewGuid();
            var versionId = Guid.NewGuid();

            var type = new TypeTable();
            type.Id.Value = typeId;
            type.GroupId.Value = group.GroupId;
            type.Name.Value = name;
            type.Extension.Value = extension;
            type.SecurityCoherence.Value = Guid.NewGuid();

            var version = new TypeVersionTable();
            version.Id.Value = versionId;
            version.SecurityCoherence.Value = Guid.NewGuid();
            version.TypeId.Value = typeId;
            version.Version.Value = 1;
            version.Contract.Value = contract;
            version.Sha256.Value = string.IsNullOrEmpty(contract) ? string.Empty : Sha.Sha256_hash(contract);

            using (var trans = Types.Dto.Sql.GetTransaction())
            {
                Types.Insert(type);
                TypeVersions.Insert(version);
                type.CurrentVersionId.Value = versionId;
                Types.Update(type);
                trans.Commit();
            }

            return GetType(user, groupName, name);

        }

        public TypeEntity GetType(UserEntity user, string groupName, string typeName)
        {

            var path = user.ResolveGroup(groupName);
            var group = user.CheckGroup(path, AccessEntityEnum.Reader, objectKingEnum.Type);

            if (!group.TypesAreLoaded)
                Types.LoadTypesForUser(user);

            TypeEntity type = group.GetType(typeName);

            return type;

        }

        public TypeEntity UpdateExtension(UserEntity user, string groupName, string nameType, string extension)
        {

            if (!extension.StartsWith("."))
                extension = "." + extension;

            var path = user.ResolveGroup(groupName);
            var group = user.CheckGroup(path, AccessEntityEnum.Operator, objectKingEnum.Type);
            TypeEntity type = group.GetType(nameType);
            if (type == null)
            {
                type = GetType(user, groupName, nameType);

                if (type == null)
                    throw new Exceptions.InvalidValueException($"{nameof(nameType)}");

            }

            if (type.Extension != extension)
            {
                var t = new TypeTable(type.TypeId, type.TypeName, type.Extension, type.Group.GroupId, type.CurrentVersion.Id, type.SecurityCoherence);
                t.Extension.Value = extension;
                if (Types.UpdateExtension(t))
                {
                    type.SecurityCoherence = type.SecurityCoherence;
                    type.Extension = extension;
                }
            }

            return type;

        }

        public TypeEntity UpdateContract(UserEntity user, string groupName, string nameType, string contract)
        {

            var path = user.ResolveGroup(groupName);
            var group = user.CheckGroup(path, AccessEntityEnum.Operator, objectKingEnum.Type);
            TypeEntity type = group.GetType(nameType);
            if (type == null)
            {

                type = GetType(user, groupName, nameType);

                if (type == null)
                    throw new Exceptions.InvalidValueException($"{nameof(nameType)}");

            }

            var sha256 = Sha.Sha256_hash(contract);
            if (type.CurrentVersion.Sha256 != sha256)
            {

                var t2 = new TypeVersionTable(Guid.NewGuid(), type.CurrentVersion.Version + 1, type.TypeId, contract, sha256, Guid.NewGuid());
                var t1 = new TypeTable(type.TypeId, type.TypeName, type.Extension, type.Group.GroupId, type.CurrentVersion.Id, type.SecurityCoherence);

                t1.CurrentVersionId.Value = t2.Id;
                using (var trans = Types.Dto.Sql.GetTransaction(false))
                    if (TypeVersions.Insert(t2))
                        if (Types.Update(t1))
                        {
                            trans.Commit();

                            type.SecurityCoherence = t1.SecurityCoherence.Value;
                            type.CurrentVersion.Id = t2.Id;
                            type.SecurityCoherence = t2.SecurityCoherence.Value;
                            type.CurrentVersion.Contract = contract;
                            type.CurrentVersion.Sha256 = sha256;

                        }

            }

            return type;


        }

        public void LoadTypesHistory(UserEntity user)
        {
            foreach (var group in user.Groups())
                TypeVersions.LoadTypesHistory(group);

        }

        public void LoadTypesHistory(GroupEntity group)
        {

            TypeVersions.LoadTypesHistory(group);
        }

        //public List<TypeEntity> GetTypes(UserEntity user, string groupName)
        //{

        //    var path = user.ResolveGroupApplication(groupName);
        //    var group = user.CheckGroup(path, AccessEntityEnum.List, objectKingEnum.Type);

        //    var o = Types.ReadAllForGroup(group.GroupId);

        //    Types.ReadVersionsByGroupIds(o);

        //    return o;

        //}





    }
}
