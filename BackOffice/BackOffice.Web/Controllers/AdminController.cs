﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class AdminController : BaseController
    {
        public virtual ActionResult Index()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };          

            //get provider menu
            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        #region Menu

        private List<BackOffice.Models.General.GenericMenu> GetAdminMenu
            (BackOffice.Models.Provider.ProviderViewModel vAdminInfo)
        {
            List<BackOffice.Models.General.GenericMenu> oReturn = new List<Models.General.GenericMenu>();

            //get current controller action
            string oCurrentController = BackOffice.Web.Controllers.BaseController.CurrentControllerName;
            string oCurrentAction = BackOffice.Web.Controllers.BaseController.CurrentActionName;

            #region Administration

            //header
            BackOffice.Models.General.GenericMenu oMenuAux = new Models.General.GenericMenu()
            {
                Name = "Administración",
                Position = 0,
                ChildMenu = new List<Models.General.GenericMenu>(),
            };

            //Geolocalization
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Usuarios",
                Url = Url.Action
                    (MVC.Provider.ActionNames.GIProviderUpsert,
                    MVC.Provider.Name),
                Position = 0,
                IsSelected =
                    (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                    oCurrentController == MVC.Provider.Name),
            });


            //Geolocalization
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Geolocalización",
                Url = Url.Action
                    (MVC.Provider.ActionNames.GIProviderUpsert,
                    MVC.Provider.Name),
                Position = 0,
                IsSelected =
                    (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                    oCurrentController == MVC.Provider.Name),
            });

            //Standart Economy Activity
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Acividades Económicas Estandar",
                Url = Url.Action
                    (MVC.Provider.ActionNames.GICompanyContactUpsert,
                    MVC.Provider.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Provider.ActionNames.GICompanyContactUpsert &&
                    oCurrentController == MVC.Provider.Name),
            });

            //Specific Economy Activity
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Acividades Economicas Específicas",
                Url = Url.Action
                    (MVC.Provider.ActionNames.GICompanyContactUpsert,
                    MVC.Provider.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Provider.ActionNames.GICompanyContactUpsert &&
                    oCurrentController == MVC.Provider.Name),
            });

            //Banks
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Bancos",
                Url = Url.Action
                    (MVC.Provider.ActionNames.GICompanyContactUpsert,
                    MVC.Provider.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Provider.ActionNames.GICompanyContactUpsert &&
                    oCurrentController == MVC.Provider.Name),
            });

            //Certificactions Companies
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Empresas Certificadoras",
                Url = Url.Action
                    (MVC.Provider.ActionNames.GICompanyContactUpsert,
                    MVC.Provider.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Provider.ActionNames.GICompanyContactUpsert &&
                    oCurrentController == MVC.Provider.Name),
            });

            //Certificados
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Certificados",
                Url = Url.Action
                    (MVC.Provider.ActionNames.GICompanyContactUpsert,
                    MVC.Provider.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Provider.ActionNames.GICompanyContactUpsert &&
                    oCurrentController == MVC.Provider.Name),
            });

            //Resoluciones
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Resoluciones",
                Url = Url.Action
                    (MVC.Provider.ActionNames.GICompanyContactUpsert,
                    MVC.Provider.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Provider.ActionNames.GICompanyContactUpsert &&
                    oCurrentController == MVC.Provider.Name),
            });

            //TRM
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "TRM",
                Url = Url.Action
                    (MVC.Provider.ActionNames.GICompanyContactUpsert,
                    MVC.Provider.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Provider.ActionNames.GICompanyContactUpsert &&
                    oCurrentController == MVC.Provider.Name),
            });

            //add menu
            oReturn.Add(oMenuAux);

            #endregion

            return oReturn;
        }

        #endregion
    }
}