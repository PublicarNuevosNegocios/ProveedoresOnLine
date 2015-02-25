using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Manager.General;

namespace WebCrawler.Manager.CrawlerInfo
{
    public class BalancesInfo
    {
        #region Balances Info

        public static List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> GetBalancesInfo(string ParId, string PublicId)
        {
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> oBalancesInfo = null;

            HtmlDocument HtmlDoc = WebCrawler.Manager.WebCrawlerManager.GetHtmlDocumnet(ParId, enumMenu.GeneralBalance.ToString());

            return oBalancesInfo;
        }

        #endregion

        #region Finantial Info

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> GetFinantialInfo(string ParId, string PublicId)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oFinantial = null;

            HtmlDocument HtmlDoc = WebCrawler.Manager.WebCrawlerManager.GetHtmlDocumnet(ParId, enumMenu.LegalInfo.ToString());

            if (HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']") != null)
            {
                oFinantial = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

                HtmlNodeCollection table = HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']");

                HtmlNodeCollection rowsTable1 = table[3].SelectNodes(".//tr"); // Información Tributaria

                #region Income Statement

                if (rowsTable1 != null)
                {
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oIncomeStatementInfo = null;

                    Console.WriteLine("\nIncome Statement\n");

                    for (int i = 1; i < rowsTable1.Count; i++)
                    {
                        HtmlNodeCollection cols = rowsTable1[i].SelectNodes(".//td");

                        oIncomeStatementInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = 0,
                            ItemName = string.Empty,
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialType.IncomeStatementInfoType,
                            },
                            Enable = true,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        };

                        oIncomeStatementInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IS_Year,
                            },
                            Value = cols[0].InnerText.ToString() != "&nbsp;" ? cols[0].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oIncomeStatementInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IS_GrossIncome,
                            },
                            Value = cols[1].InnerText.ToString() != "&nbsp;" ? cols[1].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oIncomeStatementInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IS_NetIncome,
                            },
                            Value = cols[2].InnerText.ToString() != "&nbsp;" ? cols[2].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oIncomeStatementInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IS_GrossEstate,
                            },
                            Value = cols[3].InnerText.ToString() != "&nbsp;" ? cols[3].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oIncomeStatementInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IS_LiquidHeritage,
                            },
                            Value = cols[4].InnerText.ToString() != "&nbsp;" ? cols[4].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        if (cols[5].InnerHtml.Contains("href") && cols[5].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[5].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[5].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumFinancialType.IncomeStatementInfoType.ToString(), PublicId);

                            oIncomeStatementInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumFinancialInfoType.IS_FileIncomeStatement,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (oIncomeStatementInfo != null)
                        {
                            oFinantial.Add(oIncomeStatementInfo);
                        }
                    }

                    if (oIncomeStatementInfo == null)
                    {
                        Console.WriteLine("\nIncome Statement no tiene datos disponibles.\n");
                    }
                }

                #endregion

                HtmlNodeCollection rowsTable2 = table[5].SelectNodes(".//tr"); // Información Bancaria

                #region Bank Info

                if (rowsTable2 != null)
                {
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oBankInfo = null;

                    Console.WriteLine("\nBank Info\n");

                    for (int i = 1; i < rowsTable2.Count; i++)
                    {
                        oBankInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = 0,
                            ItemName = string.Empty,
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialType.BankInfoType,
                            },
                            Enable = true,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        };

                        HtmlNodeCollection cols = rowsTable2[i].SelectNodes(".//td");

                        //Get bank type
                        string BankName = cols[0].InnerText.ToString();

                        ProveedoresOnLine.Company.Models.Util.GenericItemModel oBankType = Util.Bank_GetByName(BankName);

                        oBankInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IB_Bank,
                            },
                            Value = oBankType.ItemId.ToString(),
                            Enable = true,
                        });

                        //Get account type
                        ProveedoresOnLine.Company.Models.Util.CatalogModel oAccountType = Util.ProviderOptions_GetByName(1001, cols[1].InnerText.ToString());

                        oBankInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IB_AccountType,
                            },
                            Value = oAccountType == null ? string.Empty : oAccountType.ItemId.ToString(),
                            Enable = true,
                        });

                        oBankInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IB_AccountNumber,
                            },
                            Value = cols[2].InnerText.ToString() != "&nbsp;" ? cols[2].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oBankInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IB_AccountHolder,
                            },
                            Value = cols[3].InnerText.ToString() != "&nbsp;" ? cols[3].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oBankInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IB_ABA,
                            },
                            Value = cols[6].InnerText.ToString() != "&nbsp;" ? cols[6].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oBankInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IB_Swift,
                            },
                            Value = cols[7].InnerText.ToString() != "&nbsp;" ? cols[7].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oBankInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IB_IBAN,
                            },
                            Value = cols[9].InnerText.ToString() != "&nbsp;" ? cols[9].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oBankInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.IB_Customer,
                            },
                            Value = string.Empty,
                            Enable = true,
                        });

                        if (cols[8].InnerHtml.Contains("href") && cols[8].InnerHtml.Contains(".pdf"))
                        {
                            string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                            string urlS3 = string.Empty;

                            if (cols[8].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                urlDownload = cols[8].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                            }

                            urlS3 = WebCrawler.Manager.WebCrawlerManager.UploadFile(urlDownload, enumFinancialType.IncomeStatementInfoType.ToString(), PublicId);

                            oBankInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumFinancialInfoType.IB_AccountFile,
                                },
                                Value = urlS3,
                                Enable = true,
                            });
                        }

                        if (oBankInfo.ItemInfo != null && oBankInfo.ItemInfo.Count > 0)
                        {
                            oFinantial.Add(oBankInfo);
                        }
                    }

                    if (oBankInfo == null)
                    {
                        Console.WriteLine("\nBank Info no tiene datos disponibles.\n");
                    }
                }

                #endregion
            }

            return oFinantial;
        }

        #endregion
    }
}
