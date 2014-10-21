using DocumentManagement.Provider.Models;
using DocumentManagement.Provider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Controller
{
    public class Provider
    {
        static public string ProviderUpsert(ProviderModel ProviderToUpsert)
        {
            string oResult = DAL.Controller.ProviderDataController.Instance.ProviderUpsert
                (ProviderToUpsert.ProviderPublicId
                ,ProviderToUpsert.Name
                ,ProviderToUpsert.IdentificationType.ItemId
                ,ProviderToUpsert.IdentificationNumber
                ,ProviderToUpsert.Email);            

            if (ProviderToUpsert.RelatedProviderInfo.Count > 0)
            {
                foreach (var item in ProviderToUpsert.RelatedProviderInfo)
                {
                    DAL.Controller.ProviderDataController.Instance.ProviderInfoUpsert
                        (ProviderToUpsert.ProviderPublicId,
                        item.ProviderInfoId,
                        item.ProviderInfoType.ItemId,
                        item.Value, item.LargeValue);
                }
            }
            else if (ProviderToUpsert.RelatedProviderCustomerInfo.Count > 0)
            {
                foreach (var item in ProviderToUpsert.RelatedProviderCustomerInfo)
                {
                    DAL.Controller.ProviderDataController.Instance.ProviderCustomerInfoUpsert
                        (ProviderToUpsert.ProviderPublicId,
                        ProviderToUpsert.CustomerPublicId,
                        item.ProviderInfoId,
                        item.ProviderInfoType.ItemId,
                        item.Value, item.LargeValue);
                }
            }

            return oResult;
        }

        static public List<ProviderModel> ProviderSearch(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderSearch(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public static ProviderModel ProviderGetByIdentification(string IdentificationNumber, int IdenificationTypeId, string CustomerPublicId)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderGetByIdentification(IdentificationNumber, IdenificationTypeId, CustomerPublicId);
        }

        public static ProviderModel ProviderGetById(string ProviderPublicId, int? StepId)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderGetById(ProviderPublicId, StepId);
        }

    }
}
