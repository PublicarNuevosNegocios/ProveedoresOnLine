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

        public static void GetBalancesInfo(string ParId, string PublicId)
        {
            HtmlDocument HtmlDoc = WebCrawler.Manager.WebCrawlerManager.GetHtmlDocumnet(ParId, enumMenu.GeneralBalance.ToString());

            if (HtmlDoc.DocumentNode.SelectNodes("//table[@class='tablaborde_01']") != null)
            {

                ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel oBalance = null;

                HtmlNodeCollection tables = HtmlDoc.DocumentNode.SelectNodes("//table[@class='tablaborde_01']");

                HtmlNodeCollection rowsTable1 = tables[0].SelectNodes(".//tr");//Activo
                HtmlNodeCollection rowsTable2 = tables[1].SelectNodes(".//tr");//Pasivo
                HtmlNodeCollection rowsTable3 = tables[2].SelectNodes(".//tr");//Patrimonio
                HtmlNodeCollection rowsTable4 = tables[3].SelectNodes(".//tr");//Estado de Resultados

                Dictionary<int, string> oList = new Dictionary<int, string>();

                if (rowsTable1 != null)
                {
                    Console.WriteLine("\nAssets info\n");

                    HtmlNodeCollection col = rowsTable1[0].SelectNodes(".//td");

                    string[] dato1 = col[1].InnerText.Split(new char[] { ';' });
                    string[] dato2 = col[3].InnerText.Split(new char[] { ';' });

                    if (Convert.ToInt32(dato1[1].Replace("&nbsp", "").Replace("\n", "").Replace("O", "")) > Convert.ToInt32(dato2[1].Replace("&nbsp", "").Replace("\n", "").Replace("O", "")))
                    {
                        oList.Add(Convert.ToInt32(dato2[1].Replace("&nbsp", "").Replace("\n", "").Replace("O", "")), dato2[2].Replace("\n", "") + ",3");
                        oList.Add(Convert.ToInt32(dato1[1].Replace("&nbsp", "").Replace("\n", "").Replace("O", "")), dato1[2].Replace("\n", "") + ",1");
                    }
                    else
                    {
                        oList.Add(Convert.ToInt32(dato1[1].Replace("&nbsp", "").Replace("\n", "").Replace("O", "")), dato1[2].Replace("\n", "") + ",1");
                        oList.Add(Convert.ToInt32(dato2[1].Replace("&nbsp", "").Replace("\n", "").Replace("O", "")), dato2[2].Replace("\n", "") + ",3");
                    }

                    foreach (int year in oList.Keys)
                    {
                        string values = oList.Where(x => x.Key == year).Select(x => x.Value).FirstOrDefault();

                        string[] oValues = values.Split(new char[] { ',' });

                        string currency = oValues[0].Replace(" ", "").Replace("Pesos", "");
                        int i = Convert.ToInt32(oValues[1]);

                        //Get currency info
                        ProveedoresOnLine.Company.Models.Util.CatalogModel oCurrency = Util.ProviderOptions_GetByName(108, currency);

                        oBalance = new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel()
                        {
                            ItemId = 0,
                            ItemName = "Balance del año " + year.ToString(),
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialType.BalanceSheetInfoType,
                            },
                            Enable = true,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                            BalanceSheetInfo = new List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel>(),
                        };

                        oBalance.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.SH_Year,
                            },
                            Value = year.ToString(),
                            Enable = true,
                        });

                        oBalance.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumFinancialInfoType.SH_Currency,
                            },
                            Value = oCurrency != null ? oCurrency.ItemId.ToString() : string.Empty,
                            Enable = true,
                        });

                        #region Activo

                        for (int j = 2; j < rowsTable1.Count; j++)
                        {
                            HtmlNodeCollection cols = rowsTable1[j].SelectNodes(".//td");

                            string value = string.Empty;
                            value = cols[i].InnerText;

                            string text = string.Empty;
                            text = cols[0].InnerText;

                            Regex reg = new Regex("[^a-zA-Z0-9 ]");

                            value = value.Normalize(NormalizationForm.FormD);
                            value = reg.Replace(value.ToLower().Replace(" ", "").Replace("\n", ""), "");

                            text = text.Normalize(NormalizationForm.FormD);
                            text = reg.Replace(text.Replace(" ", "").Replace("\n", ""), "");

                            if (text == "DisponibleenCajayBancos")
                            {
                                oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                {
                                    RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = 3122, //Disponible en Caja y Bancos
                                    },
                                    Value = Convert.ToDecimal(value),
                                });
                            }

                            if (text == "Inversiones")
                            {
                                oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                {
                                    RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = 3123, //Inversiones
                                    },
                                    Value = Convert.ToDecimal(value),
                                });
                            }

                            if (text == "CuentasporCobraryOtrosNetoaCortoPlazo")
                            {
                                oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                {
                                    RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = 3124, //Cuentas por Cobrar y Otros - Neto a Corto Plazo
                                    },
                                    Value = Convert.ToDecimal(value),
                                });
                            }

                            if (text == "Inventarios")
                            {
                                oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                {
                                    RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = 3125, //Inventarios
                                    },
                                    Value = Convert.ToDecimal(value),
                                });
                            }

                            if (text == "InversionesaLargoplazo")
                            {
                                oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                {
                                    RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = 3126, //Inversiones a Largo plazo
                                    },
                                    Value = Convert.ToDecimal(value),
                                });
                            }

                            if (text == "DeudoresaLargoplazo")
                            {
                                oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                {
                                    RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = 3127, //Deudores a Largo plazo
                                    },
                                    Value = Convert.ToDecimal(value),
                                });
                            }

                            if (text == "PROPIEDADPLANTAYEQUIPO")
                            {
                                oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                {
                                    RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = 3128, //Propiedad planta y equipo
                                    },
                                    Value = Convert.ToDecimal(value),
                                });
                            }

                            if (text == "MenosDepreciacioacutenAcumulada")
                            {
                                oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                {
                                    RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                   {
                                       ItemId = 3129, //Depreciación acumulada
                                   },
                                    Value = Convert.ToDecimal("139176647"),
                                });
                            }

                            if (text == "Intangibles")
                            {
                                oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                {
                                    RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = 3130, //Intangibles
                                    },
                                    Value = Convert.ToDecimal(value),
                                });
                            }

                            if (text == "OtrosActivos")
                            {
                                oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                {
                                    RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = 3131, //Otros Activos
                                    },
                                    Value = Convert.ToDecimal(value),
                                });
                            }
                        }

                        #endregion

                        #region Pasivo

                        if (rowsTable2 != null)
                        {
                            for (int j = 2; j < rowsTable2.Count; j++)
                            {
                                HtmlNodeCollection cols = rowsTable2[j].SelectNodes(".//td");

                                string value = string.Empty;
                                value = cols[i].InnerText;

                                string text = string.Empty;
                                text = cols[0].InnerText;

                                Regex reg = new Regex("[^a-zA-Z0-9 ]");

                                value = value.Normalize(NormalizationForm.FormD);
                                value = reg.Replace(value.ToLower().Replace(" ", "").Replace("\n", ""), "");

                                text = text.Normalize(NormalizationForm.FormD);
                                text = reg.Replace(text.Replace(" ", "").Replace("\n", ""), "");

                                if (text == "ObligacionesFinancierasCortoplazo")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3132, //Obligaciones financieras - corto plazo
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "Impuestos")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3133, //Impuestos
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "Proveedores")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3134, //Proveedores
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "CuentasporPagaryOtrosCortoPlazo")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3135, //Cuentas por pagar y otros - corto plazo
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "ObligacionesLaboralesCortoplazo")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3136, //Obligaciones laborales - corto plazo
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "ObligacionesFinancierasLargoplazo")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3137, //Obligaciones financieras - largo plazo
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "CuentasporPagarLargoplazo")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3138, //Cuentas por pagar - largo plazo
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "ObligacionesLaboralesLargoplazo")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3139, //Obligaciones laborales - largo plazo
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "PasivosEstimadosyProvisiones")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3140, //Pasivos estimados y provisiones
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nPasivo no tiene datos disponibles\n");
                        }

                        #endregion

                        #region Patrimonio

                        if (rowsTable3 != null)
                        {
                            for (int j = 2; j < rowsTable3.Count; j++)
                            {
                                HtmlNodeCollection cols = rowsTable3[j].SelectNodes(".//td");

                                string value = string.Empty;
                                value = cols[i].InnerText;

                                string text = string.Empty;
                                text = cols[0].InnerText;

                                Regex reg = new Regex("[^a-zA-Z0-9 ]");

                                value = value.Normalize(NormalizationForm.FormD);
                                value = reg.Replace(value.ToLower().Replace(" ", "").Replace("\n", ""), "");

                                text = text.Normalize(NormalizationForm.FormD);
                                text = reg.Replace(text.Replace(" ", "").Replace("\n", ""), "");

                                if (text == "CapitalSocial")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3141, //Capital social
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "ReservasValorizaciones")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3142, //Reservas, valorizaciones
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "UtilidadesRetenidas")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3143, //Utilidades retenidas
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nPatrimonio no tiene datos disponibles\n");
                        }

                        #endregion

                        #region Estado de Resultados

                        if (rowsTable4 != null)
                        {
                            for (int j = 1; j < rowsTable4.Count; j++)
                            {
                                HtmlNodeCollection cols = rowsTable4[j].SelectNodes(".//td");

                                string value = string.Empty;
                                value = cols[i].InnerText;

                                string text = string.Empty;
                                text = cols[0].InnerText;

                                Regex reg = new Regex("[^a-zA-Z0-9 ]");

                                value = value.Normalize(NormalizationForm.FormD);
                                value = reg.Replace(value.ToLower().Replace(" ", "").Replace("\n", ""), "");

                                text = text.Normalize(NormalizationForm.FormD);
                                text = reg.Replace(text.Replace(" ", "").Replace("\n", ""), "");

                                if (text == "INGRESOSOPERACIONALES")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3821, //Ingresos Operacionales
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "COSTODEVENTAS")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3822, //Costos de Ventas
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "GASTOSDEADMINISTRACIoacuteN")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3825, //Gastos de Administración
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "GASTOSDEVENTAS")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3826, //Gastos de Ventas
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "DEPRECIACIOacuteNYAMORTIZACIOacuteN")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3827, //Depreciación y Amortización
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "INGRESOSNOOPERACIONALES")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3833, //Ingresos no Operacionales
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "INTERESES")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3834, //Intereses
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "OTROSGASTOSNOOPERACIONALES")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3835, //Otros Gastos no Operacionales
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "IMPUESTOS")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3844, //Impuestos
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }

                                if (text == "RESERVALEGAL")
                                {
                                    oBalance.BalanceSheetInfo.Add(new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                    {
                                        RelatedAccount = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = 3854, //Reserva Legal
                                        },
                                        Value = Convert.ToDecimal(value),
                                    });
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nEstado de Resultados no tiene datos disponibles\n");
                        }

                        #endregion

                        if (oBalance.ItemInfo != null && oBalance.BalanceSheetInfo.Count > 0 && oBalance.ItemInfo.Count > 0)
                        {
                            ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel ProviderBalanceToUpsert = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                            {
                                RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                                {
                                    CompanyPublicId = PublicId,
                                },
                                RelatedBalanceSheet = new List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel>(),
                            };

                            ProviderBalanceToUpsert.RelatedBalanceSheet.Add(oBalance);

                            try
                            {
                                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetUpsert(ProviderBalanceToUpsert);
                            }
                            catch (System.Exception e)
                            {
                                Console.WriteLine("\nNo se pudo cargar el balance del año " + year +". Error , " + e.Message + "\n");
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("\nBalances no tiene datos disponibles.\n");
            }
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
