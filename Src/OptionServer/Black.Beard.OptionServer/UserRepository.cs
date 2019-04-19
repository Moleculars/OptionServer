using System;
using System.Data;

namespace Bb.OptionServer
{

    public class UserRepository // : Repository<UserEntity>
    {

        public UserRepository(SqlManager provider)
        {
            _provider = provider;
        }

        public bool Save(UserEntity user)
        {

            DbUpdateContext ctx = new DbUpdateContext()
            {
                Factory = _provider.Factory,
            };
            user.GenerateSave(ctx);

            var u = Read(user.Id);
            if (u == null)
            {
                string sql = @"INSERT INTO [dbo].[Users] ([Id], [Username], [Pseudo], [Email], [HashPassword], [AccessProfile], [LastUpdate], [SecurityCoherence]) VALUES (@id, @username, @pseudo, @email, @hashPassword, @accessProfile, CURRENT_TIMESTAMP, NEWID())";
                return _provider.Update(sql, ctx.Items.ToArray());
            }
            else
            {
                string sql = @"UPDATE [Users] SET [Username] = @username, [Pseudo] = @pseudo, [Email] = @email, [HashPassword] = @hashPassword, [AccessProfile]=@accessProfile, [LastUpdate]= CURRENT_TIMESTAMP, [SecurityCoherence]=NEWID() WHERE [Id] = @id AND [SecurityCoherence]=@securityCoherence";
                return _provider.Update(sql, ctx.Items.ToArray());
            }

        }

        public UserEntity Read(Guid id)
        {

            string sql = @"SELECT [Id], [Username], [Pseudo], [Email], [HashPassword], [AccessProfile], [LastUpdate], [SecurityCoherence] FROM [Users] WHERE [Id] = @id";

            var reader = _provider.Read<UserEntity>(sql, _provider.CreateParameter("id", DbType.Guid, id));
            foreach (UserEntity user in reader)
                return user;

            return null;

        }

        public UserEntity Read(string username)
        {

            string sql = @"SELECT [Id], [Username], [Pseudo], [Email], [HashPassword], [AccessProfile], [LastUpdate], [SecurityCoherence] FROM [Users] WHERE [Username] = @username";

            var reader = _provider.Read<UserEntity>(sql, _provider.CreateParameter("username", DbType.String, username));
            foreach (UserEntity user in reader)
                return user;

            return null;

        }


        public bool IsEmpty()
        {

            if (!_isNotEmpty)
            {

                string sql = @"SELECT COUNT(Id) FROM [Users]";
                var value = (int)_provider.ReadScalar(sql);
                if (value > 0)
                    _isNotEmpty = true;

            }

            return ! _isNotEmpty;

        }


        private static bool _isNotEmpty = false;
        private readonly SqlManager _provider;

    }
}
