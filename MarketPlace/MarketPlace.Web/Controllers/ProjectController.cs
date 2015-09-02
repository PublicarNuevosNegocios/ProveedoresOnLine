using MarketPlace.Models.General;
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
                    ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectSearch(SessionModel.CurrentCompany.CompanyPublicId,
                    SearchParam, null, Convert.ToInt32(!string.IsNullOrEmpty(PageNumber) ? PageNumber : "0"), 100, out oTotalRows);

                if (oProjects != null && oProjects.Count > 0)
                {
                    oProjects.All(x =>
                    {
                        oModel.RelatedSearchProject.Add(new MarketPlace.Models.Project.ProjectSearchViewModel(x));
                        return true;
                    });
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
            foreach (var oAreaItem in oModel.RelatedProjectConfig.GetEvaluationAreas())
            {
                areas_name++;
                dtProvidersProject.Columns.Add("Area_" + areas_name.ToString(), typeof(string));
            }
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
                    foreach (var oAreaItem in oModel.RelatedProjectConfig.GetEvaluationAreas())
                    {
                        MarketPlace.Models.General.enumApprovalStatus? oApprovalAreaStatus = oProjectProvider.GetApprovalStatusByArea(oAreaItem.EvaluationItemId);
                        string oEvalResult = string.Empty, oEvalValue = string.Empty, oAprobate = string.Empty;
                        decimal oRatting = oProjectProvider.GetRatting(oAreaItem.EvaluationItemId);
                        switch (oAreaItem.EvaluationItemUnit)
                        {
                            case MarketPlace.Models.General.enumEvaluationItemUnitType.LooseWin:
                                if (oRatting >= 100)
                                {
                                    oEvalResult = "Pasa";
                                }
                                else
                                {
                                    oEvalResult = "No Pasa";
                                }
                                break;

                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Percent:
                                if (oRatting >= oAreaItem.AprobalPercent)
                                {
                                    oEvalResult = "Pasa";
                                    oEvalValue = oRatting.ToString("#,0.##") + " %";
                                }
                                else
                                {
                                    oEvalResult = "No Pasa";
                                    oEvalValue = oRatting.ToString("#,0.##") + " %";
                                }
                                break;

                            case MarketPlace.Models.General.enumEvaluationItemUnitType.Informative:
                                oEvalResult = "Informativo";
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
                        rowProvider["Area_" + areas_name.ToString()] = oEvalResult + " " + oEvalValue + " " + oAprobate;
                    }
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