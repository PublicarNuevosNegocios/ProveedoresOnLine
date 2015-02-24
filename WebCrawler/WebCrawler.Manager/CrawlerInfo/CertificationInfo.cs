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
    public class CertificationInfo
    {
        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> GetCertificationInfo(string ParId, string PublicId)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCertifications = null;

            HtmlDocument HtmlDoc = WebCrawler.Manager.WebCrawlerManager.GetHtmlDocumnet(ParId, enumMenu.Certifications.ToString());

            if (HtmlDoc.DocumentNode.SelectNodes("//table/tr") != null)
            {
                oCertifications = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

                HtmlNodeCollection table = HtmlDoc.DocumentNode.SelectNodes("//table");
                HtmlNodeCollection rows = table[1].SelectNodes(".//tr");

                ProveedoresOnLine.Company.Models.Util.GenericItemModel oCertificationsInfo = null;

                Console.WriteLine("\nCertifications Info");
                
                for (int i = 1; i < rows.Count; i++)
                {
                    HtmlNodeCollection cols = rows[i].SelectNodes(".//td");

                    oCertificationsInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                    {
                        ItemId = 0,
                        ItemName = string.Empty,
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumHSEQType.Certifications,
                        },
                        Enable = true,
                        ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                    };

                    //Get certification company
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oCertificationCompany = Util.CertificationCompany_GetByName(cols[2].InnerText.ToString());

                    oCertificationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumHSEQInfoType.C_CertificationCompany,
                        },
                        Value = oCertificationCompany == null ? string.Empty : oCertificationCompany.ItemId.ToString(),
                        Enable = true,
                    });

                    //Get certification name
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oCertificationName = Util.Certifications_GetByName(cols[3].InnerText.ToString());

                    oCertificationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumHSEQInfoType.C_Rule,
                        },
                        Value = oCertificationName == null ? string.Empty : oCertificationName.ItemId.ToString(),
                        Enable = true,
                    });

                    oCertificationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumHSEQInfoType.C_StartDateCertification,
                        },
                        Value = cols[5].InnerText != string.Empty ? Convert.ToDateTime(cols[5].InnerText).ToString("yyyy-MM-dd") : string.Empty,
                        Enable = true,
                    });

                    oCertificationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumHSEQInfoType.C_EndDateCertification,
                        },
                        Value = cols[6].InnerText != string.Empty ? Convert.ToDateTime(cols[6].InnerText).ToString("yyyy-MM-dd") : string.Empty,
                        Enable = true,
                    });

                    oCertificationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumHSEQInfoType.C_CCS,
                        },
                        Value = cols[4].InnerText.ToString() != "&nbsp;" ? cols[4].InnerText.ToString() : string.Empty,
                        Enable = true,
                    });

                    string urlS3 = string.Empty;
                    if (cols[8].InnerHtml.Contains("href") && cols[8].InnerHtml.Contains(".pdf"))
                    {
                        string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;

                        if (cols[8].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                        {
                            urlDownload = cols[8].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                        }

                        urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.Certifications.ToString(), PublicId);

                        oCertificationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.C_CertificationFile,
                            },
                            Value = urlS3,
                            Enable = true
                        });
                    }

                    oCertificationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumHSEQInfoType.C_Scope,
                        },
                        LargeValue = cols[7].InnerText.ToString() != "&nbsp;" ? cols[7].InnerText.ToString() : string.Empty,
                        Enable = true,
                    });

                    if (oCertificationsInfo.ItemInfo != null && oCertificationsInfo.ItemInfo.Count > 0)
                    {
                        oCertifications.Add(oCertificationsInfo);
                    }
                }
                if (oCertificationsInfo == null)
                {
                    Console.WriteLine("\nCertifications no tiene datos disponibles.\n");
                    return null;
                }
            }

            return oCertifications;
        }
    }
}
