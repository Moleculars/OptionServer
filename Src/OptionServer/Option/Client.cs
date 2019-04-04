using System.Net.Http;
using System.Threading.Tasks;

namespace Bb.Option
{

    public class Client
    {

        public static async Task<T> Post<T>(string path, object model)
        {

            var client = new HttpClient
            {
                BaseAddress = new System.Uri(Helper.Parameters.ServerUrl),
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Option client");

            if (!string.IsNullOrEmpty(Helper.Parameters.Token))
            client.DefaultRequestHeaders.Add("authorization", Helper.Parameters.Token);

            var msg = model.Serialize();
            HttpResponseMessage response = await client.PostAsync(path, msg);

            if (response.IsSuccessStatusCode)
            {
                var payloadResponse = await response.Content.ReadAsStringAsync();
                if (typeof(T) == typeof(string))
                    return (T)(object)payloadResponse;

                T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(payloadResponse);
                return result;
            }
            else
            {
                var _response1 = response.StatusCode.ToString();
                throw new System.Net.WebException($"Request failed {response.StatusCode}");
            }


        }

        public static async Task<T> Get<T>(string path)
        {

            var client = new HttpClient
            {
                BaseAddress = new System.Uri(Helper.Parameters.ServerUrl),
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Option client");

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var payloadResponse = await response.Content.ReadAsStringAsync();
                if (typeof(T) == typeof(string))
                    return (T)(object)payloadResponse;

                T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(payloadResponse);
                return result;
            }
            else
            {
                var _response1 = response.StatusCode.ToString();
                throw new System.Net.WebException($"Request failed {response.StatusCode}");
            }

        }

    }


}
