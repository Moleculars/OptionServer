using Bb.Option;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Option
{
    public partial class Program
    {

        private static void AdduserCommand(CommandLineApplication app)
        {

            var cmd = app.Command("user", config =>
            {

            });

            cmd.Command("add", config =>
            {

                config.Description = "Add user in the option server";
                var validator = new groupArgument(config);

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
                        return 1;

                    var model = new CreateUserInputModel()
                    {
                        Login = argUsername.Value,
                        Password = argPassword.Value,
                        Pseudo = argPseudo.Value,
                        Mail = argEmail.Value
                    };

                    var result = Client.Post<UserCreatedResultModel>("api/user/add", model);
                    result.Wait();

                    Console.WriteLine($"added {argUsername.Value} : with key {result.Result.Id}");

                    return 0;

                });
            });

            cmd.Command("connect", config =>
            {

                config.Description = "connect a specific user";
                var validator = new groupArgument(config);

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
                        return 1;

                    var model = new LoginInputModel()
                    {
                        Login = argUsername.Value,
                        Password = argPassword.Value,
                    };

                    try
                    {

                        var result = Client.Post<string>("/api/user/connect", model);
                        result.Wait();
                        Helper.Parameters.Token = result.Result;
                        Console.WriteLine($"{argUsername.Value} connected");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return 1;
                    }

                    return 0;

                });
            });

            cmd.Command("help", config =>
            {
                config.Description = "show help for commands that manager user";

                config.OnExecute(() =>
                {
                    config.ShowHelp();
                    return 0;
                });
            });

        }

    }
}
