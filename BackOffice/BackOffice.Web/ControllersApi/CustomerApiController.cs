using BackOffice.Models.Provider;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyCustomer.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class CustomerApiController : BaseApiController
    {
        #region Search Methods

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Customer.CustomerSearchViewModel> SMCustomerSearch
            (string SMCustomerSearch,
            string SearchParam,
            string PageNumber,
            string RowCount)
        {
            string oCompanyType =
                    ((int)(BackOffice.Models.General.enumCompanyType.Buyer)).ToString() + "," +
                    ((int)(BackOffice.Models.General.enumCompanyType.BuyerProvider)).ToString();

            int oPageNumber = string.IsNullOrEmpty(PageNumber) ? 0 : Convert.ToInt32(PageNumber.Trim());

            int oRowCount = Convert.ToInt32(string.IsNullOrEmpty(RowCount) ?
                BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value :
                RowCount.Trim());

            int oTotalRows;

            List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oSearchResult =
                ProveedoresOnLine.Company.Controller.Company.CompanySearch
                    (oCompanyType,
                    SearchParam,
                    string.Empty,
                    oPageNumber,
                    oRowCount,
                    out oTotalRows);

            List<BackOffice.Models.Customer.CustomerSearchViewModel> oReturn = new List<Models.Customer.CustomerSearchViewModel>();

            if (oSearchResult != null && oSearchResult.Count > 0)
            {
                oSearchResult.All(sr =>
                {
                    oReturn.Add(new Models.Customer.CustomerSearchViewModel(sr, oTotalRows));
                    return true;
                });
            }

            return oReturn;
        }

        #endregion

        #region Customer Rules

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Customer.CustomerRoleViewModel> UserRolesByCustomer
        (string UserRolesByCustomer,
            string CustomerPublicId)
        {
            List<BackOffice.Models.Customer.CustomerRoleViewModel> oReturn = new List<Models.Customer.CustomerRoleViewModel>();

            if (UserRolesByCustomer == "true")
            {
                List<ProveedoresOnLine.Company.Models.Company.UserCompany> oUsersRole = ProveedoresOnLine.Company.Controller.Company.RoleCompany_GetUsersByPublicId
                    (CustomerPublicId);

                if (oUsersRole != null)
                {
                    oUsersRole.All(x =>
                        {
                            oReturn.Add(new BackOffice.Models.Customer.CustomerRoleViewModel(x));
                            return true;
                        });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Customer.CustomerRoleViewModel> UserCompanyUpsert
            (string UserCompanyUpsert,
            string CustomerPublicId)
        {
            List<BackOffice.Models.Customer.CustomerRoleViewModel> oReturn = new List<Models.Customer.CustomerRoleViewModel>();

            if (UserCompanyUpsert == "true" &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]))
            {
                BackOffice.Models.Customer.CustomerRoleViewModel oDataToUpsert =
                    (BackOffice.Models.Customer.CustomerRoleViewModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
                                typeof(BackOffice.Models.Customer.CustomerRoleViewModel));

                ProveedoresOnLine.Company.Models.Company.UserCompany oUserCompany = new ProveedoresOnLine.Company.Models.Company.UserCompany()
                {
                    UserCompanyId = string.IsNullOrEmpty(oDataToUpsert.UserCompanyId) ? 0 : Convert.ToInt32(oDataToUpsert.UserCompanyId),
                    User = oDataToUpsert.User,
                    RelatedRole = new GenericItemModel()
                    {
                        ItemId = Convert.ToInt32(oDataToUpsert.RoleCompanyId),
                    },
                    Enable = true,
                };

                ProveedoresOnLine.Company.Controller.Company.UserCompanyUpsert(oUserCompany);
            }

            return oReturn;
        }

        #endregion

        #region Survey Config

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Customer.SurveyConfigViewModel> SCSurveyConfigSearch
            (string SCSurveyConfigSearch,
            string CustomerPublicId,
            string SearchParam,
            string Enable,
            string PageNumber,
            string RowCount)
        {
            List<BackOffice.Models.Customer.SurveyConfigViewModel> oReturn = new List<Models.Customer.SurveyConfigViewModel>();

            if (SCSurveyConfigSearch == "true")
            {
                int oPageNumber = string.IsNullOrEmpty(PageNumber) ? 0 : Convert.ToInt32(PageNumber.Trim());

                int oRowCount = Convert.ToInt32(string.IsNullOrEmpty(RowCount) ?
                    BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value :
                    RowCount.Trim());

                bool oEnable = (!string.IsNullOrEmpty(Enable) && Enable == "true");

                int oTotalRows;
                List<ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel> oSearchResult = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyConfigSearch
                    (CustomerPublicId, SearchParam, oEnable, oPageNumber, oRowCount, out oTotalRows);

                if (oSearchResult != null && oSearchResult.Count > 0)
                {
                    oSearchResult.All(x =>
                    {
                        oReturn.Add(new BackOffice.Models.Customer.SurveyConfigViewModel(x));
                        return true;
                    });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Customer.SurveyConfigViewModel SCSurveyConfigUpsert
            (string SCSurveyConfigUpsert,
            string CustomerPublicId)
        {
            BackOffice.Models.Customer.SurveyConfigViewModel oReturn = null;

            if (SCSurveyConfigUpsert == "true" &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]))
            {
                BackOffice.Models.Customer.SurveyConfigViewModel oDataToUpsert =
                    (BackOffice.Models.Customer.SurveyConfigViewModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
                                typeof(BackOffice.Models.Customer.SurveyConfigViewModel));

                ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel oSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel()
                {
                    RelatedCustomer = new CustomerModel()
                    {
                        RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                        {
                            CompanyPublicId = CustomerPublicId,
                        },
                    },
                    ItemId = string.IsNullOrEmpty(oDataToUpsert.SurveyConfigId) ? 0 : Convert.ToInt32(oDataToUpsert.SurveyConfigId.Trim()),
                    ItemName = oDataToUpsert.SurveyName,
                    Enable = oDataToUpsert.SurveyEnable,
                    ItemInfo = new List<GenericItemInfoModel>() 
                    { 
                        new GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.GroupId) ? 0 : Convert.ToInt32(oDataToUpsert.GroupId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumSurveyConfigInfoType.Group
                            },
                            Value = oDataToUpsert.Group,
                            ValueName = oDataToUpsert.GroupName,
                            Enable = true,                            
                        },
                        new GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.StepEnableId) ? 0 : Convert.ToInt32(oDataToUpsert.StepEnableId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumSurveyConfigInfoType.StepEnable
                            },
                            Value = oDataToUpsert.StepEnable.ToString().ToLower().Trim(),
                            Enable = true,                            
                        }
                    },
                };

                oSurveyConfig = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyConfigUpsert(oSurveyConfig);

                //create return object
                oReturn = new BackOffice.Models.Customer.SurveyConfigViewModel
                    (oSurveyConfig);
            }
            return oReturn;
        }



        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Customer.SurveyConfigItemViewModel> SCSurveyConfigItemGetBySurveyConfigId
            (string SCSurveyConfigItemGetBySurveyConfigId,
            string SurveyConfigId,
            string ParentSurveyConfigItem,
            string Enable)
        {
            List<BackOffice.Models.Customer.SurveyConfigItemViewModel> oReturn = new List<Models.Customer.SurveyConfigItemViewModel>();

            if (SCSurveyConfigItemGetBySurveyConfigId == "true")
            {
                bool oEnable = (!string.IsNullOrEmpty(Enable) && Enable == "true");

                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oSearchResult =
                    ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyConfigItemGetBySurveyConfigId
                    (Convert.ToInt32(SurveyConfigId.Trim()),
                    string.IsNullOrEmpty(ParentSurveyConfigItem) ? null : (int?)Convert.ToInt32(ParentSurveyConfigItem.Trim()),
                    oEnable);

                if (oSearchResult != null && oSearchResult.Count > 0)
                {
                    oSearchResult.All(x =>
                    {
                        oReturn.Add(new BackOffice.Models.Customer.SurveyConfigItemViewModel(x));
                        return true;
                    });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Customer.SurveyConfigItemViewModel SCSurveyConfigItemUpsert
            (string SCSurveyConfigItemUpsert,
            string CustomerPublicId,
            string SurveyConfigId)
        {
            BackOffice.Models.Customer.SurveyConfigItemViewModel oReturn = null;

            if (SCSurveyConfigItemUpsert == "true" &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]))
            {
                BackOffice.Models.Customer.SurveyConfigItemViewModel oDataToUpsert =
                    (BackOffice.Models.Customer.SurveyConfigItemViewModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
                                typeof(BackOffice.Models.Customer.SurveyConfigItemViewModel));

                ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel oSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel()
                {
                    RelatedCustomer = new CustomerModel()
                    {
                        RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                        {
                            CompanyPublicId = CustomerPublicId,
                        },
                    },
                    ItemId = Convert.ToInt32(SurveyConfigId.Trim()),
                    RelatedSurveyConfigItem = new List<GenericItemModel>()
                    {
                        new GenericItemModel()
                        {
                            ItemId = string.IsNullOrEmpty(oDataToUpsert.SurveyConfigItemId) ? 0 : Convert.ToInt32(oDataToUpsert.SurveyConfigItemId.Trim()),
                            ItemName = oDataToUpsert.SurveyConfigItemName,
                            ItemType = new CatalogModel()
                            {
                                ItemId = Convert.ToInt32(oDataToUpsert.SurveyConfigItemTypeId.Trim()),
                            },
                            ParentItem = string.IsNullOrEmpty(oDataToUpsert.ParentSurveyConfigItem) ? null : 
                                new GenericItemModel(){ ItemId = Convert.ToInt32(oDataToUpsert.ParentSurveyConfigItem.Trim())},
                            Enable = oDataToUpsert.SurveyConfigItemEnable,

                            ItemInfo = new List<GenericItemInfoModel>()
                            {
                                new GenericItemInfoModel()
                                {
                                    ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.SurveyConfigItemInfoOrderId) ? 0 : Convert.ToInt32(oDataToUpsert.SurveyConfigItemInfoOrderId.Trim()),
                                    ItemInfoType = new CatalogModel()
                                    {
                                        ItemId = (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.Order
                                    },
                                    Value = oDataToUpsert.SurveyConfigItemInfoOrder.Replace(" ",""),
                                    Enable = true,
                                },
                                new GenericItemInfoModel()
                                {
                                    ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.SurveyConfigItemInfoWeightId) ? 0 : Convert.ToInt32(oDataToUpsert.SurveyConfigItemInfoWeightId.Trim()),
                                    ItemInfoType = new CatalogModel()
                                    {
                                        ItemId = (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.Weight
                                    },
                                    Value = oDataToUpsert.SurveyConfigItemInfoWeight.Replace(" ",""),
                                    Enable = true,
                                },
                            },
                        },
                    }
                };

                if (oSurveyConfig.RelatedSurveyConfigItem.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemType.Question)
                {
                    #region get question

                    oSurveyConfig.RelatedSurveyConfigItem.FirstOrDefault().ItemInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.SurveyConfigItemInfoHasDescriptionId) ? 0 : Convert.ToInt32(oDataToUpsert.SurveyConfigItemInfoHasDescriptionId.Trim()),
                        ItemInfoType = new CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.HasDescription
                        },
                        Value = oDataToUpsert.SurveyConfigItemInfoHasDescription.ToString().ToLower(),
                        Enable = true,
                    });

                    oSurveyConfig.RelatedSurveyConfigItem.FirstOrDefault().ItemInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.SurveyConfigItemInfoIsMandatoryId) ? 0 : Convert.ToInt32(oDataToUpsert.SurveyConfigItemInfoIsMandatoryId.Trim()),
                        ItemInfoType = new CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.IsMandatory
                        },
                        Value = oDataToUpsert.SurveyConfigItemInfoIsMandatory.ToString().ToLower(),
                        Enable = true,
                    });

                    #endregion
                }

                oSurveyConfig = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyConfigItemUpsert(oSurveyConfig);

                //create return object
                oReturn = new BackOffice.Models.Customer.SurveyConfigItemViewModel
                    (oSurveyConfig.RelatedSurveyConfigItem.FirstOrDefault());
            }
            return oReturn;
        }


        #endregion
    }
}
