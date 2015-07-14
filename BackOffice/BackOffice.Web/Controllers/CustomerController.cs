using BackOffice.Models.General;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class CustomerController : BaseController
    {
        public virtual ActionResult Index()
        {
            string oSearchParam = string.IsNullOrEmpty(Request["SearchParam"]) ? null : Request["SearchParam"];

            string oCompanyType =
                    ((int)(BackOffice.Models.General.enumCompanyType.Buyer)).ToString() + "," +
                    ((int)(BackOffice.Models.General.enumCompanyType.BuyerProvider)).ToString();

            //generic model info
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
            };

            return View(oModel);
        }

        #region General Info

        public virtual ActionResult GICustomerUpsert(string CustomerPublicId)
        {
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                CustomActivityTree = ProveedoresOnLine.Company.Controller.Company.TreeGetByType((int)enumTreeType.EconomicActivityCustom),
                SurveyGroup = ProveedoresOnLine.Company.Controller.Company.TreeGetByType((int)enumTreeType.SurveyGroupCustom),
            };

            if (oModel.CustomActivityTree == null)
            {
                oModel.CustomActivityTree = new List<TreeModel>();
            }

            if (oModel.SurveyGroup == null)
            {
                oModel.SurveyGroup = new List<TreeModel>();
            }

            if (!string.IsNullOrEmpty(CustomerPublicId))
            {
                //get provider info
                oModel.RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                };

                //get provider menu
                oModel.CustomerMenu = GetCustomerMenu(oModel);
            }
            //eval upsert action
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                //get provider request info
                ProveedoresOnLine.Company.Models.Company.CompanyModel CompanyToUpsert = GetCustomerRequest();

                //upsert provider
                CompanyToUpsert = ProveedoresOnLine.Company.Controller.Company.CompanyUpsert(CompanyToUpsert);

                //eval redirect url
                if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "next" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.NextMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.NextMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.NextMenu.Url);
                }
                else if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "last" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.LastMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.LastMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.LastMenu.Url);
                }
                else
                {
                    return RedirectToAction(MVC.Customer.ActionNames.GICustomerUpsert, MVC.Customer.Name,
                        new { CustomerPublicId = CompanyToUpsert.CompanyPublicId });
                }
            }

            return View(oModel);
        }

        #region Private methods

        private ProveedoresOnLine.Company.Models.Company.CompanyModel GetCustomerRequest()
        {
            //get company
            ProveedoresOnLine.Company.Models.Company.CompanyModel oReturn = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
            {
                CompanyPublicId = Request["CustomerPublicId"],
                IdentificationType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = Convert.ToInt32(Request["IdentificationType"]),
                },
                IdentificationNumber = Request["IdentificationNumber"],
                CompanyName = Request["CompanyName"],
                CompanyType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = Convert.ToInt32(Request["CompanyType"]),
                },
                Enable = !string.IsNullOrEmpty(Request["Enable"]),
                CompanyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            };

            //get company info
            Request.Form.AllKeys.Where(x => x.Contains("CompanyInfoType_")).All(req =>
            {
                string[] strSplit = req.Split('_');

                if (strSplit.Length >= 3)
                {
                    oReturn.CompanyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = !string.IsNullOrEmpty(strSplit[2]) ? Convert.ToInt32(strSplit[2].Trim()) : 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = Convert.ToInt32(strSplit[1].Trim())
                        },
                        Value = Request[req],
                        Enable = true,
                    });
                }
                return true;
            });
            return oReturn;
        }

        #endregion

        #endregion

        #region Role

        public virtual ActionResult ROCustomerUserUpsert(string CustomerPublicId)
        {
            //generic model info
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                },
                RelatedRoleCompanyList = new List<Models.Customer.CustomerRoleViewModel>(),
            };

            //get role company list
            ProveedoresOnLine.Company.Models.Company.CompanyModel oRules = ProveedoresOnLine.Company.Controller.Company.RoleCompany_GetByPublicId(CustomerPublicId);

            if (oRules != null && oRules.RelatedRole != null && oRules.RelatedRole.Count > 0)
            {
                oRules.RelatedRole.All(y =>
                {
                    oModel.RelatedRoleCompanyList.Add(new Models.Customer.CustomerRoleViewModel(y));
                    return true;
                });
            }

            //get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);

            return View(oModel);
        }

        #endregion

        #region SurveyConfig

        public virtual ActionResult SCSurveyConfigUpsert(string CustomerPublicId)
        {
            //generic model info
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                },
            };

            //get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult SCSurveyConfigItemUpsert(string CustomerPublicId, string SurveyConfigId)
        {
            //generic model info
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                },
                RelatedSurveyConfig = new Models.Customer.SurveyConfigViewModel(ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyConfigGetById(Convert.ToInt32(SurveyConfigId.Trim()))),
                RelatedRoleCompanyList = new List<Models.Customer.CustomerRoleViewModel>(),
            };

            //get role company list
            ProveedoresOnLine.Company.Models.Company.CompanyModel oRules = ProveedoresOnLine.Company.Controller.Company.RoleCompany_GetByPublicId(CustomerPublicId);

            if (oRules != null && oRules.RelatedRole != null && oRules.RelatedRole.Count > 0)
            {
                oRules.RelatedRole.All(y =>
                {
                    oModel.RelatedRoleCompanyList.Add(new Models.Customer.CustomerRoleViewModel(y));
                    return true;
                });
            }

            //get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);

            return View(oModel);
        }

        #endregion

        #region ProjectConfig

        public virtual ActionResult PCProjectConfigUpsert(string CustomerPublicId)
        {
            //generic model info
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                },
            };

            //get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult PCEvaluationItemUpsert(string CustomerPublicId, string ProjectProviderId)
        {
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                },
                RelatedProjectConfig = new Models.Customer.ProjectConfigViewModel(ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectConfigGetById(Convert.ToInt32(ProjectProviderId.Trim()), true)),
                ProjectConfigOptions = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.CatalogGetProjectConfigOptions(),
                RelatedRoleCompanyList = new List<Models.Customer.CustomerRoleViewModel>(),
            };

            //get role company list
            ProveedoresOnLine.Company.Models.Company.CompanyModel oRules = ProveedoresOnLine.Company.Controller.Company.RoleCompany_GetByPublicId(CustomerPublicId);

            if (oRules != null && oRules.RelatedRole != null && oRules.RelatedRole.Count > 0)
            {
                oRules.RelatedRole.All(y =>
                {
                    oModel.RelatedRoleCompanyList.Add(new Models.Customer.CustomerRoleViewModel(y));
                    return true;
                });
            }

            //get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);
            return View(oModel);
        }

        public virtual ActionResult PCEvaluationCriteriaUpsert(string CustomerPublicId, string ProjectProviderId, string EvaluationItemId)
        {
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                },
                RelatedRoleCompanyList = new List<Models.Customer.CustomerRoleViewModel>(),
                RelatedProjectConfig = new Models.Customer.ProjectConfigViewModel(ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectConfigGetById(Convert.ToInt32(ProjectProviderId.Trim()), true)),
                ProjectConfigOptions = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.CatalogGetProjectConfigOptions(),
            };

            //get role company list
            ProveedoresOnLine.Company.Models.Company.CompanyModel oRules = ProveedoresOnLine.Company.Controller.Company.RoleCompany_GetByPublicId(CustomerPublicId);

            if (oRules != null && oRules.RelatedRole != null && oRules.RelatedRole.Count > 0)
            {
                oRules.RelatedRole.All(x =>
                {
                    oModel.RelatedRoleCompanyList.Add(new Models.Customer.CustomerRoleViewModel(x));
                    return true;
                });
            }

            List<GenericItemModel> oRelatedEvaluationItem = new List<GenericItemModel>();

            List<GenericItemModel> oSearchResult = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.GetAllEvaluationItemByProjectConfig
                    (Convert.ToInt32(ProjectProviderId),
                    null,
                    (int)enumEvaluationItemType.EvaluationCriteria,
                    Convert.ToInt32(EvaluationItemId),
                    true);

            oModel.RelatedProjectConfig.RelatedProjectProvider.RelatedEvaluationItem = oSearchResult;

            //get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);

            //eval request
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                GenericItemModel EvaluationCriteriaToUpsert = GetEvaluationCriteriaInfoRequest();

                EvaluationCriteriaToUpsert.ItemId = Convert.ToInt32(EvaluationItemId);

                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.EvaluationItemInfoUpsert(EvaluationCriteriaToUpsert);
            }

            return View(oModel);
        }

        public virtual ActionResult PCEvaluationItemShowCriteria(int ProjectConfigId, string CustomerPublicId, string ProjectProviderId, string EvaluationItemId)
        {
            BackOffice.Models.Customer.CustomerViewModel oModel = null;
            if (CustomerPublicId.Length > 0 && ProjectProviderId.Length > 0 && EvaluationItemId.Length > 0)
            {
                oModel = new Models.Customer.CustomerViewModel()
                {
                    CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                    RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                    {
                        RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                    },
                    RelatedRoleCompanyList = new List<Models.Customer.CustomerRoleViewModel>(),
                    RelatedProjectConfig = new Models.Customer.ProjectConfigViewModel(ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectConfigGetById(Convert.ToInt32(ProjectProviderId.Trim()), true)),
                    ProjectConfigOptions = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.CatalogGetProjectConfigOptions(),
                };

            //Lista de los criterios de evaluacion
            
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.GetAllEvaluationItemByProjectConfig(ProjectConfigId, null, 1401002, 1, true);

    
            }
            //get provider menu
            if (oModel!= null)
                oModel.CustomerMenu = GetCustomerMenu(oModel);
            return View(oModel);
        }

        #region Private Methods

        private GenericItemModel GetEvaluationCriteriaInfoRequest()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
               && bool.Parse(Request["UpsertAction"]))
            {
                GenericItemModel oReturn = null;

                if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404001"])
                    && bool.Parse(Request["EvaluationCriteria_1404001"]))
                {
                    oReturn = new GenericItemModel()
                    {
                        Enable = true,
                        ItemInfo = new List<GenericItemInfoModel>(){
                            new GenericItemInfoModel(){
                                ItemInfoId = int.Parse(Request["EC_UnitId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.Unit,
                                },
                                Value = Request["EC_Unit"],
                                Enable = true,
                            },
                            new GenericItemInfoModel(){
                                ItemInfoId =  int.Parse(Request["EC_EvaluationCriteriaId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.EvaluationCriteria,
                                },
                                Value = Request["EC_EvaluationCriteria"],
                                Enable = true,
                            },
                            new GenericItemInfoModel(){
                                ItemInfoId = int.Parse(Request["EC_OrderId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.Order,
                                },
                                Value = Request["EC_Order"],
                                Enable = true,
                            },
                        },
                    };

                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404002"])
                    && bool.Parse(Request["EvaluationCriteria_1404002"]))
                {

                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404003"])
                    && bool.Parse(Request["EvaluationCriteria_1404003"]))
                {

                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404004"])
                    && bool.Parse(Request["EvaluationCriteria_1404004"]))
                {

                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404005"])
                    && bool.Parse(Request["EvaluationCriteria_1404005"]))
                {

                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404006"])
                    && bool.Parse(Request["EvaluationCriteria_1404006"]))
                {

                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404007"])
                    && bool.Parse(Request["EvaluationCriteria_1404007"]))
                {

                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404008"])
                    && bool.Parse(Request["EvaluationCriteria_1404008"]))
                {

                }

                return oReturn;
            }

            return null;
        }

        #endregion

        #endregion

        #region Menu

        private List<BackOffice.Models.General.GenericMenu> GetCustomerMenu
            (BackOffice.Models.Customer.CustomerViewModel vCustomerInfo)
        {
            List<BackOffice.Models.General.GenericMenu> oReturn = new List<Models.General.GenericMenu>();

            if (vCustomerInfo.RelatedCustomer != null &&
                vCustomerInfo.RelatedCustomer.RelatedCompany != null &&
                !string.IsNullOrEmpty(vCustomerInfo.RelatedCustomer.RelatedCompany.CompanyPublicId))
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
                        (MVC.Customer.ActionNames.GICustomerUpsert,
                        MVC.Customer.Name,
                        new { CustomerPublicId = vCustomerInfo.RelatedCustomer.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Customer.ActionNames.GICustomerUpsert &&
                        oCurrentController == MVC.Customer.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Role company

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Acceso a la plataforma",
                    Position = 1,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Company User
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Usuarios de la compañia",
                    Url = Url.Action
                        (MVC.Customer.ActionNames.ROCustomerUserUpsert,
                        MVC.Customer.Name,
                        new { CustomerPublicId = vCustomerInfo.RelatedCustomer.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Customer.ActionNames.ROCustomerUserUpsert &&
                        oCurrentController == MVC.Customer.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region survey config

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Evaluación de desempeño",
                    Position = 2,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Company User
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Evaluaciones",
                    Url = Url.Action
                        (MVC.Customer.ActionNames.SCSurveyConfigUpsert,
                        MVC.Customer.Name,
                        new { CustomerPublicId = vCustomerInfo.RelatedCustomer.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        ((oCurrentAction == MVC.Customer.ActionNames.SCSurveyConfigUpsert ||
                        oCurrentAction == MVC.Customer.ActionNames.SCSurveyConfigItemUpsert) &&
                        oCurrentController == MVC.Customer.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Project config

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Configuración de Precalificaciones",
                    Position = 3,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Company User
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Precalificaciones",
                    Url = Url.Action
                        (MVC.Customer.ActionNames.PCProjectConfigUpsert,
                        MVC.Customer.Name,
                        new { CustomerPublicId = vCustomerInfo.RelatedCustomer.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        ((oCurrentAction == MVC.Customer.ActionNames.PCProjectConfigUpsert ||
                        oCurrentAction == MVC.Customer.ActionNames.PCEvaluationItemUpsert) &&
                        oCurrentController == MVC.Customer.Name),
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
            }
            return oReturn;
        }

        #endregion

    }
}