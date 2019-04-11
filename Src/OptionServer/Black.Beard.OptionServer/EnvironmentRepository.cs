using System;
using System.Collections.Generic;
using System.Data;

namespace Bb.OptionServer
{

    public class EnvironmentRepository
    {

        public EnvironmentRepository(SqlManager provider)
        {
            _provider = provider;
        }

        public bool Save(EnvironmentEntity env)
        {

            EnvironmentEntity u1 = null;
            bool result = false;

            if (env.Id != Guid.Empty)
            {
                u1 = Read(env.Id);
                if (u1 != null && u1.GroupId != env.GroupId)
                    throw new Exceptions.InvalidValueException(nameof(env.GroupId));

            }
            else
                env.Id = Guid.NewGuid();

            if (env.SecurityCoherence == Guid.Empty)
                env.SecurityCoherence = Guid.NewGuid();

            DbUpdateContext ctx = new DbUpdateContext()
            {
                Factory = _provider.Factory,
            };
            env.GenerateSave(ctx);
            ctx.Items.Add(_provider.CreateParameter("newSecurityCoherence", DbType.Guid, Guid.NewGuid()));

            if (u1 == null)
            {
                string sql = @"INSERT INTO [dbo].[Environment] ([Id], [Name], [GroupId], [LastUpdate], [SecurityCoherence]) VALUES(@id, @Name, @groupId, CURRENT_TIMESTAMP, @newSecurityCoherence)";
                result = _provider.Update(sql, ctx.Items.ToArray());
            }
            else
            {
                string sql = @"
UPDATE [dbo].[Environment] SET [Name] = @name, [LastUpdate] = CURRENT_TIMESTAMP, [SecurityCoherence] = @newSecurityCoherence
WHERE [Id] = @id AND [SecurityCoherence]=@securityCoherence";
                result = _provider.Update(sql, ctx.Items.ToArray());
            }

            var u2 = Read(env.Id);
            env.SecurityCoherence = u2.SecurityCoherence;
            env.LastUpdate = u2.LastUpdate;

            return result;

        }

        public EnvironmentEntity Read(Guid id)
        {

            string sql = @"SELECT [Id], [Name], [GroupId], [LastUpdate], [SecurityCoherence] FROM [dbo].[Environment] WHERE [Id] = @id";

            var reader = _provider.Read<EnvironmentEntity>(sql, _provider.CreateParameter("id", DbType.Guid, id));
            foreach (EnvironmentEntity env in reader)
                return env;

            return null;

        }

        public IEnumerable<EnvironmentEntity> ReadAll(Guid groupId)
        {

            string sql = @"SELECT [Id], [Name], [GroupId], [LastUpdate], [SecurityCoherence] FROM [dbo].[Environment] WHERE [GroupId] = @groupId";

            var reader = _provider.Read<EnvironmentEntity>(sql, _provider.CreateParameter("groupId", DbType.Guid, groupId));

            foreach (EnvironmentEntity env in reader)
                yield return env;

        }

        public EnvironmentEntity Read(Guid groupId, string name)
        {

            string sql = @"SELECT [Id], [Name], [GroupId], [LastUpdate], [SecurityCoherence] FROM [dbo].[Environment] WHERE [GroupId]=@groupId AND [Name] = @name";

            var reader = _provider.Read<EnvironmentEntity>(sql,
                _provider.CreateParameter("groupId", DbType.Guid, groupId),
                _provider.CreateParameter("name", DbType.String, name)
                );
            foreach (EnvironmentEntity env in reader)
                return env;

            return null;

        }

        private readonly SqlManager _provider;

    }
}
