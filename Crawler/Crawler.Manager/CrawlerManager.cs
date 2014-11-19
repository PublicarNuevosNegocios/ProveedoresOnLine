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
        public static void CrawlerBasicInfo(string ParProviderId, string ProviderPublicId)
        {
            Console.WriteLine("\n Proveedor con id: " + ParProviderId + "\n");

            System.Net.WebClient oWebClient = new System.Net.WebClient();
            oWebClient.Headers.Add("Cookie", Crawler.Manager.Models.InternalSettings.Instance
                                                [Crawler.Manager.Models.Constants.C_Settings_SessionKey].
                                                Value);
            List<string> oSettingsList = Crawler.Manager.Models.InternalSettings.Instance[Crawler.Manager.Models.Constants.C_Settings_DetailInfo].Value.
                                        Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                                        ToList();

            //Create Provider
            DocumentManagement.Provider.Models.Provider.ProviderModel NewRealtedProviderInfo = new DocumentManagement.Provider.Models.Provider.ProviderModel()
            {
                ProviderPublicId = ProviderPublicId,
                RelatedProviderInfo = new List<DocumentManagement.Provider.Models.Provider.ProviderInfoModel>(),
            };


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
                                UploadFile(ProviderPublicId, folderSave, settings, NewRealtedProviderInfo);

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

            //save provider info
            //DocumentManagement.Provider.Controller.Provider.ProviderCustomerInfoUpsert(NewRealtedProviderInfo);
        }

        public static void UploadFile
            (string ProviderPublicId,
            string urlFile,
            string SettingsName,
            DocumentManagement.Provider.Models.Provider.ProviderModel NewRealtedProviderInfo)
        {
            //upload file
            string strFile = urlFile.TrimEnd('\\') +
                        "\\ProviderFile_" +
                        ProviderPublicId + "_" +
                        "0" + "_" +
                        DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            File.Copy(urlFile, strFile);
            //load file to s3
            string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                        (strFile,
                        Crawler.Manager.Models.InternalSettings.Instance
                            [Crawler.Manager.Models.Constants.C_Settings_File_RemoteDirectoryProvider].Value +
                            ProviderPublicId + "\\");
            File.Delete(strFile);

            NewRealtedProviderInfo.RelatedProviderInfo.Add(new DocumentManagement.Provider.Models.Provider.ProviderInfoModel()
            {
                ProviderInfoType = GetProviderInfoType(urlFile, SettingsName),
                LargeValue = strRemoteFile,
            });
        }

        private static DocumentManagement.Provider.Models.Util.CatalogModel GetProviderInfoType
            (string urlFile, string SettingsName)
        {
            DocumentManagement.Provider.Models.Util.CatalogModel oReturn = new DocumentManagement.Provider.Models.Util.CatalogModel()
            {
                ItemId = 1,
            };

            if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_Legal.Contains(SettingsName) && urlFile.Contains("socios"))
            {
                oReturn.ItemId = 371;
            }
            else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_Legal.Contains(SettingsName) && urlFile.Contains("sociossssssssssss"))
            {
                oReturn.ItemId = 100000;
            }

            ////Obtener ProviderInfoType
            //if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_Balance.Contains(SettingsName))
            //{
            //    Console.WriteLine("tipo " + SettingsName);
            //}
            //else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_ExperienceActivities.Contains(SettingsName))
            //{
            //    Console.WriteLine("tipo " + SettingsName);
            //}
            //else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_HSE.Contains(SettingsName))
            //{
            //    Console.WriteLine("tipo " + SettingsName);
            //}
            //else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_Legal.Contains(SettingsName))
            //{
            //    Console.WriteLine("tipo " + SettingsName);
            //}


            return oReturn;
        }
    }
}
