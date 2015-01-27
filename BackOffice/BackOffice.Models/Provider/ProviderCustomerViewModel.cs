using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderCustomerViewModel
    {
        public string ProviderCustomerId { get; set; }

        public ProveedoresOnLine.Company.Models.Company.CompanyModel RelatedCompany { get; set; }

        public ProveedoresOnLine.Company.Models.Util.CatalogModel RelatedStatus { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel RelatedCompanyProviderInfo { get; set; }

        public bool Enable { get; set; }

        #region CustomerProvider

        public string CP_CustomerProviderId { get; set; }
        public string CP_CustomerPublicId { get; set; }
        public string CP_Customer { get; set; }
        public string CP_Status { get; set; }
        public string CP_Enable { get; set; }

        #endregion

        #region CustomerProviderInfo

        public string CPI_CustomerProviderInfoId { get; set; }
        public string CPI_TrackingType { get; set; }
        public string CPI_Tracking { get; set; }
        public string CPI_LastModify { get; set; }
        public string CPI_Enable { get; set; }

        #endregion

        public ProviderCustomerViewModel() { }

        public ProviderCustomerViewModel(string oProviderCustomerId
                                        , ProveedoresOnLine.Company.Models.Company.CompanyModel oRelatedCompany
                                        , ProveedoresOnLine.Company.Models.Util.CatalogModel oRelatedStatus
                                        , bool oEnable)
        {
            CP_CustomerProviderId = oProviderCustomerId;
            CP_CustomerPublicId = oRelatedCompany.CompanyPublicId;
            CP_Customer = oRelatedCompany.CompanyName;
            CP_Status = oRelatedStatus.ItemName;
            CP_Enable = Enable.ToString();
        }

        public ProviderCustomerViewModel(GenericItemInfoModel oRelatedCustomerProviderInfo)
        {
            CPI_CustomerProviderInfoId = oRelatedCustomerProviderInfo.ItemInfoId.ToString();
            CPI_TrackingType = oRelatedCustomerProviderInfo.ItemInfoType.ItemName;
            CPI_Tracking = oRelatedCustomerProviderInfo.Value;
            CPI_LastModify = oRelatedCustomerProviderInfo.LastModify.ToString();
            CPI_Enable = oRelatedCustomerProviderInfo.Enable.ToString();
        }

        public ProviderCustomerViewModel(ProveedoresOnLine.Company.Models.Company.CompanyModel oRelatedCompany)
        {
            RelatedCompany = oRelatedCompany;
        }

        public ProviderCustomerViewModel(string oProviderCustomerId
                                       , ProveedoresOnLine.Company.Models.Company.CompanyModel oRelatedCompany                                       
                                       , bool oEnable)
        {
            CP_CustomerProviderId = oProviderCustomerId;
            CP_CustomerPublicId = oRelatedCompany.CompanyPublicId;
            CP_Customer = oRelatedCompany.CompanyName;            
            CP_Enable = Enable.ToString();
        }

    }
}
