﻿using BackOffice.Models.General;
using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyCustomer.Models.Customer;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class ProviderController : BaseController
    {
        public virtual ActionResult Index()
        {
            string oSearchParam = string.IsNullOrEmpty(Request["SearchParam"]) ? null : Request["SearchParam"];
            string oSearchFilter = string.Join(",", (Request["SearchFilter"] ?? string.Empty).Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            oSearchFilter = string.IsNullOrEmpty(oSearchFilter) ? null : oSearchFilter;

            string oCompanyType =
                    ((int)(BackOffice.Models.General.enumCompanyType.Provider)).ToString() + "," +
                    ((int)(BackOffice.Models.General.enumCompanyType.BuyerProvider)).ToString();

            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                SearchFilter = ProveedoresOnLine.Company.Controller.Company.CompanySearchFilter(oCompanyType, oSearchParam, oSearchFilter),
            };

            if (oModel.SearchFilter == null)
                oModel.SearchFilter = new List<GenericFilterModel>();

            return View(oModel);
        }

        #region General Info

        public virtual ActionResult GIProviderUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            //eval upsert action
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                //get provider request info
                ProveedoresOnLine.Company.Models.Company.CompanyModel CompanyToUpsert = GetProviderRequest();

                //upsert provider
                CompanyToUpsert = ProveedoresOnLine.Company.Controller.Company.CompanyUpsert(CompanyToUpsert);

                //Create Provider By Customer Publicar
                CustomerModel oCustomerModel = new CustomerModel();
                oCustomerModel.RelatedProvider = new List<CustomerProviderModel>();

                oCustomerModel.RelatedProvider.Add(new CustomerProviderModel()
                {
                    RelatedProvider = new CompanyModel()
                    {
                        CompanyPublicId = CompanyToUpsert.CompanyPublicId,
                    },
                    Status = new CatalogModel()
                    {
                        ItemId = Convert.ToInt32(BackOffice.Models.General.enumProviderCustomerStatus.Creation),
                    },
                    Enable = true,
                });

                oCustomerModel.RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_PublicarPublicId].Value);

                ProveedoresOnLine.CompanyCustomer.Controller.Customer.CustomerProviderUpsert(oCustomerModel);

                //eval company partial index
                List<int> InfoTypeModified = new List<int>() { 2 };
                InfoTypeModified.AddRange(CompanyToUpsert.CompanyInfo.Select(x => x.ItemInfoType.ItemId));
                ProveedoresOnLine.Company.Controller.Company.CompanyPartialIndex(CompanyToUpsert.CompanyPublicId, InfoTypeModified);

                //eval redirect url
                if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "next" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.NextMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.NextMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.NextMenu.Url);
                }
                else if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "last" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.LastMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.LastMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.LastMenu.Url);
                }
                else
                {
                    return RedirectToAction(MVC.Provider.ActionNames.GIProviderUpsert, MVC.Provider.Name, new { ProviderPublicId = CompanyToUpsert.CompanyPublicId });
                }
            }

            return View(oModel);
        }

        public virtual ActionResult GICompanyContactUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult GIPersonContactUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult GIBranchUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult GIDistributorUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        #region Private methods

        private ProveedoresOnLine.Company.Models.Company.CompanyModel GetProviderRequest()
        {
            //get company
            ProveedoresOnLine.Company.Models.Company.CompanyModel oReturn = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
            {
                CompanyPublicId = Request["ProviderPublicId"],
                IdentificationType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = Convert.ToInt32(Request["IdentificationType"]),
                },
                IdentificationNumber = Request["IdentificationNumber"],
                CompanyName = Request["CompanyName"],
                CompanyType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = Convert.ToInt32(Request["CompanyType"]),
                },
                Enable = !string.IsNullOrEmpty(Request["Enable"]),
                CompanyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            };

            //get company info
            Request.Form.AllKeys.Where(x => x.Contains("CompanyInfoType_")).All(req =>
            {
                string[] strSplit = req.Split('_');

                if (strSplit.Length >= 3)
                {
                    oReturn.CompanyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = !string.IsNullOrEmpty(strSplit[2]) ? Convert.ToInt32(strSplit[2].Trim()) : 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = Convert.ToInt32(strSplit[1].Trim())
                        },
                        Value = Request[req],
                        Enable = true,
                    });
                }
                return true;
            });
            return oReturn;
        }

        #endregion

        #endregion

        #region Commercial Info

        public virtual ActionResult CIExperiencesUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        #endregion

        #region HSEQ Info

        public virtual ActionResult HICertificationsUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertficationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.Certifications, true),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
            //return View(new Models.Provider.ProviderViewModel());
        }

        public virtual ActionResult HIHealtyPoliticUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertficationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.CompanyHealtyPolitic, true),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
            //return View(new Models.Provider.ProviderViewModel());
        }

        public virtual ActionResult HIRiskPoliciesUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertficationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.CompanyRiskPolicies, true),
                };
            }

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            //eval request
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                ProviderModel ProviderToUpsert = new ProviderModel();
                ProviderToUpsert.RelatedCertification = new List<GenericItemModel>();
                ProviderToUpsert.RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel();

                ProviderToUpsert.RelatedCompany.CompanyPublicId = ProviderPublicId;

                //get request info
                ProviderToUpsert.RelatedCertification.Add(this.GetHIRiskPoliciesInfoRequest());

                //upsert provider info
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertificationUpsert(ProviderToUpsert);

                //eval company partial index
                List<int> InfoTypeModified = new List<int>() { 2 };

                ProviderToUpsert.RelatedCertification.All(x =>
                {
                    InfoTypeModified.AddRange(x.ItemInfo.Select(y => y.ItemInfoType.ItemId));
                    return true;
                });

                ProveedoresOnLine.Company.Controller.Company.CompanyPartialIndex(ProviderToUpsert.RelatedCompany.CompanyPublicId, InfoTypeModified);

                oModel = new Models.Provider.ProviderViewModel()
                {
                    ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                    RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                    {
                        RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                        RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertficationGetBasicInfo(ProviderPublicId, (int)BackOffice.Models.General.enumHSEQType.CompanyRiskPolicies, true),
                    },
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);

                //eval redirect url
                if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "next" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.NextMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.NextMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.NextMenu.Url);
                }
                else if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "last" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.LastMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.LastMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.LastMenu.Url);
                }
            }

            return View(oModel);
        }

        #endregion

        #region Financial Info

        public virtual ActionResult FIBalanceSheetUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            //eval upsert action
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                //get balance sheet request info
                ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel ProviderToUpsert = GetBalanceSheetRequest();

                //upsert sheet request
                ProviderToUpsert = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetUpsert(ProviderToUpsert);

                //eval company partial index
                List<int> InfoTypeModified = new List<int>() { 5 };

                ProviderToUpsert.RelatedBalanceSheet.All(x =>
                {
                    InfoTypeModified.AddRange(x.ItemInfo.Select(y => y.ItemInfoType.ItemId));
                    return true;
                });

                ProveedoresOnLine.Company.Controller.Company.CompanyPartialIndex(ProviderToUpsert.RelatedCompany.CompanyPublicId, InfoTypeModified);

                //eval redirect url
                if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "next" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.NextMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.NextMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.NextMenu.Url);
                }
                else if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "last" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.LastMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.LastMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.LastMenu.Url);
                }
                else
                {
                    return RedirectToAction(MVC.Provider.ActionNames.FIBalanceSheetUpsert, MVC.Provider.Name, new { ProviderPublicId = oModel.RelatedProvider.RelatedCompany.CompanyPublicId });
                }
            }

            return View(oModel);
        }

        public virtual ActionResult FITaxUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult FIIncomeStatementUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult FIBankUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        #region Private methods

        private ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel GetBalanceSheetRequest()
        {
            //List<Tuple<decimal, decimal>> lstCurrencyConversion = null;

            //get provider
            ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oReturn = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
            {
                RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = Request["ProviderPublicId"],
                },

                RelatedBalanceSheet = new List<BalanceSheetModel>()
                { 
                    new BalanceSheetModel()
                    {
                        ItemId = string.IsNullOrEmpty(Request["FinancialId"]) ? 0 : Convert.ToInt32(Request["FinancialId"]),
                        ItemName = Request["FinancialName"],
                        ItemType = new CatalogModel()
                        {
                            ItemId = (int)enumFinancialType.BalanceSheetInfoType,
                        },
                        Enable = !string.IsNullOrEmpty(Request["Enable"]),
                        ItemInfo = new List<GenericItemInfoModel>()
                        {
                            new GenericItemInfoModel()
                            {
                                ItemInfoId = string.IsNullOrEmpty(Request["SH_YearId"]) ? 0 : Convert.ToInt32(Request["SH_YearId"]),
                                ItemInfoType = new CatalogModel()
                                {
                                    ItemId = (int)enumFinancialInfoType.SH_Year,
                                },
                                Value = Request["SH_Year"],
                                Enable = true,
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId = string.IsNullOrEmpty(Request["SH_BalanceSheetFileId"]) ? 0 : Convert.ToInt32(Request["SH_BalanceSheetFileId"]),
                                ItemInfoType = new CatalogModel()
                                {
                                    ItemId = (int)enumFinancialInfoType.SH_BalanceSheetFile,
                                },
                                Value = Request["SH_BalanceSheetFile"],
                                Enable = true,
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId = string.IsNullOrEmpty(Request["SH_CurrencyId"]) ? 0 : Convert.ToInt32(Request["SH_CurrencyId"]),
                                ItemInfoType = new CatalogModel()
                                {
                                    ItemId = (int)enumFinancialInfoType.SH_Currency,
                                },
                                Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value.Replace(" ", ""),
                                Enable = true,
                            },
                        },
                        BalanceSheetInfo = new List<BalanceSheetDetailModel>(),
                    },
                }
            };


            StringBuilder strAux = new StringBuilder();

            Request.Form.AllKeys.All(rq =>
            {
                if (rq.StartsWith("AccountPostName_"))
                {
                    oReturn.RelatedBalanceSheet.FirstOrDefault().BalanceSheetInfo.Add
                        (new BalanceSheetDetailModel()
                        {
                            RelatedAccount = new GenericItemModel()
                            {
                                ItemId = Convert.ToInt32(rq.Split('_')[1].Trim())
                            },
                            Value = Convert.ToDecimal(Request[rq]),
                        });

                    strAux.AppendLine("                            new Models.Provider.BalanceSheetDetailModel()");
                    strAux.AppendLine("                            {");
                    strAux.AppendLine("                                RelatedAccount = new Company.Models.Util.GenericItemModel()");
                    strAux.AppendLine("                                {");
                    strAux.AppendLine("                                    ItemId = " + rq.Split('_')[1].Trim() + ",");
                    strAux.AppendLine("                                },");
                    strAux.AppendLine("                                Value = Convert.ToDecimal(\"" + Request[rq] + "\"),");
                    strAux.AppendLine("                            },");

                }
                return true;
            });

            string x = strAux.ToString();

            #region Metodo 1

            ////get account info
            //List<GenericItemModel> olstAccount =
            //    ProveedoresOnLine.Company.Controller.Company.CategoryGetFinantialAccounts();

            ////get current values
            //List<BalanceSheetDetailModel> olstBalanceSheetDetail = null;

            //if (oReturn.RelatedBalanceSheet.FirstOrDefault().ItemId > 0)
            //{
            //    olstBalanceSheetDetail = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetGetByFinancial
            //        (Convert.ToInt32(oReturn.RelatedBalanceSheet.FirstOrDefault().ItemId));
            //}

            //if (olstBalanceSheetDetail == null)
            //    olstBalanceSheetDetail = new List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel>();


            ////get Currency GetRate 
            //decimal oCurrencyRate = Currency_GetRate
            //    (Convert.ToInt32(Request["SH_Currency"]),
            //    Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value.Replace(" ", "")),
            //    Convert.ToInt32(Request["SH_Year"]));

            ////get account info

            ////key: AccountId
            ////Tuple<Order,AccountObject,BalanceObject,RequestValue,CurrencyValue>
            //Dictionary<int, Tuple<int, GenericItemModel, BalanceSheetDetailModel, decimal, decimal>> AccountValues = new Dictionary<int, Tuple<int, GenericItemModel, BalanceSheetDetailModel, decimal, decimal>>();

            ////fill account object
            //GetAccountArray(
            //    AccountValues,
            //    oCurrencyRate,
            //    null,
            //    olstAccount,
            //    olstBalanceSheetDetail);

            ////execute account formula and refresh related balancesheet
            //AccountValues.Select(aci => aci.Value).OrderBy(aci => aci.Item1).All(aci =>
            //{
            //    int oAccountInfoType = aci.Item2.ItemInfo.
            //        Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.AI_IsValue).
            //        Select(x => Convert.ToInt32(x.Value.Replace(" ", ""))).
            //        DefaultIfEmpty(1).
            //        FirstOrDefault();

            //    //balance only for value types exlude title types (2)
            //    if (oAccountInfoType == 0 || oAccountInfoType == 1)
            //    {
            //        decimal oAccountDecimalValue = 0;

            //        #region Get account value

            //        if (oAccountInfoType == 0)
            //        {
            //            //is formula field
            //            string strFormula = aci.Item2.ItemInfo.
            //                Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.AI_Formula).
            //                Select(x => x.LargeValue).
            //                DefaultIfEmpty(string.Empty).
            //                FirstOrDefault().ToLower().Replace(" ", "");

            //            if (!string.IsNullOrEmpty(strFormula))
            //            {
            //                foreach (var RegexResult in (new Regex("\\[+\\d*\\]+", RegexOptions.IgnoreCase)).Matches(strFormula))
            //                {
            //                    int oAccountId = Convert.ToInt32(RegexResult.ToString().Replace("[", "").Replace("]", ""));

            //                    strFormula = strFormula.Replace(RegexResult.ToString(), AccountValues[oAccountId].Item5.ToString());
            //                }
            //                oAccountDecimalValue = MathEval(strFormula);
            //            }
            //        }
            //        else if (oAccountInfoType == 1)
            //        {
            //            //is value field
            //            oAccountDecimalValue = aci.Item5;
            //        }

            //        #endregion

            //        #region refresh balance object

            //        if (aci.Item3.BalanceSheetId > 0)
            //        {
            //            //update balance value
            //            aci.Item3.Value = oAccountDecimalValue;
            //            aci.Item3.Enable = true;
            //        }
            //        else
            //        {
            //            //new balance
            //            aci.Item3.RelatedAccount = aci.Item2;
            //            aci.Item3.Value = oAccountDecimalValue;
            //            aci.Item3.Enable = true;
            //        }
            //        #endregion
            //    }
            //    else
            //    {
            //        //-1 to exlude balance sheet item in database
            //        aci.Item3.BalanceSheetId = -1;
            //    }
            //    return true;
            //});

            ////execute validations
            //AccountValues.Select(aci => aci.Value).OrderBy(aci => aci.Item1).All(aci =>
            //{
            //    //is formula field
            //    string strValidationFormula = aci.Item2.ItemInfo.
            //        Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.AI_ValidationRule).
            //        Select(x => x.LargeValue).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault().ToLower().Replace(" ", "");

            //    if (!string.IsNullOrEmpty(strValidationFormula))
            //    {
            //        foreach (var RegexResult in (new Regex("[\\[\\d\\]]+", RegexOptions.IgnoreCase)).Matches(strValidationFormula))
            //        {
            //            int oAccountId = Convert.ToInt32(RegexResult.ToString().Replace("[", "").Replace("]", ""));

            //            strValidationFormula = strValidationFormula.Replace(RegexResult.ToString(), AccountValues[oAccountId].Item5.ToString());
            //        }
            //        if (!MathComparison(strValidationFormula))
            //        {
            //            throw new Exception("No se cumple la regla de validación para la cuenta " + aci.Item2.ItemId + "-" + aci.Item2.ItemInfo);
            //        }
            //    }
            //    return true;
            //});

            ////add balancesheet to result

            //oReturn.RelatedBalanceSheet.FirstOrDefault().BalanceSheetInfo = AccountValues.
            //    Select(aci => aci.Value).
            //    Where(aci => aci.Item3.BalanceSheetId > -1).
            //    OrderBy(aci => aci.Item1).
            //    Select(aci => aci.Item3).
            //    ToList();

            #endregion

            return oReturn;
        }

        //recursive hierarchy get account
        private void GetAccountArray
            (Dictionary<int, Tuple<int, GenericItemModel, BalanceSheetDetailModel, decimal, decimal>> AccountValues,
            decimal CurrencyRate,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAccount,
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> lstAccount,
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel> lstBalanceSheetDetail)
        {
            lstAccount.
                Where(ac => RelatedAccount != null ?
                        (ac.ParentItem != null && ac.ParentItem.ItemId == RelatedAccount.ItemId) :
                        (ac.ParentItem == null)).
                OrderBy(ac => ac.ItemInfo.
                    Where(aci => aci.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.AI_Order).
                    Select(aci => Convert.ToInt32(aci.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault()).
                All(ac =>
                {
                    GetAccountArray(
                        AccountValues,
                        CurrencyRate,
                        ac,
                        lstAccount,
                        lstBalanceSheetDetail);

                    AccountValues[ac.ItemId] = new Tuple<int, GenericItemModel, BalanceSheetDetailModel, decimal, decimal>
                        (AccountValues.Count,
                        ac,
                        lstBalanceSheetDetail.Where(bsd => bsd.RelatedAccount.ItemId == ac.ItemId).DefaultIfEmpty(new BalanceSheetDetailModel()).FirstOrDefault(),
                        Convert.ToDecimal(Request["AccountPostName_" + ac.ItemId.ToString()], System.Globalization.CultureInfo.InvariantCulture),
                        Convert.ToDecimal(Request["AccountPostName_" + ac.ItemId.ToString()], System.Globalization.CultureInfo.InvariantCulture) * CurrencyRate);

                    return true;
                });
        }

        #endregion

        #endregion

        #region Legal Info

        public virtual ActionResult LIChaimberOfCommerceUpsert(string ProviderPublicId)
        {
            int oTotalRows;
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.ChaimberOfCommerce, true),

                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            //eval upsert action
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                ProviderModel ProviderToUpsert = new ProviderModel();
                ProviderToUpsert.RelatedLegal = new List<GenericItemModel>();
                ProviderToUpsert.RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel();

                ProviderToUpsert.RelatedCompany.CompanyPublicId = ProviderPublicId;

                //get request Info
                ProviderToUpsert.RelatedLegal.Add(this.GetChaimberOfCommerceInfoRequest());

                if (string.IsNullOrEmpty(ProviderToUpsert.RelatedLegal.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)enumLegalInfoType.CP_InscriptionCity).Select(x => x.Value).FirstOrDefault()))
                {
                    List<GeographyModel> reqCities = new List<GeographyModel>();
                    reqCities = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(Request["CityId"], null, 0, 0, out oTotalRows);
                    ProviderToUpsert.RelatedLegal.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)enumLegalInfoType.CP_InscriptionCity).Select(x => x.Value = reqCities.FirstOrDefault().City.ItemId.ToString()).FirstOrDefault();
                }
                //upsert provider
                ProviderToUpsert = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalUpsert(ProviderToUpsert);

                //eval company partial index
                List<int> InfoTypeModified = new List<int>() { 6 };

                ProviderToUpsert.RelatedLegal.All(x =>
                {
                    InfoTypeModified.AddRange(x.ItemInfo.Select(y => y.ItemInfoType.ItemId));
                    return true;
                });

                ProveedoresOnLine.Company.Controller.Company.CompanyPartialIndex(ProviderToUpsert.RelatedCompany.CompanyPublicId, InfoTypeModified);

                oModel = new Models.Provider.ProviderViewModel()
                {
                    ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                    RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                    {
                        RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                        RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.ChaimberOfCommerce, true),
                    },
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);

                //eval redirect url
                if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "next" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.NextMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.NextMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.NextMenu.Url);
                }
                else if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "last" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.LastMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.LastMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.LastMenu.Url);
                }
            }

            return View(oModel);
        }

        public virtual ActionResult LIRutUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.RUT, true),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }

        public virtual ActionResult LICIFINUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.CIFIN, true),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }

        public virtual ActionResult LISARLAFTUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.SARLAFT, true),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }

        public virtual ActionResult LIResolutionUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.SARLAFT, true),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }

        #region Private methods

        private GenericItemModel GetChaimberOfCommerceInfoRequest()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
               && bool.Parse(Request["UpsertAction"]))
            {
                //get ChaimberInfo                
                GenericItemModel RelatedLegal = new GenericItemModel
                {
                    ItemId = Convert.ToInt32(Request["NameInfoId"]),
                    ItemType = new CatalogModel()
                    {
                        ItemId = Convert.ToInt32(enumLegalType.ChaimberOfCommerce),
                    },
                    ItemName = Request["ChaimberName"],
                    Enable = true,

                    ItemInfo = new List<GenericItemInfoModel>
                        {                               
                            new GenericItemInfoModel()
                            {                                
                                ItemInfoId = int.Parse(Request["ConstitutionDateId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_ConstitutionDate
                                },
                                Value = Request["ConstitutionDate"],
                                Enable = true
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId = int.Parse(Request["UndefinedDateId"]),
                                ItemInfoType = new CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_UndefinedDate,
                                },
                                Value = Request["UndefinedDate"] == null ? "off" : Request["UndefinedDate"],
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =  Convert.ToInt32(Request["ValidityDateId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_ConstitutionEndDate
                                },
                                Value = Request["ValidityDate"],
                                Enable = true
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =  Convert.ToInt32(Request["SelectedCityId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_InscriptionCity
                                },
                                Value = Request["SelectedCity"],
                                Enable = true
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =  Convert.ToInt32(Request["InscriptionNumberId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_InscriptionNumber
                                },
                                Value = Request["InscriptionNumber"],
                                Enable = true
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =  Convert.ToInt32(Request["CertificateURLId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_ExistenceAndLegalPersonCertificate
                                },
                                Value = Request["CertificateURL"],
                                Enable = true
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =  Convert.ToInt32(Request["ExpeditionCertificatedDateId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_CertificateExpeditionDate
                                },
                                Value = Request["ExpeditionCertificatedDate"],
                                Enable = true
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =   Convert.ToInt32(Request["SocialObjectId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_SocialObject
                                },
                                Value = Request["SocialObject"],
                                Enable = true
                            },
                        }
                };
                //Validación del archivo cuando viene desocupado en el formulario
                if (RelatedLegal.ItemInfo != null && RelatedLegal.ItemInfo.Count() > 0)
                {
                    if (string.IsNullOrEmpty(RelatedLegal.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)enumLegalInfoType.CP_ExistenceAndLegalPersonCertificate).Select(x => x.Value).FirstOrDefault()))
                    {
                        RelatedLegal.ItemInfo.Remove(RelatedLegal.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)enumLegalInfoType.CP_ExistenceAndLegalPersonCertificate).Select(x => x).FirstOrDefault());
                    }
                }
                return RelatedLegal;
            }
            return null;
        }

        private GenericItemModel GetHIRiskPoliciesInfoRequest()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
               && bool.Parse(Request["UpsertAction"]))
            {
                //get ARL info
                GenericItemModel RelatedARL = new GenericItemModel
                {
                    ItemId = Convert.ToInt32(Request["NameInfoId"]),
                    ItemType = new CatalogModel()
                    {
                        ItemId = Convert.ToInt32(enumHSEQType.CompanyRiskPolicies),
                    },
                    ItemName = Request["RiskPoliciesName"],
                    Enable = Request["Enable"] == "true" ? true : false,
                    ItemInfo = new List<GenericItemInfoModel>
                    {
                        new GenericItemInfoModel()
                        {                                
                            ItemInfoId = int.Parse(Request["SelectedOccupationalHazardsId"]),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CR_SystemOccupationalHazards
                            },
                            Value = Request["OccupationalHazardsId"],
                            Enable = true,
                        },
                        new GenericItemInfoModel()
                        {
                            ItemInfoId = int.Parse(Request["RateARLId"]),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CR_RateARL
                            },
                            Value = Request["RateARL"],
                            Enable = true,
                        },
                        new GenericItemInfoModel()
                        {
                            ItemInfoId = int.Parse(Request["CertificateAffiliateARLId"]),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumHSEQInfoType.CR_CertificateAffiliateARL
                            },
                            Value = Request["CertificateAffiliateARL"],
                            Enable = true,
                        },
                    },
                };

                //if (!string.IsNullOrEmpty(Request["CertificateAffiliateARL"]) && Request["CertificateAffiliateARL"].Length > 0)
                //{
                //    GenericItemInfoModel oGenericItem = new GenericItemInfoModel()
                //    {
                //        ItemInfoId = int.Parse(Request["CertificateAffiliateARLId"]),
                //        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                //        {
                //            ItemId = (int)enumHSEQInfoType.CR_CertificateAffiliateARL
                //        },
                //        Value = Request["CertificateAffiliateARL"],
                //        Enable = true,
                //    };

                //    RelatedARL.ItemInfo.Add(oGenericItem);
                //}
                //Validación del archivo cuando viene desocupado en el formulario
                if (RelatedARL.ItemInfo != null && RelatedARL.ItemInfo.Count() > 0)
                {
                    if (string.IsNullOrEmpty(RelatedARL.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)enumHSEQInfoType.CR_CertificateAffiliateARL).Select(x => x.Value).FirstOrDefault()))
                    {
                        RelatedARL.ItemInfo.Remove(RelatedARL.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)enumHSEQInfoType.CR_CertificateAffiliateARL).Select(x => x).FirstOrDefault());
                    }
                }

                return RelatedARL;
            }
            return null;
        }

        #endregion

        #endregion

        #region Customer Provider

        public virtual ActionResult CPCustomerProviderStatus(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }

        #endregion

        #region Menu

        private List<BackOffice.Models.General.GenericMenu> GetProviderMenu
            (BackOffice.Models.Provider.ProviderViewModel vProviderInfo)
        {
            List<BackOffice.Models.General.GenericMenu> oReturn = new List<Models.General.GenericMenu>();

            if (vProviderInfo.RelatedProvider != null &&
                vProviderInfo.RelatedProvider.RelatedCompany != null &&
                !string.IsNullOrEmpty(vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId))
            {
                //get current controller action
                string oCurrentController = BackOffice.Web.Controllers.BaseController.CurrentControllerName;
                string oCurrentAction = BackOffice.Web.Controllers.BaseController.CurrentActionName;

                #region General Info

                //header
                BackOffice.Models.General.GenericMenu oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información general",
                    Position = 0,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Basic info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información básica",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company contact info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Contacto principal de la empresa",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GICompanyContactUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GICompanyContactUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Branch info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Sucursales",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIBranchUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIBranchUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });


                //Person contact info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información de personas de contacto",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIPersonContactUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIPersonContactUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Distributor info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Distribuidores",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIDistributorUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 4,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIDistributorUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Commercial Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información comercial",
                    Position = 1,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Experience
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Experiencias",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.CIExperiencesUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.CIExperiencesUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region HSEQ Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "HSEQ",
                    Position = 2,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Certifications
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Certificaciones",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.HICertificationsUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.HICertificationsUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company healty politic
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Salud, medio ambiente y seguridad",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.HIHealtyPoliticUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.HIHealtyPoliticUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company risk policies
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Sistema de riesgos laborales",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.HIRiskPoliciesUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.HIRiskPoliciesUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Financial Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información financiera",
                    Position = 3,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Balancesheet info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Estados financieros",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.FIBalanceSheetUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.FIBalanceSheetUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //tax info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Impuestos",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.FITaxUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.FITaxUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //income statement
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Declaración de renta",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.FIIncomeStatementUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.FIIncomeStatementUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //bank info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información bancaria",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.FIBankUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.FIBankUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Legal Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información Legal",
                    Position = 4,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //chaimber of commerce
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Cámara de comercio",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.LIChaimberOfCommerceUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LIChaimberOfCommerceUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //RUT
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Registro único tributario",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.LIRutUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LIRutUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //CIFIN
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "CIFIN",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.LICIFINUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LICIFINUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //SARLAFT
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "SARLAFT",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.LISARLAFTUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LISARLAFTUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Resolution
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Resoluciones",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.LIResolutionUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 4,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LIResolutionUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Customer Provider

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Compradores relacionados",
                    Position = 5,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Customer provider
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Seguimiento",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.CPCustomerProviderStatus,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.CPCustomerProviderStatus &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region last next menu

                BackOffice.Models.General.GenericMenu MenuAux = null;

                oReturn.OrderBy(x => x.Position).All(pm =>
                {
                    pm.ChildMenu.OrderBy(x => x.Position).All(sm =>
                    {
                        if (MenuAux != null)
                        {
                            MenuAux.NextMenu = sm;
                        }
                        sm.LastMenu = MenuAux;
                        MenuAux = sm;

                        return true;
                    });

                    return true;
                });

                #endregion
            }
            return oReturn;
        }

        #endregion

        public virtual ActionResult DownloadFile(string SearchParam, string SearchFilter)
        {
            string oSearchParam = string.IsNullOrEmpty(Request["SearchParam"]) ? null : Request["SearchParam"];
            string oSearchFilter = string.Join(",", (Request["SearchFilter"] ?? string.Empty).Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            oSearchFilter = string.IsNullOrEmpty(oSearchFilter) ? null : oSearchFilter;

            string oCompanyType =
                    ((int)(BackOffice.Models.General.enumCompanyType.Provider)).ToString() + "," +
                    ((int)(BackOffice.Models.General.enumCompanyType.BuyerProvider)).ToString();

            int oPageNumber = 0;
            int oRowCount = 65000;
            int oTotalRows;

            List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oCompanyList =
                ProveedoresOnLine.Company.Controller.Company.CompanySearch
                    (oCompanyType,
                    null,
                    oSearchFilter,
                    oPageNumber,
                    oRowCount,
                    out oTotalRows);

            //Get the providers
            List<ProviderModel> oProviders = new List<ProviderModel>();

            oCompanyList.All(x =>
            {
                oProviders.Add(new ProviderModel
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(x.CompanyPublicId),
                });
                return true;
            });

            //Set the legal i
            oProviders.All(x =>
            {
                x.RelatedLegal = new List<GenericItemModel>();
                x.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo
                    (x.RelatedCompany.CompanyPublicId, (int)enumLegalType.Designations, true);
                return true;
            });

            //Write the document
            StringBuilder data = new StringBuilder();
            string strSep = ";";

            oProviders.All(x =>
            {
                if (oProviders.IndexOf(x) == 0)
                {
                    data.AppendLine
                    ("\"" + "NOMBRE" + "\"" + strSep +
                        "\"" + "TIPO IDENTIFICACION" + "\"" + strSep +
                        "\"" + "NUMERO IDENTIFICACION" + "\"" + strSep +
                        "\"" + "TIPO" + "\"" + strSep +
                        "\"" + "ID INTERNO" + "\"");
                    data.AppendLine
                        ("\"" + x.RelatedCompany.CompanyName + "\"" + "" + strSep +
                        "\"" + x.RelatedCompany.IdentificationType.ItemName + "\"" + strSep +
                        "\"" + x.RelatedCompany.IdentificationNumber + "\"" + strSep +
                        "\"" + "Company" + "\"" + strSep +
                        "\"" + x.RelatedCompany.CompanyPublicId + "\"");
                    if (x.RelatedLegal != null && x.RelatedLegal.Count > 0)
                    {
                        x.RelatedLegal.All(y =>
                        {
                            data.AppendLine
                                (
                                    "\"" + y.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalInfoType.CD_PartnerName).Select(n => n.Value).FirstOrDefault() + "\"" + "" + strSep +
                                    "\"" + "CC" + "\"" + strSep +
                                    "\"" + y.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalInfoType.CD_PartnerIdentificationNumber).Select(n => n.Value).FirstOrDefault() + "\"" + "" + strSep +
                                    "\"" + "Person" + "\"" + strSep +
                                    "\"" + x.RelatedCompany.CompanyPublicId + "\""
                                );
                            return true;
                        });
                    }
                }
                else
                {
                    data.AppendLine
                        ("\"" + x.RelatedCompany.CompanyName + "\"" + "" + strSep +
                        "\"" + x.RelatedCompany.IdentificationType.ItemName + "\"" + strSep +
                        "\"" + x.RelatedCompany.IdentificationNumber + "\"" + strSep +
                        "\"" + "Company" + "\"" + strSep +
                        "\"" + x.RelatedCompany.CompanyPublicId + "\"");
                    if (x.RelatedLegal != null && x.RelatedLegal.Count > 0)
                    {
                        x.RelatedLegal.All(y =>
                        {
                            data.AppendLine
                                (
                                    "\"" + y.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalInfoType.CD_PartnerName).Select(n => n.Value).FirstOrDefault() + "\"" + "" + strSep +
                                    "\"" + "CC" + "\"" + strSep +
                                    "\"" + y.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalInfoType.CD_PartnerIdentificationNumber).Select(n => n.Value).FirstOrDefault() + "\"" + "" + strSep +
                                    "\"" + "Person" + "\"" + strSep +
                                    "\"" + x.RelatedCompany.CompanyPublicId + "\""
                                );
                            return true;
                        });
                    }
                }
                return true;
            });

            byte[] buffer = Encoding.ASCII.GetBytes(data.ToString().ToCharArray());

            return File(buffer, "application/csv", "Proveedores_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv");
        }
    }
}