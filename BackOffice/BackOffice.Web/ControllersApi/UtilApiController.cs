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
        public List<ProveedoresOnLine.Company.Models.Util.GeographyModel> SearchGeography
            (string SearchGeography, string SearchParam)
        {
            List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oReturn = new List<ProveedoresOnLine.Company.Models.Util.GeographyModel>();

            if (SearchGeography == "true")
            {
                oReturn = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography
                    (SearchParam,
                    0,
                    Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance
                        [BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value));
            }
            return oReturn;
        }
    }
}
