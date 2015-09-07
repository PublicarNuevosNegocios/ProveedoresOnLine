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

                return View(oModel);
                            
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

    }
}