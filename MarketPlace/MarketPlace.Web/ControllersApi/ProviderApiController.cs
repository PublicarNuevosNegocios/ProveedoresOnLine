using MarketPlace.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class ProviderApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> GITrackingInfo
            (string GITrackingInfo, string ProviderPublicId)
        {
            if (GITrackingInfo == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oTrakingInf =
                   ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCustomerProviderGetAllTracking(
                   SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

                return oTrakingInf;
            }
            return null;
        }
    }
}
