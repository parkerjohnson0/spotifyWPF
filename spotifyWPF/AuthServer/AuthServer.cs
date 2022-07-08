using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Documents;
using Newtonsoft.Json.Linq;

namespace spotifyWPF.AuthServer
{
    public class AuthServer
    {
        private string CLIENT_ID = "cec124a2ea744be8ab134fec1d755280";
        private string CLIENT_SECRET = "45e9dcbf2c9644c0afbf79c610c4a762";
        private string BASE_URL = "https://accounts.spotify.com/";
        private string RESPONSE_TYPE = "code";
        private string REDIRECT_URI = "http://localhost:5160/callback";
        private HttpListener _listener;
        private HttpClient _client;
        public HttpListenerRequest Request { get; set; }

        public AuthServer()
        {
            _listener = new HttpListener();
            _client = new HttpClient();
            _listener.Prefixes.Add("http://localhost:5160/callback/");
        }

        /// <summary>
        /// Start server, wait for user to accept or deny user permissions. Returns bool stating if successful.
        /// </summary>
        /// <returns>bool</returns>
        public async Task<bool> Start()
        {
            _listener.Start();
            HttpListenerContext context = await _listener.GetContextAsync();
            Request = context.Request;
            HttpListenerResponse response = context.Response;
            if (Request.QueryString.Get("error") != null)
            {
                return false;
            }

            return true;
        }

        public void OpenBrowser()
        {
            string url =
                $"{BASE_URL}authorize?client_id={CLIENT_ID}&response_type={RESPONSE_TYPE}&redirect_uri={REDIRECT_URI}" +
                "&scope=playlist-read-collaborative playlist-modify-public playlist-read-private playlist-modify-private " +
                "user-read-playback-state streaming user-read-playback-state";
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.FileName = url;
            proc.Start();
        }

        public async Task<string> getAccessToken()
        {
            string ret = "";
            string code = Request.QueryString.Get("code");
            var form = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", REDIRECT_URI },
                
            };
            byte[] bytes = Encoding.UTF8.GetBytes($"{CLIENT_ID}:{CLIENT_SECRET}");
            string url = $"{BASE_URL}api/token";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Y2VjMTI0YTJlYTc0NGJlOGFiMTM0ZmVjMWQ3NTUyODA6NDVlOWRjYmYyYzk2NDRjMGFmYmY3OWM2MTBjNGE3NjI=");
            request.Content = new FormUrlEncodedContent(form);
            HttpResponseMessage resp = await _client.SendAsync(request);
            JObject obj = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
            return (string)obj["access_token"];
        }
    }
}