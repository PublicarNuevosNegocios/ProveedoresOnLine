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
    public class SurveyApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<Tuple<int, string>> SurveyConfigSearchAC
            (string SurveyConfigSearchAC,
            string SearchParam)
        {
            List<Tuple<int, string>> oReturn = new List<Tuple<int, string>>();

            if (SurveyConfigSearchAC == "true")
            {
                List<ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel> SearchResult = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.MP_SurveyConfigSearch
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    SearchParam,
                    0,
                    Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim()));

                if (SearchResult != null && SearchResult.Count > 0)
                {
                    oReturn = SearchResult.
                        OrderBy(x => x.ItemName).
                        Select(x => new Tuple<int, string>
                            (x.ItemId,
                            x.ItemInfo.
                                Where(y => y.ItemInfoType.ItemId == (int)enumSurveyConfigInfoType.Group).
                                Select(y => string.IsNullOrEmpty(y.ValueName) ? string.Empty : y.ValueName + " - ").
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault() + x.ItemName)).
                        ToList();
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public MarketPlace.Models.Survey.SurveyViewModel SurveyUpsert
            (string SurveyUpsert)
        {
            MarketPlace.Models.Survey.SurveyViewModel oReturn = null;

            if (SurveyUpsert == "true")
            {
                ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyToUpsert = GetSurveyUpsertRequest();

                SurveyToUpsert = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyUpsert(SurveyToUpsert);

                oReturn = new Models.Survey.SurveyViewModel(SurveyToUpsert);
            }

            return oReturn;
        }

        #region PrivateMethods

        private ProveedoresOnLine.SurveyModule.Models.SurveyModel GetSurveyUpsertRequest()
        {
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oReturn = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
            {
                SurveyPublicId = System.Web.HttpContext.Current.Request["SurveyPublicId"],
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = System.Web.HttpContext.Current.Request["ProviderPublicId"],
                    }
                },
                RelatedSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel()
                {
                    ItemId = Convert.ToInt32(System.Web.HttpContext.Current.Request["SurveyConfigId"].Trim()),
                },
                Enable = true,

                SurveyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
            };

            //get company info


            System.Web.HttpContext.Current.Request.Form.AllKeys.Where(x => x.Contains("SurveyInfo_")).All(req =>
            {
                string[] strSplit = req.Split('_');

                if (strSplit.Length >= 3)
                {
                    oReturn.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = !string.IsNullOrEmpty(strSplit[2]) ? Convert.ToInt32(strSplit[2].Trim()) : 0,
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
