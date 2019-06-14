using System;

namespace Bb.OptionServer.Repositories
{

    public class Repository<T>
        where T : IPrimaryTable, new()
    {

        public Repository(DtoSqlManager dtoManager)
        {
            _dtoManager = dtoManager;
        }

        public DtoSqlManager Dto => _dtoManager;

        public T Read(Guid id)
        {

            var users = _dtoManager.Select<T>(c => c.Id == id);

            foreach (T user in users)
                return user;

            return default(T);

        }

        public bool Insert(T item, bool reload = false)
        {

            bool result;

            result = _dtoManager.Insert(item);

            if (reload)
                MapExtension.RemapFrom(item, Read(item.Id)).Reset();

            return result;

        }

        public bool Update(T item, bool reload = false)
        {

            bool result;

            result = _dtoManager.Update(item);

            if (reload)
                MapExtension.RemapFrom(item, Read(item.Id)).Reset();

            return result;

        }

        public bool Remove(T item)
        {
            bool result;
            result = _dtoManager.Remove(item);
            return result;
        }


        //public bool Save(T item, bool reload = false)
        //{

        //    bool result;
        //    var u = Read(item.Id);

        //    if (u == null)
        //    {

        //        result = _dtoManager.Insert(item);

        //        if (reload)
        //            MapExtension.RemapFrom(item, Read(item.Id)).Reset();

        //        return result;
        //    }
        //    else
        //    {

        //        result = _dtoManager.Update(item);

        //        if (reload)
        //            MapExtension.RemapFrom(item, Read(item.Id)).Reset();

        //    }

        //    return result;

        //}

        protected readonly DtoSqlManager _dtoManager;

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
