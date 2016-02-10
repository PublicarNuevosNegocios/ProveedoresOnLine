﻿using MarketPlace.Models.Company;
using MarketPlace.Models.Compare;
using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using Microsoft.Reporting.WebForms;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ThirdKnowledgeController : BaseController
    {
        public virtual ActionResult Index()
        {
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;


            return View();
        }

        public virtual ActionResult TKSingleSearch()
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.RelatedThirdKnowledge = new ThirdKnowledgeViewModel();
            List<PlanModel> oCurrentPeriodList = new List<PlanModel>();

            try
            {
                oModel.ProviderMenu = GetThirdKnowledgeControllerMenu();

                //Clean the season url saved
                if (SessionModel.CurrentURL != null)
                    SessionModel.CurrentURL = null;

                //Get The Active Plan By Customer 
                oCurrentPeriodList = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetCurrenPeriod(SessionModel.CurrentCompany.CompanyPublicId, true);

                if (oCurrentPeriodList != null && oCurrentPeriodList.Count > 0)
                {
                    oModel.RelatedThirdKnowledge.HasPlan = true;

                    //Get The Most Recently Period When Plan is More Than One
                    oModel.RelatedThirdKnowledge.CurrentPlanModel = oCurrentPeriodList.OrderByDescending(x => x.CreateDate).First();
                }
                else
                {
                    oModel.RelatedThirdKnowledge.HasPlan = false;
                }
                return View(oModel);
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
        }

        public virtual ActionResult TKMasiveSearch()
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.RelatedThirdKnowledge = new ThirdKnowledgeViewModel();
            List<PlanModel> oCurrentPeriodList = new List<PlanModel>();

            try
            {
                oModel.ProviderMenu = GetThirdKnowledgeControllerMenu();

                //Clean the season url saved
                if (SessionModel.CurrentURL != null)
                    SessionModel.CurrentURL = null;

                //Get The Active Plan By Customer 
                oCurrentPeriodList = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetCurrenPeriod(SessionModel.CurrentCompany.CompanyPublicId, true);

                if (oCurrentPeriodList != null && oCurrentPeriodList.Count > 0)
                {
                    oModel.RelatedThirdKnowledge.HasPlan = true;

                    //Get The Most Recently Period When Plan is More Than One
                    oModel.RelatedThirdKnowledge.CurrentPlanModel = oCurrentPeriodList.OrderByDescending(x => x.CreateDate).First();
                }
                else
                {
                    oModel.RelatedThirdKnowledge.HasPlan = false;
                }
                return View(oModel);
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
        }

        public virtual ActionResult TKDetailSingleSearch(string QueryBasicPublicId, string ReturnUrl)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            TDQueryInfoModel QueryDetailInfo = new TDQueryInfoModel();
            try
            {
                oModel.ProviderMenu = GetThirdKnowledgeControllerMenu();
                //Clean the season url saved
                if (SessionModel.CurrentURL != null)
                    SessionModel.CurrentURL = null;

                //Get The Active Plan By Customer 
                QueryDetailInfo = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.QueryDetailGetByBasicPublicID(QueryBasicPublicId);

                oModel.RelatedThidKnowledgeSearch = new ThirdKnowledgeViewModel(QueryDetailInfo.DetailInfo);

                if (ReturnUrl == "null")
                    oModel.RelatedThidKnowledgeSearch.ReturnUrl = ReturnUrl;

                oModel.RelatedThidKnowledgeSearch.QueryBasicPublicId = QueryBasicPublicId;

                //Get report generator
                if (Request["DownloadReport"] == "true")
                {
                    #region Set Parameters

                    List<ReportParameter> parameters = new List<ReportParameter>();

                    //Customer Info
                    parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName));
                    parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber));
                    parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName));
                    parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo));

                    //Query Detail Info
                    parameters.Add(new ReportParameter("ThirdKnowledgeText", MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.MP_TK_TextImage].Value));
                    parameters.Add(new ReportParameter("NameResult", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.NameResult) ? oModel.RelatedThidKnowledgeSearch.NameResult : "--"));
                    parameters.Add(new ReportParameter("IdentificationType", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.TypeDocument) ? oModel.RelatedThidKnowledgeSearch.TypeDocument : "--"));
                    parameters.Add(new ReportParameter("IdentificationNumber", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.IdentificationNumberResult) ? oModel.RelatedThidKnowledgeSearch.IdentificationNumberResult : "--"));
                    parameters.Add(new ReportParameter("Zone", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.Zone) ? oModel.RelatedThidKnowledgeSearch.Zone : "--"));
                    parameters.Add(new ReportParameter("Priority", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.Priority) ? oModel.RelatedThidKnowledgeSearch.Priority : "--"));
                    parameters.Add(new ReportParameter("Offence", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.Offense) ? oModel.RelatedThidKnowledgeSearch.Offense : "--"));
                    parameters.Add(new ReportParameter("Peps", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.Peps) ? oModel.RelatedThidKnowledgeSearch.Peps : "--"));
                    parameters.Add(new ReportParameter("ListName", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.ListName) ? oModel.RelatedThidKnowledgeSearch.ListName : "--"));
                    parameters.Add(new ReportParameter("Alias", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.Alias) ? oModel.RelatedThidKnowledgeSearch.Alias : "--"));
                    parameters.Add(new ReportParameter("LastUpdate", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.LastModifyDate) ? oModel.RelatedThidKnowledgeSearch.LastModifyDate : "--"));
                    parameters.Add(new ReportParameter("QueryCreateDate", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.RegisterDate) ? oModel.RelatedThidKnowledgeSearch.RegisterDate : "--"));
                    parameters.Add(new ReportParameter("Link", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.Link) ? oModel.RelatedThidKnowledgeSearch.Link : "--"));
                    parameters.Add(new ReportParameter("MoreInformation", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.MoreInfo) ? oModel.RelatedThidKnowledgeSearch.MoreInfo : "--"));

                    parameters.Add(new ReportParameter("User", SessionModel.CurrentLoginUser.Name.ToString() + " " + SessionModel.CurrentLoginUser.LastName.ToString()));
                    parameters.Add(new ReportParameter("ReportCreateDate", DateTime.Now.ToString()));
                    parameters.Add(new ReportParameter("Group", !string.IsNullOrEmpty(oModel.RelatedThidKnowledgeSearch.GroupName) ? oModel.RelatedThidKnowledgeSearch.GroupName : "--"));



                    Tuple<byte[], string, string> ThirdKnowledgeReport = ProveedoresOnLine.Reports.Controller.ReportModule.TK_QueryDetailReport(
                                                                    enumCategoryInfoType.PDF.ToString(),
                                                                    parameters,
                                                                    Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "TK_Report_ThirdKnowledgeQueryDetail.rdlc");

                    parameters = null;
                    return File(ThirdKnowledgeReport.Item1, ThirdKnowledgeReport.Item2, ThirdKnowledgeReport.Item3);

                    #endregion
                }

                return View(oModel);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public virtual ActionResult TKThirdKnowledgeSearch(
            string SearchOrderType,
            string OrderOrientation,
            string PageNumber,
            string InitDate,
            string EndDate)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.RelatedThidKnowledgeSearch = new ThirdKnowledgeViewModel();
            List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryModel> oQueryModel = new List<TDQueryModel>();

            int TotalRows;

            oQueryModel = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.ThirdKnowledgeSearch(
                SessionModel.CurrentCompany.CompanyPublicId,
                Convert.ToInt32(SearchOrderType),
                OrderOrientation == "1" ? true : false,
                Convert.ToInt32(PageNumber),
                20,
                out TotalRows);

            if (!string.IsNullOrEmpty(InitDate) && !string.IsNullOrEmpty(EndDate) &&
                oQueryModel != null && oQueryModel.Count > 0)
            {
                oQueryModel = oQueryModel.Where(x =>
                                                    Convert.ToDateTime(x.CreateDate.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(InitDate) &&
                                                    Convert.ToDateTime(x.CreateDate.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(EndDate)).
                                                    Select(x => x).ToList();

                oModel.RelatedThidKnowledgeSearch.InitDate = Convert.ToDateTime(InitDate);
                oModel.RelatedThidKnowledgeSearch.EndDate = Convert.ToDateTime(EndDate);

                oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult = oQueryModel.OrderByDescending(x => x.CreateDate).ToList();
            }
            else if (oQueryModel != null && oQueryModel.Count > 0)
            {
                oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult = oQueryModel.OrderByDescending(x => x.CreateDate).ToList(); 
            }
            else
            {
                oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult = new List<TDQueryModel>();
            }            

            oModel.ProviderMenu = GetThirdKnowledgeControllerMenu();

            return View(oModel);
        }

        public virtual ActionResult TKThirdKnowledgeDetail(
            string QueryPublicId
            , string InitDate
            , string EndDate
            , string Enable
            , string IsSuccess)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.RelatedThidKnowledgeSearch = new ThirdKnowledgeViewModel();
            oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult = new List<TDQueryModel>();
            int TotalRows = 0;
            List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryModel> oQueryResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.ThirdKnowledgeSearchByPublicId
                (SessionModel.CurrentCompany.CompanyPublicId
                , QueryPublicId
                , Enable == "1" ? true : false
                ,0//FIRST PAGE
                , Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim())
                ,out TotalRows
                );
            oModel.RelatedThidKnowledgeSearch.TotalRows = TotalRows;
            oModel.RelatedThidKnowledgeSearch.TotalPages = (int)Math.Ceiling((decimal)((decimal)oModel.RelatedThidKnowledgeSearch.TotalRows / (decimal)oModel.RelatedThidKnowledgeSearch.RowCount));
            
            oModel.RelatedThidKnowledgeSearch.StartPage = 0;
            oModel.RelatedThidKnowledgeSearch.LastPage = oModel.RelatedThidKnowledgeSearch.PagesLimit;

            if (oQueryResult != null && oQueryResult.Count > 0)
                oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult = oQueryResult;
            else if (IsSuccess == "Finalizado")
                oModel.RelatedThidKnowledgeSearch.Message = "La búsqueda no arrojó resultados.";

            if (!string.IsNullOrEmpty(InitDate) && !string.IsNullOrEmpty(EndDate))
            {
                oModel.RelatedThidKnowledgeSearch.InitDate = Convert.ToDateTime(InitDate);
                oModel.RelatedThidKnowledgeSearch.EndDate = Convert.ToDateTime(EndDate);
            }

            //Get report generator
            if (Request["DownloadReport"] == "true")
            {
                #region Set Parameters
                //Get Request
                var obbkRQB = oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult.Where(x => x.RelatedQueryBasicInfoModel != null).FirstOrDefault().RelatedQueryBasicInfoModel.Where(i => i.DetailInfo != null).FirstOrDefault();
                string searchName = "";
                string searchIdentification = "";
                if (obbkRQB.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.RequestName).Select(y => y.Value).DefaultIfEmpty(String.Empty).FirstOrDefault().ToString().Length > 0)
                {
                    searchName = obbkRQB.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.RequestName).Select(y => y.Value).DefaultIfEmpty(String.Empty).FirstOrDefault().ToString();
                }
                if (obbkRQB.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdNumberRequest).Select(y => y.Value).DefaultIfEmpty(String.Empty).FirstOrDefault().ToString().Length > 0)
                {
                    searchIdentification += obbkRQB.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdNumberRequest).Select(y => y.Value).DefaultIfEmpty(String.Empty).FirstOrDefault().ToString();
                }

                List<ReportParameter> parameters = new List<ReportParameter>();

                //Customer Info
                parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName));
                parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber));
                parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName));
                parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo));

                //Query Info
                parameters.Add(new ReportParameter("ThirdKnowledgeText", MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.MP_TK_TextImage].Value));
                parameters.Add(new ReportParameter("User", oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult.Where(x => x.User != null).Select(x => x.User).DefaultIfEmpty("No hay campo").FirstOrDefault()));
                parameters.Add(new ReportParameter("CreateDate", oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult.Where(x => x.CreateDate.AddHours(-5).ToString() != null).Select(x => x.CreateDate.ToString()).DefaultIfEmpty("No hay campo").FirstOrDefault()));
                parameters.Add(new ReportParameter("QueryType", oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult.Where(x => x.SearchType != null).Select(x => x.SearchType.ItemName).DefaultIfEmpty("No hay campo").FirstOrDefault()));
                parameters.Add(new ReportParameter("Status", oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult.Where(x => x.QueryStatus != null).Select(x => x.QueryStatus.ItemName).DefaultIfEmpty("No hay campo").FirstOrDefault()));
                parameters.Add(new ReportParameter("searchName", searchName));
                parameters.Add(new ReportParameter("searchIdentification", searchIdentification));
                parameters.Add(new ReportParameter("IsSuccess",oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult.Where(x => x != null).Select(x => x.IsSuccess).FirstOrDefault().ToString()));

                DataTable data = new DataTable();
                data.Columns.Add("Alias");
                data.Columns.Add("IdentificationResult");
                data.Columns.Add("NameResult");
                data.Columns.Add("Offense");
                data.Columns.Add("Peps");
                data.Columns.Add("Priority");
                data.Columns.Add("Status");
                data.Columns.Add("ListName");

                DataRow row;
                foreach (var query in oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult.FirstOrDefault().RelatedQueryBasicInfoModel)
                {
                    row = data.NewRow();

                    row["Alias"] = query.Alias;
                    row["IdentificationResult"] = query.IdentificationResult;
                    row["NameResult"] = query.NameResult;
                    row["Offense"] = query.Offense;
                    row["Peps"] = query.Peps;
                    row["Priority"] = query.Priority;
                    row["Status"] = query.Status == "True" ? "Activo" : "Inactivo";
                    row["ListName"] = query.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumThirdKnowledgeColls.ListName).
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    data.Rows.Add(row);
                }

                Tuple<byte[], string, string> ThirdKnowledgeReport = ProveedoresOnLine.Reports.Controller.ReportModule.TK_QueryReport(
                                                                enumCategoryInfoType.PDF.ToString(),
                                                                data,
                                                                parameters,
                                                                Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "TK_Report_ThirdKnowledgeQuery.rdlc");

                parameters = null;
                return File(ThirdKnowledgeReport.Item1, ThirdKnowledgeReport.Item2, ThirdKnowledgeReport.Item3);

                #endregion
            }

            oModel.ProviderMenu = GetThirdKnowledgeControllerMenu();


            if (oModel != null)
            {
                List<Tuple<string, List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryInfoModel>>> oGroup = new List<Tuple<string, List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryInfoModel>>>();
                List<string> Item1 = new List<string>();

                oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult.All(
                item =>
                {
                    item.RelatedQueryBasicInfoModel.All(x =>
                    {
                        Item1.Add(x.DetailInfo.Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumThirdKnowledgeColls.GroupName).Select(y => y.Value).FirstOrDefault());
                        return true;
                    });
                    Item1 = Item1.GroupBy(x => x).Select(grp => grp.First()).ToList();

                    List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryInfoModel> oItem2 = new List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryInfoModel>();
                    Tuple<string, List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryInfoModel>> oTupleItem = new Tuple<string, List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryInfoModel>>("", new List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryInfoModel>());

                    Item1.All(x =>
                    {
                        if (item.RelatedQueryBasicInfoModel.Where(td => td.DetailInfo.Any(inf => inf.Value == x)) != null)
                        {
                            oItem2 = item.RelatedQueryBasicInfoModel.Where(td => td.DetailInfo.Any(inf => inf.Value == x)).Select(td => td).ToList();
                            oTupleItem = new Tuple<string, List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryInfoModel>>(x, oItem2);
                            oGroup.Add(oTupleItem);
                        }
                        return true;
                    });
                    return true;
                });

                oModel.Group = oGroup;
            }




            return View(oModel);
        }

        #region Menu

        private List<GenericMenu> GetThirdKnowledgeControllerMenu()
        {
            List<GenericMenu> oReturn = new List<GenericMenu>();

            string oCurrentController = MarketPlace.Web.Controllers.BaseController.CurrentControllerName;
            string oCurrentAction = MarketPlace.Web.Controllers.BaseController.CurrentActionName;

            #region Menu Usuario

            MarketPlace.Models.General.GenericMenu oMenuAux = new GenericMenu();

            //header
            oMenuAux = new GenericMenu()
            {
                Name = "Menú Usuario",
                Position = 0,
                ChildMenu = new List<GenericMenu>(),
            };

            foreach (var module in SessionModel.CurrentUserModules())
            {
                if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ThirdKnowledge)
                {
                    //Consulta individual
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Consulta Individual",
                        Url = Url.RouteUrl
                                (MarketPlace.Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.ThirdKnowledge.Name,
                                    action = MVC.ThirdKnowledge.ActionNames.TKSingleSearch
                                }),
                        Position = 0,
                        IsSelected =
                            (oCurrentAction == MVC.ThirdKnowledge.ActionNames.TKSingleSearch &&
                            oCurrentController == MVC.ThirdKnowledge.Name)
                    });

                    //Consulta masiva
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Consulta Masiva",
                        Url = Url.RouteUrl
                                (MarketPlace.Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.ThirdKnowledge.Name,
                                    action = MVC.ThirdKnowledge.ActionNames.TKMasiveSearch
                                }),
                        Position = 1,
                        IsSelected =
                            (oCurrentAction == MVC.ThirdKnowledge.ActionNames.TKMasiveSearch &&
                            oCurrentController == MVC.ThirdKnowledge.Name)
                    });

                    //Consulta individual
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Mis Consultas",
                        Url = Url.RouteUrl
                                (MarketPlace.Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.ThirdKnowledge.Name,
                                    action = MVC.ThirdKnowledge.ActionNames.TKThirdKnowledgeSearch
                                }),
                        Position = 2,
                        IsSelected =
                            (oCurrentAction == MVC.ThirdKnowledge.ActionNames.TKThirdKnowledgeSearch &&
                            oCurrentController == MVC.ThirdKnowledge.Name)
                    });
                }
            }

            #endregion

            //get is selected menu
            oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

            //add menu
            oReturn.Add(oMenuAux);

            return oReturn;
        }

        #endregion
    }
}