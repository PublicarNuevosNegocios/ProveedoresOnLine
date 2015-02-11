using System;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Manager.General;

namespace WebCrawler.Manager
{
    public static class WebCrawlerManager
    {
        public static void WebCrawlerInfo(string ParId, string PublicId)
        {
            Console.WriteLine("\n Registro del proveedor: " + PublicId + " y ParId: " + ParId);
            Console.WriteLine("\n Start Date: " + DateTime.Now.ToString());

            MyWebClient oWebClient = new MyWebClient();

            oWebClient.Headers.Add("Cookie", WebCrawler.Manager.General.InternalSettings.Instance
                                                [Constants.C_Settings_SessionKey].
                                                Value);

            List<string> oSettingsList = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_CrawlerTags].Value.
                Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                ToList();

            foreach (var item in oSettingsList)
            {
                string settings = item.ToString();

                string oParURL = WebCrawler.Manager.General.InternalSettings.Instance
                    ["CrawlerURL_" + settings].
                    Value.
                    Replace("{{ParProviderId}}", ParId);

                Console.WriteLine("\n Download " + settings);

                string strHtml = oWebClient.DownloadString(oParURL);

                HtmlDocument HtmlDoc = new HtmlDocument();
                HtmlDoc.LoadHtml(strHtml);
            }
        }

        public class MyWebClient : System.Net.WebClient
        {
            protected override System.Net.WebRequest GetWebRequest(Uri uri)
            {
                System.Net.WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 20 * 60 * 1000;
                return w;
            }
        }
    }
}
