using System;
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
        public List<BackOffice.Models.Provider.ProviderComercialViewModel> CICommercialGetByType
            (string CICommercialGetByType,
            string ProviderPublicId,
            string CommercialType)
        {
            List<BackOffice.Models.Provider.ProviderComercialViewModel> oReturn = new List<Models.Provider.ProviderComercialViewModel>();

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
                        oReturn.Add(new BackOffice.Models.Provider.ProviderComercialViewModel(x, oActivity, oCustomActivity));
                        return true;
                    });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Provider.ProviderComercialViewModel CICommercialUpsert
            (string CICommercialUpsert,
            string ProviderPublicId,
            string CommercialType)
        {
            BackOffice.Models.Provider.ProviderComercialViewModel oReturn = null;

            //if (GIContactUpsert == "true" &&
            //    !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]) &&
            //    !string.IsNullOrEmpty(ContactType))
            //{
            //    List<string> lstUsedFiles = new List<string>();

            //    BackOffice.Models.Provider.ProviderContactViewModel oDataToUpsert =
            //        (BackOffice.Models.Provider.ProviderContactViewModel)
            //        (new System.Web.Script.Serialization.JavaScriptSerializer()).
            //        Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
            //                    typeof(BackOffice.Models.Provider.ProviderContactViewModel));

            //    ProveedoresOnLine.Company.Models.Company.CompanyModel oCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
            //    {
            //        CompanyPublicId = ProviderPublicId,
            //        RelatedContact = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>() 
            //        { 
            //            new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
            //            {
            //                ItemId = string.IsNullOrEmpty(oDataToUpsert.ContactId) ? 0 : Convert.ToInt32(oDataToUpsert.ContactId.Trim()),
            //                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //                {
            //                    ItemId = Convert.ToInt32(ContactType.Trim()),
            //                },
            //                ItemName = oDataToUpsert.ContactName,
            //                Enable = oDataToUpsert.Enable,
            //                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            //            },
            //        }
            //    };

            //    if (oCompany.RelatedContact.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumContactType.CompanyContact)
            //    {
            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CC_ValueId) ? 0 : Convert.ToInt32(oDataToUpsert.CC_ValueId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CC_Value
            //            },
            //            Value = oDataToUpsert.CC_Value,
            //            Enable = true,
            //        });
            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CC_CompanyContactTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.CC_CompanyContactTypeId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CC_CompanyContactType
            //            },
            //            Value = oDataToUpsert.CC_CompanyContactType,
            //            Enable = true,
            //        });
            //    }
            //    else if (oCompany.RelatedContact.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumContactType.PersonContact)
            //    {
            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_PersonContactTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_PersonContactTypeId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_PersonContactType
            //            },
            //            Value = oDataToUpsert.CP_PersonContactType,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_IdentificationTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_IdentificationTypeId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationType
            //            },
            //            Value = oDataToUpsert.CP_IdentificationType,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_IdentificationNumberId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_IdentificationNumberId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationNumber
            //            },
            //            Value = oDataToUpsert.CP_IdentificationNumber,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_IdentificationCityId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_IdentificationCityId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationCity
            //            },
            //            Value = oDataToUpsert.CP_IdentificationCity,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_IdentificationFileId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_IdentificationFileId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationFile
            //            },
            //            Value = oDataToUpsert.CP_IdentificationFile,
            //            Enable = true,
            //        });

            //        lstUsedFiles.Add(oDataToUpsert.CP_IdentificationFile);

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_PhoneId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_PhoneId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_Phone
            //            },
            //            Value = oDataToUpsert.CP_Phone,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_EmailId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_EmailId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_Email
            //            },
            //            Value = oDataToUpsert.CP_Email,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_NegotiationId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_NegotiationId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.CP_Negotiation
            //            },
            //            Value = oDataToUpsert.CP_Negotiation,
            //            Enable = true,
            //        });
            //    }
            //    else if (oCompany.RelatedContact.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumContactType.Brach)
            //    {
            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_RepresentativeId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_RepresentativeId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Representative
            //            },
            //            Value = oDataToUpsert.BR_Representative,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_AddressId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_AddressId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Address
            //            },
            //            Value = oDataToUpsert.BR_Address,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_CityId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_CityId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_City
            //            },
            //            Value = oDataToUpsert.BR_City,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_PhoneId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_PhoneId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Phone
            //            },
            //            Value = oDataToUpsert.BR_Phone,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_FaxId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_FaxId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Fax
            //            },
            //            Value = oDataToUpsert.BR_Fax,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_EmailId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_EmailId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Email
            //            },
            //            Value = oDataToUpsert.BR_Email,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_WebsiteId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_WebsiteId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Website
            //            },
            //            Value = oDataToUpsert.BR_Website,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_LatitudeId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_LatitudeId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Latitude
            //            },
            //            Value = oDataToUpsert.BR_Latitude,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_LongitudeId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_LongitudeId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Longitude
            //            },
            //            Value = oDataToUpsert.BR_Longitude,
            //            Enable = true,
            //        });
            //    }
            //    else if (oCompany.RelatedContact.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumContactType.Distributor)
            //    {
            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_DistributorTypeId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_DistributorTypeId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_DistributorType
            //            },
            //            Value = oDataToUpsert.DT_DistributorType,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_RepresentativeId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_RepresentativeId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_Representative
            //            },
            //            Value = oDataToUpsert.DT_Representative,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_EmailId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_EmailId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_Email
            //            },
            //            Value = oDataToUpsert.DT_Email,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_PhoneId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_PhoneId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_Phone
            //            },
            //            Value = oDataToUpsert.DT_Phone,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_CityId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_CityId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_City
            //            },
            //            Value = oDataToUpsert.DT_City,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.BR_EmailId) ? 0 : Convert.ToInt32(oDataToUpsert.BR_EmailId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.BR_Email
            //            },
            //            Value = oDataToUpsert.BR_Email,
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_DateIssueId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_DateIssueId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_DateIssue
            //            },
            //            Value = string.IsNullOrEmpty(oDataToUpsert.DT_DateIssue) ?
            //                string.Empty :
            //                DateTime.ParseExact(oDataToUpsert.DT_DateIssue, BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value, System.Globalization.CultureInfo.InvariantCulture).
            //                ToString(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value),
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_DueDateId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_DueDateId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_DueDate
            //            },
            //            Value = string.IsNullOrEmpty(oDataToUpsert.DT_DueDate) ?
            //                string.Empty :
            //                DateTime.ParseExact(oDataToUpsert.DT_DueDate, BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_KendoToServer].Value, System.Globalization.CultureInfo.InvariantCulture).
            //                ToString(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DateFormat_Server].Value),
            //            Enable = true,
            //        });

            //        oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            //        {
            //            ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.DT_DistributorFileId) ? 0 : Convert.ToInt32(oDataToUpsert.DT_DistributorFileId.Trim()),
            //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
            //            {
            //                ItemId = (int)BackOffice.Models.General.enumContactInfoType.DT_DistributorFile
            //            },
            //            Value = oDataToUpsert.DT_DistributorFile,
            //            Enable = true,
            //        });
            //    }

            //    oCompany = ProveedoresOnLine.Company.Controller.Company.ContactUpsert(oCompany);

            //    List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oCities = null;

            //    if (ContactType == ((int)BackOffice.Models.General.enumContactType.Brach).ToString())
            //    {
            //        oCities = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, null, 0, 0);
            //    }

            //    oReturn = new Models.Provider.ProviderContactViewModel(oCompany.RelatedContact.FirstOrDefault(), oCities);

            //    //register used files
            //    LogManager.ClientLog.FileUsedCreate(lstUsedFiles);
            //}
            return oReturn;
        }

        #endregion

        #region HSEQ
        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Provider.ProviderHSEQViewModel> HICertificationGetByType
            (string HICertificationGetByType,
            string ProviderPublicId,
            string CertificationType)
        {
            List<BackOffice.Models.Provider.ProviderHSEQViewModel> oReturn = new List<Models.Provider.ProviderHSEQViewModel>();

            if (HICertificationGetByType == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertficationGetBasicInfo
                    (ProviderPublicId,
                    string.IsNullOrEmpty(CertificationType) ? null : (int?)Convert.ToInt32(CertificationType.Trim()));

                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oRule = null;
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCompanyRule = null;

                if (oCertification != null)
                {

                    if (CertificationType == ((int)BackOffice.Models.General.enumHSEQType.Certifications).ToString())
                    {
                        oRule = ProveedoresOnLine.Company.Controller.Company.CategorySearchByRules(null, 0, 0);
                        oCompanyRule = ProveedoresOnLine.Company.Controller.Company.CategorySearchByCompanyRules(null, 0, 0);
                    }

                    oCertification.All(x =>
                        {
                            oReturn.Add(new BackOffice.Models.Provider.ProviderHSEQViewModel(x, oRule, oCompanyRule));
                            return true;
                        });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Provider.ProviderHSEQViewModel HICertificationUpsert
            (string HICertificationUpsert,
            string ProviderPublicId,
            string CertificationType)
        {
            BackOffice.Models.Provider.ProviderHSEQViewModel oReturn = null;

            if (HICertificationUpsert == "true" &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]) &&
                !string.IsNullOrEmpty(CertificationType))
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
                                ItemId = Convert.ToInt32(CertificationType.Trim()),
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

                if (oLegalInfo != null)
                {
                    oLegalInfo.All(x =>
                    {
                        oReturn.Add(new BackOffice.Models.Provider.ProviderLegalViewModel(x));
                        return true;
                    });
                }
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Provider.ProviderLegalViewModel LILegalInfoUpsert
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
                            ItemId = string.IsNullOrEmpty(LegalId.Trim()) ? 0 : Convert.ToInt32(LegalId.Trim()),
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

                if (oProvider.RelatedLegal.FirstOrDefault().ItemType.ItemId == (int)BackOffice.Models.General.enumLegalType.ChaimberOfCommerce)
                {
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_PartnerNameId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_PartnerNameId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.CP_PartnerName
                        },
                        Value = oDataToUpsert.CP_PartnerName,
                        Enable = true,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_PartnerIdentificationNumberId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_PartnerIdentificationNumberId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.CP_PartnerIdentificationNumber
                        },
                        Value = oDataToUpsert.CP_PartnerIdentificationNumber,
                        Enable = true,
                    });
                    oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = string.IsNullOrEmpty(oDataToUpsert.CP_PartnerRankId) ? 0 : Convert.ToInt32(oDataToUpsert.CP_PartnerRankId.Trim()),
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumLegalInfoType.CP_PartnerRank
                        },
                        Value = oDataToUpsert.CP_PartnerRank,
                        Enable = true,
                    });
                }
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalInfoUpsert(oProvider.RelatedLegal.FirstOrDefault());
            }

            return oReturn;
        }
        #endregion
    }
}
