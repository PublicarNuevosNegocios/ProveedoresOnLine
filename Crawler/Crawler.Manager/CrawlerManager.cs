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
                RelatedProviderCustomerInfo = new List<DocumentManagement.Provider.Models.Provider.ProviderInfoModel>(),
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
                        HtmlAttribute att = link.Attributes["href"];
                        if (att.Value.Contains(".pdf"))
                        {
                            try
                            {
                                string urlDownload = Crawler.Manager.Models.InternalSettings.Instance
                                                        [Crawler.Manager.Models.Constants.C_Settings_UrlDownload].
                                                        Value;

                                string folderSave = Crawler.Manager.Models.InternalSettings.Instance
                                                                [Crawler.Manager.Models.Constants.C_Settings_FolderSave].
                                                                Value
                                                                + "\\"
                                                                + ProviderPublicId
                                                                + "\\"
                                                                + settings;
                                if (!File.Exists(folderSave))
                                {
                                    System.IO.Directory.CreateDirectory(folderSave);
                                }


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

                                string folder = folderSave + "\\" + cadena;
                                message = att.Value.ToString();

                                oWebClient.DownloadFile(urlDownload, folder);

                                //Integración con Document Management
                                UploadFile(ProviderPublicId, folder, folderSave, settings, NewRealtedProviderInfo);

                                Console.WriteLine(message);
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
                    Console.WriteLine("la sección " + settings + " no tiene documentos para descargar." + "\n");
                }
            }

            try
            {
                DocumentManagement.Provider.Models.Provider.ProviderModel CurrentProviderInfo =
                    DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, null);

                NewRealtedProviderInfo.RelatedProviderInfo.All(destino =>
                {
                    if (("351,349,362,").ToString().Contains(destino.ProviderInfoType.ItemId.ToString() + ","))
                    {
                        //multiple file
                    }
                    else
                    {
                        //single file
                        destino.ProviderInfoId =
                            CurrentProviderInfo.RelatedProviderInfo.
                            Where(x => destino.ProviderInfoType.ItemId == x.ProviderInfoType.ItemId).
                            Select(x => x.ProviderInfoId).
                            DefaultIfEmpty(0).
                            FirstOrDefault();
                    }

                    return true;
                });

                //save provider info
                DocumentManagement.Provider.Controller.Provider.ProviderInfoUpsert(NewRealtedProviderInfo);
                Console.WriteLine("Se ha guardado el Proveedor" + "\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error! " + e.Message + " No se puede guardar la información del proveedor." + "\n");
            }
        }

        public static void UploadFile
            (string ProviderPublicId,
            string urlFile,
            string urlNewFile,
            string SettingsName,
            DocumentManagement.Provider.Models.Provider.ProviderModel NewRealtedProviderInfo)
        {
            //upload file
            string strFile = string.Empty;

            strFile = urlNewFile +
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
                            "Test\\" + ProviderPublicId + "\\");
            File.Delete(strFile);

            if (SettingsName == "Balance" || SettingsName == "ExperienceActivities" || SettingsName == "QualityActivities")
            {
                int l = urlFile.LastIndexOf('\\');
                string nameFile = urlFile.Substring(l + 1, urlFile.Length - l - 1).Replace(".pdf", "");
                l = strFile.LastIndexOf('\\');
                string nameS3File = strFile.Substring(l + 1, strFile.Length - l - 1);
                string oLargeValue = "{\"ProviderInfoId\":\"\",\"IsDelete\":false,\"ProviderInfoUrl\":\"" +
                strRemoteFile + "\",\"FileName\":\"" + nameS3File + "\",\"Name\":\"" + nameFile + "\"}";             

                NewRealtedProviderInfo.RelatedProviderInfo.Add(new DocumentManagement.Provider.Models.Provider.ProviderInfoModel()
                {
                    ProviderInfoType = GetProviderInfoType(urlFile, SettingsName),
                    LargeValue = oLargeValue,
                });
            }
            else
            {
                DocumentManagement.Provider.Models.Provider.ProviderInfoModel oProviderInfoModel = new DocumentManagement.Provider.Models.Provider.ProviderInfoModel()
                {
                    ProviderInfoType = GetProviderInfoType(urlFile, SettingsName)
                };

                if (oProviderInfoModel.ProviderInfoType.ItemId == 352)
                {
                    int l = urlFile.LastIndexOf('\\');
                    string nameFile = urlFile.Substring(l + 1, urlFile.Length - l - 1).Replace(".pdf", "");
                    l = strFile.LastIndexOf('\\');
                    string nameS3File = strFile.Substring(l + 1, strFile.Length - l - 1);
                    string oLargeValue = "{\"ProviderInfoId\":\"\",\"IsDelete\":false,\"ProviderInfoUrl\":\"" +
                    strRemoteFile + "\",\"FileName\":\"" + nameS3File + "\",\"Name\":\"" + nameFile + "\"}";    

                    oProviderInfoModel.LargeValue = oLargeValue;
                    NewRealtedProviderInfo.RelatedProviderInfo.Add(oProviderInfoModel);
                }
                else
                {
                    oProviderInfoModel.LargeValue = strRemoteFile;
                    NewRealtedProviderInfo.RelatedProviderInfo.Add(oProviderInfoModel);
                }
            }
        }

        private static DocumentManagement.Provider.Models.Util.CatalogModel GetProviderInfoType
            (string urlFile, string SettingsName)
        {
            urlFile = urlFile.ToLower();

            DocumentManagement.Provider.Models.Util.CatalogModel oReturn = new DocumentManagement.Provider.Models.Util.CatalogModel()
            {
                ItemId = 1,
            };

            if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_Legal.Contains(SettingsName))
            {
                //archivos de legales
                if (urlFile.Contains("camara") || urlFile.Contains("comercio"))
                {
                    oReturn.ItemId = 343; //Cámara de Comercio (No mayor a 1 mes, formato PDF)
                }
                else if (urlFile.Contains("rut"))
                {
                    oReturn.ItemId = 344; //RUT (Lo más reciente posible, formato PDF)
                }
                else if (urlFile.Contains("contribuyente"))
                {
                    oReturn.ItemId = 345; //Resolución de Gran Contribuyente (Si aplica, formato PDF)
                }
                else if (urlFile.Contains("autorretenedor"))
                {
                    oReturn.ItemId = 346; //Resolución de Autorretenedor (Si aplica, formato PDF)
                }
                else if (urlFile.Contains("certificación") || urlFile.Contains("bancaria") || urlFile.Contains("certificacion"))
                {
                    oReturn.ItemId = 347; //Certificación Bancaria (No mayor a 2 meses, formato PDF)
                }
                else if (urlFile.Contains("ministerio") && urlFile.Contains("transporte"))
                {
                    oReturn.ItemId = 368; //Resolución del Ministerio de Transporte (Si la empresa es de transporte - Aplica)
                }
                else
                {
                    oReturn.ItemId = 352; //Ingrese la documentación adicional que desee adjuntar
                }
            }
            else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_Balance.Contains(SettingsName))
            {
                //grilla
                oReturn.ItemId = 351; //Estados Financieros (deben traer firmas del Contador, Representante Legal y Revisor Fiscal)
            }
            else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_ExperienceActivities.Contains(SettingsName))
            {
                //grilla
                oReturn.ItemId = 349; //Experiencias Comerciales
            }
            else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_HSE.Contains(SettingsName))
            {
                //archivos de HSE
                if (urlFile.Contains("segudidad") || urlFile.Contains("ambiente"))
                {
                    oReturn.ItemId = 353; //Política de Seguridad, Salud y Ambiente
                }
                else if (urlFile.Contains("no alcohol") || urlFile.Contains("drogas") || urlFile.Contains("fumadores"))
                {
                    oReturn.ItemId = 354; //Política de no alcohol, drogas y fumadores
                }
                else if (urlFile.Contains("ocupacional"))
                {
                    oReturn.ItemId = 355; //|Programa de Salud Ocupacional
                }
                else if (urlFile.Contains("reglamento") || urlFile.Contains("higiene") || urlFile.Contains("industrial"))
                {
                    oReturn.ItemId = 356; //Reglamento de Higiene y Seguridad Industrial
                }
                else if (urlFile.Contains("matriz"))
                {
                    oReturn.ItemId = 357; //Matriz de identificación de peligros, evaluación y control de riesgos
                }
                else if (urlFile.Contains("responsabilidad social"))
                {
                    oReturn.ItemId = 358; //Responsabilidad Social Empresarial
                }
                else if (urlFile.Contains("seguridad empresarial") || urlFile.Contains("empresarial y logistica"))
                {
                    oReturn.ItemId = 359; //Programa de Seguridad Empresarial y Logística
                }
                else if (urlFile.Contains("contratacion") || urlFile.Contains("politica") || urlFile.Contains("política"))
                {
                    oReturn.ItemId = 360; //Política de Contratación de Personal
                }
                else if (urlFile.Contains("arl") || urlFile.Contains("accidentalidad"))
                {
                    oReturn.ItemId = 361; //Certificado Accidentalidad ARL
                }
                else
                {
                    oReturn.ItemId = 352; //Ingrese la documentación adicional que desee adjuntar
                }
            }
            else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_QualityActivities.Contains(SettingsName))
            {
                //grilla
                oReturn.ItemId = 362; //Certificaciones de Calidad (ISO, OHSAS, etc)
            }
            else if (Crawler.Manager.Models.Constants.C_Settings_CrawlerUrl_DetailInfo_BasicInfo.Contains(SettingsName))
            {
                if (urlFile.Contains("representante") && urlFile.Contains("documento") || urlFile.Contains("c c") || urlFile.Contains("c.c.") || urlFile.Contains("c.c") || urlFile.Contains(" c c"))
                {
                    oReturn.ItemId = 348; //Documento de Identidad del Representante Legal (Formato PDF)
                }
                else
                {
                    oReturn.ItemId = 352; //Ingrese la documentación adicional que desee adjuntar
                }
            }
            return oReturn;
        }
    }
}
