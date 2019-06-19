using Bb.OptionServer;
using Bb.OptionServer.Repositories;
using Bb.OptionServer.Repositories.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Black.Beard.OptionServer.UnitTests
{
    [TestClass]
    public class UserSqlServerUnitTest
    {

        [TestMethod]
        public void TestInsertdUpdateAnUser()
        {

            UserRepository rep = new UserRepository(SqlServerDataConnector.GetSqlServerManager());

            var user = new UsersTable();
            user.Id.Value = Guid.NewGuid();
            user.Username.Value = Guid.NewGuid().ToString();
            user.Email.Value = Guid.NewGuid().ToString() + "@yopmail.com";
            user.Pseudo.Value = Guid.NewGuid().ToString();
            user.AccessProfile.Value = 0;
            //SecurityCoherence = Guid.NewGuid(),
            user.HashPassword.Value = "toto";

            rep.Insert(user);

            var items = user.Changed().ToList();

            Assert.AreEqual(items.Any(), false);

            user.Email.Value = Guid.NewGuid().ToString() + "@yopmail.com";

            rep.Update(user);

            var userId = (Guid)user.Id;
            var userBis = rep.Read((Guid)user.Id);

            Assert.AreEqual(user.Id.Value, userBis.Id.Value);

        }




        //[TestMethod]
        //public void TestSaveUser()
        //{

        //    var manager = SqlServerDataConnector.GetManager();

        //    var userRepository = new UserRepository(manager);
        //    var user = new UserEntity()
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = $"user" + Guid.NewGuid().ToString(),
        //        Email = "toto@test.fr",
        //        Pseudo = "titi",
        //        HashPassword = "toto"
        //    };

        //    var test = userRepository.Save(user);

        //    Assert.AreEqual(userRepository.Read(user.Id) != null, true);

        //}

        //[TestMethod]
        //public void TestSaveGroupApplication()
        //{

        //    var manager = SqlServerDataConnector.GetManager();

        //    var userRepository = new UserRepository(manager);
        //    var user = new UserEntity()
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = $"user" + Guid.NewGuid().ToString(),
        //        Email = "toto@test.fr",
        //        Pseudo = "titi",
        //        HashPassword = "toto"
        //    };
        //    userRepository.Save(user);

        //    var repGroup = new ApplicationGroupRepository(manager);
        //    string groupName = $"{user.Username}.group" + Guid.NewGuid().ToString();
        //    var g = repGroup.Create(user.Id, groupName);
        //    Assert.AreEqual(g, true);

        //    var groups = repGroup.GetAccessesByForUser(user.Id);

        //    Assert.AreEqual(groups.Count, 1);

        //    var user1 = new UserEntity()
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = $"user" + Guid.NewGuid().ToString(),
        //        Email = "toto@test.fr",
        //        Pseudo = "titi",
        //        HashPassword = "toto"
        //    };
        //    userRepository.Save(user1);

        //    var grp2 = repGroup.GetAccessByGoupApplicationName(user.Id, groupName).FirstOrDefault();
        //    Assert.AreEqual(grp2.ApplicationAccess, AccessEntityEnum.Owner);

        //    repGroup.SetAccess(user.Id, user1.Id, grp2.ApplicationGroupName, AccessEntityEnum.Add, AccessEntityEnum.Read, AccessEntityEnum.Read);
        //    var grp3 = repGroup.GetAccessByGoupApplicationName(user1.Id, groupName).FirstOrDefault();
        //    Assert.AreEqual(grp3.ApplicationAccess, AccessEntityEnum.Add);
        //    Assert.AreEqual(grp3.EnvironmentAccess, AccessEntityEnum.Read);

        //    repGroup.SetAccess(user.Id, user1.Id, grp2.ApplicationGroupName, AccessEntityEnum.Add, AccessEntityEnum.Add, AccessEntityEnum.Add);
        //    var grp4 = repGroup.GetAccessByGoupApplicationName(user1.Id, groupName).FirstOrDefault();
        //    Assert.AreEqual(grp4.EnvironmentAccess, AccessEntityEnum.Add);

        //    var repEnv = new EnvironmentRepository(manager);
        //    var e = new EnvironmentEntity()
        //    {
        //        Id = Guid.NewGuid(),
        //        GroupId = grp2.ApplicationGroupId,
        //        Name = "Debug1",
        //    };
        //    repEnv.Save(e);

        //    e.Name = "Debug";
        //    repEnv.Save(e);

        //    var repType = new TypeRepository(manager);
        //    var type = new TypeEntity()
        //    {
        //        Name = "fichier.json1",
        //        Extension = ".json",
        //        GroupId = grp2.ApplicationGroupId,
        //        Version = new Bb.Entities.TypeVersionEntity()
        //        {
        //            Contract = "test",
        //        },
        //    };
        //    repType.Insert(type);

        //}


    }

}
