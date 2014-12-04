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
        public Dictionary<string, string> SearchGeography
            (string SearchGeography, string SearchParam, string CityId)
        {
            Dictionary<string, string> oReturn = new Dictionary<string, string>();

            if (SearchGeography == "true")
            {
                oReturn = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography
                    (SearchParam,
                    string.IsNullOrEmpty(CityId) ? null : (int?)Convert.ToInt32(CityId.Trim()),
                    0,
                    Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance
                        [BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value)).
                    ToDictionary(k => k.Country.ItemName + "," + k.State.ItemName + "," + k.City.ItemName,
                                v => v.City.ItemId);
            }
            return oReturn;
        }
    }
}
