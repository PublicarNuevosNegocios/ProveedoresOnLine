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
