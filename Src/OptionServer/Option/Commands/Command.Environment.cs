using Bb.Option.Printings;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;

namespace Bb.Option.Commands
{
    public static partial class Command
    {

        public static CommandLineApplication CommandEnvironment(this CommandLineApplication app)
        {

            var cmd = app.Command("env", config =>
            {

                config.Description = "create, list, ... environment";
                config.HelpOption(HelpFlag);

            });

            cmd.Command("add", config =>
            {

                config.Description = "Add a new environment";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                var argenvironmentName = validator.Argument("environmentName",
                    "environment name (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return r;

                    if (validator.Evaluate() > 0)
                        return 2;

                    var model = new EnviromnentModel()
                    {
                        Groupname = Helper.Parameters.WorkingGroup,
                        EnvironmentName = argenvironmentName.Value,
                    };

                    var result = Client.Post<RootResultModel<EnviromnentModel>>("api/environment/add", model, GetToken());
                    result.Wait();

                    Output.WriteLine($"environment {argenvironmentName.Value} is added on group {result.Result.Datas.Groupname}.");

                    return 0;

                });
            });

            cmd.Command("list", config =>
            {

                config.Description = "list all environments for working group is setted";
                config.HelpOption(HelpFlag);

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return 2;

                    var result = Client.Get<RootResultModel<List<EnviromnentModel>>>($"api/environment/list/{Helper.Parameters.WorkingGroup}", GetToken());
                    result.Wait();

                    PrintDataExtensions.ClearBorder();
                    ConvertToDatatable
                        .ConvertList(result.Result.Datas, "environments")
                        .Print();

                    return 0;

                });
            });

            return app;

        }

    }
}
