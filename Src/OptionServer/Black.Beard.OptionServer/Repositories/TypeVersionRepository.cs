using Bb.OptionServer.Entities;
using Bb.OptionServer.Repositories.Tables;

namespace Bb.OptionServer.Repositories
{

    internal class TypeVersionRepository : Repository<TypeVersionTable>
    {

        public TypeVersionRepository(DtoSqlManager manager) : base(manager)
        {

        }

        public void LoadTypesHistory(GroupEntity group)
        {

            string sql = @"
              SELECT 
	               v.[Id]
                  ,v.[LastUpdate]
                  ,v.[Version]
                  ,v.[TypeId]
                  ,v.[Contract]
                  ,v.[Sha256]
                  ,v.[SecurityCoherence]

              FROM [Options].[dbo].[Type] t
              INNER JOIN [Options].[dbo].[TypeVersion] v ON t.[Id] = v.[TypeId]
              WHERE [GroupId] = @groupId";

            ObjectMapping mapping = DtoSqlManager.GetMapping(typeof(TypeVersionTable));

            var versions = Dto.Sql.Read<TypeVersionTable>(sql, mapping, Dto.Sql.CreateParameter("groupId", System.Data.DbType.Guid, group.GroupId));

            foreach (var item in versions)
            {

                var type = group.GetType(item.TypeId);
                if (type == null)
                    if (System.Diagnostics.Debugger.IsAttached)
                        System.Diagnostics.Debugger.Break();

                if(!type.Versions.TryGetValue(item.Id, out TypeVersionEntity version))
                {
                    type.Versions.Add(item.Id, version = new TypeVersionEntity()
                    {
                        Id = item.Id,
                        Contract = item.Contract,
                        Sha256 = item.Sha256,
                        Version = item.Version,
                        SecurityCoherence = item.SecurityCoherence
                    });
                }
                else
                {
                    version.SecurityCoherence = item.SecurityCoherence;
                    version.Sha256 = item.Sha256;
                    version.Version = item.Version;
                    version.Contract = item.Contract;
                }

            }

        }



        //        internal void Insert(TypeVersionEntity version)
        //        {
        //            version.SecurityCoherence = Guid.NewGuid();
        //            version.Version++;
        //            if (string.IsNullOrEmpty(version.Contract))
        //                version.Sha256 = string.Empty;
        //            else
        //                version.Sha256 = TypeVersionEntity.Sha256_hash(version.Contract);

        //            DbUpdateContext ctx = new DbUpdateContext()
        //            {
        //                Factory = _provider.Factory,
        //            };
        //            version.GenerateSave(ctx);

        //            string sql = @"
        //            INSERT INTO [dbo].[TypeVersion] ([Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence]) 
        //            VALUES(@id, @version, @typeId, @contract, @sha256, CURRENT_TIMESTAMP, @securityCoherence)";
        //            _provider.Update(sql, ctx.Items.ToArray());

        //        }

        //        internal void Update(TypeVersionEntity version)
        //        {

        //            version.SecurityCoherence = Guid.NewGuid();
        //            version.Version++;
        //            if (string.IsNullOrEmpty(version.Contract))
        //                version.Sha256 = string.Empty;
        //            else
        //                version.Sha256 = TypeVersionEntity.Sha256_hash(version.Contract);

        //            DbUpdateContext ctx = new DbUpdateContext()
        //            {
        //                Factory = _provider.Factory,
        //            };
        //            version.GenerateSave(ctx);

        //            string sql = @"
        //            INSERT INTO [dbo].[TypeVersion] ([Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence]) 
        //            VALUES(@id, @version, @typeId, @contract, @sha256, CURRENT_TIMESTAMP, @securityCoherence)";
        //            _provider.Update(sql, ctx.Items.ToArray());

        //        }

        //        internal IEnumerable<TypeVersionEntity> ReadByGroupId(Guid groupId)
        //        {
        //            string sql = @"
        //                SELECT 
        //	                v.[Id],
        //                    v.[LastUpdate],
        //                    v.[Version],
        //                    v.[TypeId],
        //                    v.[Contract],
        //                    v.[Sha256],
        //                    v.[SecurityCoherence]
        //                FROM [Options].[dbo].[Type] t
        //                LEFT JOIN [Options].[dbo].[TypeVersion] v ON t.CurrentVersionId = v.Id
        //                WHERE t.GroupId = @groupId
        //";

        //            var reader = _provider.Read<TypeVersionEntity>(sql, 

        //                _provider.CreateParameter("groupId", DbType.Guid, groupId)

        //                );

        //            foreach (TypeVersionEntity version in reader)
        //                yield return version;

        //        }

        //        public TypeVersionEntity ReadById(Guid id)
        //        {

        //            string sql = @"SELECT [Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence] 
        //                           FROM [Options].[dbo].[TypeVersion] WHERE [Id] = @id";

        //            var reader = _provider.Read<TypeVersionEntity>(sql, _provider.CreateParameter("id", DbType.Guid, id));
        //            foreach (TypeVersionEntity version in reader)
        //                return version;

        //            return null;

        //        }

        //        public TypeVersionEntity ReadByContract(Guid typeid, string contract)
        //        {

        //            string sql = @"SELECT [Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence] 
        //                           FROM [Options].[dbo].[TypeVersion] WHERE [Id] = @id";

        //            string sha256 = TypeVersionEntity.Sha256_hash(contract);

        //            var reader = _provider.Read<TypeVersionEntity>(sql,
        //                _provider.CreateParameter("id", DbType.Guid, typeid),
        //                _provider.CreateParameter("sha256", DbType.String, sha256)
        //                );

        //            foreach (TypeVersionEntity version in reader)
        //                return version;

        //            return null;

        //        }

        //        public TypeVersionEntity ReadBySha256(Guid typeid, string sha256)
        //        {

        //            string sql = @"SELECT [Id], [Version], [TypeId], [Contract], [Sha256], [LastUpdate], [SecurityCoherence] 
        //                           FROM [Options].[dbo].[TypeVersion] WHERE [Id] = @id";

        //            var reader = _provider.Read<TypeVersionEntity>(sql,
        //                _provider.CreateParameter("id", DbType.Guid, typeid),
        //                _provider.CreateParameter("sha256", DbType.String, sha256)
        //                );

        //            foreach (TypeVersionEntity version in reader)
        //                return version;

        //            return null;

        //        }


    }
}
