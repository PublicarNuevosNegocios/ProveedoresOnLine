using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
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

                if (oTrakingInf != null && oTrakingInf.Count > 0)
                {
                    oTrakingInf.All(x =>
                    {
                        TrackingDetailViewModel oTracking = new TrackingDetailViewModel();
                        oTracking = (TrackingDetailViewModel)(new System.Web.Script.Serialization.JavaScriptSerializer()).
                            Deserialize(x.ItemName, typeof(TrackingDetailViewModel));
                        x.ItemName = oTracking.Description;
                        return true;
                    });
                }                

                return oTrakingInf;
            }
            return null;
        }
    }
}
