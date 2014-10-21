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
            //return DAL.Controller.ProviderDataController.Instance.ProviderUpsert(CustomerPublicId, ProviderPublicId, Name, IdentificationType, CustomerProviderInfoType, IdentificationNumber, Email, Status);
            return null;
        }

        static public void ProviderInfoUpsert(ProviderModel ProviderToUpsert)
        {
            //return DAL.Controller.ProviderDataController.Instance.ProviderInfoUpsert(ProviderInfoId, ProviderPublicId, ProviderInfoType, Value, LargeValue);
        }

        static public void ProviderCustomerInfoUpsert(ProviderModel ProviderToUpsert)
        {
            //return DAL.Controller.ProviderDataController.Instance.ProviderCustomerInfoUpsert(ProviderCustomerInfoId, ProviderPublicId, CustomerPublicId, ProviderCustomerInfoType, Value, LargeValue);
        }

        static public List<ProviderModel> ProviderSearch(string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderSearch(SearchParam, PageNumber, RowCount);
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
