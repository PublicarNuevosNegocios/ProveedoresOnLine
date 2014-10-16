using Customer.Provider.Interfaces;
using Customer.Provider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Provider.DAL.MySQLDAO
{
    internal class Provider_MySqlDao : IProviderData
    {
        private ADO.Interfaces.IADO DataInstance;
        public Provider_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Constants.P_ProviderConnectionName);
        }

        public string ProviderUpsert(string CustomerPublicId, string ProviderPublicId, string Name, Models.enumIdentificationType IdentificationType, string IdentificationNumber, string Email, Models.enumProcessStatus Status)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", (int)IdentificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vEmail", Email));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "C_Customer_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
            return null;
        }

        public string ProviderInfoUpsert(int ProviderInfoId, string ProviderPublicId, Models.enumProviderInfoType ProviderInfoType, string Value, string LargeValue)
        {
            throw new NotImplementedException();
        }

        public string ProviderCustomerInfoUpsert(int ProviderCustomerInfoId, string ProviderPublicId, string CustomerPublicId, Models.enumProviderCustomerInfoType ProviderCustomerInfoType, string Value, string LargeValue)
        {
            throw new NotImplementedException();
        }
    }
}
