using Bb.Option;
using Bb.Option.Commands;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Option
{

    public partial class Program
    {

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
                ;

                int result = app.Execute(args);

                if (result == 0)
                    Helper.Save();

                else if (result == 1)
                    app.ShowHelp();

                Environment.ExitCode = result;

            }
            catch (Exception e)
            {
                
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);

                if (e.HResult > 0)
                    Environment.ExitCode = e.HResult;

                Environment.ExitCode = 1;

            }

        }

 
    }
}
