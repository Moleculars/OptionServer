using Bb.OptionServer.Entities;
using Bb.OptionServer.Repositories.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Bb.OptionServer.Repositories
{

    public class TypeRepository : Repository<TypeTable>
    {

        public TypeRepository(DtoSqlManager manager) : base(manager)
        {

            _mapping = DtoSqlManager.GetMapping(GetType());


        }

        public List<TypeTable> ReadAllForGroup(Guid groupId)
        {
            return base._dtoManager.Select<TypeTable>(c => c.GroupId == groupId).ToList();

        }

        public TypeTable Read(Guid groupId, string name)
        {

            return ReadAllForGroup(groupId).Where(c => c.Name == name).FirstOrDefault();

        }

        public void LoadTypesForUser(UserEntity user)
        {

            string sql1 = @"
            SELECT 
				t.Id AS [TypeId],
	            t.Name as [TypeName],
	            t.Extension,
	            t.GroupId,
	            t.SecurityCoherence as [TypeSecurityCoherence],

	            t.CurrentVersionId,
	            v.Sha256,
                v.Contract,
	            v.SecurityCoherence as [VersionSecurityCoherence],
	            v.Version
	 
            FROM [Type] t
            LEFT JOIN TypeVersion v ON t.CurrentVersionId = v.Id

            WHERE t.GroupId IN (

	            SELECT DISTINCT aa.ApplicationGroupId
                FROM ApplicationGroupAccess aa
                LEFT JOIN ApplicationGroup ag ON ag.Id = aa.ApplicationGroupId
                LEFT JOIN Users u ON u.Id = aa.UserId

                WHERE (aa.UserId = @userId OR ag.OwnerUserId = @userId)
            )
        ";

            var sql = Dto.Sql;

            var arguserId = sql.CreateParameter("userId", DbType.Guid, user.Id);
            var datas = sql.Read<TypeEntity>(sql1, arguserId).ToList();

            foreach (TypeEntity item in datas)
            {
                var group = user.Group(item.Group.GroupId);
                group.AddType(item);
            }

        }


        public bool UpdateExtension(TypeTable type)
        {

            if (string.IsNullOrEmpty(type.Extension))
                throw new Exceptions.InvalidValueException(nameof(type.Extension));

            var result = Update(type);

            return result;

        }

        //public bool UpdateContract(TypeEntity type)
        //{

        //    bool result = false;

        //    if (string.IsNullOrEmpty(type.Extension))
        //        throw new Exceptions.InvalidValueException(nameof(type.Extension));

        //    string sql = @"UPDATE [dbo].[Type] SET [Name] = @name, [Extension]=@extension, [LastUpdate] = CURRENT_TIMESTAMP, [SecurityCoherence] = @newSecurityCoherenceType
        //    WHERE [Id] = @id AND [SecurityCoherence]=@securityCoherence";

        //    using (var trans = _provider.GetTransaction())
        //    {

        //        type.CurrentVersionId = type.Version.Id = Guid.NewGuid();
        //        _version.Insert(type.Version);

        //        DbUpdateContext ctx = new DbUpdateContext()
        //        {
        //            Factory = _provider.Factory,
        //        };
        //        type.GenerateSave(ctx);
        //        var p1 = _provider.CreateParameter("newSecurityCoherenceType", DbType.Guid, Guid.NewGuid());

        //        result = _provider.Update(sql, ctx.Items.ToArray());

        //        trans.Commit();

        //    }

        //    return result;

        //}


        //internal void ReadVersionsByGroupIds(List<TypeEntity> types)
        //{

        //    var indexByCurrentId = types.ToLookup(c => c.CurrentVersionId);
        //    var groupIds = new HashSet<Guid>(types.Select(c => c.GroupId));

        //    foreach (Guid groupId in groupIds)
        //        foreach (var version in _version.ReadByGroupId(groupId))
        //            if (indexByCurrentId.Contains(version.Id))
        //                foreach (var item2 in indexByCurrentId[version.Id])
        //                    item2.Version = version;

        //}



        //public TypeEntity Read(Guid id)
        //{

        //    string sql = @"SELECT [Id], [Name], [Extension], [GroupId], [CurrentVersionId], [LastUpdate], [SecurityCoherence] FROM [Options].[dbo].[Type] WHERE [Id] = @id";

        //    var reader = _provider.Read<TypeEntity>(sql, _provider.CreateParameter("id", DbType.Guid, id));
        //    foreach (TypeEntity env in reader)
        //        return env;

        //    return null;

        //}

        //public TypeEntity Read(Guid groupId, string name)
        //{

        //    string sql = @"SELECT [Id], [Name], [Extension], [GroupId], [CurrentVersionId], [LastUpdate], [SecurityCoherence] FROM [Options].[dbo].[Type] WHERE [GroupId]=@groupId AND [Name] = @name";

        //    var reader = _provider.Read<TypeEntity>(sql,
        //        _provider.CreateParameter("groupId", DbType.Guid, groupId),
        //        _provider.CreateParameter("name", DbType.String, name)
        //        );
        //    foreach (TypeEntity env in reader)
        //        return env;

        //    return null;

        //}

        //public IEnumerable<TypeEntity> ReadAll(Guid groupId)
        //{

        //    string sql = @"SELECT [Id], [Name], [Extension], [GroupId], [CurrentVersionId], [LastUpdate], [SecurityCoherence] FROM [Options].[dbo].[Type] WHERE [GroupId]=@groupId";

        //    var reader = _provider.Read<TypeEntity>(sql,
        //        _provider.CreateParameter("groupId", DbType.Guid, groupId)
        //        );

        //    foreach (TypeEntity env in reader)
        //        yield return env;

        //}

        private readonly TypeVersionRepository _version;
        private readonly ObjectMapping _mapping;
    }
}
