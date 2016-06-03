using BackOffice.Models.General;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            if (Request["OtherProvidersId"] != null)
            {
                oReturn.CompanyInfo.Add(new GenericItemInfoModel()
                {
                    ItemInfoId = !string.IsNullOrEmpty(Request["OtherProvidersId"]) ? Convert.ToInt32(Request["OtherProvidersId"]) : 0,
                    ItemInfoType = new CatalogModel()
                    {
                        ItemId = (int)BackOffice.Models.General.enumCompanyInfoType.OtherProviders,
                    },
                    Value = Request["OtherProviders"] == "on" ? "1" : "0",
                    Enable = true,
                });
            }

            if (Request["CustomDataId"] != null)
            {
                oReturn.CompanyInfo.Add(new GenericItemInfoModel()
                {
                    ItemInfoId = !string.IsNullOrEmpty(Request["CustomDataId"]) ? Convert.ToInt32(Request["CustomDataId"]) : 0,
                    ItemInfoType = new CatalogModel()
                    {
                        ItemId = (int)BackOffice.Models.General.enumCompanyInfoType.CustomData,
                    },
                    Value = Request["CustomData"] == "on" ? "1" : "0",
                    Enable = true,
                });
            }

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

            //Lista de los criterios de evaluacion
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
            ProveedoresOnLine.ProjectModule.Controller.ProjectModule.GetAllEvaluationItemByProjectConfig(ProjectConfigId, null, (int)BackOffice.Models.General.enumEvaluationItemType.EvaluationCriteria, 1, true);
            oModel.RelatedProjectConfig.RelatedProjectProvider.RelatedEvaluationItem = oReturn;

            //get provider menu
            if (oModel != null)
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
                    string EvaluationValues = Request["EC_InfoType"] != null ? Request["EC_InfoType"] : string.Empty;
                    EvaluationValues += Request["Selected_Value"] != null ? "_" + Request["Selected_Value"] : string.Empty;
                    EvaluationValues += Request["EC_Operator"] != null ? "_" + Request["EC_Operator"] : string.Empty;
                    EvaluationValues += Request["EC_Result"] != null ? Request["EC_Result"] : string.Empty;


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
                                ItemInfoId = int.Parse(Request["EC_RatingId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.Rating,
                                },
                                Value = Request["EC_Rating"],
                                Enable = true,
                            },
                            new GenericItemInfoModel(){
                                ItemInfoId = int.Parse(Request["EC_EvaluationCriteriaId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.EvaluationCriteria,
                                },
                                Value = Request["EC_EvaluationCriteria"],
                                Enable = true,
                            },
                            new GenericItemInfoModel(){
                                ItemInfoId = int.Parse(Request["EC_EvaluationValuesId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.InfoType_Value_Operator,
                                },
                                Value = EvaluationValues,
                                Enable = true,
                            }
                        }
                    };
                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404003"])
                    && bool.Parse(Request["EvaluationCriteria_1404003"]))
                {
                    string EvaluationValues = Request["EC_InfoType"] != null ? Request["EC_InfoType"] : string.Empty;
                    EvaluationValues += Request["EC_Value"] != null ? "_" + Request["EC_Value"] : string.Empty;
                    EvaluationValues += Request["EC_Operator"] != null ? "_" + Request["EC_Operator"] : string.Empty;
                    EvaluationValues += Request["EC_Result"] != null ? Request["EC_Result"] : string.Empty;


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
                                ItemInfoId = int.Parse(Request["EC_RatingId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.Rating,
                                },
                                Value = Request["EC_Rating"],
                                Enable = true,
                            },
                            new GenericItemInfoModel(){
                                ItemInfoId = int.Parse(Request["EC_EvaluationCriteriaId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.EvaluationCriteria,
                                },
                                Value = Request["EC_EvaluationCriteria"],
                                Enable = true,
                            },
                            new GenericItemInfoModel(){
                                ItemInfoId = int.Parse(Request["EC_EvaluationValuesId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.InfoType_Value_Operator,
                                },
                                Value = EvaluationValues,
                                Enable = true,
                            }
                        }
                    };
                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404004"])
                    && bool.Parse(Request["EvaluationCriteria_1404004"]))
                {
                    string EvaluationValues = Request["EC_InfoType"] != null ? Request["EC_InfoType"] : string.Empty;
                    EvaluationValues += Request["EC_Value"] != null ? "_" + Request["EC_Value"] : string.Empty;
                    EvaluationValues += Request["EC_Operator"] != null ? "_" + Request["EC_Operator"] : string.Empty;
                    EvaluationValues += Request["EC_Result"] != null ? Request["EC_Result"] : string.Empty;


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
                                ItemInfoId = int.Parse(Request["EC_RatingId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.Rating,
                                },
                                Value = Request["EC_Rating"],
                                Enable = true,
                            },
                            new GenericItemInfoModel(){
                                ItemInfoId = int.Parse(Request["EC_EvaluationCriteriaId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.EvaluationCriteria,
                                },
                                Value = Request["EC_EvaluationCriteria"],
                                Enable = true,
                            },
                            new GenericItemInfoModel(){
                                ItemInfoId = int.Parse(Request["EC_EvaluationValuesId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.InfoType_Value_Operator,
                                },
                                Value = EvaluationValues,
                                Enable = true,
                            }
                        }
                    };
                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404005"])
                    && bool.Parse(Request["EvaluationCriteria_1404005"]))
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
                                ItemInfoId = int.Parse(Request["EC_RatingId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.Rating,
                                },
                                Value = Request["EC_Rating"],
                                Enable = true,
                            },
                            new GenericItemInfoModel(){
                                ItemInfoId = int.Parse(Request["EC_EvaluationCriteriaId"]),
                                ItemInfoType = new CatalogModel(){
                                    ItemId = (int)BackOffice.Models.General.enumEvaluationItemInfoType.EvaluationCriteria,
                                },
                                Value = Request["EC_EvaluationCriteria"],
                                Enable = true,
                            },
                        }
                    };
                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404006"])
                    && bool.Parse(Request["EvaluationCriteria_1404006"]))
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
                                Enable = true
                            },
                            new GenericItemInfoModel(){
                                ItemInfoId = int.Parse(Request["EC_EvaluationCriteriaId"]),
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
                        }
                    };
                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404007"])
                    && bool.Parse(Request["EvaluationCriteria_1404007"]))
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
                                ItemInfoId = int.Parse(Request["EC_EvaluationCriteriaId"]),
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
                        }
                    };
                }
                else if (!string.IsNullOrEmpty(Request["EvaluationCriteria_1404008"])
                    && bool.Parse(Request["EvaluationCriteria_1404008"]))
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
                                ItemInfoId = int.Parse(Request["EC_EvaluationCriteriaId"]),
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
                        }
                    };
                }
                return oReturn;
            }
            return null;
        }

        #endregion

        #endregion

        #region ThirdKnowledge

        public virtual ActionResult TDAssignedPlan(string CustomerPublicId)
        {
            //generic model info
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                ThirdKnowledgeOptions = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.CatalogGetThirdKnowledgeOptions(),
                RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                },
            };

            //get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);

            return View(oModel);
        }

        #endregion

        #region AditionalDocuments

        public virtual ActionResult ADAditionalDocumentsUpsert(string CustomerPublicId)
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

            oModel.CatalogGetAllModuleOptions = ProveedoresOnLine.Company.Controller.Company.CatalogGetAllModuleOptions();

            //get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);

            return View(oModel);
        }

        #endregion

        #region CalificationProjectConfig

        public virtual ActionResult CPCCalificationProjectConfigUpsert(string CustomerPublicId)
        {
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

        public virtual ActionResult CPCCalificationProjectConfigValidateUpsert(string CustomerPublicId,string CalificationProjectConfigId) 
        {
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                },
                RelatedCalificationProjectConfig = new Models.Customer.CalificationProjectConfigViewModel(ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfig_GetByCalificationProjectConfigId(Convert.ToInt32(CalificationProjectConfigId))),

            };
            //Get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);
            return View();
        }

        public virtual ActionResult CPCCalificationProjectConfigItemUpsert(string CustomerPublicId, string CalificationProjectConfigId)
        {
            BackOffice.Models.Customer.CustomerViewModel oModel = new Models.Customer.CustomerViewModel()
            {
                CustomerOptions = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CatalogGetCustomerOptions(),
                RelatedCustomer = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                },
                RelatedCalificationProjectConfig = new Models.Customer.CalificationProjectConfigViewModel(ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfig_GetByCalificationProjectConfigId(Convert.ToInt32(CalificationProjectConfigId))),
                CalificationProjectOptions = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigOptions(),
                CalificationProjectCategoryOptions = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigCategoryOptions(),
            };

            //Get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);

            return View(oModel);
        }

        #endregion

        #region Reports

        public virtual ActionResult DownloadReport(string CustomerPublicId)
        {
            List<ProveedoresOnLine.Reports.Models.Reports.CustomerProviderReportModel> oReturn = new List<ProveedoresOnLine.Reports.Models.Reports.CustomerProviderReportModel>();
            oReturn = ProveedoresOnLine.Reports.Controller.ReportModule.R_ProviderCustomerReport(CustomerPublicId);

            //Write Document
            StringBuilder data = new StringBuilder();
            string strSep = ";";

            oReturn.All(x =>
            {
                if (oReturn.IndexOf(x) == 0)
                {
                    data.AppendLine
                    ("\"" + "Proveedor" + "\"" + strSep +
                    "\"" + "Tipo de Identificación" + "\"" + strSep +
                    "\"" + "Número de Identificación" + "\"" + strSep +
                    "\"" + "Estado del Proveedor" + "\"" + strSep +
                    "\"" + "Comprador Relacionado" + "\"" + strSep +
                    "\"" + "Pais" + "\"" + strSep +
                    "\"" + "Estado" + "\"" + strSep +
                    "\"" + "Ciudad" + "\"" + strSep +
                    "\"" + "Representante Legal" + "\"" + strSep +
                    "\"" + "Teléfono" + "\"" + strSep +
                    "\"" + "Correo Electrónico" + "\"");
                }
                else
                {
                    data.AppendLine
                    ("\"" + x.ProviderName + "\"" + strSep +
                    "\"" + x.ProviderIdentificationType + "\"" + strSep +
                    "\"" + x.ProviderIdentificationNumber + "\"" + strSep +
                    "\"" + x.ProviderStatus + "\"" + strSep +
                    "\"" + x.CustomerName + "\"" + strSep +
                    "\"" + x.Country + "\"" + strSep +
                    "\"" + x.State + "\"" + strSep +
                    "\"" + x.City + "\"" + strSep +
                    "\"" + x.Representant + "\"" + strSep +
                    "\"" + x.Telephone + "\"" + strSep +
                    "\"" + x.Email + "\"");
                }

                return true;
            });

            byte[] buffer = Encoding.Default.GetBytes(data.ToString().ToCharArray());

            return File(buffer, "application/csv", "ProveedoresXCliente_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv");
        }

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

                #region survey config

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Conocimiento de Terceros",
                    Position = 3,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Company User
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Asignación del plan",
                    Url = Url.Action
                        (MVC.Customer.ActionNames.TDAssignedPlan,
                        MVC.Customer.Name,
                        new { CustomerPublicId = vCustomerInfo.RelatedCustomer.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Customer.ActionNames.TDAssignedPlan &&
                        oCurrentController == MVC.Customer.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region calification project config

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Proceso de Calificación",
                    Position = 4,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Calification Project config
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Configuración del proceso",
                    Url = Url.Action
                        (MVC.Customer.ActionNames.CPCCalificationProjectConfigUpsert,
                        MVC.Customer.Name,
                        new { CustomerPublicId = vCustomerInfo.RelatedCustomer.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Customer.ActionNames.CPCCalificationProjectConfigUpsert &&
                        oCurrentController == MVC.Customer.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Project config

                ////header
                //oMenuAux = new Models.General.GenericMenu()
                //{
                //    Name = "Configuración de Precalificaciones",
                //    Position = 5,
                //    ChildMenu = new List<Models.General.GenericMenu>(),
                //};

                ////Company User
                //oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                //{
                //    Name = "Precalificaciones",
                //    Url = Url.Action
                //        (MVC.Customer.ActionNames.PCProjectConfigUpsert,
                //        MVC.Customer.Name,
                //        new { CustomerPublicId = vCustomerInfo.RelatedCustomer.RelatedCompany.CompanyPublicId }),
                //    Position = 0,
                //    IsSelected =
                //        ((oCurrentAction == MVC.Customer.ActionNames.PCProjectConfigUpsert ||
                //        oCurrentAction == MVC.Customer.ActionNames.PCEvaluationItemUpsert) &&
                //        oCurrentController == MVC.Customer.Name),
                //});

                ////get is selected menu
                //oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                ////add menu
                //oReturn.Add(oMenuAux);

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