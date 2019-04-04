using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Bb.Http.Helpers
{

    internal static class ContentExtension
    {

        public static HttpContent Serialize(this object self)
        {
            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(self);
            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
            return c;
        }

        public static void AddHeader(this HttpRequestHeaders self, Dictionary<string, object> dictionary)
        {

            foreach (var item in dictionary)
                if (item.Value != null)
                    self.Add(item.Key, item.Value.ToString());

        }


    }


}
