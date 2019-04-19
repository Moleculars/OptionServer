using Bb.Option.Commands;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Bb.Option
{


    //public class Client : BbClientHttp
    //{

    //    public Client(Uri server) : base (server)
    //    {

    //    }

    //    protected override object ThrowWebRequest(HttpResponseMessage response)
    //    {

    //        //switch (response.StatusCode)
    //        //{
    //        //    case System.Net.HttpStatusCode.Accepted:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.AlreadyReported:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Ambiguous:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.BadGateway:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.BadRequest:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Conflict:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Continue:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Created:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.EarlyHints:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.ExpectationFailed:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.FailedDependency:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Forbidden:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Found:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.GatewayTimeout:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Gone:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.HttpVersionNotSupported:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.IMUsed:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.InsufficientStorage:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.InternalServerError:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.LengthRequired:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Locked:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.LoopDetected:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.MethodNotAllowed:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.MisdirectedRequest:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Moved:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.MovedPermanently:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.MultipleChoices:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.MultiStatus:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.NetworkAuthenticationRequired:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.NoContent:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.NonAuthoritativeInformation:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.NotAcceptable:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.NotExtended:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.NotFound:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.NotImplemented:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.NotModified:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.OK:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.PartialContent:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.PaymentRequired:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.PermanentRedirect:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.PreconditionFailed:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.PreconditionRequired:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Processing:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Redirect:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.RedirectKeepVerb:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.RedirectMethod:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.RequestedRangeNotSatisfiable:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.RequestEntityTooLarge:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.RequestHeaderFieldsTooLarge:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.RequestTimeout:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.RequestUriTooLong:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.ResetContent:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.SeeOther:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.ServiceUnavailable:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.SwitchingProtocols:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.TemporaryRedirect:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.TooManyRequests:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Unauthorized:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.UnavailableForLegalReasons:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.UnprocessableEntity:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.UnsupportedMediaType:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.Unused:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.UpgradeRequired:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.UseProxy:
    //        //        break;
    //        //    case System.Net.HttpStatusCode.VariantAlsoNegotiates:
    //        //        break;
    //        //    default:
    //        //        break;
    //        //}

    //        return base.ThrowWebRequest(response);
    //    }

    //}

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


        public static string LoadContentFromText(byte[] text)
        {

            string textContents = string.Empty;
            System.Text.Encoding encoding = null;

            using (MemoryStream fs = new MemoryStream(text))
            {

                Ude.CharsetDetector cdet = new Ude.CharsetDetector();
                cdet.Feed(fs);
                cdet.DataEnd();
                if (cdet.Charset != null)
                    encoding = System.Text.Encoding.GetEncoding(cdet.Charset);
                else
                    encoding = System.Text.Encoding.UTF8;

                fs.Position = 0;

                byte[] ar = new byte[text.Length];
                fs.Read(ar, 0, ar.Length);
                textContents = encoding.GetString(ar);
            }

            if (textContents.StartsWith("ï»¿"))
                textContents = textContents.Substring(3);

            if (encoding != System.Text.Encoding.UTF8)
            {
                var datas = System.Text.Encoding.UTF8.GetBytes(textContents);
                textContents = System.Text.Encoding.UTF8.GetString(datas);
            }

            return textContents;

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
