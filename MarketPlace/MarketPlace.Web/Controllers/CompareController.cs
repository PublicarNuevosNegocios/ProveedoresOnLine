using MarketPlace.Models.Compare;
using MarketPlace.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class CompareController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        #region Commercial Info

        public virtual ActionResult CIExperiencesCompare(string CompareId, string Currency)
        {
            //get compare info
            ProveedoresOnLine.CompareModule.Models.CompareModel oCompareResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                CompareGetDetailByType
                ((int)enumCompareType.Commercial,
                Convert.ToInt32(CompareId.Replace(" ", "")),
                MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                null,
                MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

            MarketPlace.Models.Compare.CompareViewModel oModel = new Models.Compare.CompareViewModel(oCompareResult);
            oModel.CompareCurrency = !string.IsNullOrEmpty(Currency) ?
                Convert.ToInt32(Currency.Replace(" ", "")) :
                Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value);
            oModel.Year = null;
            oModel.CompareType = enumCompareType.Commercial;

            oModel.GetCompareDetail();

            oModel.CompareMenu = GetCompareMenu(oModel);

            return View(oModel);
        }

        #endregion

        #region HSEQ Info

        public virtual ActionResult HIHSECompare(string CompareId, string Currency)
        {
            //get compare info
            ProveedoresOnLine.CompareModule.Models.CompareModel oCompareResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                CompareGetDetailByType
                ((int)enumCompareType.Certifications,
                Convert.ToInt32(CompareId.Replace(" ", "")),
                MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                null,
                MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

            MarketPlace.Models.Compare.CompareViewModel oModel = new Models.Compare.CompareViewModel(oCompareResult);
            oModel.CompareCurrency = !string.IsNullOrEmpty(Currency) ?
                Convert.ToInt32(Currency.Replace(" ", "")) :
                Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value);

            oModel.Year = null;
            oModel.CompareType = enumCompareType.Certifications;

            oModel.GetCompareDetail();

            oModel.CompareMenu = GetCompareMenu(oModel);

            return View(oModel);
        }

        #endregion

        #region Financial Info

        public virtual ActionResult FIBalanceSheetInfoCompare(string CompareId, string Currency, string Year)
        {
            //get compare info
            ProveedoresOnLine.CompareModule.Models.CompareModel oCompareResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                CompareGetDetailByType
                ((int)enumCompareType.Financial,
                Convert.ToInt32(CompareId.Replace(" ", "")),
                MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                string.IsNullOrEmpty(Year) ? null : (int?)Convert.ToInt32(Year.Replace(" ", "")),
                MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

            MarketPlace.Models.Compare.CompareViewModel oModel = new Models.Compare.CompareViewModel(oCompareResult);
            oModel.CompareCurrency = !string.IsNullOrEmpty(Currency) ?
                Convert.ToInt32(Currency.Replace(" ", "")) :
                Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value);

            oModel.Year = string.IsNullOrEmpty(Year) ? null : (int?)Convert.ToInt32(Year.Replace(" ", ""));
            oModel.CompareType = enumCompareType.Financial;

            oModel.GetCompareDetail();

            oModel.CompareMenu = GetCompareMenu(oModel);

            return View(oModel);
        }

        #endregion

        #region Menu

        private List<GenericMenu> GetCompareMenu(CompareViewModel vCompareInfo)
        {
            List<GenericMenu> oReturn = new List<GenericMenu>();

            if (vCompareInfo != null && vCompareInfo.RelatedCompare != null && vCompareInfo.RelatedCompare.CompareId > 0)
            {
                string oCurrentController = MarketPlace.Web.Controllers.BaseController.CurrentControllerName;
                string oCurrentAction = MarketPlace.Web.Controllers.BaseController.CurrentActionName;

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
                                action = MVC.Compare.ActionNames.CIExperiencesCompare,
                                CompareId = vCompareInfo.RelatedCompare.CompareId,
                                Currency = vCompareInfo.CompareCurrency,
                            }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Compare.ActionNames.CIExperiencesCompare &&
                        oCurrentController == MVC.Compare.Name),
                });

                #endregion

                #region HSEQ Info

                //header HSEQ
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "HSEQ",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Compare.Name,
                                action = MVC.Compare.ActionNames.HIHSECompare,
                                CompareId = vCompareInfo.RelatedCompare.CompareId,
                                Currency = vCompareInfo.CompareCurrency,
                            }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Compare.ActionNames.HIHSECompare &&
                        oCurrentController == MVC.Compare.Name),
                });

                #endregion

                #region Financial Info

                //header Balancesheet
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información Financiera",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Compare.Name,
                                action = MVC.Compare.ActionNames.FIBalanceSheetInfoCompare,
                                CompareId = vCompareInfo.RelatedCompare.CompareId,
                                Currency = vCompareInfo.CompareCurrency,
                            }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Compare.ActionNames.FIBalanceSheetInfoCompare &&
                        oCurrentController == MVC.Compare.Name),
                });

                #endregion

                //add menu
                oReturn.Add(oMenuAux);
            }
            return oReturn;
        }

        #endregion
    }
}