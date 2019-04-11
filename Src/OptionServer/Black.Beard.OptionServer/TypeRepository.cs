using System;
using System.Data;

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

            type.Id = Guid.NewGuid();
            type.CurrentVersionId = Guid.NewGuid();
            type.SecurityCoherence = Guid.NewGuid();

            if (type.Version == null)
                type.Version = new Entities.TypeVersionEntity()
                {
                    Id = type.CurrentVersionId,
                    TypeId = type.Id,
                };

            if (!type.Extension.StartsWith("."))
                type.Extension = "." + type.Extension;

            if (type.Version == null)
                type.Version = new Entities.TypeVersionEntity()
                {
                    Id = type.CurrentVersionId,
                    TypeId = type.Id,
                };

            DbUpdateContext ctx = new DbUpdateContext()
            {
                Factory = _provider.Factory,
            };
            type.GenerateSave(ctx);


            string sql = @"INSERT INTO [dbo].[Type] ([Id], [Name], [Extension], [CurrentVersionId], [GroupId], [LastUpdate], [SecurityCoherence]) VALUES(@id, @Name, @extension, null, @groupId, CURRENT_TIMESTAMP, @securityCoherenceType)";

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

        public bool Update(TypeEntity type)
        {

            bool result = false;

            if (string.IsNullOrEmpty(type.Extension))
                throw new Exceptions.InvalidValueException(nameof(type.Extension));

            if (!type.Extension.StartsWith("."))
                type.Extension = "." + type.Extension;

            var version = _version.ReadByContract(type.Id, type.Version.Contract);

            string sql = @"UPDATE [dbo].[Type] SET [Name] = @name, [Extension]=@extension, [LastUpdate] = CURRENT_TIMESTAMP, [SecurityCoherence] = @newSecurityCoherenceType
            WHERE [Id] = @id AND [SecurityCoherence]=@securityCoherence";

            using (var trans = _provider.GetTransaction())
            {

                if (version == null)
                {
                    type.CurrentVersionId = type.Version.Id = Guid.NewGuid();
                    _version.Insert(type.Version);
                }

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

        private readonly SqlManager _provider;
        private readonly TypeVersionRepository _version;
    }
}
