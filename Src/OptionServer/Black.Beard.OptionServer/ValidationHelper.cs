using Bb.OptionServer.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace Bb.OptionServer
{
    public static class ValidationHelper
    {

        static ValidationHelper()
        {
            _reg = new Regex("\\w*[\\w\\d]*", RegexOptions.Compiled);
        }

        public static bool ValidateName(string name, out Exception e)
        {

            e = null;

            if (name.Contains("."))
                e = new InvalidNameException($"{name} can't contains '.'");

            else if (!_reg.Match(name).Success)
                e = new InvalidNameException(name);

            return e != null;

        }


        private static readonly Regex _reg;

    }


}
