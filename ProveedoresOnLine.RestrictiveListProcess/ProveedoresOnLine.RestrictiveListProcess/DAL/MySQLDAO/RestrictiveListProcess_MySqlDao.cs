using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.DAL.MySQLDAO
{
    internal class RestrictiveListProcess_MySqlDao : IRestrictiveListProcess
    {
        private ADO.Interfaces.IADO DataInstance;

        public RestrictiveListProcess_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.RestrictiveListProcess.Models.Constants.R_POL_RestrictiveListProcessConnectionName);
        }


        public List<ProviderModel> GetProviderByStatus(int Status, string CustomerPublicId)
        {

            //CompanyModel

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", Status));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "BP_GetProviderByStatus",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return null;
        }

    }
}
