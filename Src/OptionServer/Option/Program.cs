using Bb.Option;
using Bb.Option.Commands;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Option
{

    public partial class Program
    {
        public static int ExitCode { get; private set; }

        public static void Main(string[] args)
        {

            try
            {

                var app = new CommandLineApplication()
                    .Initialize()
                    .CommandServer()
                    .CommandUser()
                    .CommandGroup()
                    .CommandEnvironment()
                    .CommandType()
                ;

                int result = app.Execute(args);

                if (result == 0)
                    Helper.Save();

                else if (result == 1)
                    app.ShowHelp();

                Console.Out.Flush();
                Console.Error.Flush();

                Environment.ExitCode = Program.ExitCode = result;

            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);

                Console.Out.Flush();
                Console.Error.Flush();

                if (e.HResult > 0)
                    Environment.ExitCode = Program.ExitCode = e.HResult;

                Environment.ExitCode = Program.ExitCode = 1;

            }

        }


    }
}
