using Bb.Option;
using Bb.Option.Validators;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Bb.Option.Commands
{
    public static partial class Command
    {

        public static CommandLineApplication CommandServer(this CommandLineApplication app)
        {

            var cmd = app.Command("server", config =>
            {

                config.Description = "set url on working configuration server";                
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                var serverName = validator.Argument("server",
                    "Url server of the option server. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                    );


                config.OnExecute(() =>
                {

                    if (validator.Evaluate() > 0)
                        return 1;

                    var uri = new Uri(serverName.Value);

                    Helper.Parameters.ServerUrl = serverName.Value;
                    Console.WriteLine("server option setted on : " + Helper.Parameters.ServerUrl);

                    return 0;

                });
            });

            return app;

        }

    }
}
