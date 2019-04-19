using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Bb.OptionServer
{

    public class TypeRepository
    {

        public TypeRepository(SqlManager provider)
        {
            _provider = provider;
            _version = new TypeVersionRepository(provider);
        }

        public bool Insert(TypeEntity type)
        {

            bool result = false;

            if (string.IsNullOrEmpty(type.Extension))
                throw new Exceptions.InvalidValueException(nameof(type.Extension));

            type.SecurityCoherence = Guid.NewGuid();
            type.Version.SecurityCoherence = Guid.NewGuid();

            if (!type.Extension.StartsWith("."))
                type.Extension = "." + type.Extension;

            DbUpdateContext ctx = new DbUpdateContext()
            {
                Factory = _provider.Factory,
            };
            type.GenerateSave(ctx);


            string sql = @"INSERT INTO [dbo].[Type] ([Id], [Name], [Extension], [CurrentVersionId], [GroupId], [LastUpdate], [SecurityCoherence]) VALUES(@id, @Name, @extension, @currentVersionId, @groupId, CURRENT_TIMESTAMP, @securityCoherence)";

            using (var trans = _provider.GetTransaction())
            {
                result = _provider.Update(sql, ctx.Items.ToArray());
                _version.Insert(type.Version);
                trans.Commit();
            }

            var u2 = Read(type.Id);
            type.LastUpdate = u2.LastUpdate;

            var u3 = _version.ReadById(type.CurrentVersionId);
            type.Version.LastUpdate = u2.LastUpdate;

            return result;

        }

        internal void ReadVersionsByGroupIds(List<TypeEntity> types)
        {

            var indexByCurrentId = types.ToLookup(c => c.CurrentVersionId);
            var groupIds = new HashSet<Guid>(types.Select(c => c.GroupId));

            foreach (Guid groupId in groupIds)
                foreach (var version in _version.ReadByGroupId(groupId))
                    if (indexByCurrentId.Contains(version.Id))
                        foreach (var item2 in indexByCurrentId[version.Id])
                            item2.Version = version;

        }

        public bool UpdateContract(TypeEntity type)
        {

            bool result = false;

            if (string.IsNullOrEmpty(type.Extension))
                throw new Exceptions.InvalidValueException(nameof(type.Extension));

            string sql = @"UPDATE [dbo].[Type] SET [Name] = @name, [Extension]=@extension, [LastUpdate] = CURRENT_TIMESTAMP, [SecurityCoherence] = @newSecurityCoherenceType
            WHERE [Id] = @id AND [SecurityCoherence]=@securityCoherence";

            using (var trans = _provider.GetTransaction())
            {

                type.CurrentVersionId = type.Version.Id = Guid.NewGuid();
                _version.Insert(type.Version);

                DbUpdateContext ctx = new DbUpdateContext()
                {
                    Factory = _provider.Factory,
                };
                type.GenerateSave(ctx);
                var p1 = _provider.CreateParameter("newSecurityCoherenceType", DbType.Guid, Guid.NewGuid());

                result = _provider.Update(sql, ctx.Items.ToArray());

                trans.Commit();

            }

            return result;

        }

        public bool UpdateExtension(TypeEntity type)
        {

            bool result = false;

            if (string.IsNullOrEmpty(type.Extension))
                throw new Exceptions.InvalidValueException(nameof(type.Extension));

            string sql = @"UPDATE [dbo].[Type] SET [Name] = @name, [Extension]=@extension, [LastUpdate] = CURRENT_TIMESTAMP, [SecurityCoherence] = @newSecurityCoherenceType
            WHERE [Id] = @id AND [SecurityCoherence]=@securityCoherence";

            DbUpdateContext ctx = new DbUpdateContext()
            {
                Factory = _provider.Factory,
            };
            type.GenerateSave(ctx);
            ctx.Items.Add(_provider.CreateParameter("newSecurityCoherenceType", DbType.Guid, Guid.NewGuid()));
            result = _provider.Update(sql, ctx.Items.ToArray());

            return result;

        }


        public TypeEntity Read(Guid id)
        {

            string sql = @"SELECT [Id], [Name], [Extension], [GroupId], [CurrentVersionId], [LastUpdate], [SecurityCoherence] FROM [Options].[dbo].[Type] WHERE [Id] = @id";

            var reader = _provider.Read<TypeEntity>(sql, _provider.CreateParameter("id", DbType.Guid, id));
            foreach (TypeEntity env in reader)
                return env;

            return null;

        }

        public TypeEntity Read(Guid groupId, string name)
        {

            string sql = @"SELECT [Id], [Name], [Extension], [GroupId], [CurrentVersionId], [LastUpdate], [SecurityCoherence] FROM [Options].[dbo].[Type] WHERE [GroupId]=@groupId AND [Name] = @name";

            var reader = _provider.Read<TypeEntity>(sql,
                _provider.CreateParameter("groupId", DbType.Guid, groupId),
                _provider.CreateParameter("name", DbType.String, name)
                );
            foreach (TypeEntity env in reader)
                return env;

            return null;

        }

        public IEnumerable<TypeEntity> ReadAll(Guid groupId)
        {

            string sql = @"SELECT [Id], [Name], [Extension], [GroupId], [CurrentVersionId], [LastUpdate], [SecurityCoherence] FROM [Options].[dbo].[Type] WHERE [GroupId]=@groupId";

            var reader = _provider.Read<TypeEntity>(sql,
                _provider.CreateParameter("groupId", DbType.Guid, groupId)
                );

            foreach (TypeEntity env in reader)
                yield return env;

        }

        private readonly SqlManager _provider;
        private readonly TypeVersionRepository _version;

    }
}
