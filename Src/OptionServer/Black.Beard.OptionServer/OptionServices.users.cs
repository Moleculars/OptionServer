using Bb.OptionServer.Entities;
using Bb.Security.Jwt;
using Bb.OptionServer.Repositories.Tables;
using System;
using System.Security.Authentication;
using Bb.OptionServer.Exceptions;

namespace Bb.OptionServer
{

    public partial class OptionServices
    {

        public UsersTable AddUser(string username, string password, string email, string pseudo)
        {

            var user = Users.Read(username);
            if (user != null)
                throw new AllreadyExistException(nameof(username));

            user = new UsersTable();
            user.Id.Value = Guid.NewGuid();
            user.Username.Value = username;
            user.Pseudo.Value = pseudo;
            user.Email.Value = email;
            user.HashPassword.Value = Sha.HashPassword(password);

            user.AccessProfile.Value = (int)(_users.IsEmpty()
                ? UserProfileEnum.Administrator
                : UserProfileEnum.Classical);


            if (Users.Insert(user))
                user = Users.Read(username);

            else
                user = null;

            return user;

        }

        public UserEntity User(Guid userId)
        {
            var user = Users.Read(userId);

            if (user == null)
                throw new MissingUserException(userId.ToString());

            var _user = new UserEntity()
            {
                Id = user.Id,
                Email = user.Email,
                Pseudo = user.Pseudo,
                Username = user.Username,
                LastUpdate = user.LastUpdate,
                HashPassword = user.HashPassword,
                SecurityCoherence = user.SecurityCoherence,
                AccessProfile = (UserProfileEnum)user.AccessProfile.Value,
            };

            Users.LoadGroupForUser(_user);

            return _user;

        }

        public object SetDocument(UserEntity user, ApplicationEntity applicationEntity, object environmentName, object typeName, string name, object content)
        {
            throw new NotImplementedException();
        }

        public UserEntity User(string username)
        {
            var user = Users.Read(username);

            if (user == null)
                throw new MissingUserException(username);

            var _user = new UserEntity()
            {
                Id = user.Id,
                Email = user.Email,
                Pseudo = user.Pseudo,
                Username = user.Username,
                LastUpdate = user.LastUpdate,
                HashPassword = user.HashPassword,
                SecurityCoherence = user.SecurityCoherence,
                AccessProfile = (UserProfileEnum)user.AccessProfile.Value,
            };

            Users.LoadGroupForUser(_user);

            return _user;

        }

        public UserEntity Authenticate(string username, string password)
        {

            UserEntity auth = User(username);

            if (auth == null)
                throw new AuthenticationException();

            var hash = Sha.HashPassword(password);
            if (auth.HashPassword != hash)
                throw new AuthenticationException();

            return auth;

        }

        public string GetToken(UserEntity user, JwtTokenConfiguration _tokenConfiguration)
        {

            string token = new JwtTokenManager(_tokenConfiguration)
                                .AddMail(user.Email)
                                .AddPseudo(user.Pseudo)
                                .AddSubject(user.Username)
                                .AddExpiry(60)
                                .Build();

            return token;

        }

    }
}
