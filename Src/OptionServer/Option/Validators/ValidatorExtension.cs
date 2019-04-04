using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;

namespace Bb.Option.Validators
{


    public class groupArgument
    {


        public groupArgument(CommandLineApplication config)
        {
            _config = config;
            _dic = new Dictionary<CommandArgument, Container>();
        }


        public CommandArgument Argument(string name, string Description, params Func<CommandArgument, int>[] validators)
        {
            return Argument(name, Description, false, validators);
        }

        public CommandArgument Argument(string name, string Description, bool multipleValues, params Func<CommandArgument, int>[] validators)
        {

            var cmd = _config.Argument(name, Description, multipleValues);

            Validate(cmd, validators);

            return cmd;

        }

        public CommandArgument Validate(CommandArgument cmd, params Func<CommandArgument, int>[] validators)
        {

            if (!_dic.TryGetValue(cmd, out Container list))
                _dic.Add(cmd, list = new Container(cmd));

            foreach (var item in validators)
                list.Add(item);

            return cmd;

        }

        internal int Evaluate()
        {
            int result = 0;
            foreach (var item in _dic)
            {
                var r = item.Value.Evaluate();
                if (r > 0)
                    result = r;
            }
            return result;

        }

        private class Container
        {

            private readonly CommandArgument cmd;
            private readonly List<Func<CommandArgument, int>> _list;

            public Container(CommandArgument cmd)
            {
                this.cmd = cmd;
                _list = new List<Func<CommandArgument, int>>();
            }

            internal void Add(Func<CommandArgument, int> item)
            {

                _list.Add(item);

            }

            internal int Evaluate()
            {

                int result = 0;
                foreach (var item in _list)
                {

                    var r = item(cmd);
                    if (r > 0)
                        result = r;

                }

                return result;

            }

        }

        private readonly CommandLineApplication _config;
        private Dictionary<CommandArgument, Container> _dic = new Dictionary<CommandArgument, Container>();

    }


    public static class ValidatorExtension
    {


        public static int EvaluateRequired(CommandArgument command)
        {

            if (string.IsNullOrWhiteSpace(command.Value))
                return Error("{0} must be specified", command);

            return 0;

        }


        public static int Error(string message, CommandArgument arg)
        {
            Console.WriteLine(string.Format(message, arg.Name));
            return 1;
        }

    }
}
