using MarketPlace.Models.General;
using System.Collections.Generic;
using System.Linq;
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

        #endregion public static properties

        #region Menu

        public List<GenericMenu> GetHeaderMenu()
        {
            List<GenericMenu> oReturn = new List<Models.General.GenericMenu>();
            int oPosition = 1;
            enumCompanyType oCurrentCompanyType = MarketPlace.Models.General.SessionModel.CurrentCompanyType;
            List<int> oCurrentUserModules = MarketPlace.Models.General.SessionModel.CurrentUserModules();

            #region Customer

            if (oCurrentCompanyType == MarketPlace.Models.General.enumCompanyType.Buyer)
            {
                //Home
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
                    IsSelected = (CurrentControllerName == MVC.Provider.Name &&
                                    CurrentActionName == MVC.Provider.ActionNames.Index),
                });
                oPosition++;

                if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceCustomerModules.ProviderSearch))
                {
                    //Modulo de Busqueda de Proveedores
                    oReturn.Add(new GenericMenu()
                    {
                        Name = "Búsqueda de Proveedores",
                        Position = oPosition,
                        Url = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.Search
                            }),
                        IsSelected = (CurrentControllerName == MVC.Provider.Name &&
                                    CurrentActionName == MVC.Provider.ActionNames.Search),
                    });
                    oPosition++;
                }

                if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceCustomerModules.ProviderCompare))
                {
                    //Modulo de comparación
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

                if (oCurrentUserModules.Any(x =>
                        (x == (int)enumMarketPlaceCustomerModules.ProviderSelectionCreate) ||
                        (x == (int)enumMarketPlaceCustomerModules.ProviderSelectionAward) ||
                        (x == (int)enumMarketPlaceCustomerModules.ProviderSelectionAudit)))
                {
                    //Modulo de Proceso de Selección
                    oReturn.Add(new GenericMenu()
                    {
                        Name = "Proceso de Selección",
                        Position = oPosition,
                        Url = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Project.Name,
                                action = MVC.Project.ActionNames.Index,
                                ProjectStatus = string.Empty,
                                SearchParam = string.Empty,
                                SearchFilter = string.Empty,
                                SearchOrderType = string.Empty,
                                OrderOrientation = string.Empty,
                                PageNumber = string.Empty,
                            }),
                        IsSelected = (CurrentControllerName == MVC.Project.Name &&
                                    CurrentActionName == MVC.Project.ActionNames.Index),
                    });
                    oPosition++;
                }

                if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceCustomerModules.ProviderStats))
                {
                    //Modulo de estadisticas - Proveedores
                    oReturn.Add(new GenericMenu()
                    {
                        Name = "Estadísticas",
                        Position = oPosition,
                        Url = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Stats.Name,
                                action = MVC.Stats.ActionNames.STProviderStats
                            }),
                        IsSelected = (CurrentControllerName == MVC.Stats.ActionNames.STProviderStats),
                    });
                    oPosition++;
                }
                else if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceCustomerModules.ProviderRatingView))
                {
                    //Modulo de estadisticas - Evaluación de Desempeño
                    oReturn.Add(new GenericMenu()
                    {
                        Name = "Estadísticas",
                        Position = oPosition,
                        Url = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Stats.Name,
                                action = MVC.Stats.ActionNames.STSurveyStats
                            }),
                        IsSelected = (CurrentControllerName == MVC.Stats.ActionNames.STSurveyStats),
                    });
                    oPosition++;
                }
                else if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceCustomerModules.ProviderSelectionCreate))
                {
                    //Modulo de estadisticas - Proceso de Selección
                    oReturn.Add(new GenericMenu()
                    {
                        Name = "Estadísticas",
                        Position = oPosition,
                        Url = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Stats.Name,
                                action = MVC.Stats.ActionNames.STProjectStats
                            }),
                        IsSelected = (CurrentControllerName == MVC.Stats.ActionNames.STProjectStats),
                    });
                    oPosition++;
                }

                if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceCustomerModules.ThirdKnowledge))
                {
                    //Conocimiento de terceros
                    oReturn.Add(new GenericMenu()
                    {
                        Name = "Conocimiento de Terceros",
                        Position = oPosition,
                        Url = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.ThirdKnowledge.Name,
                                action = MVC.ThirdKnowledge.ActionNames.TKSingleSearch
                            }),
                        IsSelected = (CurrentControllerName == MVC.ThirdKnowledge.ActionNames.TKSingleSearch),
                    });
                    oPosition++;
                }

                if (oCurrentCompanyType == MarketPlace.Models.General.enumCompanyType.Buyer)
                {
                    if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceCustomerModules.ProviderRatingCreate))
                    {
                        //Modulo de Reportes
                        oReturn.Add(new GenericMenu()
                        {
                            Name = "Reportes",
                            Position = oPosition,
                            Url = Url.RouteUrl(
                                MarketPlace.Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.Report.Name,
                                    action = MVC.Report.ActionNames.RP_SV_SurveyGeneralInfoReport
                                }),
                            IsSelected = (CurrentControllerName == MVC.Report.ActionNames.PRGeneral),
                        });
                        oPosition++;
                    }
                }
            }

            #endregion Customer

            #region Provider

            //provider options
            if (oCurrentCompanyType == MarketPlace.Models.General.enumCompanyType.Provider)
            {
                //home provider
                oReturn.Add(new GenericMenu()
                {
                    Name = "Mi Empresa",
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

            #endregion Provider

            #region BuyerProvider

            if (oCurrentCompanyType == MarketPlace.Models.General.enumCompanyType.BuyerProvider)
            {
                //Home
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
                    IsSelected = (CurrentControllerName == MVC.Provider.Name &&
                                    CurrentActionName == MVC.Provider.ActionNames.Index),
                });
                oPosition++;
                if (oCurrentUserModules.Any(x => x == (int)enumMarketPlaceCustomerModules.ProviderSearch))
                {
                    //Modulo de Busqueda de Proveedores
                    oReturn.Add(new GenericMenu()
                    {
                        Name = "Búsqueda de Proveedores",
                        Position = oPosition,
                        Url = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.Search
                            }),
                        IsSelected = (CurrentControllerName == MVC.Provider.Name &&
                                    CurrentActionName == MVC.Provider.ActionNames.Search),
                    });
                    oPosition++;
                }
            }

            #endregion BuyerProvider

            return oReturn;
        }

        #endregion Menu

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

        #endregion generic file actions
    }
}