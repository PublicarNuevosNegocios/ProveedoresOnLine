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

        public virtual ActionResult AdminUserUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            //get provider menu
            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminGeoUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            //get provider menu
            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminBankUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            //get provider menu
            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminCompanyRulesUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminRulesUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminResolutionUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminEcoActivityUpsert(int TreeId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminEcoGroupUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminTreeUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),                
            };           
            
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

            //Users
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Usuarios",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminUserUpsert,
                    MVC.Admin.Name),
                Position = 0,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminUserUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });


            //Geolocalization
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Geolocalización",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminGeoUpsert,
                    MVC.Admin.Name),
                Position = 0,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminGeoUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Standart Economy Activity
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Acividades Económicas",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminEcoActivityUpsert,
                    MVC.Admin.Name, new { TreeId = 0 }),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminEcoActivityUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Arboles
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Maestras",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminTreeUpsert,
                    MVC.Admin.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminTreeUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });
         
            //Stantart Group
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Grupos",
                    Url = Url.Action
                            (MVC.Admin.ActionNames.AdminEcoGroupUpsert,
                            MVC.Admin.Name),
                    Position = 1,
                    IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminEcoGroupUpsert &&
                    oCurrentController == MVC.Admin.Name),
                });

            //Banks
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Bancos",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminBankUpsert,
                    MVC.Admin.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminBankUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Certificactions Companies
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Empresas Certificadoras",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminCompanyRulesUpsert,
                    MVC.Admin.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminCompanyRulesUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Certificados
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Certificados",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminRulesUpsert,
                    MVC.Admin.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminRulesUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Resoluciones
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Resoluciones",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminResolutionUpsert,
                    MVC.Admin.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminResolutionUpsert &&
                    oCurrentController == MVC.Admin.Name),
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