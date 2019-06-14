using Bb.OptionServer.Repositories.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.OptionServer.Repositories
{

    public class ApplicationRepository : Repository<ApplicationTable>
    {

        public ApplicationRepository(DtoSqlManager dtoManager) 
            : base(dtoManager)
        {
        }

        public List<ApplicationTable> GetForGroup(Guid groupId)
        {
            return this._dtoManager.Select<ApplicationTable>(c => c.GroupId == groupId)
                .ToList();
        }

        //public List<ApplicationEntity> GetEntityByname(string fullgroupName)
        //{
        //    string sql1 = @"SELECT a.Id, a.Name, a.SecurityCoherence FROM Application a WHERE a.Name = @name";
        //    var argName = _provider.CreateParameter("name", DbType.String, fullgroupName);
        //    var datas = _provider.Read<ApplicationGroupEntity>(sql1, argName).ToList();
        //    return datas;
        //}

        //        public void SetAccess(Guid ownerId, Guid toUserId, string fullgroupName, AccessEntityEnum accessApplication)
        //        {

        //            // Check exist for me
        //            var groups = GetByname(fullgroupName);
        //            if (groups == null || groups.Count == 0)
        //                throw new Exceptions.InvalidNameException(fullgroupName);

        //            // Check I have enought right for attribute right for other user 
        //            var group = groups.FirstOrDefault(c => c.OwnerUserId == ownerId);
        //            if (groups == null)
        //                throw new Exceptions.NotEnoughtRightException(fullgroupName);

        //            var item = GetAccessByGoupApplicationName(toUserId, fullgroupName).FirstOrDefault();

        //            bool result;

        //            if (item == null)
        //            {
        //                string sql = @"
        //INSERT INTO [dbo].[ApplicationAccess] ([ApplicationId], [UserId], [AccessApplication], [AccessType], [AccessEnvironment], [LastUpdate] ,[SecurityCoherence])
        //VALUES (@applicationId, @userId, @accessApplication, @accessType, @accessEnvironment, CURRENT_TIMESTAMP, NEWID())
        //";

        //                result = _provider.Update(sql,
        //                    _provider.CreateParameter("applicationGroupId", DbType.Guid, group.Id),
        //                    _provider.CreateParameter("userId", DbType.Guid, toUserId),
        //                    _provider.CreateParameter("accessApplication", DbType.Int32, accessApplication)
        //                );


        //            }
        //            else if (item.ApplicationAccess != accessApplication)
        //            {

        //                string sql = @"
        //UPDATE [dbo].[ApplicationGroupAccess] 
        //SET	
        //    [AccessApplication] = @accessApplication, [LastUpdate] = CURRENT_TIMESTAMP, [SecurityCoherence] = NEWID()
        //WHERE [SecurityCoherence] = @securityCoherence AND [ApplicationGroupId] = @applicationGroupId AND [UserId]=@userId
        //";

        //                result = _provider.Update(sql,
        //                    _provider.CreateParameter("applicationGroupId", DbType.Guid, group.Id),
        //                    _provider.CreateParameter("userId", DbType.Guid, toUserId),
        //                    _provider.CreateParameter("accessApplication", DbType.Int32, accessApplication),
        //                    _provider.CreateParameter("SecurityCoherence", DbType.Guid, item.SecurityCoherence)
        //                );
        //            }

        //        }

        //        public List<ApplicationGroupAccessEntity> GetAccessByGoupApplicationName(Guid userId, string fullGroupName)
        //        {

        //            string sql1 = @"
        //SELECT 
        //	u.Id [UserId],
        //	u.Username [Username],
        //	ag.OwnerUserId [Owner],
        //	ag.Id [ApplicationGroupId],
        //	ag.Name [ApplicationGroupName], 
        //	ac.AccessApplication [ApplicationAccess],
        //	ac.AccessEnvironment [EnvironmentAccess],
        //	ac.AccessType [TypeAccess],
        //    ac.[SecurityCoherence]
        //FROM ApplicationGroupAccess ac
        //LEFT JOIN ApplicationGroup ag ON ag.Id = ac.ApplicationGroupId
        //LEFT JOIN Users u ON u.Id = ac.UserId

        //WHERE (u.Id = @userId OR ag.OwnerUserId = @userId)
        //    AND ag.Name = @name 
        //";

        //            var argName = _provider.CreateParameter("name", DbType.String, fullGroupName);
        //            var arguserId = _provider.CreateParameter("userId", DbType.Guid, userId);
        //            var datas = _provider.Read<ApplicationGroupAccessEntity>(sql1, argName, arguserId).ToList();
        //            return datas;

        //        }

        //        public IEnumerable<ApplicationAccessEntity> GetApplicationAccesses(string username, Guid application)
        //        {

        //            string sql = @"
        //            SELECT 
        //	             ac.[UserId]
        //	            ,u.[Username]
        //	            ,ag.Id AS [GroupId]
        //	            ,ag.Name as [GroupName]
        //	            ,a.[Id] AS [ApplicationId]
        //	            ,a.[Name] AS [ApplicationName]
        //	            ,ac.[SecurityCoherence]
        //	            ,ac.[AccessApplication]
        //            FROM dbo.Users u
        //            INNER JOIN [dbo].[ApplicationAccess] ac ON ac.UserId = u.Id
        //            INNER JOIN [dbo].[Application] a ON a.Id = ac.ApplicationId
        //            INNER JOIN [dbo].[ApplicationGroup] ag ON ag.Id = a.GroupId

        //            WHERE u.Username = @username AND ac.ApplicationId = @applicationId";

        //            var reader = _provider.Read<ApplicationAccessEntity>(sql
        //                , _provider.CreateParameter("username", DbType.String, username)
        //                , _provider.CreateParameter("applicationId", DbType.Guid, application)
        //                );

        //            foreach (ApplicationAccessEntity item in reader)
        //                yield return item;


        //        }


        //        public List<ApplicationEntity> GetApplicationsAccessesForUser(Guid userId)
        //        {

        //        }

        //        public ApplicationEntity GetApplicationsAccessesForUserByName(Guid userId, string name)
        //        {

        //            string sql1 = @"
        //SELECT 
        //	ag.Id [ApplicationGroupId],
        //	ag.Name [ApplicationGroupName], 
        //	a.Id [ApplicationId],
        //	a.Name [ApplicationName],
        //	ac.AccessApplication [GroupApplicationAccess],
        //    a.SecurityCoherence [SecurityCoherenceApplication],
        //    a.SecurityCoherence [SecurityCoherence]
        //FROM ApplicationGroupAccess ac
        //LEFT JOIN ApplicationGroup ag ON ag.Id = ac.ApplicationGroupId
        //LEFT JOIN Users u ON u.Id = ac.UserId
        //INNER JOIN Application a ON ag.Id = a.GroupId

        //WHERE ac.AccessApplication > 0
        //	AND (u.Id = @userId OR ag.OwnerUserId = @userId)
        //    AND a.Name = @name
        //";

        //            var datas = _provider.Read<ApplicationEntity>(sql1,
        //                _provider.CreateParameter("userId", DbType.Guid, userId),
        //                _provider.CreateParameter("name", DbType.String, name)
        //                ).FirstOrDefault();

        //            return datas;

        //        }

      
    }




}
