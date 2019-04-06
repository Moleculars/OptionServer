using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Bb.Option.Validators
{


    public static class ValidatorExtension
    {


        public static int EvaluateAccessEnum(CommandOption command)
        {
            // Program._access = "('" + string.Join("','", Enum.GetNames(typeof(AccessModuleEnum))) + "')";

            if (command.HasValue())
            {

                var value = command
                    .Value()
                    .Trim()
                    .Trim('\'')
                    .Trim();

                if (!Enum.TryParse(typeof(AccessModuleEnum), value, out object e))
                {
                    return Error("{0} is unexpected", command);
                }
            }

            return 0;

        }

        public static int EvaluateRequired(CommandArgument command)
        {

            if (string.IsNullOrWhiteSpace(command.Value))
                return Error("{0} must be specified", command);

            return 0;

        }

        public static void Stop()
        {
            System.Diagnostics.Debugger.Launch();
            System.Diagnostics.Debugger.Break();
        }

        public static int Error(string message)
        {
            Console.Error.WriteLine("");
            Console.Error.WriteLine(message);
            return 1;
        }

        public static int Error(string message, CommandArgument arg)
        {
            Console.Error.WriteLine("");
            Console.Error.WriteLine(string.Format(message, arg.Name));
            return 1;
        }

        public static int Error(string message, CommandOption arg)
        {
            Console.Error.WriteLine("");
            Console.Error.WriteLine(string.Format(message, arg.Template));



            return 1;
        }

        internal static bool CheckToken()
        {
            
            if (string.IsNullOrEmpty(Helper.Parameters.Token))
                return false;

            return DateTime.Now < Helper.Parameters.TokenExpiration;

        }

    }
}
