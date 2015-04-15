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
                //get project request
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = GetProjectUpsertRequest();

                //validate related compare
                if (oProjectToUpsert.ProjectInfo.
                        Any(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Compare &&
                                !string.IsNullOrEmpty(x.Value)))
                {
                    int oCompareId = oProjectToUpsert.ProjectInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Compare &&
                                    !string.IsNullOrEmpty(x.Value)).
                        Select(x => Convert.ToInt32(x.Value.Replace(" ", ""))).
                        DefaultIfEmpty(0).
                        FirstOrDefault();

                    ProveedoresOnLine.CompareModule.Models.CompareModel oCompare = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                            CompareGetCompanyBasicInfo
                            (oCompareId,
                            MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                            MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

                    if (oCompare != null &&
                        oCompare.RelatedProvider != null &&
                        oCompare.RelatedProvider.Count > 0)
                    {
                        //get compare providers

                        oProjectToUpsert.RelatedProjectProvider = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel>();

                        oCompare.RelatedProvider.All(prv =>
                        {
                            oProjectToUpsert.RelatedProjectProvider.Add(new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                            {
                                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                                {
                                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                                    {
                                        CompanyPublicId = prv.RelatedCompany.CompanyPublicId,
                                    }
                                },
                                Enable = true,
                            });

                            return true;
                        });
                    }

                }

                //upsert project
                oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectUpsert(oProjectToUpsert);

                return oProjectToUpsert.ProjectPublicId;
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

        #region private methods

        private ProveedoresOnLine.ProjectModule.Models.ProjectModel GetProjectUpsertRequest()
        {
            ProveedoresOnLine.ProjectModule.Models.ProjectModel oReturn = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
            {
                ProjectPublicId = System.Web.HttpContext.Current.Request["ProjectPublicId"],
                ProjectName = System.Web.HttpContext.Current.Request["ProjectName"],
                ProjectStatus = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)MarketPlace.Models.General.enumProjectStatus.Open,
                },
                RelatedProjectConfig = new ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel()
                {
                    ItemId = Convert.ToInt32(System.Web.HttpContext.Current.Request["ProjectConfig"].Replace(" ", "")),
                },
                Enable = true,
                ProjectInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            };

            //get company info
            System.Web.HttpContext.Current.Request.Form.AllKeys.Where(x => x.Contains("ProjectInfo_")).All(req =>
            {
                string[] strSplit = req.Split('_');

                if (strSplit.Length >= 3)
                {
                    oReturn.ProjectInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
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
