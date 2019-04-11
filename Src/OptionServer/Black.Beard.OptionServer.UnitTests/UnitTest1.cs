using Bb;
using Bb.OptionServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;
using System.Linq;

namespace Black.Beard.OptionServer.UnitTests
{
    [TestClass]
    public class UnitTest1
    {

        static UnitTest1()
        {
            _name = System.Data.SqlClient.SqlClientFactory.Instance.GetType().Name;
            DbProviderFactories.RegisterFactory(_name, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        [TestMethod]
        public void TestSaveUser()
        {

            var manager = GetManager();

            var userRepository = new UserRepository(manager);
            var user = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Username = $"user" + Guid.NewGuid().ToString(),
                Email = "toto@test.fr",
                Pseudo = "titi",
                HashPassword = "toto"
            };

            var test = userRepository.Save(user);

            Assert.AreEqual(userRepository.Read(user.Id) != null, true);

        }

        [TestMethod]
        public void TestSaveGroupApplication()
        {

            var manager = GetManager();

            var userRepository = new UserRepository(manager);
            var user = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Username = $"user" + Guid.NewGuid().ToString(),
                Email = "toto@test.fr",
                Pseudo = "titi",
                HashPassword = "toto"
            };
            userRepository.Save(user);

            var repGroup = new ApplicationGroupRepository(manager);
            string groupName = $"{user.Username}.group" + Guid.NewGuid().ToString();
            var g = repGroup.Create(user.Id, groupName);
            Assert.AreEqual(g, true);

            var groups = repGroup.GetAccessesByForUser(user.Id);

            Assert.AreEqual(groups.Count, 1);

            var user1 = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Username = $"user" + Guid.NewGuid().ToString(),
                Email = "toto@test.fr",
                Pseudo = "titi",
                HashPassword = "toto"
            };
            userRepository.Save(user1);

            var grp2 = repGroup.GetAccessByGoupApplicationName(user.Id, groupName).FirstOrDefault();
            Assert.AreEqual(grp2.ApplicationAccess, AccessEntityEnum.Admin);

            repGroup.SetAccess(user.Id, user1.Id, grp2.ApplicationGroupName, AccessEntityEnum.Add, AccessEntityEnum.Read, AccessEntityEnum.Read);
            var grp3 = repGroup.GetAccessByGoupApplicationName(user1.Id, groupName).FirstOrDefault();
            Assert.AreEqual(grp3.ApplicationAccess, AccessEntityEnum.Add);
            Assert.AreEqual(grp3.EnvironmentAccess, AccessEntityEnum.Read);

            repGroup.SetAccess(user.Id, user1.Id, grp2.ApplicationGroupName, AccessEntityEnum.Add, AccessEntityEnum.Add, AccessEntityEnum.Add);
            var grp4 = repGroup.GetAccessByGoupApplicationName(user1.Id, groupName).FirstOrDefault();
            Assert.AreEqual(grp4.EnvironmentAccess, AccessEntityEnum.Add);

            var repEnv = new EnvironmentRepository(manager);
            var e = new EnvironmentEntity()
            {
                Id = Guid.NewGuid(),
                GroupId = grp2.ApplicationGroupId,
                Name = "Debug1",
            };
            repEnv.Save(e);

            e.Name = "Debug";
            repEnv.Save(e);

            var repType = new TypeRepository(manager);
            var type = new TypeEntity()
            {
                Name = "fichier.json1",
                Extension = ".json",
                GroupId = grp2.ApplicationGroupId,
                Version = new Bb.Entities.TypeVersionEntity()
                {
                    Contract = "test",
                },
            };
            repType.Insert(type);

        }



        private static SqlManager GetManager()
        {

            var manager = new SqlManager(new SqlManagerConfiguration()
            {
                ProviderInvariantName = _name,
                ConnectionString = @"Data Source=L00280\SQLEXPRESS;Initial Catalog=Options;Integrated Security=True",
            });

            return manager;

        }

        private static readonly string _name;

    }
}
