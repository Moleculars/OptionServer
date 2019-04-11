using Bb.Http.Helpers;
using Bb.Option.Models;
using Bb.Option.Printings;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;

namespace Bb.Option.Commands
{
    public partial class Command
    {

        public static CommandLineApplication CommandGroup(this CommandLineApplication app)
        {

            var cmd = app.Command("group", config =>
            {
                config.Description = "Manage group of application";
                config.HelpOption(HelpFlag);
            });

            cmd.Command("add", config =>
            {

                config.Description = "add a new group that can contains futures application";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, true);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );

                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    var result = Client.Get<RootResultModel<GroupApplicationResult>>($"api/ApplicationGroup/add/{argGroupName.Value}", GetToken());
                    result.Wait();

                    if (result.Result != null)
                    {
                        Console.WriteLine($"group '{argGroupName.Value}' created");
                        Helper.Parameters.WorkingGroup = argGroupName.Value;
                        Console.WriteLine($"working group setted on {Helper.Parameters.WorkingGroup}");

                        PrintDataExtensions.ClearBorder();
                        ConvertToDatatable.Convert(new GroupModel(result.Result.Datas), "list of groups you can to use")
                        .PrintList();

                    }

                    return 0;

                });

            });

            cmd.Command("set", config =>
            {

                config.Description = "set group for working";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, true);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );

                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    Helper.Parameters.WorkingGroup = argGroupName.Value;
                    Console.WriteLine($"working group setted on {Helper.Parameters.WorkingGroup}");

                    return 0;

                });

            });

            cmd.Command("get", config =>
            {

                config.Description = "return information on application group if your privilege allow";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, true);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );

                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    var result = Client.Get<RootResultModel<GroupApplicationResult>>($"api/ApplicationGroup/get/{argGroupName.Value}", GetToken());
                    result.Wait();

                    PrintDataExtensions.ClearBorder();
                    ConvertToDatatable
                        .Convert(result.Result.Datas, "group")
                        .PrintList();

                    return 0;

                });

            });

            cmd.Command("list", config =>
            {

                config.Description = "return list of application group thatB your privilege allow";
                config.HelpOption(HelpFlag);

                config.OnExecute(() =>
                {

                    var result = Client.Get<RootResultModel<List<GroupApplicationResult>>>("api/ApplicationGroup/list", GetToken());
                    result.Wait();

                    List<GroupModel> _list = new List<GroupModel>();
                    foreach (var item in result.Result.Datas)
                        _list.Add(new GroupModel(item));

                    PrintDataExtensions.ExtendedASCIIBorder();
                    ConvertToDatatable
                        .ConvertList(_list, "list of groups you can to use")
                        .Print();

                    return 0;

                });

            });

            cmd.Command("grant", config =>
            {

                config.Description = "grant or restrict privilege for a specific user on group application if you are owner";
                config.HelpOption(HelpFlag);

                config.Description = "show help for command group";

                var validator = new GroupArgument(config, true);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of application. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );

                var argUsername = validator.Argument("to_username",
                    "User that must privilege elevated."
                    , ValidatorExtension.EvaluateRequired
                    );


                var argappli = validator.Option("-appli",
                    "privilege on application configuration. " + Command._access
                    , ValidatorExtension.EvaluateAccessEnum
                    );

                var argType = validator.Option("-type",
                    "privilege on type configuration. " + Command._access
                    , ValidatorExtension.EvaluateAccessEnum
                    );

                var argEnv = validator.Option("-env",
                    "privilege on environment configuration." + Command._access
                    , ValidatorExtension.EvaluateAccessEnum
                    );

                var argAll = validator.Option("-all",
                    "privilege on (type, envorinment, application) configuration in one specification. " + Command._access
                    , ValidatorExtension.EvaluateAccessEnum
                    );

                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    var accessApplication = AccessModuleEnum.None;
                    var AccessType = AccessModuleEnum.None;
                    var AccessEnvironment = AccessModuleEnum.None;

                    if (argappli.HasValue())
                        accessApplication = Read(argappli);

                    if (argType.HasValue())
                        AccessType = Read(argType);

                    if (argEnv.HasValue())
                        AccessEnvironment = Read(argEnv);

                    if (argAll.HasValue())
                        accessApplication = AccessEnvironment = AccessEnvironment = Read(argAll);

                    var model = new GrantModel()
                    {

                        User = argUsername.Value,
                        GroupName = argGroupName.Value,

                        AccessApplication = accessApplication,
                        AccessType = AccessType,
                        AccessEnvironment = AccessEnvironment,

                    };

                    var result = Client.Post<RootResultModel<GroupApplicationResult>>($"api/ApplicationGroup/access", model, GetToken());
                    result.Wait();
                    var r = new GroupModel(result.Result.Datas);

                    PrintDataExtensions.ClearBorder();
                    ConvertToDatatable
                        .Convert(r, "result accesses")
                        .PrintList();

                    return 0;

                });
            });

            cmd.Command("revoke", config =>
            {

                config.Description = "revoke privilege for a specific user on group application if you are owner";
                config.HelpOption(HelpFlag);

                config.Description = "show help for command group";
                var validator = new GroupArgument(config, true);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );

                var argUsername = validator.Argument("to_username",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );

                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    var model = new GrantModel()
                    {

                        User = argUsername.Value,
                        GroupName = argGroupName.Value,

                        AccessApplication = AccessModuleEnum.None,
                        AccessType = AccessModuleEnum.None,
                        AccessEnvironment = AccessModuleEnum.None,

                    };

                    var result = Client.Post<RootResultModel<GroupApplicationResult>>($"api/ApplicationGroup/access", model, GetToken());
                    result.Wait();
                    var r = new GroupModel(result.Result.Datas);

                    PrintDataExtensions.ClearBorder();
                    ConvertToDatatable
                        .Convert(r, "result accesses")
                        .PrintList();

                    return 0;
                });

            });

            return app;

        }

        private static Dictionary<string, object> GetToken()
        {
            return new
            {
                authorization = Helper.Parameters.Token
            }.GetDictionnaryProperties(false);
        }
    }
}
