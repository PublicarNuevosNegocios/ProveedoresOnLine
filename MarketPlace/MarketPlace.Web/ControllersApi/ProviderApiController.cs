using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
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

        #region Provider Reports
        [HttpPost]
        [HttpGet]
        public void RPSurveyFilterReport(string RPSurveyFilterReport, string ProviderPublicId)
        {
            if (RPSurveyFilterReport == "true")
            {
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
            }
        }

        #endregion

        #region Private Functions

        private ProveedoresOnLine.Company.Models.Util.GenericItemModel GetSurveyReportFilterRequest()
        {
            ProveedoresOnLine.Company.Models.Util.GenericItemModel oReturn = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
            {
                ItemId = 0,
                ItemName = "SurveyFilterReport",
                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)enumSurveyType.SurveyReport,
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

        //TODO: Acá va l función que genera el pdf
        //Recibe los parametros del reporte
        //

        #endregion
    }
}
