using DocumentManagementClient.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using DocumentManagementClient.Manager.Models;
using DocumentManagementClient.Manager.Models.Customer;

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

        public List<Models.Customer.CustomerModel> CustomerSearch(string IdentificationNumber, string Name)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber == string.Empty ? null : IdentificationNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name == string.Empty ? null : Name));       

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_Customer_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<CustomerModel> oReturn = null;
            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from pm in response.DataTableResult.AsEnumerable()
                           select new CustomerModel()
                           {
                               CustomerPublicId = pm.Field<string>("CustomerPublicId"),
                               IdentificationNumber = pm.Field<string>("IdentificationNumber"),
                               IdentificationType = (enumIdentificationType)pm.Field<int>("IdentificationType"),
                               Name = pm.Field<string>("Name"),
                               LastModify = pm.Field<DateTime>("LastModify"),
                               CreateDate = pm.Field<DateTime>("CreateDate"),                           
                           }).ToList();
            }

            return oReturn;
        }
    }
}
