using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class ProjectApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public string ProjectUpsert
            (string ProjectUpsert)
        {
            if (ProjectUpsert == "true")
            {
                //ProveedoresOnLine.CompareModule.Models.CompareModel oCompareToUpsert = new ProveedoresOnLine.CompareModule.Models.CompareModel()
                //{
                //    CompareId = string.IsNullOrEmpty(CompareId) ? 0 : Convert.ToInt32(CompareId),
                //    CompareName = CompareName,
                //    User = MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                //    Enable = true,
                //};

                //if (!string.IsNullOrEmpty(ProviderPublicId))
                //{
                //    oCompareToUpsert.RelatedProvider = new List<ProveedoresOnLine.CompareModule.Models.CompareCompanyModel>()
                //    {
                //        new ProveedoresOnLine.CompareModule.Models.CompareCompanyModel()
                //        {
                //            Enable = true,
                //            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                //            {
                //                CompanyPublicId = ProviderPublicId,
                //            },
                //        },
                //    };
                //}

                //oCompareToUpsert = ProveedoresOnLine.CompareModule.Controller.CompareModule.CompareUpsert(oCompareToUpsert);

                //return oCompareToUpsert.CompareId;
            }

            return string.Empty;
        }

        [HttpPost]
        [HttpGet]
        public bool ProjectAddCompany
            (string ProjectAddCompany,
            string ProjectPublicId,
            string ProviderPublicId)
        {
            if (ProjectAddCompany == "true")
            {
                //ProveedoresOnLine.CompareModule.Models.CompareModel oCompareToUpsert = new ProveedoresOnLine.CompareModule.Models.CompareModel()
                //{
                //    CompareId = Convert.ToInt32(CompareId),
                //    RelatedProvider = new List<ProveedoresOnLine.CompareModule.Models.CompareCompanyModel>() 
                //    { 
                //        new ProveedoresOnLine.CompareModule.Models.CompareCompanyModel()
                //        {
                //            Enable = true,
                //            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                //            {
                //                CompanyPublicId = ProviderPublicId,
                //            },
                //        }, 
                //    }
                //};

                //ProveedoresOnLine.CompareModule.Controller.CompareModule.CompareCompanyUpsert(oCompareToUpsert);

                //return true;
            }

            return false;
        }

        [HttpPost]
        [HttpGet]
        public bool ProjectRemoveCompany
            (string ProjectRemoveCompany,
            string ProjectPublicId,
            string ProviderPublicId)
        {
            if (ProjectRemoveCompany == "true")
            {
                //ProveedoresOnLine.CompareModule.Models.CompareModel oCompareToUpsert = new ProveedoresOnLine.CompareModule.Models.CompareModel()
                //{
                //    CompareId = Convert.ToInt32(CompareId),
                //    RelatedProvider = new List<ProveedoresOnLine.CompareModule.Models.CompareCompanyModel>() 
                //    { 
                //        new ProveedoresOnLine.CompareModule.Models.CompareCompanyModel()
                //        {
                //            Enable = false,
                //            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                //            {
                //                CompanyPublicId = ProviderPublicId,
                //            },
                //        }, 
                //    }
                //};

                //ProveedoresOnLine.CompareModule.Controller.CompareModule.CompareCompanyUpsert(oCompareToUpsert);

                //return true;
            }

            return false;
        }

        [HttpPost]
        [HttpGet]
        public List<MarketPlace.Models.Compare.CompareViewModel> ProjectSearch
            (string ProjectSearch,
            string SearchParam,
            string PageNumber,
            string RowCount)
        {
            if (ProjectSearch == "true")
            {
                //int oTotalRows;

                //List<ProveedoresOnLine.CompareModule.Models.CompareModel> lstCompare = ProveedoresOnLine.CompareModule.Controller.CompareModule.CompareSearch
                //    (SearchParam,
                //    MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                //    string.IsNullOrEmpty(PageNumber) ? 0 : Convert.ToInt32(PageNumber.Replace(" ", "")),
                //    string.IsNullOrEmpty(RowCount) ? 0 : Convert.ToInt32(RowCount.Replace(" ", "")),
                //    out oTotalRows);

                //List<MarketPlace.Models.Compare.CompareViewModel> oReturn = new List<Models.Compare.CompareViewModel>();

                //if (lstCompare != null && lstCompare.Count > 0)
                //{
                //    lstCompare.All(x =>
                //    {
                //        oReturn.Add(new Models.Compare.CompareViewModel(x) { TotalRows = oTotalRows });

                //        return true;
                //    });
                //}

                //return oReturn;
            }
            return null;
        }

    }
}
