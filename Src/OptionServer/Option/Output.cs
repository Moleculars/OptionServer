using System;

namespace Bb.Option
{

    public class Output
    {

        internal static void Write(string message, params object[] args)
        {
            Write(string.Format(message, args));
        }

        internal static void WriteLine(string message, params object[] args)
        {
            WriteLine(string.Format(message, args));
        }

        internal static void WriteLine()
        {
            WriteLine(string.Empty);
        }

        internal static void ErrorWriteLine(string message, params object[] args)
        {
            ErrorWriteLine(string.Format(message));
        }

        internal static void ErrorWriteLine()
        {
            ErrorWriteLine(string.Empty);
        }


        internal static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        internal static void Write(string message)
        {
            Console.Write(message);
        }


        internal static void ErrorWriteLine(string message)
        {

            var c = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ForegroundColor = c;

            Console.Error.Flush();

        }


    }

}
