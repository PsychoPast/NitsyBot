using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace NitsyBot.Core
{
    public static class FetchStringRessources
    {
        private const string githubToken = "f36779387840f768063db2527626cf89e96860cf";
        public static string GetContent(string input)
        {
            string output = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(input);
            request.Headers.Add("Authorization", githubToken);
            request.AllowAutoRedirect = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (MemoryStream stream = new MemoryStream())
            {
                response.GetResponseStream().CopyTo(stream);
                output = Encoding.ASCII.GetString(stream.ToArray());
            }
            response.Close();
            return output;
        }
    }
    public class Strings
    {
        public static Dictionary<string, Dictionary<string, string>> strings;
        private const string fileurl = "";
        public Strings()
        {
            strings = new Dictionary<string, Dictionary<string, string>>();
            strings = JsonConvert.DeserializeObject<Dictionary<string,Dictionary<string, string>>>(FetchStringRessources.GetContent(fileurl));
        }
    }
}