using Bb;
using Bb.OptionServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Black.Beard.OptionServer.UnitTests
{

    [TestClass]
    public class OptionServiceUnitTest
    {

        private readonly DtoSqlManager _dto;

        public OptionServiceUnitTest()
        {

            _dto = SqlServerDataConnector.GetSqlServerManager();

        }

        [TestMethod]
        public void CreateUser()
        {

            var manager = new OptionServices(_dto);
            var user = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(user.Username, user.HashPassword, user.Email, user.Pseudo);

            Assert.AreEqual(u != null, true);

        }

        [TestMethod]
        public void AuthenticateUser()
        {

            var manager = new OptionServices(_dto);
            var user = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(user.Username, user.HashPassword, user.Email, user.Pseudo);

            var u2 = manager.Authenticate(user.Username, user.HashPassword);

            Assert.AreEqual(u2 != null, true);

        }

        [TestMethod]
        public void CreateGroup()
        {

            var manager = new OptionServices(_dto);
            var user = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(user.Username, user.HashPassword, user.Email, user.Pseudo);
            var u2 = manager.User(user.Username);
            var group1 = manager.CreateGroupApplication(u2, "group1");

            Assert.AreEqual(group1 != null, true);

        }

        [TestMethod]
        public void SetAccess()
        {

            var manager = new OptionServices(_dto);
            var userOwnerExpected = SqlServerDataConnector.GetUserInfo();
            var userToGrantExpected = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(userOwnerExpected.Username, userOwnerExpected.HashPassword, userOwnerExpected.Email, userOwnerExpected.Pseudo);
            var userOwner = manager.User(userOwnerExpected.Username);
            userOwner = manager.CreateGroupApplication(userOwner, "group1");

            var group = userOwner.Groups("group1").First();


            var u2 = manager.AddUser(userToGrantExpected.Username, userToGrantExpected.HashPassword, userToGrantExpected.Email, userToGrantExpected.Pseudo);
            var userToGrant = manager.User(userToGrantExpected.Username);

            userToGrant = manager.SetAccess(userOwner, userToGrant, group.GroupName, AccessEntityEnum.Operator, AccessEntityEnum.Reader, AccessEntityEnum.Reader);

            var results = manager.SetAccess(userOwner, userToGrant, group.GroupName, AccessEntityEnum.Reader, AccessEntityEnum.Reader, AccessEntityEnum.Reader);
            var result = results.Groups("group1").First();

            Assert.AreEqual(result.Owner.Id, userOwner.Id);

        }

        [TestMethod]
        public void ListApplicationGroup()
        {

            var manager = new OptionServices(_dto);
            var userOwnerExpected = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(userOwnerExpected.Username, userOwnerExpected.HashPassword, userOwnerExpected.Email, userOwnerExpected.Pseudo);
            var userOwner = manager.User(userOwnerExpected.Username);
            userOwner = manager.CreateGroupApplication(userOwner, "group1");
            userOwner = manager.CreateGroupApplication(userOwner, "group2");

            Assert.AreEqual(userOwner.OwnerAccess.Count(), 1);

        }

        [TestMethod]
        public void AddEnvironment()
        {

            var manager = new OptionServices(_dto);
            var userOwnerExpected = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(userOwnerExpected.Username, userOwnerExpected.HashPassword, userOwnerExpected.Email, userOwnerExpected.Pseudo);
            var userOwner = manager.User(userOwnerExpected.Username);
            userOwner = manager.CreateGroupApplication(userOwner, "group1");

            manager.AddEnvironment(userOwner, "group1", "debug");
            Assert.AreEqual(userOwner.Groups("group1").First().GetEnvironment("debug") != null, true);

            manager.AddEnvironment(userOwner, "group1", "test");
            Assert.AreEqual(userOwner.Groups("group1").First().GetEnvironment("test") != null, true);

            userOwner = manager.User(userOwnerExpected.Username);
            manager.LoadEnvironments(userOwner, "group1");
            Assert.AreEqual(userOwner.Groups("group1").First().GetEnvironment("debug") != null, true);
            Assert.AreEqual(userOwner.Groups("group1").First().GetEnvironment("test") != null, true);

        }

        [TestMethod]
        public void AddType()
        {

            var manager = new OptionServices(_dto);
            var userOwnerExpected = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(userOwnerExpected.Username, userOwnerExpected.HashPassword, userOwnerExpected.Email, userOwnerExpected.Pseudo);
            var userOwner = manager.User(userOwnerExpected.Username);
            userOwner = manager.CreateGroupApplication(userOwner, "group1");

            var type = manager.AddType(userOwner, "group1", "type1", "json", string.Empty);

            Assert.AreEqual(type.Extension, ".json");
            Assert.AreEqual(type.TypeName, "type1");


        }

        [TestMethod]
        public void AddTypeAndChangeExtension()
        {

            var manager = new OptionServices(_dto);
            var userOwnerExpected = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(userOwnerExpected.Username, userOwnerExpected.HashPassword, userOwnerExpected.Email, userOwnerExpected.Pseudo);
            var userOwner = manager.User(userOwnerExpected.Username);
            userOwner = manager.CreateGroupApplication(userOwner, "group1");

            var type = manager.AddType(userOwner, "group1", "type1", "json", string.Empty);

            var type2 = manager.UpdateExtension(userOwner, "group1", "type1", "txt");

            type = manager.GetType(userOwner, "group1", "type1");

            Assert.AreEqual(type.Extension, ".txt");

        }

        [TestMethod]
        public void AddTypeAndChangeContract()
        {

            string contract = "{ \"Id\" : { \"_type\" : \"string\" } }";

            var manager = new OptionServices(_dto);
            var userOwnerExpected = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(userOwnerExpected.Username, userOwnerExpected.HashPassword, userOwnerExpected.Email, userOwnerExpected.Pseudo);
            var userOwner = manager.User(userOwnerExpected.Username);
            userOwner = manager.CreateGroupApplication(userOwner, "group1");

            var type = manager.AddType(userOwner, "group1", "type1", "json", string.Empty);

            var type2 = manager.UpdateContract(userOwner, "group1", "type1", contract);

            type = manager.GetType(userOwner, "group1", "type1");

            Assert.AreEqual(type.CurrentVersion.Contract, contract);

            manager.LoadTypesHistory(userOwner);
            Assert.AreEqual(type.Versions.Count, 2);

            var group = userOwner.Groups("group1").First();
            type = group.GetType("type1");
            Assert.AreEqual(type.Versions.Count, 2);
        }

        [TestMethod]
        public void AddApplication()
        {

            var manager = new OptionServices(_dto);
            var userOwnerExpected = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(userOwnerExpected.Username, userOwnerExpected.HashPassword, userOwnerExpected.Email, userOwnerExpected.Pseudo);
            var userOwner = manager.User(userOwnerExpected.Username);
            userOwner = manager.CreateGroupApplication(userOwner, "group1");

            var application = manager.AddApplication(userOwner, "group1", "application1");
            userOwner = manager.User(userOwnerExpected.Username);

            var _app = userOwner.Groups("group1").First().Applications["application1"];
            Assert.AreEqual(application.Name, _app.Name);
            Assert.AreEqual(application.Id, _app.Id);

            var app = userOwner.ResolveApplication("application1");
            Assert.AreEqual(app.Application.Infos.Id, _app.Id);
            Assert.AreEqual(app.Group.Infos.GroupId, _app.Group.GroupId);
            Assert.AreEqual(app.Owner.Infos.Id, _app.Group.Owner.Id);

            var app2 = userOwner.ResolveApplication("group1.application1");
            Assert.AreEqual(app2.Application.Infos.Id, _app.Id);
            Assert.AreEqual(app2.Group.Infos.GroupId, _app.Group.GroupId);
            Assert.AreEqual(app2.Owner.Infos.Id, _app.Group.Owner.Id);

            var app3 = userOwner.ResolveApplication(userOwner.Pseudo + ".group1.application1");
            Assert.AreEqual(app3.Application.Infos.Id, _app.Id);
            Assert.AreEqual(app3.Group.Infos.GroupId, _app.Group.GroupId);
            Assert.AreEqual(app3.Owner.Infos.Id, _app.Group.Owner.Id);

        }

        [TestMethod]
        public void ApplicationAddDocument()
        {

            var manager = new OptionServices(_dto);
            var userOwnerExpected = SqlServerDataConnector.GetUserInfo();

            var u = manager.AddUser(userOwnerExpected.Username, userOwnerExpected.HashPassword, userOwnerExpected.Email, userOwnerExpected.Pseudo);
            var userOwner = manager.User(userOwnerExpected.Username);
            userOwner = manager.CreateGroupApplication(userOwner, "group1");

            var type = manager.AddType(userOwner, "group1", "type1", "json", string.Empty);
            var environment = manager.AddEnvironment(userOwner, "group1", "debug");
            var application = manager.AddApplication(userOwner, "group1", "application1");

            var doc = manager.SetDocument(userOwner, "application1", "debug", "type1", "doc1", "{ }");

            manager.LoadDocuments(application, userOwner, environment, true);

            var docs = application.DocumentByName("doc1.json").FirstOrDefault();

            Assert.AreEqual(docs.ConfigurationName, "doc1.json");

            manager.DeleteDocument(userOwner, "application1", "doc1.json", docs.Version, docs.Environment.EnvironmentName);



        }


        //[TestMethod]
        //public void TestSaveGroupApplication()
        //{

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

        //[TestMethod]
        //public void TestInsertdUpdateAnUser()
        //{

        //    var manager = SqlServerDataConnector.GetManager();
        //    DtoSqlManager dtoManager = new DtoSqlManager(new SqlServerQueryGenerator(manager));

        //    var user = new UsersTable()
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = Guid.NewGuid().ToString(),
        //        Email = Guid.NewGuid().ToString() + "@yopmail.com",
        //        Pseudo = Guid.NewGuid().ToString(),
        //        AccessProfile = 0,
        //        //SecurityCoherence = Guid.NewGuid(),
        //        HashPassword = "toto",
        //    };

        //    dtoManager.Insert(user);

        //    var items = user.IsChanged().ToList();

        //    Assert.AreEqual(items.Any(), false);

        //    user.Email = Guid.NewGuid().ToString() + "@yopmail.com";

        //    dtoManager.Update(user);

        //    var userId = (Guid)user.Id;
        //    var userBis = dtoManager.Select<UsersTable>(c => c.Id == (Guid)user.Id).FirstOrDefault();

        //    Assert.AreEqual(user.Id, userBis.Id);

        //}

    }

}
