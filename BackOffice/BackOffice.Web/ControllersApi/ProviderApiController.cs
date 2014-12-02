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
        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Provider.ProviderContactViewModel> ContactGetByType
            (string ContactGetByType,
            string ProviderPublicId,
            string ContactType)
        {
            List<BackOffice.Models.Provider.ProviderContactViewModel> oReturn = new List<Models.Provider.ProviderContactViewModel>();

            if (ContactGetByType == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oContact = ProveedoresOnLine.Company.Controller.Company.ContactGetBasicInfo
                    (ProviderPublicId,
                    string.IsNullOrEmpty(ContactType) ? null : (int?)Convert.ToInt32(ContactType.Trim()));

                if (oContact != null)
                {
                    oContact.All(x =>
                    {
                        oReturn.Add(new BackOffice.Models.Provider.ProviderContactViewModel(x));
                        return true;
                    });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public BackOffice.Models.Provider.ProviderContactViewModel ContactUpsert
            (string ContactUpsert,
            string ProviderPublicId,
            string ContactType)
        {
            BackOffice.Models.Provider.ProviderContactViewModel oReturn = null;

            if (ContactUpsert == "true" &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]) &&
                !string.IsNullOrEmpty(ContactType))
            {
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

                oCompany = ProveedoresOnLine.Company.Controller.Company.ContactUpsert(oCompany);
                oReturn = new Models.Provider.ProviderContactViewModel(oCompany.RelatedContact.FirstOrDefault());
            }
            return oReturn;
        }
    }
}
