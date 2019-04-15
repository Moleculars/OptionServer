using Bb.Option.Commands;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            if (!ValidatorExtension.CheckToken())
                Helper.Parameters.Token = null;

        }

        public static void Save()
        {
            string txt = JsonConvert.SerializeObject(Parameters);
            File.WriteAllLines(filename, new List<string>() { txt }, Encoding.UTF8);
        }

        public static Parameters Parameters { get; set; }


        public static string LoadContentFromFile(string _path)
        {

            string fileContents = string.Empty;
            System.Text.Encoding encoding = null;
            FileInfo _file = new FileInfo(_path);

            using (FileStream fs = _file.OpenRead())
            {

                Ude.CharsetDetector cdet = new Ude.CharsetDetector();
                cdet.Feed(fs);
                cdet.DataEnd();
                if (cdet.Charset != null)
                    encoding = System.Text.Encoding.GetEncoding(cdet.Charset);
                else
                    encoding = System.Text.Encoding.UTF8;

                fs.Position = 0;

                byte[] ar = new byte[_file.Length];
                fs.Read(ar, 0, ar.Length);
                fileContents = encoding.GetString(ar);
            }

            if (fileContents.StartsWith("ï»¿"))
                fileContents = fileContents.Substring(3);

            if (encoding != System.Text.Encoding.UTF8)
            {
                var datas = System.Text.Encoding.UTF8.GetBytes(fileContents);
                fileContents = System.Text.Encoding.UTF8.GetString(datas);
            }

            return fileContents;

        }



        public static string SerializeContract(this string self)
        {

            if (string.IsNullOrEmpty(self))
                return string.Empty;

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(self);
            var result = Convert.ToBase64String(bytes);
            return result;
        }


        public static string DeserializeContract(this string self)
        {

            if (string.IsNullOrEmpty(self))
                return string.Empty;

            byte[] bytes = Convert.FromBase64String(self); ;
            string result = System.Text.Encoding.UTF8.GetString(bytes);

            return result;

        }


        public static List<TypeToShow> ConvertDataToShow(List<TypeModel> datas)
        {
            return datas.Select(c => new TypeToShow(c)).ToList();
        }


        private static readonly string filename;

    }

}
