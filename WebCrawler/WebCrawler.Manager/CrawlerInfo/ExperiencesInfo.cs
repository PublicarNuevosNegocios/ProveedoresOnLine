using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Manager.General;
using System.Text.RegularExpressions;

namespace WebCrawler.Manager.CrawlerInfo
{
    public class ExperiencesInfo
    {
        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> GetExperiencesInfo(string ParId, string PublicId)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oExperiencesInfo = null;

            HtmlDocument HtmlDoc = WebCrawler.Manager.WebCrawlerManager.GetHtmlDocumnet(ParId, enumMenu.Experience.ToString());

            if (HtmlDoc.DocumentNode.SelectNodes("//table") != null)
            {
                HtmlNodeCollection table = HtmlDoc.DocumentNode.SelectNodes("//table");

                if (table[2] != null)
                {
                    Dictionary<string, Tuple<string, string>> oUrl = new Dictionary<string, Tuple<string, string>>();

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
                                    
                                    if (!oUrl.ContainsKey(cols[2].InnerText.ToString()))
                                    {
                                        string urlS3 = string.Empty;

                                        if (cols[11].InnerHtml.Contains(".pdf"))
                                        {
                                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;

                                            if (cols[11].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                                            {
                                                urlDownload = cols[11].ChildNodes["a"].Attributes["href"].Value.Replace("../", urlDownload);
                                                urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumCommercialType.Experience.ToString(), PublicId);
                                            }                                                
                                        }

                                        oUrl.Add(cols[2].InnerText, new Tuple<string, string>(oExperience[1].ToString().Replace("../", WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value.ToString()), urlS3));
                                    }
                                }
                            }
                        }
                    }

                    //get experiences info.
                    if (oUrl != null && oUrl.Count > 0)
                    {
                        oExperiencesInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

                        Console.WriteLine("\nExperiences Info\n");

                        foreach (string key in oUrl.Keys)
                        {
                            string url = oUrl.Where(x => x.Key == key).Select(x => x.Value.Item1).FirstOrDefault();
                            string urlS3 = oUrl.Where(x => x.Key == key).Select(x => x.Value.Item2).FirstOrDefault();

                            string strHtml = WebCrawler.Manager.WebCrawlerManager.oWebClient.DownloadString(url);
                            HtmlDocument htmlDoc = new HtmlDocument();
                            htmlDoc.LoadHtml(strHtml);

                            //Create Model
                            ProveedoresOnLine.Company.Models.Util.GenericItemModel oExperiences = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                            {
                                ItemId = 0,
                                ItemName = string.Empty,
                                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumCommercialType.Experience,
                                },
                                Enable = true,
                                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                            };                             

                            if (htmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']") != null)
                            {
                                HtmlNodeCollection t = htmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']");
                                
                                foreach (HtmlNode nodeSelectTipo in t[0].SelectNodes(".//select[@id='tipo']"))
                                {
                                    string[] optionList = nodeSelectTipo.InnerHtml.Split(new char[] { '<' });
                                    string option = string.Empty;
                                    option = optionList.Where(x => x.Contains("selected") != null).FirstOrDefault() != null ? optionList.Where(x => x.Contains("selected") != null).FirstOrDefault() : string.Empty;
                                    option = option.Replace("option", "").Replace("value", "").Replace("selected", "");

                                    option = option.Normalize(NormalizationForm.FormD);
                                    Regex reg = new Regex("[^a-zA-Z0-9 ]");
                                    option = reg.Replace(option.Replace(" ", ""), "");

                                    option = Regex.Replace(option, @"[\d-]", string.Empty);

                                    //Get contract type
                                    ProveedoresOnLine.Company.Models.Util.CatalogModel oGetContractType = Util.ProviderOptions_GetByName(303, option);

                                    oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                    {
                                        ItemInfoId = 0,
                                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = (int)enumCommercialInfoType.EX_ContractType,
                                        },
                                        Value = oGetContractType != null ? oGetContractType.ItemId.ToString() : string.Empty,
                                        Enable = true,
                                    });
                                }

                                foreach (HtmlNode nodeSelectMoneda in t[0].SelectNodes(".//select[@id='moneda']"))
                                {
                                    string[] optionList = nodeSelectMoneda.InnerHtml.Split(new char[] { '<' });
                                    string option = string.Empty;
                                    option = optionList.Where(x => x.Contains("selected") != null).FirstOrDefault() != null ? optionList.Where(x => x.Contains("selected") != null).FirstOrDefault() : string.Empty;
                                    option = option.Replace("option", "").Replace("value", "").Replace("selected", "");

                                    option = option.Normalize(NormalizationForm.FormD);
                                    Regex reg = new Regex("[^a-zA-Z0-9 ]");
                                    option = reg.Replace(option.Replace(" ", ""), "");

                                    option = Regex.Replace(option, @"[\d-]", string.Empty);

                                    //Get currency
                                    ProveedoresOnLine.Company.Models.Util.CatalogModel oCurrency = Util.ProviderOptions_GetByName(108, option);

                                    oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                    {
                                        ItemInfoId = 0,
                                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = (int)enumCommercialInfoType.EX_Currency,
                                        },
                                        Value = oCurrency != null ? oCurrency.ItemId.ToString() : string.Empty,
                                        Enable = true,
                                    });
                                }

                                DateTime date = new DateTime();

                                foreach (HtmlNode nodeInput in t[0].SelectNodes(".//input"))
                                {
                                    HtmlAttribute AttId = nodeInput.Attributes["id"];
                                    HtmlAttribute AttValue = nodeInput.Attributes["value"];

                                    if (AttId.Value == "c")
                                    {
                                        oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                        {
                                            ItemInfoId = 0,
                                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = (int)enumCommercialInfoType.EX_DateIssue,
                                            },
                                            Value =  DateTime.TryParse(AttValue.Value, out date) == true ? date.ToString("yyyy-MM-dd") : string.Empty,
                                            Enable = true,
                                        });
                                    }

                                    if (AttId.Value == "fffin")
                                    {
                                        oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                        {
                                            ItemInfoId = 0,
                                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = (int)enumCommercialInfoType.EX_DueDate,
                                            },
                                            Value =  DateTime.TryParse(AttValue.Value, out date) == true ? date.ToString("yyyy-MM-dd") : string.Empty,
                                            Enable = true,
                                        });
                                    }

                                    if (AttId.Value == "entidad")
                                    {
                                        oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                        {
                                            ItemInfoId = 0,
                                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = (int)enumCommercialInfoType.EX_Client,
                                            },
                                            Value = AttValue.Value,
                                            Enable = true,
                                        });
                                    }

                                    if (AttId.Value == "valor")
                                    {
                                        oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                        {
                                            ItemInfoId = 0,
                                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = (int)enumCommercialInfoType.EX_ContractValue,
                                            },
                                            Value = AttValue.Value,
                                            Enable = true,
                                        });
                                    }

                                    if (key != string.Empty)
                                    {
                                        oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                        {
                                            ItemInfoId = 0,
                                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = (int)enumCommercialInfoType.EX_ContractNumber,
                                            },
                                            Value = key,
                                            Enable = true,
                                        });
                                    }

                                    if (AttId.Value == "telefono")
                                    {
                                        oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                        {
                                            ItemInfoId = 0,
                                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = (int)enumCommercialInfoType.EX_Phone,
                                            },
                                            Value = AttValue.Value,
                                            Enable = true,
                                        });
                                    }

                                    if (urlS3 != string.Empty)
                                    {
                                        oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                        {
                                            ItemInfoId = 0,
                                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = (int)enumCommercialInfoType.EX_ExperienceFile,
                                            },
                                            Value = urlS3,
                                            Enable = true,
                                        });
                                    }

                                }

                                foreach (HtmlNode nodeArea in t[0].SelectNodes(".//textarea"))
                                {
                                    oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                    {
                                        ItemInfoId = 0,
                                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = (int)enumCommercialInfoType.EX_ContractSubject,
                                        },
                                        Value = nodeArea.InnerText,
                                        Enable = true,
                                    });
                                }

                                string defaultActivity = string.Empty;
                                string customActivity = string.Empty;

                                foreach (HtmlNode node in t[1].SelectNodes(".//tr"))
                                {
                                    if (node.InnerHtml.Contains("checked"))
                                    {
                                        string ourl = node.InnerHtml;

                                        foreach (HtmlNode n in node.SelectNodes(".//td/a"))
                                        {
                                            string values = n.InnerHtml;

                                            //Get custom or default activity
                                            ProveedoresOnLine.Company.Models.Util.GenericItemModel oActivity = Util.Activities_GetByName(n.InnerText);
                                            ProveedoresOnLine.Company.Models.Util.GenericItemModel oCustomActivity = Util.CustomActivities_GetByName(n.InnerText);

                                            if (oActivity != null)
                                            {
                                                defaultActivity += defaultActivity != string.Empty ? oActivity != null ? "," + oActivity.ItemId.ToString() : string.Empty : oActivity.ItemId.ToString();
                                            }
                                            if (oCustomActivity != null)
                                            {
                                                customActivity += customActivity != string.Empty ? oCustomActivity != null ? "," + oCustomActivity.ItemId.ToString() : string.Empty : oCustomActivity.ItemId.ToString();
                                            }
                                        }
                                    }
                                }

                                if (defaultActivity != string.Empty)
                                {
                                    oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                    {
                                        ItemInfoId = 0,
                                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = (int)enumCommercialInfoType.EX_EconomicActivity,
                                        },
                                        LargeValue = defaultActivity,
                                        Enable = true,
                                    });
                                }

                                if (customActivity != string.Empty)
                                {
                                    oExperiences.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                    {
                                        ItemInfoId = 0,
                                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = (int)enumCommercialInfoType.EX_CustomEconomicActivity,
                                        },
                                        LargeValue = customActivity,
                                        Enable = true,
                                    });
                                }

                            }

                            if (oExperiences != null && oExperiences.ItemInfo.Count > 0)
                            {
                                oExperiencesInfo.Add(oExperiences);
                            }
                        }
                    }
                }
            }

            if (oExperiencesInfo == null)
            {
                Console.WriteLine("Experiences no tiene datos disponibles.");
            }

            return oExperiencesInfo;
        }
    }
}
