using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Manager.General;

namespace WebCrawler.Manager.CrawlerInfo
{
    public class ExperiencesInfo
    {
        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> GetExperiencesInfo(string ParId, string PublicId)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = null;

            HtmlDocument HtmlDoc = WebCrawler.Manager.WebCrawlerManager.GetHtmlDocumnet(ParId, enumMenu.Experience.ToString());

            if (HtmlDoc.DocumentNode.SelectNodes("//table") != null)
            {
                HtmlNodeCollection table = HtmlDoc.DocumentNode.SelectNodes("//table");

                if (table[2] != null)
                {
                    Dictionary<string, string> oUrlList = new Dictionary<string, string>();

                    HtmlNodeCollection rowsTable1 = table[2].SelectNodes(".//tr");//CompanyRiskPolicies Form
 
                    foreach (HtmlNode node in table[2].SelectNodes("//td"))
                    {
                        if (node.InnerHtml.Contains("<table"))
                        {
                            foreach (HtmlNode n2 in node.SelectNodes(".//table/tr"))
                            {
                                if (n2.InnerHtml.Contains("javascript:"))
                                {
                                    HtmlNodeCollection cols = n2.SelectNodes(".//td");

                                    string[] oExperience = cols[0].ChildNodes["a"].Attributes["href"].Value.ToString().Split(new char[] { '\'' });

                                    oUrlList.Add(cols[2].InnerText.ToString(), oExperience[1].ToString().Replace("..", WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value.ToString()));

                                    Console.WriteLine("url -> " + cols[0].ChildNodes["a"].Attributes["href"].Value + "\n");
                                    Console.WriteLine("contrato -> " + cols[2].InnerText + "\n");
                                }                                
                            }   
                        }                                             
                    }

                    //foreach (HtmlNode node in table[2].SelectNodes("//a[@href]"))
                    //{
                    //    HtmlAttribute att = node.Attributes["href"];
                    //    if (att.Value.Contains("javascript:"))
                    //    {
                    //        string[] oExperience = att.Value.ToString().Split(new char[] { '\'' });

                    //        oUrlList.Add("", oExperience[1].ToString().Replace("..", WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value.ToString()));
                    //    }
                    //}
                }
            }

            return oReturn;
        }
    }
}
