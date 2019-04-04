using Bb.Option;
using Bb.Option.Validators;
using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Option
{
    public partial class Program
    {

        private static void AdServerCommand(CommandLineApplication app)
        {

            var cmd = app.Command("server", config =>
            {


                var validator = new groupArgument(config);

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

        }
    }
}
