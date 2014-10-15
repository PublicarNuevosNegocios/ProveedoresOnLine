using DocumentManagementClient.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using DocumentManagementClient.Manager.Models;

namespace DocumentManagementClient.Manager.DAL.MySQLDAO
{    
    internal class Customer_MySqlDao : ICustomerData
    {
        private ADO.Interfaces.IADO DataInstance; 
        public Customer_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Constants.C_CustomerConnectionName);
        }

        public string CustomerUpsert(string CustomerPublicId, string Name, enumIdentificationType IdentificationType, string IdentificationNumber)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", (int)IdentificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "C_Customer_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
        }
    }
}
