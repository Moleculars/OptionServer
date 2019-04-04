using Bb.Option;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System;
using Bb.Http.Helpers;

namespace Option
{
    public partial class Program
    {


        private static void AddGroupCommand(CommandLineApplication app)
        {

            var cmd = app.Command("group", config =>
            {
                config.Description = "Manage group of application";
            });

            cmd.Command("add", config =>
            {

                var validator = new groupArgument(config);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );


                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    var result = Client.Post<GroupApplicationResult>($"add/{argGroupName.Value}",
                        new {
                            Authorisation = Helper.Parameters.Token
                        }.GetDictionnaryProperties(false)
                        );
                    result.Wait();

                    return 0;

                });

            });

            cmd.Command("get", config =>
            {

                var validator = new groupArgument(config);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );

                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    var result = Client.Post<GroupApplicationResult>($"get/{argGroupName.Value}",
                        new {
                            Authorisation = Helper.Parameters.Token
                        }.GetDictionnaryProperties(false)
                        );
                    result.Wait();

                    return 0;

                });

            });

            cmd.Command("list", config =>
            {

                config.OnExecute(() =>
                {

                    var result = Client.Get<GroupApplicationResult>("list",
                        new {
                            Authorisation = Helper.Parameters.Token
                        }.GetDictionnaryProperties(false)
                        );
                    result.Wait();

                    return 0;

                });

            });



            cmd.Command("grant", config =>
            {

                config.Description = "show help for command group";

                var validator = new groupArgument(config);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );


                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    return 0;

                });
            });

            cmd.Command("revoke", config =>
            {
                config.Description = "show help for command group";
                var validator = new groupArgument(config);

                var argGroupName = validator.Argument("groupName",
                    "application group name that contains all configuration of your pool of aapplication. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );

                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    return 0;
                });
            });

        }

    }
}
