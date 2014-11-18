using Crawler.Manager.Models;
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
            
            System.Net.WebClient oWebClient = new System.Net.WebClient();
            oWebClient.Headers.Add("Cookie", Crawler.Manager.Models.InternalSettings.Instance
                                                [Crawler.Manager.Models.Constants.C_Settings_SessionKey].
                                                Value);
            List<string> oSettingsList = Crawler.Manager.Models.InternalSettings.Instance[Crawler.Manager.Models.Constants.C_Settings_DetailInfo].Value.
                                        Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                                        ToList();
            foreach (var item in oSettingsList)
            {
                string settings = item.ToString();

                string oParUrl = Crawler.Manager.Models.InternalSettings.Instance
                    ["CrawlerUrl_DetailInfo_" + settings].
                    Value.
                    Replace("{{ParProviderId}}", ParProviderId);

                string strHtml = oWebClient.DownloadString(oParUrl);

                HtmlDocument HtmlDoc = new HtmlDocument();
                HtmlDoc.LoadHtml(strHtml);

                string message = string.Empty;

                if (HtmlDoc.DocumentNode.SelectNodes("//a[@href]") != null)
                {

                    foreach (HtmlNode link in HtmlDoc.DocumentNode.SelectNodes("//a[@href]"))
                    {
                        string urlDownload = Crawler.Manager.Models.InternalSettings.Instance
                                                        [Crawler.Manager.Models.Constants.C_Settings_UrlDownload].
                                                        Value;

                        string folderSave = Crawler.Manager.Models.InternalSettings.Instance
                                                        [Crawler.Manager.Models.Constants.C_Settings_FolderSave].
                                                        Value
                                                        + "\\"
                                                        + ParProviderId
                                                        + "\\"
                                                        + settings;
                        if (!File.Exists(folderSave))
                        {
                            System.IO.Directory.CreateDirectory(folderSave);
                        }

                        HtmlAttribute att = link.Attributes["href"];
                        if (att.Value.Contains(".pdf"))
                        {
                            try
                            {
                                if (settings == "Balance")
                                {
                                    urlDownload += "/generales/" + att.Value.Replace("..", "");
                                }
                                else
                                {
                                    urlDownload += att.Value.Replace("..", "");
                                }

                                string cadena = att.Value.ToString();
                                int l = cadena.IndexOf('&');
                                if (l > 0)
                                {
                                    cadena = cadena.Substring(l, cadena.Length - l);
                                    l = cadena.IndexOf('=');
                                    if (l > 0)
                                    {
                                        cadena = cadena.Substring(l + 1, cadena.Length - l - 1);
                                        l = cadena.IndexOf('&');
                                        if (l > 0)
                                        {
                                            cadena = cadena.Substring(l, cadena.Length - l);
                                            l = cadena.IndexOf('=');
                                            if (l > 0)
                                            {
                                                cadena = cadena.Substring(l + 1, cadena.Length - l - 1);
                                                cadena = cadena.Replace(@"\", @"").Replace('"', ' ');
                                            }
                                        }
                                        else
                                        {
                                            cadena = cadena.Replace(@"\", @"").Replace('"', ' ');
                                        }
                                    }
                                    else
                                    {
                                        cadena = cadena.Replace(@"\", @"").Replace('"', ' ');
                                    }
                                }

                                folderSave += "\\" + cadena;

                                message = att.Value.ToString();

                                oWebClient.DownloadFile(urlDownload, folderSave);

                                //Integración con Document Management

                                Console.WriteLine(message);

                                urlDownload = string.Empty;
                                folderSave = string.Empty;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error! " + e.Message + " Archivo:" + att.Value.ToString());
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("la sección " + settings + " no tiene documentos para descargar.");
                }
            }
        }
    }
}
