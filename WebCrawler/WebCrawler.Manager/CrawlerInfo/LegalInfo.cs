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
    public class LegalInfo
    {
        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> GetLegalInfo(string ParId, string PublicId)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oLegal = null;

            HtmlDocument HtmlDoc = WebCrawler.Manager.WebCrawlerManager.GetHtmlDocumnet(ParId, enumMenu.LegalInfo.ToString());

            if (HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']") != null)
            {
                HtmlNodeCollection table = HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']");

                HtmlNodeCollection rowsTable1 = table[1].SelectNodes(".//tr"); //Socios

                #region Designations

                if (rowsTable1 != null)
                {
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oDesignationsInfo = null;

                    Console.WriteLine("\nIncome Statement\n");

                    for (int i = 1; i < rowsTable1.Count; i++)
                    {
                        HtmlNodeCollection cols = rowsTable1[i].SelectNodes(".//td");

                        oDesignationsInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = 0,
                            ItemName = string.Empty,
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalType.Designations,
                            },
                            Enable = true,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        };

                        oDesignationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.CD_PartnerName,
                            },
                            Value = "",
                            Enable = true
                        });

                        oDesignationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.CD_PartnerIdentificationNumber,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oDesignationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.CD_PartnerRank,
                            },
                            Value = "",
                            Enable = true,
                        });

                        if (oDesignationsInfo != null)
                        {
                            oLegal.Add(oDesignationsInfo);
                        }
                    }

                    if (oDesignationsInfo == null)
                    {
                        Console.WriteLine("\nDesignatiosn no tiene datos disponibles.\n");
                    }
                }

                #endregion

                HtmlNodeCollection rowsTable2 = table[2].SelectNodes(".//tr"); //Información Tributaria - Resoluciones

                #region Resolutions

                if (rowsTable2 != null)
                {
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oRUTInfo = null;

                    Console.WriteLine("\nRUT\n");

                    for (int i = 1; i < rowsTable1.Count; i++)
                    {
                        HtmlNodeCollection cols = rowsTable1[i].SelectNodes(".//td");

                        oRUTInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = 0,
                            ItemName = string.Empty,
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalType.RUT,
                            },
                            Enable = true,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        };

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_PersonType,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_LargeContributor,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_LargeContributorReceipt,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_LargeContributorDate,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_SelfRetainer,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_SelfRetainerReciept,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_SelfRetainerDate,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_EntityType,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_IVA,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_TaxPayerType,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_ICA,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_RUTFile,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_LargeContributorFile,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oRUTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.R_SelfRetainerFile,
                            },
                            Value = "",
                            Enable = true,
                        });

                        if (oRUTInfo != null)
                        {
                            oLegal.Add(oRUTInfo);
                        }
                    }
                    if (oRUTInfo == null)
                    {
                        Console.WriteLine("\nRUT no tiene datos disponibles.\n");
                    }
                }

                #endregion

                HtmlNodeCollection rowsTable3 = table[8].SelectNodes(".//tr"); //SARLAFT

                #region SARLAFT

                if (rowsTable1 != null)
                {
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oSARLAFTInfo = null;

                    Console.WriteLine("\nSARLAFT\n");

                    for (int i = 1; i < rowsTable1.Count; i++)
                    {
                        HtmlNodeCollection cols = rowsTable1[i].SelectNodes(".//td");

                        oSARLAFTInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = 0,
                            ItemName = string.Empty,
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalType.SARLAFT,
                            },
                            Enable = true,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        };

                        oSARLAFTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.SF_ProcessDate,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oSARLAFTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.SF_PersonType,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oSARLAFTInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.SF_SARLAFTFile,
                            },
                            Value = "",
                            Enable = true,
                        });

                        if (oSARLAFTInfo != null)
                        {
                            oLegal.Add(oSARLAFTInfo);
                        }
                    }

                    if (oSARLAFTInfo == null)
                    {
                        Console.WriteLine("\nSARLAFT no tiene datos disponibles.\n");
                    }
                }

                #endregion

                HtmlNodeCollection rowsTable4 = table[4].SelectNodes(".//tr"); //CIFIN

                #region CIFIN

                if (rowsTable1 != null)
                {
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oCIFINInfo = null;

                    Console.WriteLine("\nCIFIN\n");

                    for (int i = 1; i < rowsTable1.Count; i++)
                    {
                        HtmlNodeCollection cols = rowsTable1[i].SelectNodes(".//td");

                        oCIFINInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = 0,
                            ItemName = string.Empty,
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalType.CIFIN,
                            },
                            Enable = true,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        };

                        oCIFINInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.CF_QueryDate,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oCIFINInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.CF_ResultQuery,
                            },
                            Value = "",
                            Enable = true,
                        });

                        oCIFINInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumLegalInfoType.CF_AutorizationFile,
                            },
                            Value = "",
                            Enable = true,
                        });

                        if (oCIFINInfo != null)
                        {
                            oLegal.Add(oCIFINInfo);
                        }
                    }

                    if (oCIFINInfo == null)
                    {
                        Console.WriteLine("\nCIFIN no tiene datos disponibles.\n");
                    }
                }

                #endregion
            }

            return oLegal;
        }
    }
}
