﻿using DocumentManagement.Provider.Interfaces;
using DocumentManagement.Provider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.DAL.MySQLDAO
{
    internal class Provider_MySqlDao : IProviderData
    {
        private ADO.Interfaces.IADO DataInstance;
        public Provider_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Constants.P_ProviderConnectionName);
        }

        public string ProviderUpsert(string CustomerPublicId, string ProviderPublicId, string Name, Enumerations.enumIdentificationType IdentificationType, string IdentificationNumber, string Email, Enumerations.enumProcessStatus Status)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", IdentificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vEmail", Email));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)Status));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_Provider_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
        }

        public string ProviderInfoUpsert(int ProviderInfoId, string ProviderPublicId, Enumerations.enumProviderInfoType ProviderInfoType, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderInfoId", ProviderInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderInfoType", (int)ProviderInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_ProviderInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return null;
        }

        public string ProviderCustomerInfoUpsert(int ProviderCustomerInfoId, string ProviderPublicId, string CustomerPublicId, Enumerations.enumProviderCustomerInfoType ProviderCustomerInfoType, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderCustomerInfoId", ProviderCustomerInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderCustomerInfoType", (int)ProviderCustomerInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_ProviderCustomerInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return null;
        }       
    }
}
