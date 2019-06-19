using Bb.Option.Printings;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System.Collections.Generic;
using System.Linq;

namespace Bb.Option.Commands
{
    public static partial class Command
    {

        public static CommandLineApplication CommandConfiguration(this CommandLineApplication app)
        {

            var cmd = app.Command("config", config =>
            {

                config.Description = "create, list, ... configuration document";
                config.HelpOption(HelpFlag);

            });

            cmd.Command("write", config =>
            {

                config.Description = "Add a new configuration document in the application for current working group is setted";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                var argApplicationName = validator.Argument("applicationName",
                    "application name (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    , ValidatorExtension.EvaluateName
                    );

                var argFile = validator.Argument("filepath",
                    "file must be registered in the server(this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    , ValidatorExtension.EvaluateFileExist
                    );

                var argName = validator.Argument("name",
                    "name must be registered in the server(this argument is required,if you want keep name of the file specify '-')"
                    , ValidatorExtension.EvaluateRequired
                    , ValidatorExtension.EvaluateName
                    );

                var argType = validator.Argument("type",
                    "type of configuration(this argument is required,if you want resolve type from filename specify '-')"
                    , ValidatorExtension.EvaluateRequired
                    , ValidatorExtension.EvaluateName
                    );

                var argEnv = validator.Argument("environment",
                    "environment name of configuration(this argument is required,if you want resolve type from filename specify '-')"
                    , ValidatorExtension.EvaluateRequired
                    , ValidatorExtension.EvaluateName
                    );

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return r;

                    if (string.IsNullOrEmpty(Helper.Parameters.WorkingGroup))
                    {
                        Output.ErrorWriteLine("Please set the working group before");
                        return 1;
                    }

                    string n = System.IO.Path.GetFileNameWithoutExtension(argFile.Value.GetFilename().Name);
                    string[] names = n.Split('.');

                    string name = argName.Value;
                    if (name == "-")
                        name = names[0];

                    string environment = argEnv.Value;
                    if (environment == "-")
                    {
                        if (names.Length >= 3)
                        {
                            environment = names[1];
                            var resultEnv = Client.Get<RootResultModel<List<EnviromnentModel>>>($"api/environment/list/{Helper.Parameters.WorkingGroup}", GetToken());
                            resultEnv.Wait();
                            var items = resultEnv.Result.Datas.Select(c => c.EnvironmentName).ToDictionary(c => c.ToLowerInvariant(), c => c);
                            if (!items.TryGetValue(environment.ToLowerInvariant(), out string r1))
                            {
                                Output.ErrorWriteLine($"environment {environment} can't be resolved from filename");
                                return 1;
                            }
                            environment = r1;
                        }
                        else
                        {
                            Output.ErrorWriteLine("environment can't be resolved from filename");
                            return 1;
                        }
                    }

                    string type = argType.Value;
                    if (type == "-")
                    {

                        if (names.Length > 1)
                        {
                            type = names[names.Length];

                            var resultExtensions = Client.Get<RootResultModel<List<TypeModel>>>($"api/type/extension/{Helper.Parameters.WorkingGroup}/{type}", GetToken());
                            resultExtensions.Wait();
                            var items = resultExtensions.Result;
                            if (items.Datas.Count == 0)
                            {
                                Output.ErrorWriteLine($"type can't be resolve from extension {type}");
                                return 1;
                            }
                            else if (items.Datas.Count > 1)
                            {
                                Output.ErrorWriteLine($"Ambigues types, {string.Join(", ", items.Datas.Select(c => c.TypeName))} ");
                                return 1;
                            }
                            else
                                type = items.Datas.First().TypeName;
                        }
                        else
                        {
                            Output.ErrorWriteLine("type can't be resolved from filename");
                            return 1;
                        }

                    }

                    ConfigurationModel _config = new ConfigurationModel()
                    {
                        GroupName = Helper.Parameters.WorkingGroup,
                        ApplicationName = argApplicationName.Value,
                        Datas = Helper.LoadContentFromFile(argFile.Value.GetFilename().FullName),
                        Name = name,
                        Environment = "",
                        Type = "",
                    };

                    var result = Client.Post<RootResultModel<ApplicationModel>>($"api/configuration/add", _config, GetToken());
                    result.Wait();

                    Output.WriteLine($"cofiguration '{_config.Name}' is added on {result.Result.Datas.GroupName}.{_config.ApplicationName}.");

                    return 0;

                });
            });

            cmd.Command("list", config =>
            {

                config.Description = "list all configuration documents in the applications for current working group is setted";
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
