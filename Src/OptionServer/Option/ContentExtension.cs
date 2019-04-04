using System.Net.Http;
using System.Text;

namespace Bb.Option
{
    public static class ContentExtension
    {

        public static HttpContent Serialize(this object self)
        {
            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(self);
            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
            return c;
        }


    }


}
