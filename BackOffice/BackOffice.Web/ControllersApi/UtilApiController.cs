using BackOffice.Models.Admin;
using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class UtilApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByGeography
            (string CategorySearchByGeography, string SearchParam, string CityId)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();
            int oTotalRows;
            if (CategorySearchByGeography == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> Cities =
                    ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography
                    (string.IsNullOrEmpty(SearchParam) ? null : SearchParam,
                        string.IsNullOrEmpty(CityId) ? null : (int?)Convert.ToInt32(CityId),
                        0,
                        Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[
                            BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault
                        ].Value), out oTotalRows);

                if (Cities != null && Cities.Count > 0)
                {
                    oReturn = Cities.Select(x => new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                    {
                        ItemId = x.City.ItemId,
                        ItemName = x.Country.ItemName + "," + x.State.ItemName + "," + x.City.ItemName,
                    }).ToList();
                }
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<AdminCategoryViewModel> GetAllGeography
            (string GetAllGeography, string SearchParam, string CityId, int PageNumber, int RowCount, string IsAutoComplete)
        {
            List<BackOffice.Models.Admin.AdminCategoryViewModel> oReturn = new List<Models.Admin.AdminCategoryViewModel>();
            if (GetAllGeography == "true")
            {
                int oTotalCount;
                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> GeographyAdmin =
                    ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeographyAdmin
                    (SearchParam, PageNumber, Convert.ToInt32(RowCount), out oTotalCount);

                if (GeographyAdmin != null)
                {
                    GeographyAdmin.All(x =>
                    {
                        oReturn.Add(new BackOffice.Models.Admin.AdminCategoryViewModel(x));
                        return true;
                    });
                }

                if (IsAutoComplete == "true")
                {
                    oReturn = oReturn.Where(x => x.GIT_Country.ToLower().Contains(SearchParam.ToLower())).Select(x => x).ToList();
                }
                else
                {
                    oReturn.All(x =>
                    {
                        x.AllTotalRows = oTotalCount;
                        return true;
                    });
                }
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public AdminCategoryViewModel CategoryUpsert(string CategoryUpsert, string CategoryType, string TreeId)
        {
            AdminCategoryViewModel oReturn = null;

            if (CategoryUpsert == "true" && !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"])
              && !string.IsNullOrEmpty(CategoryType))
            {
                AdminCategoryViewModel oDataToUpsert =
                    (AdminCategoryViewModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
                                typeof(AdminCategoryViewModel));

                #region Geolocalization
                if (CategoryType == "AdminGeo")
                {

                    GenericItemModel oCountryToUpsert = new GenericItemModel();
                    List<GenericItemInfoModel> oCountryInfo = new List<GenericItemInfoModel>();

                    GenericItemModel oCityToUpsert = new GenericItemModel();
                    List<GenericItemInfoModel> oCityInfo = new List<GenericItemInfoModel>();

                    GenericItemModel oStateToUpsert = new GenericItemModel();
                    List<GenericItemInfoModel> oStateInfo = new List<GenericItemInfoModel>();

                    //Country
                    oCountryToUpsert = new GenericItemModel()
                    {
                        ItemId = string.IsNullOrEmpty(oDataToUpsert.GIT_CountryId) ? 0 : Convert.ToInt32(oDataToUpsert.GIT_CountryId),
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),

                        ItemName = oDataToUpsert.GIT_Country,
                        Enable = oDataToUpsert.GIT_CountryEnable,
                        ItemInfo = new List<GenericItemInfoModel>()
                    };
                    oCountryInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.GIT_CountryTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.GIT_CountryTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.GI_GeographyType
                        },
                        Value = Convert.ToString((int)BackOffice.Models.General.enumCategoryInfoType.GIT_Country),
                        Enable = oDataToUpsert.GIT_CountryEnable,
                    });

                    oCountryInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.GIT_CountryDirespCodeId) ? 0 : Convert.ToInt32(oDataToUpsert.GIT_CountryDirespCodeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode
                        },
                        Value = oDataToUpsert.GIT_CountryDirespCode,
                        Enable = oDataToUpsert.GIT_CountryEnable,
                    });

                    oCountryToUpsert.ItemInfo.AddRange(oCountryInfo);

                    //City
                    oCityToUpsert = new GenericItemModel()
                    {
                        ItemId = string.IsNullOrEmpty(oDataToUpsert.AG_CityId) ? 0 : Convert.ToInt32(oDataToUpsert.AG_CityId),
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),
                        ItemName = oDataToUpsert.AG_City,
                        Enable = oDataToUpsert.GI_CityEnable,
                        ItemInfo = new List<GenericItemInfoModel>()
                    };

                    oCityInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.GI_CapitalTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.GI_CapitalTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.GI_CapitalType
                        },
                        Value = oDataToUpsert.GI_CapitalType,
                        Enable = oDataToUpsert.GI_CityEnable,
                    });

                    oCityInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.GI_CityDirespCodeId) ? 0 : Convert.ToInt32(oDataToUpsert.GI_CityDirespCodeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode
                        },
                        Value = oDataToUpsert.GI_CityDirespCode,
                        Enable = oDataToUpsert.GI_CityEnable,
                    });

                    oCityToUpsert.ItemInfo.AddRange(oCityInfo);

                    //State
                    oStateToUpsert = new GenericItemModel()
                    {
                        ItemId = string.IsNullOrEmpty(oDataToUpsert.GIT_StateId) ? 0 : Convert.ToInt32(oDataToUpsert.GIT_StateId),
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),
                        ItemName = oDataToUpsert.GIT_State,
                        Enable = oDataToUpsert.GIT_StateEnable,
                        ItemInfo = new List<GenericItemInfoModel>()
                    };

                    oStateInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.GIT_StateDirespCodeId) ? 0 : Convert.ToInt32(oDataToUpsert.GIT_StateDirespCodeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode
                        },
                        Value = oDataToUpsert.GIT_StateDirespCode,
                        Enable = oDataToUpsert.GIT_StateEnable,
                    });

                    oStateToUpsert.ItemInfo.AddRange(oStateInfo);

                    GenericItemModel CountryResult = new GenericItemModel();
                    GenericItemModel StateResult = new GenericItemModel();
                    GenericItemModel CityResult = new GenericItemModel();

                    CountryResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(1, oCountryToUpsert);

                    oStateToUpsert.ParentItem = new GenericItemModel() { ItemId = CountryResult.ItemId };
                    StateResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(1, oStateToUpsert);

                    oCityToUpsert.ParentItem = new GenericItemModel() { ItemId = StateResult.ItemId };
                    CityResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(1, oCityToUpsert);

                    GeographyModel oResult = new GeographyModel();

                    oResult.Country = CountryResult;
                    oResult.State = StateResult;
                    oResult.City = CityResult;

                    oReturn = new AdminCategoryViewModel(oResult);
                }

                #endregion

                #region Banks
                if (CategoryType == "AdminBank")
                {
                    GenericItemModel oBankToUpsert = new GenericItemModel();
                    List<GenericItemInfoModel> oBankInfo = new List<GenericItemInfoModel>();

                    //Country
                    oBankToUpsert = new GenericItemModel()
                    {
                        ItemId = string.IsNullOrEmpty(oDataToUpsert.B_BankId) ? 0 : Convert.ToInt32(oDataToUpsert.B_BankId),
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),

                        ItemName = oDataToUpsert.B_Bank,
                        Enable = oDataToUpsert.B_BankEnable,
                        ItemInfo = new List<GenericItemInfoModel>()
                    };
                    oBankInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.B_CityId) ? 0 : Convert.ToInt32(oDataToUpsert.B_CityId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.B_Location,
                        },
                        Value = oDataToUpsert.B_CityId,
                        Enable = oDataToUpsert.B_BankEnable,
                    });
                    oBankInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.B_BankCodeId) ? 0 : Convert.ToInt32(oDataToUpsert.B_BankCodeId.Trim()),
                        ItemInfoType = new CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.B_Code,
                        },
                        Value = oDataToUpsert.B_BankCode,
                        Enable = oDataToUpsert.B_BankEnable,
                    });

                    GenericItemModel BankResult = new GenericItemModel();

                    oBankToUpsert.ItemInfo.AddRange(oBankInfo);
                    BankResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(11, oBankToUpsert);
                }
                #endregion

                #region CompanyRules

                if (CategoryType == "AdminCompanyRules")
                {
                    GenericItemModel oCompanyRulesToUpsert = new GenericItemModel();

                    oCompanyRulesToUpsert = new GenericItemModel()
                    {
                        ItemId = string.IsNullOrEmpty(oDataToUpsert.CR_CompanyRuleId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_CompanyRuleId),
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),

                        ItemName = oDataToUpsert.CR_CompanyRule,
                        Enable = oDataToUpsert.CR_CompanyRuleEnable,
                        ItemInfo = new List<GenericItemInfoModel>()
                    };

                    GenericItemModel CompanyRuleResult = new GenericItemModel();

                    CompanyRuleResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(2, oCompanyRulesToUpsert);
                }

                #endregion

                #region Rules

                if (CategoryType == "AdminRules")
                {
                    GenericItemModel oRulesToUpsert = new GenericItemModel();

                    oRulesToUpsert = new GenericItemModel()
                    {
                        ItemId = string.IsNullOrEmpty(oDataToUpsert.R_RuleId) ? 0 : Convert.ToInt32(oDataToUpsert.R_RuleId),
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),

                        ItemName = oDataToUpsert.R_Rule,
                        Enable = oDataToUpsert.R_RuleEnable,
                        ItemInfo = new List<GenericItemInfoModel>()
                    };

                    GenericItemModel RuleResult = new GenericItemModel();

                    RuleResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(3, oRulesToUpsert);
                }

                #endregion

                #region Resolution

                if (CategoryType == "AdminResolution")
                {
                    GenericItemModel oResolutionToUpsert = new GenericItemModel();

                    oResolutionToUpsert = new GenericItemModel()
                    {
                        ItemId = string.IsNullOrEmpty(oDataToUpsert.RS_ResolutionId) ? 0 : Convert.ToInt32(oDataToUpsert.RS_ResolutionId),
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),

                        ItemName = oDataToUpsert.RS_Resolution,
                        Enable = oDataToUpsert.RS_ResolutionEnable,
                        ItemInfo = new List<GenericItemInfoModel>(),
                    };

                    GenericItemModel ResolutionResult = new GenericItemModel();
                    ResolutionResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(8, oResolutionToUpsert);
                }

                #endregion

                #region Standar Economy Activities

                if (CategoryType == "AdminEcoAcEstandar")
                {
                    GenericItemModel oActivityToUpsert = new GenericItemModel();
                    List<GenericItemInfoModel> oActivityInfo = new List<GenericItemInfoModel>();

                    //Activity
                    oActivityToUpsert = new GenericItemModel()
                    {
                        ItemId = string.IsNullOrEmpty(oDataToUpsert.ECS_EconomyActivityId) ? 0 : Convert.ToInt32(oDataToUpsert.ECS_EconomyActivityId),
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),

                        ItemName = oDataToUpsert.ECS_EconomyActivity,
                        Enable = oDataToUpsert.ECS_Enable,
                        ItemInfo = new List<GenericItemInfoModel>()
                    };
                    oActivityInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.ECS_TypeId) ? 0 : Convert.ToInt32(oDataToUpsert.ECS_TypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.EA_Type
                        },
                        Value = oDataToUpsert.ECS_Type,
                        Enable = oDataToUpsert.ECS_Enable,
                    });
                    oActivityInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.ECS_GroupId) ? 0 : Convert.ToInt32(oDataToUpsert.ECS_GroupId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.EA_Group
                        },
                        Value = oDataToUpsert.ECS_Group,
                        Enable = oDataToUpsert.ECS_Enable,
                    });
                    oActivityInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.ECS_CategoryId) ? 0 : Convert.ToInt32(oDataToUpsert.ECS_CategoryId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.EA_Category
                        },
                        Value = oDataToUpsert.ECS_Category,
                        Enable = oDataToUpsert.ECS_Enable,
                    });
                    if (TreeId != "4")
                    {
                        oActivityInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new CatalogModel()
                                {
                                    ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.EA_IsCustom
                                },
                                Value = "true",
                                Enable = true,
                            });
                    }
                    GenericItemModel oActivityResult = new GenericItemModel();

                    oActivityToUpsert.ItemInfo.AddRange(oActivityInfo);
                    oActivityResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(Convert.ToInt32(TreeId), oActivityToUpsert);
                }

                #endregion

                #region Group

                if (CategoryType == "AdminEcoGroupEstandar")
                {
                    GenericItemModel oGroupToUpsert = new GenericItemModel();

                    oGroupToUpsert = new GenericItemModel()
                    {
                        ItemId = string.IsNullOrEmpty(oDataToUpsert.G_GroupId) ? 0 : Convert.ToInt32(oDataToUpsert.G_GroupId),
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),

                        ItemName = oDataToUpsert.G_Group,
                        Enable = oDataToUpsert.G_GroupEnable,
                        ItemInfo = new List<GenericItemInfoModel>(),
                    };

                    GenericItemModel ResolutionResult = new GenericItemModel();
                    ResolutionResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(7, oGroupToUpsert);
                }

                #endregion

                #region Tree
                if (CategoryType == "AdminTree")
                {
                    TreeModel oTreeModel = new TreeModel();

                    oTreeModel = new TreeModel()
                    {
                        TreeId = string.IsNullOrEmpty(oDataToUpsert.T_TreeId) ? 0 : Convert.ToInt32(oDataToUpsert.T_TreeId),
                        TreeName = oDataToUpsert.T_TreeName,
                        Enable = oDataToUpsert.T_TreeEnable,
                    };

                    TreeModel oTreeResult = new TreeModel();

                    //Create the category default
                    GenericItemModel oTreeToUpsert = new GenericItemModel();
                    List<GenericItemInfoModel> oTreeInfo = new List<GenericItemInfoModel>();
                    GenericItemModel oTreeCategoryResult = new GenericItemModel();

                    oTreeToUpsert = new GenericItemModel()
                    {
                        ItemId = 0,
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),

                        ItemName = oDataToUpsert.T_TreeName + "_" + "Activity Default",
                        Enable = true,
                        ItemInfo = new List<GenericItemInfoModel>(),
                    };

                    oTreeInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.EA_IsCustom
                        },
                        Value = "true",
                        Enable = true,
                    });

                    oTreeToUpsert.ItemInfo.AddRange(oTreeInfo);

                    //Save Tree
                    oTreeResult = ProveedoresOnLine.Company.Controller.Company.TreeUpsert(oTreeModel);
                    //Save Category
                    oTreeCategoryResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(oTreeResult.TreeId, oTreeToUpsert);

                    oReturn = new AdminCategoryViewModel();
                    oReturn.T_TreeId = oTreeResult.TreeId.ToString();
                    oReturn.T_TreeName = oTreeResult.TreeName;
                }
                #endregion

                #region TRM
                if (CategoryType == "AdminTRM")
                {
                    CurrencyExchangeModel oExchangeToUpsert = new CurrencyExchangeModel();

                    oExchangeToUpsert = new CurrencyExchangeModel()
                    {
                        IssueDate = DateTime.Parse(oDataToUpsert.C_IssueDate),
                        MoneyTypeFrom = new CatalogModel(){ ItemId = Convert.ToInt32(oDataToUpsert.C_MoneyTypeFromId), ItemName = oDataToUpsert.C_MoneyTypeFromName},
                        MoneyTypeTo = new CatalogModel(){ ItemId = Convert.ToInt32(oDataToUpsert.C_MoneyTypeToId), ItemName = oDataToUpsert.C_MoneyTypeToName},
                        Rate = Convert.ToDecimal(oDataToUpsert.C_Rate),                        
                    };

                    oExchangeToUpsert.CurrencyExchangeId = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeInsert(oExchangeToUpsert);
                    oReturn = new AdminCategoryViewModel();
                    
                    oReturn.C_CurrentExchangeId = oExchangeToUpsert.CurrencyExchangeId.ToString();                    
                    oReturn.C_IssueDate = oDataToUpsert.C_IssueDate;
                    oReturn.C_MoneyTypeFromId = oDataToUpsert.C_MoneyTypeFromId;
                    oReturn.C_MoneyTypeToId = oDataToUpsert.C_MoneyTypeToId;
                    oReturn.C_Rate = oDataToUpsert.C_Rate;
                }
                #endregion
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.General.EconomicActivityViewModel> CategorySearchByActivity
            (string CategorySearchByActivity, string SearchParam, string IsDefault)
        {
            List<BackOffice.Models.General.EconomicActivityViewModel> oReturn =
                new List<BackOffice.Models.General.EconomicActivityViewModel>();

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oActivities = null;

            if (CategorySearchByActivity == "true" && IsDefault == "true")
            {
                oActivities = ProveedoresOnLine.Company.Controller.Company.CategorySearchByActivity
                    (string.IsNullOrEmpty(SearchParam) ? null : SearchParam,
                        0,
                        Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[
                            BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault
                        ].Value));
            }
            else if (CategorySearchByActivity == "true" && IsDefault == "false")
            {
                oActivities = ProveedoresOnLine.Company.Controller.Company.CategorySearchByCustomActivity
                    (string.IsNullOrEmpty(SearchParam) ? null : SearchParam,
                        0,
                        Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[
                            BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault
                        ].Value));
            }

            if (oActivities != null && oActivities.Count > 0)
            {
                oReturn = oActivities.Select(x => new BackOffice.Models.General.EconomicActivityViewModel()
                {
                    EconomicActivityId = x.ItemId.ToString(),
                    ActivityName = x.ItemName,
                    ActivityType = x.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.EA_Type).
                        Select(y => y.Value + " - " + y.ValueName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    ActivityGroup = x.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.EA_Group).
                        Select(y => y.Value + " - " + y.ValueName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    ActivityCategory = x.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.EA_Category).
                        Select(y => y.Value + " - " + y.ValueName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                }).ToList();
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByRule
            (string CategorySearchByRule, string SearchParam)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                       new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            if (CategorySearchByRule == "true")
            {
                oReturn = ProveedoresOnLine.Company.Controller.Company.CategorySearchByRules
                    (SearchParam,
                    0,
                    Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[
                                BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault
                            ].Value));
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByCompanyRule
            (string CategorySearchByCompanyRule, string SearchParam)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            if (CategorySearchByCompanyRule == "true")
            {
                oReturn = ProveedoresOnLine.Company.Controller.Company.CategorySearchByCompanyRules
                    (SearchParam,
                    0,
                    Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[
                                BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault
                            ].Value));
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByResolution
            (string CategorySearchByResolution, string SearchParam)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();
            if (CategorySearchByResolution == "true")
            {
                oReturn = ProveedoresOnLine.Company.Controller.Company.CategorySearchByResolution
                    (SearchParam,
                    0,
                    Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[
                                BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault
                            ].Value));
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByARL
            (string CategorySearchByARL, string SearchParam)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();
            if (CategorySearchByARL == "true")
            {
                oReturn = ProveedoresOnLine.Company.Controller.Company.CategorySearchByARLCompany
                    (SearchParam,
                    0,
                    Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[
                                    BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault
                                    ].Value));
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByBank
            (string CategorySearchByBank, string SearchParam)
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            if (CategorySearchByBank == "true")
            {
                oReturn = ProveedoresOnLine.Company.Controller.Company.CategorySearchByBank
                    (SearchParam,0,0);
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<AdminCategoryViewModel> CategorySearchByBankAdmin
            (string CategorySearchByBankAdmin, string SearchParam, int PageNumber, int RowCount, string IsAutoComplete)
        {
            List<AdminCategoryViewModel> oReturn = new List<AdminCategoryViewModel>();
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oBanks =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            int oTotalCount = 0;
            if (CategorySearchByBankAdmin == "true")
            {
                oBanks = ProveedoresOnLine.Company.Controller.Company.CategorySearchByBankAdmin
                    (SearchParam, PageNumber, RowCount, out oTotalCount);
            }

            if (oBanks != null)
            {
                oBanks.All(x =>
                {
                    oReturn.Add(new BackOffice.Models.Admin.AdminCategoryViewModel(x));
                    return true;
                });
            }
            if (IsAutoComplete == "true")
            {
                oReturn = oReturn.Where(x => x.B_Bank.ToLower().Contains(SearchParam.ToLower())).Select(x => x).ToList();
            }
            else
            {
                oReturn.All(x =>
                {
                    x.AllTotalRows = oTotalCount;
                    return true;
                });
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<AdminCategoryViewModel> CategorySearchByCompanyRulesAdmin
            (string CategorySearchByCompanyRulesAdmin, string SearchParam, int PageNumber, int RowCount)
        {
            List<AdminCategoryViewModel> oReturn = new List<AdminCategoryViewModel>();
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCompanyRules =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            int oTotalCount = 0;
            if (CategorySearchByCompanyRulesAdmin == "true")
            {
                oCompanyRules = ProveedoresOnLine.Company.Controller.Company.CategorySearchByCompanyRulesAdmin
                            (SearchParam, PageNumber, RowCount, out oTotalCount);
            }

            if (oCompanyRules != null)
            {
                oCompanyRules.All(x =>
                {
                    oReturn.Add(new BackOffice.Models.Admin.AdminCategoryViewModel(x));
                    return true;
                });
            }
            else
            {
                oReturn.All(x =>
                {
                    x.AllTotalRows = oTotalCount;
                    return true;
                });
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<AdminCategoryViewModel> CategorySearchByRulesAdmin
            (string CategorySearchByRulesAdmin, string SearchParam, int PageNumber, int RowCount)
        {
            List<AdminCategoryViewModel> oReturn = new List<AdminCategoryViewModel>();
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oRules =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            int oTotalCount = 0;
            if (CategorySearchByRulesAdmin == "true")
            {
                oRules = ProveedoresOnLine.Company.Controller.Company.CategorySearchByRulesAdmin
                            (SearchParam, PageNumber, RowCount, out oTotalCount);
            }

            if (oRules != null)
            {
                oRules.All(x =>
                {
                    oReturn.Add(new BackOffice.Models.Admin.AdminCategoryViewModel(x));
                    return true;
                });
            }
            else
            {
                oReturn.All(x =>
                {
                    x.AllTotalRows = oTotalCount;
                    return true;
                });
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<AdminCategoryViewModel> CategorySearchByICA
        (string CategorySearchByICA, string SearchParam, int PageNumber, int RowCount)
        {
            List<AdminCategoryViewModel> oReturn = new List<AdminCategoryViewModel>();
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oICA =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            int oTotalCount = 0;
            if (CategorySearchByICA == "true")
            {
                oICA = ProveedoresOnLine.Company.Controller.Company.CategorySearchByICA
                            (SearchParam, PageNumber, RowCount, out oTotalCount);
            }

            if (oICA != null)
            {
                oICA.All(x =>
                {
                    oReturn.Add(new BackOffice.Models.Admin.AdminCategoryViewModel(x));
                    return true;
                });
            }
            else
            {
                oReturn.All(x =>
                {
                    x.AllTotalRows = oTotalCount;
                    return true;
                });
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<AdminCategoryViewModel> CategorySearchByResolutionAdmin
            (string CategorySearchByResolutionAdmin, string SearchParam, int PageNumber, int RowCount)
        {
            List<AdminCategoryViewModel> oReturn = new List<AdminCategoryViewModel>();
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oResolution =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            int oTotalCount = 0;
            if (CategorySearchByResolutionAdmin == "true")
            {
                oResolution = ProveedoresOnLine.Company.Controller.Company.CategorySearchByResolutionAdmin
                            (SearchParam, PageNumber, RowCount, out oTotalCount);
            }

            if (oResolution != null)
            {
                oResolution.All(x =>
                {
                    oReturn.Add(new BackOffice.Models.Admin.AdminCategoryViewModel(x));
                    return true;
                });
            }
            else
            {
                oReturn.All(x =>
                {
                    x.AllTotalRows = oTotalCount;
                    return true;
                });
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<AdminCategoryViewModel> CategorySearchByEcoActivityAdmin
            (string CategorySearchByEcoActivityAdmin, string SearchParam, int PageNumber, int RowCount, int TreeId)
        {
            List<AdminCategoryViewModel> oReturn = new List<AdminCategoryViewModel>();
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oStandarActivityAdmin =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            int oTotalCount = 0;
            if (CategorySearchByEcoActivityAdmin == "true")
            {
                oStandarActivityAdmin = ProveedoresOnLine.Company.Controller.Company.CategorySearchByEcoActivityAdmin
                            (SearchParam, PageNumber, RowCount, TreeId, out oTotalCount);
            }

            if (oStandarActivityAdmin != null)
            {
                oStandarActivityAdmin.All(x =>
                {
                    oReturn.Add(new BackOffice.Models.Admin.AdminCategoryViewModel(x));
                    return true;
                });
            }

            oReturn.All(x =>
            {
                x.AllTotalRows = oTotalCount;
                return true;
            });

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<AdminCategoryViewModel> CategorySearchByEcoGroupAdmin
            (string CategotySearchByGroupStandarAdmin, string SearchParam, int PageNumber, int RowCount, int TreeId)
        {
            List<AdminCategoryViewModel> oReturn = new List<AdminCategoryViewModel>();
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oStandarGroupAdmin =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            int oTotalCount = 0;
            if (CategotySearchByGroupStandarAdmin == "true")
            {
                oStandarGroupAdmin = ProveedoresOnLine.Company.Controller.Company.CategorySearchByEcoGroupAdmin
                            (SearchParam, PageNumber, RowCount, TreeId, out oTotalCount);
            }

            if (oStandarGroupAdmin != null)
            {
                oStandarGroupAdmin.All(x =>
                {
                    oReturn.Add(new BackOffice.Models.Admin.AdminCategoryViewModel(x));
                    return true;
                });
            }

            oReturn.All(x =>
            {
                x.AllTotalRows = oTotalCount;
                return true;
            });

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<AdminCategoryViewModel> CategorySearchByTreeAdmin
            (string CategorySearchByTreeAdmin, string SearchParam, int PageNumber, int RowCount)
        {
            List<AdminCategoryViewModel> oReturn = new List<AdminCategoryViewModel>();
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oTreeAdmin =
                new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

            if (CategorySearchByTreeAdmin == "true")
            {
                oTreeAdmin = ProveedoresOnLine.Company.Controller.Company.CategorySearchByTreeAdmin
                            (SearchParam, PageNumber, RowCount);
            }

            if (oTreeAdmin != null)
            {
                oTreeAdmin.All(x =>
                {
                    oReturn.Add(new BackOffice.Models.Admin.AdminCategoryViewModel(x));
                    return true;
                });
            }

            oReturn.All(x =>
            {
                x.AllTotalRows = oTreeAdmin.Count();
                return true;
            });

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<AdminCategoryViewModel> CategorySearchByTRAdmin
            (string CategorySearchByTreeAdmin)
        {
            List<AdminCategoryViewModel> oReturn = new List<AdminCategoryViewModel>();
            List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel> oExchange =
                new List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel>();

            if (CategorySearchByTreeAdmin == "true")
            {
                oExchange = ProveedoresOnLine.Company.Controller.Company.CurrentExchangeGetAllAdmin();
            }

            if (oExchange != null)
            {
                oExchange.All(x =>
                {
                    oReturn.Add(new BackOffice.Models.Admin.AdminCategoryViewModel(x));
                    return true;
                });
            }         

            return oReturn;
        }

    }
}
