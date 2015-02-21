using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                HtmlNodeCollection rowsTable1 = table[0].SelectNodes(".//tr");//CompanyRiskPolicies Form

                ProveedoresOnLine.Company.Models.Util.GenericItemModel oCompanyCompanyRiskPoliciesInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                {
                    ItemId = 0,
                    ItemName = string.Empty,
                    ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                    {
                        ItemId = (int)enumHSEQType.CompanyRiskPolicies,
                    },
                    Enable = true,
                    ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                };

                if (rowsTable1 != null)
                {
                    foreach (HtmlNode node in table[0].SelectNodes(".//select[@name='en_arp']"))
                    {
                        string[] optionList = node.InnerHtml.Split(new char[] { '<' });
                        string option = optionList.Where(x => x.Contains("selected")).FirstOrDefault();
                        option = option.Replace("option", "").Replace("value", "").Replace("selected", "");

                        option = option.Normalize(NormalizationForm.FormD);
                        Regex reg = new Regex("[^a-zA-Z0-9 ]");
                        option = reg.Replace(option.Replace(" ", ""), "");

                        //Remove id
                        option = Regex.Replace(option, @"[\d-]", string.Empty);

                        //Get ARL company
                        ProveedoresOnLine.Company.Models.Util.GenericItemModel oCompanyCertification = Util.CompanyARL_GetByName(option);

                        oCompanyCompanyRiskPoliciesInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CR_SystemOccupationalHazards,
                            },
                            Value = oCompanyCertification.ItemName,
                            Enable = true,
                        });
                    }

                    foreach (HtmlNode node in table[0].SelectNodes(".//select[@name='valor_arp']"))
                    {
                        string[] optionList = node.InnerHtml.Split(new char[] { '<' });
                        string option = optionList.Where(x => x.Contains("selected")).FirstOrDefault();
                        option = option.Replace("option", "").Replace("value", "").Replace("selected", ",");

                        string[] opt = option.Split(new char[] { ',' });

                        option = opt[1].Normalize(NormalizationForm.FormD);
                        Regex reg = new Regex("[^a-zA-Z0-9 ]");
                        option = reg.Replace(option.Replace(" ", ""), "");

                        //Get Rate arl
                        ProveedoresOnLine.Company.Models.Util.CatalogModel oRateARL = Util.ProviderOptions_GetByName(212, option);

                        oCompanyCompanyRiskPoliciesInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CR_RateARL,
                            },
                            Value = oRateARL.ItemId.ToString(),
                            Enable = true,
                        });
                    }
                }

                HtmlNodeCollection rowsTable2 = table[1].SelectNodes(".//tr"); //CompanyHealtyPolitic - CertificatesAccident

                if (rowsTable2 != null)
                {
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oCertificationInfo = null;
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oCertificationAccidentInfo = null;

                    for (int i = 1; i < rowsTable2.Count; i++)
                    {
                        oCertificationInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = 0,
                            ItemName = string.Empty,
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQType.CompanyHealtyPolitic,
                            },
                            Enable = true,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        };

                        oCertificationAccidentInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = 0,
                            ItemName = string.Empty,
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQType.CertificatesAccident,
                            },
                            Enable = true,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        };

                        HtmlNodeCollection cols = rowsTable2[i].SelectNodes(".//td");

                        oCertificationInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CH_Year,
                            },
                            Value = cols[0].InnerText.ToString() != "&nbsp;" ? cols[0].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        if (cols[6].InnerHtml.Contains("href") && cols[6].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[6].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[6].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCertificationInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CH_PoliticsSecurity,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (cols[7].InnerHtml.Contains("href") && cols[7].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[7].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[7].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCertificationInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CH_PoliticsNoAlcohol,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (cols[8].InnerHtml.Contains("href") && cols[8].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[8].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[8].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCompanyCompanyRiskPoliciesInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CR_CertificateAffiliateARL,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (cols[9].InnerHtml.Contains("href") && cols[9].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[9].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[9].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCertificationInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CH_ProgramOccupationalHealth
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (cols[10].InnerHtml.Contains("href") && cols[10].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[10].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[10].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCertificationInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CH_RuleIndustrialSecurity,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (cols[11].InnerHtml.Contains("href") && cols[11].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[11].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[11].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCertificationInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CH_MatrixRiskControl,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (cols[12].InnerHtml.Contains("href") && cols[12].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[12].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[12].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCertificationInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CH_CorporateSocialResponsability,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (cols[12].InnerHtml.Contains("href") && cols[12].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[12].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[12].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCertificationInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CH_ProgramEnterpriseSecurity,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (cols[14].InnerHtml.Contains("href") && cols[14].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[14].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[14].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCertificationInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CH_PoliticsRecruiment,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (cols[16].InnerHtml.Contains("href") && cols[16].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[16].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[16].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCertificationInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CH_CertificationsForm,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        oCertificationAccidentInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CA_Year,
                            },
                            Value = cols[0].InnerText.ToString() != "&nbsp;" ? cols[0].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        if (cols[15].InnerHtml.Contains("href") && cols[15].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[15].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[15].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumHSEQType.CompanyHealtyPolitic.ToString(), PublicId);

                            oCertificationAccidentInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumHSEQInfoType.CA_CertificateAccidentARL,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        oCertificationAccidentInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CA_ManHoursWorked,
                            },
                            Value = cols[1].InnerText.ToString() != "&nbsp;" ? cols[1].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oCertificationAccidentInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CA_Fatalities,
                            },
                            Value = cols[2].InnerText.ToString() != "&nbsp;" ? cols[2].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oCertificationAccidentInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CA_NumberAccident,
                            },
                            Value = cols[3].InnerText.ToString() != "&nbsp;" ? cols[3].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oCertificationAccidentInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CA_NumberAccidentDisabling,
                            },
                            Value = cols[4].InnerText.ToString() != "&nbsp;" ? cols[4].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oCertificationAccidentInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CA_DaysIncapacity,
                            },
                            Value = cols[5].InnerText.ToString() != "&nbsp;" ? cols[5].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        //Add Certification Info
                        if (oCertificationInfo.ItemInfo.Count > 0)
                        {
                            oHSEQInfo.Add(oCertificationInfo);
                        }

                        //Add Certification Accident Info
                        if (oCertificationAccidentInfo.ItemInfo.Count > 0)
                        {
                            oHSEQInfo.Add(oCertificationAccidentInfo);
                        }
                    }
                }
                //Add Company Company Risk Policies Info
                if (oCompanyCompanyRiskPoliciesInfo.ItemInfo.Count > 0)
                {
                    oHSEQInfo.Add(oCompanyCompanyRiskPoliciesInfo);
                }
            }
            return oHSEQInfo;
        }
    }
}
