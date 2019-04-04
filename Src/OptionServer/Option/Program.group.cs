using Bb.Http.Helpers;
using Bb.Option;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Option
{
    public partial class Program
    {


        private static void AddGroupCommand(CommandLineApplication app)
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

                var validator = new GroupArgument(config);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );


                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    var result = Client.Get<GroupApplicationResult>($"api/ApplicationGroup/add/{argGroupName.Value}",
                        new
                        {
                            Authorisation = Helper.Parameters.Token
                        }.GetDictionnaryProperties(false)
                        );
                    result.Wait();

                    return 0;

                });

            });

            cmd.Command("get", config =>
            {

                config.Description = "return information on application group if your privilege allow";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );

                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    var result = Client.Get<GroupApplicationResult>($"api/ApplicationGroup/get/{argGroupName.Value}",
                        new
                        {
                            Authorisation = Helper.Parameters.Token
                        }.GetDictionnaryProperties(false)
                        );
                    result.Wait();

                    return 0;

                });

            });

            cmd.Command("list", config =>
            {

                config.Description = "return list of application group if your privilege allow";
                config.HelpOption(HelpFlag);

                config.OnExecute(() =>
                {

                    var result = Client.Get<GroupApplicationResult>("api/ApplicationGroup/list",
                        new
                        {
                            Authorisation = Helper.Parameters.Token
                        }.GetDictionnaryProperties(false)
                        );
                    result.Wait();

                    return 0;

                });

            });

            cmd.Command("grant", config =>
            {

                config.Description = "grant or restrict privilege for a specific user on group application if you are owner";
                config.HelpOption(HelpFlag);

                config.Description = "show help for command group";

                var validator = new GroupArgument(config);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of application. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );

                var argUsername = validator.Argument("to_username",
                    "User that must privilege elevated."
                    , ValidatorExtension.EvaluateRequired
                    );


                var argappli = validator.Option("-appli",
                    "privilege on application configuration. " + Program._access
                    , ValidatorExtension.EvaluateAccessEnum
                    );

                var argType = validator.Option("-type",
                    "privilege on type configuration. " + Program._access
                    , ValidatorExtension.EvaluateAccessEnum
                    );

                var argEnv = validator.Option("-env",
                    "privilege on environment configuration." + Program._access
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

                    var model = new GrantModel()
                    {

                        User = argUsername.Value,
                        GroupName = argGroupName.Value,

                        AccessApplication = accessApplication,
                        AccessType = AccessType,
                        AccessEnvironment = AccessEnvironment,

                    };

                    var result = Client.Post<GroupApplicationResult>($"api/ApplicationGroup/get/{argGroupName.Value}", model,
                        new
                        {
                            Authorisation = Helper.Parameters.Token
                        }.GetDictionnaryProperties(false)
                        );
                    result.Wait();

                    return 0;

                });
            });

            cmd.Command("revoke", config =>
            {

                config.Description = "revoke privilege for a specific user on group application if you are owner";
                config.HelpOption(HelpFlag);

                config.Description = "show help for command group";
                var validator = new GroupArgument(config);

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

                    var result = Client.Post<GroupApplicationResult>($"api/ApplicationGroup/get/{argGroupName.Value}", model,
                        new
                        {
                            Authorisation = Helper.Parameters.Token
                        }.GetDictionnaryProperties(false)
                        );
                    result.Wait();

                    return 0;
                });

            });

        }

        private static AccessModuleEnum Read(CommandOption cmd)
        {
            return (AccessModuleEnum)Enum.Parse(typeof(AccessModuleEnum),
                cmd
                .Value()
                .Trim()
                .Trim('\'')
                .Trim()
            );
        }
    }
}
