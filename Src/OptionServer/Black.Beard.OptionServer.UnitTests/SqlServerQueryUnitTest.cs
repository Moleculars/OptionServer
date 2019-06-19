using Bb.OptionServer;
using Bb.OptionServer.Dao;
using Bb.OptionServer.Repositories.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Black.Beard.OptionServer.UnitTests
{

    [TestClass]
    public class SqlServerQueryUnitTest
    {

        public SqlServerQueryUnitTest()
        {
            _dto = SqlServerDataConnector.GetSqlServerManager();

        }

        [TestMethod]
        public void TestGenerateInsertCommand()
        {

            var user = SqlServerDataConnector.GetUserTable();

            ObjectMapping mapping = DtoSqlManager.GetMapping(typeof(UsersTable));

            QuerySqlCommand queryModel = ExtractQueryGenerator.Insert(user, mapping);

            Assert.AreEqual(queryModel.Fields.Count, 8);
            Assert.AreEqual(queryModel.FilterFields.Count, 0);

        }

        [TestMethod]
        public void TestGenerateInsertSqlCommand()
        {

            var user = SqlServerDataConnector.GetUserTable();

            ObjectMapping mapping = DtoSqlManager.GetMapping(typeof(UsersTable));
            QuerySqlCommand queryModel = ExtractQueryGenerator.Insert(user, mapping);

            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.Id)).Name, nameof(UsersTable.Id));
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.Pseudo)).Name, nameof(UsersTable.Pseudo));
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.Email)).Name, nameof(UsersTable.Email));
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.HashPassword)).Name, nameof(UsersTable.HashPassword));
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.Username)).Name, nameof(UsersTable.Username));
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.AccessProfile)).Name, nameof(UsersTable.AccessProfile));
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.LastUpdate)).Name, nameof(UsersTable.LastUpdate));
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.SecurityCoherence)).Name, nameof(UsersTable.SecurityCoherence));

            QueryCommand query = _dto.Generator.Generate(queryModel);
            string expectedSql = "INSERT INTO [dbo].[Users] ([Id], [Username], [Pseudo], [Email], [HashPassword], [LastUpdate], [SecurityCoherence], [AccessProfile]) VALUES (@id, @username, @pseudo, @email, @hashPassword, CURRENT_TIMESTAMP, @securityCoherence, @accessProfile)";

            Assert.AreEqual(query.CommandText.ToString(), expectedSql);

        }


        [TestMethod]
        public void TestGenerateDeleteCommand()
        {

            var user = SqlServerDataConnector.GetUserTable();

            ObjectMapping mapping = DtoSqlManager.GetMapping(typeof(UsersTable));

            QuerySqlCommand queryModel = ExtractQueryGenerator.Remove(user, mapping);

            Assert.AreEqual(queryModel.Fields.Count, 0);
            Assert.AreEqual(queryModel.FilterFields.Count, 2);

            Assert.AreEqual(queryModel.FilterFields.First(c => c.Name == nameof(UsersTable.Id)).Name, nameof(UsersTable.Id));
            Assert.AreEqual(queryModel.FilterFields.First(c => c.Name == nameof(UsersTable.SecurityCoherence)).Name, nameof(UsersTable.SecurityCoherence));

        }

        [TestMethod]
        public void TestGenerateDeleteSqlCommand()
        {

            var user = SqlServerDataConnector.GetUserTable();

            ObjectMapping mapping = DtoSqlManager.GetMapping(typeof(UsersTable));

            QuerySqlCommand queryModel = ExtractQueryGenerator.Remove(user, mapping);
            QueryCommand query = _dto.Generator.Generate(queryModel);

            string expectedSql = "DELETE FROM [dbo].[Users] WHERE [Id] = @id AND [SecurityCoherence] = @securityCoherence";
            Assert.AreEqual(query.CommandText.ToString(), expectedSql);

        }

        [TestMethod]
        public void TestGenerateUpdateCommand()
        {

            var user = SqlServerDataConnector.GetUserTable();

            ObjectMapping mapping = DtoSqlManager.GetMapping(typeof(UsersTable));

            QuerySqlCommand queryModel = ExtractQueryGenerator.Update(user, mapping);

            Assert.AreEqual(queryModel.Fields.Count, 0);
            Assert.AreEqual(queryModel.FilterFields.Count, 2);

            user.AccessProfile.Value = 2;
            queryModel = ExtractQueryGenerator.Update(user, mapping);

            Assert.AreEqual(queryModel.Fields.Count, 3);
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.AccessProfile)).Name, nameof(UsersTable.AccessProfile));
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.LastUpdate)).Name, nameof(UsersTable.LastUpdate));
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.SecurityCoherence)).Name, nameof(UsersTable.SecurityCoherence));
            Assert.AreEqual(queryModel.Fields.First(c => c.Name == nameof(UsersTable.SecurityCoherence)).VariableName, "new" + nameof(UsersTable.SecurityCoherence));

            Assert.AreEqual(queryModel.FilterFields.Count, 2);
            Assert.AreEqual(queryModel.FilterFields.First(c => c.Name == nameof(UsersTable.Id)).Name, nameof(UsersTable.Id));
            Assert.AreEqual(queryModel.FilterFields.First(c => c.Name == nameof(UsersTable.SecurityCoherence)).Name, nameof(UsersTable.SecurityCoherence));

        }

        [TestMethod]
        public void TestGenerateUpdateSqlCommand()
        {

            var user = SqlServerDataConnector.GetUserTable();
            user.AccessProfile.Value = 2;

            ObjectMapping mapping = DtoSqlManager.GetMapping(typeof(UsersTable));

            QuerySqlCommand queryModel = ExtractQueryGenerator.Update(user, mapping);

            QueryCommand query = _dto.Generator.Generate(queryModel);

            string expectedSql = "UPDATE [dbo].[Users] SET [AccessProfile] = @accessProfile, [LastUpdate] = CURRENT_TIMESTAMP, [SecurityCoherence] = @newSecurityCoherence WHERE [Id] = @id AND [SecurityCoherence] = @securityCoherence";

            Assert.AreEqual(query.CommandText.ToString(), expectedSql);


        }
        
        private readonly DtoSqlManager _dto;

    }

}
