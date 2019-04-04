using Bb.Option;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Option
{

    public partial class Program
    {

        public static void Main(string[] args)
        {

            Helper.Load();

            var app = new CommandLineApplication();

            AdduserCommand(app);
            AdServerCommand(app);
            //AddConnectuserCommand(app);
            AddGroupCommand(app);

            app.HelpOption("-? | -h | --help");
            app.Execute(args);

            Helper.Save();

        }


        public static BbClientHttp Client => new BbClientHttp(new Uri(Helper.Parameters.ServerUrl));


        private static int Error(string message, CommandArgument arg)
        {
            Console.WriteLine(string.Format(message, arg.Name));
            return 1;
        }

    }
}
