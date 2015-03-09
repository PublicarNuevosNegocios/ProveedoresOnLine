﻿using BackOffice.Models.General;
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
                CustomActivityTree = ProveedoresOnLine.Company.Controller.Company.CategorySearchByTreeAdmin(null, 0, 1000),
            };

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
            };

            //get provider menu
            oModel.CustomerMenu = GetCustomerMenu(oModel);

            return View(oModel);
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