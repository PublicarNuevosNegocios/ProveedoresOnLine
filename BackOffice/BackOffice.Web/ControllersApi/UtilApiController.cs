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

            if (CategorySearchByGeography == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> Cities =
                    ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography
                    (string.IsNullOrEmpty(SearchParam) ? null : SearchParam,
                        string.IsNullOrEmpty(CityId) ? null : (int?)Convert.ToInt32(CityId),
                        0,
                        Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[
                            BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault
                        ].Value));

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
            else if (CategorySearchByActivity == "true" && IsDefault == "true")
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
                    EconomicActivityId = x.ItemId,
                    Name = x.ItemName,
                    Type = x.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.Type).
                        Select(y => y.Value + " - " + y.ValueName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    Group = x.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.Group).
                        Select(y => y.Value + " - " + y.ValueName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    Category = x.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.Category).
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
    }
}
