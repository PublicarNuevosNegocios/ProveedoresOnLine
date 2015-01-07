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
            (string GetAllGeography, string SearchParam, string CityId, int PageNumber, int RowCount)
        {

            List<BackOffice.Models.Admin.AdminCategoryViewModel> oReturn = new List<Models.Admin.AdminCategoryViewModel>();
            if (GetAllGeography == "true")
            {
                int oTotalCount;
                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> Cities =
                    ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography
                    (string.IsNullOrEmpty(SearchParam) ? null : SearchParam,
                        string.IsNullOrEmpty(CityId) ? null : (int?)Convert.ToInt32(CityId), PageNumber,
                        Convert.ToInt32(RowCount), out oTotalCount);

                if (Cities != null)
                {
                    Cities.All(x =>
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
            }
            return oReturn.OrderBy(x => x.GIT_Country).ToList();
        }

        [HttpPost]
        [HttpGet]
        public AdminCategoryViewModel CategoryUpsert(string CategoryUpsert, string CategoryType)
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
                        Enable = oDataToUpsert.Enable,
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
                    Enable = oDataToUpsert.Enable,
                });

                //City
                oCityToUpsert = new GenericItemModel()
                    {
                        ItemId = string.IsNullOrEmpty(oDataToUpsert.AG_CityId) ? 0 : Convert.ToInt32(oDataToUpsert.AG_CityId),
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),                        
                        ItemName = oDataToUpsert.AG_City,
                        Enable = oDataToUpsert.Enable,
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
                    Enable = oDataToUpsert.Enable,
                });

                oCityInfo.Add(new GenericItemInfoModel()
                {
                    ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.GI_CityDirespCodeId) ? 0 : Convert.ToInt32(oDataToUpsert.GI_CityDirespCodeId.Trim()),
                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                    {
                        ItemId = (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode
                    },
                    Value = oDataToUpsert.GI_CityDirespCode,
                    Enable = oDataToUpsert.Enable,
                });

                oCityToUpsert.ItemInfo.AddRange(oCityInfo);

                //State
                oStateToUpsert = new GenericItemModel()
                {
                    ItemId = string.IsNullOrEmpty(oDataToUpsert.GIT_StateId) ? 0 : Convert.ToInt32(oDataToUpsert.GIT_StateId),
                    ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel(),
                    ItemName = oDataToUpsert.GIT_State,
                    Enable = oDataToUpsert.Enable,
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
                    Enable = oDataToUpsert.Enable,
                });

                oStateToUpsert.ItemInfo.AddRange(oStateInfo);

                GenericItemModel CountryResult = new GenericItemModel();
                GenericItemModel StateResult = new GenericItemModel();

                CountryResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(1, oCountryToUpsert);

                oStateToUpsert.ParentItem = new GenericItemModel() { ItemId = CountryResult.ItemId };
                StateResult = ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(1, oStateToUpsert);

                oCityToUpsert.ParentItem = new GenericItemModel(){ItemId = StateResult.ItemId};
                ProveedoresOnLine.Company.Controller.Company.CategoryUpsert(1, oCityToUpsert);
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
                    (SearchParam,
                    0,
                    0);
            }
            return oReturn;
        }


    }
}
