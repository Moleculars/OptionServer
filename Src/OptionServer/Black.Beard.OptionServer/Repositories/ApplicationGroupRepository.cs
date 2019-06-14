using Bb.OptionServer.Repositories.Tables;
using System;
using System.Linq;

namespace Bb.OptionServer.Repositories
{


    public class ApplicationGroupRepository : Repository<ApplicationGroupTable>
    {

        public ApplicationGroupRepository(DtoSqlManager dtoManager) : base(dtoManager)
        {

        }

        public ApplicationGroupTable GetByName(Guid userId, string groupName)
        {
            var datas = _dtoManager.Select<ApplicationGroupTable>(c => c.Name == groupName && c.OwnerUserId == userId).FirstOrDefault();
            return datas;
        }

        //public List<ApplicationGroupAccessEntity> GetGroupWithAccessForUser(Guid userId)
        //{

        //    string sql1 = @"
        //SELECT 

        //	ag.Id [Id],
        //	ag.Name [Name], 

        //    ag.[OwnerUserId] [OwnerUserId],
        //    u2.[Pseudo] [OwnerPseudo],

        //    aa.[UserId] [UserId],
        //    u.[Pseudo] [Pseudo],
            
        //    aa.AccessApplication [ApplicationAccess],
        //	aa.AccessEnvironment [EnvironmentAccess],
        //	aa.AccessType [TypeAccess],

        //    aa.SecurityCoherence

        //FROM ApplicationGroupAccess aa
        //LEFT JOIN ApplicationGroup ag ON ag.Id = aa.ApplicationGroupId
        //LEFT JOIN Users u2 ON u2.Id = ag.OwnerUserId
        //LEFT JOIN Users u ON u.Id = aa.UserId

        //WHERE (u.Id = @userId OR ag.OwnerUserId = @userId)
        //";

        //    var sql = Dto.Sql;

        //    var arguserId = sql.CreateParameter("userId", DbType.Guid, userId);
        //    var datas = sql.Read<ApplicationGroupAccessEntity>(sql1, arguserId).ToList();
        //    return datas;

        //}



        //        public void SetAccess(Guid ownerId, Guid toUserId, string fullgroupName, AccessEntityEnum accessApplication, AccessEntityEnum accessType, AccessEntityEnum accessEnvironment)
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
        //INSERT INTO [dbo].[ApplicationGroupAccess] ([ApplicationGroupId], [UserId], [AccessApplication], [AccessType], [AccessEnvironment], [LastUpdate] ,[SecurityCoherence])
        //VALUES (@applicationGroupId, @userId, @accessApplication, @accessType, @accessEnvironment, CURRENT_TIMESTAMP, @securityCoherence)
        //";

        //                result = _provider.Update(sql,
        //                    _provider.CreateParameter("applicationGroupId", DbType.Guid, group.Id),
        //                    _provider.CreateParameter("userId", DbType.Guid, toUserId),
        //                    _provider.CreateParameter("accessApplication", DbType.Int32, accessApplication),
        //                    _provider.CreateParameter("accessType", DbType.Int32, accessType),
        //                    _provider.CreateParameter("accessEnvironment", DbType.Int32, accessEnvironment),
        //                    _provider.CreateParameter("securityCoherence", DbType.Guid, Guid.NewGuid())

        //                );


        //            }
        //            else if (item.ApplicationAccess != accessApplication || item.EnvironmentAccess != accessEnvironment || item.TypeAccess != accessType)
        //            {

        //                string sql = @"
        //UPDATE [dbo].[ApplicationGroupAccess] 
        //SET	
        //    [AccessApplication] = @accessApplication, [AccessType] = @accessType, [AccessEnvironment] = @accessEnvironment, [LastUpdate] = CURRENT_TIMESTAMP, [SecurityCoherence] = @newSecurityCoherence
        //WHERE [SecurityCoherence] = @securityCoherence AND [ApplicationGroupId] = @applicationGroupId AND [UserId]=@userId
        //";

        //                result = _provider.Update(sql,
        //                    _provider.CreateParameter("applicationGroupId", DbType.Guid, group.Id),
        //                    _provider.CreateParameter("userId", DbType.Guid, toUserId),
        //                    _provider.CreateParameter("accessApplication", DbType.Int32, accessApplication),
        //                    _provider.CreateParameter("accessType", DbType.Int32, accessType),
        //                    _provider.CreateParameter("accessEnvironment", DbType.Int32, accessEnvironment),
        //                    _provider.CreateParameter("securityCoherence", DbType.Guid, item.SecurityCoherence),
        //                    _provider.CreateParameter("newSecurityCoherence", DbType.Guid, Guid.NewGuid())
        //                );
        //            }

        //        }

        //        public List<ApplicationGroupAccessEntity> GetAccessesByForUser(Guid userId)
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
        //WHERE u.Id = @userId OR ag.OwnerUserId = @userId";

        //            var argUser = _provider.CreateParameter("userId", DbType.Guid, userId);
        //            var datas = _provider.Read<ApplicationGroupAccessEntity>(sql1, argUser).ToList();

        //            return datas;

        //        }

        //public bool Save(UserEntity user)
        //{
        //    DbUpdateContext ctx = new DbUpdateContext()
        //    {
        //        Factory = _provider.Factory,
        //    };
        //    user.GenerateSave(ctx);
        //    var u = Read(user.Id);
        //    if (u == null)
        //    {
        //        string sql = @"INSERT INTO [dbo].[Users] ([Id], [Username], [Pseudo], [Email], [HashPassword], [LastUpdate], [SecurityCoherence]) VALUES (@id, @username, @pseudo, @email, @hashPassword, CURRENT_TIMESTAMP, NEWID())";
        //        return _provider.Update(sql, user, ctx.Items);
        //    }
        //    else
        //    {
        //        string sql = @"UPDATE [Users] ([Username] = @username, [Pseudo] = @pseudo, [Email] = @email, [HashPassword] = @hashPassword, [LastUpdate]= CURRENT_TIMESTAMP, [SecurityCoherence]=NEWID() WHERE [Id] = @id AND [SecurityCoherence]=@securityCoherence";
        //        return _provider.Update(sql, user, ctx.Items);
        //    }
        //}

    }


    //    public class ApplicationGroupRepository //: Repository<UserEntity>
    //    {

    //        public ApplicationGroupRepository(SqlManager provider)
    //        {
    //            _provider = provider;
    //        }

    //        public bool Create(Guid userId, string groupName)
    //        {

    //            string sql1 = "INSERT INTO [dbo].[ApplicationGroup] ([Id], [Name], [OwnerUserId], [LastUpdate], [SecurityCoherence]) VALUES (@id, @name, @ownerUserId, CURRENT_TIMESTAMP, @securityCoherence)";
    //            string sql2 = "INSERT INTO [dbo].[ApplicationGroupAccess] ([ApplicationGroupId], [UserId], [AccessApplication], [AccessEnvironment], [AccessType], [LastUpdate], [SecurityCoherence]) VALUES (@applicationGroupId, @userId, @accessApplication, @accessEnvironment, @accessType, CURRENT_TIMESTAMP, @securityCoherence)";

    //            Guid applicationId = Guid.NewGuid();

    //            using (var transaction = _provider.GetTransaction())
    //            {

    //                var result = _provider.Update(sql1,
    //                    _provider.CreateParameter("id", DbType.Guid, applicationId),
    //                    _provider.CreateParameter("name", DbType.String, groupName),
    //                    _provider.CreateParameter("ownerUserId", DbType.Guid, userId),
    //                    _provider.CreateParameter("securityCoherence", DbType.Guid, Guid.NewGuid())
    //                    );

    //                if (result)
    //                {

    //                    result = _provider.Update(sql2,
    //                        _provider.CreateParameter("applicationGroupId", DbType.Guid, applicationId),
    //                        _provider.CreateParameter("userId", DbType.Guid, userId),
    //                        _provider.CreateParameter("accessApplication", DbType.Int32, AccessEntityEnum.Owner),
    //                        _provider.CreateParameter("accessEnvironment", DbType.Int32, AccessEntityEnum.Owner),
    //                        _provider.CreateParameter("accessType", DbType.Int32, AccessEntityEnum.Owner),
    //                        _provider.CreateParameter("securityCoherence", DbType.Guid, Guid.NewGuid())
    //                        );

    //                    transaction.Commit();

    //                    return result;

    //                }

    //            }

    //            return false;

    //        }

    //        public List<ApplicationGroupEntity> GetByname(string username, string groupName)
    //        {
    //            return GetByname($"{username}.{groupName}");
    //        }

    //        public List<ApplicationGroupEntity> GetByname(string fullgroupName)
    //        {
    //            string sql1 = @"SELECT ag.Id, ag.Name, ag.OwnerUserId, ag.SecurityCoherence FROM ApplicationGroup ag WHERE ag.Name = @name";
    //            var argName = _provider.CreateParameter("name", DbType.String, fullgroupName);
    //            var datas = _provider.Read<ApplicationGroupEntity>(sql1, argName).ToList();
    //            return datas;
    //        }

    //        public void SetAccess(Guid ownerId, Guid toUserId, string fullgroupName, AccessEntityEnum accessApplication, AccessEntityEnum accessType, AccessEntityEnum accessEnvironment)
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
    //INSERT INTO [dbo].[ApplicationGroupAccess] ([ApplicationGroupId], [UserId], [AccessApplication], [AccessType], [AccessEnvironment], [LastUpdate] ,[SecurityCoherence])
    //VALUES (@applicationGroupId, @userId, @accessApplication, @accessType, @accessEnvironment, CURRENT_TIMESTAMP, @securityCoherence)
    //";

    //                result = _provider.Update(sql,
    //                    _provider.CreateParameter("applicationGroupId", DbType.Guid, group.Id),
    //                    _provider.CreateParameter("userId", DbType.Guid, toUserId),
    //                    _provider.CreateParameter("accessApplication", DbType.Int32, accessApplication),
    //                    _provider.CreateParameter("accessType", DbType.Int32, accessType),
    //                    _provider.CreateParameter("accessEnvironment", DbType.Int32, accessEnvironment),
    //                    _provider.CreateParameter("securityCoherence", DbType.Guid, Guid.NewGuid())

    //                );


    //            }
    //            else if ( item.ApplicationAccess != accessApplication || item.EnvironmentAccess != accessEnvironment || item.TypeAccess != accessType )
    //            {

    //                                string sql = @"
    //UPDATE [dbo].[ApplicationGroupAccess] 
    //SET	
    //    [AccessApplication] = @accessApplication, [AccessType] = @accessType, [AccessEnvironment] = @accessEnvironment, [LastUpdate] = CURRENT_TIMESTAMP, [SecurityCoherence] = @newSecurityCoherence
    //WHERE [SecurityCoherence] = @securityCoherence AND [ApplicationGroupId] = @applicationGroupId AND [UserId]=@userId
    //";

    //                result = _provider.Update(sql,
    //                    _provider.CreateParameter("applicationGroupId", DbType.Guid, group.Id),
    //                    _provider.CreateParameter("userId", DbType.Guid, toUserId),
    //                    _provider.CreateParameter("accessApplication", DbType.Int32, accessApplication),
    //                    _provider.CreateParameter("accessType", DbType.Int32, accessType),
    //                    _provider.CreateParameter("accessEnvironment", DbType.Int32, accessEnvironment),
    //                    _provider.CreateParameter("securityCoherence", DbType.Guid, item.SecurityCoherence),
    //                    _provider.CreateParameter("newSecurityCoherence", DbType.Guid, Guid.NewGuid())
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

    //        public List<ApplicationGroupAccessEntity> GetAccessesByForUser(Guid userId)
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
    //WHERE u.Id = @userId OR ag.OwnerUserId = @userId";

    //            var argUser = _provider.CreateParameter("userId", DbType.Guid, userId);
    //            var datas = _provider.Read<ApplicationGroupAccessEntity>(sql1, argUser).ToList();

    //            return datas;

    //        }

    //        //public bool Save(UserEntity user)
    //        //{
    //        //    DbUpdateContext ctx = new DbUpdateContext()
    //        //    {
    //        //        Factory = _provider.Factory,
    //        //    };
    //        //    user.GenerateSave(ctx);
    //        //    var u = Read(user.Id);
    //        //    if (u == null)
    //        //    {
    //        //        string sql = @"INSERT INTO [dbo].[Users] ([Id], [Username], [Pseudo], [Email], [HashPassword], [LastUpdate], [SecurityCoherence]) VALUES (@id, @username, @pseudo, @email, @hashPassword, CURRENT_TIMESTAMP, NEWID())";
    //        //        return _provider.Update(sql, user, ctx.Items);
    //        //    }
    //        //    else
    //        //    {
    //        //        string sql = @"UPDATE [Users] ([Username] = @username, [Pseudo] = @pseudo, [Email] = @email, [HashPassword] = @hashPassword, [LastUpdate]= CURRENT_TIMESTAMP, [SecurityCoherence]=NEWID() WHERE [Id] = @id AND [SecurityCoherence]=@securityCoherence";
    //        //        return _provider.Update(sql, user, ctx.Items);
    //        //    }
    //        //}

    //        private readonly SqlManager _provider;

    //    }




}
