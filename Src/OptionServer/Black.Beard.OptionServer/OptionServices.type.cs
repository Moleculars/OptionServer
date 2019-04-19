using Bb.OptionServer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb
{

    public partial class OptionServices
    {


        public TypeEntity GetType(string username, string groupName, string typeName)
        {

            var group = CheckGroup(username, groupName, AccessEntityEnum.Read, objectKingEnum.Type);

            var o = Types.Read(group.ApplicationGroupId, typeName);
            if (o != null)
                Types.ReadVersionsByGroupIds(new List<TypeEntity>() { o });

            return o;

        }

        public List<TypeEntity> GetTypes(string username, string groupName)
        {

            var group = CheckGroup(username, groupName, AccessEntityEnum.Read, objectKingEnum.Type);

            var o = Types.ReadAll(group.ApplicationGroupId).ToList();

            Types.ReadVersionsByGroupIds(o);

            return o;

        }

        public TypeEntity UpdateContract(string username, string groupName, string nameType, string contract)
        {

            var group = CheckGroup(username, groupName, AccessEntityEnum.Write, objectKingEnum.Type);

            var type = Types.Read(group.ApplicationGroupId, nameType);
            if (type == null)
                throw new Exceptions.InvalidValueException($"{nameof(nameType)}");

            if (type.Version.Contract != contract)
            {
                var id = Guid.NewGuid();
                type.Version = new Entities.TypeVersionEntity()
                {
                    Id = id,
                    TypeId = type.Id,
                    Contract = contract,
                    Sha256 = string.IsNullOrEmpty(contract) ? string.Empty : Entities.TypeVersionEntity.Sha256_hash(contract),
                    Version = 0,
                };

                Types.UpdateContract(type);

                Types.ReadVersionsByGroupIds(new List<TypeEntity>() { type });

            }


            return type;

        }

        public TypeEntity UpdateExtension(string username, string groupName, string nameType, string extension)
        {

            if (!extension.StartsWith("."))
                extension = "." + extension;

            var group = CheckGroup(username, groupName, AccessEntityEnum.Write, objectKingEnum.Type);

            var type = Types.Read(group.ApplicationGroupId, nameType);
            if (type == null)
                throw new Exceptions.InvalidValueException($"{nameof(nameType)}");

            if (type.Extension != extension)
            {
                type.Extension = extension;
                Types.UpdateExtension(type);
            }

            Types.ReadVersionsByGroupIds(new List<TypeEntity>() { type });

            return type;

        }

        public TypeEntity AddType(string username, string groupName, string name, string extension, string contract)
        {

            var group = CheckGroup(username, groupName, AccessEntityEnum.Add, objectKingEnum.Type);

            var typeId = Guid.NewGuid();
            var id = Guid.NewGuid();
            var type = new TypeEntity()
            {
                Id = typeId,
                GroupId = group.ApplicationGroupId,
                Name = name,
                CurrentVersionId = id,
                Extension = extension,
                Version = new Entities.TypeVersionEntity()
                {
                    Id = id,
                    TypeId = typeId,
                    Contract = contract,
                    Sha256 = string.IsNullOrEmpty(contract) ? string.Empty : Entities.TypeVersionEntity.Sha256_hash(contract),
                    Version = 0,
                }
            };

            Types.Insert(type);

            return type;

        }

        //public TypeEntity Type(Guid groupId, string name)
        //{
        //    var type = Types.Read(groupId, name);
        //    return type;
        //}

        private TypeRepository Types => _types ?? (_types = new TypeRepository(_manager));

        private TypeRepository _types;


    }
}
