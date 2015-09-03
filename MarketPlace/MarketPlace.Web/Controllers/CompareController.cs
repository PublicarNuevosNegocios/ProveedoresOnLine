using MarketPlace.Models.Compare;
using MarketPlace.Models.General;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class CompareController : BaseController
    {
        public virtual ActionResult Index()
        {
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;
            return View();
        }

        public virtual ActionResult CompareDetail (string CompareId, string CompareType, string Currency, string Year) {
            //Clean the season url saved
            if (SessionModel.CurrentURL != null)
                SessionModel.CurrentURL = null;

            if (Request["DownloadReport"] == "true")
            {
                return File(Convert.FromBase64String(Request["File"]), Request["MimeType"], Request["FileName"]);
            }
            else
            {
                //get compare info
                ProveedoresOnLine.CompareModule.Models.CompareModel oCompareResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.CompareGetDetailByType(
                string.IsNullOrEmpty(CompareType) ? (int)enumCompareType.Commercial : Convert.ToInt32(CompareType.Replace(" ", "")),
                Convert.ToInt32(CompareId.Replace(" ", "")),
                SessionModel.CurrentLoginUser.Email,
                string.IsNullOrEmpty(Year) ? null : (int?)Convert.ToInt32(Year),
                SessionModel.CurrentCompany.CompanyPublicId);

                CompareDetailViewModel oModel = new CompareDetailViewModel(oCompareResult);

                oModel.CompareCurrency = !string.IsNullOrEmpty(Currency) ?
                    Convert.ToInt32(Currency.Replace(" ", "")) :
                    Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_COP].Value);
                oModel.Year = string.IsNullOrEmpty(Year) ? null : (int?)Convert.ToInt32(Year);
                oModel.CompareType = string.IsNullOrEmpty(CompareType) ? enumCompareType.Commercial : (enumCompareType)Convert.ToInt32(CompareType.Replace(" ", ""));

                oModel.CompareMenu = GetCompareMenu(oModel);
                //oModel.CompareReportModel = Report_CompareDetail(oModel);

                return View(oModel);
            }
                
        }

        #region Menu

        private List<GenericMenu> GetCompareMenu(CompareDetailViewModel vCompareInfo)
        {
            List<GenericMenu> oReturn = new List<GenericMenu>();

            if (vCompareInfo != null && vCompareInfo.RelatedCompare != null && vCompareInfo.RelatedCompare.CompareId > 0)
            {
                //header
                MarketPlace.Models.General.GenericMenu oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Comparar por:",
                    Position = 0,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                #region Commercial Info

                //header experience
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información comercial",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Compare.Name,
                                action = MVC.Compare.ActionNames.CompareDetail,
                                CompareId = vCompareInfo.RelatedCompare.CompareId,
                                CompareType = ((int)enumCompareType.Commercial).ToString(),
                                Currency = vCompareInfo.CompareCurrency,
                                Year = vCompareInfo.Year,
                            }),
                    Position = 0,
                    IsSelected = vCompareInfo.CompareType == enumCompareType.Commercial,
                });

                #endregion Commercial Info

                #region HSEQ Info

                //header HSEQ
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Certificaciones",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Compare.Name,
                                action = MVC.Compare.ActionNames.CompareDetail,
                                CompareId = vCompareInfo.RelatedCompare.CompareId,
                                CompareType = ((int)enumCompareType.Certifications).ToString(),
                                Currency = vCompareInfo.CompareCurrency,
                                Year = vCompareInfo.Year,
                            }),
                    Position = 1,
                    IsSelected = vCompareInfo.CompareType == enumCompareType.Certifications,
                });

                #endregion HSEQ Info

                #region Financial Info

                //header Balancesheet
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información financiera",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Compare.Name,
                                action = MVC.Compare.ActionNames.CompareDetail,
                                CompareId = vCompareInfo.RelatedCompare.CompareId,
                                CompareType = ((int)enumCompareType.Financial).ToString(),
                                Currency = vCompareInfo.CompareCurrency,
                                Year = vCompareInfo.Year,
                            }),
                    Position = 2,
                    IsSelected = vCompareInfo.CompareType == enumCompareType.Financial,
                });

                #endregion Financial Info

                //add menu
                oReturn.Add(oMenuAux);
            }
            return oReturn;
        }

        #endregion Menu

        #region Pivate Functions
        private GenericReportModel Report_CompareDetail(CompareDetailViewModel oModel)
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            GenericReportModel oReporModel = new GenericReportModel();

            //CustomerInfo
            parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName));
            parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber));
            parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo));
            ////ProviderInfo
            //parameters.Add(new ReportParameter("ProviderName", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName));
            //parameters.Add(new ReportParameter("ProviderIdentificationType", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName));
            //parameters.Add(new ReportParameter("ProviderIdentificationNumber", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber));

            ////SurveyInfo
            //parameters.Add(new ReportParameter("SurveyConfigName", oModel.RelatedSurvey.SurveyConfigName));
            //parameters.Add(new ReportParameter("SurveyRating", oModel.RelatedSurvey.SurveyRating.ToString()));
            //parameters.Add(new ReportParameter("SurveyStatusName", oModel.RelatedSurvey.SurveyStatusName));
            //parameters.Add(new ReportParameter("SurveyIssueDate", oModel.RelatedSurvey.SurveyIssueDate));
            //parameters.Add(new ReportParameter("SurveyEvaluator", oModel.RelatedSurvey.SurveyEvaluator));
            //parameters.Add(new ReportParameter("SurveyLastModify", oModel.RelatedSurvey.SurveyLastModify));
            //parameters.Add(new ReportParameter("SurveyResponsible", oModel.RelatedSurvey.SurveyResponsible));
            //parameters.Add(new ReportParameter("SurveyAverage", oModel.RelatedSurvey.Average.ToString()));

            //if (oModel.RelatedSurvey.SurveyRelatedProject == null)
            //{
            //    parameters.Add(new ReportParameter("SurveyRelatedProject", "NA"));
            //}
            //else
            //{
            //    parameters.Add(new ReportParameter("SurveyRelatedProject", oModel.RelatedSurvey.SurveyRelatedProject));
            //}

            //DataTable data = new DataTable();
            //data.Columns.Add("Area");
            //data.Columns.Add("Question");
            //data.Columns.Add("Answer");
            //data.Columns.Add("QuestionRating");
            //data.Columns.Add("QuestionWeight");
            //data.Columns.Add("QuestionDescription");

            //DataRow row;
            //foreach (var EvaluationArea in
            //            oModel.RelatedSurvey.GetSurveyConfigItem(MarketPlace.Models.General.enumSurveyConfigItemType.EvaluationArea, null))
            //{
            //    var lstQuestion = oModel.RelatedSurvey.GetSurveyConfigItem
            //        (MarketPlace.Models.General.enumSurveyConfigItemType.Question, EvaluationArea.SurveyConfigItemId);

            //    foreach (var Question in lstQuestion)
            //    {
            //        if (Question.QuestionType != "118002")
            //        {
            //            row = data.NewRow();
            //            row["Area"] = EvaluationArea.Name;
            //            row["Question"] = Question.Order + " " + Question.Name;

            //            var QuestionInfo = oModel.RelatedSurvey.GetSurveyItem(Question.SurveyConfigItemId);
            //            var lstAnswer = oModel.RelatedSurvey.GetSurveyConfigItem
            //                (MarketPlace.Models.General.enumSurveyConfigItemType.Answer, Question.SurveyConfigItemId);

            //            foreach (var Answer in lstAnswer)
            //            {
            //                if (QuestionInfo != null && QuestionInfo.Answer == Answer.SurveyConfigItemId)
            //                {
            //                    row["Answer"] = Answer.Name;
            //                }
            //            }

            //            if (string.IsNullOrEmpty(row["Answer"].ToString()))
            //            {
            //                row["Answer"] = "Sin Responder";
            //                row["QuestionRating"] = "NA";
            //            }
            //            else
            //            {
            //                row["QuestionRating"] = QuestionInfo.Ratting;
            //            }

            //            row["QuestionWeight"] = Question.Weight;

            //            if (QuestionInfo != null && QuestionInfo.DescriptionText != null)
            //            {
            //                row["QuestionDescription"] = QuestionInfo.DescriptionText;
            //            }
            //            else
            //            {
            //                row["QuestionDescription"] = "";
            //            }
            //            data.Rows.Add(row);
            //        }
            //    }
            //}
            //Tuple<byte[], string, string> EvaluatorDetailReport = ProveedoresOnLine.Reports.Controller.ReportModule.SV_EvaluatorDetailReport(
            //                                                enumCategoryInfoType.PDF.ToString(),
            //                                                data,
            //                                                parameters,
            //                                                Models.General.InternalSettings.Instance[Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "SV_Report_EvaluatorDetail.rdlc");

            //oReporModel.File = EvaluatorDetailReport.Item1;
            //oReporModel.MimeType = EvaluatorDetailReport.Item2;
            //oReporModel.FileName = EvaluatorDetailReport.Item3;

            return oReporModel;
        }
        #endregion Pivate Functions
    }
}