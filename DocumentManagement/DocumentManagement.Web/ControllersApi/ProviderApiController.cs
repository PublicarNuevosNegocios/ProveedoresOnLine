using DocumentManagement.Models.Provider;
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
                (SearchParam, PageNumber, RowCount, out oTotalRows, Convert.ToBoolean(Unique));

            oReturn.TotalRows = oTotalRows;
            if (CustomerPublicId != null && FormPublicId != null)
            {
                oProviderlst = oProviderlst.Where(x => x.CustomerPublicId == CustomerPublicId
                                     && x.FormPublicId == FormPublicId).Select(x => x).ToList();          
            }
            if (CustomerPublicId != null && FormPublicId == null)
            {
                oProviderlst = oProviderlst.Where(x => x.CustomerPublicId == CustomerPublicId).Select(x => x).ToList();     
            }

            oReturn.RelatedProvider = new List<ProviderItemSearchModel>();
            oProviderlst.All(prv =>
            {
                oReturn.RelatedProvider.Add(new ProviderItemSearchModel()
                {
                    RelatedProvider = prv,
                    codSalesforce = prv.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 403).Select(x => x.Value).FirstOrDefault() == null ? string.Empty : prv.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 403).Select(x => x.Value).FirstOrDefault(),
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
    }
}