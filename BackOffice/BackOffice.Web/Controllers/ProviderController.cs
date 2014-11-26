using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class ProviderController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult UpsertProvider(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel();

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }

        #region Private Methods

        private List<BackOffice.Models.General.GenericMenu> GetProviderMenu
            (BackOffice.Models.Provider.ProviderViewModel vProviderInfo)
        {
            List<BackOffice.Models.General.GenericMenu> oReturn = new List<Models.General.GenericMenu>();

            if (vProviderInfo.RelatedProvider != null &&
                vProviderInfo.RelatedProvider.RelatedCompany != null &&
                !string.IsNullOrEmpty(vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId))
            {
                //get current controller action
                string oCurrentController = BackOffice.Web.Controllers.BaseController.CurrentControllerName;
                string oCurrentAction = BackOffice.Web.Controllers.BaseController.CurrentActionName;

                #region General Info

                //header
                BackOffice.Models.General.GenericMenu oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información general",
                    Position = 0,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Basic info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información básica",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //Contact info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información de contacto",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //Branch info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Sucursales",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //Distributor info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Distribuidores",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Commercial Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información comercial",
                    Position = 1,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Experience
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Experiencias",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //Economic activity
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Actividades economicas",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region HSEQ Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "HSEQ",
                    Position = 2,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Certifications
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Certificaciones",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //Company healty politic
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Salud, medio ambiente y seguridad",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //Company risk policies
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Sistema de riesgos laborales",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Financial Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información financiera",
                    Position = 3,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Balancesheet info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Balances financieros",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //tax info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Impuestos",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //money laundering
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Lavado de activos",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //income statement
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Declaración de renta",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //bank info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información bancaria",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 4,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Legal Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información Legal",
                    Position = 4,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //chaimber of commerce
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Camara y comercio",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //RUT
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Registro unico tributario",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //CIFIN
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "CIFIN",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //SARLAFT
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "SARLAFT",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //Resolution
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Resoluciones",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.UpsertProvider,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 4,
                    IsSelected =
                        (oCurrentController == MVC.Provider.ActionNames.UpsertProvider &&
                        oCurrentAction == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

            }


            return oReturn;
        }

        #endregion
    }
}