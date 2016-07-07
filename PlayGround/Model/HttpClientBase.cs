using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace PlayGround.Model
{
    public class HttpClientBase
    {
        #region Static Members

        private static readonly HttpClientBase _current = new HttpClientBase();

        public static HttpClientBase Current
        {
            get { return _current; }
        }

        #region Static Methods
        public static HttpClient GetOptimizedClient()
        {
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip });
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            return client;
        }

        private static HttpContent GetHttpContent(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var content = new ByteArrayContent(bytes);

            content.Headers.Add("Content-Length", Convert.ToString(bytes.Length));
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            return content;
        }

        private static HttpContent GetHttpContent<T>(T obj)
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (!props.Any())
            {
                return null;
            }
            var sb = new StringBuilder();
            foreach (var prop in props)
            {
                sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(prop.Name), prop.GetValue(obj) ?? string.Empty);
            }
            sb = sb.Remove(sb.Length - 1, 1);
            return GetHttpContent(sb.ToString());
        }
        #endregion

        #endregion

        #region Constructor
        private HttpClientBase()
        {

        }
        #endregion

        #region Public Methods
        public async Task<TResp> PostJson<TReq, TResp>(TReq req, string url)
        {
            using (var client = GetOptimizedClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", "amginetech", "amginetech"))));
                client.DefaultRequestHeaders.Add("Trace-Id", Guid.NewGuid().ToString());
                
                var response = await client.PostAsync(url, GetHttpContent<TReq>(req));
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResp>(responseJson);
            }
        }
        public async Task<TResp> PostJson<TResp>(string req, string url)
        {
            using (var client = GetOptimizedClient())
            {
                var response = await client.PostAsync(url, GetHttpContent(req));
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResp>(responseJson);
            }
        }
        #endregion

    }
}
