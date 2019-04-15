using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.IO;

namespace Bb.Option.Validators
{


    public static class ValidatorExtension
    {


        public static int EvaluateGroupName()
        {

            if (string.IsNullOrEmpty(Helper.Parameters.WorkingGroup))
                return ValidatorExtension.Error($"working group must be setted. please considere use '{Constants.RootCommand} group set <groupName>'.");

            return 0;

        }

        public static int EvaluateFileExist(CommandOption option)
        {

            if (option.HasValue())
            {
                FileInfo file = new FileInfo( option.Value());
                if (!file.Exists)
                    return Error($"file '{file.FullName}' not found");
            }

            return 0;

        }

        public static int EvaluateAccessEnum(CommandOption option)
        {
            // Program._access = "('" + string.Join("','", Enum.GetNames(typeof(AccessModuleEnum))) + "')";

            if (option.HasValue())
            {

                var value = option
                    .Value()
                    .Trim()
                    .Trim('\'')
                    .Trim();

                if (!Enum.TryParse(typeof(AccessModuleEnum), value, out object e))
                {
                    return Error("{0} is unexpected", option);
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
            Output.ErrorWriteLine();
            Output.ErrorWriteLine(message);
            return 1;
        }

        public static int Error(string message, CommandArgument arg)
        {
            Output.ErrorWriteLine();
            Output.ErrorWriteLine(message, arg.Name);
            return 1;
        }

        public static int Error(string message, CommandOption arg)
        {
            Output.ErrorWriteLine();
            Output.ErrorWriteLine(message, arg.Template);



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
