using Bb.OptionServer.Entities;
using Bb.OptionServer.Repositories.Tables;
using System;
using System.Data;
using System.Linq;

namespace Bb.OptionServer.Repositories
{


    public class UserRepository : Repository<UsersTable>
    {

        public UserRepository(DtoSqlManager dtoManager) : base(dtoManager)
        {

        }

        public UsersTable Read(string username)
        {

            var users = _dtoManager.Select<UsersTable>(c => c.Username == username);

            foreach (UsersTable user in users)
                return user;

            return null;

        }

        public UsersTable ReadByPseudo(string pseudo)
        {

            var users = _dtoManager.Select<UsersTable>(c => c.Pseudo == pseudo);

            foreach (UsersTable user in users)
                return user;

            return null;

        }

        public void LoadGroupForUser(UserEntity user)
        {

            string sql1 = @"
        SELECT 

        	ag.Id [Id],
        	ag.Name [Name], 

            u2.[Pseudo] [OwnerPseudo],
            ag.[OwnerUserId] [OwnerUserId],
            u2.[Pseudo] [OwnerPseudo],

            aa.AccessApplication [ApplicationAccess],
        	aa.AccessEnvironment [EnvironmentAccess],
        	aa.AccessType [TypeAccess],

            aa.SecurityCoherence,

			a.Id AS [ApplicationId],
			a.Name AS [ApplicationName],
			a.SecurityCoherence AS [SecurityCoherenceApplication]

        FROM ApplicationGroupAccess aa
        INNER JOIN ApplicationGroup ag ON ag.Id = aa.ApplicationGroupId
        INNER JOIN Users u2 ON u2.Id = ag.OwnerUserId

		LEFT JOIN Application a ON a.GroupId = ag.Id

        WHERE (aa.UserId = @userId OR ag.OwnerUserId = @userId)

        ";

            var sql = Dto.Sql;

            var arguserId = sql.CreateParameter("userId", DbType.Guid, user.Id);
            var datas = sql.Read<ApplicationUserGroupEntity>(sql1, arguserId).ToList();

            foreach (ApplicationUserGroupEntity item in datas)
            {

                if (!user.OwnerAccess.TryGetValue(item.OwnerPseudo, out OwnerEntity o))
                    user.OwnerAccess.Add(item.OwnerPseudo, o = new OwnerEntity()
                    {
                        Id = item.OwnerUserId.Value,
                        Pseudo = item.OwnerPseudo
                    });

                if (!o.Groups.TryGetValue(item.Name, out GroupEntity g))
                {

                    o.Groups.Add(item.Name, g = new GroupEntity(o)
                    {
                        GroupId = item.Id.Value,
                        GroupName = item.Name,
                        ApplicationAccess = item.ApplicationAccess,
                        EnvironmentAccess = item.EnvironmentAccess,
                        TypeAccess = item.TypeAccess,
                        SecurityCoherence = item.SecurityCoherence,
                        IsOwner = user.Id == o.Id,
                    });

                    user.Append(g);
                }
          
                if (item.ApplicationId.HasValue && item.ApplicationId != Guid.Empty)
                {

                    if (!g.Applications.TryGetValue(item.ApplicationName, out ApplicationEntity a))
                    {

                        g.Applications.Add(item.ApplicationName, a = new ApplicationEntity()
                        {
                            Group = g,
                            Id = item.ApplicationId.Value,
                            Name = item.ApplicationName,
                            SecurityCoherence = item.SecurityCoherenceApplication.Value
                        });

                        user.Append(a);
                    }

                }

            }


        }




        public bool IsEmpty()
        {

            if (!_isNotEmpty)
            {

                string sql = @"SELECT COUNT(Id) FROM [Users]";
                var value = (int)_dtoManager.Sql.ReadScalar(sql);
                if (value > 0)
                    _isNotEmpty = true;

            }

            return !_isNotEmpty;

        }

        private static bool _isNotEmpty = false;

    }


    //public class UserRepository // : Repository<UserEntity>
    //{

    //    public UserRepository(DtoSqlManager dtoManager)
    //    {
    //        _dtoManager = dtoManager;

    //    }

    //    public bool Save(UsersTable user)
    //    {

    //        //DbUpdateContext ctx = new DbUpdateContext()
    //        //{
    //        //    Factory = _provider.Factory,
    //        //};

    //        //user.GenerateSave(ctx);

    //        var u = Read(user.Id);
    //        if (u == null)
    //        {
    //            string sql = @"INSERT INTO [dbo].[Users] ([Id], [Username], [Pseudo], [Email], [HashPassword], [AccessProfile], [LastUpdate], [SecurityCoherence]) VALUES (@id, @username, @pseudo, @email, @hashPassword, @accessProfile, CURRENT_TIMESTAMP, @securityCoherence)";
    //            return _provider.Update(sql, ctx.Items.ToArray());
    //        }
    //        else
    //        {
    //            ctx.Items.Add(_provider.CreateParameter("newSecurityCoherence", DbType.Guid, Guid.NewGuid()));
    //            string sql = @"UPDATE [Users] SET [Username] = @username, [Pseudo] = @pseudo, [Email] = @email, [HashPassword] = @hashPassword, [AccessProfile]=@accessProfile, [LastUpdate]= CURRENT_TIMESTAMP, [SecurityCoherence]=@newSecurityCoherence WHERE [Id] = @id AND [SecurityCoherence]=@securityCoherence";
    //            return _provider.Update(sql, ctx.Items.ToArray());
    //        }

    //    }

    //    public bool Save(UserEntity user)
    //    {

    //        DbUpdateContext ctx = new DbUpdateContext()
    //        {
    //            Factory = _provider.Factory,
    //        };
    //        user.GenerateSave(ctx);

    //        var u = Read(user.Id);
    //        if (u == null)
    //        {
    //            string sql = @"INSERT INTO [dbo].[Users] ([Id], [Username], [Pseudo], [Email], [HashPassword], [AccessProfile], [LastUpdate], [SecurityCoherence]) VALUES (@id, @username, @pseudo, @email, @hashPassword, @accessProfile, CURRENT_TIMESTAMP, @securityCoherence)";
    //            return _provider.Update(sql, ctx.Items.ToArray());
    //        }
    //        else
    //        {
    //            ctx.Items.Add(_provider.CreateParameter("newSecurityCoherence", DbType.Guid, Guid.NewGuid()));
    //            string sql = @"UPDATE [Users] SET [Username] = @username, [Pseudo] = @pseudo, [Email] = @email, [HashPassword] = @hashPassword, [AccessProfile]=@accessProfile, [LastUpdate]= CURRENT_TIMESTAMP, [SecurityCoherence]=@newSecurityCoherence WHERE [Id] = @id AND [SecurityCoherence]=@securityCoherence";
    //            return _provider.Update(sql, ctx.Items.ToArray());
    //        }

    //    }

    //    public UserEntity Read(Guid id)
    //    {

    //        string sql = @"SELECT [Id], [Username], [Pseudo], [Email], [HashPassword], [AccessProfile], [LastUpdate], [SecurityCoherence] FROM [Users] WHERE [Id] = @id";

    //        var reader = _provider.Read<UserEntity>(sql, _provider.CreateParameter("id", DbType.Guid, id));
    //        foreach (UserEntity user in reader)
    //            return user;

    //        return null;

    //    }

    //    public UserEntity Read(string username)
    //    {

    //        string sql = @"SELECT [Id], [Username], [Pseudo], [Email], [HashPassword], [AccessProfile], [LastUpdate], [SecurityCoherence] FROM [Users] WHERE [Username] = @username";

    //        var reader = _provider.Read<UserEntity>(sql, _provider.CreateParameter("username", DbType.String, username));
    //        foreach (UserEntity user in reader)
    //            return user;

    //        return null;

    //    }


    //    public bool IsEmpty()
    //    {

    //        if (!_isNotEmpty)
    //        {

    //            string sql = @"SELECT COUNT(Id) FROM [Users]";
    //            var value = (int)_provider.ReadScalar(sql);
    //            if (value > 0)
    //                _isNotEmpty = true;

    //        }

    //        return !_isNotEmpty;

    //    }


    //    private static bool _isNotEmpty = false;
    //    private readonly DtoSqlManager _dtoManager;
    //    private readonly SqlManager _provider;

    //}

}
