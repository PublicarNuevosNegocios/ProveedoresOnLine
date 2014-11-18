using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Manager
{
    public static class CrawlerManager
    {
        public static void CrawlerBasicInfo(string ParProviderId)
        {
            Console.WriteLine("\n Proveedor con id: " + ParProviderId + "\n");

            string oParUrl = Crawler.Manager.Models.InternalSettings.Instance
                [Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_HSE].
                Value.
                Replace("{{ParProviderId}}", ParProviderId);

            System.Net.WebClient oWebClient = new System.Net.WebClient();
            oWebClient.Headers.Add("Cookie", Crawler.Manager.Models.InternalSettings.Instance
                                                [Crawler.Manager.Models.Constants.C_Settings_SessionKey].
                                                Value);

            string strHtml = oWebClient.DownloadString(oParUrl);

            //

            oParUrl = Crawler.Manager.Models.InternalSettings.Instance
                [Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_Legal].
                Value.
                Replace("{{ParProviderId}}", ParProviderId);

            strHtml += oWebClient.DownloadString(oParUrl);

            //

            HtmlDocument HtmlDoc = new HtmlDocument();
            HtmlDoc.LoadHtml(strHtml);

            string message = string.Empty;       

            foreach (HtmlNode link in HtmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string folderSave = Crawler.Manager.Models.InternalSettings.Instance
                                                [Crawler.Manager.Models.Constants.C_Settings_FolderSave].
                                                Value;
                string urlDownload = "https://www.parservicios.com/parservi/procesos";

                HtmlAttribute att = link.Attributes["href"];
                if (att.Value.Contains(".pdf"))
                {
                    try
                    {
                        urlDownload += att.Value.Replace("..", "");

                        string cadena = att.Value.ToString();
                        int l = cadena.IndexOf('&');
                        if (l > 0)
                        {
                            cadena = cadena.Substring(l, cadena.Length - l);
                            l = cadena.IndexOf('=');
                            if (l > 0)
                            {
                                cadena = cadena.Substring(l + 1, cadena.Length - l - 1);
                                //cadena = cadena.Replace("\", '');
                            }
                        }

                        folderSave += "/" + cadena;

                        message = att.Value.ToString();

                        oWebClient.DownloadFile(urlDownload, folderSave);

                        Console.WriteLine(message);

                        urlDownload = string.Empty;
                        folderSave = string.Empty;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error! " + e.Message);
                    }                    
                }
            }
        }
    }
}
