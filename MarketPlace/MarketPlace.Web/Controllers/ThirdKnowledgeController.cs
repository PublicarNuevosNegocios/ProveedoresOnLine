using MarketPlace.Models.Company;
using MarketPlace.Models.Compare;
using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using MarketPlace.Models.ThirdKnowledge;
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

                int oTotalRows = 0;

                //Get The Active Plan By Customer 
                QueryDetailInfo = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.QueryDetailGetByBasicPublicID(QueryBasicPublicId);

                List<TDQueryModel> oQueryModel = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.ThirdKnowledgeSearchByPublicId(
                    SessionModel.CurrentCompany.CompanyPublicId, QueryDetailInfo != null ? QueryDetailInfo.QueryPublicId : string.Empty, true, 0, 20, out oTotalRows);
                oModel.RelatedThidKnowledgeSearch = new ThirdKnowledgeViewModel(QueryDetailInfo.DetailInfo);

                if (ReturnUrl == "null")
                    oModel.RelatedThidKnowledgeSearch.ReturnUrl = ReturnUrl;

                oModel.RelatedThidKnowledgeSearch.QueryBasicPublicId = QueryBasicPublicId;
                oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult = oQueryModel;

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

                    parameters.Add(new ReportParameter("SearchName", oModel.RelatedThidKnowledgeSearch.RequestName));
                    parameters.Add(new ReportParameter("SearchIdentification", oModel.RelatedThidKnowledgeSearch.IdNumberRequest));

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

        public virtual ActionResult TKThirdKnowledgeSearch(string PageNumber, string InitDate, string EndDate, string SearchType, string Status)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.RelatedThidKnowledgeSearch = new ThirdKnowledgeViewModel();
            List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryModel> oQueryModel = new List<TDQueryModel>();

            oModel.RelatedThidKnowledgeSearch.RelatedThidKnowledgePager = new Models.ThirdKnowledge.ThirdKnowledgeSearchViewModel()
            {
                PageNumber = !string.IsNullOrEmpty(PageNumber) ? Convert.ToInt32(PageNumber) : 0,
            };
            int TotalRows = 0;
            oModel.RelatedThidKnowledgeSearch.RelatedThidKnowledgePager.PageNumber = !string.IsNullOrEmpty(PageNumber) ? Convert.ToInt32(PageNumber) : 0;

            oQueryModel = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.ThirdKnowledgeSearch(
                SessionModel.CurrentCompany.CompanyPublicId,
                !string.IsNullOrEmpty(InitDate) ? InitDate : "",
                !string.IsNullOrEmpty(EndDate) ? EndDate : "",
                oModel.RelatedThidKnowledgeSearch.RelatedThidKnowledgePager.PageNumber,
                Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim()),
                SearchType,
                Status,
                out TotalRows);

            oModel.RelatedThidKnowledgeSearch.RelatedThidKnowledgePager.TotalRows = TotalRows;

            if (oQueryModel != null && oQueryModel.Count > 0)
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

        public virtual ActionResult TKThirdKnowledgeDetail(string QueryPublicId, string PageNumber, string InitDate, string EndDate, string Enable, string IsSuccess)
        {
            int oTotalRowsAux = Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim());
            if (Request["DownloadReport"] == "true")
                oTotalRowsAux = 10000;

            ProviderViewModel oModel = new ProviderViewModel();
            oModel.RelatedThidKnowledgeSearch = new ThirdKnowledgeViewModel();
            oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult = new List<TDQueryModel>();
            oModel.RelatedThidKnowledgeSearch.RelatedThidKnowledgePager = new Models.ThirdKnowledge.ThirdKnowledgeSearchViewModel()
            {
                PageNumber = !string.IsNullOrEmpty(PageNumber) ? Convert.ToInt32(PageNumber) : 0,
            };
            int TotalRows = 0;
            List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryModel> oQueryResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.ThirdKnowledgeSearchByPublicId
                (SessionModel.CurrentCompany.CompanyPublicId
                , QueryPublicId
                , Enable == "1" ? true : false
                , oModel.RelatedThidKnowledgeSearch.RelatedThidKnowledgePager.PageNumber
                , oTotalRowsAux
                , out TotalRows);

            oModel.RelatedThidKnowledgeSearch.RelatedThidKnowledgePager.TotalRows = TotalRows;

            if (oQueryResult != null && oQueryResult.Count > 0)
                oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult = oQueryResult;
            else if (IsSuccess == "Finalizado")
                oModel.RelatedThidKnowledgeSearch.Message = "La búsqueda no arrojó resultados.";

            if (!string.IsNullOrEmpty(InitDate) && !string.IsNullOrEmpty(EndDate))
            {
                oModel.RelatedThidKnowledgeSearch.InitDate = Convert.ToDateTime(InitDate);
                oModel.RelatedThidKnowledgeSearch.EndDate = Convert.ToDateTime(EndDate);
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

                List<Tuple<string, List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryInfoModel>>> oGroupOrder = new List<Tuple<string, List<TDQueryInfoModel>>>();

                oGroupOrder.AddRange(oGroup.Where(x => x.Item1 == "LISTAS RESTRICTIVAS - Criticidad Alta"));
                oGroupOrder.AddRange(oGroup.Where(x => x.Item1 == "DELITOS E INHABILIDADES CONTRA EL ESTADO - Criticidad Media"));
                oGroupOrder.AddRange(oGroup.Where(x => x.Item1 == "LISTAS FINANCIERAS - Criticidad Media"));
                oGroupOrder.AddRange(oGroup.Where(x => x.Item1 == "LISTAS PEPS - Criticidad Baja"));
                oGroupOrder.AddRange(oGroup.Where(x => x.Item1 == "SIN COINCIDENCIAS"));
                oModel.Group = oGroupOrder;
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
                parameters.Add(new ReportParameter("IsSuccess", oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult.Where(x => x != null).Select(x => x.IsSuccess).FirstOrDefault().ToString()));

                /*data for rls*/
                DataTable data_rst = new DataTable();
                data_rst.Columns.Add("Alias");
                data_rst.Columns.Add("IdentificationResult");
                data_rst.Columns.Add("NameResult");
                data_rst.Columns.Add("Offense");
                data_rst.Columns.Add("Peps");
                data_rst.Columns.Add("Priority");
                data_rst.Columns.Add("Status");
                data_rst.Columns.Add("ListName");
                DataRow row_rst;
                List<TDQueryInfoModel> lrs = oModel.Group.Where(x => x.Item1 == "LISTAS RESTRICTIVAS - Criticidad Alta").Select(x => x.Item2).FirstOrDefault();
                if (lrs != null)
                    lrs.All(y =>
                    {
                        row_rst = data_rst.NewRow();
                        row_rst["Alias"] = y.Alias;
                        row_rst["IdentificationResult"] = y.IdentificationResult;
                        row_rst["NameResult"] = y.NameResult;
                        row_rst["Offense"] = y.Offense;
                        row_rst["Peps"] = y.Peps;
                        row_rst["Priority"] = y.Priority;
                        row_rst["Status"] = y.Status == "True" ? "Activo" : "Inactivo";
                        row_rst["ListName"] = "LISTAS RESTRICTIVAS - Criticidad Alta";
                        data_rst.Rows.Add(row_rst);
                        return true;
                    });

                /*data for dce*/
                DataTable data_dce = new DataTable();
                data_dce.Columns.Add("Alias");
                data_dce.Columns.Add("IdentificationResult");
                data_dce.Columns.Add("NameResult");
                data_dce.Columns.Add("Offense");
                data_dce.Columns.Add("Peps");
                data_dce.Columns.Add("Priority");
                data_dce.Columns.Add("Status");
                data_dce.Columns.Add("ListName");
                DataRow row_dce;
                List<TDQueryInfoModel> dce = oModel.Group.Where(x => x.Item1 == "DELITOS E INHABILIDADES CONTRA EL ESTADO - Criticidad Media").Select(x => x.Item2).FirstOrDefault();
                if (dce != null)
                    dce.All(y =>
                    {
                        row_dce = data_dce.NewRow();
                        row_dce["Alias"] = y.Alias;
                        row_dce["IdentificationResult"] = y.IdentificationResult;
                        row_dce["NameResult"] = y.NameResult;
                        row_dce["Offense"] = y.Offense;
                        row_dce["Peps"] = y.Peps;
                        row_dce["Priority"] = y.Priority;
                        row_dce["Status"] = y.Status == "True" ? "Activo" : "Inactivo";
                        row_dce["ListName"] = "DELITOS E INHABILIDADES CONTRA EL ESTADO - Criticidad Media";
                        data_dce.Rows.Add(row_dce);
                        return true;
                    });

                /*data for fnc*/
                DataTable data_fnc = new DataTable();
                data_fnc.Columns.Add("Alias");
                data_fnc.Columns.Add("IdentificationResult");
                data_fnc.Columns.Add("NameResult");
                data_fnc.Columns.Add("Offense");
                data_fnc.Columns.Add("Peps");
                data_fnc.Columns.Add("Priority");
                data_fnc.Columns.Add("Status");
                data_fnc.Columns.Add("ListName");
                DataRow row_fnc;
                List<TDQueryInfoModel> fnc = oModel.Group.Where(x => x.Item1 == "LISTAS FINANCIERAS - Criticidad Media").Select(x => x.Item2).FirstOrDefault();
                if (fnc != null)
                    fnc.All(y =>
                    {
                        row_fnc = data_fnc.NewRow();
                        row_fnc["Alias"] = y.Alias;
                        row_fnc["IdentificationResult"] = y.IdentificationResult;
                        row_fnc["NameResult"] = y.NameResult;
                        row_fnc["Offense"] = y.Offense;
                        row_fnc["Peps"] = y.Peps;
                        row_fnc["Priority"] = y.Priority;
                        row_fnc["Status"] = y.Status == "True" ? "Activo" : "Inactivo";
                        row_fnc["ListName"] = "LISTAS FINANCIERAS - Criticidad Media";
                        data_fnc.Rows.Add(row_fnc);
                        return true;
                    });

                /*data for psp*/
                DataTable data_psp = new DataTable();
                data_psp.Columns.Add("Alias");
                data_psp.Columns.Add("IdentificationResult");
                data_psp.Columns.Add("NameResult");
                data_psp.Columns.Add("Offense");
                data_psp.Columns.Add("Peps");
                data_psp.Columns.Add("Priority");
                data_psp.Columns.Add("Status");
                data_psp.Columns.Add("ListName");
                DataRow row_psp;
                List<TDQueryInfoModel> psp = oModel.Group.Where(x => x.Item1 == "LISTAS PEPS - Criticidad Baja").Select(x => x.Item2).FirstOrDefault();
                if (psp != null)
                    psp.All(y =>
                    {
                        row_psp = data_psp.NewRow();
                        row_psp["Alias"] = y.Alias;
                        row_psp["IdentificationResult"] = y.IdentificationResult;
                        row_psp["NameResult"] = y.NameResult;
                        row_psp["Offense"] = y.Offense;
                        row_psp["Peps"] = y.Peps;
                        row_psp["Priority"] = y.Priority;
                        row_psp["Status"] = y.Status == "True" ? "Activo" : "Inactivo";
                        row_psp["ListName"] = "LISTAS PEPS - Criticidad Baja";
                        data_psp.Rows.Add(row_psp);
                        return true;
                    });


                /*data for snv*/
                DataTable data_snc = new DataTable();
                data_snc.Columns.Add("IdentificationResult");
                data_snc.Columns.Add("NameResult");
                DataRow row_snc;
                List<TDQueryInfoModel> snc = oModel.Group.Where(x => x.Item1 == "SIN COINCIDENCIAS").Select(x => x.Item2).FirstOrDefault();
                if (snc != null)
                    snc.All(y =>
                    {
                        row_snc = data_snc.NewRow();
                        row_snc["IdentificationResult"] = y.DetailInfo.Where(x => x.ItemInfoType != null && x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumThirdKnowledgeColls.IdNumberRequest).Select(x => x.Value).FirstOrDefault().ToString();
                        row_snc["NameResult"] = y.DetailInfo.Where(x => x.ItemInfoType != null && x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumThirdKnowledgeColls.RequestName).Select(x => x.Value).FirstOrDefault().ToString();
                        data_snc.Rows.Add(row_snc);
                        return true;
                    });

                Tuple<byte[], string, string> ThirdKnowledgeReport = ProveedoresOnLine.Reports.Controller.ReportModule.TK_QueryReport(
                                                                enumCategoryInfoType.Excel.ToString(),
                                                                data_rst,
                                                                data_dce,
                                                                data_fnc,
                                                                data_psp,
                                                                data_snc,
                                                                parameters,
                                                                Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "TK_Report_ThirdKnowledgeQuery.rdlc");
                parameters = null;
                return File(ThirdKnowledgeReport.Item1, ThirdKnowledgeReport.Item2, ThirdKnowledgeReport.Item3);

                #endregion
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