using MarketPlace.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    [MarketPlace.Web.Controllers.Filters.LogginActionFilter]
    public partial class BaseController : Controller
    {
        #region public static properties

        public static string CurrentControllerName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            }
        }

        public static string CurrentActionName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            }
        }

        #endregion

        #region Menu

        public List<GenericMenu> GetHeaderMenu()
        {
            List<GenericMenu> oReturn = new List<Models.General.GenericMenu>();
            int oPosition = 1;
            MarketPlace.Models.General.enumCompanyType oCurrentCompanyType = MarketPlace.Models.General.SessionModel.CurrentCompanyType;
            List<int> oCurrentUserModules = MarketPlace.Models.General.SessionModel.CurrentUserModules();

            #region Customer

            //customer options
            if (oCurrentCompanyType == MarketPlace.Models.General.enumCompanyType.Buyer ||
                oCurrentCompanyType == MarketPlace.Models.General.enumCompanyType.BuyerProvider)
            {
                //home customer
                oReturn.Add(new GenericMenu()
                {
                    Name = "Inicio",
                    Position = oPosition,
                    Url = Url.RouteUrl(
                        MarketPlace.Models.General.Constants.C_Routes_Default,
                        new
                        {
                            controller = MVC.Customer.Name,
                            action = MVC.Customer.ActionNames.Index
                        }),
                    IsSelected = (CurrentControllerName == MVC.Customer.Name),
                });
                oPosition++;

                if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceCustomerModules.ProviderSearch))
                {
                    //provider search
                    oReturn.Add(new GenericMenu()
                    {
                        Name = "Busqueda de Proveedores",
                        Position = oPosition,
                        Url = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Customer.Name,
                                action = MVC.Customer.ActionNames.Index
                            }),
                        IsSelected = (CurrentControllerName == MVC.Provider.Name),
                    });
                    oPosition++;
                }

                if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceCustomerModules.ProviderCompare))
                {
                    //provider compare
                    oReturn.Add(new GenericMenu()
                    {
                        Name = "Comparación de Proveedores",
                        Position = oPosition,
                        Url = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Compare.Name,
                                action = MVC.Compare.ActionNames.Index
                            }),
                        IsSelected = (CurrentControllerName == MVC.Compare.Name),
                    });
                    oPosition++;
                }

                //if (oCurrentUserModules.Any(x =>
                //        (x == (int)enumMarketPlaceCustomerModules.ProviderSelectionCreate) ||
                //        (x == (int)enumMarketPlaceCustomerModules.ProviderSelectionAward) ||
                //        (x == (int)enumMarketPlaceCustomerModules.ProviderSelectionAudit)))
                //{
                //    //provider selection
                //    oReturn.Add(new GenericMenu()
                //    {
                //        Name = "Proceso de selección",
                //        Position = oPosition,
                //        Url = Url.RouteUrl(
                //            MarketPlace.Models.General.Constants.C_Routes_Default,
                //            new
                //            {
                //                controller = MVC.Project.Name,
                //                action = MVC.Project.ActionNames.Index
                //            }),
                //        IsSelected = (CurrentControllerName == MVC.Project.Name &&
                //                    CurrentActionName == MVC.Project.ActionNames.Index),
                //    });
                //    oPosition++;
                //}

            }

            #endregion

            #region Provider

            //provider options
            if (oCurrentCompanyType == MarketPlace.Models.General.enumCompanyType.Provider ||
                oCurrentCompanyType == MarketPlace.Models.General.enumCompanyType.BuyerProvider)
            {
                //home provider
                oReturn.Add(new GenericMenu()
                {
                    Name = "Mi empresa",
                    Position = oPosition,
                    Url = Url.RouteUrl(
                        MarketPlace.Models.General.Constants.C_Routes_Default,
                        new
                        {
                            controller = MVC.Provider.Name,
                            action = MVC.Provider.ActionNames.Index
                        }),
                    IsSelected = (CurrentControllerName == MVC.Provider.Name &&
                                CurrentActionName == MVC.Provider.ActionNames.Index),
                });
                oPosition++;

                //if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceProviderModules.MarketComparison))
                //{
                //    //Market Comparison 
                //    oReturn.Add(new GenericMenu()
                //    {
                //        Name = "Comparación de mercado",
                //        Position = oPosition,
                //        Url = Url.RouteUrl(
                //            MarketPlace.Models.General.Constants.C_Routes_Default,
                //            new
                //            {
                //                controller = MVC.Provider.Name,
                //                action = MVC.Provider.ActionNames.Index
                //            }),
                //        IsSelected = (CurrentControllerName == MVC.Provider.Name &&
                //                    CurrentActionName == MVC.Provider.ActionNames.Index),
                //    });
                //    oPosition++;
                //}

                //if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceProviderModules.ProviderStatistics))
                //{
                //    //Provider Statistics
                //    oReturn.Add(new GenericMenu()
                //    {
                //        Name = "Estadisticas de consulta",
                //        Position = oPosition,
                //        Url = Url.RouteUrl(
                //            MarketPlace.Models.General.Constants.C_Routes_Default,
                //            new
                //            {
                //                controller = MVC.Provider.Name,
                //                action = MVC.Provider.ActionNames.Index
                //            }),
                //        IsSelected = (CurrentControllerName == MVC.Provider.Name &&
                //                    CurrentActionName == MVC.Provider.ActionNames.Index),
                //    });
                //    oPosition++;
                //}
            }


            #endregion

            return oReturn;
        }

        #endregion

        #region generic file actions

        public virtual FileResult GetPdfFileBytes(string FilePath)
        {
            byte[] bytes = new byte[] { };
            if (!string.IsNullOrEmpty(FilePath) && FilePath.IndexOf(".pdf") > 0)
            {
                bytes = (new System.Net.WebClient()).DownloadData(FilePath);
            }
            return File(bytes, "application/pdf");
        }

        #endregion
    }
}