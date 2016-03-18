using MarketPlace.Models.Compare;
using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using MarketPlace.Models.Survey;
using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ReportController : BaseController
    {
        public virtual ActionResult Index()
        {
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
            {
                MarketPlace.Models.General.SessionModel.CurrentURL = null;
            }
            return View();
        }

        public virtual ActionResult PRGeneral()
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.ProviderMenu = GetReportControllerMenu();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

            return View(oModel);
        }

        public virtual ActionResult RP_SV_SurveyGeneralInfoReport()
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.ProviderMenu = GetReportControllerMenu();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

            if (Request["SurveyGeneralInfoReport"] == "True")
            {
                byte[] buffer = null;
                StringBuilder data = new StringBuilder();
                List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> surveyByProvider = ProveedoresOnLine.Reports.Controller.ReportModule.SurveyGetAllByCustomer(SessionModel.CurrentCompany.CompanyPublicId);
                string strSep = ";";
                data.AppendLine
                        (
                        "\"" + "TIPO EVALUACION" + "\"" + strSep +
                        "\"" + "RESPONSABLE" + "\"" + strSep +
                        "\"" + "PROVEEDOR" + "\"" + strSep +
                        "\"" + "PROYECTO" + "\"" + strSep +
                        "\"" + "OBSERVACIONES" + "\"" + strSep +
                        "\"" + "ESTADO" + "\"" + strSep +
                        "\"" + "FECHA DE ENVIO" + "\"" + strSep +
                        "\"" + "FECHA DE CADUCIDAD" + "\"" + strSep +
                        "\"" + "EVALUADORES" + "\"" + strSep +
                        "\"" + "CALIFICACION" + "\"" + strSep +
                        "\"" + "ULTIMA MODIFICACION" + "\"" + strSep +
                        "\"" + "AREA" + "\"" + strSep +
                        "\"" + "COMENTARIO AREA" + "\"" + strSep +
                        "\"" + "PREGUNTAS" + "\"" + strSep +
                        "\"" + "RESPUESTAS" + "\"" + strSep +
                        "\"" + "COMENTARIO RESPUESTA" + "\"");
                if (surveyByProvider != null && surveyByProvider.Count > 0)
                {
                    surveyByProvider.All(x =>
                    {
                        if (x != null && x.SurveyInfo.Count > 0)
                        {
                            data.AppendLine
                            (
                            "\"" + x.RelatedSurveyConfig.ItemName.ToString() + "\"" + strSep +
                            "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Responsible).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                            "\"" + x.RelatedProvider.RelatedCompany.CompanyName + "\"" + "" + strSep +
                            "\"" + "N/D" + "\"" + "" + strSep +
                            "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Comments).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                            "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).Select(a => a.ValueName).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                            "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.IssueDate).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                            "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.ExpirationDate).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                            "\"" + "N/D" + "\"" + "" + strSep +
                            "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Rating).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                            "\"" + x.LastModify.ToString("dd/MM/yyyy") + "\"" + "" + strSep +
                            "\"" + "N/D" + "\"" + "" + strSep +
                            "\"" + "N/D" + "\"" + "" + strSep +
                            "\"" + "N/D" + "\"" + "" + strSep +
                            "\"" + "N/D" + "\"" + "" + strSep +
                            "\"" + "N/D" + "\""
                            );
                            /* survey */
                            if (x.ChildSurvey != null && x.ChildSurvey.Count > 0)
                            {
                                /* areas by survey*/
                                string area_name = "N/D";
                                string description_area = "N/D";
                                string evaluator = "N/D";
                                string question = "N/D";
                                string answer_question = "N/D";
                                string description_question = "N/D";
                                string project_name = "N/D";
                                x.ChildSurvey.All(y =>
                                {
                                    if (y != null && y.ParentSurveyPublicId == x.SurveyPublicId)
                                    {
                                        y.RelatedSurveyConfig.RelatedSurveyConfigItem.All(a =>
                                        {
                                            evaluator = y.User.ToString();
                                            area_name = "N/D";
                                            project_name = "N/D";
                                            /*area*/
                                            if (a.ItemType.ItemId == (int)enumSurveyConfigItemType.EvaluationArea)
                                            {
                                                /* project name */
                                                project_name = y.SurveyInfo.Where(np=> np != null &&  np.ItemInfoType.ItemId == (int)enumSurveyInfoType.Project).Select(np=>np.ValueName).DefaultIfEmpty("N/D").FirstOrDefault();
                                                /* Description for area */
                                                area_name = a.ItemName.ToString();
                                                description_area = "N/D";
                                                y.RelatedSurveyItem.Where(b => b.RelatedSurveyConfigItem.ItemId == a.ItemId).All(c =>
                                                {
                                                    c.ItemInfo.All(da =>
                                                    {
                                                        if (da.ItemInfoType != null && da.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.Answer)
                                                        {
                                                            description_area = da.Value;
                                                        }
                                                        return true;
                                                    });
                                                    /*question by area*/
                                                    question = "N/D";
                                                    answer_question = "N/D";
                                                    description_question = "N/D";
                                                    /* get  questions by area */
                                                    y.RelatedSurveyConfig.RelatedSurveyConfigItem.All(e => {
                                                        if (e.ParentItem != null && e.ParentItem.ItemId == a.ItemId && e.ItemType.ItemId == (int)enumSurveyConfigItemType.Question)
                                                        {
                                                            /*question*/
                                                            question = e.ItemName.ToString();
                                                            /*answer by question*/
                                                            answer_question = y.RelatedSurveyConfig.RelatedSurveyConfigItem.Where(f => f.ParentItem != null && f.ParentItem.ItemId == e.ItemId && f.ItemType.ItemId == (int)enumSurveyConfigItemType.Answer).Select(f => f.ItemName).DefaultIfEmpty("N/D").FirstOrDefault();
                                                            /*get detail for question*/
                                                            y.RelatedSurveyItem.Where(r => r.RelatedSurveyConfigItem.ItemId == e.ItemId).All(g => {
                                                                description_question = g.ItemInfo.Where(h => h.ItemInfoType != null && h.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.DescriptionText).Select(h => h.Value).DefaultIfEmpty("N/D").FirstOrDefault();
                                                                /*add line to print*/
                                                                data.AppendLine(
                                                                    "\"" + x.RelatedSurveyConfig.ItemName.ToString() + "\"" + strSep +
                                                                    "\"" + x.SurveyInfo.Where(j => j.ItemInfoType.ItemId == (int)enumSurveyInfoType.Responsible).Select(j => j.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                                                                    "\"" + x.RelatedProvider.RelatedCompany.CompanyName + "\"" + "" + strSep +
                                                                    "\"" + project_name + "\"" + "" + strSep +
                                                                    "\"" + y.SurveyInfo.Where(k => k.ItemInfoType.ItemId == (int)enumSurveyInfoType.Comments).Select(k => k.Value).DefaultIfEmpty("").FirstOrDefault() + "\"" + "" + strSep +
                                                                    "\"" + y.SurveyInfo.Where(k => k.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).Select(k => k.ValueName).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                                                                    "\"" + x.SurveyInfo.Where(j => j.ItemInfoType.ItemId == (int)enumSurveyInfoType.IssueDate).Select(j => j.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                                                                    "\"" + x.SurveyInfo.Where(j => j.ItemInfoType.ItemId == (int)enumSurveyInfoType.ExpirationDate).Select(j => j.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                                                                    /*get evaluator*/
                                                                    "\"" + evaluator + "\"" + strSep +
                                                                    "\"" + x.SurveyInfo.Where(j => j.ItemInfoType.ItemId == (int)enumSurveyInfoType.Rating).Select(j => j.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + "" + strSep +
                                                                    "\"" + x.LastModify.ToString("dd/MM/yyyy") + "\"" + "" + strSep +
                                                                    /*get area name*/
                                                                    "\"" + area_name + "\"" + "" + strSep +
                                                                    "\"" + description_area + "\"" + "" + strSep +
                                                                    "\"" + question + "\"" + "" + strSep +
                                                                    "\"" + answer_question + "\"" + "" + strSep +
                                                                    "\"" + description_question + "\""
                                                                );
                                                                return true;
                                                            });
                                                        }
                                                        return true;
                                                    });
                                                    return true;
                                                });

                                            }
                                            return true;
                                        });
                                    }//FIN EVALUACIONES
                                    return true;
                                });
                            }
                        }
                        return true;
                    });
                    buffer = Encoding.Default.GetBytes(data.ToString().ToCharArray());
                }
                return File(buffer, "application/csv", "InformacionGeneral_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv");
            }
            return View(oModel);
        }

        #region Menu

        private List<GenericMenu> GetReportControllerMenu()
        {
            List<GenericMenu> oReturn = new List<GenericMenu>();

            string oCurrentController = MarketPlace.Web.Controllers.BaseController.CurrentControllerName;
            string oCurrentAction = MarketPlace.Web.Controllers.BaseController.CurrentActionName;

            #region Reports

            List<int> oCurrentModule = MarketPlace.Models.General.SessionModel.CurrentUserModules();
            List<int> oCurrentMenu = MarketPlace.Models.General.SessionModel.CurrentProviderMenu();

            if (oCurrentModule.Any(x => x == (int)MarketPlace.Models.General.enumModule.ReportsInfo) && 
                oCurrentMenu.Any(x => x == (int)MarketPlace.Models.General.enumProviderMenu.Survey))
            {
                MarketPlace.Models.General.GenericMenu oMenuAux = new GenericMenu();

                #region Survey Report
                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Evaluación de Desempeño",
                    Position = 0,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Información General
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información General",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Report.Name,
                                action = MVC.Report.ActionNames.RP_SV_SurveyGeneralInfoReport,
                            }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Report.ActionNames.RP_SV_SurveyGeneralInfoReport &&
                        oCurrentController == MVC.Report.Name),
                });
                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);
                #endregion
            }

            #endregion

            return oReturn;
        }

        #endregion
    }
}