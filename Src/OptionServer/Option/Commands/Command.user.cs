using Bb.Option;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Bb.Option.Commands
{
    public static partial class Command
    {

        public static CommandLineApplication CommandUser(this CommandLineApplication app)
        {

            var cmd = app.Command("user", config =>
            {

                config.Description = "create, lock, unlock, connect user";                
                config.HelpOption(HelpFlag);

            });

            cmd.Command("add", config =>
            {

                config.Description = "Add a new user on the option server";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                var argUsername = validator.Argument("username",
                    "username of user. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                var argPassword = validator.Argument("password",
                    "password of user. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                var argPseudo = validator.Argument("pseudo",
                    "pseudo of the new user. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                var argEmail = validator.Argument("email",
                    "email of the new user. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 2;

                    var model = new CreateUserInputModel()
                    {
                        Login = argUsername.Value,
                        Password = argPassword.Value,
                        Pseudo = argPseudo.Value,
                        Mail = argEmail.Value
                    };

                    var result = Client.Post<RootResultModel<UserCreatedResultModel>>("api/user/add", model);
                    result.Wait();

                    Output.WriteLine($"user {argUsername.Value} is added");

                    return 0;

                });
            });

            cmd.Command("connect", config =>
            {

                config.Description = "Authenticate user";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                 var argUsername = validator.Argument(
                    "username",
                    "username of user. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                var argPassword = validator.Argument(
                    "password",
                    "password of user. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 2;

                    var model = new LoginInputModel()
                    {
                        Login = argUsername.Value,
                        Password = argPassword.Value,
                    };

                    try
                    {

                        var result = Client.Post<RootResultModel<string>>("/api/user/connect", model);
                        result.Wait();
                        Helper.Parameters.Token = result.Result.Datas;
                        Helper.Parameters.TokenExpiration = DateTime.Now.AddMinutes(60);
                        Output.WriteLine($"{argUsername.Value} is connected");
                        Helper.Parameters.WorkingGroup = null;
                    }
                    catch (Exception e)
                    {
                        Output.WriteLine(e.Message);
                        return 1;
                    }

                    return 0;

                });
            });

            return app;

        }

    }
}
