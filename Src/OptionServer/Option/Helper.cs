using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bb.Option
{
    public static class Helper
    {

        static Helper()
        {
            var _path = new FileInfo(typeof(Helper).Assembly.Location).Directory.FullName;
            filename = Path.Combine(_path, "mem.json");
        }

        public static void Load()
        {
            if (File.Exists(filename))
            {
                var txt = File.ReadAllText(filename);
                Parameters = JsonConvert.DeserializeObject<Parameters>(txt);
            }
            else
                Parameters = new Parameters();
        }

        public static void Save()
        {
            string txt = JsonConvert.SerializeObject(Parameters);
            File.WriteAllLines(filename, new List<string>() { txt }, Encoding.UTF8);
        }

        public static Parameters Parameters { get; set; }

        private static readonly string filename;

    }

}
