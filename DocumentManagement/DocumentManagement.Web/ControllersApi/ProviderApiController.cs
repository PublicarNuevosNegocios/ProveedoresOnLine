using DocumentManagement.Models.Provider;
using DocumentManagement.Provider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DocumentManagement.Web.ControllersApi
{
    public class ProviderApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public ProviderSearchModel ProviderSearch
            (string ProviderSearchVal, string SearchParam, int PageNumber, int RowCount, string CustomerPublicId, string FormPublicId, string Unique)
        {
            ProviderSearchModel oReturn = new ProviderSearchModel();

            int oTotalRows;
            List<DocumentManagement.Provider.Models.Provider.ProviderModel> oProviderlst = DocumentManagement.Provider.Controller.Provider.ProviderSearch
                (SearchParam, CustomerPublicId, FormPublicId, PageNumber, RowCount, out oTotalRows, Convert.ToBoolean(Unique));

            oReturn.TotalRows = oTotalRows;

            oReturn.RelatedProvider = new List<ProviderItemSearchModel>();
            oProviderlst.All(prv =>
            {
                #region Lead     
                prv.RelatedProviderCustomerInfo.All(y =>
                           {
                               if (y.ProviderInfoType.ItemId == 403)
                                   y.Value = DocumentManagement.Models.General.InternalSettings.Instance[DocumentManagement.Models.General.Constants.C_Settings_Path_SalesForce].Value + y.Value;
                               return true;
                           }); 
                #endregion

                oReturn.RelatedProvider.Add(new ProviderItemSearchModel()
                {
                    RelatedProvider = prv,
                    codSalesforce = prv.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 403).Select(x => x.Value).FirstOrDefault() == null ? string.Empty : prv.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 403).Select(x => x.Value).FirstOrDefault(),
                    checkDigit = prv.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 378).Select(x => x.Value).FirstOrDefault() == null ? string.Empty : prv.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 378).Select(x => x.Value).FirstOrDefault(),
                    lastModify = prv.LogCreateDate,
                    LastModifyUser = prv.LogUser,
                    CustomerInfoTypeId = prv.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 403).Select(x => x.ProviderInfoId).FirstOrDefault() == null ? 0 : prv.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 403).Select(x => x.ProviderInfoId).FirstOrDefault(),
                    checkDigitInfoId = prv.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 378).Select(x => x.ProviderInfoId).FirstOrDefault() == null ? 0 : prv.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 378).Select(x => x.ProviderInfoId).FirstOrDefault(),
                    oTotalRows = oTotalRows
                });
                return true;
            });

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public DocumentManagement.Models.Customer.FormSearchModel FormSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount)
        {
            int oTotalRows;

            DocumentManagement.Models.Customer.FormSearchModel oReturn = new DocumentManagement.Models.Customer.FormSearchModel();
            oReturn.RelatedForm = DocumentManagement.Customer.Controller.Customer.FormSearch(CustomerPublicId, SearchParam, PageNumber, RowCount, out oTotalRows);

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<LogManager.Models.LogModel> ProviderLogByPublicId
        (string ProviderLog,
            string ProviderPublicId)
        {
            List<LogManager.Models.LogModel> oReturn = null;

            if (ProviderLog == "true")
            {
                oReturn = DocumentManagement.Provider.Controller.Provider.ProviderLog(ProviderPublicId);
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<ChangesControlModel> ChangesControlSearch(string ChangesControlSearch, string SearchParam, int PageNumber, int RowCount)
        {
            int oTotalRows;

            List<ChangesControlModel> oReturn = DocumentManagement.Provider.Controller.Provider.ChangesControlSearch(SearchParam, PageNumber, RowCount, out oTotalRows);

            return oReturn; 
        }
    }
}