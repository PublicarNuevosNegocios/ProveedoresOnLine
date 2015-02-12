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
            Console.WriteLine("Registro del proveedor: " + PublicId + " y ParId: " + ParId + "\n");
            Console.WriteLine("Start Date: " + DateTime.Now.ToString() + "\n");

            ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel();

            List<string> oSettingsList = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_CrawlerTags].Value.
               Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
               ToList();
            
            //redirect submenu
            foreach (var item in oSettingsList)
            {
                if (item.ToString() == enumMenu.GeneralInfo.ToString())
                {
                    oProvider.RelatedCompany = GeneralInfo(ParId, PublicId);
                }
            }

            try
            {
                //Provider upsert
                //oProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.ProviderUpsert(oProvider);
                Console.WriteLine("Se agregó el proveedor " + oProvider.RelatedCompany.CompanyName + "\n");

                //Relation Provider with Publicar
                //SetCompanyProvider(oProvider.RelatedCompany.CompanyPublicId);

                //Update search filters
                //ProveedoresOnLine.Company.Controller.Company.CompanySearchFill(oProvider.RelatedCompany.CompanyPublicId);
                //ProveedoresOnLine.Company.Controller.Company.CompanyFilterFill(oProvider.RelatedCompany.CompanyPublicId);
            }
            catch (System.Exception e)
            {                
                Console.WriteLine("Error, " + e.Message + "\n");
            }

            Console.WriteLine("Finalizó el proceso. " + DateTime.Now.ToString() + "\n");
        }

        #region Get Html Document

        public static HtmlDocument GetHtmlDocumnet(string ParId, string Setting)
        {
            HtmlDocument htmlDoc = new HtmlDocument();

            MyWebClient oWebClient = new MyWebClient();

            oWebClient.Headers.Add("Cookie", WebCrawler.Manager.General.InternalSettings.Instance
                                                [Constants.C_Settings_SessionKey].
                                                Value);

            string oParURL = WebCrawler.Manager.General.InternalSettings.Instance
                    ["CrawlerURL_" + Setting].
                    Value.
                    Replace("{{ParProviderId}}", ParId);

            string strHtml = oWebClient.DownloadString(oParURL);

            htmlDoc.LoadHtml(strHtml);

            return htmlDoc;
        }

        #endregion

        #region General Info

        public static ProveedoresOnLine.Company.Models.Company.CompanyModel GeneralInfo(string ParId, string ProviderPublicId)
        {
            ProveedoresOnLine.Company.Models.Company.CompanyModel oCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel();

            HtmlDocument HtmlDoc = GetHtmlDocumnet(ParId, enumMenu.GeneralInfo.ToString());

            if (HtmlDoc.DocumentNode.SelectNodes("//input") != null) // General Info
            {
                foreach (HtmlNode node in HtmlDoc.DocumentNode.SelectNodes("//input"))
                {
                    HtmlAttribute AttId = node.Attributes["id"];
                    HtmlAttribute AttValue = node.Attributes["value"];
                    if (AttId != null && AttValue != null)
                    {
                        //get provider nit
                        if (AttId.Value == "nit2")//Nit
                        {
                            oCompany.IdentificationNumber = AttValue.Value.ToString();
                        }
                        else if (AttId.Value == "b")//Dígito de Verificación
                        {
                            oCompany.IdentificationNumber += AttValue.Value.ToString();
                        }
                        else if (AttId.Value == "razon")//Razón Social
                        {
                            oCompany.CompanyName = AttValue.Value.ToString();
                        }
                        else if (AttId.Value == "razon2")//Nombre Comercial
                        {
                            oCompany.CompanyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumCompanyInfoType.ComercialName,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                }
                            };
                        }
                    }
                }
            }
            else if (HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']/tr") != null)
            {
                foreach (HtmlNode node in HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']/tr"))
                {
                    foreach (HtmlNode item in node.SelectNodes("//td"))
                    {
                        string a = item.ToString();
                    }
                }
            }
            else
            {
                Console.WriteLine("la sección " + enumMenu.GeneralInfo.ToString() + " no tiene información para descargar." + "\n");
            }

            oCompany.CompanyInfo.Add(
                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                {
                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                    {
                        ItemId = (int)enumCompanyInfoType.ProviderPaymentInfo,
                    },
                    Value = Convert.ToString((int)enumCategoryInfoType.NotApplicate),
                });

            oCompany.IdentificationType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            {
                ItemId = (int)enumCategoryInfoType.NIT,
            };
            oCompany.CompanyType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            {
                ItemId = (int)enumCategoryInfoType.Provider,
            };
            oCompany.Enable = true;

            return oCompany;
        }

        #endregion

        #region Commercial Info

        #endregion

        #region Certification Info

        #endregion

        #region Finantial Info

        #endregion

        #region BalanceSheet Info

        #endregion

        #region Legal Info

        #endregion

        #region Upload File

        public static string UploadFile(
            string ProviderPublicId,
            string UrlFile,
            string UrlNewFile)
        {
            string strRemoteFile = string.Empty;

            string strFile = string.Empty;

            strFile = UrlNewFile +
                "\\ProviderFile_" +
                ProviderPublicId + "_" +
                "0" + "_" +
                DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            File.Copy(UrlFile, strFile);

            //load file to s3
            strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                        (strFile,
                        WebCrawler.Manager.General.InternalSettings.Instance
                        [WebCrawler.Manager.General.Constants.C_Settings_File_RemoteDirectoryProvider].Value +
                            ProviderPublicId + "\\");

            File.Delete(strFile);

            return strRemoteFile;
        }

        #endregion

        #region Asociate to Customer Publicar

        public static void SetCompanyProvider(string ProviderId)
        {
            //Create Provider By Customer Publicar
            ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel oCustomerModel = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel();
            oCustomerModel.RelatedProvider = new List<ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerProviderModel>();

            oCustomerModel.RelatedProvider.Add(new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerProviderModel()
            {
                RelatedProvider = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = ProviderId,
                },
                Status = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = Convert.ToInt32(enumProviderCustomerStatus.Creation),
                },
                Enable = true,
            });

            oCustomerModel.RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(WebCrawler.Manager.General.InternalSettings.Instance[WebCrawler.Manager.General.Constants.C_Settings_PublicarPublicId].Value);

            ProveedoresOnLine.CompanyCustomer.Controller.Customer.CustomerProviderUpsert(oCustomerModel);
        }

        #endregion

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
