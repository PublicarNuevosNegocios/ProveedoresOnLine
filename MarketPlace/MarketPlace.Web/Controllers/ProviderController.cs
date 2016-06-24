using ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch;
using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using Microsoft.Reporting.WebForms;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.SurveyModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ProveedoresOnLine.CalificationProject.Models.CalificationProject;

namespace MarketPlace.Web.Controllers
{
    public partial class ProviderController : BaseController
    {
        public virtual ActionResult Index()
        {
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            ProviderViewModel oModel = new ProviderViewModel();
            return View(oModel);
        }

        #region Provider search

        public virtual ActionResult Search
            (string ProjectPublicId,
            string CompareId,
            string SearchParam,
            string SearchFilter,
            string SearchOrderType,
            string OrderOrientation,
            string PageNumber)
        {
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            ProviderSearchViewModel oModel = null;

            if (SessionModel.CurrentCompany != null &&
                !string.IsNullOrEmpty(SessionModel.CurrentCompany.CompanyPublicId))
            {
                //get basic search model
                oModel = new ProviderSearchViewModel()
                {
                    ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                    SearchParam = SearchParam,
                    SearchFilter = SearchFilter == null ? null : (SearchFilter.Trim(new char[] { ',' }).Length > 0 ? SearchFilter.Trim(new char[] { ',' }) : null),
                    SearchOrderType = string.IsNullOrEmpty(SearchOrderType) ? MarketPlace.Models.General.enumSearchOrderType.Relevance : (MarketPlace.Models.General.enumSearchOrderType)Convert.ToInt32(SearchOrderType),
                    OrderOrientation = string.IsNullOrEmpty(OrderOrientation) ? false : ((OrderOrientation.Trim().ToLower() == "1") || (OrderOrientation.Trim().ToLower() == "true")),
                    PageNumber = string.IsNullOrEmpty(PageNumber) ? 0 : Convert.ToInt32(PageNumber),
                    ProviderSearchResult = new List<ProviderLiteViewModel>(),
                };

                #region Providers

                //search providers
                int oTotalRowsAux;
                List<ProviderModel> oProviderResult =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchNew
                    (SessionModel.CurrentCompany.CompanyPublicId,
                    SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    (int)oModel.SearchOrderType,
                    oModel.OrderOrientation,
                    oModel.PageNumber,
                    oModel.RowCount,
                    out oTotalRowsAux);

                oModel.TotalRows = oTotalRowsAux;

                List<GenericFilterModel> oFilterModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchFilterNew
                    (SessionModel.CurrentCompany.CompanyPublicId,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false);

                if (oFilterModel != null)
                {
                    oModel.ProviderFilterResult = oFilterModel.Where(x => x.CustomerPublicId == SessionModel.CurrentCompany.CompanyPublicId).ToList();
                }

                //parse view model
                if (oProviderResult != null && oProviderResult.Count > 0)
                {
                    oProviderResult.All(prv =>
                    {
                        oModel.ProviderSearchResult.Add
                            (new ProviderLiteViewModel(prv));

                        return true;
                    });
                }

                #endregion Providers

                if (!string.IsNullOrEmpty(ProjectPublicId))
                {
                    #region Project

                    //get current project
                    ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectResult = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.
                        ProjectGetByIdLite
                        (ProjectPublicId,
                        SessionModel.CurrentCompany.CompanyPublicId);

                    if (oProjectResult != null && !string.IsNullOrEmpty(oProjectResult.ProjectPublicId))
                    {
                        oModel.RelatedProject = new Models.Project.ProjectViewModel(oProjectResult);
                    }

                    #endregion Project
                }
                else
                {
                    #region Compare

                    if (!string.IsNullOrEmpty(CompareId))
                    {
                        //get current compare
                        ProveedoresOnLine.CompareModule.Models.CompareModel oCompareResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                            CompareGetCompanyBasicInfo
                            (Convert.ToInt32(CompareId.Replace(" ", "")),
                            SessionModel.CurrentLoginUser.Email,
                            SessionModel.CurrentCompany.CompanyPublicId);

                        if (oCompareResult != null && oCompareResult.CompareId > 0)
                        {
                            oModel.RelatedCompare = new Models.Compare.CompareViewModel(oCompareResult);
                        }
                    }

                    #endregion Compare

                    #region Project config

                    //get project config items
                    List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> oProjectConfigResult = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.
                        MPProjectConfigGetByCustomer(SessionModel.CurrentCompany.CompanyPublicId);

                    if (oProjectConfigResult != null && oProjectConfigResult.Count > 0)
                    {
                        oModel.RelatedProjectConfig = new List<Models.Project.ProjectConfigViewModel>();

                        oProjectConfigResult.All(pjc =>
                        {
                            oModel.RelatedProjectConfig.Add(new Models.Project.ProjectConfigViewModel(pjc));
                            return true;
                        });
                    }

                    #endregion Project config
                }
            }

            return View(oModel);
        }

        #endregion Provider search

        #region General Info

        public virtual ActionResult GIProviderInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                // GET ALL BASIC INFO
                ProviderModel response = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPGetBasicInfo(ProviderPublicId);

                #region Basic Info

                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany = oProvider.RelatedCompany;

                oModel.ContactCompanyInfo = response.ContactCompanyInfo;
                oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

                if (oModel.ContactCompanyInfo != null)
                {
                    oModel.ContactCompanyInfo.All(x =>
                    {
                        oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                        return true;
                    });
                }

                #endregion Basic Info

                #region Legal Info

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = response.RelatedLegal;
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }

                #endregion Legal Info

                #region Basic Financial Info

                List<GenericItemModel> oFinancial = response.RelatedFinantial;
                oModel.RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                if (oFinancial != null)
                {
                    decimal oExchange;
                    oExchange = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(
                                Convert.ToInt32(oFinancial.FirstOrDefault().ItemInfo.FirstOrDefault().ValueName),
                                Convert.ToInt32(Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_CurrencyExchange_COP].Value),
                                Convert.ToInt32(oFinancial.FirstOrDefault().ItemName));

                    oFinancial.All(x =>
                    {
                        oModel.RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(x, oExchange));
                        return true;
                    });

                    if (oModel.RelatedFinancialBasicInfo != null && oModel.RelatedFinancialBasicInfo.Count > 0)
                    {
                        oModel.RelatedFinancialBasicInfo.FirstOrDefault().BI_JobCapital =
                            ((Convert.ToDecimal(oModel.RelatedFinancialBasicInfo.Where(x => !string.IsNullOrWhiteSpace(x.BI_CurrentActive)).Select(x => x.BI_CurrentActive).DefaultIfEmpty("0").FirstOrDefault())
                            - Convert.ToDecimal(oModel.RelatedFinancialBasicInfo.Where(x => !string.IsNullOrWhiteSpace(x.BI_CurrentPassive)).Select(x => x.BI_CurrentPassive).DefaultIfEmpty("0").FirstOrDefault()))).ToString("#,0.##");
                    }
                }

                #endregion Basic Financial Info

                #region K_Contract Info

                List<GenericItemModel> oRelatedKContractInfo = response.RelatedKContractInfo;
                oModel.RelatedKContractInfo = new List<ProviderFinancialViewModel>();
                if (oRelatedKContractInfo != null)
                {
                    oRelatedKContractInfo.All(x =>
                    {
                        oModel.RelatedKContractInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });
                }

                #endregion K_Contract Info

                #region HSEQ

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<GenericItemModel>();
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = response.RelatedCertification;
                oModel.RelatedHSEQlInfo = new List<ProviderHSEQViewModel>();
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null
                && oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.Count > 0)
                {
                    List<GenericItemModel> basicCert = response.RelatedCertificationBasic;
                    if (basicCert != null && basicCert.Count > 0)
                    {
                        oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.AddRange(basicCert);
                    }

                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.All(x =>
                    {
                        oModel.RelatedHSEQlInfo.Add(new ProviderHSEQViewModel(x));
                        return true;
                    });
                }

                oModel.RelatedCertificationBasicInfo = response.RelatedCertificationBasicInfo;

                #endregion HSEQ

                #region CalificationProject
                List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> oCalProject = new List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel>();
                oCalProject = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.
                                                        CalificationProject_GetByCustomer(SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId, true);

                List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigValidateModel> oValidateModel = new List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigValidateModel>();

                oModel.ProviderCalification = new ProviderCalificationViewModel();

                if (oCalProject != null &&
                    oCalProject.Count > 0)
                {
                    oValidateModel = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectValidate_GetByProjectConfigId(oCalProject.FirstOrDefault().ProjectConfigModel.CalificationProjectConfigId, true);
                    oModel.ProviderCalification.ProRelatedCalificationProject = oCalProject;
                    oModel.ProviderCalification.TotalScore = oCalProject.FirstOrDefault().TotalScore;
                    oModel.ProviderCalification.TotalCalification = GetCalificationScore(oCalProject, oValidateModel);
                }
                else
                {
                    oModel.ProviderCalification.ProRelatedCalificationProject = new List<CalificationProjectBatchModel>();
                    oModel.ProviderCalification.TotalScore = 0;
                    oModel.ProviderCalification.TotalCalification = string.Empty;
                }
                

                #endregion

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            if (Request["DownloadReport"] == "true")
            {
                GenericReportModel Report = RPGerencial(oModel);
                return File(Report.File, Report.MimeType, Report.FileName);
            }

            return View(oModel);
        }

        public virtual ActionResult GIPersonContactInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;
            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.ContactCompanyInfo = new List<GenericItemModel>();
                oModel.ContactCompanyInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(ProviderPublicId, (int)enumContactType.PersonContact);
                oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

                if (oModel.ContactCompanyInfo != null)
                {
                    oModel.ContactCompanyInfo.All(x =>
                    {
                        oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                        return true;
                    });
                }
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult GIBranchInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            int oTotalRows;

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;
            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.ContactCompanyInfo = new List<GenericItemModel>();
                oModel.ContactCompanyInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(ProviderPublicId, (int)enumContactType.Brach);
                oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oCities = null;

                oCities = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, null, 0, 0, out oTotalRows);

                if (oModel.ContactCompanyInfo != null)
                {
                    oModel.ContactCompanyInfo.All(x =>
                    {
                        oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
                oModel.RelatedGeneralInfo = oModel.RelatedGeneralInfo.OrderByDescending(x => x.BR_IsPrincipal).ToList();
            }
            return View(oModel);
        }

        public virtual ActionResult GIDistributorInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                   Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                               (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                               x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.ContactCompanyInfo = new List<GenericItemModel>();

                oModel.ContactCompanyInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(ProviderPublicId, (int)enumContactType.Distributor);
                oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

                if (oModel.ContactCompanyInfo != null)
                {
                    oModel.ContactCompanyInfo.All(x =>
                    {
                        oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                        return true;
                    });
                }
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult GITrackingInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;
            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                  Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                              (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                              x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                ////get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedTrackingInfo = new List<GenericItemModel>();

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult GIBlackList(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;
            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);
            var oProvider = olstProvider.
                 Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                             (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                             x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                ////get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedTrackingInfo = new List<GenericItemModel>();
                oModel.RelatedBlackListInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListGetBasicInfo(ProviderPublicId);

                List<GenericItemModel> oDesignations = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.Designations);
                if (oDesignations != null && oDesignations.Count > 0)
                {
                    oModel.RelatedDesignationsInfo = new List<ProviderDesignationsViewModel>();
                    oDesignations.All(x =>
                    {
                        oModel.RelatedDesignationsInfo.Add(new ProviderDesignationsViewModel(x));
                        return true;
                    });
                }
                List<BlackListModel> oListNoCoincidencesList = new List<BlackListModel>();
                if (oModel.RelatedBlackListInfo == null)
                {
                    oModel.RelatedBlackListInfo = new List<BlackListModel>();

                    BlackListModel oListNoCoincidences = new BlackListModel()
                    {
                        CompanyPublicId = ProviderPublicId,
                        BlackListInfo = new List<GenericItemInfoModel>(),
                        BlackListStatus = new CatalogModel() { ItemId = (int)enumBlackListStatus.DontShowAlert }
                    };
                    oListNoCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoType = new CatalogModel()
                        {
                            ItemName = "Nombre del Grupo",
                        },
                        Value = "SIN COINCIDENCIAS",
                    });
                    oListNoCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoType = new CatalogModel()
                        {
                            ItemName = "Nombre Consultado",
                        },
                        Value = oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName,
                    });
                    oListNoCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoType = new CatalogModel()
                        {
                            ItemName = "Identificación Consultada",
                        },
                        Value = oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber,
                    });

                    if (oModel.RelatedDesignationsInfo != null && oModel.RelatedDesignationsInfo.Count > 0)
                    {
                        oModel.RelatedDesignationsInfo.All(x =>
                            {
                                BlackListModel oListCoincidences = new BlackListModel()
                                {
                                    CompanyPublicId = ProviderPublicId,
                                    BlackListInfo = new List<GenericItemInfoModel>(),
                                    BlackListStatus = new CatalogModel() { ItemId = (int)enumBlackListStatus.DontShowAlert }
                                };
                                oListCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoType = new CatalogModel()
                                    {
                                        ItemName = "Nombre del Grupo",
                                    },
                                    Value = "SIN COINCIDENCIAS",
                                });
                                oListCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoType = new CatalogModel()
                                    {
                                        ItemName = "Nombre Consultado",
                                    },
                                    Value = x.CD_PartnerName,
                                });
                                oListCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoType = new CatalogModel()
                                    {
                                        ItemName = "Identificación Consultada",
                                    },
                                    Value = x.CD_PartnerIdentificationNumber,
                                });
                                oListCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoType = new CatalogModel()
                                    {
                                        ItemName = "Cargo",
                                    },
                                    Value = x.CD_PartnerRank,
                                });
                                oModel.RelatedBlackListInfo.Add(oListCoincidences);
                                return true;
                            });
                    }
                    oModel.RelatedBlackListInfo.Add(oListNoCoincidences);
                }
                else
                {
                    if (oModel.RelatedDesignationsInfo != null && oModel.RelatedDesignationsInfo.Count > 0)
                    {

                        List<ProviderDesignationsViewModel> oCoincidences = new List<ProviderDesignationsViewModel>();
                        oModel.RelatedBlackListInfo.All(y =>
                            {
                                oCoincidences.Add(new ProviderDesignationsViewModel()
                                {
                                    oCD_PartnerIdentificationNumber = y.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Identificación Consultada").Select(x => x.Value).FirstOrDefault(),
                                    oCD_PartnerName = y.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre Consultado").Select(x => x.Value).FirstOrDefault(),
                                    oCD_PartnerRank = y.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Cargo").Select(x => x.Value).FirstOrDefault(),
                                });
                                return true;
                            });
                        //oCoincidences                                               

                        oModel.RelatedDesignationsInfo.All(x =>
                            {
                                if (!oCoincidences.Any(p => p.oCD_PartnerIdentificationNumber == x.CD_PartnerIdentificationNumber))
                                {
                                    BlackListModel oListCoincidences = new BlackListModel()
                                    {
                                        CompanyPublicId = ProviderPublicId,
                                        BlackListInfo = new List<GenericItemInfoModel>(),
                                        BlackListStatus = new CatalogModel() { ItemId = (int)enumBlackListStatus.DontShowAlert }
                                    };
                                    oListCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                                    {
                                        ItemInfoType = new CatalogModel()
                                        {
                                            ItemName = "Nombre del Grupo",
                                        },
                                        Value = "SIN COINCIDENCIAS",
                                    });
                                    oListCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                                    {
                                        ItemInfoType = new CatalogModel()
                                        {
                                            ItemName = "Nombre Consultado",
                                        },
                                        Value = x.CD_PartnerName,
                                    });
                                    oListCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                                    {
                                        ItemInfoType = new CatalogModel()
                                        {
                                            ItemName = "Identificación Consultada",
                                        },
                                        Value = x.CD_PartnerIdentificationNumber,
                                    });
                                    oListCoincidences.BlackListInfo.Add(new GenericItemInfoModel()
                                    {
                                        ItemInfoType = new CatalogModel()
                                        {
                                            ItemName = "Cargo",
                                        },
                                        Value = x.CD_PartnerRank,
                                    });
                                    oModel.RelatedBlackListInfo.Add(oListCoincidences);
                                }
                                return true;
                            });
                    }
                }


                //oProvidersToCompare = Process.RelatedProvider.Where(y => oCoincidences.Any(c => c.IdentificationResult == y.RelatedCompany.IdentificationNumber && y.RelatedCompany.CompanyName == c.NameResult)).ToList();

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            //Get report generator
            if (Request["DownloadReport"] == "true")
            {
                /*generate data for report*/
                List<string> oGroupListName = new List<string>();
                foreach (var alert in oModel.RelatedBlackListInfo)
                {
                    oGroupListName.Add(alert.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre del Grupo").Select(x => x.Value).FirstOrDefault());
                }
                oGroupListName = oGroupListName.GroupBy(x => x).Select(x => x.First()).ToList();

                /*get company info*/
                List<string> oOrderGroup = new List<string>();

                oOrderGroup.Add(oGroupListName.Where(x => x == "LISTAS RESTRICTIVAS - Criticidad Alta").Select(x => x).FirstOrDefault());
                oOrderGroup.Add(oGroupListName.Where(x => x == "DELITOS E INHABILIDADES CONTRA EL ESTADO - Criticidad Media").Select(x => x).FirstOrDefault());
                oOrderGroup.Add(oGroupListName.Where(x => x == "LISTAS FINANCIERAS - Criticidad Media").Select(x => x).FirstOrDefault());
                oOrderGroup.Add(oGroupListName.Where(x => x == "LISTAS PEPS - Criticidad Baja").Select(x => x).FirstOrDefault());
                oOrderGroup.Add(oGroupListName.Where(x => x == "SIN COINCIDENCIAS").Select(x => x).FirstOrDefault());

                string searchName = "";
                string searchIdentification = "--";
                string User = "ProveeodresOnLine";
                string CreateDate = oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == 203012).Select(x => x.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                string IsSuccess = "--";

                List<ReportParameter> parameters = new List<ReportParameter>();
                //Customer Info
                parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName));
                parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber));
                parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName));
                parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo));
                //Query Info
                parameters.Add(new ReportParameter("ThirdKnowledgeText", MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.MP_TK_TextImage].Value));
                parameters.Add(new ReportParameter("User", User));
                parameters.Add(new ReportParameter("CreateDate", !string.IsNullOrEmpty(CreateDate) ? CreateDate : "--"));
                parameters.Add(new ReportParameter("searchName", searchName));
                parameters.Add(new ReportParameter("searchIdentification", searchIdentification));
                parameters.Add(new ReportParameter("IsSuccess", IsSuccess));

                DataTable data_rst = new DataTable();
                DataTable data_dce = new DataTable();
                DataTable data_fnc = new DataTable();
                DataTable data_psp = new DataTable();
                DataTable data_na = new DataTable();

                foreach (var grp in oOrderGroup)
                {
                    /*Set data by group*/
                    List<ProveedoresOnLine.CompanyProvider.Models.Provider.BlackListModel> oInfoToPrint = new List<ProveedoresOnLine.CompanyProvider.Models.Provider.BlackListModel>();
                    oInfoToPrint = oModel.RelatedBlackListInfo.Where(x => x.BlackListInfo.Any(a => a.Value == grp)).Select(x => x).ToList();

                    #region LISTAS RESTRICTIVAS - Criticidad Alta

                    //rst "LISTAS RESTRICTIVAS - Criticidad Alta"
                    if (grp != null && grp.CompareTo("LISTAS RESTRICTIVAS - Criticidad Alta") == 0)
                    {
                        data_rst.Columns.Add("IdentificationResult");
                        data_rst.Columns.Add("NameResult");
                        data_rst.Columns.Add("Offense");
                        data_rst.Columns.Add("Peps");
                        data_rst.Columns.Add("Priority");
                        data_rst.Columns.Add("Status");
                        data_rst.Columns.Add("ListName");
                        data_rst.Columns.Add("IdentificationSearch");
                        data_rst.Columns.Add("NameSearch");
                        data_rst.Columns.Add("Cargo");
                        DataRow row_rst;

                        foreach (var item in oInfoToPrint)
                        {
                            row_rst = data_rst.NewRow();
                            row_rst["IdentificationResult"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Documento de Identidad").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_rst["NameResult"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre Completo").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_rst["Offense"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Cargo o Delito").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_rst["Peps"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Peps").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_rst["Priority"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Prioridad").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_rst["Status"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Estado").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault().ToLower();
                            row_rst["ListName"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre de la Lista").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_rst["IdentificationSearch"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Identificación Consultada").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_rst["NameSearch"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre Consultado").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_rst["Cargo"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Cargo").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            data_rst.Rows.Add(row_rst);
                        }
                    }

                    #endregion

                    #region  DELITOS E INHABILIDADES CONTRA EL ESTADO - Criticidad Media

                    //dce "DELITOS E INHABILIDADES CONTRA EL ESTADO - Criticidad Media"
                    if (grp != null && grp.CompareTo("DELITOS E INHABILIDADES CONTRA EL ESTADO - Criticidad Media") == 0)
                    {
                        data_dce.Columns.Add("IdentificationResult");
                        data_dce.Columns.Add("NameResult");
                        data_dce.Columns.Add("Offense");
                        data_dce.Columns.Add("Peps");
                        data_dce.Columns.Add("Priority");
                        data_dce.Columns.Add("Status");
                        data_dce.Columns.Add("ListName");
                        data_dce.Columns.Add("IdentificationSearch");
                        data_dce.Columns.Add("NameSearch");
                        data_dce.Columns.Add("Cargo");
                        DataRow row_dce;
                        foreach (var item in oInfoToPrint)
                        {
                            row_dce = data_dce.NewRow();
                            row_dce["IdentificationResult"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Documento de Identidad").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_dce["NameResult"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre Completo").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_dce["Offense"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Cargo o Delito").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_dce["Peps"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Peps").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_dce["Priority"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Prioridad").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_dce["Status"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Estado").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault().ToLower();
                            row_dce["ListName"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre de la Lista").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_dce["IdentificationSearch"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Identificación Consultada").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_dce["NameSearch"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre Consultado").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_dce["Cargo"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Cargo").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            data_dce.Rows.Add(row_dce);
                        }
                    }

                    #endregion

                    #region LISTAS FINANCIERAS - Criticidad Media

                    //fnc "LISTAS FINANCIERAS - Criticidad Media"
                    if (grp != null && grp.CompareTo("LISTAS FINANCIERAS - Criticidad Media") == 0)
                    {
                        data_fnc.Columns.Add("IdentificationResult");
                        data_fnc.Columns.Add("NameResult");
                        data_fnc.Columns.Add("Offense");
                        data_fnc.Columns.Add("Peps");
                        data_fnc.Columns.Add("Priority");
                        data_fnc.Columns.Add("Status");
                        data_fnc.Columns.Add("ListName");
                        data_fnc.Columns.Add("IdentificationSearch");
                        data_fnc.Columns.Add("NameSearch");
                        data_fnc.Columns.Add("Cargo");
                        DataRow row_fnc;
                        foreach (var item in oInfoToPrint)
                        {
                            row_fnc = data_fnc.NewRow();
                            row_fnc["IdentificationResult"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Documento de Identidad").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_fnc["NameResult"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre Completo").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_fnc["Offense"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Cargo o Delito").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_fnc["Peps"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Peps").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_fnc["Priority"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Prioridad").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_fnc["Status"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Estado").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault().ToLower();
                            row_fnc["ListName"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre de la Lista").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_fnc["IdentificationSearch"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Identificación Consultada").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_fnc["NameSearch"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre Consultado").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_fnc["Cargo"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Cargo").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            data_fnc.Rows.Add(row_fnc);
                        }
                    }

                    #endregion

                    #region LISTAS PEPS - Criticidad Baja

                    //psp "LISTAS PEPS - Criticidad Baja"
                    if (grp != null && grp.CompareTo("LISTAS PEPS - Criticidad Baja") == 0)
                    {
                        data_psp.Columns.Add("IdentificationResult");
                        data_psp.Columns.Add("NameResult");
                        data_psp.Columns.Add("Offense");
                        data_psp.Columns.Add("Peps");
                        data_psp.Columns.Add("Priority");
                        data_psp.Columns.Add("Status");
                        data_psp.Columns.Add("ListName");
                        data_psp.Columns.Add("IdentificationSearch");
                        data_psp.Columns.Add("NameSearch");
                        data_psp.Columns.Add("Cargo");
                        DataRow row_psp;
                        foreach (var item in oInfoToPrint)
                        {
                            row_psp = data_psp.NewRow();
                            row_psp["IdentificationResult"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Documento de Identidad").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_psp["NameResult"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre Completo").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_psp["Offense"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Cargo o Delito").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_psp["Peps"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Peps").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_psp["Priority"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Prioridad").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_psp["Status"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Estado").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault().ToLower();
                            row_psp["ListName"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre de la Lista").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_psp["IdentificationSearch"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Identificación Consultada").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_psp["NameSearch"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Nombre Consultado").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_psp["Cargo"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Cargo").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            data_psp.Rows.Add(row_psp);
                        }
                    }

                    #endregion

                    #region Sin Coincidencias

                    //na "No hay coincidencias"
                    if (grp != null && grp.CompareTo("SIN COINCIDENCIAS") == 0)
                    {
                        data_na.Columns.Add("NameSearch");
                        data_na.Columns.Add("IdentificationSearch");
                        data_na.Columns.Add("Appointment");
                        DataRow row_na;
                        foreach (var item in oInfoToPrint)
                        {
                            row_na = data_na.NewRow();
                            row_na["NameSearch"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Razón Social" || x.ItemInfoType.ItemName == "Nombre Consultado").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_na["IdentificationSearch"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Identificación Consultada").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            row_na["Appointment"] = item.BlackListInfo.Where(x => x.ItemInfoType.ItemName == "Cargo").Select(x => x.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                            data_na.Rows.Add(row_na);
                        }
                    }

                    #endregion
                }

                string fileFormat = Request["ThirdKnowledge_cmbFormat"] != null ? Request["ThirdKnowledge_cmbFormat"].ToString() : "pdf";
                Tuple<byte[], string, string> ThirdKnowledgeReport = ProveedoresOnLine.Reports.Controller.ReportModule.TK_GIBlackListQueryReport(
                                                                fileFormat,
                                                                data_rst,
                                                                data_dce,
                                                                data_fnc,
                                                                data_psp,
                                                                data_na,
                                                                parameters,
                                                                Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "TK_Report_GIBlackListReport.rdlc");
                parameters = null;
                return File(ThirdKnowledgeReport.Item1, ThirdKnowledgeReport.Item2, ThirdKnowledgeReport.Item3);
            }

            return View(oModel);
        }

        public virtual ActionResult GIBlackListDetail(string ProviderPublicId, int BlackListId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;
            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                 Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                             (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                             x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();
            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                ////get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedTrackingInfo = new List<GenericItemModel>();
                oModel.RelatedBlackListInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListGetBasicInfo(ProviderPublicId);
                oModel.RelatedBlackListInfo = oModel.RelatedBlackListInfo.Where(x => x.BlackListId == BlackListId).Select(x => x).ToList();
                Models.ThirdKnowledge.ThirdKnowledgeViewModel tk_search = new Models.ThirdKnowledge.ThirdKnowledgeViewModel();
                oModel.RelatedThidKnowledgeSearch = tk_search;
                oModel.RelatedBlackListInfo.All(x =>
                {
                    x.BlackListInfo.All(y =>
                    {
                        if (y.ItemInfoType.ItemName == "Alias") { oModel.RelatedThidKnowledgeSearch.Alias = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Nombre Consultado" || y.ItemInfoType.ItemName == "Razón Social") { oModel.RelatedThidKnowledgeSearch.RequestName = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Identificación Consultada") { oModel.RelatedThidKnowledgeSearch.IdentificationNumberResult = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Cargo o Delito") { oModel.RelatedThidKnowledgeSearch.Offense = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Peps") { oModel.RelatedThidKnowledgeSearch.Peps = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Prioridad") { oModel.RelatedThidKnowledgeSearch.Priority = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Estado") { oModel.RelatedThidKnowledgeSearch.Status = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Fecha Registro") { oModel.RelatedThidKnowledgeSearch.RegisterDate = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Documento de Identidad") { oModel.RelatedThidKnowledgeSearch.IdNumberRequest = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Fecha de Actualizacion") { oModel.RelatedThidKnowledgeSearch.LastModifyDate = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Nombre del Grupo") { oModel.RelatedThidKnowledgeSearch.GroupName = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Nombre Completo") { oModel.RelatedThidKnowledgeSearch.NameResult = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Nombre de la Lista") { oModel.RelatedThidKnowledgeSearch.ListName = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Otra Información") { oModel.RelatedThidKnowledgeSearch.MoreInfo = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Zona") { oModel.RelatedThidKnowledgeSearch.Zone = y.Value == null ? string.Empty : y.Value; }
                        if (y.ItemInfoType.ItemName == "Link") { oModel.RelatedThidKnowledgeSearch.Link = y.Value == null ? string.Empty : y.Value; }
                        return true;
                    });
                    return true;
                });
            }
            //if report download
            oModel.ProviderMenu = GetProviderMenu(oModel);


            //Get report generator
            if (Request["DownloadReport"] == "true" && oModel.RelatedBlackListInfo != null && oModel.RelatedBlackListInfo.Count > 0)
            {
                #region Set Parameters
                List<ReportParameter> parameters = new List<ReportParameter>();
                //Customer Info
                parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName != null ? SessionModel.CurrentCompany.CompanyName : "--"));
                parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber != null ? SessionModel.CurrentCompany.IdentificationNumber : "--"));
                parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName != null ? SessionModel.CurrentCompany.IdentificationType.ItemName : "--"));
                parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo != null ? SessionModel.CurrentCompany_CompanyLogo : "--"));
                parameters.Add(new ReportParameter("SearchName", oModel.RelatedThidKnowledgeSearch.NameResult != null ? oModel.RelatedThidKnowledgeSearch.NameResult : "--"));
                parameters.Add(new ReportParameter("SearchIdentification", oModel.RelatedThidKnowledgeSearch.IdentificationNumberResult != null ? oModel.RelatedThidKnowledgeSearch.IdentificationNumberResult : "--"));
                //Query Detail Info

                parameters.Add(new ReportParameter("ThirdKnowledgeText", MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.MP_TK_TextImage].Value != null ? MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.MP_TK_TextImage].Value : "--"));
                parameters.Add(new ReportParameter("NameResult", oModel.RelatedThidKnowledgeSearch.RequestName != null && oModel.RelatedThidKnowledgeSearch.RequestName.Length > 0 ? oModel.RelatedThidKnowledgeSearch.RequestName : "--"));
                parameters.Add(new ReportParameter("IdentificationType", "--"));
                parameters.Add(new ReportParameter("IdentificationNumber", oModel.RelatedThidKnowledgeSearch.IdNumberRequest != null ? oModel.RelatedThidKnowledgeSearch.IdNumberRequest : "--"));
                parameters.Add(new ReportParameter("Zone", oModel.RelatedThidKnowledgeSearch.Zone != null && oModel.RelatedThidKnowledgeSearch.Zone.Length > 0 ? oModel.RelatedThidKnowledgeSearch.Zone : "--"));
                parameters.Add(new ReportParameter("Priority", oModel.RelatedThidKnowledgeSearch.Priority != null && oModel.RelatedThidKnowledgeSearch.Priority.Length > 0 ? oModel.RelatedThidKnowledgeSearch.Priority : "--"));
                parameters.Add(new ReportParameter("Offence", oModel.RelatedThidKnowledgeSearch.Offense != null && oModel.RelatedThidKnowledgeSearch.Offense.Length > 0 ? oModel.RelatedThidKnowledgeSearch.Offense : "--"));
                parameters.Add(new ReportParameter("Peps", oModel.RelatedThidKnowledgeSearch.Peps != null && oModel.RelatedThidKnowledgeSearch.Peps.Length > 0 ? oModel.RelatedThidKnowledgeSearch.Peps : "--"));
                parameters.Add(new ReportParameter("ListName", oModel.RelatedThidKnowledgeSearch.ListName != null && oModel.RelatedThidKnowledgeSearch.ListName.Length > 0 ? oModel.RelatedThidKnowledgeSearch.ListName : "--"));
                parameters.Add(new ReportParameter("Alias", oModel.RelatedThidKnowledgeSearch.Alias != null && oModel.RelatedThidKnowledgeSearch.Alias.Length > 0 ? oModel.RelatedThidKnowledgeSearch.Alias : "--"));
                parameters.Add(new ReportParameter("LastUpdate", oModel.RelatedThidKnowledgeSearch.LastModifyDate != null && oModel.RelatedThidKnowledgeSearch.LastModifyDate.Length > 0 ? oModel.RelatedThidKnowledgeSearch.LastModifyDate : "--"));
                parameters.Add(new ReportParameter("QueryCreateDate", DateTime.Now.AddHours(-5).ToString()));
                parameters.Add(new ReportParameter("Link", oModel.RelatedThidKnowledgeSearch.Link != null && oModel.RelatedThidKnowledgeSearch.Link.Length > 0 ? oModel.RelatedThidKnowledgeSearch.Link : "--"));
                parameters.Add(new ReportParameter("MoreInformation", oModel.RelatedThidKnowledgeSearch.MoreInfo != null && oModel.RelatedThidKnowledgeSearch.MoreInfo.Length > 0 ? oModel.RelatedThidKnowledgeSearch.MoreInfo : "--"));
                parameters.Add(new ReportParameter("User", "ProveedoresOnLine"));
                parameters.Add(new ReportParameter("ReportCreateDate", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == 203012).Select(x => x.Value).DefaultIfEmpty("--").FirstOrDefault()));
                parameters.Add(new ReportParameter("Group", oModel.RelatedThidKnowledgeSearch.GroupName != null && oModel.RelatedThidKnowledgeSearch.GroupName.Length > 0 ? oModel.RelatedThidKnowledgeSearch.GroupName : "--"));
                parameters.Add(new ReportParameter("Status", oModel.RelatedThidKnowledgeSearch.Status != null && oModel.RelatedThidKnowledgeSearch.Status.Length > 0 ? oModel.RelatedThidKnowledgeSearch.Status : "--"));

                string fileFormat = Request["ThirdKnowledge_cmbFormat"] != null ? Request["ThirdKnowledge_cmbFormat"].ToString() : "pdf";
                Tuple<byte[], string, string> ThirdKnowledgeReport = ProveedoresOnLine.Reports.Controller.ReportModule.TK_QueryDetailReport(
                                                                fileFormat,
                                                                parameters,
                                                                Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "TK_Report_GIBlackListReportDetail.rdlc");
                parameters = null;
                return File(ThirdKnowledgeReport.Item1, ThirdKnowledgeReport.Item2, ThirdKnowledgeReport.Item3);

                #endregion
            }
            return View(oModel);
        }

        public virtual ActionResult GICalificationProjectDetail(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;
            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> oCalProject = new List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel>();
                oCalProject = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProject_GetByCustomer(SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId, true);

                List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigValidateModel> oValidateModel = new List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigValidateModel>();

                if (oCalProject != null &&
                    oCalProject.Count > 0)
                {
                    oValidateModel = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectValidate_GetByProjectConfigId(oCalProject.FirstOrDefault().ProjectConfigModel.CalificationProjectConfigId, true);
                    oModel.ProviderCalification = new ProviderCalificationViewModel();
                    oModel.ProviderCalification.ProRelatedCalificationProject = oCalProject;
                    oModel.ProviderCalification.TotalScore = oCalProject.FirstOrDefault().TotalScore;
                    oModel.ProviderCalification.TotalCalification = GetCalificationScore(oCalProject, oValidateModel);
                }
                else
                {
                    oModel.ProviderCalification.ProRelatedCalificationProject = new List<CalificationProjectBatchModel>();
                    oModel.ProviderCalification.TotalScore = 0;
                    oModel.ProviderCalification.TotalCalification = string.Empty;
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            if (Request["DownloadReport"] == "true")
            {
                //Add Report download
            }

            return View(oModel);
        }

        #endregion General Info

        #region Legal Info

        public virtual ActionResult LIChaimberOfCommerceInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                  Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                              (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                              x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.ChaimberOfCommerce);

                List<GenericItemModel> oDesignations = null;

                oDesignations = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.Designations);
                oModel.RelatedDesignationsInfo = new List<ProviderDesignationsViewModel>();

                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }
                else
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<GenericItemModel>();
                }
                oModel.RelatedDesignationsInfo = new List<ProviderDesignationsViewModel>();
                if (oDesignations != null && oDesignations.Count > 0)
                {
                    oDesignations.All(x =>
                    {
                        oModel.RelatedDesignationsInfo.Add(new ProviderDesignationsViewModel(x));
                        return true;
                    });
                }
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult LIRutInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                  Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                              (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                              x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.RUT);
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult LICIFINInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;
            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                 Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                             (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                             x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.CIFIN);
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult LISARLAFTInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                  Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                              (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                              x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.SARLAFT);
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();
                List<CatalogModel> oEntitieType = Models.Company.CompanyUtil.ProviderOptions.Where(x => x.CatalogId == 212).Select(x => x).ToList();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                    oModel.RelatedLegalInfo = oModel.RelatedLegalInfo.OrderByDescending(x => Convert.ToDateTime(x.SF_ProcessDate)).ToList();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult LIResolutionInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.Resoluciones);
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        #endregion Legal Info

        #region Financial Info

        public virtual ActionResult FIBalanceSheetInfo(string ProviderPublicId, string ViewName, string Year, string Currency)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get request info
                int? oYear = !string.IsNullOrEmpty(Request["Year"]) ?
                    (int?)Convert.ToInt32(Request["Year"].Replace(" ", "")) :
                    null;

                int oCurrencyValidate = 0;

                int oCurrencyType = !string.IsNullOrEmpty(Request["Currency"]) && int.TryParse(Request["Currency"].ToString(), out oCurrencyValidate) == true ?
                    Convert.ToInt32(Request["Currency"].Replace(" ", "")) :
                    Convert.ToInt32(Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_CurrencyExchange_COP].Value);

                string oViewName = !string.IsNullOrEmpty(Request["ViewName"]) ?
                    Request["ViewName"].Replace(" ", "") :
                    MVC.Desktop.Shared.Views.ViewNames._P_FI_Indicators;

                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCompanyGetBasicInfo(ProviderPublicId);

                //get balance info
                List<BalanceSheetModel> oBalanceAux =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPBalanceSheetGetByYear
                    (ProviderPublicId,
                    oYear,
                    oCurrencyType);

                //fill view models
                oModel.RelatedFinancialInfo = new List<ProviderFinancialViewModel>();
                if (oBalanceAux != null && oBalanceAux.Count > 0)
                {
                    oBalanceAux.All(bs =>
                    {
                        oModel.RelatedFinancialInfo.Add(new ProviderFinancialViewModel(bs));
                        return true;
                    });
                }

                oModel.RelatedBalanceSheetInfo = new List<ProviderBalanceSheetViewModel>();
                if (oBalanceAux != null && oBalanceAux.Count > 0)
                {
                    List<BalanceSheetModel> oBalancetemp = new List<BalanceSheetModel>();

                    foreach (var item in oBalanceAux)
                    {
                        if (item.BalanceSheetInfo.Count == 0)
                        {
                            oBalancetemp.Add(item);
                        }
                    }

                    int cont = 0;
                    foreach (var item in oBalancetemp)
                    {
                        if (cont < 1)
                        {
                            oBalanceAux.Remove(item);
                            cont++;
                        }
                    }

                    oModel.RelatedBalanceSheetInfo = GetBalanceSheetViewModel
                        (null,
                        oBalanceAux,
                        oViewName);
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            if (Request["DownloadReportFinancial"] == "true")
            {
                // GET ALL BASIC INFO
                ProviderModel response = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPGetBasicInfo(ProviderPublicId);

                #region Basic Info

                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany = response.RelatedCompany;

                oModel.ContactCompanyInfo = response.ContactCompanyInfo;
                oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

                if (oModel.ContactCompanyInfo != null)
                {
                    oModel.ContactCompanyInfo.All(x =>
                    {
                        oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                        return true;
                    });
                }

                #endregion Basic Info

                #region Legal Info

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = response.RelatedLegal;
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }

                #endregion Legal Info

                #region Basic Financial Info

                List<GenericItemModel> oFinancial = response.RelatedFinantial;
                oModel.RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                if (oFinancial != null)
                {
                    decimal oExchange;
                    oExchange = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(
                                Convert.ToInt32(oFinancial.FirstOrDefault().ItemInfo.FirstOrDefault().ValueName),
                                Convert.ToInt32(Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_CurrencyExchange_COP].Value),
                                Convert.ToInt32(oFinancial.FirstOrDefault().ItemName));

                    oFinancial.All(x =>
                    {
                        oModel.RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(x, oExchange));
                        return true;
                    });

                    if (oModel.RelatedFinancialBasicInfo != null && oModel.RelatedFinancialBasicInfo.Count > 0)
                    {
                        oModel.RelatedFinancialBasicInfo.FirstOrDefault().BI_JobCapital =
                            ((Convert.ToDecimal(oModel.RelatedFinancialBasicInfo.Where(x => !string.IsNullOrWhiteSpace(x.BI_CurrentActive)).Select(x => x.BI_CurrentActive).DefaultIfEmpty("0").FirstOrDefault())
                            - Convert.ToDecimal(oModel.RelatedFinancialBasicInfo.Where(x => !string.IsNullOrWhiteSpace(x.BI_CurrentPassive)).Select(x => x.BI_CurrentPassive).DefaultIfEmpty("0").FirstOrDefault()))).ToString("#,0.##");
                    }
                }

                #endregion Basic Financial Info

                #region K_Contract Info

                List<GenericItemModel> oRelatedKContractInfo = response.RelatedKContractInfo;
                oModel.RelatedKContractInfo = new List<ProviderFinancialViewModel>();
                if (oRelatedKContractInfo != null)
                {
                    oRelatedKContractInfo.All(x =>
                    {
                        oModel.RelatedKContractInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });
                }

                #endregion K_Contract Info

                #region HSEQ

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<GenericItemModel>();
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = response.RelatedCertification;
                oModel.RelatedHSEQlInfo = new List<ProviderHSEQViewModel>();
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null
                && oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.Count > 0)
                {
                    List<GenericItemModel> basicCert = response.RelatedCertificationBasic;
                    if (basicCert != null && basicCert.Count > 0)
                    {
                        oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.AddRange(basicCert);
                    }

                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.All(x =>
                    {
                        oModel.RelatedHSEQlInfo.Add(new ProviderHSEQViewModel(x));
                        return true;
                    });
                }

                oModel.RelatedCertificationBasicInfo = response.RelatedCertificationBasicInfo;

                #endregion HSEQ

                GenericReportModel Report = RPFinancial(oModel);
                return File(Report.File, Report.MimeType, Report.FileName);
            }

            return View(oModel);
        }

        public virtual ActionResult FITaxInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                 Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                             (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                             x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetBasicInfo(ProviderPublicId, (int)enumFinancialType.TaxInfoType);
                oModel.RelatedFinancialInfo = new List<ProviderFinancialViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial.All(x =>
                    {
                        oModel.RelatedFinancialInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });

                    oModel.RelatedFinancialInfo = oModel.RelatedFinancialInfo.OrderByDescending(x => Convert.ToInt32(!string.IsNullOrEmpty(x.TX_Year) ? x.TX_Year : string.Empty)).ToList();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult FIIncomeStatementInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                 Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                             (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                             x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetBasicInfo(ProviderPublicId, (int)enumFinancialType.IncomeStatementInfoType);
                oModel.RelatedFinancialInfo = new List<ProviderFinancialViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial.All(x =>
                    {
                        oModel.RelatedFinancialInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult FIBankInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                 Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                             (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                             x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetBasicInfo(ProviderPublicId, (int)enumFinancialType.BankInfoType);
                oModel.RelatedFinancialInfo = new List<ProviderFinancialViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial.All(x =>
                    {
                        oModel.RelatedFinancialInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult FIKContract(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;
            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetBasicInfo(ProviderPublicId, (int)enumFinancialType.KRecruitment);
                oModel.RelatedFinancialInfo = new List<ProviderFinancialViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial.All(x =>
                    {
                        oModel.RelatedFinancialInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        #region Private Methods

        private List<ProviderBalanceSheetViewModel> GetBalanceSheetViewModel
            (GenericItemModel RelatedAccount,
            List<BalanceSheetModel> lstBalanceSheet,
            string oViewName)
        {
            List<ProviderBalanceSheetViewModel> oReturn = new List<ProviderBalanceSheetViewModel>();

            Models.Company.CompanyUtil.FinancialAccount.
                Where(ac =>
                    RelatedAccount != null ?
                        (ac.ParentItem != null && ac.ParentItem.ItemId == RelatedAccount.ItemId) :
                        (ac.ParentItem == null &&
                            (oViewName == MVC.Desktop.Shared.Views.ViewNames._P_FI_Indicators ? ac.ItemId == 4915 : ac.ItemId != 4915))).
                OrderBy(ac => ac.ItemInfo.
                    Where(aci => aci.ItemInfoType.ItemId == (int)enumCategoryInfoType.AI_Order).
                    Select(aci => Convert.ToInt32(aci.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault()).
                All(ac =>
                {
                    ProviderBalanceSheetViewModel oItemToAdd =
                        new Models.Provider.ProviderBalanceSheetViewModel()
                        {
                            RelatedAccount = ac,
                            RelatedBalanceSheetDetail = new List<ProviderBalanceSheetDetailViewModel>(),
                        };

                    int oOrder = 0;
                    decimal oHorizontalValue = 0;
                    string oAccountUnit = ac.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)enumCategoryInfoType.AI_Unit).
                        Select(y => y.Value.Replace(" ", "")).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    lstBalanceSheet.
                        OrderByDescending(bs => bs.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.SH_Year).
                            Select(y => Convert.ToInt32(y.Value)).
                            DefaultIfEmpty(DateTime.Now.Year).
                            FirstOrDefault()).
                        All(bs =>
                        {
                            var oItemDetailToAdd = new ProviderBalanceSheetDetailViewModel()
                            {
                                Order = oOrder,
                            };

                            //get balance to add
                            if (bs.BalanceSheetInfo != null &&
                                bs.BalanceSheetInfo.Count > 0 &&
                                bs.BalanceSheetInfo.Any(bsd => bsd.RelatedAccount.ItemId == ac.ItemId))
                            {
                                //get item to add
                                oItemDetailToAdd = new ProviderBalanceSheetDetailViewModel()
                                {
                                    RelatedBalanceSheetDetail = bs.BalanceSheetInfo.
                                        Where(bsd => bsd.RelatedAccount.ItemId == ac.ItemId).
                                        FirstOrDefault(),
                                };
                            }
                            else
                            {
                                oItemDetailToAdd.RelatedBalanceSheetDetail = new BalanceSheetDetailModel()
                                {
                                    RelatedAccount = ac,
                                    Value = 0,
                                };
                            }

                            #region Eval Vertical Formula

                            string strVerticalFormula = ac.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)enumCategoryInfoType.AI_VerticalFormula).
                                Select(x => x.LargeValue).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault().ToLower().Replace(" ", "");

                            if (!string.IsNullOrEmpty(strVerticalFormula))
                            {
                                //loop for standar values
                                foreach (var RegexResult in (new Regex("\\[+\\d*\\]+", RegexOptions.IgnoreCase)).Matches(strVerticalFormula))
                                {
                                    int oAccountId = Convert.ToInt32(RegexResult.ToString().Replace("[", "").Replace("]", ""));

                                    decimal oAccountValue = bs.BalanceSheetInfo.
                                        Where(bsd => bsd.RelatedAccount.ItemId == oAccountId).
                                        Select(bsd => bsd.Value != 0 ? bsd.Value : 1).
                                        DefaultIfEmpty(1).
                                        FirstOrDefault();

                                    strVerticalFormula = strVerticalFormula.Replace
                                        (RegexResult.ToString(),
                                        oAccountValue.ToString("0.0").Replace(",", "."));
                                }
                                oItemDetailToAdd.VerticalValue = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MathEval(strVerticalFormula);
                            }

                            #endregion Eval Vertical Formula

                            //add balance detail value
                            oItemToAdd.RelatedBalanceSheetDetail.Add(oItemDetailToAdd);
                            oOrder++;

                            if (oAccountUnit == "$" && oItemDetailToAdd.RelatedBalanceSheetDetail != null)
                            {
                                #region Horizontal value

                                //calc horizontal value
                                if (oOrder == 1)
                                {
                                    oHorizontalValue = oItemDetailToAdd.RelatedBalanceSheetDetail.Value;
                                }
                                else if (oOrder > 1)
                                {
                                    oHorizontalValue = oHorizontalValue - oItemDetailToAdd.RelatedBalanceSheetDetail.Value;
                                }

                                #endregion Horizontal value
                            }

                            return true;
                        });

                    //add horizontal analisis value
                    oItemToAdd.HorizontalValue = oHorizontalValue;

                    //get child values
                    oItemToAdd.ChildBalanceSheet = GetBalanceSheetViewModel
                        (ac,
                        lstBalanceSheet,
                        oViewName);

                    //add account item
                    oReturn.Add(oItemToAdd);

                    return true;
                });

            return oReturn;
        }

        #endregion Private Methods

        #endregion Financial Info

        #region Commercial Info

        public virtual ActionResult CIExperiencesInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCommercialGetBasicInfo
                    (ProviderPublicId,
                    (int)enumCommercialType.Experience,
                    SessionModel.CurrentCompany.CompanyPublicId);

                oModel.RelatedComercialInfo = new List<ProviderComercialViewModel>();
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial != null
                    && oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial.Count > 0)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial.All(x =>
                    {
                        oModel.RelatedComercialInfo.Add(new ProviderComercialViewModel(x));
                        return true;
                    });
                }
                else
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial = new List<GenericItemModel>();

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        #endregion Commercial Info

        #region HSEQ Info

        public virtual ActionResult HICertificationsInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                  Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                              (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                              x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.Certifications);
                oModel.RelatedHSEQlInfo = new List<ProviderHSEQViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.All(x =>
                    {
                        oModel.RelatedHSEQlInfo.Add(new ProviderHSEQViewModel(x));
                        return true;
                    });
                }
                else
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<GenericItemModel>();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult HIHealtyPoliticInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.CompanyHealtyPolitic);
                oModel.RelatedHSEQlInfo = new List<ProviderHSEQViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.All(x =>
                    {
                        oModel.RelatedHSEQlInfo.Add(new ProviderHSEQViewModel(x));
                        return true;
                    });

                    oModel.RelatedHSEQlInfo = oModel.RelatedHSEQlInfo.OrderByDescending(x => !string.IsNullOrEmpty(x.CH_Year) ? Convert.ToInt32(x.CH_Year).ToString() : "0").ToList();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult HIRiskPoliciesInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model

                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                List<GenericItemModel> certARL = new List<GenericItemModel>();
                List<GenericItemModel> certAccident = new List<GenericItemModel>();
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<GenericItemModel>();

                certAccident = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo
                    (ProviderPublicId, (int)enumHSEQType.CertificatesAccident);

                if (certAccident != null)
                {
                    foreach (var item in certAccident)
                    {
                        oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.Add(item);
                    }
                }

                certARL = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo
                    (ProviderPublicId, (int)enumHSEQType.CompanyRiskPolicies);

                if (certARL != null)
                {
                    foreach (var item in certARL)
                    {
                        oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.Add(item);
                    }
                }

                oModel.RelatedHSEQlInfo = new List<ProviderHSEQViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.All(x =>
                    {
                        oModel.RelatedHSEQlInfo.Add(new ProviderHSEQViewModel(x));
                        return true;
                    });
                }
                else
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<GenericItemModel>();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        #endregion HSEQ Info

        #region Aditional Document Info

        public virtual ActionResult ADAditionalDocument(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedAditionalDocuments = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPAditionalDocumentGetInfoByType(SessionModel.CurrentCompany.CompanyPublicId, (int)MarketPlace.Models.General.enumAditionalDocumentType.AditionalDocument, ProviderPublicId);
                oModel.RelatedAditionalDocumentBasicInfo = new List<ProviderAditionalDocumentViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedAditionalDocuments != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedAditionalDocuments.All(x =>
                    {
                        oModel.RelatedAditionalDocumentBasicInfo.Add(new ProviderAditionalDocumentViewModel(x));

                        return true;
                    });
                }
                else
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedAditionalDocuments = new List<GenericItemModel>();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }

        public virtual ActionResult ADAditionalData(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedAditionalDocuments = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPAditionalDocumentGetInfoByType(SessionModel.CurrentCompany.CompanyPublicId, (int)MarketPlace.Models.General.enumAditionalDocumentType.AditionalData, ProviderPublicId);
                oModel.RelatedAditionalDocumentBasicInfo = new List<ProviderAditionalDocumentViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedAditionalDocuments != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedAditionalDocuments.All(x =>
                    {
                        oModel.RelatedAditionalDocumentBasicInfo.Add(new ProviderAditionalDocumentViewModel(x));

                        return true;
                    });
                }
                else
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedAditionalDocuments = new List<GenericItemModel>();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }

        #endregion Aditional Document Info

        #region Survey

        public virtual ActionResult SVSurveySearch
            (string ProviderPublicId,
            string SearchOrderType,
            string OrderOrientation,
            string PageNumber,
            string InitDate,
            string EndDate)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.ProviderMenu = GetProviderMenu(oModel);

                oModel.RelatedSurveySearch = new Models.Survey.SurveySearchViewModel()
                {
                    SearchOrderType = string.IsNullOrEmpty(SearchOrderType) ? enumSurveySearchOrderType.LastModify : (enumSurveySearchOrderType)Convert.ToInt32(SearchOrderType),
                    OrderOrientation = string.IsNullOrEmpty(OrderOrientation) ? false : ((OrderOrientation.Trim().ToLower() == "1") || (OrderOrientation.Trim().ToLower() == "true")),
                    PageNumber = string.IsNullOrEmpty(PageNumber) ? 0 : Convert.ToInt32(PageNumber),
                    SurveySearchResult = new List<Models.Survey.SurveyViewModel>(),
                };

                if (SessionModel.CurrentCompany != null &&
                !string.IsNullOrEmpty(SessionModel.CurrentCompany.CompanyPublicId))
                {
                    //search survey
                    int oTotalRowsAux;
                    List<SurveyModel> oSurveyResults = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveySearch
                            (SessionModel.CurrentCompany.CompanyPublicId,
                            oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                            (int)oModel.RelatedSurveySearch.SearchOrderType,
                            oModel.RelatedSurveySearch.OrderOrientation,
                            oModel.RelatedSurveySearch.PageNumber,
                            oModel.RelatedSurveySearch.RowCount,
                            out oTotalRowsAux);

                    //Validar q no tenga evaluaciones
                    if (SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().RelatedCompanyRole.ParentRoleCompany == null)
                    {
                        if (oSurveyResults != null)
                        {
                            oSurveyResults = oSurveyResults.Where(x => x.ParentSurveyPublicId == null).Select(x => x).ToList();
                        }
                    }
                    else
                    {
                        if (oSurveyResults != null)
                        {
                            List<SurveyModel> oChildSurvey = new List<SurveyModel>();
                            oSurveyResults.All(x =>
                                {
                                    oChildSurvey.Add(ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetByUser(x.SurveyPublicId, SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().User));

                                    oChildSurvey.All(y =>
                                    {
                                        if (y != null && y.ParentSurveyPublicId == x.SurveyPublicId && y.SurveyInfo.Count == 0)
                                            y.SurveyInfo = x.SurveyInfo;

                                        return true;
                                    });
                                    return true;
                                });
                            oSurveyResults = oChildSurvey.Where(x => x != null).ToList();
                        }
                    }
                    if (!string.IsNullOrEmpty(InitDate) && !string.IsNullOrEmpty(EndDate)
                        && oSurveyResults != null && oSurveyResults.Count > 0)
                    {
                        oSurveyResults = oSurveyResults.Where(x =>
                                                        Convert.ToDateTime(x.CreateDate.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(InitDate) &&
                                                        Convert.ToDateTime(x.CreateDate.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(EndDate)).
                                                        Select(x => x).ToList();
                    }
                    oModel.RelatedSurveySearch.TotalRows = oTotalRowsAux;

                    //parse view model
                    if (oSurveyResults != null && oSurveyResults.Count > 0)
                    {
                        //Get the Average
                        decimal Average = 0;
                        //Get ClosedSurve
                        List<SurveyModel> ClosedSurvey = oSurveyResults.Where(x => x.SurveyInfo.
                                                        Where(y => y.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).
                                                        Select(y => y.Value == ((int)enumSurveyStatus.Close).ToString()).FirstOrDefault()).
                                                        Select(x => x).ToList();
                        oSurveyResults.All(srv =>
                        {
                            oModel.RelatedSurveySearch.SurveySearchResult.Add
                                (new MarketPlace.Models.Survey.SurveyViewModel(srv));
                            return true;
                        });

                        if (oModel.RelatedSurveySearch.SurveySearchResult != null && oModel.RelatedSurveySearch.SurveySearchResult.Count > 0)
                        {
                            oModel.RelatedSurveySearch.SurveySearchResult.All(sv =>
                            {
                                if (sv.SurveyStatus == enumSurveyStatus.Close)
                                    Average = (Average += Convert.ToDecimal(sv.SurveyRating.ToString("#,0.##")));
                                return true;
                            });
                            Average = Average != 0 ? Average / oModel.RelatedSurveySearch.SurveySearchResult.Where(x => x.SurveyStatus == enumSurveyStatus.Close).Count() : 0;
                        }

                        //Set the Average
                        oModel.RelatedSurveySearch.SurveySearchResult.FirstOrDefault().Average = Average;
                        if (!string.IsNullOrEmpty(InitDate) && !string.IsNullOrEmpty(EndDate))
                        {
                            oModel.RelatedSurveySearch.SurveySearchResult.FirstOrDefault().FilterDateIni = Convert.ToDateTime(InitDate);
                            oModel.RelatedSurveySearch.SurveySearchResult.FirstOrDefault().FilterEndDate = Convert.ToDateTime(EndDate);
                        }
                    }
                }
            }

            #region report generator

            if (Request["UpsertRequest"] == "true")
            {
                List<ReportParameter> parameters = new List<ReportParameter>();
                ProviderModel oToInsert = new ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = ProviderPublicId,
                    },
                    RelatedReports = new List<GenericItemModel>(),
                };
                oToInsert.RelatedReports.Add(this.GetSurveyReportFilterRequest());
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPReportUpsert(oToInsert);

                parameters.Add(new ReportParameter("currentCompanyName", SessionModel.CurrentCompany.CompanyName));
                parameters.Add(new ReportParameter("currentCompanyTipoDni", SessionModel.CurrentCompany.IdentificationType.ItemName));
                parameters.Add(new ReportParameter("currentCompanyDni", SessionModel.CurrentCompany.IdentificationNumber));
                parameters.Add(new ReportParameter("currentCompanyLogo", SessionModel.CurrentCompany_CompanyLogo));
                parameters.Add(new ReportParameter("providerName", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName.ToString()));
                parameters.Add(new ReportParameter("providerTipoDni", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName.ToString()));
                parameters.Add(new ReportParameter("providerDni", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber.ToString()));
                //order items reports
                if (oToInsert.RelatedReports != null)
                {
                    oToInsert.RelatedReports.All(x =>
                    {
                        parameters.Add(new ReportParameter("remarks",
                            x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)enumSurveyInfoType.RP_Observation).Select(y => y.Value).
                            DefaultIfEmpty("-").
                            FirstOrDefault() + "."));

                        parameters.Add(new ReportParameter("actionPlan",
                            x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)enumSurveyInfoType.RP_ImprovementPlan).Select(y => y.Value).
                            DefaultIfEmpty("-").
                            FirstOrDefault() + "."));

                        parameters.Add(new ReportParameter("dateStart", Convert.ToDateTime(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                    (int)enumSurveyInfoType.RP_InitDateReport).Select(y => y.Value).
                                    DefaultIfEmpty("-").
                                    FirstOrDefault()).ToString("dd/MM/yyyy")));

                        parameters.Add(new ReportParameter("dateEnd", Convert.ToDateTime(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)enumSurveyInfoType.RP_EndDateReport).Select(y => y.Value).
                            DefaultIfEmpty("-").
                            FirstOrDefault()).ToString("dd/MM/yyyy")));

                        parameters.Add(new ReportParameter("average",
                            x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)enumSurveyInfoType.RP_ReportAverage).Select(y => y.Value).
                            DefaultIfEmpty("-").
                            FirstOrDefault()));

                        parameters.Add(new ReportParameter("reportDate",
                            x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)enumSurveyInfoType.RP_ReportDate).Select(y => y.Value).
                            DefaultIfEmpty("-").
                            FirstOrDefault()));

                        parameters.Add(new ReportParameter("responsible",
                            x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)enumSurveyInfoType.RP_ReportResponsable).Select(y => y.Value).
                            DefaultIfEmpty("-").
                            FirstOrDefault()));
                        return true;
                    });
                }
                parameters.Add(new ReportParameter("author", SessionModel.CurrentCompanyLoginUser.RelatedUser.Name.ToString() + " " + SessionModel.CurrentCompanyLoginUser.RelatedUser.LastName.ToString()));

                Tuple<byte[], string, string> report = ProveedoresOnLine.Reports.Controller.ReportModule.CP_SurveyReportDetail(
                                                    (int)enumReportType.RP_SurveyReport,
                                                    enumCategoryInfoType.PDF.ToString(),
                                                    parameters,
                                                    Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "SV_Report_SurveyDetail.rdlc");
                parameters = null;
                return File(report.Item1, report.Item2, report.Item3);
            }

            #endregion report generator

            return View(oModel);
        }

        public virtual ActionResult SVSurveyDetail(string ProviderPublicId, string SurveyPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider != null)
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.ProviderMenu = GetProviderMenu(oModel);
                //get survey info
                oModel.RelatedSurvey = new Models.Survey.SurveyViewModel
                    (ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId));
            }

            if (Request["DownloadReport"] == "true")
            {
                GenericReportModel SurveyReport = Report_SurveyGeneral(oModel);
                return File(SurveyReport.File, SurveyReport.MimeType, SurveyReport.FileName);
            }

            return View(oModel);
        }

        public virtual ActionResult SVSurveyEvaluatorDetail(string ProviderPublicId, string SurveyPublicId, string User)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            if (Request["DownloadReport"] == "true")
            {
                return File(Convert.FromBase64String(Request["File"]), Request["MimeType"], Request["FileName"]);
            }
            else
            {
                //get basic provider info
                var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                    (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

                var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

                //validate provider permisions
                if (oProvider != null)
                {
                    //get provider view model
                    oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                    oModel.ProviderMenu = GetProviderMenu(oModel);
                    //get survey info
                    oModel.RelatedSurvey = new Models.Survey.SurveyViewModel
                        (ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetByUser(SurveyPublicId, User));
                }
                oModel.SurveytReportModel = new GenericReportModel();
                oModel.SurveytReportModel = Report_SurveyEvaluatorDetail(oModel);

                return View(oModel);
            }
        }

        public virtual ActionResult SVSurveyReport(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                 Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                             (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                             x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedReports = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPReportGetBasicInfo(ProviderPublicId, (int)enumReportType.RP_SurveyReport);
                oModel.RelatedReportInfo = new List<ProviderReportsViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedReports != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedReports.All(x =>
                    {
                        oModel.RelatedReportInfo.Add(new ProviderReportsViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }

        public virtual ActionResult SVSurveyProgram(string ProviderPublicId, string SurveyPublicId, string ProjectPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                 Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                             (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                             x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
                {
                    SurveyModel SurveyToUpsert = GetSurveyUpsertRequest();
                    SurveyToUpsert = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyUpsert(SurveyToUpsert);
                }
                if (!string.IsNullOrEmpty(SurveyPublicId))//si es editar
                {
                    if (oProvider != null)
                    {
                        oModel.RelatedSurvey = new Models.Survey.SurveyViewModel(ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId));
                        if (oModel.RelatedSurvey != null)
                        {
                            oModel.RelatedSurvey.RelatedSurvey.ChildSurvey = new List<SurveyModel>();
                            List<string> Evaluators = oModel.RelatedSurvey.SurveyEvaluatorList.GroupBy(x => x).Select(grp => grp.First()).ToList();

                            Evaluators.All(evt =>
                            {
                                oModel.RelatedSurvey.RelatedSurvey.ChildSurvey.Add(
                                (ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetByUser(SurveyPublicId, evt)));
                                return true;
                            });
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(ProjectPublicId))
                {
                    oModel.RelatedProject = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetById(ProjectPublicId, SessionModel.CurrentCompany.CompanyPublicId);
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }

        #endregion Survey

        #region Reports

        public GenericReportModel RPGerencial(ProviderViewModel oModel)
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            GenericReportModel oReporModel = new GenericReportModel();

            #region Set Parameters

            //CustomerInfo
            parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName));
            parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber));
            parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo));

            //ProviderInfo
            parameters.Add(new ReportParameter("ProviderName", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName));
            parameters.Add(new ReportParameter("ProviderIdentificationType", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("ProviderIdentificationNumber", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber));

            #region Basic Info

            if (!string.IsNullOrEmpty(oModel.RelatedGeneralInfo.Where(x => x.PC_RepresentantType == "Legal").Select(x => x.PC_ContactName).FirstOrDefault()))
                parameters.Add(new ReportParameter("Representant", oModel.RelatedGeneralInfo.Where(x => x.PC_RepresentantType == "Legal").Select(x => x.PC_ContactName).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("Representant", "NA"));

            if (oModel.RelatedLegalInfo.Count > 0 && !string.IsNullOrEmpty(oModel.RelatedLegalInfo.FirstOrDefault().CP_InscriptionNumber)
                && !string.IsNullOrWhiteSpace(oModel.RelatedLegalInfo.FirstOrDefault().CP_InscriptionNumber))
                parameters.Add(new ReportParameter("InscriptionNumber", oModel.RelatedLegalInfo.FirstOrDefault().CP_InscriptionNumber));
            else
                parameters.Add(new ReportParameter("InscriptionNumber", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Address).FirstOrDefault()))
                parameters.Add(new ReportParameter("Address", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Address).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("Address", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_City).FirstOrDefault()))
                parameters.Add(new ReportParameter("City", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_City).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("City", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Phone).FirstOrDefault()))
                parameters.Add(new ReportParameter("Phone", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Phone).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("Phone", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Fax).FirstOrDefault()))
                parameters.Add(new ReportParameter("Fax", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Fax).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("Fax", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Website).FirstOrDefault()))
                parameters.Add(new ReportParameter("WebSite", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Website).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("WebSite", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Email).FirstOrDefault()))
                parameters.Add(new ReportParameter("Email", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Email).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("Email", "NA"));

            if (oModel.RelatedLegalInfo.Count > 0 && !string.IsNullOrWhiteSpace(oModel.RelatedLegalInfo.FirstOrDefault().CP_SocialObject))
                parameters.Add(new ReportParameter("SocialObject", oModel.RelatedLegalInfo.FirstOrDefault().CP_SocialObject));
            else
                parameters.Add(new ReportParameter("SocialObject", "NA"));

            #endregion Basic Info

            #region Finacial Info

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrEmpty(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_Year != null).Select(x => x.BI_Year).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("BalanceYear", "AÑO " + oModel.RelatedFinancialBasicInfo.Where(x => x.BI_Year != null).Select(x => x.BI_Year).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("BalanceYear", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrEmpty(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalActive != null).Select(x => x.BI_TotalActive).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("TotalActive", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalActive != null).Select(x => x.BI_TotalActive).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("TotalActive", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPassive != null).Select(x => x.BI_TotalPassive).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("TotalPasive", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPassive != null).Select(x => x.BI_TotalPassive).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("TotalPasive", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPatrimony != null).Select(x => x.BI_TotalPatrimony).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("TotalPatrimony", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPatrimony != null).Select(x => x.BI_TotalPatrimony).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("TotalPatrimony", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_OperationIncome != null).Select(x => x.BI_OperationIncome).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("OperationIncome", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_OperationIncome != null).Select(x => x.BI_OperationIncome).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("OperationIncome", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_IncomeBeforeTaxes != null).Select(x => x.BI_IncomeBeforeTaxes).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("IncomeBeforeTaxes", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_IncomeBeforeTaxes != null).Select(x => x.BI_IncomeBeforeTaxes).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("IncomeBeforeTaxes", "NA"));

            parameters.Add(new ReportParameter("JobCapital", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_JobCapital != null).Select(x => x.BI_JobCapital).DefaultIfEmpty("NA").FirstOrDefault()));

            parameters.Add(new ReportParameter("Altman", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_Altman != null).Select(x => x.BI_Altman).DefaultIfEmpty("NA").FirstOrDefault()));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_ExerciseUtility != null).Select(x => x.BI_ExerciseUtility).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("ExcerciseUtility", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_ExerciseUtility != null).Select(x => x.BI_ExerciseUtility).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("ExcerciseUtility", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_EBITDA != null).Select(x => x.BI_EBITDA).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("EBITDA", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_EBITDA != null).Select(x => x.BI_EBITDA).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("EBITDA", "NA"));

            #endregion Finacial Info

            #region HSEQ Info

            if (oModel.RelatedHSEQlInfo != null && oModel.RelatedHSEQlInfo.Count > 0)
            {
                parameters.Add(new ReportParameter("SystemOccupationalHazards", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards));
                parameters.Add(new ReportParameter("RateARL", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL));
                parameters.Add(new ReportParameter("LTIFResult", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || string.IsNullOrEmpty(oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult) ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult));
            }
            else
            {
                parameters.Add(new ReportParameter("SystemOccupationalHazards", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards));
                parameters.Add(new ReportParameter("RateARL", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL));
                parameters.Add(new ReportParameter("LTIFResult", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult));
            }

            #endregion HSEQ Info

            #region Conocimiento de Terceros

            if (string.IsNullOrEmpty(oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == 203012).Select(x => x.Value).DefaultIfEmpty(string.Empty).FirstOrDefault()))
            {
                parameters.Add(new ReportParameter("LastUpdate", "2015-01-01 12:00:00"));
            }
            else
            {
                parameters.Add(new ReportParameter("LastUpdate", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == 203012).Select(x => x.Value).DefaultIfEmpty(string.Empty).FirstOrDefault()));
            }

            if (oModel.RelatedLiteProvider.ProviderAlertRisk == MarketPlace.Models.General.enumBlackListStatus.DontShowAlert)
            {
                parameters.Add(new ReportParameter("Alert", "No se encontraron coincidencias en listas restrictivas."));
            }
            else
            {
                parameters.Add(new ReportParameter("Alert", "Se encontraron coincidencias en listas restrictivas."));
            }

            oModel.RelatedBlackListInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListGetBasicInfo(oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId);

            DataTable data3 = new DataTable();
            data3.Columns.Add("PersonName");
            data3.Columns.Add("PersonCargo");
            data3.Columns.Add("PersonLista");
            data3.Columns.Add("PersonDelito");
            data3.Columns.Add("PersonState");

            DataRow row3;
            if (oModel.RelatedBlackListInfo != null)
            {
                foreach (var item in oModel.RelatedBlackListInfo.Where(x => x != null))
                {
                    row3 = data3.NewRow();
                    row3["PersonName"] = "N/A";
                    row3["PersonCargo"] = "N/A";
                    row3["PersonLista"] = "N/A";
                    row3["PersonDelito"] = "N/A";
                    row3["PersonState"] = "N/A";
                    foreach (var info in item.BlackListInfo)
                    {
                        if (info.ItemInfoType.ItemName == "RazonSocial")
                        {
                            row3["PersonName"] = info.Value;
                        }
                        else if (info.ItemInfoType.ItemName == "Cargo")
                        {
                            row3["PersonCargo"] = info.Value;
                        }
                        else if (info.ItemInfoType.ItemName == "Lista")
                        {
                            row3["PersonLista"] = info.Value;
                        }
                        else if (info.ItemInfoType.ItemName == "Delito, cargo o resultado de la consulta")
                        {
                            row3["PersonDelito"] = info.Value;
                        }
                        else if (info.ItemInfoType.ItemName == "Estado")
                        {
                            row3["PersonState"] = info.Value;
                        }
                    }
                    data3.Rows.Add(row3);
                }
            }

            #endregion Conocimiento de Terceros

            #region Personas de Contacto

            oModel.ContactCompanyInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId, (int)enumContactType.PersonContact);
            oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

            if (oModel.ContactCompanyInfo != null)
            {
                oModel.ContactCompanyInfo.All(x =>
                {
                    oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                    return true;
                });
            }

            DataTable data2 = new DataTable();
            data2.Columns.Add("ContactType");
            data2.Columns.Add("ContactName");
            data2.Columns.Add("ContactPhone");
            data2.Columns.Add("ContactMail");

            DataRow row2;
            foreach (var item in oModel.RelatedGeneralInfo.Where(x => x != null))
            {
                row2 = data2.NewRow();

                row2["ContactType"] = item.PC_RepresentantType;
                row2["ContactName"] = item.PC_ContactName;
                row2["ContactPhone"] = item.PC_TelephoneNumber;
                row2["ContactMail"] = item.PC_Email;

                data2.Rows.Add(row2);
            }

            #endregion Personas de Contacto

            #region ComercialExperience

            oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCommercialGetBasicInfo
                (oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                (int)enumCommercialType.Experience,
                SessionModel.CurrentCompany.CompanyPublicId);

            if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial != null)
            {
                parameters.Add(new ReportParameter("TotalExperiences", oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial.Count.ToString()));
            }
            else
            {
                parameters.Add(new ReportParameter("TotalExperiences", "0"));
            }

            #endregion ComercialExperience

            #region Kcontratación

            DataTable data = new DataTable();
            data.Columns.Add("EvaluationCriteria");
            data.Columns.Add("Provider");
            data.Columns.Add("Consultant");
            data.Columns.Add("Builder");

            DataRow row;
            foreach (var item in oModel.RelatedKContractInfo.Where(x => x != null))
            {
                row = data.NewRow();

                row["EvaluationCriteria"] = item.FK_RoleType;
                row["Provider"] = Convert.ToDouble(item.FK_TotalScore).ToString("#,0.##");
                row["Consultant"] = Convert.ToDouble(item.FK_TotalOrgCapacityScore).ToString("#,0.##");
                row["Builder"] = Convert.ToDouble(item.FK_TotalKValueScore).ToString("#,0.##");

                data.Rows.Add(row);
            }

            #endregion Kcontratación

            #region CalificationProject
            DataTable data4 = new DataTable();
            data4.Columns.Add("ItemModuleName");
            data4.Columns.Add("ItemScore");

            DataRow row4;

            if (oModel.ProviderCalification.ProRelatedCalificationProject != null &&
                oModel.ProviderCalification.ProRelatedCalificationProject.Count > 0)
            {
                foreach (var CalProject in oModel.ProviderCalification.ProRelatedCalificationProject)
                {
                    foreach (var CalProjectItem in oModel.ProviderCalification.ProRelatedCalificationProject.FirstOrDefault().CalificationProjectItemBatchModel)
                    {
                        row4 = data4.NewRow();

                        row4["ItemModuleName"] = CalProjectItem.CalificationProjectConfigItem.CalificationProjectConfigItemType.ItemName;
                        row4["ItemScore"] = CalProjectItem.ItemScore;

                        data4.Rows.Add(row4);
                    }
                }
            }
            else
            {
                row4 = data4.NewRow();

                row4["ItemModuleName"] = " ";
                row4["ItemScore"] = " ";

                data4.Rows.Add(row4);
            }

            parameters.Add(new ReportParameter("CalificationProjectName", oModel.ProviderCalification.ProRelatedCalificationProject != null && oModel.ProviderCalification.ProRelatedCalificationProject.Count > 0 ? oModel.ProviderCalification.ProRelatedCalificationProject.FirstOrDefault().ProjectConfigModel.CalificationProjectConfigName : " "));
            parameters.Add(new ReportParameter("CalificationProjectTotalScore", oModel.ProviderCalification.ProRelatedCalificationProject != null && oModel.ProviderCalification.ProRelatedCalificationProject.Count > 0 ? oModel.ProviderCalification.ProRelatedCalificationProject.FirstOrDefault().TotalScore.ToString() : " "));
            parameters.Add(new ReportParameter("CalificationProjectLastModify", oModel.ProviderCalification.ProRelatedCalificationProject != null && oModel.ProviderCalification.ProRelatedCalificationProject.Count > 0 ? oModel.ProviderCalification.ProRelatedCalificationProject.FirstOrDefault().LastModify.ToString() : " "));
            parameters.Add(new ReportParameter("CalificationProjectCal", !string.IsNullOrEmpty(oModel.ProviderCalification.TotalCalification.ToString()) ? oModel.ProviderCalification.TotalCalification.ToString() : " "));
            
            #endregion

            #endregion Set Parameters

            string fileFormat = Request["ThirdKnowledge_cmbFormat"] != null ? Request["ThirdKnowledge_cmbFormat"].ToString() : "pdf";
            Tuple<byte[], string, string> GerencialReport = ProveedoresOnLine.Reports.Controller.ReportModule.CP_GerencialReport(
                                                            fileFormat,
                                                            data,
                                                            data2,
                                                            data3,
                                                            data4,
                                                            parameters,
                                                            Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "C_Report_GerencialInfo.rdlc");

            oReporModel.File = GerencialReport.Item1;
            oReporModel.MimeType = GerencialReport.Item2;
            oReporModel.FileName = GerencialReport.Item3;

            return oReporModel;
        }

        public virtual ActionResult RPGeneral(string SearchParam, string SearchFilter)
        {
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            ProviderSearchViewModel oModel = null;

            List<ProviderModel> oProviderResult;

            if (SessionModel.CurrentCompany != null &&
                !string.IsNullOrEmpty(SessionModel.CurrentCompany.CompanyPublicId))
            {
                //get basic search model
                oModel = new ProviderSearchViewModel()
                {
                    ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                    SearchParam = SearchParam,
                    SearchFilter = SearchFilter == null ? null : (SearchFilter.Trim(new char[] { ',' }).Length > 0 ? SearchFilter.Trim(new char[] { ',' }) : null),
                    SearchOrderType = MarketPlace.Models.General.enumSearchOrderType.Relevance,
                    OrderOrientation = false,
                    PageNumber = 0,
                    ProviderSearchResult = new List<ProviderLiteViewModel>(),
                };

                #region Providers

                //search providers
                int oTotalRowsAux;
                oProviderResult =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchNew
                    (SessionModel.CurrentCompany.CompanyPublicId,
                    SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    (int)oModel.SearchOrderType,
                    oModel.OrderOrientation,
                    oModel.PageNumber,
                    //oModel.RowCount,
                    65000,
                    out oTotalRowsAux);

                oModel.TotalRows = oTotalRowsAux;

                List<GenericFilterModel> oFilterModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchFilterNew
                    (SessionModel.CurrentCompany.CompanyPublicId,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false);

                if (oFilterModel != null)
                {
                    oModel.ProviderFilterResult = oFilterModel.Where(x => x.CustomerPublicId == SessionModel.CurrentCompany.CompanyPublicId).ToList();
                }

                //Branch Info

                //parse view model
                if (oProviderResult != null && oProviderResult.Count > 0)
                {
                    oProviderResult.All(prv =>
                    {
                        prv.RelatedCommercial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(prv.RelatedCompany.CompanyPublicId, (int)enumContactType.Brach);
                        return true;
                    });
                }

                #endregion Providers

                #region Crete Excel

                //Write the document
                StringBuilder data = new StringBuilder();
                string strSep = ";";

                oProviderResult.All(x =>
                {
                    string Address = string.Empty;
                    string Telephone = string.Empty;
                    string Representative = string.Empty;
                    string Country = string.Empty;
                    int CityId = 0;
                    string City = string.Empty;
                    string State = string.Empty;

                    if (x.RelatedCommercial != null)
                    {
                        x.RelatedCommercial.Where(y => y != null).All(y =>
                        {
                            bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                            z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                            if (isPrincipal)
                            {
                                Address = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Address).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                                Telephone = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Phone).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                                Representative = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Representative).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();

                                int oTotalRows = 0;

                                string sCityid = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).Select(z => z.Value).FirstOrDefault();
                                if (sCityid.Length > 0)
                                    CityId = Convert.ToInt32(sCityid);
                                else
                                    CityId = 0;

                                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oGeographyModel = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, CityId, 0, 10000, out oTotalRows);


                                Country = (oGeographyModel != null && oGeographyModel.FirstOrDefault().Country.ItemName.Length > 0 && oGeographyModel.FirstOrDefault().Country.ItemName != null) ? oGeographyModel.FirstOrDefault().Country.ItemName : "N/D";
                                City = (oGeographyModel != null && oGeographyModel.FirstOrDefault().City.ItemName.Length > 0 && oGeographyModel.FirstOrDefault().City.ItemName != null) ? oGeographyModel.FirstOrDefault().City.ItemName : "N/D";
                                State = (oGeographyModel != null && oGeographyModel.FirstOrDefault().State.ItemName.Length > 0 && oGeographyModel.FirstOrDefault().State.ItemName != null) ? oGeographyModel.FirstOrDefault().State.ItemName : "N/D";
                            }

                            return true;
                        });

                        if (oProviderResult.IndexOf(x) == 0)
                        {
                            data.AppendLine
                            ("\"" + "Tipo Identificacion" + "\"" + strSep +
                                "\"" + "Numero Identificacion" + "\"" + strSep +
                                "\"" + "Razon Social" + "\"" + strSep +
                                "\"" + "País" + "\"" + strSep +

                                "\"" + "Ciudad" + "\"" + strSep +
                                "\"" + "Estado" + "\"" + strSep +

                                "\"" + "Dirección" + "\"" + strSep +
                                "\"" + "Telefono" + "\"" + strSep +

                                "\"" + "Representante" + "\"");
                            data.AppendLine
                                ("\"" + x.RelatedCompany.IdentificationType.ItemName + "\"" + strSep +
                                "\"" + x.RelatedCompany.IdentificationNumber + "\"" + strSep +
                                "\"" + x.RelatedCompany.CompanyName + "\"" + "" + strSep +
                                "\"" + Country + "\"" + "" + strSep +
                                "\"" + City + "\"" + strSep +
                                "\"" + State + "\"" + "" + strSep +
                                "\"" + Address + "\"" + strSep +
                                "\"" + Telephone + "\"" + strSep +
                                "\"" + Representative + "\"");
                        }
                        else
                        {
                            data.AppendLine
                                ("\"" + x.RelatedCompany.IdentificationType.ItemName + "\"" + strSep +
                                "\"" + x.RelatedCompany.IdentificationNumber + "\"" + strSep +
                                "\"" + x.RelatedCompany.CompanyName + "\"" + "" + strSep +
                                "\"" + Country + "\"" + "" + strSep +
                                "\"" + City + "\"" + strSep +
                                "\"" + State + "\"" + "" + strSep +
                                "\"" + Address + "\"" + strSep +
                                "\"" + Telephone + "\"" + strSep +
                                "\"" + Representative + "\"");
                        }
                    }
                    else
                    {
                        if (oProviderResult.IndexOf(x) == 0)
                        {
                            data.AppendLine
                            ("\"" + "Tipo Identificacion" + "\"" + strSep +
                                "\"" + "Numero Identificacion" + "\"" + strSep +
                                "\"" + "Razon Social" + "\"" + strSep +
                                "\"" + "País" + "\"" + strSep +

                                "\"" + "Ciudad" + "\"" + strSep +
                                "\"" + "Estado" + "\"" + strSep +

                                "\"" + "Dirección" + "\"" + strSep +
                                "\"" + "Telefono" + "\"" + strSep +

                                "\"" + "Representante" + "\"");
                            data.AppendLine
                                ("\"" + x.RelatedCompany.IdentificationType.ItemName + "\"" + strSep +
                                "\"" + x.RelatedCompany.IdentificationNumber + "\"" + strSep +
                                "\"" + x.RelatedCompany.CompanyName + "\"" + "" + strSep +
                                "\"" + Country + "\"" + "" + strSep +
                                "\"" + City + "\"" + strSep +
                                "\"" + State + "\"" + "" + strSep +
                                "\"" + Address + "\"" + strSep +
                                "\"" + Telephone + "\"" + strSep +
                                "\"" + Representative + "\"");
                        }
                        else
                        {
                            data.AppendLine
                                ("\"" + x.RelatedCompany.IdentificationType.ItemName + "\"" + strSep +
                                "\"" + x.RelatedCompany.IdentificationNumber + "\"" + strSep +
                                "\"" + x.RelatedCompany.CompanyName + "\"" + "" + strSep +
                                "\"" + "ND" + "\"" + "" + strSep +
                                "\"" + "ND" + "\"" + strSep +
                                "\"" + "ND" + "\"" + "" + strSep +
                                "\"" + "ND" + "\"" + strSep +
                                "\"" + "ND" + "\"" + strSep +
                                "\"" + "ND" + "\"");
                        }
                    }
                    return true;
                });

                byte[] buffer = Encoding.Default.GetBytes(data.ToString().ToCharArray());

                #endregion Crete Excel

                return File(buffer, "application/csv", "Proveedores_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv");
            }

            return null;
        }

        public virtual ActionResult RPDIAN(string SearchParam, string SearchFilter)
        {
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            ProviderSearchViewModel oModel = null;

            List<ProviderModel> oProviderResult;

            if (SessionModel.CurrentCompany != null &&
                !string.IsNullOrEmpty(SessionModel.CurrentCompany.CompanyPublicId))
            {
                //get basic search model
                oModel = new ProviderSearchViewModel()
                {
                    ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                    SearchParam = SearchParam,
                    SearchFilter = SearchFilter == null ? null : (SearchFilter.Trim(new char[] { ',' }).Length > 0 ? SearchFilter.Trim(new char[] { ',' }) : null),
                    SearchOrderType = MarketPlace.Models.General.enumSearchOrderType.Relevance,
                    OrderOrientation = false,
                    PageNumber = 0,
                    ProviderSearchResult = new List<ProviderLiteViewModel>(),
                };

                #region Providers

                //search providers
                int oTotalRowsAux;
                oProviderResult =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchNew
                    (SessionModel.CurrentCompany.CompanyPublicId,
                    SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    (int)oModel.SearchOrderType,
                    oModel.OrderOrientation,
                    oModel.PageNumber,
                    //oModel.RowCount,
                    65000,
                    out oTotalRowsAux);

                oModel.TotalRows = oTotalRowsAux;

                List<GenericFilterModel> oFilterModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchFilterNew
                    (SessionModel.CurrentCompany.CompanyPublicId,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false);

                if (oFilterModel != null)
                {
                    oModel.ProviderFilterResult = oFilterModel.Where(x => x.CustomerPublicId == SessionModel.CurrentCompany.CompanyPublicId).ToList();
                }

                //Branch Info


                //parse view model
                if (oProviderResult != null && oProviderResult.Count > 0)
                {
                    oProviderResult.All(prv =>
                    {
                        ProviderModel response = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPGetBasicInfo(prv.RelatedCompany.CompanyPublicId);
                        prv.RelatedFinantial = response.RelatedFinantial;
                        prv.RelatedCommercial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(prv.RelatedCompany.CompanyPublicId, (int)enumContactType.Brach);
                        prv.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(prv.RelatedCompany.CompanyPublicId, (int)enumLegalType.RUT);

                        return true;
                    });
                }

                #endregion Providers

                #region Build Excel

                //Write the document
                StringBuilder data = new StringBuilder();
                string strSep = ";";
                string strProvidersName = "\"" + "PROVEEDOR" + "\"";
                string Address = string.Empty;
                string Telephone = string.Empty;
                string Mail = string.Empty;
                string Representative = string.Empty;
                string Country = string.Empty;
                int CityId = 0;
                string City = string.Empty;
                string State = string.Empty;
                string AERut = string.Empty;
                string Income = string.Empty;
                string Utility = string.Empty;
                string Etibda = string.Empty;

                oProviderResult.All(x =>
                {
                    if (!string.IsNullOrEmpty(x.RelatedCompany.CompanyName))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + x.RelatedCompany.CompanyName + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }

                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "TIPO DE IDENTIFICACIÓN" + "\"";
                oProviderResult.All(x =>
                {
                    if (!string.IsNullOrEmpty(x.RelatedCompany.IdentificationType.ItemName))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + x.RelatedCompany.IdentificationType.ItemName + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }
                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "NÚMERO DE IDENTIFICACIÓN" + "\"";
                oProviderResult.All(x =>
                {
                    if (!string.IsNullOrEmpty(x.RelatedCompany.IdentificationNumber))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + x.RelatedCompany.IdentificationNumber + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }
                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "PAÍS" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedCommercial != null)
                    {
                        x.RelatedCommercial.Where(y => y != null).All(y =>
                        {
                            bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                            z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                            if (isPrincipal)
                            {
                                int oTotalRows = 0;
                                CityId = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).Select(z => Convert.ToInt32(z.Value)).DefaultIfEmpty(0).FirstOrDefault();
                                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oGeographyModel = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, CityId, 0, 10000, out oTotalRows);

                                Country = oGeographyModel.FirstOrDefault().Country.ItemName;
                            }
                            return true;
                        });
                    }
                    if (!string.IsNullOrEmpty(Country))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + Country + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }

                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "CIUDAD" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedCommercial != null)
                    {
                        x.RelatedCommercial.Where(y => y != null).All(y =>
                        {
                            bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                            z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                            if (isPrincipal)
                            {
                                int oTotalRows = 0;
                                CityId = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).Select(z => Convert.ToInt32(z.Value)).DefaultIfEmpty(0).FirstOrDefault();
                                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oGeographyModel = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, CityId, 0, 10000, out oTotalRows);

                                City = oGeographyModel.FirstOrDefault().City.ItemName;
                            }
                            return true;
                        });
                    }
                    if (!string.IsNullOrEmpty(City))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + City + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }

                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "DEPARTAMENTO" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedCommercial != null)
                    {
                        x.RelatedCommercial.Where(y => y != null).All(y =>
                        {
                            bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                            z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                            if (isPrincipal)
                            {
                                int oTotalRows = 0;
                                CityId = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).Select(z => Convert.ToInt32(z.Value)).DefaultIfEmpty(0).FirstOrDefault();
                                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oGeographyModel = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, CityId, 0, 10000, out oTotalRows);

                                State = oGeographyModel.FirstOrDefault().State.ItemName;
                            }
                            return true;
                        });
                    }
                    if (!string.IsNullOrEmpty(State))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + State + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }

                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "REPRESENTANTE LEGAL" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedCommercial != null)
                    {
                        x.RelatedCommercial.Where(y => y != null).All(y =>
                        {
                            bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                            z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                            if (isPrincipal)
                            {
                                Representative = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Representative).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                            }
                            return true;
                        });
                    }
                    if (!string.IsNullOrEmpty(Representative))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + Representative + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }

                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "TELÉFONO" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedCommercial != null)
                    {
                        x.RelatedCommercial.Where(y => y != null).All(y =>
                        {
                            bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                            z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                            if (isPrincipal)
                            {
                                Telephone = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Phone).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                            }
                            return true;
                        });
                    }
                    if (!string.IsNullOrEmpty(Telephone))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + Telephone + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }

                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "CORREO" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedCommercial != null)
                    {
                        x.RelatedCommercial.Where(y => y != null).All(y =>
                        {
                            bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                            z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                            if (isPrincipal)
                            {
                                Mail = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Email).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                            }
                            return true;
                        });
                    }
                    if (!string.IsNullOrEmpty(Mail))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + Mail + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }

                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "DIRECCIÓN" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedCommercial != null)
                    {
                        x.RelatedCommercial.Where(y => y != null).All(y =>
                        {
                            bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                            z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                            if (isPrincipal)
                            {
                                Address = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Address).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                            }
                            return true;
                        });
                    }
                    if (!string.IsNullOrEmpty(Address))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + Address + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }

                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "ACTIVIDAD ECONOMICA" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedLegal != null)
                    {
                        List<ProviderLegalViewModel> RelatedLegalInfo = new List<ProviderLegalViewModel>();

                        x.RelatedLegal.All(z =>
                        {
                            RelatedLegalInfo.Add(new ProviderLegalViewModel(z));
                            return true;
                        });
                        AERut = RelatedLegalInfo.Select(r => r.R_ICAName).DefaultIfEmpty(string.Empty).FirstOrDefault();

                    }
                    if (!string.IsNullOrEmpty(AERut))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + AERut + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }

                    return true;
                });
                data.AppendLine(strProvidersName);


                strProvidersName = "\"" + "INGRESOS" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedFinantial != null)
                    {
                        List<ProviderFinancialBasicInfoViewModel> RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                        decimal oExchange;
                        oExchange = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(
                                    Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemInfo.FirstOrDefault().ValueName),
                                    Convert.ToInt32(Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_CurrencyExchange_COP].Value),
                                    Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemName));

                        x.RelatedFinantial.All(z =>
                        {
                            RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(z, oExchange));
                            return true;
                        });
                        Income = RelatedFinancialBasicInfo.Select(r => r.BI_OperationIncome).DefaultIfEmpty(string.Empty).FirstOrDefault();

                    }
                    if (!string.IsNullOrEmpty(Income))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + Income + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }

                    return true;
                });
                data.AppendLine(strProvidersName);


                strProvidersName = "\"" + "UTILIDAD NETA" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedFinantial != null)
                    {

                        List<ProviderFinancialBasicInfoViewModel> RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                        decimal oExchange;
                        oExchange = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(
                                    Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemInfo.FirstOrDefault().ValueName),
                                    Convert.ToInt32(Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_CurrencyExchange_COP].Value),
                                    Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemName));

                        x.RelatedFinantial.All(z =>
                        {
                            RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(z, oExchange));
                            return true;
                        });
                        Utility = RelatedFinancialBasicInfo.Select(r => r.BI_IncomeBeforeTaxes).DefaultIfEmpty(string.Empty).FirstOrDefault();

                    }
                    if (!string.IsNullOrEmpty(Utility))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + Utility + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }
                    return true;
                });
                data.AppendLine(strProvidersName);

                strProvidersName = "\"" + "EBITDA" + "\"";
                oProviderResult.All(x =>
                {
                    if (x.RelatedFinantial != null)
                    {

                        List<ProviderFinancialBasicInfoViewModel> RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                        decimal oExchange;
                        oExchange = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(
                                    Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemInfo.FirstOrDefault().ValueName),
                                    Convert.ToInt32(Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_CurrencyExchange_COP].Value),
                                    Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemName));

                        x.RelatedFinantial.All(z =>
                        {
                            RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(z, oExchange));
                            return true;
                        });
                        Etibda = RelatedFinancialBasicInfo.Select(r => r.BI_EBITDA).DefaultIfEmpty(string.Empty).FirstOrDefault();

                    }
                    if (!string.IsNullOrEmpty(Etibda))
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + Etibda + "\"";
                    }
                    else
                    {
                        strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                    }
                    return true;
                });
                data.AppendLine(strProvidersName);

                byte[] buffer = Encoding.Default.GetBytes(data.ToString().ToCharArray());

                #endregion Crete Excel

                return File(buffer, "application/csv", "Proveedores_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv");
            }
            return null;
        }

        public GenericReportModel RPFinancial(ProviderViewModel oModel)
        {
            string oViewBalance = "_P_FI_Balance";
            string oViewIndicators = "_P_FI_Indicators";


            List<ReportParameter> parameters = new List<ReportParameter>();
            GenericReportModel oReporModel = new GenericReportModel();

            #region Set Parameters
            //CustomerInfo
            parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName));
            parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber));
            parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo));

            //ProviderInfo
            parameters.Add(new ReportParameter("ProviderName", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName));
            parameters.Add(new ReportParameter("ProviderIdentificationType", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("ProviderIdentificationNumber", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber));

            #region Basic Info

            if (!string.IsNullOrEmpty(oModel.RelatedGeneralInfo.Where(x => x.PC_RepresentantType == "Legal").Select(x => x.PC_ContactName).FirstOrDefault()))
                parameters.Add(new ReportParameter("Representant", oModel.RelatedGeneralInfo.Where(x => x.PC_RepresentantType == "Legal").Select(x => x.PC_ContactName).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("Representant", "NA"));

            if (oModel.RelatedLegalInfo.Count > 0 && !string.IsNullOrEmpty(oModel.RelatedLegalInfo.FirstOrDefault().CP_InscriptionNumber)
                && !string.IsNullOrWhiteSpace(oModel.RelatedLegalInfo.FirstOrDefault().CP_InscriptionNumber))
                parameters.Add(new ReportParameter("InscriptionNumber", oModel.RelatedLegalInfo.FirstOrDefault().CP_InscriptionNumber));
            else
                parameters.Add(new ReportParameter("InscriptionNumber", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Address).FirstOrDefault()))
                parameters.Add(new ReportParameter("Address", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Address).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("Address", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_City).FirstOrDefault()))
                parameters.Add(new ReportParameter("City", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_City).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("City", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Phone).FirstOrDefault()))
                parameters.Add(new ReportParameter("Phone", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Phone).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("Phone", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Fax).FirstOrDefault()))
                parameters.Add(new ReportParameter("Fax", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Fax).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("Fax", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Website).FirstOrDefault()))
                parameters.Add(new ReportParameter("WebSite", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Website).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("WebSite", "NA"));

            if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Email).FirstOrDefault()))
                parameters.Add(new ReportParameter("Email", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Email).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("Email", "NA"));

            if (oModel.RelatedLegalInfo.Count > 0 && !string.IsNullOrWhiteSpace(oModel.RelatedLegalInfo.FirstOrDefault().CP_SocialObject))
                parameters.Add(new ReportParameter("SocialObject", oModel.RelatedLegalInfo.FirstOrDefault().CP_SocialObject));
            else
                parameters.Add(new ReportParameter("SocialObject", "NA"));

            #endregion Basic Info

            #region Finacial Info
            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrEmpty(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_Year != null).Select(x => x.BI_Year).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("BalanceYear", "AÑO " + oModel.RelatedFinancialBasicInfo.Where(x => x.BI_Year != null).Select(x => x.BI_Year).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("BalanceYear", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrEmpty(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalActive != null).Select(x => x.BI_TotalActive).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("TotalActive", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalActive != null).Select(x => x.BI_TotalActive).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("TotalActive", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPassive != null).Select(x => x.BI_TotalPassive).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("TotalPasive", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPassive != null).Select(x => x.BI_TotalPassive).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("TotalPasive", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPatrimony != null).Select(x => x.BI_TotalPatrimony).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("TotalPatrimony", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPatrimony != null).Select(x => x.BI_TotalPatrimony).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("TotalPatrimony", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_OperationIncome != null).Select(x => x.BI_OperationIncome).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("OperationIncome", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_OperationIncome != null).Select(x => x.BI_OperationIncome).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("OperationIncome", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_IncomeBeforeTaxes != null).Select(x => x.BI_IncomeBeforeTaxes).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("IncomeBeforeTaxes", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_IncomeBeforeTaxes != null).Select(x => x.BI_IncomeBeforeTaxes).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("IncomeBeforeTaxes", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrEmpty(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_JobCapital != null).Select(x => x.BI_JobCapital).DefaultIfEmpty("NA").FirstOrDefault()))
                parameters.Add(new ReportParameter("JobCapital", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_JobCapital != null).Select(x => x.BI_JobCapital).DefaultIfEmpty("NA").FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("JobCapital", "N/A"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_ExerciseUtility != null).Select(x => x.BI_ExerciseUtility).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("ExcerciseUtility", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_ExerciseUtility != null).Select(x => x.BI_ExerciseUtility).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("ExcerciseUtility", "NA"));

            if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrEmpty(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_EBITDA != null).Select(x => x.BI_EBITDA).DefaultIfEmpty("").FirstOrDefault()))
                parameters.Add(new ReportParameter("EBITDA", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_EBITDA != null).Select(x => x.BI_EBITDA).FirstOrDefault()));
            else
                parameters.Add(new ReportParameter("EBITDA", "NA"));

            parameters.Add(new ReportParameter("Altman", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_Altman != null).Select(x => x.BI_Altman).DefaultIfEmpty("NA").FirstOrDefault()));

            #endregion Finacial Info

            #region HSEQ Info

            if (oModel.RelatedHSEQlInfo != null && oModel.RelatedHSEQlInfo.Count > 0)
            {
                parameters.Add(new ReportParameter("SystemOccupationalHazards", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards));
                parameters.Add(new ReportParameter("RateARL", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL));
                parameters.Add(new ReportParameter("LTIFResult", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || string.IsNullOrEmpty(oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult) ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult));
            }
            else
            {
                parameters.Add(new ReportParameter("SystemOccupationalHazards", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards));
                parameters.Add(new ReportParameter("RateARL", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL));
                parameters.Add(new ReportParameter("LTIFResult", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult));
            }

            #endregion HSEQ Info

            #region Conocimiento de Terceros

            if (string.IsNullOrEmpty(oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == 203012).Select(x => x.Value).DefaultIfEmpty(string.Empty).FirstOrDefault()))
            {
                parameters.Add(new ReportParameter("LastUpdate", "2015-01-01 12:00:00"));
            }
            else
            {
                parameters.Add(new ReportParameter("LastUpdate", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == 203012).Select(x => x.Value).DefaultIfEmpty(string.Empty).FirstOrDefault()));
            }

            if (oModel.RelatedLiteProvider.ProviderAlertRisk == MarketPlace.Models.General.enumBlackListStatus.DontShowAlert)
            {
                parameters.Add(new ReportParameter("Alert", "No se encontraron coincidencias en listas restrictivas."));
            }
            else
            {
                parameters.Add(new ReportParameter("Alert", "Se encontraron coincidencias en listas restrictivas."));
            }

            oModel.RelatedBlackListInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListGetBasicInfo(oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId);

            DataTable data5 = new DataTable();
            data5.Columns.Add("PersonName");
            data5.Columns.Add("PersonCargo");
            data5.Columns.Add("PersonLista");
            data5.Columns.Add("PersonDelito");
            data5.Columns.Add("PersonState");

            DataRow row5;
            if (oModel.RelatedBlackListInfo != null)
            {
                foreach (var item in oModel.RelatedBlackListInfo.Where(x => x != null))
                {
                    row5 = data5.NewRow();
                    row5["PersonName"] = "N/A";
                    row5["PersonCargo"] = "N/A";
                    row5["PersonLista"] = "N/A";
                    row5["PersonDelito"] = "N/A";
                    row5["PersonState"] = "N/A";
                    foreach (var info in item.BlackListInfo)
                    {
                        if (info.ItemInfoType.ItemName == "RazonSocial")
                        {
                            row5["PersonName"] = info.Value;
                        }
                        else if (info.ItemInfoType.ItemName == "Cargo")
                        {
                            row5["PersonCargo"] = info.Value;
                        }
                        else if (info.ItemInfoType.ItemName == "Lista")
                        {
                            row5["PersonLista"] = info.Value;
                        }
                        else if (info.ItemInfoType.ItemName == "Delito, cargo o resultado de la consulta")
                        {
                            row5["PersonDelito"] = info.Value;
                        }
                        else if (info.ItemInfoType.ItemName == "Estado")
                        {
                            row5["PersonState"] = info.Value;
                        }
                    }
                    data5.Rows.Add(row5);
                }
            }

            #endregion Conocimiento de Terceros

            #region Personas de Contacto

            oModel.ContactCompanyInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId, (int)enumContactType.PersonContact);
            oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

            if (oModel.ContactCompanyInfo != null)
            {
                oModel.ContactCompanyInfo.All(x =>
                {
                    oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                    return true;
                });
            }

            DataTable data4 = new DataTable();
            data4.Columns.Add("ContactType");
            data4.Columns.Add("ContactName");
            data4.Columns.Add("ContactPhone");
            data4.Columns.Add("ContactMail");

            DataRow row4;
            foreach (var item in oModel.RelatedGeneralInfo.Where(x => x != null))
            {
                row4 = data4.NewRow();

                row4["ContactType"] = item.PC_RepresentantType;
                row4["ContactName"] = item.PC_ContactName;
                row4["ContactPhone"] = item.PC_TelephoneNumber;
                row4["ContactMail"] = item.PC_Email;

                data4.Rows.Add(row4);
            }

            #endregion Personas de Contacto

            #region ComercialExperience

            oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCommercialGetBasicInfo
                (oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                (int)enumCommercialType.Experience,
                SessionModel.CurrentCompany.CompanyPublicId);

            if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial != null)
            {
                parameters.Add(new ReportParameter("TotalExperiences", oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial.Count.ToString()));
            }
            else
            {
                parameters.Add(new ReportParameter("TotalExperiences", "0"));
            }

            #endregion ComercialExperience

            #region Kcontratación

            DataTable data3 = new DataTable();
            data3.Columns.Add("EvaluationCriteria");
            data3.Columns.Add("Provider");
            data3.Columns.Add("Consultant");
            data3.Columns.Add("Builder");

            DataRow row3;
            foreach (var item in oModel.RelatedKContractInfo.Where(x => x != null))
            {
                row3 = data3.NewRow();

                row3["EvaluationCriteria"] = item.FK_RoleType;
                row3["Provider"] = Convert.ToDouble(item.FK_TotalScore).ToString("#,0.##"); ;
                row3["Consultant"] = Convert.ToDouble(item.FK_TotalOrgCapacityScore).ToString("#,0.##"); ;
                row3["Builder"] = Convert.ToDouble(item.FK_TotalKValueScore).ToString("#,0.##"); ;

                data3.Rows.Add(row3);
            }

            #endregion Kcontratación

            //get request info
            int oYear = 0;
            if (oModel.RelatedCertificationBasicInfo != null && oModel.RelatedCertificationBasicInfo.Count > 0)
                oYear = Int32.Parse(oModel.RelatedFinancialInfo.Where(x => x.SH_Year != null).Select(x => x.SH_Year).DefaultIfEmpty(null).FirstOrDefault());

            int oCurrencyValidate = 0;

            int oCurrencyType = !string.IsNullOrEmpty(Request["Currency"]) && int.TryParse(Request["Currency"].ToString(), out oCurrencyValidate) == true ?
                Convert.ToInt32(Request["Currency"].Replace(" ", "")) :
                Convert.ToInt32(Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_CurrencyExchange_COP].Value);

            //get balance info
            List<BalanceSheetModel> oBalanceAux =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPBalanceSheetGetByYear
                (oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                null,
                oCurrencyType);

            //fill view models
            oModel.RelatedFinancialInfo = new List<ProviderFinancialViewModel>();
            if (oBalanceAux != null && oBalanceAux.Count > 0)
            {
                oBalanceAux.All(bs =>
                {
                    oModel.RelatedFinancialInfo.Add(new ProviderFinancialViewModel(bs));
                    return true;
                });
            }

            oModel.RelatedBalanceSheetInfo = new List<ProviderBalanceSheetViewModel>();
            if (oBalanceAux != null && oBalanceAux.Count > 0)
            {
                List<BalanceSheetModel> oBalancetemp = new List<BalanceSheetModel>();

                foreach (var item in oBalanceAux)
                {
                    if (item.BalanceSheetInfo.Count == 0)
                    {
                        oBalancetemp.Add(item);
                    }
                }

                int cont = 0;
                foreach (var item in oBalancetemp)
                {
                    if (cont < 1)
                    {
                        oBalanceAux.Remove(item);
                        cont++;
                    }
                }

                oModel.RelatedBalanceSheetInfo = GetBalanceSheetViewModel
                    (null,
                    oBalanceAux,
                    oViewBalance);

            }

            string lastYear = "N/A";
            string oldYear = "N/A";

            var oBalanceToShow = oModel.RelatedFinancialInfo.Where(x => x.SH_HasValues).OrderByDescending(x => x.SH_Year).ToList();
            if (oBalanceToShow != null && oBalanceToShow.Count > 0)
            {
                lastYear = oBalanceToShow[0].SH_Year;

                if (oBalanceToShow.Count <= 1)
                {
                    var oldYearNum = Convert.ToInt32(lastYear) - 1;
                    oldYear = oldYearNum.ToString();
                }
                else
                {
                    oldYear = oBalanceToShow[1].SH_Year;
                }
            }

            #region Balance Info

            DataTable data = new DataTable();
            data.Columns.Add("Cuenta");
            data.Columns.Add("Valor1");
            data.Columns.Add("AV1");
            data.Columns.Add("Valor2");
            data.Columns.Add("AV2");
            data.Columns.Add("AH");

            oModel.RelatedBalanceSheetInfo.All(x =>
            {
                data = GetAllValues_Balance(data, x);

                return true;
            });

            #endregion

            #region Indicators

            DataTable data2 = new DataTable();
            data2.Columns.Add("Concepto");
            data2.Columns.Add("Valor1");
            data2.Columns.Add("Valor2");
            data2.Columns.Add("Unidad");
            data2.Columns.Add("Formula");
            data2.Columns.Add("Interpretación");

            if (oBalanceAux != null && oBalanceAux.Count > 0)
            {
                List<BalanceSheetModel> oBalancetemp = new List<BalanceSheetModel>();

                foreach (var item in oBalanceAux)
                {
                    if (item.BalanceSheetInfo.Count == 0)
                    {
                        oBalancetemp.Add(item);
                    }
                }

                int cont = 0;
                foreach (var item in oBalancetemp)
                {
                    if (cont < 1)
                    {
                        oBalanceAux.Remove(item);
                        cont++;
                    }
                }

                oModel.RelatedBalanceSheetInfo = GetBalanceSheetViewModel
                    (null,
                    oBalanceAux,
                    oViewIndicators);

            }

            oModel.RelatedBalanceSheetInfo.All(x =>
            {
                data2 = GetAllValues_Indicators(data2, x);

                return true;
            });

            #endregion



            parameters.Add(new ReportParameter("BalanceLastYear", lastYear));
            parameters.Add(new ReportParameter("BalanceOldYear", oldYear));

            #endregion

            Tuple<byte[], string, string> GerencialReport = ProveedoresOnLine.Reports.Controller.ReportModule.F_FinancialReport(
                                                enumCategoryInfoType.PDF.ToString(),
                                                data,
                                                data2,
                                                data3,
                                                data4,
                                                data5,
                                                parameters,
                                                Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "F_FinancialReport.rdlc");

            oReporModel.File = GerencialReport.Item1;
            oReporModel.MimeType = GerencialReport.Item2;
            oReporModel.FileName = GerencialReport.Item3;

            return oReporModel;
        }

        #endregion Reports

        #region CustomData

        public virtual ActionResult CDCustomData(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId).FirstOrDefault();

            oModel.CustomData = IntegrationPlatform.Controller.IntegrationPlatform.MP_CustomerProvider_GetCustomData(SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            //validate provider permisions
            if (oProvider != null)
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.ProviderMenu = GetProviderMenu(oModel);
                oModel.ProviderOptions = oModel.ProviderOptions = IntegrationPlatform.Controller.IntegrationPlatform.CatalogGetSanofiOptions();
            }

            return View(oModel);
        }

        #endregion

        #region Pivate Functions

        private GenericReportModel Report_SurveyGeneral(ProviderViewModel oModel)
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            GenericReportModel oReporModel = new GenericReportModel();

            //CustomerInfo
            parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName));
            parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber));
            parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo));
            //ProviderInfo
            parameters.Add(new ReportParameter("ProviderName", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName));
            parameters.Add(new ReportParameter("ProviderIdentificationType", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("ProviderIdentificationNumber", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber));

            //SurveyInfo
            parameters.Add(new ReportParameter("SurveyConfigName", oModel.RelatedSurvey.SurveyConfigName));
            parameters.Add(new ReportParameter("SurveyRating", oModel.RelatedSurvey.SurveyRating.ToString()));
            parameters.Add(new ReportParameter("SurveyStatusName", oModel.RelatedSurvey.SurveyStatusName));
            parameters.Add(new ReportParameter("SurveyIssueDate", oModel.RelatedSurvey.SurveyIssueDate));
            parameters.Add(new ReportParameter("SurveyLastModify", oModel.RelatedSurvey.SurveyLastModify));
            parameters.Add(new ReportParameter("SurveyResponsible", oModel.RelatedSurvey.SurveyResponsible));
            parameters.Add(new ReportParameter("SurveyAverage", oModel.RelatedSurvey.Average.ToString()));

            // DataSet Evaluators table
            DataTable data = new DataTable();
            data.Columns.Add("SurveyEvaluatorDetail");
            data.Columns.Add("SurveyStatusNameDetail");
            data.Columns.Add("SurveyRatingDetail");
            data.Columns.Add("SurveyProgressDetail");

            List<Models.Survey.SurveyViewModel> EvaluatorDetailList = new List<Models.Survey.SurveyViewModel>();

            foreach (var Evaluator in oModel.RelatedSurvey.SurveyEvaluatorList.Distinct())
            {
                Models.Survey.SurveyViewModel SurveyEvaluatorDetail = new Models.Survey.SurveyViewModel
                        (ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetByUser(oModel.RelatedSurvey.SurveyPublicId, Evaluator));

                EvaluatorDetailList.Add(SurveyEvaluatorDetail);

                DataRow row;
                row = data.NewRow();
                row["SurveyEvaluatorDetail"] = SurveyEvaluatorDetail.SurveyEvaluator;
                row["SurveyStatusNameDetail"] = SurveyEvaluatorDetail.SurveyStatusName;
                row["SurveyRatingDetail"] = SurveyEvaluatorDetail.SurveyRating;
                row["SurveyProgressDetail"] = SurveyEvaluatorDetail.SurveyProgress.ToString() + "%";
                data.Rows.Add(row);
            }

            // DataSet Area's Table
            DataTable data2 = new DataTable();
            data2.Columns.Add("SurveyAreaName");
            data2.Columns.Add("SurveyAreaRating");
            data2.Columns.Add("SurveyAreaWeight");

            foreach (var EvaluationArea in
                        oModel.RelatedSurvey.GetSurveyConfigItem(MarketPlace.Models.General.enumSurveyConfigItemType.EvaluationArea, null))
            {
                int RatingforArea = 0;

                foreach (Models.Survey.SurveyViewModel SurveyDetailInfo in EvaluatorDetailList)
                {
                    var EvaluationAreaInf = SurveyDetailInfo.GetSurveyItem(EvaluationArea.SurveyConfigItemId);

                    if (EvaluationAreaInf != null)
                    {
                        RatingforArea = RatingforArea + (int)EvaluationAreaInf.Ratting;
                    }
                }

                DataRow row;
                row = data2.NewRow();
                row["SurveyAreaName"] = EvaluationArea.Name;
                row["SurveyAreaRating"] = RatingforArea;
                row["SurveyAreaWeight"] = EvaluationArea.Weight.ToString() + "%";
                data2.Rows.Add(row);
            }

            Tuple<byte[], string, string> SurveyGeneralReport = ProveedoresOnLine.Reports.Controller.ReportModule.SV_GeneralReport(
                                                            data,
                                                            data2,
                                                            parameters,
                                                            enumCategoryInfoType.PDF.ToString(),
                                                            Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim());

            oReporModel.File = SurveyGeneralReport.Item1;
            oReporModel.MimeType = SurveyGeneralReport.Item2;
            oReporModel.FileName = SurveyGeneralReport.Item3;
            return oReporModel;
        }

        private GenericReportModel Report_SurveyEvaluatorDetail(ProviderViewModel oModel)
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            GenericReportModel oReporModel = new GenericReportModel();

            //CustomerInfo
            parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName));
            parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber));
            parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo));
            //ProviderInfo
            parameters.Add(new ReportParameter("ProviderName", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName));
            parameters.Add(new ReportParameter("ProviderIdentificationType", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("ProviderIdentificationNumber", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber));

            //SurveyInfo
            parameters.Add(new ReportParameter("SurveyConfigName", oModel.RelatedSurvey.SurveyConfigName));
            parameters.Add(new ReportParameter("SurveyRating", oModel.RelatedSurvey.SurveyRating.ToString()));
            parameters.Add(new ReportParameter("SurveyStatusName", oModel.RelatedSurvey.SurveyStatusName));
            parameters.Add(new ReportParameter("SurveyIssueDate", oModel.RelatedSurvey.SurveyIssueDate));
            parameters.Add(new ReportParameter("SurveyEvaluator", oModel.RelatedSurvey.SurveyEvaluator));
            parameters.Add(new ReportParameter("SurveyLastModify", oModel.RelatedSurvey.SurveyLastModify));
            parameters.Add(new ReportParameter("SurveyResponsible", oModel.RelatedSurvey.SurveyResponsible));
            parameters.Add(new ReportParameter("SurveyAverage", oModel.RelatedSurvey.Average.ToString()));

            if (oModel.RelatedSurvey.SurveyRelatedProject == null)
            {
                parameters.Add(new ReportParameter("SurveyRelatedProject", "NA"));
            }
            else
            {
                parameters.Add(new ReportParameter("SurveyRelatedProject", oModel.RelatedSurvey.SurveyRelatedProject));
            }

            DataTable data = new DataTable();
            data.Columns.Add("Area");
            data.Columns.Add("Question");
            data.Columns.Add("Answer");
            data.Columns.Add("QuestionRating");
            data.Columns.Add("QuestionWeight");
            data.Columns.Add("QuestionDescription");

            DataRow row;
            foreach (var EvaluationArea in
                        oModel.RelatedSurvey.GetSurveyConfigItem(MarketPlace.Models.General.enumSurveyConfigItemType.EvaluationArea, null))
            {
                var lstQuestion = oModel.RelatedSurvey.GetSurveyConfigItem
                    (MarketPlace.Models.General.enumSurveyConfigItemType.Question, EvaluationArea.SurveyConfigItemId);

                foreach (var Question in lstQuestion)
                {
                    if (Question.QuestionType != "118002")
                    {
                        row = data.NewRow();
                        row["Area"] = EvaluationArea.Name;
                        row["Question"] = Question.Order + " " + Question.Name;

                        var QuestionInfo = oModel.RelatedSurvey.GetSurveyItem(Question.SurveyConfigItemId);
                        var lstAnswer = oModel.RelatedSurvey.GetSurveyConfigItem
                            (MarketPlace.Models.General.enumSurveyConfigItemType.Answer, Question.SurveyConfigItemId);

                        foreach (var Answer in lstAnswer)
                        {
                            if (QuestionInfo != null && QuestionInfo.Answer == Answer.SurveyConfigItemId)
                            {
                                row["Answer"] = Answer.Name;
                            }
                        }

                        if (string.IsNullOrEmpty(row["Answer"].ToString()))
                        {
                            row["Answer"] = "Sin Responder";
                            row["QuestionRating"] = "NA";
                        }
                        else
                        {
                            row["QuestionRating"] = QuestionInfo.Ratting;
                        }

                        row["QuestionWeight"] = Question.Weight;

                        if (QuestionInfo != null && QuestionInfo.DescriptionText != null)
                        {
                            row["QuestionDescription"] = QuestionInfo.DescriptionText;
                        }
                        else
                        {
                            row["QuestionDescription"] = "";
                        }
                        data.Rows.Add(row);
                    }
                }
            }

            Tuple<byte[], string, string> EvaluatorDetailReport = ProveedoresOnLine.Reports.Controller.ReportModule.SV_EvaluatorDetailReport(
                                                                enumCategoryInfoType.PDF.ToString(),
                                                                data,
                                                                parameters,
                                                                Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "SV_Report_EvaluatorDetail.rdlc");

            oReporModel.File = EvaluatorDetailReport.Item1;
            oReporModel.MimeType = EvaluatorDetailReport.Item2;
            oReporModel.FileName = EvaluatorDetailReport.Item3;

            return oReporModel;
        }

        private SurveyModel GetSurveyUpsertRequest()
        {
            List<Tuple<string, int, int, int>> EvaluatorsRoleObj = new List<Tuple<string, int, int, int>>();
            List<string> EvaluatorsEmail = new List<string>();

            #region Parent Survey

            //Armar el Survey Model Del Papá
            SurveyModel oReturn = new SurveyModel()
            {
                ChildSurvey = new List<SurveyModel>(),
                SurveyPublicId = System.Web.HttpContext.Current.Request["SurveyPublicId"],
                RelatedProvider = new ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = System.Web.HttpContext.Current.Request["ProviderPublicId"],
                    }
                },
                RelatedSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel()
                {
                    ItemId = Convert.ToInt32(System.Web.HttpContext.Current.Request["SurveyConfigId"].Trim()),
                },
                Enable = true,
                User = SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().User, //Responsable
                SurveyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
            };

            System.Web.HttpContext.Current.Request.Form.AllKeys.Where(x => x.Contains("SurveyInfo_")).All(req =>
            {
                string[] strSplit = req.Split('_');

                //Set Parent Survey Info
                if (strSplit.Length >= 3)
                {
                    oReturn.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = !string.IsNullOrEmpty(strSplit[2]) ? Convert.ToInt32(strSplit[2].Trim()) : 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = Convert.ToInt32(strSplit[1].Trim())
                        },
                        Value = System.Web.HttpContext.Current.Request[req],
                        Enable = true,
                    });

                    //Get Evaluator Rol info
                    if (Convert.ToInt32(strSplit[1].Trim()) == (int)enumSurveyInfoType.Evaluator)
                        EvaluatorsRoleObj.Add(new Tuple<string, int, int, int>(System.Web.HttpContext.Current.Request[req],
                                                                                Convert.ToInt32(strSplit[4].Trim()),
                                                                                Convert.ToInt32(strSplit[2].Trim()),
                                                                                Convert.ToInt32(strSplit[5].Trim())));
                }
                return true;
            });

            #endregion Parent Survey

            if (EvaluatorsRoleObj != null && EvaluatorsRoleObj.Count > 0)
            {
                EvaluatorsEmail = new List<string>();
                EvaluatorsEmail = EvaluatorsRoleObj.GroupBy(x => x.Item1).Select(grp => grp.First().Item1).ToList();

                #region Child Survey

                //Set survey by evaluators
                EvaluatorsEmail.All(x =>
                {
                    oReturn.ChildSurvey.Add(new SurveyModel()
                    {
                        SurveyPublicId = System.Web.HttpContext.Current.Request["SurveyPublicId"],
                        RelatedProvider = new ProviderModel()
                        {
                            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = System.Web.HttpContext.Current.Request["ProviderPublicId"],
                            }
                        },
                        RelatedSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel()
                        {
                            ItemId = Convert.ToInt32(System.Web.HttpContext.Current.Request["SurveyConfigId"].Trim()),
                        },
                        Enable = true,
                        User = x,//Evaluator,
                        SurveyInfo = new List<GenericItemInfoModel>()
                    });
                    return true;
                });

                //Set SurveyChild Info
                oReturn.ChildSurvey.All(it =>
                {
                    List<Tuple<int, int, int>> AreaIdList = new List<Tuple<int, int, int>>();
                    AreaIdList.AddRange(EvaluatorsRoleObj.Where(y => y.Item1 == it.User).Select(y => new Tuple<int, int, int>(y.Item2, y.Item3, y.Item4)).ToList());
                    AreaIdList = AreaIdList.GroupBy(x => x.Item1).Select(grp => grp.First()).ToList();
                    if (AreaIdList != null)
                    {
                        AreaIdList.All(a =>
                        {
                            it.SurveyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = a.Item2 != null ? a.Item2 : 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyInfoType.CurrentArea
                                },
                                Value = a.Item1.ToString() + "_" + a.Item3.ToString(),
                                Enable = true,
                            });
                            return true;
                        });

                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Project
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Project).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });

                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.StartDate
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.StartDate).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });

                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.EndDate
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.EndDate).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });

                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.IssueDate
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.IssueDate).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Reminder
                            },
                            Value = "false",
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Contract
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Contract).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Status
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Comments
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Comments).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Responsible
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Responsible).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.ExpirationDate
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.ExpirationDate).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        List<GenericItemInfoModel> oEvaluators = new List<GenericItemInfoModel>();
                        oEvaluators = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Evaluator).Select(x => x).ToList();
                        oEvaluators = oEvaluators.GroupBy(x => x.Value).Select(grp => grp.First()).ToList();

                        //EvaluatorsRoleObj.GroupBy(x => x.Item1).Select(grp => grp.First().Item1).ToList();
                        if (oEvaluators.Count > 0)
                        {
                            oEvaluators.All(x =>
                            {
                                it.SurveyInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumSurveyInfoType.Evaluator
                                    },
                                    Value = x.Value,
                                    Enable = true,
                                });
                                return true;
                            });
                        }
                    }
                    return true;
                });

                #endregion Child Survey
            }
            return oReturn;
        }

        private GenericItemModel GetSurveyReportFilterRequest()
        {
            GenericItemModel oReturn = new GenericItemModel()
            {
                ItemId = 0,
                ItemName = "SurveyFilterReport",
                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)enumReportType.RP_SurveyReport,
                },
                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),

                Enable = true,
            };

            System.Web.HttpContext.Current.Request.Form.AllKeys.Where(x => x.Contains("SurveyInfo_")).All(req =>
            {
                string[] strSplit = req.Split('_');
                if (strSplit.Length > 0)
                {
                    oReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = Convert.ToInt32(strSplit[1].Trim())
                        },
                        Value = System.Web.HttpContext.Current.Request[req],
                        Enable = true,
                    });
                }
                return true;
            });

            return oReturn;
        }

        private DataTable GetAllValues_Balance(DataTable data, ProviderBalanceSheetViewModel oProviderBalanceSheetViewModel)
        {
            int oCurrencyValidate = 0;

            int oCurrentCurrency = !string.IsNullOrEmpty(Request["Currency"]) && int.TryParse(Request["Currency"].ToString(), out oCurrencyValidate) == true ?
                                    Convert.ToInt32(Request["Currency"].Replace(" ", "")) :
                                    Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_COP].Value);

            if (oProviderBalanceSheetViewModel.ChildBalanceSheet != null && oProviderBalanceSheetViewModel.ChildBalanceSheet.Count > 0)
            {
                foreach (var oBalanceInfo in oProviderBalanceSheetViewModel.ChildBalanceSheet)
                {
                    this.GetAllValues_Balance(data, oBalanceInfo);
                }
            }
            if (oProviderBalanceSheetViewModel.AccountType != 2)
            {
                if (!string.IsNullOrEmpty(oProviderBalanceSheetViewModel.AccountFormula))
                {
                    DataRow row;
                    row = data.NewRow();
                    row["Cuenta"] = oProviderBalanceSheetViewModel.RelatedAccount.ItemName;

                    int i = 0;
                    foreach (var oBalanceValues in oProviderBalanceSheetViewModel.RelatedBalanceSheetDetail.Where(x => x.RelatedBalanceSheetDetail != null).OrderByDescending(x => x.Order))
                    {
                        if (i == 0)
                        {
                            row["Valor1"] = oBalanceValues.RelatedBalanceSheetDetail.Value.ToString("#,0.##") + " " + MarketPlace.Models.Company.CompanyUtil.ProviderOptions.Where(x => x.ItemId == oCurrentCurrency).Select(x => x.ItemName).FirstOrDefault();
                            row["AV1"] = oBalanceValues.VerticalValue != null ? oBalanceValues.VerticalValue.Value.ToString("#,0.##") : string.Empty;

                            i++;
                        }
                        else
                        {
                            row["Valor2"] = oBalanceValues.RelatedBalanceSheetDetail.Value.ToString("#,0.##") + " " + MarketPlace.Models.Company.CompanyUtil.ProviderOptions.Where(x => x.ItemId == oCurrentCurrency).Select(x => x.ItemName).FirstOrDefault();
                            row["AV2"] = oBalanceValues.VerticalValue != null ? oBalanceValues.VerticalValue.Value.ToString("#,0.##") : string.Empty;
                        }
                    }

                    row["AH"] = !String.IsNullOrEmpty(oProviderBalanceSheetViewModel.HorizontalValue.ToString()) ? oProviderBalanceSheetViewModel.HorizontalValue.Value.ToString("#,0.##") + " " + MarketPlace.Models.Company.CompanyUtil.ProviderOptions.Where(x => x.ItemId == oCurrentCurrency).Select(x => x.ItemName).FirstOrDefault() : string.Empty;
                    data.Rows.Add(row);
                }
                else
                {
                    DataRow row;
                    row = data.NewRow();
                    row["Cuenta"] = oProviderBalanceSheetViewModel.RelatedAccount.ItemName;

                    int i = 0;
                    foreach (var oBalanceValues in oProviderBalanceSheetViewModel.RelatedBalanceSheetDetail.Where(x => x.RelatedBalanceSheetDetail != null).OrderByDescending(x => x.Order))
                    {
                        if (i == 0)
                        {
                            row["Valor1"] = oBalanceValues.RelatedBalanceSheetDetail.Value.ToString("#,0.##") + " " + MarketPlace.Models.Company.CompanyUtil.ProviderOptions.Where(x => x.ItemId == oCurrentCurrency).Select(x => x.ItemName).FirstOrDefault();
                            row["AV1"] = oBalanceValues.VerticalValue != null ? oBalanceValues.VerticalValue.Value.ToString("#,0.##") : string.Empty;

                            i++;
                        }
                        else
                        {
                            row["Valor2"] = oBalanceValues.RelatedBalanceSheetDetail.Value.ToString("#,0.##") + " " + MarketPlace.Models.Company.CompanyUtil.ProviderOptions.Where(x => x.ItemId == oCurrentCurrency).Select(x => x.ItemName).FirstOrDefault();
                            row["AV2"] = oBalanceValues.VerticalValue != null ? oBalanceValues.VerticalValue.Value.ToString("#,0.##") : string.Empty;
                        }
                    }

                    row["AH"] = !String.IsNullOrEmpty(oProviderBalanceSheetViewModel.HorizontalValue.ToString()) ? oProviderBalanceSheetViewModel.HorizontalValue.Value.ToString("#,0.##") + " " + MarketPlace.Models.Company.CompanyUtil.ProviderOptions.Where(x => x.ItemId == oCurrentCurrency).Select(x => x.ItemName).FirstOrDefault() : string.Empty;
                    data.Rows.Add(row);
                }
            }

            return data;
        }

        private DataTable GetAllValues_Indicators(DataTable data, ProviderBalanceSheetViewModel oProviderBalanceSheetViewModel)
        {
            if (oProviderBalanceSheetViewModel.ChildBalanceSheet != null && oProviderBalanceSheetViewModel.ChildBalanceSheet.Count > 0)
            {
                foreach (var oBalanceInfo in oProviderBalanceSheetViewModel.ChildBalanceSheet)
                {
                    this.GetAllValues_Indicators(data, oBalanceInfo);
                }
            }
            else
            {
                DataRow row;
                row = data.NewRow();

                if (oProviderBalanceSheetViewModel.AccountType != 2)
                {
                    row["Concepto"] = oProviderBalanceSheetViewModel.RelatedAccount.ItemName;

                    int i = 0;
                    foreach (var oBalanceValues in oProviderBalanceSheetViewModel.RelatedBalanceSheetDetail.Where(x => x.RelatedBalanceSheetDetail != null).OrderByDescending(x => x.Order))
                    {
                        if (i == 0)
                        {
                            row["Valor1"] = oBalanceValues.RelatedBalanceSheetDetail.Value.ToString("#,0.##");

                            i++;
                        }
                        else
                        {
                            row["Valor2"] = oBalanceValues.RelatedBalanceSheetDetail.Value.ToString("#,0.##");
                        }
                    }

                    row["Unidad"] = oProviderBalanceSheetViewModel.AccountUnit;
                    row["Formula"] = oProviderBalanceSheetViewModel.AccountFormulaText;
                    row["Interpretación"] = oProviderBalanceSheetViewModel.AccountFormulaDescription.Replace("<br/>", "");

                    data.Rows.Add(row);

                    return data;
                }
                else if (oProviderBalanceSheetViewModel.AccountType == 2)
                {
                    row["Concepto"] = oProviderBalanceSheetViewModel.RelatedAccount.ItemName;
                    row["Valor1"] = " - ";
                    row["Valor2"] = " - ";
                    row["Unidad"] = " - ";
                    row["Formula"] = " - ";
                    row["Interpretación"] = " - ";

                    data.Rows.Add(row);
                }
            }

            return data;
        }

        private string GetCalificationScore(List<CalificationProjectBatchModel> oProviderCalModel, List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigValidateModel> oValidate)
        {
            string oTotalScore = "";
            if (oProviderCalModel != null && oProviderCalModel.Count > 0 && oValidate != null && oValidate.Count > 0)
            {
                oProviderCalModel.FirstOrDefault().ProjectConfigModel.ConfigValidateModel = oValidate;
                //CalificationProjectModel oConfigModel = new CalificationProjectModel();
                //oConfigModel.ConfigValidateModel = oValidate;
                //oConfigModel = oProviderCalModel.FirstOrDefault().ProjectConfigModel.ConfigValidateModel.All();


                oProviderCalModel.FirstOrDefault().ProjectConfigModel.ConfigValidateModel.All(x =>
                {
                    switch (x.Operator.ItemId)
                    {
                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorQue:
                            if (oProviderCalModel.FirstOrDefault().TotalScore > int.Parse(x.Value))
                            {
                                oTotalScore = x.Result;
                            }
                            break;

                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorQue:
                            if (oProviderCalModel.FirstOrDefault().TotalScore < int.Parse(x.Value))
                            {
                                oTotalScore = x.Result;
                            }
                            break;

                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorOIgual:
                            if (oProviderCalModel.FirstOrDefault().TotalScore <= int.Parse(x.Value))
                            {
                                oTotalScore = x.Result;
                            }
                            break;

                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorOIgual:
                            if (oProviderCalModel.FirstOrDefault().TotalScore >= int.Parse(x.Value))
                            {
                                oTotalScore = x.Result;
                            }
                            break;

                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Entre:

                            int minValue = 0;
                            int maxValue = 0;

                            string[] oValue = x.Value.Split(',');
                            minValue = int.Parse(oValue[0]);
                            maxValue = int.Parse(oValue[1]);

                            if (oProviderCalModel.FirstOrDefault().TotalScore < maxValue && oProviderCalModel.FirstOrDefault().TotalScore > minValue)
                            {
                                oTotalScore = x.Result;
                            }

                            break;
                    }
                    return true;
                });

            }
            return oTotalScore;
        }

        #endregion Pivate Functions

        #region Menu

        private List<GenericMenu> GetProviderMenu(ProviderViewModel vProviderInfo)
        {
            List<GenericMenu> oReturn = new List<GenericMenu>();

            if (vProviderInfo.RelatedLiteProvider != null)
            {
                string oCurrentController = CurrentControllerName;
                string oCurrentAction = CurrentActionName;

                List<int> oCurrentProviderMenu = SessionModel.CurrentProviderMenu();
                List<int> oCurrentProviderSubMenu;

                GenericMenu oMenuAux;

                #region GeneralInfo

                if (oCurrentProviderMenu.Any(x => x == (int)enumProviderMenu.GeneralInfo))
                {
                    //header
                    oMenuAux = new GenericMenu()
                    {
                        Name = "Información General",
                        Position = 0,
                        ChildMenu = new List<GenericMenu>(),
                    };

                    oCurrentProviderSubMenu = SessionModel.CurrentProviderSubMenu((int)enumProviderMenu.GeneralInfo);

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.ProviderResume))
                    {
                        //Basic info
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Resumen del Proveedor",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.GIProviderInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 0,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.ContactPersonInfo))
                    {
                        //Company persons Contact info
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Información de Personas de Contacto",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.GIPersonContactInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 1,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.GIPersonContactInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.Branches))
                    {
                        //Branch
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Sucursales",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.GIBranchInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 2,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.GIBranchInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.RepresentantInfo))
                    {
                        //Distributors
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Representación y/o Distribuciones",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.GIDistributorInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 3,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.GIDistributorInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.RestrictiveList))
                    {
                        //Listas Restrictivas
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Listas Restrictivas",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.GIBlackList,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 4,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.GIBlackList &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.ProviderCalification))
                    {
                        //Calificación del Proveedor.
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Calificación del proveedor",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.GICalificationProjectDetail,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                                    }),
                            Position = 5,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.GICalificationProjectDetail &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.Tracing))
                    {
                        //Seguimientos
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Seguimientos",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.GITrackingInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 6,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.GITrackingInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    //get is selected menu
                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    //add menu
                    oReturn.Add(oMenuAux);
                }

                #endregion

                #region Legal Info


                if (oCurrentProviderMenu.Any(x => x == (int)enumProviderMenu.LegalInfo))
                {
                    //header
                    oMenuAux = new GenericMenu()
                    {
                        Name = "Información Legal",
                        Position = 1,
                        ChildMenu = new List<GenericMenu>(),
                    };

                    oCurrentProviderSubMenu = new List<int>();

                    oCurrentProviderSubMenu = SessionModel.CurrentProviderSubMenu((int)enumProviderMenu.LegalInfo);

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.ChaimberOfCommerce))
                    {
                        //chaimber of commerce
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Cámara de Comercio",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.LIChaimberOfCommerceInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 0,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.LIChaimberOfCommerceInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.RUT))
                    {
                        //RUT
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Registro Único Tributario",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.LIRutInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 1,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.LIRutInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.Sarlaft))
                    {
                        //SARLAFT
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "SARLAFT",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.LISARLAFTInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 3,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.LISARLAFTInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.Resolutions))
                    {
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Resoluciones",
                            Url = Url.RouteUrl
                                (Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.Provider.Name,
                                    action = MVC.Provider.ActionNames.LIResolutionInfo,
                                    ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                }),
                            Position = 4,
                            IsSelected =
                            (oCurrentAction == MVC.Provider.ActionNames.LIResolutionInfo &&
                            oCurrentController == MVC.Provider.Name),
                        });
                    }

                    //get is selected menu
                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    //add menu
                    oReturn.Add(oMenuAux);
                }

                #endregion

                #region Financial Info

                if (oCurrentProviderMenu.Any(x => x == (int)enumProviderMenu.FinancialInfo))
                {
                    //header
                    oMenuAux = new GenericMenu()
                    {
                        Name = "Información Financiera",
                        Position = 2,
                        ChildMenu = new List<GenericMenu>(),
                    };

                    oCurrentProviderSubMenu = new List<int>();

                    oCurrentProviderSubMenu = SessionModel.CurrentProviderSubMenu((int)enumProviderMenu.FinancialInfo);

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.FinancialStates))
                    {
                        //financial states
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Estados Financieros",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.FIBalanceSheetInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 0,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.FIBalanceSheetInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.IncomeStatement))
                    {
                        //income statement
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Declaración de Renta",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.FIIncomeStatementInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 2,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.FIIncomeStatementInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.BankInfo))
                    {
                        //Bank Info
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Información Bancaria",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.FIBankInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 3,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.FIBankInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.KContract))
                    {
                        //K Contract
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "K Contratación",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.FIKContract,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 2,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.FIKContract &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    //get is selected menu
                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    //add menu
                    oReturn.Add(oMenuAux);
                }

                #endregion

                #region Commercial Info

                if (oCurrentProviderMenu.Any(x => x == (int)enumProviderMenu.CommercialInfo))
                {
                    //header
                    oMenuAux = new GenericMenu()
                    {
                        Name = "Información Comercial",
                        Position = 3,
                        ChildMenu = new List<GenericMenu>(),
                    };

                    oCurrentProviderSubMenu = new List<int>();

                    oCurrentProviderSubMenu = SessionModel.CurrentProviderSubMenu((int)enumProviderMenu.CommercialInfo);

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.Experiences))
                    {
                        //Experience
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Experiencias",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.CIExperiencesInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 0,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.CIExperiencesInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    //get is selected menu
                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    //add menu
                    oReturn.Add(oMenuAux);
                }

                #endregion

                #region HSEQ Info

                if (oCurrentProviderMenu.Any(x => x == (int)enumProviderMenu.HSEQInfo))
                {
                    //header
                    oMenuAux = new GenericMenu()
                    {
                        Name = "Información HSEQ",
                        Position = 4,
                        ChildMenu = new List<GenericMenu>(),
                    };

                    oCurrentProviderSubMenu = new List<int>();

                    oCurrentProviderSubMenu = SessionModel.CurrentProviderSubMenu((int)enumProviderMenu.HSEQInfo);

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.Certifications))
                    {
                        //Certifications
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Certificaciones",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.HICertificationsInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 0,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.HICertificationsInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.Secure))
                    {
                        //Company healty politic
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Política de Seguridad, Salud y Medio Ambiente",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.HIHealtyPoliticInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 1,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.HIHealtyPoliticInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.Risk))
                    {
                        //Company healty politic
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Sistema de Riesgos Laborales",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.HIRiskPoliciesInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 2,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.HIRiskPoliciesInfo &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    //get is selected menu
                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    //add menu
                    oReturn.Add(oMenuAux);
                }

                #endregion

                #region Aditional Info

                if (oCurrentProviderMenu.Any(x => x == (int)enumProviderMenu.AditionalInfo))
                {
                    //header
                    oMenuAux = new GenericMenu()
                    {
                        Name = "Información Adicional",
                        Position = 5,
                        ChildMenu = new List<GenericMenu>(),
                    };

                    oCurrentProviderSubMenu = new List<int>();

                    oCurrentProviderSubMenu = SessionModel.CurrentProviderSubMenu((int)enumProviderMenu.AditionalInfo);

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.AddInformation))
                    {
                        //Aditional Documents
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Información Anexa",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.ADAditionalDocument,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 0,
                            IsSelected =
                                    (oCurrentAction == MVC.Provider.ActionNames.ADAditionalDocument &&
                                    oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.AddData))
                    {
                        //Aditional Data
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Datos Adicionales",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.ADAditionalData,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 1,
                            IsSelected =
                                    (oCurrentAction == MVC.Provider.ActionNames.ADAditionalData &&
                                    oCurrentController == MVC.Provider.Name),
                        });
                    }

                    //get is selected menu
                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    //add menu
                    oReturn.Add(oMenuAux);
                }

                #endregion

                #region Survey Info

                if (oCurrentProviderMenu.Any(x => x == (int)enumProviderMenu.Survey))
                {
                    //header
                    oMenuAux = new GenericMenu()
                    {
                        Name = "Evaluación de Desempeño",
                        Position = 6,
                        ChildMenu = new List<GenericMenu>(),
                    };

                    oCurrentProviderSubMenu = new List<int>();

                    oCurrentProviderSubMenu = SessionModel.CurrentProviderSubMenu((int)enumProviderMenu.Survey);

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.SurveyList))
                    {
                        //survey list
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Lista de Evaluaciones",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.SVSurveySearch,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 0,
                            IsSelected =
                                ((oCurrentAction == MVC.Provider.ActionNames.SVSurveySearch ||
                                oCurrentAction == MVC.Provider.ActionNames.SVSurveyDetail) &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.SurveyReports))
                    {
                        //survey reports
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Reportes de Evaluaciones",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.SVSurveyReport,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 1,
                            IsSelected =
                                    (oCurrentAction == MVC.Provider.ActionNames.SVSurveyReport &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    if (oCurrentProviderSubMenu.Any(y => y == (int)enumProviderSubMenu.SurveyProgram))
                    {
                        //survey program
                        oMenuAux.ChildMenu.Add(new GenericMenu()
                        {
                            Name = "Programar Evaluación",
                            Url = Url.RouteUrl
                                    (Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.SVSurveyProgram,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 2,
                            IsSelected =
                                    (oCurrentAction == MVC.Provider.ActionNames.SVSurveyProgram &&
                                oCurrentController == MVC.Provider.Name),
                        });
                    }

                    //get is selected menu
                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    //add menu
                    oReturn.Add(oMenuAux);
                }

                #endregion

                #region Custom Data

                List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oRelatedCustomer = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.GetCustomerProviderByCustomData(vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId);

                string oCustomData = oRelatedCustomer.Where(x => x.CompanyPublicId == SessionModel.CurrentCompany.CompanyPublicId).Select(x => x.CompanyPublicId).DefaultIfEmpty(string.Empty).FirstOrDefault();

                if (!string.IsNullOrEmpty(oCustomData))
                {
                    //header
                    oMenuAux = new GenericMenu()
                    {
                        Name = "Datos por cliente",
                        Position = 6,
                        ChildMenu = new List<GenericMenu>(),
                    };

                    //survey list
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Campos personalizados",
                        Url = Url.RouteUrl
                                (Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.Provider.Name,
                                    action = MVC.Provider.ActionNames.CDCustomData,
                                    ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                }),
                        Position = 0,
                        IsSelected =
                            ((oCurrentAction == MVC.Provider.ActionNames.CDCustomData ||
                            oCurrentAction == MVC.Provider.ActionNames.CDCustomData) &&
                            oCurrentController == MVC.Provider.Name),
                    });

                    //get is selected menu
                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    //add menu
                    oReturn.Add(oMenuAux);
                }

                #endregion
            }
            return oReturn;
        }

        #endregion Menu
    }
}