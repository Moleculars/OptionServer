﻿using Bb.OptionServer;
using Bb.OptionServer.Entities;
using Bb.OptionServer.Repositories.Tables;
using System;
using System.Data.Common;

namespace Black.Beard.OptionServer.UnitTests
{
    internal class SqlServerDataConnector
    {

        static SqlServerDataConnector()
        {
            _name = System.Data.SqlClient.SqlClientFactory.Instance.GetType().Name;
            DbProviderFactories.RegisterFactory(_name, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        public static DtoSqlManager GetSqlServerManager()
        {

            var manager = new SqlManager(new SqlManagerConfiguration()
            {
                ProviderInvariantName = _name,
                ConnectionString = @"Data Source=L00280\SQLEXPRESS;Initial Catalog=Options;Integrated Security=True",
            });

            return new DtoSqlManager(new SqlServerQueryGenerator(manager));

        }

        public static UserEntity GetUserInfo()
        {

            var username = Guid.NewGuid().ToString().Substring(0, 8);

            var user = new UserEntity()
            {
                Username = $"user_{username}",
                Email = $"{username}@yopmail.com",
                Pseudo = $"{username}_Pseudo",
                HashPassword = $"{username}_password"
            };



            return user;

        }

        public static UsersTable GetUserTable()
        {
            var username = Guid.NewGuid().ToString().Substring(0, 8);
            var user = new UsersTable
            (
                Guid.NewGuid(), 
                $"user_{username}", 
                $"{username}_Pseudo", 
                $"{username}@yopmail.com", 
                "test", DateTimeOffset.Now, 
                Guid.NewGuid(), 0
            );
            return user;
        }


        private static readonly string _name;

    }

}
