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
    public class BalancesInfo
    {
        #region Balance Info

        public static List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> GetBalancesInfo(string ParId, string PublicId)
        {
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> oBalancesInfo = null;

            HtmlDocument HtmlDoc = WebCrawler.Manager.WebCrawlerManager.GetHtmlDocumnet(ParId, enumMenu.GeneralBalance.ToString());

            if (HtmlDoc.DocumentNode.SelectNodes("//table[@class='tablaborde_01']") != null)
            {
                oBalancesInfo = new List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel>();

                ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel oBalance = new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel()
                {
                    ItemId = 0,
                    ItemName = string.Empty,
                    ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                    {
                        ItemId = (int)enumFinancialType.BalanceSheetInfoType,
                    },
                    Enable = true,
                    ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                    BalanceSheetInfo = new List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel>(),
                };

                HtmlNodeCollection tables = HtmlDoc.DocumentNode.SelectNodes("//table[@class='tablaborde_01']");

                HtmlNodeCollection rowsTable1 = tables[0].SelectNodes(".//tr");//Activo

                #region Activo

                if (rowsTable1 != null)
                {
                    Console.WriteLine("\nActivo info\n");

                    #region Get Years - Currency

                    HtmlNodeCollection col = rowsTable1[0].SelectNodes(".//td");

                    string[] dato1 = col[1].InnerText.Split(new char[] { ';' });
                    string[] dato2 = col[3].InnerText.Split(new char[] { ';' });

                    int año1 = Convert.ToInt32(dato1[1].Replace("&nbsp", "").Replace("\n", "").Replace("O", ""));
                    string formato1 = dato1[2].Replace("\n", "");

                    int año2 = Convert.ToInt32(dato2[1].Replace("&nbsp", "").Replace("\n", "").Replace("O", ""));
                    string formato2 = dato2[2].Replace("\n", "");

                    //get currency
                    //ProveedoresOnLine.Company.Models.Util.CatalogModel oCurrencyYear1 = Util.ProviderOptions_GetByName(108, formato1);-

                    //get currency
                    //ProveedoresOnLine.Company.Models.Util.CatalogModel oCurrencyYear2 = Util.ProviderOptions_GetByName(108, formato2);

                    //get accounts
                    List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oFinantialAccounts = ProveedoresOnLine.Company.Controller.Company.CategoryGetFinantialAccounts();

                    oBalance.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumFinancialInfoType.SH_Year,
                        },
                        Value = string.Empty,
                        Enable = true,
                    });

                    oBalance.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumFinancialInfoType.SH_Currency,
                        },
                        Value = "",
                        Enable = true,
                    });
                    
                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                    {
                        BalanceSheetId = 0,
                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = 0,
                        },
                        Value = 0,
                        Enable = true,
                    });


                    for (int i = 2; i < rowsTable1.Count; i++)
                    {
                        string acc = oFinantialAccounts.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId == 109002).Select(x => x.Value).FirstOrDefault();

                        Console.WriteLine(acc);

                        HtmlNodeCollection cols = rowsTable1[i].SelectNodes(".//td");

                        Console.WriteLine("cuentas --> " );
                    }

                    #endregion

                    //for (int i = 0; i < rowsTable1.Count; i++)
                    //{
                    //    HtmlNodeCollection cols = rowsTable1[i].SelectNodes(".//td");

                    //    if (i == 0)
                    //    {
                    //        string[] dato1 = cols[1].InnerText.Split(new char[] { ';' });
                    //        string[] dato2 = cols[3].InnerText.Split(new char[] { ';' });

                    //        int año1 = Convert.ToInt32(dato1[1].Replace("&nbsp", "").Replace("\n", "").Replace("O", ""));
                    //        string formato1 = dato1[2].Replace("\n", "");
                            
                    //        int año2 = Convert.ToInt32(dato2[1].Replace("&nbsp", "").Replace("\n", "").Replace("O", ""));
                    //        string formato2 = dato2[2].Replace("\n", "");

                    //        //get currency
                    //        ProveedoresOnLine.Company.Models.Util.CatalogModel oCurrency = Util.ProviderOptions_GetByName(108, formato1);

                    //        if (año1 < año2)
                    //        {
                                
                    //        }

                    //        Console.WriteLine("Año --> " + cols[1].InnerText);
                    //        Console.WriteLine("Año --> " + cols[3].InnerText);
                    //    }
                    //}
                }
                else
                {
                    Console.WriteLine("\nActivo no tiene datos disponibles.\n");
                }

                #endregion

                HtmlNodeCollection rowsTable2 = tables[1].SelectNodes(".//tr");//Pasivo

                #region Pasivo

                if (rowsTable2 != null)
                {
                    Console.WriteLine("\nPasivo info\n");
                }
                else
                {
                    Console.WriteLine("\nPasivo no tiene datos disponibles.\n");
                }

                #endregion

                HtmlNodeCollection rowsTable3 = tables[2].SelectNodes(".//tr");//Patrimonio

                #region Patrimonio

                if (rowsTable3 != null)
                {
                    Console.WriteLine("\nPatrimonio info.\n");
                }
                else
                {
                    Console.WriteLine("\nPatrimonio no tiene datos disponibles.\n");
                }

                #endregion

                HtmlNodeCollection rowsTable4 = tables[3].SelectNodes(".//tr");//Estado de resultados

                #region Estado de resultados

                if (rowsTable4 != null)
                {
                    Console.WriteLine("\nEstado de resultados info.\n");
                }
                else
                {
                    Console.WriteLine("\nEstado de resultados no tiene datos disponibles.\n");
                }

                #endregion
            }

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
