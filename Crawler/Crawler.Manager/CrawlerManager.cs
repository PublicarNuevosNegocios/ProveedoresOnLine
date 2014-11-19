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
                                UploadFile(ProviderPublicId, folderSave, settings);                               

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

        public static void UploadFile(string ProviderPublicId, string urlFile, string SettingsName)
        {
            DocumentManagement.Provider.Models.Provider.ProviderModel RealtedProvider = DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, null);
            DocumentManagement.Customer.Models.Customer.CustomerModel RealtedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(RealtedProvider.CustomerPublicId);
            
            int FieldId = 0;
            
            //Obtener ProviderInfoType
            if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_Balance.Contains(SettingsName))
            {
                Console.WriteLine("tipo " + SettingsName);
            }
            else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_ExperienceActivities.Contains(SettingsName))
            {
                Console.WriteLine("tipo " + SettingsName);
            }
            else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_HSE.Contains(SettingsName))
            {
                Console.WriteLine("tipo " + SettingsName);
            }
            else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_Legal.Contains(SettingsName))
            {
                Console.WriteLine("tipo " + SettingsName);
            }
            else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_QualityActivities.Contains(SettingsName))
            {
                FieldId = 362;

                DocumentManagement.Provider.Models.Provider.ProviderInfoModel oModel = new DocumentManagement.Provider.Models.Provider.ProviderInfoModel()
                {
                    ProviderInfoType = new DocumentManagement.Provider.Models.Util.CatalogModel()
                    {
                        ItemId = FieldId,
                    },
                };

                RealtedProvider.RelatedProviderInfo.Add(oModel);
            }
            else
            {
                Console.WriteLine("no hay tipo");
            }

            string strFile = urlFile.TrimEnd('\\') +
                        "\\ProviderFile_" +
                        RealtedProvider.CustomerPublicId + "_" +
                        FieldId + "_" +
                        DateTime.Now.ToString("yyyyMMddHHmmss") + "." +
                        urlFile.Split('.').DefaultIfEmpty("pdf").LastOrDefault();
            
            ////load file to s3
            //string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
            //            (strFile,
            //            Crawler.Manager.Models.InternalSettings.Instance
            //                [Crawler.Manager.Models.Constants.C_Settings_File_RemoteDirectoryProvider].Value +
            //                RealtedProvider.CustomerPublicId + "\\");

            DocumentManagement.Provider.Models.Util.CatalogModel oProviderInfoType = GetProviderInfoType(RealtedProvider, FieldId);

            //DocumentManagement.Provider.Models.Provider.ProviderInfoModel oReturn = new DocumentManagement.Provider.Models.Provider.ProviderInfoModel()
            //{
            //    ProviderInfoId = 0,
            //    ProviderInfoType = oProviderInfoType,
            //    LargeValue = strRemoteFile,
            //};
        }

        private static DocumentManagement.Provider.Models.Util.CatalogModel GetProviderInfoType(DocumentManagement.Provider.Models.Provider.ProviderModel GenericModels, int FieldId)
        {
            DocumentManagement.Provider.Models.Util.CatalogModel oReturn = null;

            DocumentManagement.Customer.Models.Customer.CustomerModel oCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(GenericModels.CustomerPublicId);
                        
            oCustomer.RelatedForm.All(f =>
            {
                f.RelatedStep.All(s =>
                {
                    s.RelatedField.All(fi =>
                    {
                        if (fi.FieldId == FieldId)
                        {
                            oReturn = new DocumentManagement.Provider.Models.Util.CatalogModel()
                            {
                                CatalogId = fi.ProviderInfoType.CatalogId,
                                CatalogName = fi.ProviderInfoType.CatalogName,
                                ItemId = fi.ProviderInfoType.ItemId,
                                ItemName = fi.ProviderInfoType.ItemName,
                            };
                        }

                        return true;
                    });

                    return true;
                });
                return true;
            });

            return oReturn;
        }
    }
}
