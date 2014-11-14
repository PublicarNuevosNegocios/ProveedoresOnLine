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

            #region CreateFolderSave

            //string carpeta = Path.GetDirectoryName(ParProviderId);
            //if (!Directory.Exists(carpeta))
            //{
            //    Directory.CreateDirectory(carpeta);
            //}

            //String savePath = carpeta;

            #endregion


            foreach (HtmlNode link in HtmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];
                if (att.Value.Contains(".pdf"))
                {
                    try
                    {
                        message = att.Value.ToString();                        
                        Console.WriteLine(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error! " + e.Message);
                    }                    
                }
            }

            //xxx[0].Attributes["href"].Value;

            //(new System.Net.WebClient()).DownloadFile(oParUrl,@"D:\Proyectos\Github\ProveedoresOnLine\Crawler\Crawler\1.html");
            //(new System.Net.WebClient()).DownloadFile(att.Value, Crawler.Manager.Models.InternalSettings.Instance
            //                                    [Crawler.Manager.Models.Constants.C_Settings_FolderSave].
            //                                    Value + att.Value.ToString() + ".pdf");

        }
    }
}
