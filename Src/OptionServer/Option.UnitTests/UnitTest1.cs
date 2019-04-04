using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Option.UnitTests
{

    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestSetServer()
        {

            string[] line = new string[]
            {
                "server",
                "http://localhost:5001"
            };

            Option.Program.Main(line);


            Bb.Option.Helper.Parameters = null;
            Bb.Option.Helper.Load();

            Assert.AreEqual(Bb.Option.Helper.Parameters.ServerUrl != null, true);

        }

        [TestMethod]
        public void TestCreateUser()
        {

            string username1 = "user" + Guid.NewGuid()
                .ToString()
                .Replace("{", "")
                .Replace("}", "")
                .Split('-')[0]
                ;
            string pass1 = Guid.NewGuid().ToString();


            string username2 = "user" + Guid.NewGuid()
                .ToString()
                .Replace("{", "")
                .Replace("}", "")
                .Split('-')[0]
                ;

            string pass2 = Guid.NewGuid().ToString();

            Option.Program.Main(new string[] { "server", "https://localhost:5001" });
            Option.Program.Main(new string[] { "user", "add", username1, pass1, "pseudo1", "email@yopmail.com" });
            Option.Program.Main(new string[] { "user", "add", username2, pass2, "pseudo1", "email@yopmail.com" });
            Option.Program.Main(new string[] { "user", "connect", username1, pass1 });

            Option.Program.Main(new string[] { "group", "add", "groupPar1" });
            Option.Program.Main(new string[] { "group", "list" });

            Option.Program.Main(new string[] { "group", "connect", "groupPar1" });

            Bb.Option.Helper.Parameters = null;
            Bb.Option.Helper.Load();
            Assert.AreEqual(Bb.Option.Helper.Parameters.Token != null, true);




        }

    }
}
