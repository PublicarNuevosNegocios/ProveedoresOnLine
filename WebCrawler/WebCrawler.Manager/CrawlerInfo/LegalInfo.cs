using System;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Manager.General;

namespace WebCrawler.Manager.CrawlerInfo
{
    public class LegalInfo
    {
        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> GetLegalInfo(string ParId, string PublicId)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = null;

            HtmlDocument HtmlDoc = WebCrawler.Manager.WebCrawlerManager.GetHtmlDocumnet(ParId, enumMenu.LegalInfo.ToString());

            return oReturn;
        }
    }
}
