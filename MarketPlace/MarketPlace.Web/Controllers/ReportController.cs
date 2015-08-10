using MarketPlace.Models.Compare;
using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
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
                        "\"" + "Tipo Evaluacion" + "\"" + strSep +
                        "\"" + "Responsable" + "\"" + strSep +
                        "\"" + "Proyecto" + "\"" + strSep +
                        "\"" + "Observaciones" + "\"" + strSep +
                        "\"" + "Estado" + "\"" + strSep +
                        "\"" + "Fecha Asignación" + "\"" + strSep +
                        "\"" + "Fecha Vencimiento" + "\"" + strSep +
                        "\"" + "Evaluadores" + "\"" + strSep +
                        "\"" + "Calificación" + "\"" + strSep +
                        "\"" + "Última Modificación" + "\"");
                surveyByProvider.All(x =>
                {
                    if (x != null && x.SurveyInfo.Count > 0)
                    {
                        data.AppendLine
                        (
                        "\"" + x.RelatedSurveyConfig.ItemName.ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Responsible).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Project).Select(a => a.ValueName).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Comments).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).Select(a => a.ValueName).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.StartDate).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.EndDate).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + "N/D" + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Rating).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + "N/D" + "\""
                        );
                        if (x.ChildSurvey != null && x.ChildSurvey.Count > 0)
                        {
                            x.ChildSurvey.All(y =>
                            {
                                if (y != null && y.ParentSurveyPublicId == x.SurveyPublicId)
                                {
                                    data.AppendLine
                                      (
                                        "\"" + x.RelatedSurveyConfig.ItemName.ToString() + "\"" + strSep +
                                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Responsible).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Project).Select(a => a.ValueName).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                                        "\"" + y.SurveyInfo.Where(b => b.ItemInfoType.ItemId == (int)enumSurveyInfoType.Comments).Select(b => b.Value).DefaultIfEmpty("").FirstOrDefault() + "\"" + strSep +
                                        "\"" + y.SurveyInfo.Where(b => b.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).Select(b => b.ValueName).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.StartDate).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.EndDate).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                                       "\"" + y.User.ToString() + "\"" + strSep +
                                       "\"" + y.SurveyInfo.Where(b => b.ItemInfoType.ItemId == (int)enumSurveyInfoType.Rating).Select(b => b.Value).DefaultIfEmpty("").FirstOrDefault() + "\"" + strSep +
                                       "\"" + y.LastModify.ToString("dd/MM/yyyy") + "\""
                                      );
                                }
                                return true;
                            });
                        }
                    }
                    return true;
                });
                buffer = Encoding.ASCII.GetBytes(data.ToString().ToCharArray());
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

            foreach (var module in MarketPlace.Models.General.SessionModel.CurrentUserModules())
            {
                MarketPlace.Models.General.GenericMenu oMenuAux = new GenericMenu();

                if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ProviderDetail)
                {
                    #region Provider Report Menu
                    //header
                    oMenuAux = new GenericMenu()
                    {
                        Name = "Proveedores",
                        Position = 0,
                        ChildMenu = new List<GenericMenu>(),
                    };

                    //Gerencial
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Informe Gerencial",
                        Url = Url.RouteUrl
                                (MarketPlace.Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.Report.Name,
                                    action = MVC.Report.ActionNames.PRGeneral
                                }),
                        Position = 0,
                        IsSelected =
                            (oCurrentAction == MVC.Report.ActionNames.PRGeneral &&
                            oCurrentController == MVC.Report.Name)
                    });

                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    oReturn.Add(oMenuAux);

                    #endregion
                }
                if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ProviderRatingCreate)
                {
                    #region Survey Report
                    //header
                    oMenuAux = new Models.General.GenericMenu()
                    {
                        Name = "Evaluación de Desempeño",
                        Position = 1,
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
            }

            #endregion

            return oReturn;
        }

        #endregion
    }
}