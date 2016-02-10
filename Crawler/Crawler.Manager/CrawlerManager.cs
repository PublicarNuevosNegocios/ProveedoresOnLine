using Crawler.Manager.Models;
using HtmlAgilityPack;
using ProveedoresOnLine.Company.Models.Util;
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
            Console.WriteLine("Start Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n");

            //System.Net.WebClient oWebClient = new System.Net.WebClient();

            MyWebClient oWebClient = new MyWebClient();

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

                                Console.WriteLine("Start download: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                oWebClient.DownloadFile(urlDownload, folder);
                                Console.WriteLine("End download: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n");

                                //Integración con Document Management
                                UploadFile(ProviderPublicId, folder, folderSave, settings, NewRealtedProviderInfo);

                                System.Threading.Thread.Sleep(
                                    Convert.ToInt32(Crawler.Manager.Models.InternalSettings.Instance
                                    [Crawler.Manager.Models.Constants.C_Settings_TimerSleep].Value));

                                Console.WriteLine(message);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error! " + e.Message + " Archivo:" + att.Value.ToString() + "\n");
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
                Console.WriteLine("Se ha guardado el Proveedor");
                Console.WriteLine("End Date: " + DateTime.Now.ToString("dddd/MM/yyyy HH:mm:ss") + "\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error! " + e.Message + " No se puede guardar la información del proveedor." + "\n");
                Console.WriteLine("End Date: " + DateTime.Now.ToString("dddd/MM/yyyy HH:mm:ss"));
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
                            ProviderPublicId + "\\");
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

        public static void addQuestions()
        {
            List<string> oProviderPublicId = new List<string>();

            #region ProviderList

            //oProviderPublicId.Add("1882BED8");
            //oProviderPublicId.Add("1250DA28");
            //oProviderPublicId.Add("165490C9");
            //oProviderPublicId.Add("1435D2EA");
            //oProviderPublicId.Add("20D4BF66");
            //oProviderPublicId.Add("21633AAC");
            //oProviderPublicId.Add("195CFEF5");
            //oProviderPublicId.Add("22251703");
            //oProviderPublicId.Add("72FFEF86");
            //oProviderPublicId.Add("15BD9EC3"); Cuadrar
            //oProviderPublicId.Add("429B1C5C");
            //oProviderPublicId.Add("3388EB00");
            //oProviderPublicId.Add("340E25B4");
            //oProviderPublicId.Add("20640CF2");
            //oProviderPublicId.Add("AD638C88");
            //oProviderPublicId.Add("DCDA13A1");
            //oProviderPublicId.Add("138AC326");
            //oProviderPublicId.Add("E8C8B871");
            //oProviderPublicId.Add("2A762DCE");
            //oProviderPublicId.Add("12B6313F");
            //oProviderPublicId.Add("202075A2");
            //oProviderPublicId.Add("176506B8");
            //oProviderPublicId.Add("9FDE76E6");
            //oProviderPublicId.Add("2D6F780A");
            //oProviderPublicId.Add("1FD2AF08");
            //oProviderPublicId.Add("5102B05A");
            //oProviderPublicId.Add("1E07C733");
            //oProviderPublicId.Add("1269D88B");

            //David
            //oProviderPublicId.Add("80FBBE1B");
            //oProviderPublicId.Add("1758E4AB");
            //oProviderPublicId.Add("231EB383");
            //oProviderPublicId.Add("90DB5704");
            //oProviderPublicId.Add("15AC4A73");
            //oProviderPublicId.Add("16E833B4");
            //oProviderPublicId.Add("8B845F7A");
            //oProviderPublicId.Add("7354C447");
            //oProviderPublicId.Add("29F7F855");
            //oProviderPublicId.Add("4EFD0B97");
            //oProviderPublicId.Add("421686F4");
            //oProviderPublicId.Add("7F0C6ABE");
            //oProviderPublicId.Add("1C757486");
            //oProviderPublicId.Add("7CC6ED17");
            //oProviderPublicId.Add("1E5514CE");
            //oProviderPublicId.Add("11080CED");
            //oProviderPublicId.Add("901100F8");
            //oProviderPublicId.Add("232A1476");
            //oProviderPublicId.Add("12FF44F2");
            //oProviderPublicId.Add("2A007FF5");
            //oProviderPublicId.Add("DAFDDB1D");
            //oProviderPublicId.Add("1508ECCB");
            //oProviderPublicId.Add("211680E4");
            //oProviderPublicId.Add("225CEA6B");
            //oProviderPublicId.Add("1818D40B"); ok
            //oProviderPublicId.Add("DBC2BD55"); Revisar
            //oProviderPublicId.Add("19F43DF2");
            //oProviderPublicId.Add("1C8E63AC");
            //oProviderPublicId.Add("1770E6A2");
            //oProviderPublicId.Add("27216CCC");
            //oProviderPublicId.Add("2F327B54");
            //oProviderPublicId.Add("100DD42C");
            //oProviderPublicId.Add("135BD603");
            //oProviderPublicId.Add("2460E1AC");
            //oProviderPublicId.Add("D1294CBC");
            //oProviderPublicId.Add("A4D057C0");
            //oProviderPublicId.Add("58AE7197");
            //oProviderPublicId.Add("15A234BF");
            //oProviderPublicId.Add("1FE6AA91"); revisar
            //oProviderPublicId.Add("F58F387E");
            //oProviderPublicId.Add("1AA7DED7");
            //oProviderPublicId.Add("126457FE");
            //oProviderPublicId.Add("EBDAE5D9");
            //oProviderPublicId.Add("2EDAAFDA");
            //oProviderPublicId.Add("9C727E6C");
            //oProviderPublicId.Add("10A22F76");
            //oProviderPublicId.Add("22B8ED65");
            //oProviderPublicId.Add("11420F8E");
            //oProviderPublicId.Add("1FAF80D7");
            //oProviderPublicId.Add("15772F1E");
            //oProviderPublicId.Add("1F151B81");
            //oProviderPublicId.Add("33763928");
            //oProviderPublicId.Add("D94B0C0D");
            //oProviderPublicId.Add("15E85CAD");
            //oProviderPublicId.Add("13AEEC93"); revisar
            //oProviderPublicId.Add("20BB9A27");
            //oProviderPublicId.Add("1904C10B");
            //oProviderPublicId.Add("7DBBA454");
            //oProviderPublicId.Add("16DDE2CB");
            //oProviderPublicId.Add("34C3CDBD");
            //oProviderPublicId.Add("BEED759C");
            //oProviderPublicId.Add("11C065C1");
            //oProviderPublicId.Add("28D1440D");
            //oProviderPublicId.Add("15243175"); revisar
            //oProviderPublicId.Add("18EA1B5C");
            //oProviderPublicId.Add("898585C9");
            //oProviderPublicId.Add("1B4EF664");
            //oProviderPublicId.Add("15DADFF7");
            //oProviderPublicId.Add("A905FABC");
            //oProviderPublicId.Add("370D3568");
            //oProviderPublicId.Add("1C9188F9");
            //oProviderPublicId.Add("3058F3F8");
            //oProviderPublicId.Add("1D87D2ED");revisar
            //oProviderPublicId.Add("203A94EB");
            //oProviderPublicId.Add("AF34CE8B");
            //oProviderPublicId.Add("170A1055");
            //oProviderPublicId.Add("20AACF20");
            //oProviderPublicId.Add("1F9D4C05"); revisar
            //oProviderPublicId.Add("922F5E7B");
            //oProviderPublicId.Add("1BC63880");
            //oProviderPublicId.Add("E7A2293A");
            //oProviderPublicId.Add("14792B22");
            //oProviderPublicId.Add("1DE1CCD2");
            //oProviderPublicId.Add("53B1A7BC");
            //oProviderPublicId.Add("9CCF2522");
            //oProviderPublicId.Add("E7559B04");
            //oProviderPublicId.Add("9792B569"); revisar
            //oProviderPublicId.Add("1EF1E71C");
            //oProviderPublicId.Add("94C8546F");
            //oProviderPublicId.Add("ABD8BC92");
            //oProviderPublicId.Add("289853E4");
            //oProviderPublicId.Add("9D737633");
            //oProviderPublicId.Add("283AAE5C");
            //oProviderPublicId.Add("E3D0160D");
            //oProviderPublicId.Add("18F95F44");
            //oProviderPublicId.Add("229DF4FE");
            //oProviderPublicId.Add("1C835522");
            //oProviderPublicId.Add("14AA63B0");
            //oProviderPublicId.Add("13457DE0");
            //oProviderPublicId.Add("D776C455");
            //oProviderPublicId.Add("1F4CE586");
            //oProviderPublicId.Add("877F89C1");
            //oProviderPublicId.Add("96D9CEB8");
            //oProviderPublicId.Add("234CE493"); revisar
            //oProviderPublicId.Add("1DE7FFB6");
            //oProviderPublicId.Add("16C2EA5C");
            //oProviderPublicId.Add("E6B6780B");
            //oProviderPublicId.Add("19EF9D83");
            //oProviderPublicId.Add("1B8CA06B");
            //oProviderPublicId.Add("8C7CFA03");
            //oProviderPublicId.Add("18D3FE68");
            //oProviderPublicId.Add("12C007E3");
            //oProviderPublicId.Add("1962B4E5"); revisar
            //oProviderPublicId.Add("21C16F33");
            //oProviderPublicId.Add("2ED1540E");
            //oProviderPublicId.Add("9998D267");
            //oProviderPublicId.Add("CFB8AFFA");
            //oProviderPublicId.Add("221DAD63");
            //oProviderPublicId.Add("1F2A4461");
            //oProviderPublicId.Add("C93C7FF6");
            //oProviderPublicId.Add("2A5AB362");
            //oProviderPublicId.Add("16EC39D4"); revisar
            //oProviderPublicId.Add("18DFD61E");
            //oProviderPublicId.Add("10A9A1F3");
            //oProviderPublicId.Add("1B2BD9A9");
            //oProviderPublicId.Add("128B1018");
            //oProviderPublicId.Add("2F3806B8");
            //oProviderPublicId.Add("934A7B29");
            //oProviderPublicId.Add("EEFF09D4");
            //oProviderPublicId.Add("2AE7CB67");
            oProviderPublicId.Add("4AE4D328");
            oProviderPublicId.Add("3CA734E4");
            oProviderPublicId.Add("AB11A1A4");
            oProviderPublicId.Add("1EEA15B2");
            oProviderPublicId.Add("19FB3A04");
            oProviderPublicId.Add("1EB79D65");
            oProviderPublicId.Add("294DEDCF");
            oProviderPublicId.Add("FA59A94F");
            oProviderPublicId.Add("18A19DCD");
            oProviderPublicId.Add("28936B98");
            oProviderPublicId.Add("12C99B52");
            oProviderPublicId.Add("21021F1E");
            oProviderPublicId.Add("20700EFB");
            oProviderPublicId.Add("34C651B4");
            oProviderPublicId.Add("3B2DD3F9");
            oProviderPublicId.Add("1ACD5732");
            oProviderPublicId.Add("D65A9F83");
            oProviderPublicId.Add("E058A105");
            oProviderPublicId.Add("1B2D6C51");
            oProviderPublicId.Add("16029C99");
            oProviderPublicId.Add("20B1A7D1");
            oProviderPublicId.Add("962C77EC");
            oProviderPublicId.Add("4CDD7AFD");
            oProviderPublicId.Add("1DF6FCBC");
            oProviderPublicId.Add("1C84C43E");
            oProviderPublicId.Add("207C21C5");
            oProviderPublicId.Add("17FCA6E2");
            oProviderPublicId.Add("F26929EE");
            oProviderPublicId.Add("9806F4E8");
            oProviderPublicId.Add("3F678CDD");
            oProviderPublicId.Add("3FAA2A42");
            oProviderPublicId.Add("3FECC790");
            oProviderPublicId.Add("4072025A");
            oProviderPublicId.Add("40B49FBF");
            oProviderPublicId.Add("2122AB32");
            oProviderPublicId.Add("4139DA73");
            oProviderPublicId.Add("212FFDE0");
            oProviderPublicId.Add("35243EBF");
            oProviderPublicId.Add("18A9D9A8");
            oProviderPublicId.Add("2780D195");
            oProviderPublicId.Add("219A9345");
            oProviderPublicId.Add("18C47F02");
            oProviderPublicId.Add("18D1D1AD");
            oProviderPublicId.Add("FF51412E");
            oProviderPublicId.Add("FFBBD69C");
            oProviderPublicId.Add("10091015");
            oProviderPublicId.Add("A09D3E3A");
            oProviderPublicId.Add("47FDD62E");
            oProviderPublicId.Add("782EBDD8");
            oProviderPublicId.Add("C0F551D2");
            oProviderPublicId.Add("7903E891");
            oProviderPublicId.Add("22411CB4");
            oProviderPublicId.Add("2247C609");
            oProviderPublicId.Add("224E6F60");
            oProviderPublicId.Add("19785B1C");
            oProviderPublicId.Add("1985ADC8");
            oProviderPublicId.Add("19930076");
            oProviderPublicId.Add("10BCEC2F");
            oProviderPublicId.Add("AAC337BD");
            oProviderPublicId.Add("111A2EE9");
            oProviderPublicId.Add("8441AA3B");
            oProviderPublicId.Add("84AC3FA9");
            oProviderPublicId.Add("D4F154F3");
            oProviderPublicId.Add("113B7D9A");
            oProviderPublicId.Add("53F61D47");
            oProviderPublicId.Add("547B5811");
            oProviderPublicId.Add("54BDF560");
            oProviderPublicId.Add("550092C5");
            oProviderPublicId.Add("232A4382");
            oProviderPublicId.Add("23738A37");
            oProviderPublicId.Add("237A338E");
            oProviderPublicId.Add("8F7F6C08");
            oProviderPublicId.Add("6A5732D0");
            oProviderPublicId.Add("845CAE3E");
            oProviderPublicId.Add("1270C3B5");
            oProviderPublicId.Add("D26D17DC");
            oProviderPublicId.Add("17A50EBA");
            oProviderPublicId.Add("1AED65FF");
            oProviderPublicId.Add("1B36ACB4");
            oProviderPublicId.Add("1B43FF62");
            oProviderPublicId.Add("1B51520E");
            oProviderPublicId.Add("1B5EA4BB");
            oProviderPublicId.Add("1281E71E");
            oProviderPublicId.Add("1295E323");
            oProviderPublicId.Add("1B7FF36A");
            oProviderPublicId.Add("1B8D4617");
            oProviderPublicId.Add("1BDD3623");
            oProviderPublicId.Add("1BF13226");
            oProviderPublicId.Add("1321C737");
            oProviderPublicId.Add("1328708E");
            oProviderPublicId.Add("132F19E4");
            oProviderPublicId.Add("134315E7");
            oProviderPublicId.Add("67FF72FE");
            oProviderPublicId.Add("A7A544F1");
            oProviderPublicId.Add("6BE6AB8F");
            oProviderPublicId.Add("AD0EDB20");
            oProviderPublicId.Add("6CAE83A8");
            oProviderPublicId.Add("14D6F3B5");
            oProviderPublicId.Add("155C2E7E");
            oProviderPublicId.Add("15E16932");
            oProviderPublicId.Add("B1A34673");
            oProviderPublicId.Add("1666A3FC");
            oProviderPublicId.Add("19436710");
            oProviderPublicId.Add("19C8A1DA");
            oProviderPublicId.Add("1A0B3F28");
            oProviderPublicId.Add("1A4DDC8D");
            oProviderPublicId.Add("2AEB588C");
            oProviderPublicId.Add("2B55EDD7");
            oProviderPublicId.Add("1B9AEF70");
            oProviderPublicId.Add("1BDD8CD5");
            oProviderPublicId.Add("1C202A23");
            oProviderPublicId.Add("1D80E262");
            oProviderPublicId.Add("1D878BB9");
            oProviderPublicId.Add("1DD0D26E");
            oProviderPublicId.Add("1DDE251C");
            oProviderPublicId.Add("1DE4CE71");
            oProviderPublicId.Add("1DF2211E");
            oProviderPublicId.Add("1DF8CA75");
            oProviderPublicId.Add("1E1A1923");
            oProviderPublicId.Add("1E276BD1");
            oProviderPublicId.Add("1E2E1528");
            oProviderPublicId.Add("1E7E0534");
            oProviderPublicId.Add("1E84AE89");
            oProviderPublicId.Add("1E8B57E0");
            oProviderPublicId.Add("1E920137");
            oProviderPublicId.Add("1E98AA8E");
            oProviderPublicId.Add("30FEEC9D");
            oProviderPublicId.Add("1EA5FD39");
            oProviderPublicId.Add("1EACA690");
            oProviderPublicId.Add("1EB34FE7");
            oProviderPublicId.Add("1EB9F93C");
            oProviderPublicId.Add("1EC0A292");
            oProviderPublicId.Add("1EC74BE9");
            oProviderPublicId.Add("31498867");
            oProviderPublicId.Add("1ED49E95");
            oProviderPublicId.Add("31C96EE1");
            oProviderPublicId.Add("1F248EA1");
            oProviderPublicId.Add("1F2B37F8");
            oProviderPublicId.Add("1F45DD51");
            oProviderPublicId.Add("1F4C86A8");
            oProviderPublicId.Add("1F532FFF");
            oProviderPublicId.Add("1F6082AB");
            oProviderPublicId.Add("15D80BEF");
            oProviderPublicId.Add("DADB0CA3");
            oProviderPublicId.Add("890B854B");
            oProviderPublicId.Add("DBB03780");
            oProviderPublicId.Add("E044A2D3");
            oProviderPublicId.Add("8C6D8328");
            oProviderPublicId.Add("8CB0208D");
            oProviderPublicId.Add("8CF2BDDC");
            oProviderPublicId.Add("8DBA960B");
            oProviderPublicId.Add("8DFD335A");
            oProviderPublicId.Add("8E3FD0BF");
            oProviderPublicId.Add("16CD3057");
            oProviderPublicId.Add("E46E78DB");
            oProviderPublicId.Add("3B5EA366");
            oProviderPublicId.Add("1737A12A");
            oProviderPublicId.Add("173E4A81");
            oProviderPublicId.Add("17879136");
            oProviderPublicId.Add("178E3A8D");
            oProviderPublicId.Add("1794E3E4");
            oProviderPublicId.Add("179B8D39");
            oProviderPublicId.Add("17A23690");
            oProviderPublicId.Add("25DAFFD8");
            oProviderPublicId.Add("17AF893E");
            oProviderPublicId.Add("3428F5A0");
            oProviderPublicId.Add("20A042DB");
            oProviderPublicId.Add("20A6EC32");
            oProviderPublicId.Add("20B43EDD");
            oProviderPublicId.Add("20BAE834");
            oProviderPublicId.Add("20C1918B");
            oProviderPublicId.Add("3473916A");
            oProviderPublicId.Add("34E8CF59");
            oProviderPublicId.Add("408E3A4E");
            oProviderPublicId.Add("40D0D7B3");
            oProviderPublicId.Add("41137518");
            oProviderPublicId.Add("4156127D");
            oProviderPublicId.Add("4198AFCC");
            oProviderPublicId.Add("41DB4D31");
            oProviderPublicId.Add("421DEA96");
            oProviderPublicId.Add("426087FB");
            oProviderPublicId.Add("9B852ABA");
            oProviderPublicId.Add("9BC7C81F");
            oProviderPublicId.Add("F9AA3C06");
            oProviderPublicId.Add("9C4D02E9");
            oProviderPublicId.Add("9C8FA037");
            oProviderPublicId.Add("27C548A9");
            oProviderPublicId.Add("18E1F6C0");
            oProviderPublicId.Add("18E8A017");
            oProviderPublicId.Add("18EF496E");
            oProviderPublicId.Add("18F5F2C3");
            oProviderPublicId.Add("18FC9C19");
            oProviderPublicId.Add("21E6AC62");
            oProviderPublicId.Add("21ED55B9");
            oProviderPublicId.Add("21F3FF0E");
            oProviderPublicId.Add("365DDA3B");
            oProviderPublicId.Add("A28BC3DA");
            oProviderPublicId.Add("A2CE6129");
            oProviderPublicId.Add("A310FE8E");
            oProviderPublicId.Add("10552931");
            oProviderPublicId.Add("109E6FE7");
            oProviderPublicId.Add("1988802F");
            oProviderPublicId.Add("198F2986");
            oProviderPublicId.Add("1995D2DB");
            oProviderPublicId.Add("199C7C32");
            oProviderPublicId.Add("22868C7A");
            oProviderPublicId.Add("228D35D1");
            oProviderPublicId.Add("3752FEA3");
            oProviderPublicId.Add("229A887D");
            oProviderPublicId.Add("22A131D4");
            oProviderPublicId.Add("22A7DB2A");
            oProviderPublicId.Add("506DBA0E");
            oProviderPublicId.Add("50B05773");
            oProviderPublicId.Add("50F2F4D8");
            oProviderPublicId.Add("53CFB7EC");
            oProviderPublicId.Add("1BAC377B");
            oProviderPublicId.Add("11524C01");
            oProviderPublicId.Add("1158F558");
            oProviderPublicId.Add("ADBC32DA");
            oProviderPublicId.Add("1A49AEF8");
            oProviderPublicId.Add("1A50584C");
            oProviderPublicId.Add("1A5701A3");
            oProviderPublicId.Add("234111EC");
            oProviderPublicId.Add("1A6AFDA6");
            oProviderPublicId.Add("AFD11DD5");
            oProviderPublicId.Add("57745318");
            oProviderPublicId.Add("1B1AE374");
            oProviderPublicId.Add("C36D8056");
            oProviderPublicId.Add("91C17D5F");
            oProviderPublicId.Add("5B5B8BC0");
            oProviderPublicId.Add("5B9E290F");
            oProviderPublicId.Add("93013D86");
            oProviderPublicId.Add("121A2420");
            oProviderPublicId.Add("1220CD77");
            oProviderPublicId.Add("122776CC");
            oProviderPublicId.Add("1D1699D2");
            oProviderPublicId.Add("1B18306B");
            oProviderPublicId.Add("1B1ED9C2");
            oProviderPublicId.Add("2B6F3825");
            oProviderPublicId.Add("7F06F289");
            oProviderPublicId.Add("8330C775");
            oProviderPublicId.Add("875A9DC4");
            oProviderPublicId.Add("A0DD8B31");
            oProviderPublicId.Add("A1202896");
            oProviderPublicId.Add("19D25D1D");
            oProviderPublicId.Add("2815AA5B");
            oProviderPublicId.Add("282052E6");
            oProviderPublicId.Add("191ADD26");
            oProviderPublicId.Add("2212401A");
            oProviderPublicId.Add("BF9A5444");
            oProviderPublicId.Add("4DF7A963");
            oProviderPublicId.Add("A75EE987");
            oProviderPublicId.Add("10C35A4A");
            oProviderPublicId.Add("19AD6A91");
            oProviderPublicId.Add("19B413E8");
            oProviderPublicId.Add("19C16695");
            oProviderPublicId.Add("22AB76DC");
            oProviderPublicId.Add("22B22033");
            oProviderPublicId.Add("81BE762C");
            oProviderPublicId.Add("5159A72A");
            oProviderPublicId.Add("AD186FC5");
            oProviderPublicId.Add("AD5B0D14");
            oProviderPublicId.Add("1A53F400");
            oProviderPublicId.Add("56D09003");
            oProviderPublicId.Add("1A893EB2");
            oProviderPublicId.Add("1A28113D");
            oProviderPublicId.Add("1224692B");
            oProviderPublicId.Add("12BD9FEC");
            oProviderPublicId.Add("1BBBAC37");
            oProviderPublicId.Add("A162D049");
            oProviderPublicId.Add("1C3A3D9F");
            oProviderPublicId.Add("10ED6ACB");
            oProviderPublicId.Add("1392CAB7");
            oProviderPublicId.Add("1CD37460");
            oProviderPublicId.Add("16A6F0F3");
            oProviderPublicId.Add("6FCB93C8");
            oProviderPublicId.Add("142C0178");
            oProviderPublicId.Add("1D23646C");
            oProviderPublicId.Add("194116B8");
            oProviderPublicId.Add("1DA89F2B");
            oProviderPublicId.Add("1F3D3A44");
            oProviderPublicId.Add("153D204D");
            oProviderPublicId.Add("1543C9A4");
            oProviderPublicId.Add("242EE853");
            oProviderPublicId.Add("157FBDAB");
            oProviderPublicId.Add("82029BE9");
            oProviderPublicId.Add("15FE4F15");
            oProviderPublicId.Add("2B781EC2");
            oProviderPublicId.Add("163A431C");
            oProviderPublicId.Add("1F7AECC8");
            oProviderPublicId.Add("DCEC43E7");
            oProviderPublicId.Add("8A5647D5");
            oProviderPublicId.Add("3665F074");
            oProviderPublicId.Add("174B61F1");
            oProviderPublicId.Add("17520B48");
            oProviderPublicId.Add("9224B8F8");
            oProviderPublicId.Add("EA3EF095");
            oProviderPublicId.Add("17DDEF5D");
            oProviderPublicId.Add("3E346196");
            oProviderPublicId.Add("3E76FEFB");
            oProviderPublicId.Add("1819E365");
            oProviderPublicId.Add("18208CBC");
            oProviderPublicId.Add("210A9D04");
            oProviderPublicId.Add("6B70192C");
            oProviderPublicId.Add("4368AD0A");
            oProviderPublicId.Add("189874CF");
            oProviderPublicId.Add("2764FD06");
            oProviderPublicId.Add("457D9805");
            oProviderPublicId.Add("45C0356A");
            oProviderPublicId.Add("9F6A12F3");
            oProviderPublicId.Add("191DAF8E");
            oProviderPublicId.Add("4AB1E379");
            oProviderPublicId.Add("106F934F");
            oProviderPublicId.Add("1959A395");
            oProviderPublicId.Add("22510689");
            oProviderPublicId.Add("4D096BD9");
            oProviderPublicId.Add("19D18BA8");
            oProviderPublicId.Add("22BB9BEF");
            oProviderPublicId.Add("51B87C99");
            oProviderPublicId.Add("11161CBC");
            oProviderPublicId.Add("8AA9DDDA");
            oProviderPublicId.Add("1194AE26");
            oProviderPublicId.Add("38A7B121");
            oProviderPublicId.Add("236F780B");
            oProviderPublicId.Add("11C34F82");
            oProviderPublicId.Add("11C9F8D8");
            oProviderPublicId.Add("BA55ED79");
            oProviderPublicId.Add("E3F83EAF");
            oProviderPublicId.Add("4AC834E5");
            oProviderPublicId.Add("1B329A89");
            oProviderPublicId.Add("1B3943DE");
            oProviderPublicId.Add("3D162B6C");
            oProviderPublicId.Add("65E6E895");
            oProviderPublicId.Add("5F831148");
            oProviderPublicId.Add("993C4AAF");
            oProviderPublicId.Add("127DD4F3");
            oProviderPublicId.Add("1B6E8E90");
            oProviderPublicId.Add("6BECD20F");
            oProviderPublicId.Add("B0D7F334");
            oProviderPublicId.Add("64322208");
            oProviderPublicId.Add("A0BACBE2");
            oProviderPublicId.Add("12F5BD04");
            oProviderPublicId.Add("1BDFCD4D");
            oProviderPublicId.Add("1BE676A3");
            oProviderPublicId.Add("CDFCCC97");
            oProviderPublicId.Add("835825D0");
            oProviderPublicId.Add("A3A4E19F");
            oProviderPublicId.Add("13245E62");
            oProviderPublicId.Add("132B07B7");
            oProviderPublicId.Add("1C1517FF");
            oProviderPublicId.Add("ACFA816E");
            oProviderPublicId.Add("6A70E2FA");
            oProviderPublicId.Add("138EF3C7");
            oProviderPublicId.Add("13959D1C");
            oProviderPublicId.Add("1C8656BC");
            oProviderPublicId.Add("131E8DBA");
            oProviderPublicId.Add("AD384DB3");
            oProviderPublicId.Add("ADA2E322");
            oProviderPublicId.Add("13C43E7A");
            oProviderPublicId.Add("13CAE7CF");
            oProviderPublicId.Add("1CB4F817");
            oProviderPublicId.Add("D1689208");
            oProviderPublicId.Add("17CD9E7A");
            oProviderPublicId.Add("1CEA0C9F");
            oProviderPublicId.Add("20559521");
            oProviderPublicId.Add("20603DAC");
            oProviderPublicId.Add("1D2636D4");
            oProviderPublicId.Add("1D2CE02B");
            oProviderPublicId.Add("199FEC11");
            oProviderPublicId.Add("B7A0E4A4");
            oProviderPublicId.Add("1A67C429");
            oProviderPublicId.Add("1D54D830");
            oProviderPublicId.Add("1D622ADD");
            oProviderPublicId.Add("1DB21AE9");
            oProviderPublicId.Add("1ED43784");
            oProviderPublicId.Add("31BE2176");
            oProviderPublicId.Add("C05F25FF");
            oProviderPublicId.Add("14F002A6");
            oProviderPublicId.Add("14F6ABFD");
            oProviderPublicId.Add("1DE0BC45");
            oProviderPublicId.Add("1DE7659C");
            oProviderPublicId.Add("20E92280");
            oProviderPublicId.Add("C3493BBC");
            oProviderPublicId.Add("7A5062BA");
            oProviderPublicId.Add("15254D58");
            oProviderPublicId.Add("1E51FB02");
            oProviderPublicId.Add("1E58A456");
            oProviderPublicId.Add("CA5D2781");
            oProviderPublicId.Add("CAC7BCCC");
            oProviderPublicId.Add("20818B56");
            oProviderPublicId.Add("5E4A92D1");
            oProviderPublicId.Add("94133E98");
            oProviderPublicId.Add("17B8C9BE");
            oProviderPublicId.Add("17BF7312");
            oProviderPublicId.Add("20EC20BB");
            oProviderPublicId.Add("3F1871B9");
            oProviderPublicId.Add("3F5B0F1E");
            oProviderPublicId.Add("26B44FB1");
            oProviderPublicId.Add("211AC217");
            oProviderPublicId.Add("67DDFEE5");
            oProviderPublicId.Add("68489453");
            oProviderPublicId.Add("9A51FF8A");
            oProviderPublicId.Add("26F442F0");
            oProviderPublicId.Add("2142BA1C");
            oProviderPublicId.Add("21496373");
            oProviderPublicId.Add("455732AA");
            oProviderPublicId.Add("FD92EF33");
            oProviderPublicId.Add("9EBE72E5");
            oProviderPublicId.Add("18C9E890");
            oProviderPublicId.Add("21B3F8D9");
            oProviderPublicId.Add("46E6E2F2");
            oProviderPublicId.Add("71DC0067");
            oProviderPublicId.Add("A04E2316");
            oProviderPublicId.Add("27E96758");
            oProviderPublicId.Add("18F889EE");
            oProviderPublicId.Add("36375D21");
            oProviderPublicId.Add("102FC856");
            oProviderPublicId.Add("195C75FD");
            oProviderPublicId.Add("19631F53");
            oProviderPublicId.Add("224D2F9A");
            oProviderPublicId.Add("4CE3067E");
            oProviderPublicId.Add("A607A954");
            oProviderPublicId.Add("19846E01");
            oProviderPublicId.Add("28DE8BC1");
            oProviderPublicId.Add("227527A1");
            oProviderPublicId.Add("7D84577F");
            oProviderPublicId.Add("A7975985");
            oProviderPublicId.Add("A7D9F6EA");
            oProviderPublicId.Add("19B30F5F");
            oProviderPublicId.Add("5259EF57");
            oProviderPublicId.Add("529C8CA6");
            oProviderPublicId.Add("112CEB25");
            oProviderPublicId.Add("1A16FB6E");
            oProviderPublicId.Add("23010BB7");
            oProviderPublicId.Add("53E99F88");
            oProviderPublicId.Add("542C3CED");
            oProviderPublicId.Add("AD50DFC3");
            oProviderPublicId.Add("1A3EF373");
            oProviderPublicId.Add("232903BC");
            oProviderPublicId.Add("55794FD0");
            oProviderPublicId.Add("892CAE97");
            oProviderPublicId.Add("AEE08FF4");
            oProviderPublicId.Add("1AA988D8");
            oProviderPublicId.Add("316B2609");
            oProviderPublicId.Add("59A325C6");
            oProviderPublicId.Add("11E0C740");
            oProviderPublicId.Add("1AD180E0");
            oProviderPublicId.Add("148E0031");
            oProviderPublicId.Add("5B32D5F7");
            oProviderPublicId.Add("1208BF47");
            oProviderPublicId.Add("1AF2CF90");
            oProviderPublicId.Add("2190BC2C");
            oProviderPublicId.Add("242AE21E");
            oProviderPublicId.Add("98944649");
            oProviderPublicId.Add("127354AD");
            oProviderPublicId.Add("1B5D64F5");
            oProviderPublicId.Add("9AA9314D");
            oProviderPublicId.Add("1DBA9EFB");
            oProviderPublicId.Add("129B4CB2");
            oProviderPublicId.Add("2C0894C4");
            oProviderPublicId.Add("5ACFFA95");
            oProviderPublicId.Add("9D28B19C");
            oProviderPublicId.Add("12BC9B62");
            oProviderPublicId.Add("1BA6ABAA");
            oProviderPublicId.Add("6620A7A9");
            oProviderPublicId.Add("666344F7");
            oProviderPublicId.Add("132730C7");
            oProviderPublicId.Add("9175130D");
            oProviderPublicId.Add("67B057DA");
            oProviderPublicId.Add("13487F77");
            oProviderPublicId.Add("1C328FC0");
            oProviderPublicId.Add("9E77CF08");
            oProviderPublicId.Add("68FD6ABC");
            oProviderPublicId.Add("A8667369");
            oProviderPublicId.Add("1370777C");
            oProviderPublicId.Add("49405F19");
            oProviderPublicId.Add("C81A2AA6");
            oProviderPublicId.Add("AEA53451");
            oProviderPublicId.Add("13D4638D");
            oProviderPublicId.Add("1CBE73D3");
            oProviderPublicId.Add("6E745395");
            oProviderPublicId.Add("13F5B23B");
            oProviderPublicId.Add("1CDFC284");
            oProviderPublicId.Add("6FC16661");
            oProviderPublicId.Add("336E16D6");
            oProviderPublicId.Add("1D43AE92");
            oProviderPublicId.Add("1A83FC33");
            oProviderPublicId.Add("73A89F09");
            oProviderPublicId.Add("147AECFA");
            oProviderPublicId.Add("1D64FD43");
            oProviderPublicId.Add("1BD10F16");
            oProviderPublicId.Add("1C13AC7B");
            oProviderPublicId.Add("75384F3A");
            oProviderPublicId.Add("14A2E501");
            oProviderPublicId.Add("1D8CF54A");
            oProviderPublicId.Add("1D60BF47");
            oProviderPublicId.Add("14BD8A5A");
            oProviderPublicId.Add("1DEA3801");
            oProviderPublicId.Add("21055A8A");
            oProviderPublicId.Add("C376620F");
            oProviderPublicId.Add("15217669");
            oProviderPublicId.Add("1E0B86B2");
            oProviderPublicId.Add("22526D6C");
            oProviderPublicId.Add("C58B4D13");
            oProviderPublicId.Add("1E262C0B");
            oProviderPublicId.Add("235CE2EA");
            oProviderPublicId.Add("7C8185A9");
            oProviderPublicId.Add("155D6A73");
            oProviderPublicId.Add("1E8A1819");
            oProviderPublicId.Add("27441B7B");
            oProviderPublicId.Add("CD746391");
            oProviderPublicId.Add("1EAB66CA");
            oProviderPublicId.Add("28912E5D");
            oProviderPublicId.Add("CF894E95");
            oProviderPublicId.Add("15E2A531");
            oProviderPublicId.Add("1ECCB57A");
            oProviderPublicId.Add("14F638F5");
            oProviderPublicId.Add("1603F3E2");
            oProviderPublicId.Add("1F30A188");
            oProviderPublicId.Add("2DC579D1");
            oProviderPublicId.Add("D7DCFA81");
            oProviderPublicId.Add("1667DFF0");
            oProviderPublicId.Add("1F51F039");
            oProviderPublicId.Add("2F128C9E");
            oProviderPublicId.Add("D9F1E585");
            oProviderPublicId.Add("240EB101");
            oProviderPublicId.Add("3251FE3E");
            oProviderPublicId.Add("305F9F80");
            oProviderPublicId.Add("89844256");
            oProviderPublicId.Add("16AA7D4E");
            oProviderPublicId.Add("34043AC3");
            oProviderPublicId.Add("E1DAFC03");
            oProviderPublicId.Add("1707C008");
            oProviderPublicId.Add("1FF1D051");
            oProviderPublicId.Add("35514D8F");
            oProviderPublicId.Add("E3EFE708");
            oProviderPublicId.Add("17290EB9");
            oProviderPublicId.Add("20131EFF");
            oProviderPublicId.Add("369E6071");
            oProviderPublicId.Add("8FC30347");
            oProviderPublicId.Add("174A5D67");
            oProviderPublicId.Add("37A8D5EF");
            oProviderPublicId.Add("17A0F6CA");
            oProviderPublicId.Add("208B0712");
            oProviderPublicId.Add("3B4D7132");
            oProviderPublicId.Add("ED83531B");
            oProviderPublicId.Add("17C2457A");
            oProviderPublicId.Add("20AC55C2");
            oProviderPublicId.Add("3C9A83FE");
            oProviderPublicId.Add("17DCEAD3");
            oProviderPublicId.Add("20C6FB1C");
            oProviderPublicId.Add("3DA4F97B");
            oProviderPublicId.Add("182041F8");
            oProviderPublicId.Add("17FE3983");
            oProviderPublicId.Add("54E8B62F");
            oProviderPublicId.Add("68E05038");
            oProviderPublicId.Add("185B7C3B");
            oProviderPublicId.Add("4296A7A1");
            oProviderPublicId.Add("F92BAA33");
            oProviderPublicId.Add("187CCAEB");
            oProviderPublicId.Add("2166DB34");
            oProviderPublicId.Add("43E3BA6D");
            oProviderPublicId.Add("18977045");
            oProviderPublicId.Add("2181808D");
            oProviderPublicId.Add("7273BC4C");
            oProviderPublicId.Add("10114C0B");
            oProviderPublicId.Add("18FB5C53");
            oProviderPublicId.Add("21E56C9C");
            oProviderPublicId.Add("102BF164");
            oProviderPublicId.Add("191601AD");
            oProviderPublicId.Add("220011F5");
            oProviderPublicId.Add("49DFDE0F");
            oProviderPublicId.Add("104D4014");
            oProviderPublicId.Add("1937505D");
            oProviderPublicId.Add("4AEA538D");
            oProviderPublicId.Add("1067E56E");
            oProviderPublicId.Add("19949314");
            oProviderPublicId.Add("227EA35D");
            oProviderPublicId.Add("7E1C1364");
            oProviderPublicId.Add("29185A4A");
            oProviderPublicId.Add("229948B6");
            oProviderPublicId.Add("7FC668FA");
            oProviderPublicId.Add("10E676D5");
            oProviderPublicId.Add("19D0871E");
            oProviderPublicId.Add("50E67719");
            oProviderPublicId.Add("11011C2F");
            oProviderPublicId.Add("19EB2C77");
            oProviderPublicId.Add("548B1246");
            oProviderPublicId.Add("ADAFB51B");
            oProviderPublicId.Add("1A486F31");
            oProviderPublicId.Add("23327F78");
            oProviderPublicId.Add("AEBA2A99");
            oProviderPublicId.Add("1A63148B");
            oProviderPublicId.Add("234D24D1");
            oProviderPublicId.Add("AFC4A016");
            oProviderPublicId.Add("1A7DB9E4");
            oProviderPublicId.Add("38A61044");
            oProviderPublicId.Add("57ED1023");
            oProviderPublicId.Add("11B4F84C");
            oProviderPublicId.Add("F297589C");
            oProviderPublicId.Add("9282ABD7");
            oProviderPublicId.Add("12123B03");
            oProviderPublicId.Add("22AAEC90");
            oProviderPublicId.Add("942D016C");
            oProviderPublicId.Add("122CE05D");
            oProviderPublicId.Add("2D138378");
            oProviderPublicId.Add("95D75702");
            oProviderPublicId.Add("124785B6");
            oProviderPublicId.Add("1B3195FF");
            oProviderPublicId.Add("5EB10BDF");
            oProviderPublicId.Add("1B882F62");
            oProviderPublicId.Add("621309A6");
            oProviderPublicId.Add("12B8C472");
            oProviderPublicId.Add("1BA2D4BB");
            oProviderPublicId.Add("A3B79B38");
            oProviderPublicId.Add("12D369CC");
            oProviderPublicId.Add("1BBD7A14");
            oProviderPublicId.Add("B45EF311");
            oProviderPublicId.Add("12EE0F25");
            oProviderPublicId.Add("1BD81F6E");
            oProviderPublicId.Add("7B23EED2");
            oProviderPublicId.Add("134B51DD");
            oProviderPublicId.Add("9F91FF6C");
            oProviderPublicId.Add("1365F736");
            oProviderPublicId.Add("1C50077F");
            oProviderPublicId.Add("10FF756E");
            oProviderPublicId.Add("13809C8F");
            oProviderPublicId.Add("1C6AACD8");
            oProviderPublicId.Add("B4632D3B");
            oProviderPublicId.Add("139B41E9");
            oProviderPublicId.Add("1C855231");
            oProviderPublicId.Add("15AE8618");
            oProviderPublicId.Add("13F884A3");
            oProviderPublicId.Add("1CE294EB");
            oProviderPublicId.Add("16B8FB96");
            oProviderPublicId.Add("141329FC");
            oProviderPublicId.Add("1CFD3A45");
            oProviderPublicId.Add("B43BF0D3");
            oProviderPublicId.Add("142DCF55");
            oProviderPublicId.Add("1D17DF9E");
            oProviderPublicId.Add("B5E64669");
            oProviderPublicId.Add("144874AF");
            oProviderPublicId.Add("1D3284F7");
            oProviderPublicId.Add("BBBA7207");
            oProviderPublicId.Add("14A5B766");
            oProviderPublicId.Add("1D3A59EC");
            oProviderPublicId.Add("BD64C79D");
            oProviderPublicId.Add("2F6C6C4F");
            oProviderPublicId.Add("306E18A9");
            oProviderPublicId.Add("BF0F1D32");
            oProviderPublicId.Add("1DBE690B");
            oProviderPublicId.Add("1F4F44E7");
            oProviderPublicId.Add("14EEFE1E");
            oProviderPublicId.Add("22B142C5");
            oProviderPublicId.Add("1545977E");
            oProviderPublicId.Add("1E2FA7C7");
            oProviderPublicId.Add("23BBB842");
            oProviderPublicId.Add("15603CD8");
            oProviderPublicId.Add("1E4A4D20");
            oProviderPublicId.Add("C90D1EB4");
            oProviderPublicId.Add("157AE231");
            oProviderPublicId.Add("3C166FC1");
            oProviderPublicId.Add("14458BA1");
            oProviderPublicId.Add("2288D8DE");
            oProviderPublicId.Add("2932A11B");
            oProviderPublicId.Add("825743DB");
            oProviderPublicId.Add("2A3D1699");
            oProviderPublicId.Add("1606C647");
            oProviderPublicId.Add("1EF0D68F");
            oProviderPublicId.Add("D375B5A5");
            oProviderPublicId.Add("46147143");
            oProviderPublicId.Add("D5200B3B");
            oProviderPublicId.Add("1642BA50");
            oProviderPublicId.Add("1692AA5C");
            oProviderPublicId.Add("307BD78A");
            oProviderPublicId.Add("89A07A49");
            oProviderPublicId.Add("1F90B6A8");
            oProviderPublicId.Add("31864D08");
            oProviderPublicId.Add("16C14BB8");
            oProviderPublicId.Add("1FAB5C01");
            oProviderPublicId.Add("DF1E0CBD");
            oProviderPublicId.Add("16DBF112");
            oProviderPublicId.Add("52275DCA");
            oProviderPublicId.Add("E4F23837");
            oProviderPublicId.Add("201C9ABD");
            oProviderPublicId.Add("36FD35CA");
            oProviderPublicId.Add("25484C7D");
            oProviderPublicId.Add("20374017");
            oProviderPublicId.Add("90E9B0B8");
            oProviderPublicId.Add("1767D527");
            oProviderPublicId.Add("38CF8377");
            oProviderPublicId.Add("91F42636");
            oProviderPublicId.Add("33D63584");
            oProviderPublicId.Add("39D9F8F4");
            oProviderPublicId.Add("17D913E4");
            oProviderPublicId.Add("3D3BF6BC");
            oProviderPublicId.Add("F09A8F4F");
            oProviderPublicId.Add("20D7202F");
            oProviderPublicId.Add("3E466C39");
            oProviderPublicId.Add("1807B53F");
            oProviderPublicId.Add("3F0E4468");
            oProviderPublicId.Add("9832E727");
            oProviderPublicId.Add("2105C18A");
            oProviderPublicId.Add("4018B9E6");
            oProviderPublicId.Add("1836569B");
            oProviderPublicId.Add("21630442");
            oProviderPublicId.Add("FA9890D1");
            oProviderPublicId.Add("21770047");
            oProviderPublicId.Add("44852D2A");
            oProviderPublicId.Add("2772888C");
            oProviderPublicId.Add("2191A5A0");
            oProviderPublicId.Add("9E71A819");
            oProviderPublicId.Add("21A5A1A3");
            oProviderPublicId.Add("46577AD7");
            oProviderPublicId.Add("18D636B3");
            oProviderPublicId.Add("1042BFCE");
            oProviderPublicId.Add("192CD016");
            oProviderPublicId.Add("4A8150CD");
            oProviderPublicId.Add("105D6527");
            oProviderPublicId.Add("222ADC61");
            oProviderPublicId.Add("A46DCBBB");
            oProviderPublicId.Add("195B7172");
            oProviderPublicId.Add("4C539E63");
            oProviderPublicId.Add("A5784139");
            oProviderPublicId.Add("22597DBD");
            oProviderPublicId.Add("1A999DA6");
            oProviderPublicId.Add("19CCB02F");
            oProviderPublicId.Add("50C011BE");
            oProviderPublicId.Add("19E0AC31");
            oProviderPublicId.Add("22CABC7A");
            oProviderPublicId.Add("AAAC8CAD");
            oProviderPublicId.Add("19FB518A");
            oProviderPublicId.Add("52925F55");
            oProviderPublicId.Add("37F562EF");
            oProviderPublicId.Add("230D59D8");
            oProviderPublicId.Add("8B31511A");
            oProviderPublicId.Add("1A808C49");
            oProviderPublicId.Add("57C6AAC8");
            oProviderPublicId.Add("1C4E9BC7");
            oProviderPublicId.Add("237E9894");
            oProviderPublicId.Add("11C51D5F");
            oProviderPublicId.Add("1AAF2DA5");
            oProviderPublicId.Add("5998F85F");
            oProviderPublicId.Add("1AC329AA");
            oProviderPublicId.Add("EF3EE47A");
            oProviderPublicId.Add("5710DD53");
            oProviderPublicId.Add("1A825A26");
            oProviderPublicId.Add("236C6A6F");
            oProviderPublicId.Add("B0FD582B");
            oProviderPublicId.Add("23806671");
            oProviderPublicId.Add("11C6EB3C");
            oProviderPublicId.Add("1AB0FB82");
            oProviderPublicId.Add("59AB0302");
            oProviderPublicId.Add("1AC4F787");
            oProviderPublicId.Add("90B7C4F7");
            oProviderPublicId.Add("1AD8F389");
            oProviderPublicId.Add("96215B26");
            oProviderPublicId.Add("1B2F8CEC");
            oProviderPublicId.Add("126021FD");
            oProviderPublicId.Add("68283CF1");
            oProviderPublicId.Add("12741DFF");
            oProviderPublicId.Add("1B5E2E48");
            oProviderPublicId.Add("9A4B312E");
            oProviderPublicId.Add("1B722A4A");
            oProviderPublicId.Add("9B8AF156");
            oProviderPublicId.Add("12E55CBC");
            oProviderPublicId.Add("7524189D");
            oProviderPublicId.Add("12F958BE");
            oProviderPublicId.Add("C7EA75B8");
            oProviderPublicId.Add("65A34A31");
            oProviderPublicId.Add("1BF76509");
            oProviderPublicId.Add("A3DE9D42");
            oProviderPublicId.Add("491FE333");
            oProviderPublicId.Add("103A0D65");
            oProviderPublicId.Add("224A21FD");
            oProviderPublicId.Add("A5A683D0");
            oProviderPublicId.Add("225E1E02");
            oProviderPublicId.Add("10A4A2CA");
            oProviderPublicId.Add("198EB313");
            oProviderPublicId.Add("7D537DD7");
            oProviderPublicId.Add("19A2AF15");
            oProviderPublicId.Add("4F1C06BF");
            oProviderPublicId.Add("19B6AB17");
            oProviderPublicId.Add("22A0BB60");
            oProviderPublicId.Add("ABA2A75C");
            oProviderPublicId.Add("22F754C3");
            oProviderPublicId.Add("113DD98D");
            oProviderPublicId.Add("230B50C5");
            oProviderPublicId.Add("86E6E9EB");
            oProviderPublicId.Add("1A3BE5D6");
            oProviderPublicId.Add("8826AA13");
            oProviderPublicId.Add("1A4FE1DB");
            oProviderPublicId.Add("55E00264");
            oProviderPublicId.Add("1A63DDDD");
            oProviderPublicId.Add("234DEE26");
            oProviderPublicId.Add("11D7104F");
            oProviderPublicId.Add("B8E3D258");
            oProviderPublicId.Add("11EB0C51");
            oProviderPublicId.Add("135CAEF9");
            oProviderPublicId.Add("10FAF3F5");
            oProviderPublicId.Add("92F9D671");
            oProviderPublicId.Add("1AFD149F");
            oProviderPublicId.Add("94399699");
            oProviderPublicId.Add("1B1110A1");
            oProviderPublicId.Add("99A32CC8");
            oProviderPublicId.Add("2BD91007");
            oProviderPublicId.Add("60CDD416");
            oProviderPublicId.Add("1B7BA607");
            oProviderPublicId.Add("9C22AD17");
            oProviderPublicId.Add("1B8FA20B");
            oProviderPublicId.Add("5ED2F462");
            oProviderPublicId.Add("12C0371C");
            oProviderPublicId.Add("A4357057");
            oProviderPublicId.Add("12D4331E");
            oProviderPublicId.Add("88755000");
            oProviderPublicId.Add("132ACC81");
            oProviderPublicId.Add("9043C1D5");
            oProviderPublicId.Add("133EC884");
            oProviderPublicId.Add("F3505145");
            oProviderPublicId.Add("10B22F58");
            oProviderPublicId.Add("1C3CD4CF");
            oProviderPublicId.Add("1C50D0D1");
            oProviderPublicId.Add("1426B323");
            oProviderPublicId.Add("13D155F0");
            oProviderPublicId.Add("1CBB6637");
            oProviderPublicId.Add("6E132E11");
            oProviderPublicId.Add("2E189D2C");
            oProviderPublicId.Add("B15E7043");
            oProviderPublicId.Add("1CE35E3E");
            oProviderPublicId.Add("B29E308E");
            oProviderPublicId.Add("1CF75A40");
            oProviderPublicId.Add("B3DDF0B5");
            oProviderPublicId.Add("1D4DF3A3");
            oProviderPublicId.Add("73CCB44F");
            oProviderPublicId.Add("1D61EFA6");
            oProviderPublicId.Add("74948C67");
            oProviderPublicId.Add("1D75EBA8");
            oProviderPublicId.Add("755C6480");
            oProviderPublicId.Add("1D89E7AD");
            oProviderPublicId.Add("BD06C75B");
            oProviderPublicId.Add("1D9DE3AF");
            oProviderPublicId.Add("BE4687A6");
            oProviderPublicId.Add("1DF47D12");
            oProviderPublicId.Add("7A4E128F");
            oProviderPublicId.Add("1E087915");
            oProviderPublicId.Add("C4EFDDFC");
            oProviderPublicId.Add("1E1C7517");
            oProviderPublicId.Add("7BDDC2D6");
            oProviderPublicId.Add("1E30711A");
            oProviderPublicId.Add("7CA59AEF");
            oProviderPublicId.Add("1E446D1E");
            oProviderPublicId.Add("C8AF1E72");
            oProviderPublicId.Add("1E9B067F");
            oProviderPublicId.Add("80CF70E5");
            oProviderPublicId.Add("1EAF0284");
            oProviderPublicId.Add("819748FE");
            oProviderPublicId.Add("3137FDA4");
            oProviderPublicId.Add("1ED6FA89");
            oProviderPublicId.Add("8326F945");
            oProviderPublicId.Add("1EEAF68B");
            oProviderPublicId.Add("D317B563");
            oProviderPublicId.Add("2E2C2C66");
            oProviderPublicId.Add("23DF2C44");
            oProviderPublicId.Add("2EF4047E");
            oProviderPublicId.Add("167F77AD");
            oProviderPublicId.Add("2FBBDCAD");
            oProviderPublicId.Add("169373AF");
            oProviderPublicId.Add("4D9F87A3");
            oProviderPublicId.Add("16A76FB4");
            oProviderPublicId.Add("8A2D924F");
            oProviderPublicId.Add("1F9ED2A8");
            oProviderPublicId.Add("E27F4D15");
            oProviderPublicId.Add("33224675");
            oProviderPublicId.Add("E3BF0D3C");
            oProviderPublicId.Add("2009680D");
            oProviderPublicId.Add("8F1F405E");
            oProviderPublicId.Add("201D6410");
            oProviderPublicId.Add("175B4BCF");
            oProviderPublicId.Add("9410EE83");
            oProviderPublicId.Add("209BF578");
            oProviderPublicId.Add("EE27A42C");
            oProviderPublicId.Add("3C7BFBDF");

            #endregion

            int Count = 0;

            ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel();

            oProviderPublicId.All(x =>
            {
                oProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = x,
                    },
                    RelatedAditionalDocuments = new List<GenericItemModel>()
                    {
                        new GenericItemModel()
                        {
                            ItemId = 0,
                            ItemType = new CatalogModel()
                            {
                                ItemId = 1701002,
                            },
                            ItemName = "a) ¿CUÁLES SON LOS DIAS Y HORARIOS DEFINIDOS POR SU EMPRESA PARA LA RECEPCION DEL PRODUCTO?",
                            Enable = true,
                            ItemInfo = new List<GenericItemInfoModel>(),
                        },
                    },
                };

                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.Add(
                    new GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new CatalogModel()
                        {
                            ItemId = 1703004, //Related User
                        },
                        Value = "david.moncayo@publicar.com",
                        Enable = true,
                    });

                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.Add(
                        new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new CatalogModel()
                            {
                                ItemId = 1703003, //Related customer
                            },
                            Value = "10A0C1D5", //Clientes - Polipropileno
                            Enable = true,
                        });

                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.Add(
                        new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new CatalogModel()
                            {
                                ItemId = 1703001, //DataType
                            },
                            Value = "1901003",
                            Enable = true,
                        });

                //Upsert 1 Question
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                //Upsert 2 Question
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemId = 0;
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.All(y =>
                {
                    y.ItemInfoId = 0;
                    return true;
                });
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemName = "b) ¿CUÁLES SON LOS DOCUMENTOS REQUERIDOS POR SU EMPRESA, PARA REALIZAR LA ENTREGA DEL PRODUCTO?";
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                //Upsert 3 Question
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemId = 0;
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.All(y =>
                {
                    y.ItemInfoId = 0;
                    return true;
                });
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemName = "c) ¿ES NECESARIO REALIZAR SOLICITUD DE CITA PREVIA PARA LA ENTREGA DEL PRODUCTO? CON CUÁNTO TIEMPO DE ANTICIPACIÓN A LA FECHA DE ENTREGA SE DEBE SOLICITAR LA CITA Y CON QUIEN?";
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                //Upsert 4 Question
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemId = 0;
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.All(y =>
                {
                    y.ItemInfoId = 0;
                    return true;
                });
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemName = "d) ¿EN LA ENTREGA DEL PRODUCTO, USTEDES REQUIEREN AUXILIARES DE DESCARGUE? CUÁNTOS?";
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                //Upsert 5 Question
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemId = 0;
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.All(y =>
                {
                    y.ItemInfoId = 0;
                    return true;
                });
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemName = "e) ¿CUÁLES SON LOS ELEMENTOS DE SEGURIDAD QUE DEBEN TENER LOS AUXILIARES DE DESCARGUE? QUÉ DOCUMENTOS SON REQUERIDOS DEL AUXILIAR?";
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                //Upsert 6 Question
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemId = 0;
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.All(y =>
                {
                    y.ItemInfoId = 0;
                    return true;
                });
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemName = "f) ¿CUÁL ES EL DIA DE CIERRE CONTABLE EN SU EMPRESA? DESPUÉS DE ESTE DÍA USTEDES RECIBEN PRODUCTO?";
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                Count++;

                return true;
            });
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
