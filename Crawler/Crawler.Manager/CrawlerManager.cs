using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Manager
{
    public static class CrawlerManager
    {
        public static void CrawlerBasicInfo(string ParProviderId)
        {
            string oParUrl = Crawler.Manager.Models.InternalSettings.Instance
                [Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_BasicInfo].
                Value.
                Replace("{{ParProviderId}}", ParProviderId);

            string strHtml = (new System.Net.WebClient()).DownloadString(oParUrl);

            HtmlDocument HtmlDoc = new HtmlDocument();
            HtmlDoc.LoadHtml(strHtml);

            var xxx = HtmlDoc.DocumentNode.SelectNodes("//a[@href]");

            //xxx[0].Attributes["href"].Value;

            //(new System.Net.WebClient()).DownloadFile(oParUrl,@"D:\Proyectos\Github\ProveedoresOnLine\Crawler\Crawler\1.html");

        }
    }
}
