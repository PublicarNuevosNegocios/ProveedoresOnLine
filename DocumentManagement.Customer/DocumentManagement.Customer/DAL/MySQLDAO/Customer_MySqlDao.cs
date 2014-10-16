using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using DocumentManagement.Customer.Interfaces;
using DocumentManagement.Customer.Models;

namespace DocumentManagement.Customer.DAL.MySQLDAO
{
    internal class Customer_MySqlDao : ICustomerData
    {
        private ADO.Interfaces.IADO DataInstance;

        internal Customer_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Constants.C_DMCustomerConnectionName);
        }

        #region Customer

        public string CustomerUpsert(string CustomerPublicId, string Name, int IdentificationType, string IdentificationNumber)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", IdentificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "C_Customer_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            if (response.ScalarResult != null)
                return response.ScalarResult.ToString();
            else
                return null;
        }

        #endregion

        #region Form

        public string FormUpsert(string FormPublicId, string CustomerPublicId, string Name, string TermsAndConditions, string Logo)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFormPublicId", FormPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vTermsAndConditions", TermsAndConditions));
            lstParams.Add(DataInstance.CreateTypedParameter("vLogo", Logo));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "F_Form_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            if (response.ScalarResult != null)
                return response.ScalarResult.ToString();
            else
                return null;
        }

        public int StepCreate(string FormPublicId, string Name, int Position)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFormPublicId", FormPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vPosition", Position));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "F_Step_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public void StepDelete(int StepId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vStepId", StepId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "F_Step_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public int FieldCreate(int StepId, string Name, int ProviderInfoType, int FieldType, bool IsRequired, int Position)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vStepId", StepId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderInfoType", ProviderInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vFieldType", FieldType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIsRequired", IsRequired));
            lstParams.Add(DataInstance.CreateTypedParameter("vPosition", Position));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "F_Field_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public void FieldDelete(int FieldId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFieldId", FieldId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "F_Field_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        #endregion
    }
}
