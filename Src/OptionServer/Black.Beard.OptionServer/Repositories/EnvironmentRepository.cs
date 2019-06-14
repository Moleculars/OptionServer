using Bb.OptionServer.Repositories.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.OptionServer.Repositories
{

    public class EnvironmentRepository : Repository<EnvironmentTable>
    {

        public EnvironmentRepository(DtoSqlManager manager) : base(manager)
        {

        }


        public IEnumerable<EnvironmentTable> ReadAllForGroup(Guid groupId)
        {

            var reader = base._dtoManager.Select<EnvironmentTable>(c => c.GroupId == groupId).ToList();

            //string sql = @"SELECT [Id], [Name], [GroupId], [LastUpdate], [SecurityCoherence] FROM [dbo].[Environment] WHERE [GroupId] = @groupId";
            //var _sql = _dtoManager.Sql;
            //var mapping = DtoSqlManager.GetMapping(typeof(EnvironmentTable));
            //var reader = _sql.Read<EnvironmentTable>(sql, mapping, _sql.CreateParameter("groupId", DbType.Guid, groupId));

            foreach (EnvironmentTable env in reader)
                yield return env;

        }

        public EnvironmentTable Read(Guid groupId, string name)
        {

            return ReadAllForGroup(groupId).Where(c => c.Name == name).FirstOrDefault();

        }

    }
}
