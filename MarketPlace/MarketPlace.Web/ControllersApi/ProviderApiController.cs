using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.IO;
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
        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> GITrackingInfo
            (string GITrackingInfo, string ProviderPublicId)
        {
            if (GITrackingInfo == "true")
            {
                ProveedoresOnLine.Company.Models.Util.GenericItemModel oTrakingInf =
                   ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCustomerProviderGetAllTracking(
                   SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

                List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> oReturn = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>();

                if (oTrakingInf != null && oTrakingInf.ItemInfo.Count > 0)
                {
                    oTrakingInf.ItemInfo.All(x =>
                    {
                        TrackingDetailViewModel oTracking = new TrackingDetailViewModel();
                        oTracking = (TrackingDetailViewModel)(new System.Web.Script.Serialization.JavaScriptSerializer()).
                            Deserialize(x.LargeValue, typeof(TrackingDetailViewModel));

                        oReturn.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            LargeValue = oTracking.Description,
                            CreateDate = x.CreateDate,
                            Value = oTrakingInf.ItemType.ItemName,
                        });
                        return true;
                    });
                }
                else
                {
                    oReturn.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        Value = oTrakingInf.ItemType.ItemName,
                    });
                }
                return oReturn;
            }
            return null;
        }

        #region Providers Charts

        [HttpPost]
        [HttpGet]
        public Dictionary<string, int> GetProvidersByState(string GetProvidersByState)
        {
            //Get Charts By Module
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;


            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_ProvidersStateModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };

            //Get Providers of the Company
            oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetProvidersByState(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

            Dictionary<string, int> oReturn = new Dictionary<string, int>();

            if (oRelatedChart.GenericChartsInfoModel != null && oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    oReturn.Add(x.ItemName, x.Count);
                    return true;
                });
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public Dictionary<string, int> GetNationalProvidersByState(string GetNationalProvidersByState)
        {
            //Get Charts By Module
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;


            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_ProvidersStateModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };

            List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> nazi = new List<GenericChartsModelInfo>();
            foreach (ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo var in ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetProvidersByState(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId))
            {
                if (
                    var.ItemType.CompareTo("902001") == 0 ||
                    var.ItemType.CompareTo("902002") == 0 ||
                    var.ItemType.CompareTo("902003") == 0 ||
                    var.ItemType.CompareTo("902004") == 0 ||
                    var.ItemType.CompareTo("902005") == 0 ||
                    var.ItemType.CompareTo("902009") == 0 ||
                    var.ItemType.CompareTo("902011") == 0
                    )

                {
                    nazi.Add(var);
                }
            }
            //Get Providers of the Company
            oRelatedChart.GenericChartsInfoModel = nazi;
            nazi = null;

            Dictionary<string, int> oReturn = new Dictionary<string, int>();

            if (oRelatedChart.GenericChartsInfoModel != null && oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    oReturn.Add(x.ItemName, x.Count);
                    return true;
                });
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public Dictionary<string, int> GetAlienProvidersByState(string GetAlienProvidersByState)
        {
            //Get Charts By Module 
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;


            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_ProvidersStateModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };

            List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> alien = new List<GenericChartsModelInfo>();
            foreach (ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo var in ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetProvidersByState(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId))
            {
                if (
                    var.ItemType.CompareTo("902006") == 0 ||
                    var.ItemType.CompareTo("902007") == 0 ||
                    var.ItemType.CompareTo("902008") == 0 ||
                    var.ItemType.CompareTo("902010") == 0 ||
                    var.ItemType.CompareTo("902012") == 0
                    )
                {
                    alien.Add(var);
                }
            }
            //Get Providers of the Company
            oRelatedChart.GenericChartsInfoModel = alien;
            alien = null;

            Dictionary<string, int> oReturn = new Dictionary<string, int>();

            if (oRelatedChart.GenericChartsInfoModel != null && oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    oReturn.Add(x.ItemName, x.Count);
                    return true;
                });
            }

            return oReturn;
        }

        #endregion

        
    }
}
