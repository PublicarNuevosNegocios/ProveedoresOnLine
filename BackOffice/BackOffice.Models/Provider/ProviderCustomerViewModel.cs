﻿using ProveedoresOnLine.Company.Models.Util;
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

        public bool Enable { get; set; }

        #region CustomerProvider

        public string CP_CustomerProviderId { get; set; }
        public string CP_CustomerPublicId { get; set; }
        public string CP_Customer { get; set; }
        public string CP_StatusId { get; set; }
        public string CP_Status { get; set; }
        public string CP_Enable { get; set; }

        #endregion

        public ProviderCustomerViewModel() { }

        public ProviderCustomerViewModel(ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerProviderModel oRelatedCustomer)
        {
            CP_CustomerProviderId = oRelatedCustomer.CustomerProviderId.ToString();
            CP_CustomerPublicId = oRelatedCustomer.RelatedProvider.CompanyPublicId;
            CP_Customer = oRelatedCustomer.RelatedProvider.CompanyName;
            CP_StatusId = oRelatedCustomer.Status.ItemId.ToString();
            CP_Status = oRelatedCustomer.Status.ItemName;
            CP_Enable = oRelatedCustomer.Enable.ToString();
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
