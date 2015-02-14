using System;
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

        public virtual ActionResult AdminEcoActivityUpsert(int TreeId, string TreeName)
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

        public virtual ActionResult AdminTRMUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
              {
                  ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
              };

            oModel.ProviderMenu = GetAdminMenu(oModel);
            return View(oModel);
        }

        public virtual ActionResult AdminRLDownloadProvider()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);
            return View(oModel);
        }

        public virtual ActionResult AdminRLUploadProvider()
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
                Position = 1,
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
                Position = 2,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminGeoUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Standart Economy Activity standar
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Maestras Estandar",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminEcoActivityUpsert,
                    MVC.Admin.Name, new { TreeId = 0, TreeName = ""}),
                Position = 3,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminEcoActivityUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Arboles
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Maestras Personalizadas",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminTreeUpsert,
                    MVC.Admin.Name),
                Position = 4,
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
                    Position = 5,
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
                Position = 6,
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
                Position = 7,
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
                Position = 8,
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
                Position = 9,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminResolutionUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //TRM
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "TRM",                
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminTRMUpsert,
                    MVC.Admin.Name),
                Position = 10,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminTRMUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //add menu
            oReturn.Add(oMenuAux);

            #endregion

            #region Restrictive List

            //header
            oMenuAux = new Models.General.GenericMenu()
            {
                Name = "Listas Restrictivas",
                Position = 1,
                ChildMenu = new List<Models.General.GenericMenu>(),
            };
            
            //DownLoad File
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Descarga de Provedores",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminRLDownloadProvider,
                    MVC.Admin.Name),
                Position = 0,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminRLDownloadProvider &&
                    oCurrentController == MVC.Admin.Name),
            });

            //UpLoad File
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Carga de Provedores",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminRLUploadProvider,
                    MVC.Admin.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminRLUploadProvider &&
                    oCurrentController == MVC.Admin.Name),
            });

            //get is selected menu
            oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

            //add menu
            oReturn.Add(oMenuAux);

            #endregion

            #region last next menu

            BackOffice.Models.General.GenericMenu MenuAux = null;

            oReturn.OrderBy(x => x.Position).All(pm =>
            {
                pm.ChildMenu.OrderBy(x => x.Position).All(sm =>
                {
                    if (MenuAux != null)
                    {
                        MenuAux.NextMenu = sm;
                    }
                    sm.LastMenu = MenuAux;
                    MenuAux = sm;

                    return true;
                });

                return true;
            });

            #endregion

            return oReturn;
        }

        #endregion
    }
}