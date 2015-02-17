using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Manager.General;

namespace WebCrawler.Manager.CrawlerInfo
{
    public class HSEQInfo
    {
        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> GetHSEQInfo(string ParId, string PublicId)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oHSEQInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            HtmlDocument HtmlDoc = WebCrawler.Manager.WebCrawlerManager.GetHtmlDocumnet(ParId, enumMenu.HSE.ToString());

            if (HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']") != null)
            {
                HtmlNodeCollection table = HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']");
                HtmlNodeCollection rowsTable1 = table[0].SelectNodes(".//tr");
                
                foreach (HtmlNode node in HtmlDoc.DocumentNode.SelectNodes("//select[@name='en_arp']"))
                {
                    Console.WriteLine("outer html " + node.OuterHtml.ToString());
                }
            }

            return oHSEQInfo;
        }
    }
}
