using Bb.Option;
using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Option
{

    public partial class Program
    {

        public static void Main(string[] args)
        {

            Program._access = "('" + string.Join("','", Enum.GetNames(typeof(AccessModuleEnum))) + "')";

            Helper.Load();

            var app = new CommandLineApplication();

            AdduserCommand(app);
            AdServerCommand(app);
            //AddConnectuserCommand(app);
            AddGroupCommand(app);

            app.HelpOption(HelpFlag);
            app.Execute(args);

            Helper.Save();

        }


        public static BbClientHttp Client => new BbClientHttp(new Uri(Helper.Parameters.ServerUrl));


        private static int Error(string message, CommandArgument arg)
        {
            Console.WriteLine(string.Format(message, arg.Name));
            return 1;
        }

        private const string HelpFlag = "-? |-h |--help";
        public static string _access;
    }
}
