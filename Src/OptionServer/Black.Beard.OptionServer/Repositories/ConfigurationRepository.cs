using Bb.OptionServer.Repositories.Tables;
using System;
using System.Collections.Generic;
using System.Data;

namespace Bb.OptionServer.Repositories
{

    public class ConfigurationRepository : Repository<DocumentVersionTable>
    {

        public ConfigurationRepository(DtoSqlManager dto) : base(dto)
        {

        }

        public IEnumerable<DocumentVersionTable> ReadDocumentForApplication(Guid applicationId, Guid environmentId, bool withData = false)
        {
            if (withData)
            {

                var reader = _dtoManager.Select<DocumentVersionTable>(c => c.ApplicationId == applicationId, c => c.EnvironmentId == environmentId);

                foreach (DocumentVersionTable item in reader)
                    yield return item;
            }
            else
            {

                string sql = @"
                    SELECT TOP (1000) [Id]
                          ,[ApplicationId]
                          ,[Name]
                          ,[LastUpdate]
                          ,[EnvironmentId]
                          ,[TypeId]
                          ,[TypeVersionId]
                          ,[Payload]
                          ,[Version]
                          ,[Sha256]
                          ,[Deleted]
                          ,[SecurityCoherence]
                    FROM [Options].[dbo].[DocumentVersion]
                    WHERE [ApplicationId] = @applicationId
                        AND [EnvironmentId] = environmentId
";
                var _sql = _dtoManager.Sql;
                var reader = _sql.Read<DocumentVersionTable>(sql, DtoSqlManager.GetMapping(typeof(DocumentVersionTable)), 
                    _sql.CreateParameter(nameof(DocumentVersionTable.ApplicationId), DbType.Guid, applicationId),
                    _sql.CreateParameter(nameof(DocumentVersionTable.EnvironmentId), DbType.Guid, environmentId)
                    );

                foreach (DocumentVersionTable item in reader)
                    yield return item;

            }
        }

        public IEnumerable<DocumentVersionTable> ReadDocumentForApplication(Guid applicationId, bool withData = false)
        {
            if (withData)
            {
                var reader = _dtoManager.Select<DocumentVersionTable>(c => c.ApplicationId == applicationId);

                foreach (DocumentVersionTable item in reader)
                    yield return item;
            }
            else
            {

                string sql = @"
                    SELECT TOP (1000) [Id]
                          ,[ApplicationId]
                          ,[Name]
                          ,[LastUpdate]
                          ,[EnvironmentId]
                          ,[TypeId]
                          ,[TypeVersionId]
                          ,[Payload]
                          ,[Version]
                          ,[Sha256]
                          ,[Deleted]
                          ,[SecurityCoherence]
                    FROM [Options].[dbo].[DocumentVersion]
                    WHERE [ApplicationId] = @applicationId
";
                var _sql = _dtoManager.Sql;
                var reader = _sql.Read<DocumentVersionTable>(sql, DtoSqlManager.GetMapping(typeof(DocumentVersionTable)), _sql.CreateParameter(nameof(DocumentVersionTable.ApplicationId), DbType.Guid, applicationId));

                foreach (DocumentVersionTable item in reader)
                    yield return item;

            }
        }

    }
}
