﻿using MarketPlace.Models.General;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ProjectController : BaseController
    {
        public virtual ActionResult Index(
                        string ProjectStatus,
                        string SearchParam,
                        string SearchFilter,
                        string SearchOrderType,
                        string OrderOrientation,
                        string PageNumber
                    )
        {
            MarketPlace.Models.Provider.ProviderSearchViewModel oModel = null;
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            //get basic search model

            oModel = new Models.Provider.ProviderSearchViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                SearchParam = SearchParam,
                SearchFilter = SearchFilter == null ? null : (SearchFilter.Trim(new char[] { ',' }).Length > 0 ? SearchFilter.Trim(new char[] { ',' }) : null),
                SearchOrderType = string.IsNullOrEmpty(SearchOrderType) ? MarketPlace.Models.General.enumSearchOrderType.Relevance : (MarketPlace.Models.General.enumSearchOrderType)Convert.ToInt32(SearchOrderType),
                OrderOrientation = string.IsNullOrEmpty(OrderOrientation) ? false : ((OrderOrientation.Trim().ToLower() == "1") || (OrderOrientation.Trim().ToLower() == "true")),
                PageNumber = 0,
                RelatedSearchProject = new List<Models.Project.ProjectSearchViewModel>(),
            };

            if (SessionModel.CurrentCompany != null &&
                !string.IsNullOrEmpty(SessionModel.CurrentCompany.CompanyPublicId))
            {
                //get basic search model
                int oTotalRows;
                List<ProveedoresOnLine.ProjectModule.Models.ProjectModel> oProjects =
                    ProveedoresOnLine.ProjectModule.Controller.ProjectModule.MPProjectSearch(SessionModel.CurrentCompany.CompanyPublicId,
                    SearchParam, SearchFilter, Convert.ToInt32(!string.IsNullOrEmpty(PageNumber) ? PageNumber : "0"), 100, out oTotalRows);

                if (oProjects != null && oProjects.Count > 0)
                {
                    oProjects.All(x =>
                    {
                        oModel.RelatedSearchProject.Add(new MarketPlace.Models.Project.ProjectSearchViewModel(x));
                        return true;
                    });
                }

                List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> oFilterModel = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.MPProjectSearchFilter(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    SearchParam, SearchFilter);

                if (oFilterModel != null)
                {
                    oModel.ProviderFilterResult = oFilterModel.Where(x => x.CustomerPublicId == MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId).ToList();
                }


            }
            return View(oModel);
        }

        public virtual ActionResult ProjectDetail(string ProjectPublicId)
        {
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;
            if (Request["DownloadReport"] == "true")
            {
                return File(Convert.FromBase64String(Request["ReportArray"]), Request["ReportMimeType"], Request["ReportFileName"]);
            }
            else
            {
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oCurrentProject = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.
                    ProjectGetById
                    (ProjectPublicId,
                    SessionModel.CurrentCompany.CompanyPublicId);
                MarketPlace.Models.Project.ProjectViewModel oModel = new Models.Project.ProjectViewModel(oCurrentProject);

                oModel.ProjectReportModel = new GenericReportModel();
                oModel.ProjectReportModel = Report_SelectionProcess(oModel);

                return View(oModel);
            }
        }

        public virtual ActionResult ProjectDetailRecalculate(string ProjectPublicId)
        {
            //recalculate project values
            ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectCalculate
                (ProjectPublicId);
            //return redirect to project detail
            return RedirectToRoute
                (Constants.C_Routes_Default,
                new
                {
                    controller = MVC.Project.Name,
                    action = MVC.Project.ActionNames.ProjectDetail,
                    ProjectPublicId = ProjectPublicId,
                });
        }

        public virtual ActionResult ProjectProviderDetail
            (string ProjectPublicId,
            string ProviderPublicId,
            string EvaluationAreaId)
        {
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            ProveedoresOnLine.ProjectModule.Models.ProjectModel oCurrentProject = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.
                ProjectGetByIdProviderDetail
                (ProjectPublicId,
                SessionModel.CurrentCompany.CompanyPublicId,
                ProviderPublicId);

            MarketPlace.Models.Project.ProjectViewModel oModel = new Models.Project.ProjectViewModel(oCurrentProject, ProviderPublicId);

            //get current evaluation area
            if (!string.IsNullOrEmpty(EvaluationAreaId))
            {
                oModel.RelatedProjectConfig.SetCurrentEvaluationArea(Convert.ToInt32(EvaluationAreaId.Replace(" ", "")));
            }

            if (oModel.RelatedProjectConfig.CurrentEvaluationArea == null)
            {
                oModel.RelatedProjectConfig.SetCurrentEvaluationArea(null);
            }
            /*download detail report*/
            if (Request["DownloadReport"] == "true")
            {
                string oAreaEvalResult = string.Empty, oAreaEvalUnit = string.Empty;
                decimal oAreaRatting = 0;
                string oEvalResult = string.Empty, oEvalUnit = string.Empty;
                decimal oRatting = 0;
                List<ReportParameter> parameters = new List<ReportParameter>();
                //current User
                parameters.Add(new ReportParameter("reportGeneratedBy", SessionModel.CurrentCompanyLoginUser.RelatedUser.Name.ToString()));
                //CurrentCompany
                parameters.Add(new ReportParameter("currentCompanyName", SessionModel.CurrentCompany.CompanyName));
                parameters.Add(new ReportParameter("currentCompanyTypeId", SessionModel.CurrentCompany.IdentificationType.ItemName.ToString()));
                parameters.Add(new ReportParameter("currentCompanyId", SessionModel.CurrentCompany.IdentificationNumber.ToString()));
                parameters.Add(new ReportParameter("currentCompanyLogo", SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == 203005).Select(y => y.Value).FirstOrDefault().ToString()));
                //ProviderInfo
                parameters.Add(new ReportParameter("ProviderName", oModel.CurrentProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName));
                parameters.Add(new ReportParameter("ProviderIdentification", oModel.CurrentProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName + " " + oModel.CurrentProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber));
                //Header
                parameters.Add(new ReportParameter("PJ_Name", oModel.ProjectName.ToString()));
                parameters.Add(new ReportParameter("PJ_Type", oModel.RelatedProjectConfig.ProjectConfigName.ToString()));
                parameters.Add(new ReportParameter("PJ_Date", oModel.LastModify.ToString()));
                parameters.Add(new ReportParameter("PJ_Price", oModel.ProjectAmmount.ToString("#,0.##", System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")) + " " + oModel.ProjectCurrencyTypeName.ToString()));
                parameters.Add(new ReportParameter("PJ_MinExperience", oModel.ProjectExperienceQuantity.ToString()));
                parameters.Add(new ReportParameter("PJ_InternalCodeProcess", oModel.ProjectInternalProcessNumber.ToString()));
                parameters.Add(new ReportParameter("PJ_YearsExperince", oModel.ProjectExperienceYearsValueName));

                string actividadEconomica = "";
                if (oModel.ProjectDefaultEconomicActivity != null && oModel.ProjectDefaultEconomicActivity.Count > 0)
                {
                    foreach (var dea in oModel.ProjectDefaultEconomicActivity)
                    {
                        actividadEconomica = dea.ActivityName.ToString();
                    }
                }
                else
                    if (oModel.RelatedProjectConfig.ProjectConfigExperience.CustomAcitvityEnable)
                {
                    if (oModel.ProjectCustomEconomicActivity != null && oModel.ProjectCustomEconomicActivity.Count > 0)
                    {
                        foreach (var dea in oModel.ProjectCustomEconomicActivity)
                        {
                            actividadEconomica = dea.ActivityName.ToString();
                        }
                    }
                }

                parameters.Add(new ReportParameter("PJ_ActivityName", actividadEconomica));
                string notaAdjudicacion = "";
                if (oModel.ProjectStatus == MarketPlace.Models.General.enumProjectStatus.CloseWin)
                {
                    notaAdjudicacion = oModel.ProjectAwardText.ToString();
                }
                else if (oModel.ProjectStatus == MarketPlace.Models.General.enumProjectStatus.CloseLose)
                {
                    notaAdjudicacion = oModel.ProjectCloseText.ToString();
                }

                parameters.Add(new ReportParameter("PJ_AdjudicateNote", notaAdjudicacion));
                parameters.Add(new ReportParameter("PJ_ResponsibleName", oModel.ProjectResponsible.ToString()));

                /*get all areas and get info Area*/
                List<Models.Project.EvaluationItemViewModel> oEvalItems = oModel.RelatedProjectConfig.GetEvaluationAreas();

                #region Totales

                DataTable DtTotal = new DataTable();
                DtTotal.TableName = "DS_Selection_TotalArea";
                DtTotal.Columns.Add("AreaHSEQ", typeof(string));
                DtTotal.Columns.Add("AreaTecnica", typeof(string));
                DtTotal.Columns.Add("AreaFinanciera", typeof(string));
                DtTotal.Columns.Add("AreaLegal", typeof(string));
                DtTotal.Columns.Add("Total", typeof(string));

                Decimal oTotal = 0, oTotalAreas = 0;
                DataRow oRowTotal = DtTotal.NewRow();

                foreach (var oAreaItem in oEvalItems)
                {
                    oRatting = oModel.CurrentProjectProvider.GetRatting(oAreaItem.EvaluationItemId);

                    switch (oAreaItem.EvaluationItemUnit)
                    {
                        case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                            oTotalAreas++;
                            if (oRatting >= 100)
                            {
                                oTotal += oRatting;
                                oEvalResult = oRatting + "% Pasa";
                            }
                            else
                            {
                                oTotal += 0;
                                oEvalResult = oRatting + "% No Pasa";
                            }
                            break;

                        case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:
                            oTotalAreas++;
                            if (oRatting >= oAreaItem.AprobalPercent)
                            {
                                if (oRatting >= 100)
                                {
                                    oEvalResult = "100% Pasa";
                                    oTotal += oRatting;
                                }
                                else
                                {
                                    oEvalResult = oRatting + "% Pasa";
                                    oTotal += oRatting;
                                }

                                
                            }
                            else
                            {
                                oEvalResult = oRatting + "% No Pasa";
                                oTotal += oRatting;
                            }
                            break;

                        case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                            oEvalResult = "Informativo";
                            if (oRatting >= 100)
                            {
                                oEvalResult += "Pasa";
                            }
                            else
                            {
                                oEvalResult += "No Pasa";
                            }
                            break;
                        default:
                            break;
                    }

                    if (oAreaItem.EvaluationItemName == "HSEQ") //HSEQ
                    {
                        oRowTotal["AreaHSEQ"] = oEvalResult;

                        parameters.Add(new ReportParameter("Area_HSEQ_Score", "25%"));
                        parameters.Add(new ReportParameter("Area_HSEQ_Qualification", oEvalResult));
                    }
                    else if (oAreaItem.EvaluationItemName == "TÉCNICA EXPERIENCIA") //TÉCNICA EXPERIENCIA
                    {
                        oRowTotal["AreaTecnica"] = oEvalResult;

                        parameters.Add(new ReportParameter("Area_Experience_Score", "25%"));
                        parameters.Add(new ReportParameter("Area_Experience_Qualification", oEvalResult));
                    }
                    else if (oAreaItem.EvaluationItemName == "FINANCIERO") //FINANCIERO
                    {
                        oRowTotal["AreaFinanciera"] = oEvalResult;

                        parameters.Add(new ReportParameter("Area_Financial_Score", "25%"));
                        parameters.Add(new ReportParameter("Area_Financial_Qualification", oEvalResult));
                    }
                    else if (oAreaItem.EvaluationItemName == "ASPECTOS LEGALES") //Legal
                    {
                        oRowTotal["AreaLegal"] = oEvalResult;

                        parameters.Add(new ReportParameter("Area_Legal_Score", "25%"));
                        parameters.Add(new ReportParameter("Area_Legal_Qualification", oEvalResult));
                    }
                }

                oRowTotal["Total"] = oTotal/oTotalAreas + " %";
                DtTotal.Rows.Add(oRowTotal);

                #endregion

                DataTable DtHSEQ = new DataTable();
                DataTable DtExperiences = new DataTable();
                DataTable DtFinancial = new DataTable();
                DataTable DtLegal = new DataTable();

                oEvalItems.All(Areas =>
                {
                    oModel.RelatedProjectConfig.SetCurrentEvaluationArea(Areas.EvaluationItemId);
                    oAreaRatting = oModel.CurrentProjectProvider.GetRatting(oModel.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId);

                    #region HSEQ

                    if (Areas.EvaluationItemName == "HSEQ")
                    {
                        switch (oModel.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemUnit)
                        {
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                                oAreaEvalUnit = "Pasa / No Pasa";

                                if (oAreaRatting >= 100)
                                {
                                    oAreaEvalResult = "Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = "No Pasa";
                                }
                                break;
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:

                                oAreaEvalUnit = "Mínimo " + oModel.RelatedProjectConfig.CurrentEvaluationArea.AprobalPercent.ToString("#,0.##") + "%";

                                if (oAreaRatting >= oModel.RelatedProjectConfig.CurrentEvaluationArea.AprobalPercent)
                                {
                                    oAreaEvalResult = oAreaRatting.ToString("#,0.##") + " % Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = oAreaRatting.ToString("#,0.##") + " % No Pasa";
                                }
                                break;
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                                if (oAreaRatting >= 100)
                                {
                                    oAreaEvalResult = "Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = "No Pasa";
                                }
                                break;
                            default:
                                break;
                        }
                                                
                        DtHSEQ.TableName = "DS_Selection_HSEQ";
                        DtHSEQ.Columns.Add("Criterio", typeof(string));
                        DtHSEQ.Columns.Add("Peso", typeof(string));
                        DtHSEQ.Columns.Add("Resultado", typeof(string));

                        List<MarketPlace.Models.Project.EvaluationItemViewModel> oEvaluationCriteria = oModel.RelatedProjectConfig.GetEvaluationCriteria();
                        oEvaluationCriteria.All(oCriteria =>
                        {
                            oRatting = oModel.CurrentProjectProvider.GetRatting(oCriteria.EvaluationItemId);
                            switch (oCriteria.EvaluationItemUnit)
                            {
                                case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                                    oEvalUnit = "Pasa / No Pasa";
                                    if (oRatting > oCriteria.EvaluationWeight)
                                    {
                                        oEvalResult = "Pasa";
                                    }
                                    else
                                    {
                                        oEvalResult = "No Pasa";
                                    }
                                    DataRow rowCriteria_LooseWin = DtHSEQ.NewRow();
                                    rowCriteria_LooseWin["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_LooseWin["Peso"] = oEvalUnit;
                                    rowCriteria_LooseWin["resultado"] = oEvalResult;
                                    DtHSEQ.Rows.Add(rowCriteria_LooseWin);
                                    rowCriteria_LooseWin = null;
                                    break;

                                case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:
                                    oEvalUnit = oCriteria.EvaluationWeight.ToString("#,0.##") + " %";
                                    oEvalResult = ((oRatting * oCriteria.EvaluationWeight) / 100).ToString("#,0.##") + " %";
                                    DataRow rowCriteria_Percent = DtHSEQ.NewRow();
                                    rowCriteria_Percent["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_Percent["Peso"] = oEvalUnit;
                                    rowCriteria_Percent["resultado"] = oEvalResult;
                                    DtHSEQ.Rows.Add(rowCriteria_Percent);
                                    rowCriteria_Percent = null;
                                    break;

                                case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                                    oEvalUnit = "Informativo";
                                    if (oRatting > oCriteria.EvaluationWeight)
                                    {
                                        oEvalResult = "Pasa";
                                    }
                                    else
                                    {
                                        oEvalResult = "No Pasa";
                                    }
                                    DataRow rowCriteria_Informative = DtHSEQ.NewRow();
                                    rowCriteria_Informative["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_Informative["Peso"] = oEvalUnit;
                                    rowCriteria_Informative["resultado"] = oEvalResult;
                                    DtHSEQ.Rows.Add(rowCriteria_Informative);
                                    rowCriteria_Informative = null;
                                    break;
                                default:
                                    break;
                            }

                            return true;
                        });
                    }

                    #endregion

                    #region Técnica

                    if (Areas.EvaluationItemName == "TÉCNICA EXPERIENCIA")
                    {
                        switch (oModel.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemUnit)
                        {
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                                oAreaEvalUnit = "Pasa / No Pasa";

                                if (oAreaRatting >= 100)
                                {
                                    oAreaEvalResult = "Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = "No Pasa";
                                }
                                break;
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:

                                oAreaEvalUnit = "Mínimo " + oModel.RelatedProjectConfig.CurrentEvaluationArea.AprobalPercent.ToString("#,0.##") + "%";

                                if (oAreaRatting >= oModel.RelatedProjectConfig.CurrentEvaluationArea.AprobalPercent)
                                {
                                    oAreaEvalResult = oAreaRatting.ToString("#,0.##") + " % Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = oAreaRatting.ToString("#,0.##") + " % No Pasa";
                                }
                                break;
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                                if (oAreaRatting >= 100)
                                {
                                    oAreaEvalResult = "Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = "No Pasa";
                                }
                                break;
                            default:
                                break;
                        }
                        
                        DtExperiences.TableName = "DS_Selection_Experience";
                        DtExperiences.Columns.Add("Criterio", typeof(string));
                        DtExperiences.Columns.Add("Peso", typeof(string));
                        DtExperiences.Columns.Add("Resultado", typeof(string));

                        List<MarketPlace.Models.Project.EvaluationItemViewModel> oEvaluationCriteria = oModel.RelatedProjectConfig.GetEvaluationCriteria();
                        oEvaluationCriteria.All(oCriteria =>
                        {
                            oRatting = oModel.CurrentProjectProvider.GetRatting(oCriteria.EvaluationItemId);
                            switch (oCriteria.EvaluationItemUnit)
                            {
                                case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                                    oEvalUnit = "Pasa / No Pasa";
                                    if (oRatting > oCriteria.EvaluationWeight)
                                    {
                                        oEvalResult = "Pasa";
                                    }
                                    else
                                    {
                                        oEvalResult = "No Pasa";
                                    }
                                    DataRow rowCriteria_LooseWin = DtExperiences.NewRow();
                                    rowCriteria_LooseWin["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_LooseWin["Peso"] = oEvalUnit;
                                    rowCriteria_LooseWin["resultado"] = oEvalResult;
                                    DtExperiences.Rows.Add(rowCriteria_LooseWin);
                                    rowCriteria_LooseWin = null;
                                    break;

                                case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:
                                    oEvalUnit = oCriteria.EvaluationWeight.ToString("#,0.##") + " %";
                                    oEvalResult = ((oRatting * oCriteria.EvaluationWeight) / 100).ToString("#,0.##") + " %";
                                    DataRow rowCriteria_Percent = DtExperiences.NewRow();
                                    rowCriteria_Percent["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_Percent["Peso"] = oEvalUnit;
                                    rowCriteria_Percent["resultado"] = oEvalResult;
                                    DtExperiences.Rows.Add(rowCriteria_Percent);
                                    rowCriteria_Percent = null;
                                    break;

                                case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                                    oEvalUnit = "Informativo";
                                    if (oRatting > oCriteria.EvaluationWeight)
                                    {
                                        oEvalResult = "Pasa";
                                    }
                                    else
                                    {
                                        oEvalResult = "No Pasa";
                                    }
                                    DataRow rowCriteria_Informative = DtExperiences.NewRow();
                                    rowCriteria_Informative["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_Informative["Peso"] = oEvalUnit;
                                    rowCriteria_Informative["resultado"] = oEvalResult;
                                    DtExperiences.Rows.Add(rowCriteria_Informative);
                                    rowCriteria_Informative = null;
                                    break;
                                default:
                                    break;
                            }

                            return true;
                        });
                    }

                    #endregion

                    #region Financiera

                    if (Areas.EvaluationItemName == "FINANCIERO")
                    {
                        switch (oModel.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemUnit)
                        {
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                                oAreaEvalUnit = "Pasa / No Pasa";

                                if (oAreaRatting >= 100)
                                {
                                    oAreaEvalResult = "Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = "No Pasa";
                                }
                                break;
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:

                                oAreaEvalUnit = "Mínimo " + oModel.RelatedProjectConfig.CurrentEvaluationArea.AprobalPercent.ToString("#,0.##") + "%";

                                if (oAreaRatting >= oModel.RelatedProjectConfig.CurrentEvaluationArea.AprobalPercent)
                                {
                                    oAreaEvalResult = oAreaRatting.ToString("#,0.##") + " % Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = oAreaRatting.ToString("#,0.##") + " % No Pasa";
                                }
                                break;
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                                if (oAreaRatting >= 100)
                                {
                                    oAreaEvalResult = "Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = "No Pasa";
                                }
                                break;
                            default:
                                break;
                        }
                                                
                        DtFinancial.TableName = "DS_Selection_Financial";
                        DtFinancial.Columns.Add("Criterio", typeof(string));
                        DtFinancial.Columns.Add("Peso", typeof(string));
                        DtFinancial.Columns.Add("Resultado", typeof(string));

                        List<MarketPlace.Models.Project.EvaluationItemViewModel> oEvaluationCriteria = oModel.RelatedProjectConfig.GetEvaluationCriteria();
                        oEvaluationCriteria.All(oCriteria =>
                        {
                            oRatting = oModel.CurrentProjectProvider.GetRatting(oCriteria.EvaluationItemId);
                            switch (oCriteria.EvaluationItemUnit)
                            {
                                case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                                    oEvalUnit = "Pasa / No Pasa";
                                    if (oRatting > oCriteria.EvaluationWeight)
                                    {
                                        oEvalResult = "Pasa";
                                    }
                                    else
                                    {
                                        oEvalResult = "No Pasa";
                                    }
                                    DataRow rowCriteria_LooseWin = DtFinancial.NewRow();
                                    rowCriteria_LooseWin["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_LooseWin["Peso"] = oEvalUnit;
                                    rowCriteria_LooseWin["resultado"] = oEvalResult;
                                    DtFinancial.Rows.Add(rowCriteria_LooseWin);
                                    rowCriteria_LooseWin = null;
                                    break;

                                case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:
                                    oEvalUnit = oCriteria.EvaluationWeight.ToString("#,0.##") + " %";
                                    oEvalResult = ((oRatting * oCriteria.EvaluationWeight) / 100).ToString("#,0.##") + " %";
                                    DataRow rowCriteria_Percent = DtFinancial.NewRow();
                                    rowCriteria_Percent["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_Percent["Peso"] = oEvalUnit;
                                    rowCriteria_Percent["resultado"] = oEvalResult;
                                    DtFinancial.Rows.Add(rowCriteria_Percent);
                                    rowCriteria_Percent = null;
                                    break;

                                case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                                    oEvalUnit = "Informativo";
                                    if (oRatting > oCriteria.EvaluationWeight)
                                    {
                                        oEvalResult = "Pasa";
                                    }
                                    else
                                    {
                                        oEvalResult = "No Pasa";
                                    }
                                    DataRow rowCriteria_Informative = DtFinancial.NewRow();
                                    rowCriteria_Informative["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_Informative["Peso"] = oEvalUnit;
                                    rowCriteria_Informative["resultado"] = oEvalResult;
                                    DtFinancial.Rows.Add(rowCriteria_Informative);
                                    rowCriteria_Informative = null;
                                    break;
                                default:
                                    break;
                            }

                            return true;
                        });
                    }

                    #endregion

                    #region Legal

                    if (Areas.EvaluationItemName == "ASPECTOS LEGALES")
                    {
                        switch (oModel.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemUnit)
                        {
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                                oAreaEvalUnit = "Pasa / No Pasa";

                                if (oAreaRatting >= 100)
                                {
                                    oAreaEvalResult = "Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = "No Pasa";
                                }
                                break;
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:

                                oAreaEvalUnit = "Mínimo " + oModel.RelatedProjectConfig.CurrentEvaluationArea.AprobalPercent.ToString("#,0.##") + "%";

                                if (oAreaRatting >= oModel.RelatedProjectConfig.CurrentEvaluationArea.AprobalPercent)
                                {
                                    oAreaEvalResult = oAreaRatting.ToString("#,0.##") + " % Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = oAreaRatting.ToString("#,0.##") + " % No Pasa";
                                }
                                break;
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                                if (oAreaRatting >= 100)
                                {
                                    oAreaEvalResult = "Pasa";
                                }
                                else
                                {
                                    oAreaEvalResult = "No Pasa";
                                }
                                break;
                            default:
                                break;
                        }

                        DtLegal.TableName = "DS_Selection_Legal";
                        DtLegal.Columns.Add("Criterio", typeof(string));
                        DtLegal.Columns.Add("Peso", typeof(string));
                        DtLegal.Columns.Add("Resultado", typeof(string));

                        List<MarketPlace.Models.Project.EvaluationItemViewModel> oEvaluationCriteria = oModel.RelatedProjectConfig.GetEvaluationCriteria();
                        oEvaluationCriteria.All(oCriteria =>
                        {
                            oRatting = oModel.CurrentProjectProvider.GetRatting(oCriteria.EvaluationItemId);
                            switch (oCriteria.EvaluationItemUnit)
                            {
                                case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                                    oEvalUnit = "Pasa / No Pasa";
                                    if (oRatting > oCriteria.EvaluationWeight)
                                    {
                                        oEvalResult = "Pasa";
                                    }
                                    else
                                    {
                                        oEvalResult = "No Pasa";
                                    }
                                    DataRow rowCriteria_LooseWin = DtLegal.NewRow();
                                    rowCriteria_LooseWin["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_LooseWin["Peso"] = oEvalUnit;
                                    rowCriteria_LooseWin["resultado"] = oEvalResult;
                                    DtLegal.Rows.Add(rowCriteria_LooseWin);
                                    rowCriteria_LooseWin = null;
                                    break;

                                case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:
                                    oEvalUnit = oCriteria.EvaluationWeight.ToString("#,0.##") + " %";
                                    oEvalResult = ((oRatting * oCriteria.EvaluationWeight) / 100).ToString("#,0.##") + " %";
                                    DataRow rowCriteria_Percent = DtLegal.NewRow();
                                    rowCriteria_Percent["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_Percent["Peso"] = oEvalUnit;
                                    rowCriteria_Percent["resultado"] = oEvalResult;
                                    DtLegal.Rows.Add(rowCriteria_Percent);
                                    rowCriteria_Percent = null;
                                    break;

                                case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                                    oEvalUnit = "Informativo";
                                    if (oRatting > oCriteria.EvaluationWeight)
                                    {
                                        oEvalResult = "Pasa";
                                    }
                                    else
                                    {
                                        oEvalResult = "No Pasa";
                                    }
                                    DataRow rowCriteria_Informative = DtLegal.NewRow();
                                    rowCriteria_Informative["Criterio"] = oCriteria.EvaluationItemName;
                                    rowCriteria_Informative["Peso"] = oEvalUnit;
                                    rowCriteria_Informative["resultado"] = oEvalResult;
                                    DtLegal.Rows.Add(rowCriteria_Informative);
                                    rowCriteria_Informative = null;
                                    break;
                                default:
                                    break;
                            }

                            return true;
                        });
                    }

                    #endregion

                    return true;
                });                

                /*print report*/
                Tuple<byte[], string, string> SelectionProcessReport = ProveedoresOnLine.Reports.Controller.ReportModule.PJ_SelectionProcessReportDetail(
                                                                            DtHSEQ,
                                                                            DtExperiences,
                                                                            DtFinancial,
                                                                            DtLegal,
                                                                            DtTotal,
                                                                            parameters,
                                                                            enumCategoryInfoType.PDF.ToString(),
                                                                            InternalSettings.Instance[Constants.MP_CP_ReportPath].Value.Trim()
                                                                        );
                parameters = null;
                return File(SelectionProcessReport.Item1, SelectionProcessReport.Item2, SelectionProcessReport.Item3);
            }

            return View(oModel);
        }

        #region Private Functions

        private GenericReportModel Report_SelectionProcess(MarketPlace.Models.Project.ProjectViewModel oModel)
        {
            GenericReportModel oReportModel = new GenericReportModel();

            List<ReportParameter> parameters = new List<ReportParameter>();
            //current User
            parameters.Add(new ReportParameter("reportGeneratedBy", SessionModel.CurrentCompanyLoginUser.RelatedUser.Name.ToString()));
            //CurrentCompany
            parameters.Add(new ReportParameter("currentCompanyName", SessionModel.CurrentCompany.CompanyName));
            parameters.Add(new ReportParameter("currentCompanyTypeId", SessionModel.CurrentCompany.IdentificationType.ItemName.ToString()));
            parameters.Add(new ReportParameter("currentCompanyId", SessionModel.CurrentCompany.IdentificationNumber.ToString()));
            parameters.Add(new ReportParameter("currentCompanyLogo", SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == 203005).Select(y => y.Value).FirstOrDefault().ToString()));
            //Header
            parameters.Add(new ReportParameter("PJ_Name", oModel.ProjectName.ToString()));
            parameters.Add(new ReportParameter("PJ_Type", oModel.RelatedProjectConfig.ProjectConfigName.ToString()));
            parameters.Add(new ReportParameter("PJ_Date", oModel.LastModify.ToString()));
            parameters.Add(new ReportParameter("PJ_Price", oModel.ProjectAmmount.ToString("#,0.##", System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")) + " " + oModel.ProjectCurrencyTypeName.ToString()));
            parameters.Add(new ReportParameter("PJ_MinExperience", oModel.ProjectExperienceQuantity.ToString()));
            parameters.Add(new ReportParameter("PJ_InternalCodeProcess", oModel.ProjectInternalProcessNumber.ToString()));
            parameters.Add(new ReportParameter("PJ_YearsExperince", oModel.ProjectExperienceYearsValueName));
            string actividadEconomica = "";
            if (oModel.ProjectDefaultEconomicActivity != null && oModel.ProjectDefaultEconomicActivity.Count > 0)
            {
                foreach (var dea in oModel.ProjectDefaultEconomicActivity)
                {
                    actividadEconomica = dea.ActivityName.ToString();
                }
            }
            else
                if (oModel.RelatedProjectConfig.ProjectConfigExperience.CustomAcitvityEnable)
            {
                if (oModel.ProjectCustomEconomicActivity != null && oModel.ProjectCustomEconomicActivity.Count > 0)
                {
                    foreach (var dea in oModel.ProjectCustomEconomicActivity)
                    {
                        actividadEconomica = dea.ActivityName.ToString();
                    }
                }
            }
            parameters.Add(new ReportParameter("PJ_ActivityName", actividadEconomica));
            string notaAdjudicacion = "";
            if (oModel.ProjectStatus == MarketPlace.Models.General.enumProjectStatus.CloseWin)
            {
                notaAdjudicacion = oModel.ProjectAwardText.ToString();
            }
            else if (oModel.ProjectStatus == MarketPlace.Models.General.enumProjectStatus.CloseLose)
            {
                notaAdjudicacion = oModel.ProjectCloseText.ToString();
            }
            parameters.Add(new ReportParameter("PJ_AdjudicateNote", notaAdjudicacion));
            parameters.Add(new ReportParameter("PJ_ResponsibleName", oModel.ProjectResponsible.ToString()));
            //providers:
            DataTable dtProvidersProject = new DataTable();
            dtProvidersProject.TableName = "areas";
            dtProvidersProject.Columns.Add("providerName", typeof(string));
            dtProvidersProject.Columns.Add("TypeId", typeof(string));
            dtProvidersProject.Columns.Add("providerId", typeof(string));
            dtProvidersProject.Columns.Add("estado", typeof(string));
            int areas_name = 0;
            int areas_calculate = 0;
            foreach (var oAreaItem in oModel.RelatedProjectConfig.GetEvaluationAreas())
            {
                areas_name++;
                dtProvidersProject.Columns.Add("Area_" + areas_name.ToString(), typeof(string));
                dtProvidersProject.Columns.Add("Area_Val_" + areas_name.ToString(), typeof(string));
            }
            dtProvidersProject.Columns.Add("Calc_Areas", typeof(string));

            foreach (var oProjectProvider in oModel.RelatedProjectProvider)
            {
                DataRow rowProvider = dtProvidersProject.NewRow();
                //add provider info
                rowProvider["providerName"] = oProjectProvider.RelatedProjectProvider.RelatedProvider.RelatedCompany.CompanyName.ToString();
                rowProvider["TypeId"] = oProjectProvider.RelatedProjectProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName.ToString();
                rowProvider["providerId"] = oProjectProvider.RelatedProjectProvider.RelatedProvider.RelatedCompany.IdentificationNumber.ToString();

                //add evaluation Areas
                if (oModel.RelatedProjectConfig.GetEvaluationAreas() != null && oModel.RelatedProjectConfig.GetEvaluationAreas().Count > 0)
                {
                    areas_name = 0;
                    areas_calculate = 0;
                    foreach (var oAreaItem in oModel.RelatedProjectConfig.GetEvaluationAreas())
                    {
                        areas_calculate++;
                        MarketPlace.Models.General.enumApprovalStatus? oApprovalAreaStatus = oProjectProvider.GetApprovalStatusByArea(oAreaItem.EvaluationItemId);
                        string oEvalResult = string.Empty, oEvalValue = string.Empty, oAprobate = string.Empty;
                        decimal oRatting = oProjectProvider.GetRatting(oAreaItem.EvaluationItemId);
                        switch (oAreaItem.EvaluationItemUnit)
                        {
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                                if (oRatting >= 100)
                                {
                                    oEvalValue = "100";
                                    oEvalResult = "Pasa";
                                }
                                else
                                {
                                    oEvalValue = "0";
                                    oEvalResult = "No Pasa";
                                }
                                break;

                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:
                                if (oRatting >= oAreaItem.AprobalPercent)
                                {
                                    oEvalResult = "Pasa";
                                    oEvalValue = oRatting.ToString("#,0.##");
                                }
                                else
                                {
                                    oEvalResult = "No Pasa";
                                    oEvalValue = oRatting.ToString("#,0.##");
                                }
                                break;

                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                                oEvalResult = "Informativo";
                                if (areas_calculate > 0) areas_calculate--;
                                if (oRatting >= 100)
                                {
                                    oEvalValue = "Pasa";
                                }
                                else
                                {
                                    oEvalValue = "No Pasa";
                                }
                                break;

                            default:
                                break;
                        }
                        if (oApprovalAreaStatus != null && oApprovalAreaStatus == MarketPlace.Models.General.enumApprovalStatus.Pending)
                        {
                            oAprobate = "Pendiente";
                        }
                        else if (oApprovalAreaStatus != null &&
                                oApprovalAreaStatus == MarketPlace.Models.General.enumApprovalStatus.Approved)
                        {
                            oAprobate = "Aprobado";
                        }
                        else if (oApprovalAreaStatus != null &&
                                oApprovalAreaStatus == MarketPlace.Models.General.enumApprovalStatus.Rejected)
                        {
                            oAprobate = "Rechazado";
                        }
                        areas_name++;
                        rowProvider["Area_" + areas_name.ToString()] = oEvalResult + " " + oEvalValue;// +" " + oAprobate;
                        if ( oEvalValue.Length <= 0 || oEvalValue == null ) oEvalValue = "0";
                        if (oEvalResult.CompareTo("Informativo") == 0) oEvalValue = "0";
                        rowProvider["Area_Val_" + areas_name.ToString()] = Math.Ceiling(Convert.ToDouble(oEvalValue));
                    }
                    rowProvider["Calc_Areas"] = areas_calculate.ToString();
                }
                string pj_state = "";
                MarketPlace.Models.General.enumApprovalStatus? oApprovalProviderStatus = oProjectProvider.ApprovalStatus;

                if (oModel.CurrentProjectProvider == null &&
                (oModel.ProjectStatus == MarketPlace.Models.General.enumProjectStatus.Open ||
                oModel.ProjectStatus == MarketPlace.Models.General.enumProjectStatus.OpenRefusal))
                {
                    pj_state = "Solicitar aprobación";
                }
                else if (oApprovalProviderStatus != null &&
                    oApprovalProviderStatus == MarketPlace.Models.General.enumApprovalStatus.Pending)
                {
                    pj_state = "Pendiente por aprobación";
                }
                else if (oApprovalProviderStatus != null &&
                        oApprovalProviderStatus == MarketPlace.Models.General.enumApprovalStatus.Approved)
                {
                    pj_state = "Aprobado";
                }
                else if (oApprovalProviderStatus != null &&
                        oApprovalProviderStatus == MarketPlace.Models.General.enumApprovalStatus.Rejected)
                {
                    pj_state = "Rechazado";
                }
                else if (oApprovalProviderStatus != null &&
                        oApprovalProviderStatus == MarketPlace.Models.General.enumApprovalStatus.Award)
                {
                    pj_state = "Adjudicado";
                }
                
                rowProvider["estado"] = pj_state;
                dtProvidersProject.Rows.Add(rowProvider);
            }
            Tuple<byte[], string, string> SelectionProcessReport = ProveedoresOnLine.Reports.Controller.ReportModule.PJ_SelectionProcessReport(
                dtProvidersProject,
                parameters,
                enumCategoryInfoType.PDF.ToString(),
                InternalSettings.Instance[Constants.MP_CP_ReportPath].Value.Trim()
                );
            oReportModel.File = SelectionProcessReport.Item1;
            oReportModel.MimeType = SelectionProcessReport.Item2;
            oReportModel.FileName = SelectionProcessReport.Item3;
            return oReportModel;
        }

        #endregion Private Functions
    }
}