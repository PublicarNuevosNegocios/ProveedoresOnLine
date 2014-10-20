﻿using DocumentManagement.Provider.Models;
using DocumentManagement.Provider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Interfaces
{
    interface IProviderData
    {
        #region Provider
        string ProviderUpsert(string CustomerPublicId, string ProviderPublicId, string Name, Enumerations.enumIdentificationType IdentificationType, DocumentManagement.Provider.Models.Enumerations.enumProviderCustomerInfoType CustomerProviderInfoType, string IdentificationNumber, string Email, Enumerations.enumProcessStatus Status);

        string ProviderInfoUpsert(int ProviderInfoId, string ProviderPublicId, DocumentManagement.Provider.Models.Enumerations.enumProviderInfoType ProviderInfoType, string Value, string LargeValue);

        string ProviderCustomerInfoUpsert(int ProviderCustomerInfoId, string ProviderPublicId, string CustomerPublicId, DocumentManagement.Provider.Models.Enumerations.enumProviderCustomerInfoType ProviderCustomerInfoType, string Value, string LargeValue);

        ProviderModel GetProviderByIdentificationNumberAndDocumentType(string IdentificationNumber, DocumentManagement.Provider.Models.Enumerations.enumIdentificationType IdenificationType);

        bool GetRelationProviderAndCustomer(string CustomerPublicId, string ProviderPublicId);
        #endregion      
    }
}
