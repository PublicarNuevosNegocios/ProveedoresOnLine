﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class ProviderApiController : BaseApiController
    {
        #region Generic Info

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Provider.ProviderContactViewModel> GIContactGetByType
            (string GIContactGetByType,
            string ProviderPublicId,
            string ContactType)
        {
            List<BackOffice.Models.Provider.ProviderContactViewModel> oReturn = new List<Models.Provider.ProviderContactViewModel>();

            if (GIContactGetByType == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oContact = ProveedoresOnLine.Company.Controller.Company.ContactGetBasicInfo
                    (ProviderPublicId,
                    string.IsNullOrEmpty(ContactType) ? null : (int?)Convert.ToInt32(ContactType.Trim()));

                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oCities = null;

                if (oContact != null)
                {
                    if (ContactType == ((int)BackOffice.Models.General.enumContactType.Brach).ToString() ||
                        ContactType == ((int)BackOffice.Models.General.enumContactType.Distributor).ToString())
                    {
                        oCities = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, null, 0, 0);
                    }

                    oContact.All(x =>
                    {
                        oReturn.Add(new BackOffice.Models.Provider.ProviderContactViewModel(x, oCities));
                        return true;
                    });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Provider.ProviderContactViewModel GIContactUpsert
            (string GIContactUpsert,
            string ProviderPublicId,
            string ContactType)
        {
            BackOffice.Models.Provider.ProviderContactViewModel oReturn = null;

            if (GIContactUpsert == "true" &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]) &&
                !string.IsNullOrEmpty(ContactType))
            {
                List<string> lstUsedFiles = new List<string>();

                BackOffice.Models.Provider.ProviderContactViewModel oDataToUpsert =
                    (BackOffice.Models.Provider.ProviderContactViewModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
                                typeof(BackOffice.Models.Provider.ProviderContactViewModel));

                ProveedoresOnLine.Company.Models.Company.CompanyModel oCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = ProviderPublicId,
                    RelatedContact = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>() 
                    { 
                        new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = string.IsNullOrEmpty(oDataToUpsert.ContactId) ? 0 : Convert.ToInt32(oDataToUpsert.ContactId.Trim()),
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = Convert.ToInt32(ContactType.Trim()),
                            },
                            ItemName = oDataToUpsert.ContactName,
                            Enable = oDataToUpsert.Enable,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        },
                    }
                };

                if (oCompany.RelatedContact.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumContactType.CompanyContact)
                {
                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CC_ValueId) ? 0 : Convert.ToInt32(oDataToUpsert.CC_ValueId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CC_Value
                            },
                            Value = oDataToUpsert.CC_Value,
                            Enable = true,
                        });
                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CC_CompanyContactTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.CC_CompanyContactTypeId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CC_CompanyContactType
                            },
                            Value = oDataToUpsert.CC_CompanyContactType,
                            Enable = true,
                        });
                }
                else if (oCompany.RelatedContact.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumContactType.PersonContact)
                {
                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_PersonContactTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_PersonContactTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_PersonContactType
                        },
                        Value = oDataToUpsert.CP_PersonContactType,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_IdentificationTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_IdentificationTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationType
                        },
                        Value = oDataToUpsert.CP_IdentificationType,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_IdentificationNumberId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_IdentificationNumberId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationNumber
                        },
                        Value = oDataToUpsert.CP_IdentificationNumber,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_IdentificationCityId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_IdentificationCityId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationCity
                        },
                        Value = oDataToUpsert.CP_IdentificationCity,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_IdentificationFileId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_IdentificationFileId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationFile
                        },
                        Value = oDataToUpsert.CP_IdentificationFile,
                        Enable = true,
                    });

                    lstUsedFiles.Add(oDataToUpsert.CP_IdentificationFile);

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_PhoneId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_PhoneId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_Phone
                        },
                        Value = oDataToUpsert.CP_Phone,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_EmailId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_EmailId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_Email
                        },
                        Value = oDataToUpsert.CP_Email,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_NegotiationId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_NegotiationId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_Negotiation
                        },
                        Value = oDataToUpsert.CP_Negotiation,
                        Enable = true,
                    });
                }
                else if (oCompany.RelatedContact.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumContactType.Brach)
                {
                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_RepresentativeId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_RepresentativeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Representative
                        },
                        Value = oDataToUpsert.BR_Representative,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_AddressId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_AddressId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Address
                        },
                        Value = oDataToUpsert.BR_Address,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_CityId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_CityId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_City
                        },
                        Value = oDataToUpsert.BR_City,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_PhoneId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_PhoneId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Phone
                        },
                        Value = oDataToUpsert.BR_Phone,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_FaxId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_FaxId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Fax
                        },
                        Value = oDataToUpsert.BR_Fax,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_EmailId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_EmailId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Email
                        },
                        Value = oDataToUpsert.BR_Email,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_WebsiteId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_WebsiteId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Website
                        },
                        Value = oDataToUpsert.BR_Website,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_LatitudeId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_LatitudeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Latitude
                        },
                        Value = oDataToUpsert.BR_Latitude,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_LongitudeId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_LongitudeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Longitude
                        },
                        Value = oDataToUpsert.BR_Longitude,
                        Enable = true,
                    });
                }
                else if (oCompany.RelatedContact.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumContactType.Distributor)
                {
                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_DistributorTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_DistributorTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_DistributorType
                        },
                        Value = oDataToUpsert.DT_DistributorType,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_RepresentativeId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_RepresentativeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_Representative
                        },
                        Value = oDataToUpsert.DT_Representative,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_EmailId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_EmailId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_Email
                        },
                        Value = oDataToUpsert.DT_Email,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_PhoneId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_PhoneId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_Phone
                        },
                        Value = oDataToUpsert.DT_Phone,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_CityId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_CityId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_City
                        },
                        Value = oDataToUpsert.DT_City,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_EmailId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_EmailId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Email
                        },
                        Value = oDataToUpsert.BR_Email,
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_DateIssueId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_DateIssueId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_DateIssue
                        },
                        Value = string.IsNullOrEmpty(oDataToUpsert.DT_DateIssue) ?
                            string.Empty :
                            oDataToUpsert.DT_DateIssue.Replace(" ", "").Length == BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value.Replace(" ", "").Length ?
                            oDataToUpsert.DT_DateIssue :
                            DateTime.ParseExact(
                                oDataToUpsert.DT_DateIssue,
                                BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value,
                                System.Globalization.CultureInfo.InvariantCulture).
                            ToString(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value),
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_DueDateId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_DueDateId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_DueDate
                        },
                        Value = string.IsNullOrEmpty(oDataToUpsert.DT_DueDate) ?
                            string.Empty :
                            oDataToUpsert.DT_DueDate.Replace(" ", "").Length == BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value.Replace(" ", "").Length ?
                            oDataToUpsert.DT_DueDate :
                            DateTime.ParseExact(oDataToUpsert.DT_DueDate, BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value, System.Globalization.CultureInfo.InvariantCulture).
                            ToString(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value),
                        Enable = true,
                    });

                    oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_DistributorFileId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_DistributorFileId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_DistributorFile
                        },
                        Value = oDataToUpsert.DT_DistributorFile,
                        Enable = true,
                    });

                    lstUsedFiles.Add(oDataToUpsert.DT_DistributorFile);
                }

                oCompany = ProveedoresOnLine.Company.Controller.Company.ContactUpsert(oCompany);

                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oCities = null;

                if (ContactType == ((int)BackOffice.Models.General.enumContactType.Brach).ToString())
                {
                    oCities = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, null, 0, 0);
                }

                oReturn = new Models.Provider.ProviderContactViewModel(oCompany.RelatedContact.FirstOrDefault(), oCities);

                //register used files
                LogManager.ClientLog.FileUsedCreate(lstUsedFiles);
            }
            return oReturn;
        }

        #endregion

        #region Commercial Info

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Provider.ProviderCommercialViewModel> CICommercialGetByType
            (string CICommercialGetByType,
            string ProviderPublicId,
            string CommercialType)
        {
            List<BackOffice.Models.Provider.ProviderCommercialViewModel> oReturn = new List<Models.Provider.ProviderCommercialViewModel>();

            if (CICommercialGetByType == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCommercial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CommercialGetBasicInfo
                    (ProviderPublicId,
                    string.IsNullOrEmpty(CommercialType) ? null : (int?)Convert.ToInt32(CommercialType.Trim()));

                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oActivity = null;
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCustomActivity = null;

                if (oCommercial != null)
                {
                    if (CommercialType == ((int)BackOffice.Models.General.enumCommercialType.Experience).ToString())
                    {
                        oActivity = ProveedoresOnLine.Company.Controller.Company.CategorySearchByActivity(null, 0, 0);
                        oCustomActivity = ProveedoresOnLine.Company.Controller.Company.CategorySearchByCustomActivity(null, 0, 0);
                    }

                    oCommercial.All(x =>
                    {
                        oReturn.Add(new BackOffice.Models.Provider.ProviderCommercialViewModel(x, oActivity, oCustomActivity));
                        return true;
                    });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Provider.ProviderCommercialViewModel CICommercialUpsert
            (string CICommercialUpsert,
            string ProviderPublicId,
            string CommercialType)
        {
            BackOffice.Models.Provider.ProviderCommercialViewModel oReturn = null;

            if (CICommercialUpsert == "true" &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]) &&
                !string.IsNullOrEmpty(CommercialType))
            {
                List<string> lstUsedFiles = new List<string>();

                List<Tuple<decimal, decimal>> lstCurrencyConversion = null;

                BackOffice.Models.Provider.ProviderCommercialViewModel oDataToUpsert =
                    (BackOffice.Models.Provider.ProviderCommercialViewModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
                                typeof(BackOffice.Models.Provider.ProviderCommercialViewModel));


                ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = ProviderPublicId,
                    },

                    RelatedCommercial = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>() 
                    {
                        new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = string.IsNullOrEmpty(oDataToUpsert.CommercialId) ? 0 : Convert.ToInt32(oDataToUpsert.CommercialId.Trim()),
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = Convert.ToInt32(CommercialType.Trim()),
                            },
                            ItemName = oDataToUpsert.CommercialName,
                            Enable = oDataToUpsert.Enable,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        },
                    },
                };

                if (oProvider.RelatedCommercial.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumCommercialType.Experience)
                {
                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ContractTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ContractTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractType
                        },
                        Value = oDataToUpsert.EX_ContractType,
                        Enable = true,
                    });

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_DateIssueId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_DateIssueId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_DateIssue
                        },
                        Value = string.IsNullOrEmpty(oDataToUpsert.EX_DateIssue) ?
                            string.Empty :
                            oDataToUpsert.EX_DateIssue.Replace(" ", "").Length == BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value.Replace(" ", "").Length ?
                            oDataToUpsert.EX_DateIssue :
                            DateTime.ParseExact(
                                oDataToUpsert.EX_DateIssue,
                                BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value,
                                System.Globalization.CultureInfo.InvariantCulture).
                            ToString(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value),

                        Enable = true,
                    });

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_DueDateId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_DueDateId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_DueDate
                        },
                        Value = string.IsNullOrEmpty(oDataToUpsert.EX_DueDate) ?
                            string.Empty :
                            oDataToUpsert.EX_DueDate.Replace(" ", "").Length == BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value.Replace(" ", "").Length ?
                            oDataToUpsert.EX_DueDate :
                            DateTime.ParseExact(
                                oDataToUpsert.EX_DueDate,
                                BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value,
                                System.Globalization.CultureInfo.InvariantCulture).
                            ToString(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value),

                        Enable = true,
                    });

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ClientId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ClientId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_Client
                        },
                        Value = oDataToUpsert.EX_Client,
                        Enable = true,
                    });

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ContractNumberId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ContractNumberId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractNumber
                        },
                        Value = oDataToUpsert.EX_ContractNumber,
                        Enable = true,
                    });


                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_CurrencyId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_CurrencyId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_Currency
                        },
                        Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value.Replace(" ", ""),
                        Enable = true,
                    });


                    //get experience value
                    if (BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value == oDataToUpsert.EX_Currency)
                    {
                        //value in default rate
                        lstCurrencyConversion = new List<Tuple<decimal, decimal>>() 
                        { 
                            new Tuple<decimal, decimal>(Convert.ToDecimal(oDataToUpsert.EX_ContractValue, System.Globalization.CultureInfo.InvariantCulture), Convert.ToDecimal(oDataToUpsert.EX_ContractValue, System.Globalization.CultureInfo.InvariantCulture))
                        };
                    }
                    else
                    {
                        //value in custom rate
                        lstCurrencyConversion =
                        string.IsNullOrEmpty(oDataToUpsert.EX_ContractValue) || string.IsNullOrEmpty(oDataToUpsert.EX_Currency) || string.IsNullOrEmpty(oDataToUpsert.EX_DateIssue) ?
                            new List<Tuple<decimal, decimal>>() { new Tuple<decimal, decimal>(0, 0) } :
                            BackOffice.Web.Controllers.BaseController.Currency_ConvertToStandar
                                (Convert.ToInt32(oDataToUpsert.EX_Currency),
                                Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value),
                                (oDataToUpsert.EX_DateIssue.Replace(" ", "").Length == BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value.Replace(" ", "").Length ?
                                    DateTime.ParseExact(
                                        oDataToUpsert.EX_DateIssue,
                                        BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value,
                                        System.Globalization.CultureInfo.InvariantCulture) :
                                    DateTime.ParseExact(
                                        oDataToUpsert.EX_DateIssue,
                                        BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value,
                                        System.Globalization.CultureInfo.InvariantCulture)).Year,
                                new List<decimal>() { Convert.ToDecimal(oDataToUpsert.EX_ContractValue, System.Globalization.CultureInfo.InvariantCulture) });
                    }

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ContractValueId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ContractValueId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractValue
                        },
                        Value = (lstCurrencyConversion != null && lstCurrencyConversion.Count > 0 ? lstCurrencyConversion.FirstOrDefault().Item2 : 0).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
                        Enable = true,
                    });

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_PhoneId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_PhoneId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_Phone
                        },
                        Value = oDataToUpsert.EX_Phone,
                        Enable = true,
                    });

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_BuiltAreaId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_BuiltAreaId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_BuiltArea
                        },
                        Value = oDataToUpsert.EX_BuiltArea,
                        Enable = true,
                    });

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_BuiltUnitId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_BuiltUnitId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_BuiltUnit
                        },
                        Value = oDataToUpsert.EX_BuiltUnit,
                        Enable = true,
                    });

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ExperienceFileId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ExperienceFileId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_ExperienceFile
                        },
                        Value = oDataToUpsert.EX_ExperienceFile,
                        Enable = true,
                    });

                    lstUsedFiles.Add(oDataToUpsert.EX_ExperienceFile);

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ContractSubjectId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ContractSubjectId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractSubject
                        },
                        Value = oDataToUpsert.EX_ContractSubject,
                        Enable = true,
                    });

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_EconomicActivityId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_EconomicActivityId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_EconomicActivity
                        },
                        LargeValue = string.Join(",", oDataToUpsert.EX_EconomicActivity.Select(x => x.EconomicActivityId).Distinct().ToList()),
                        Enable = true,
                    });

                    oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_CustomEconomicActivityId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_CustomEconomicActivityId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_CustomEconomicActivity
                        },
                        LargeValue = string.Join(",", oDataToUpsert.EX_CustomEconomicActivity.Select(x => x.EconomicActivityId).Distinct().ToList()),
                        Enable = true,
                    });
                }

                oProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CommercialUpsert(oProvider);

                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oActivity = null;
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCustomActivity = null;

                if (CommercialType == ((int)BackOffice.Models.General.enumCommercialType.Experience).ToString())
                {
                    oActivity = ProveedoresOnLine.Company.Controller.Company.CategorySearchByActivity(null, 0, 0);
                    oCustomActivity = ProveedoresOnLine.Company.Controller.Company.CategorySearchByCustomActivity(null, 0, 0);
                }

                oReturn = new Models.Provider.ProviderCommercialViewModel
                    (oProvider.RelatedCommercial.FirstOrDefault(), oActivity, oCustomActivity);

                //register used files
                LogManager.ClientLog.FileUsedCreate(lstUsedFiles);
            }
            return oReturn;
        }

        #endregion

        #region HSEQ
        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Provider.ProviderHSEQViewModel> HIHSEQGetByType
            (string HIHSEQGetByType,
            string ProviderPublicId,
            string HSEQType)
        {
            List<BackOffice.Models.Provider.ProviderHSEQViewModel> oReturn = new List<Models.Provider.ProviderHSEQViewModel>();

            if (HIHSEQGetByType == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertficationGetBasicInfo
                    (ProviderPublicId,
                    string.IsNullOrEmpty(HSEQType) ? null : (int?)Convert.ToInt32(HSEQType.Trim()));

                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oRule = null;
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCompanyRule = null;
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oARL = null;

                if (oCertification != null)
                {

                    if (HSEQType == ((int)BackOffice.Models.General.enumHSEQType.Certifications).ToString())
                    {
                        oRule = ProveedoresOnLine.Company.Controller.Company.CategorySearchByRules(null, 0, 0);
                        oCompanyRule = ProveedoresOnLine.Company.Controller.Company.CategorySearchByCompanyRules(null, 0, 0);
                    }
                    else if (HSEQType == ((int)BackOffice.Models.General.enumHSEQType.CompanyRiskPolicies).ToString())
                    {
                        oARL = ProveedoresOnLine.Company.Controller.Company.CategorySearchByARLCompany(null, 0, 0);
                    }

                    oCertification.All(x =>
                        {
                            oReturn.Add(new BackOffice.Models.Provider.ProviderHSEQViewModel(x, oRule, oCompanyRule, oARL));
                            return true;
                        });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Provider.ProviderHSEQViewModel HIHSEQUpsert
            (string HIHSEQUpsert,
            string ProviderPublicId,
            string HSEQType)
        {
            BackOffice.Models.Provider.ProviderHSEQViewModel oReturn = null;

            if (HIHSEQUpsert == "true" &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]) &&
                !string.IsNullOrEmpty(HSEQType))
            {
                List<string> lstUsedFiles = new List<string>();

                BackOffice.Models.Provider.ProviderHSEQViewModel oDataToUpsert =
                    (BackOffice.Models.Provider.ProviderHSEQViewModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
                                typeof(BackOffice.Models.Provider.ProviderHSEQViewModel));

                ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = ProviderPublicId,
                    },
                    RelatedCertification = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>()
                    {
                        new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = string.IsNullOrEmpty(oDataToUpsert.CertificationId) ? 0 : Convert.ToInt32(oDataToUpsert.CertificationId.Trim()),
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = Convert.ToInt32(HSEQType.Trim()),
                            },
                            ItemName = oDataToUpsert.CertificationName,
                            Enable = oDataToUpsert.Enable,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        },
                    },
                };

                if (oProvider.RelatedCertification.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumHSEQType.Certifications)
                {
                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.C_CertificationCompanyId) ? 0 : Convert.ToInt32(oDataToUpsert.C_CertificationCompanyId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.C_CertificationCompany
                        },
                        Value = oDataToUpsert.C_CertificationCompany,
                        Enable = true,
                    });
                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.C_RuleId) ? 0 : Convert.ToInt32(oDataToUpsert.C_RuleId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.C_Rule
                        },
                        Value = oDataToUpsert.C_Rule,
                        Enable = true,
                    });
                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.C_StartDateCertificationId) ? 0 : Convert.ToInt32(oDataToUpsert.C_StartDateCertificationId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.C_StartDateCertification
                        },
                        Value = string.IsNullOrEmpty(oDataToUpsert.C_StartDateCertification) ?
                            string.Empty :
                            oDataToUpsert.C_StartDateCertification.Replace(" ", "").Length == BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value.Replace(" ", "").Length ?
                            oDataToUpsert.C_StartDateCertification :
                            DateTime.ParseExact(oDataToUpsert.C_StartDateCertification, BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value, System.Globalization.CultureInfo.InvariantCulture).
                            ToString(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value),
                        Enable = true,
                    });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.C_EndDateCertificationId) ? 0 : Convert.ToInt32(oDataToUpsert.C_EndDateCertificationId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.C_EndDateCertification
                        },
                        Value = string.IsNullOrEmpty(oDataToUpsert.C_EndDateCertification) ?
                            string.Empty :
                            oDataToUpsert.C_EndDateCertification.Replace(" ", "").Length == BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value.Replace(" ", "").Length ?
                            oDataToUpsert.C_EndDateCertification :
                            DateTime.ParseExact(oDataToUpsert.C_EndDateCertification, BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value, System.Globalization.CultureInfo.InvariantCulture).
                            ToString(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value),
                        Enable = true,
                    });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.C_CCSId) ? 0 : Convert.ToInt32(oDataToUpsert.C_CCSId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.C_CCS
                        },
                        Value = oDataToUpsert.C_CCS,
                        Enable = true,
                    });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.C_CertificationFileId) ? 0 : Convert.ToInt32(oDataToUpsert.C_CertificationFileId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.C_CertificationFile
                        },
                        Value = oDataToUpsert.C_CertificationFile,
                        Enable = true,
                    });

                    lstUsedFiles.Add(oDataToUpsert.C_CertificationFile);

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.C_ScopeId) ? 0 : Convert.ToInt32(oDataToUpsert.C_ScopeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.C_Scope
                        },
                        Value = oDataToUpsert.C_Scope,
                        Enable = true,
                    });
                }
                else if (oProvider.RelatedCertification.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumHSEQType.CompanyHealtyPolitic)
                {
                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CH_YearId) ? 0 : Convert.ToInt32(oDataToUpsert.CH_YearId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CH_Year
                        },
                        Value = oDataToUpsert.CH_Year,
                        Enable = true,
                    });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CH_PoliticsSecurityId) ? 0 : Convert.ToInt32(oDataToUpsert.CH_PoliticsSecurityId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CH_PoliticsSecurity
                            },
                            Value = oDataToUpsert.CH_PoliticsSecurity,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.CH_PoliticsSecurity);

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CH_PoliticsNoAlcoholId) ? 0 : Convert.ToInt32(oDataToUpsert.CH_PoliticsNoAlcoholId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CH_PoliticsNoAlcohol
                            },
                            Value = oDataToUpsert.CH_PoliticsNoAlcohol,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.CH_PoliticsNoAlcohol);

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CH_ProgramOccupationalHealthId) ? 0 : Convert.ToInt32(oDataToUpsert.CH_ProgramOccupationalHealthId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CH_ProgramOccupationalHealth
                            },
                            Value = oDataToUpsert.CH_ProgramOccupationalHealth,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.CH_ProgramOccupationalHealth);

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CH_RuleIndustrialSecurityId) ? 0 : Convert.ToInt32(oDataToUpsert.CH_RuleIndustrialSecurityId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CH_RuleIndustrialSecurity
                            },
                            Value = oDataToUpsert.CH_RuleIndustrialSecurity,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.CH_RuleIndustrialSecurity);

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CH_MatrixRiskControlId) ? 0 : Convert.ToInt32(oDataToUpsert.CH_MatrixRiskControlId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CH_MatrixRiskControl
                            },
                            Value = oDataToUpsert.CH_MatrixRiskControl,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.CH_MatrixRiskControl);

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CH_CorporateSocialResponsabilityId) ? 0 : Convert.ToInt32(oDataToUpsert.CH_CorporateSocialResponsabilityId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CH_CorporateSocialResponsability
                            },
                            Value = oDataToUpsert.CH_CorporateSocialResponsability,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.CH_CorporateSocialResponsability);

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CH_ProgramEnterpriseSecurityId) ? 0 : Convert.ToInt32(oDataToUpsert.CH_ProgramEnterpriseSecurityId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CH_ProgramEnterpriseSecurity
                            },
                            Value = oDataToUpsert.CH_ProgramEnterpriseSecurity,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.CH_ProgramEnterpriseSecurity);

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CH_PoliticsRecruimentId) ? 0 : Convert.ToInt32(oDataToUpsert.CH_PoliticsRecruimentId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CH_PoliticsRecruiment
                            },
                            Value = oDataToUpsert.CH_PoliticsRecruiment,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.CH_PoliticsRecruiment);

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CH_CertificationsFormId) ? 0 : Convert.ToInt32(oDataToUpsert.CH_CertificationsFormId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CH_CertificationsForm
                            },
                            Value = oDataToUpsert.CH_CertificationsForm,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.CH_CertificationsForm);
                }
                else if (oProvider.RelatedCertification.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumHSEQType.CompanyRiskPolicies)
                {
                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CR_SystemOccupationalHazardsId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_SystemOccupationalHazardsId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CR_SystemOccupationalHazards
                            },
                            Value = oDataToUpsert.CR_SystemOccupationalHazards,
                            Enable = true,
                        });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CR_RateARLId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_RateARLId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CR_RateARL
                            },
                            Value = oDataToUpsert.CR_RateARL,
                            Enable = true,
                        });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CR_CertificateAffiliateARLId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_CertificateAffiliateARLId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CR_CertificateAffiliateARL
                            },
                            Value = oDataToUpsert.CR_CertificateAffiliateARL,
                            Enable = true,
                        });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CR_CertificateAccidentARLId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_CertificateAccidentARLId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CR_CertificateAccidentARL
                            },
                            Value = oDataToUpsert.CR_CertificateAccidentARL,
                            Enable = true,
                        });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CR_YearId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_YearId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CR_Year
                            },
                            Value = oDataToUpsert.CR_Year,
                            Enable = true,
                        });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CR_ManHoursWorkedId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_ManHoursWorkedId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CR_ManHoursWorked
                            },
                            Value = oDataToUpsert.CR_ManHoursWorked,
                            Enable = true,
                        });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CR_FatalitiesId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_FatalitiesId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CR_Fatalities
                            },
                            Value = oDataToUpsert.CR_Fatalities,
                            Enable = true,
                        });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CR_NumberAccidentId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_NumberAccidentId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CR_NumberAccident
                            },
                            Value = oDataToUpsert.CR_NumberAccident,
                            Enable = true,
                        });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CR_NumberAccidentDisablingId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_NumberAccidentDisablingId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CR_NumberAccidentDisabling
                            },
                            Value = oDataToUpsert.CR_NumberAccidentDisabling,
                            Enable = true,
                        });

                    oProvider.RelatedCertification.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CR_DaysIncapacityId) ? 0 : Convert.ToInt32(oDataToUpsert.CR_DaysIncapacityId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumHSEQInfoType.CR_DaysIncapacity
                            },
                            Value = oDataToUpsert.CR_DaysIncapacity,
                            Enable = true,
                        });
                }

                oProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertificationUpsert(oProvider);

                //register used files
                LogManager.ClientLog.FileUsedCreate(lstUsedFiles);
            }

            return oReturn;
        }

        #endregion

        #region Finantial Info

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Provider.ProviderFinancialViewModel> FIFinancialGetByType
            (string FIFinancialGetByType,
            string ProviderPublicId,
            string FinancialType)
        {
            List<BackOffice.Models.Provider.ProviderFinancialViewModel> oReturn = new List<Models.Provider.ProviderFinancialViewModel>();

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oBank = null;

            if (FIFinancialGetByType == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oFinancial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.FinancialGetBasicInfo
                    (ProviderPublicId,
                    string.IsNullOrEmpty(FinancialType) ? null : (int?)Convert.ToInt32(FinancialType.Trim()));

                if (FinancialType == ((int)BackOffice.Models.General.enumFinancialType.BankInfoType).ToString())
                {
                    oBank = ProveedoresOnLine.Company.Controller.Company.CategorySearchByBank(null, 0, 0);
                }

                if (oFinancial != null)
                {
                    oFinancial.All(x =>
                    {
                        oReturn.Add(new BackOffice.Models.Provider.ProviderFinancialViewModel(x, oBank));
                        return true;
                    });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Provider.ProviderBalanceSheetViewModel> FIBalanceSheetGetByFinancial
            (string FIBalanceSheetGetByFinancial,
            string FinancialId)
        {
            List<BackOffice.Models.Provider.ProviderBalanceSheetViewModel> oReturn = new List<Models.Provider.ProviderBalanceSheetViewModel>();

            if (FIBalanceSheetGetByFinancial == "true")
            {
                //get account info
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> olstAccount =
                    ProveedoresOnLine.Company.Controller.Company.CategoryGetFinantialAccounts();

                List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel> olstBalanceSheetDetail = null;

                if (!string.IsNullOrEmpty(FinancialId))
                    olstBalanceSheetDetail = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetGetByFinancial(Convert.ToInt32(FinancialId));

                if (olstBalanceSheetDetail == null)
                    olstBalanceSheetDetail = new List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel>();

                if (olstAccount != null && olstAccount.Count > 0)
                {
                    oReturn = GetBalanceSheetViewModel(null, olstAccount, olstBalanceSheetDetail);
                }
            }

            return oReturn;
        }

        //recursive hierarchy get account
        private List<BackOffice.Models.Provider.ProviderBalanceSheetViewModel> GetBalanceSheetViewModel
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAccount,
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> lstAccount,
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel> lstBalanceSheetDetail)
        {
            List<BackOffice.Models.Provider.ProviderBalanceSheetViewModel> oReturn = new List<Models.Provider.ProviderBalanceSheetViewModel>();

            lstAccount.
                Where(ac => RelatedAccount != null ?
                        (ac.ParentItem != null && ac.ParentItem.ItemId == RelatedAccount.ItemId) :
                        (ac.ParentItem == null)).
                OrderBy(ac => ac.ItemInfo.
                    Where(aci => aci.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.AI_Order).
                    Select(aci => Convert.ToInt32(aci.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault()).
                All(ac =>
                {
                    BackOffice.Models.Provider.ProviderBalanceSheetViewModel oItemToAdd =
                        new Models.Provider.ProviderBalanceSheetViewModel
                            (ac,
                            lstBalanceSheetDetail.Where(bsd => bsd.RelatedAccount.ItemId == ac.ItemId).FirstOrDefault());

                    oItemToAdd.ChildBalanceSheet = GetBalanceSheetViewModel(ac, lstAccount, lstBalanceSheetDetail);
                    oReturn.Add(oItemToAdd);

                    return true;
                });

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Provider.ProviderFinancialViewModel FIFinancialUpsert
            (string FIFinancialUpsert,
            string ProviderPublicId,
            string FinancialType)
        {
            BackOffice.Models.Provider.ProviderFinancialViewModel oReturn = null;

            if (FIFinancialUpsert == "true" &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]) &&
                !string.IsNullOrEmpty(FinancialType))
            {
                List<string> lstUsedFiles = new List<string>();

                BackOffice.Models.Provider.ProviderFinancialViewModel oDataToUpsert =
                    (BackOffice.Models.Provider.ProviderFinancialViewModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
                                typeof(BackOffice.Models.Provider.ProviderFinancialViewModel));

                ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = ProviderPublicId,
                    },
                    RelatedFinantial = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>()
                    {
                        new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = string.IsNullOrEmpty(oDataToUpsert.FinancialId) ? 0 : Convert.ToInt32(oDataToUpsert.FinancialId.Trim()),
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = Convert.ToInt32(FinancialType.Trim()),
                            },
                            ItemName = oDataToUpsert.FinancialName,
                            Enable = oDataToUpsert.Enable,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        },
                    },
                };

                if (oProvider.RelatedFinantial.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumFinancialType.IncomeStatementInfoType)
                {
                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IS_YearId) ? 0 : Convert.ToInt32(oDataToUpsert.IS_YearId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IS_Year,
                            },
                            Value = oDataToUpsert.IS_Year,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IS_GrossIncomeId) ? 0 : Convert.ToInt32(oDataToUpsert.IS_GrossIncomeId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IS_GrossIncome,
                            },
                            Value = oDataToUpsert.IS_GrossIncome,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IS_NetIncomeId) ? 0 : Convert.ToInt32(oDataToUpsert.IS_NetIncomeId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IS_NetIncome,
                            },
                            Value = oDataToUpsert.IS_NetIncome,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IS_GrossEstateId) ? 0 : Convert.ToInt32(oDataToUpsert.IS_GrossEstateId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IS_GrossEstate,
                            },
                            Value = oDataToUpsert.IS_GrossEstate,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IS_LiquidHeritageId) ? 0 : Convert.ToInt32(oDataToUpsert.IS_LiquidHeritageId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IS_LiquidHeritage,
                            },
                            Value = oDataToUpsert.IS_LiquidHeritage,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IS_FileIncomeStatementId) ? 0 : Convert.ToInt32(oDataToUpsert.IS_FileIncomeStatementId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IS_FileIncomeStatement,
                            },
                            Value = oDataToUpsert.IS_FileIncomeStatement,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.IS_FileIncomeStatement);
                }
                else if (oProvider.RelatedFinantial.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumFinancialType.BankInfoType)
                {
                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IB_BankId) ? 0 : Convert.ToInt32(oDataToUpsert.IB_BankId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IB_Bank,
                            },
                            Value = oDataToUpsert.IB_Bank,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IB_AccountTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.IB_AccountTypeId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountType,
                            },
                            Value = oDataToUpsert.IB_AccountType,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IB_AccountNumberId) ? 0 : Convert.ToInt32(oDataToUpsert.IB_AccountNumberId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountNumber,
                            },
                            Value = oDataToUpsert.IB_AccountNumber,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IB_AccountHolderId) ? 0 : Convert.ToInt32(oDataToUpsert.IB_AccountHolderId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountHolder,
                        },
                        Value = oDataToUpsert.IB_AccountHolder,
                        Enable = true,
                    });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IB_ABAId) ? 0 : Convert.ToInt32(oDataToUpsert.IB_ABAId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IB_ABA,
                            },
                            Value = oDataToUpsert.IB_ABA,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IB_SwiftId) ? 0 : Convert.ToInt32(oDataToUpsert.IB_SwiftId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IB_Swift,
                            },
                            Value = oDataToUpsert.IB_Swift,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IB_IBANId) ? 0 : Convert.ToInt32(oDataToUpsert.IB_IBANId.Trim()),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IB_IBAN,
                            },
                            Value = oDataToUpsert.IB_IBAN,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IB_CustomerId) ? 0 : Convert.ToInt32(oDataToUpsert.IB_CustomerId),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IB_Customer,
                            },
                            Value = oDataToUpsert.IB_Customer,
                            Enable = true,
                        });

                    oProvider.RelatedFinantial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.IB_AccountFileId) ? 0 : Convert.ToInt32(oDataToUpsert.IB_AccountFileId),
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountFile,
                            },
                            Value = oDataToUpsert.IB_AccountFile,
                            Enable = true,
                        });

                    lstUsedFiles.Add(oDataToUpsert.IB_AccountFile);
                }
                oProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.FinancialUpsert(oProvider);

                //register used files
                LogManager.ClientLog.FileUsedCreate(lstUsedFiles);
            }

            return oReturn;
        }

        //[HttpPost]
        //[HttpGet]
        //public BackOffice.Models.Provider.ProviderCommercialViewModel CICommercialUpsert
        //    (string CICommercialUpsert,
        //    string ProviderPublicId,
        //    string CommercialType)
        //{
        //    BackOffice.Models.Provider.ProviderCommercialViewModel oReturn = null;

        //    if (CICommercialUpsert == "true" &&
        //        !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]) &&
        //        !string.IsNullOrEmpty(CommercialType))
        //    {
        //        List<string> lstUsedFiles = new List<string>();

        //        List<Tuple<decimal, decimal>> lstCurrencyConversion = null;

        //        BackOffice.Models.Provider.ProviderCommercialViewModel oDataToUpsert =
        //            (BackOffice.Models.Provider.ProviderCommercialViewModel)
        //            (new System.Web.Script.Serialization.JavaScriptSerializer()).
        //            Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
        //                        typeof(BackOffice.Models.Provider.ProviderCommercialViewModel));


        //        ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
        //        {
        //            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
        //            {
        //                CompanyPublicId = ProviderPublicId,
        //            },

        //            RelatedCommercial = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>() 
        //            {
        //                new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
        //                {
        //                    ItemId = string.IsNullOrEmpty(oDataToUpsert.CommercialId) ? 0 : Convert.ToInt32(oDataToUpsert.CommercialId.Trim()),
        //                    ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                    {
        //                        ItemId = Convert.ToInt32(CommercialType.Trim()),
        //                    },
        //                    ItemName = oDataToUpsert.CommercialName,
        //                    Enable = oDataToUpsert.Enable,
        //                    ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
        //                },
        //            },
        //        };

        //        if (oProvider.RelatedCommercial.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumCommercialType.Experience)
        //        {
        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ContractTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ContractTypeId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractType
        //                },
        //                Value = oDataToUpsert.EX_ContractType,
        //                Enable = true,
        //            });

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_DateIssueId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_DateIssueId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_DateIssue
        //                },
        //                Value = string.IsNullOrEmpty(oDataToUpsert.EX_DateIssue) ?
        //                    string.Empty :
        //                    oDataToUpsert.EX_DateIssue.Replace(" ", "").Length == BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value.Replace(" ", "").Length ?
        //                    oDataToUpsert.EX_DateIssue :
        //                    DateTime.ParseExact(
        //                        oDataToUpsert.EX_DateIssue,
        //                        BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value,
        //                        System.Globalization.CultureInfo.InvariantCulture).
        //                    ToString(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value),

        //                Enable = true,
        //            });

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_DueDateId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_DueDateId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_DueDate
        //                },
        //                Value = string.IsNullOrEmpty(oDataToUpsert.EX_DueDate) ?
        //                    string.Empty :
        //                    oDataToUpsert.EX_DueDate.Replace(" ", "").Length == BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value.Replace(" ", "").Length ?
        //                    oDataToUpsert.EX_DueDate :
        //                    DateTime.ParseExact(
        //                        oDataToUpsert.EX_DueDate,
        //                        BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value,
        //                        System.Globalization.CultureInfo.InvariantCulture).
        //                    ToString(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value),

        //                Enable = true,
        //            });

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ClientId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ClientId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_Client
        //                },
        //                Value = oDataToUpsert.EX_Client,
        //                Enable = true,
        //            });

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ContractNumberId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ContractNumberId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractNumber
        //                },
        //                Value = oDataToUpsert.EX_ContractNumber,
        //                Enable = true,
        //            });


        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_CurrencyId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_CurrencyId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_Currency
        //                },
        //                Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value.Replace(" ", ""),
        //                Enable = true,
        //            });


        //            //get experience value
        //            if (BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value == oDataToUpsert.EX_Currency)
        //            {
        //                //value in default rate
        //                lstCurrencyConversion = new List<Tuple<decimal, decimal>>() 
        //                { 
        //                    new Tuple<decimal, decimal>(Convert.ToDecimal(oDataToUpsert.EX_ContractValue, System.Globalization.CultureInfo.InvariantCulture), Convert.ToDecimal(oDataToUpsert.EX_ContractValue, System.Globalization.CultureInfo.InvariantCulture))
        //                };
        //            }
        //            else
        //            {
        //                //value in custom rate
        //                lstCurrencyConversion =
        //                string.IsNullOrEmpty(oDataToUpsert.EX_ContractValue) || string.IsNullOrEmpty(oDataToUpsert.EX_Currency) || string.IsNullOrEmpty(oDataToUpsert.EX_DateIssue) ?
        //                    new List<Tuple<decimal, decimal>>() { new Tuple<decimal, decimal>(0, 0) } :
        //                    BackOffice.Web.Controllers.BaseController.Currency_ConvertToStandar
        //                        (Convert.ToInt32(oDataToUpsert.EX_Currency),
        //                        Convert.ToInt32(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value),
        //                        (oDataToUpsert.EX_DateIssue.Replace(" ", "").Length == BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value.Replace(" ", "").Length ?
        //                            DateTime.ParseExact(
        //                                oDataToUpsert.EX_DateIssue,
        //                                BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value,
        //                                System.Globalization.CultureInfo.InvariantCulture) :
        //                            DateTime.ParseExact(
        //                                oDataToUpsert.EX_DateIssue,
        //                                BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value,
        //                                System.Globalization.CultureInfo.InvariantCulture)).Year,
        //                        new List<decimal>() { Convert.ToDecimal(oDataToUpsert.EX_ContractValue, System.Globalization.CultureInfo.InvariantCulture) });
        //            }

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ContractValueId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ContractValueId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractValue
        //                },
        //                Value = (lstCurrencyConversion != null && lstCurrencyConversion.Count > 0 ? lstCurrencyConversion.FirstOrDefault().Item2 : 0).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
        //                Enable = true,
        //            });

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_PhoneId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_PhoneId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_Phone
        //                },
        //                Value = oDataToUpsert.EX_Phone,
        //                Enable = true,
        //            });

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_BuiltAreaId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_BuiltAreaId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_BuiltArea
        //                },
        //                Value = oDataToUpsert.EX_BuiltArea,
        //                Enable = true,
        //            });

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_BuiltUnitId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_BuiltUnitId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_BuiltUnit
        //                },
        //                Value = oDataToUpsert.EX_BuiltUnit,
        //                Enable = true,
        //            });

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ExperienceFileId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ExperienceFileId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_ExperienceFile
        //                },
        //                Value = oDataToUpsert.EX_ExperienceFile,
        //                Enable = true,
        //            });

        //            lstUsedFiles.Add(oDataToUpsert.EX_ExperienceFile);

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_ContractSubjectId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_ContractSubjectId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractSubject
        //                },
        //                Value = oDataToUpsert.EX_ContractSubject,
        //                Enable = true,
        //            });

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_EconomicActivityId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_EconomicActivityId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_EconomicActivity
        //                },
        //                LargeValue = string.Join(",", oDataToUpsert.EX_EconomicActivity.Select(x => x.EconomicActivityId).Distinct().ToList()),
        //                Enable = true,
        //            });

        //            oProvider.RelatedCommercial.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
        //            {
        //                ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.EX_CustomEconomicActivityId) ? 0 : Convert.ToInt32(oDataToUpsert.EX_CustomEconomicActivityId.Trim()),
        //                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
        //                {
        //                    ItemId = (int)BackOffice.Models.General.enumCommercialInfoType.EX_CustomEconomicActivity
        //                },
        //                LargeValue = string.Join(",", oDataToUpsert.EX_CustomEconomicActivity.Select(x => x.EconomicActivityId).Distinct().ToList()),
        //                Enable = true,
        //            });
        //        }

        //        oProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CommercialUpsert(oProvider);

        //        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oActivity = null;
        //        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCustomActivity = null;

        //        if (CommercialType == ((int)BackOffice.Models.General.enumCommercialType.Experience).ToString())
        //        {
        //            oActivity = ProveedoresOnLine.Company.Controller.Company.CategorySearchByActivity(null, 0, 0);
        //            oCustomActivity = ProveedoresOnLine.Company.Controller.Company.CategorySearchByCustomActivity(null, 0, 0);
        //        }

        //        oReturn = new Models.Provider.ProviderCommercialViewModel
        //            (oProvider.RelatedCommercial.FirstOrDefault(), oActivity, oCustomActivity);

        //        //register used files
        //        LogManager.ClientLog.FileUsedCreate(lstUsedFiles);
        //    }
        //    return oReturn;
        //}

        #endregion

        #region Legal Info

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Provider.ProviderLegalViewModel> LILegalInfoGetByType
            (string LILegalInfoGetByType,
            string ProviderPublicId,
            string LegalInfoType)
        {
            List<BackOffice.Models.Provider.ProviderLegalViewModel> oReturn = new List<Models.Provider.ProviderLegalViewModel>();

            if (LILegalInfoGetByType == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oLegalInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo
                    (ProviderPublicId,
                    string.IsNullOrEmpty(LegalInfoType) ? null : (int?)Convert.ToInt32(LegalInfoType.Trim()));

                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oEconomiActivity = null;
                if (LegalInfoType == ((int)BackOffice.Models.General.enumLegalType.RUT).ToString())
                {
                    oEconomiActivity = ProveedoresOnLine.Company.Controller.Company.CategorySearchByActivity(null, 0, 0);
                }
                if (oLegalInfo != null)
                {
                    oLegalInfo.All(x =>
                    {
                        oReturn.Add(new BackOffice.Models.Provider.ProviderLegalViewModel(x, oEconomiActivity));
                        return true;
                    });
                }
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Provider.ProviderLegalViewModel LILegalUpsert
            (string LILegalInfoUpsert,
            string ProviderPublicId,
            string LegalInfoType, string LegalId)
        {
            BackOffice.Models.Provider.ProviderLegalViewModel oReturn = null;

            if (LILegalInfoUpsert == "true" &&
               !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]) &&
               !string.IsNullOrEmpty(LegalInfoType))
            {
                BackOffice.Models.Provider.ProviderLegalViewModel oDataToUpsert =
                    (BackOffice.Models.Provider.ProviderLegalViewModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
                                typeof(BackOffice.Models.Provider.ProviderLegalViewModel));

                ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = ProviderPublicId,
                    },
                    RelatedLegal = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>()
                    {
                        new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = string.IsNullOrEmpty(oDataToUpsert.LegalId) ? 0 : Convert.ToInt32(oDataToUpsert.LegalId.Trim()),
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = Convert.ToInt32(LegalInfoType.Trim()),
                            },
                            ItemName = oDataToUpsert.LegalName,
                            Enable = oDataToUpsert.Enable,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        },
                    },
                };

                #region Designations
                if (oProvider.RelatedLegal.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumLegalType.Designations)
                {
                    ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProviderDesignations = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                    {
                        RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                        {
                            CompanyPublicId = ProviderPublicId,
                        },
                        RelatedLegal = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>()
                    {
                        new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = string.IsNullOrEmpty(oDataToUpsert.LegalId) ? 0 : Convert.ToInt32(oDataToUpsert.LegalId.Trim()),
                            ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = Convert.ToInt32(LegalInfoType.Trim()),
                            },
                            ItemName = "Designations",
                            Enable = oDataToUpsert.Enable,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        },
                    },
                    };

                    oProviderDesignations.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CD_PartnerNameId) ? 0 : Convert.ToInt32(oDataToUpsert.CD_PartnerNameId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.CD_PartnerName
                        },
                        Value = oDataToUpsert.CD_PartnerName,
                        Enable = oDataToUpsert.Enable,
                    });

                    oProviderDesignations.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CD_PartnerIdentificationNumberId) ? 0 : Convert.ToInt32(oDataToUpsert.CD_PartnerIdentificationNumberId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.CD_PartnerIdentificationNumber
                        },
                        Value = oDataToUpsert.CD_PartnerIdentificationNumber,
                        Enable = oDataToUpsert.Enable,

                    });
                    oProviderDesignations.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CD_PartnerRankId) ? 0 : Convert.ToInt32(oDataToUpsert.CD_PartnerRankId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.CD_PartnerRank
                        },
                        Value = oDataToUpsert.CD_PartnerRank,
                        Enable = oDataToUpsert.Enable,
                    });

                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalUpsert(oProviderDesignations);
                }
                #endregion

                #region RUT
                if (oProvider.RelatedLegal.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumLegalType.RUT)
                {
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_PersonTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.R_PersonTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_PersonType
                        },
                        Value = oDataToUpsert.R_PersonType,
                        Enable = oDataToUpsert.Enable,
                    });

                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_LargeContributorId) ? 0 : Convert.ToInt32(oDataToUpsert.R_LargeContributorId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_LargeContributor
                        },
                        Value = oDataToUpsert.R_LargeContributor.ToString(),
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_LargeContributorReceiptId) ? 0 : Convert.ToInt32(oDataToUpsert.R_LargeContributorReceiptId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_LargeContributorReceipt
                        },
                        Value = oDataToUpsert.R_LargeContributorReceipt,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_LargeContributorDateId) ? 0 : Convert.ToInt32(oDataToUpsert.R_LargeContributorDateId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_LargeContributorDate
                        },
                        Value = oDataToUpsert.R_LargeContributorDate,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_SelfRetainerId) ? 0 : Convert.ToInt32(oDataToUpsert.R_SelfRetainerId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_SelfRetainer
                        },
                        Value = oDataToUpsert.R_SelfRetainer.ToString(),
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_SelfRetainerRecieptId) ? 0 : Convert.ToInt32(oDataToUpsert.R_SelfRetainerRecieptId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_SelfRetainerReciept
                        },
                        Value = oDataToUpsert.R_SelfRetainerReciept,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_SelfRetainerDateId) ? 0 : Convert.ToInt32(oDataToUpsert.R_SelfRetainerDateId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_SelfRetainerDate
                        },
                        Value = oDataToUpsert.R_SelfRetainerDate,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_EntityTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.R_EntityTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_EntityType
                        },
                        Value = oDataToUpsert.R_EntityType,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_IVAId) ? 0 : Convert.ToInt32(oDataToUpsert.R_IVAId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_IVA
                        },
                        Value = oDataToUpsert.R_IVA.ToString(),
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_TaxPayerTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.R_TaxPayerTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_TaxPayerType
                        },
                        Value = oDataToUpsert.R_TaxPayerType,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_ICAId) ? 0 : Convert.ToInt32(oDataToUpsert.R_ICAId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_ICA
                        },
                        Value = oDataToUpsert.R_ICA,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_RUTFileId) ? 0 : Convert.ToInt32(oDataToUpsert.R_RUTFileId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_RUTFile
                        },
                        Value = oDataToUpsert.R_RUTFile,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_LargeContributorFileId) ? 0 : Convert.ToInt32(oDataToUpsert.R_LargeContributorFileId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_LargeContributorFile
                        },
                        Value = oDataToUpsert.R_LargeContributorFile,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.R_SelfRetainerFileId) ? 0 : Convert.ToInt32(oDataToUpsert.R_SelfRetainerFileId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.R_SelfRetainerFile
                        },
                        Value = oDataToUpsert.R_SelfRetainerFile,
                        Enable = oDataToUpsert.Enable,
                    });

                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalUpsert(oProvider);
                };
                #endregion

                #region CIFIN
                if (oProvider.RelatedLegal.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumLegalType.CIFIN)
                {
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CF_QueryDateId) ? 0 : Convert.ToInt32(oDataToUpsert.CF_QueryDateId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.CF_QueryDate
                        },
                        Value = oDataToUpsert.CF_QueryDate,
                        Enable = oDataToUpsert.Enable,
                    });

                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CF_ResultQueryId) ? 0 : Convert.ToInt32(oDataToUpsert.CF_ResultQueryId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.CF_ResultQuery
                        },
                        Value = oDataToUpsert.CF_ResultQuery,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CF_AutorizationFileId) ? 0 : Convert.ToInt32(oDataToUpsert.CF_AutorizationFileId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.CF_AutorizationFile
                        },
                        Value = oDataToUpsert.CF_AutorizationFile,
                        Enable = oDataToUpsert.Enable,
                    });

                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalUpsert(oProvider);
                };
                #endregion

                #region SARLAFT
                if (oProvider.RelatedLegal.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumLegalType.SARLAFT)
                {
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.SF_ProcessDateId) ? 0 : Convert.ToInt32(oDataToUpsert.SF_ProcessDateId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.SF_ProcessDate
                        },
                        Value = oDataToUpsert.SF_ProcessDate,
                        Enable = oDataToUpsert.Enable,
                    });

                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.SF_PersonTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.SF_PersonTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.SF_PersonType
                        },
                        Value = oDataToUpsert.SF_PersonType,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.SF_SARLAFTFileId) ? 0 : Convert.ToInt32(oDataToUpsert.SF_SARLAFTFileId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.SF_SARLAFTFile
                        },
                        Value = oDataToUpsert.SF_SARLAFTFile,
                        Enable = oDataToUpsert.Enable,
                    });

                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalUpsert(oProvider);
                };
                #endregion

                #region Resolutions
                if (oProvider.RelatedLegal.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumLegalType.Resoluciones)
                {
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.RS_EntityTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.RS_EntityTypeId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.RS_EntityType
                        },
                        Value = oDataToUpsert.RS_EntityType,
                        Enable = oDataToUpsert.Enable,
                    });

                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.RS_ResolutionFileId) ? 0 : Convert.ToInt32(oDataToUpsert.RS_ResolutionFileId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.RS_ResolutionFile
                        },
                        Value = oDataToUpsert.RS_ResolutionFile,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.RS_StartDateId) ? 0 : Convert.ToInt32(oDataToUpsert.RS_StartDateId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.RS_StartDate
                        },
                        Value = oDataToUpsert.RS_StartDate,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.RS_EndDateId) ? 0 : Convert.ToInt32(oDataToUpsert.RS_EndDateId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.RS_EndDate
                        },
                        Value = oDataToUpsert.RS_EndDate,
                        Enable = oDataToUpsert.Enable,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.RS_DescriptionId) ? 0 : Convert.ToInt32(oDataToUpsert.RS_DescriptionId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.RS_Description
                        },
                        Value = oDataToUpsert.RS_Description,
                        Enable = oDataToUpsert.Enable,
                    });

                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalUpsert(oProvider);
                };
                #endregion
            }

            return oReturn;
        }
        #endregion
    }
}
