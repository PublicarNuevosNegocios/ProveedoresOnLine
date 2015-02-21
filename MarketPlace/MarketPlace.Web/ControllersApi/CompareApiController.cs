using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class CompareApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public int CMCompareUpsert
            (string CMCompareUpsert,
            string CompareId,
            string CompareName,
            string ProviderPublicId)
        {
            if (CMCompareUpsert == "true")
            {
                ProveedoresOnLine.CompareModule.Models.CompareModel oCompareToUpsert = new ProveedoresOnLine.CompareModule.Models.CompareModel()
                {
                    CompareId = string.IsNullOrEmpty(CompareId) ? 0 : Convert.ToInt32(CompareId),
                    CompareName = CompareName,
                    User = MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                    Enable = true,
                };

                if (!string.IsNullOrEmpty(ProviderPublicId))
                {
                    oCompareToUpsert.RelatedProvider = new List<ProveedoresOnLine.CompareModule.Models.CompareCompanyModel>()
                    {
                        new ProveedoresOnLine.CompareModule.Models.CompareCompanyModel()
                        {
                            Enable = true,
                            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = ProviderPublicId,
                            },
                        },
                    };
                }

                oCompareToUpsert = ProveedoresOnLine.CompareModule.Controller.CompareModule.CompareUpsert(oCompareToUpsert);

                return oCompareToUpsert.CompareId;
            }

            return 0;
        }

        [HttpPost]
        [HttpGet]
        public MarketPlace.Models.Compare.CompareViewModel CMCompareAddCompany
            (string CMCompareAddCompany,
            string CompareId,
            string ProviderPublicId)
        {
            if (CMCompareAddCompany == "true")
            {
                ProveedoresOnLine.CompareModule.Models.CompareModel oCompareToUpsert = new ProveedoresOnLine.CompareModule.Models.CompareModel()
                {
                    CompareId = Convert.ToInt32(CompareId),
                    RelatedProvider = new List<ProveedoresOnLine.CompareModule.Models.CompareCompanyModel>() 
                    { 
                        new ProveedoresOnLine.CompareModule.Models.CompareCompanyModel()
                        {
                            Enable = true,
                            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = ProviderPublicId,
                            },
                        }, 
                    }
                };

                ProveedoresOnLine.CompareModule.Controller.CompareModule.CompareCompanyUpsert(oCompareToUpsert);

                oCompareToUpsert = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                                    CompareGetCompanyBasicInfo
                                    (Convert.ToInt32(CompareId),
                                    MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email);

                MarketPlace.Models.Compare.CompareViewModel oReturn = new Models.Compare.CompareViewModel(oCompareToUpsert);

                return oReturn;
            }

            return null;
        }


        [HttpPost]
        [HttpGet]
        public MarketPlace.Models.Compare.CompareViewModel CMCompareGet
            (string CMCompareGet,
            string CompareId)
        {
            if (CMCompareGet == "true")
            {
                ProveedoresOnLine.CompareModule.Models.CompareModel oCompareResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                    CompareGetCompanyBasicInfo
                    (Convert.ToInt32(CompareId),
                    MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email);

                MarketPlace.Models.Compare.CompareViewModel oReturn = new Models.Compare.CompareViewModel(oCompareResult);

                return oReturn;
            }
            return null;
        }

        [HttpPost]
        [HttpGet]
        public List<MarketPlace.Models.Compare.CompareViewModel> CMCompareSearch
            (string CMCompareSearch,
            string SearchParam,
            string PageNumber,
            string RowCount)
        {
            if (CMCompareSearch == "true")
            {
                //ProveedoresOnLine.CompareModule.Models.CompareModel oCompareResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                //    CompareGetCompanyBasicInfo
                //    (Convert.ToInt32(CompareId),
                //    MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email);

                //MarketPlace.Models.Compare.CompareViewModel oReturn = new Models.Compare.CompareViewModel(oCompareResult);

                //return oReturn;
            }
            return null;
        }
    }
}
