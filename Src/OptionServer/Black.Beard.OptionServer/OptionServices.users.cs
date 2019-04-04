using Bb.Exceptions;
using Bb.OptionServer;
using System;

namespace Bb
{

    public partial class OptionServices
    {

        private UserRepository Users => _users ?? (_users = new UserRepository(_manager));

        public UserEntity AddUser(string username, string password, string email, string pseudo)
        {

            var user = Users.Read(username);
            if (user != null)
                throw new AllreadyExistException(nameof(username));

            user = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Username = username,
                Pseudo = pseudo,
                Email = email,
                HashPassword = UserEntity.Hash(password),
            };

            if (Users.Save(user))
                user = Users.Read(username);

            else
                user = null;

            return user;

        }

        public UserEntity User(string username)
        {
            var user = Users.Read(username);
            return user;
        }


        private UserRepository _users;

    }
}
