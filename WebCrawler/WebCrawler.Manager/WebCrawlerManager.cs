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

                Console.WriteLine("\n Descargando Información de " + settings);

                string strHtml = oWebClient.DownloadString(oParURL);

                HtmlDocument HtmlDoc = new HtmlDocument();
                HtmlDoc.LoadHtml(strHtml);

                if (settings == enumMenu.GeneralInfo.ToString())
                {
                    GetGeneralInfo(HtmlDoc);
                }
            }
        }

        #region General Info

        public static void GetGeneralInfo(HtmlDocument HtmlDoc)
        {
            if (HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']") != null)
            {
                HtmlNodeCollection table = HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']");
                HtmlNodeCollection cells = table[0].SelectNodes("//tr/td");
                string a = string.Empty;
                foreach (var c in cells)
                {
                    HtmlAttribute att = c.Attributes["input"];
                    if (att != null)
                    {
                        if (att.Value.Contains("nit2"))
                        {
                            a += att.Value.ToString() + "\n";
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("la sección " + enumMenu.GeneralInfo.ToString() + " no tiene información para descargar." + "\n");
            }
        }

        #endregion

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
