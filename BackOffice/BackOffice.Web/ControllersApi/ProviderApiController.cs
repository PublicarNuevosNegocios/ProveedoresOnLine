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

            if (ContactUpsert == "true" && !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]))
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
                                ItemId = (int)BackOffice.Models.General.enumContactType.CompanyContact,
                            },
                            ItemName = oDataToUpsert.ContactName,
                            Enable = oDataToUpsert.Enable,
                            ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                        },
                    }
                };

                if (Convert.ToInt32(ContactType.Trim()) == (int)BackOffice.Models.General.enumContactType.CompanyContact)
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
                else if (Convert.ToInt32(ContactType.Trim()) == (int)BackOffice.Models.General.enumContactType.PersonContact)
                {

                }

                oCompany = ProveedoresOnLine.Company.Controller.Company.ContactUpsert(oCompany);
                oReturn = new Models.Provider.ProviderContactViewModel(oCompany.RelatedContact.FirstOrDefault());
            }
            return oReturn;
        }
    }
}
