using Bb.Option.Printings;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System.Collections.Generic;

namespace Bb.Option.Commands
{
    public static partial class Command
    {

        public static CommandLineApplication CommandApplication(this CommandLineApplication app)
        {

            var cmd = app.Command("appli", config =>
            {

                config.Description = "create, list, ... applications";
                config.HelpOption(HelpFlag);

            });

            cmd.Command("add", config =>
            {

                config.Description = "Add a new application in the current working appliation group";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                var argName = validator.Argument("applicationName",
                    "application name (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    , ValidatorExtension.EvaluateName
                    );

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return r;

                    if (validator.Evaluate() > 0)
                        return 2;

                    string applicationName = string.Empty;
                    var ar = argName.Value.Split('.');
                    if (ar.Length == 1)
                    {
                        if (string.IsNullOrEmpty(Helper.Parameters.WorkingGroup))
                        {
                            Output.ErrorWriteLine("no working group selected");
                            return 2;
                        }
                        applicationName = $"{Helper.Parameters.WorkingGroup}.{argName.Value}";
                    }

                    else if (ar.Length == 3)
                        applicationName = argName.Value;

                    var result = Client.Get<RootResultModel<ApplicationModel>>($"api/application/add/{applicationName}", GetToken());
                    result.Wait();

                    Output.WriteLine($"application '{argName.Value}' is added on group {result.Result.Datas.GroupName}.");

                    if (ar.Length == 3)
                        if (Helper.Parameters.WorkingGroup != $"{ar[0]}.{ar[1]}")
                        {
                            Helper.Parameters.WorkingGroup = $"{ar[0]}.{ar[1]}";
                            Output.WriteLine($"working group is setted on {Helper.Parameters.WorkingGroup}");
                        }

                    return 0;

                });
            });

            cmd.Command("list", config =>
            {

                config.Description = "list all applications for working group is setted";
                config.HelpOption(HelpFlag);

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return 2;

                    var result = Client.Get<RootResultModel<List<ApplicationModel>>>($"api/application/list/{Helper.Parameters.WorkingGroup}", GetToken());
                    result.Wait();

                    PrintDataExtensions.ClearBorder();
                    ConvertToDatatable
                        .ConvertList(result.Result.Datas, "applications")
                        .Print();

                    return 0;

                });
            });

            return app;

        }

    }
}
