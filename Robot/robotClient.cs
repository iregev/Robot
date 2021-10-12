using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace Robot
{
    public class robotClient : IRobot
    {

        private static readonly HttpClient client = new HttpClient();
        private string URL;

        public robotClient(string url)
        {
            URL = url;
        }

        public async Task<string> MoveStr(int pivotDegrees, double meters)
        {
            var responseString = await client.GetStringAsync("http://ynet.co.il");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(responseString);
            var txt = doc.DocumentNode.SelectSingleNode("//span[@class='author']");

            //var metersMoved = await new HttpClient().Send();
            //return metersMoved == meters;
            return txt.InnerText;

        }

     /*   public async Task<string> MoveJson(int pivotDegrees, double meters)
        {
//            var response = await client.GetStringAsync("https://reqres.in/api/products/3");
            var response = await client.GetStringAsync(URL);
            JObject rss = JObject.Parse(response);
            string rssVal = (string)rss["data"]["id"];
            return rssVal;

        }
     */
        public async Task<int[]> Move(int pivotDegrees, int meters)
        {
            var values = new Dictionary<string, string>
                {
                    { "distance", meters.ToString() },
                    { "angle", pivotDegrees.ToString()}
                };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(URL, content);
            var responseString = await response.Content.ReadAsStringAsync();
            JObject rss = JObject.Parse(responseString);
            string rssX = (string)rss["x"];
            string rssY = (string)rss["x"];
            int[] result = new int[2];
            result[0] = Int16.Parse(rssX);
            result[1] = Int16.Parse(rssY);
            return result;

        }
    }
}
