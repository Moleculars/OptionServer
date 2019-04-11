using Bb.Entities;
using System;
using System.Data;

namespace Bb.OptionServer
{

    internal class TypeVersionRepository
    {

        public TypeVersionRepository(SqlManager provider)
        {
            _provider = provider;
        }

        internal void Insert(TypeVersionEntity version)
        {
            version.SecurityCoherence = Guid.NewGuid();
            version.Version++;
            if (string.IsNullOrEmpty(version.Contract))
                version.Sha256 = string.Empty;
            else
                version.Sha256 = TypeVersionEntity.Sha256_hash(version.Contract);

            DbUpdateContext ctx = new DbUpdateContext()
            {
                Factory = _provider.Factory,
            };
            version.GenerateSave(ctx);

            string sql = @"
            INSERT INTO [dbo].[TypeVersion] ([Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence]) 
            VALUES(@id, @version, @typeId, @contract, @sha256, CURRENT_TIMESTAMP, @securityCoherence)";
            _provider.Update(sql, ctx.Items.ToArray());

        }

        internal void Update(TypeVersionEntity version)
        {

            version.SecurityCoherence = Guid.NewGuid();
            version.Version++;
            if (string.IsNullOrEmpty(version.Contract))
                version.Sha256 = string.Empty;
            else
                version.Sha256 = TypeVersionEntity.Sha256_hash(version.Contract);

            DbUpdateContext ctx = new DbUpdateContext()
            {
                Factory = _provider.Factory,
            };
            version.GenerateSave(ctx);

            string sql = @"
            INSERT INTO [dbo].[TypeVersion] ([Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence]) 
            VALUES(@id, @version, @typeId, @contract, @sha256, CURRENT_TIMESTAMP, @securityCoherence)";
            _provider.Update(sql, ctx.Items.ToArray());

        }

        public TypeVersionEntity ReadById(Guid id)
        {

            string sql = @"SELECT [Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence] 
                           FROM [Options].[dbo].[TypeVersion] WHERE [Id] = @id";

            var reader = _provider.Read<TypeVersionEntity>(sql, _provider.CreateParameter("id", DbType.Guid, id));
            foreach (TypeVersionEntity version in reader)
                return version;

            return null;

        }

        public TypeVersionEntity ReadByContract(Guid typeid, string contract)
        {

            string sql = @"SELECT [Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence] 
                           FROM [Options].[dbo].[TypeVersion] WHERE [Id] = @id";

            string sha256 = TypeVersionEntity.Sha256_hash(contract);

            var reader = _provider.Read<TypeVersionEntity>(sql,
                _provider.CreateParameter("id", DbType.Guid, typeid),
                _provider.CreateParameter("sha256", DbType.String, sha256)
                );

            foreach (TypeVersionEntity version in reader)
                return version;

            return null;

        }

        public TypeVersionEntity ReadBySha256(Guid typeid, string sha256)
        {

            string sql = @"SELECT [Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence] 
                           FROM [Options].[dbo].[TypeVersion] WHERE [Id] = @id";

            var reader = _provider.Read<TypeVersionEntity>(sql,
                _provider.CreateParameter("id", DbType.Guid, typeid),
                _provider.CreateParameter("sha256", DbType.String, sha256)
                );

            foreach (TypeVersionEntity version in reader)
                return version;

            return null;

        }

        private readonly SqlManager _provider;

    }
}
