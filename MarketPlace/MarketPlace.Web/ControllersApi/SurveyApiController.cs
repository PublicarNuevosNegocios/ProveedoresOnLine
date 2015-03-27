﻿using MarketPlace.Models.General;
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
                                Select(y => y.ValueName + " - ").
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault() + x.ItemName)).
                        ToList();
                }
            }

            return oReturn;
        }
    }
}