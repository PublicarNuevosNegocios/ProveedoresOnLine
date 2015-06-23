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
        public Dictionary<string, int> GetProvidersByState
            (string GetProvidersByState)
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
        #endregion

        #region Provider Reports
        [HttpPost]
        [HttpGet]
        public void RPSurveyFilterReport(string RPSurveyFilterReport, string ProviderPublicId)
        {
            List<string> oParams = new List<string>();
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

                #region Get Basic Info
                //get basic provider info
                var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                    (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

                var oProvider = olstProvider.
                    Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                                (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                                x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                                (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                                x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                                x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                    FirstOrDefault();      
                #endregion                
                #region Set Params to Report                

                ProviderViewModel oModel = new ProviderViewModel();
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                if (oToInsert.RelatedReports != null)
                {                    
                    oToInsert.RelatedReports.All(x =>
                        {
                            oParams.Add(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                (int)MarketPlace.Models.General.enumSurveyInfoType.RP_Observation).Select(y => y.Value).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault());
                            return true;
                        });                    
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ImprovementPlan).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault());
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_InitDateReport).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault());
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_EndDateReport).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault());
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ReportAverage).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault());
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ReportDate).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault());
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ReportResponsable).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault());
                        return true;
                    });
                    //Current Company info
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(SessionModel.CurrentCompany.CompanyName);
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(SessionModel.CurrentCompany.IdentificationNumber);
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(SessionModel.CurrentCompany.IdentificationType.ItemName);
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(SessionModel.CurrentCompany_CompanyLogo);
                        return true;
                    });


                    //ProviderInfo
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(oModel.RelatedLiteProvider.ProviderLogoUrl);
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName);
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName);
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        oParams.Add(oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber);
                        return true;
                    });
                } 
                #endregion               
            }
            //return new System.Web.Mvc.FileStreamResult(ProveedoresOnLine.Company.Controller.Company.MP_SVBuildGeneralReport(oParams), "application/pdf");

            System.Web.HttpContext.Current.Session["reportStreamPdf"] = ProveedoresOnLine.Company.Controller.Company.MP_SVBuildGeneralReport(oParams);;


            
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


        #endregion
    }
}
