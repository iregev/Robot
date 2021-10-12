using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace Robot
{
    public class robotClient : IRobot
    {

        private static readonly HttpClient client = new HttpClient();

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
    }
}
