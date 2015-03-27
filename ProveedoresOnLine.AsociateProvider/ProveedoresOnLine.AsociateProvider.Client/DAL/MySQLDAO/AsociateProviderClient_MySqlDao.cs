using ProveedoresOnLine.AsociateProvider.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.DAL.MySQLDAO
{
    internal class AsociateProviderClient_MySqlDao : IAsociateProviderClientData
    {
        private ADO.Interfaces.IADO DataInstance;

        public AsociateProviderClient_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.AsociateProvider.Client.Models.Constants.POL_AsociateProviderClientConnectionName);
        }

        #region AsociateProvider

        public int DMProviderUpsert(string ProviderPublicId, string ProviderName, string IdentificationType, string IdentificationNumber)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderName", ProviderName));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", IdentificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "AP_DMProviderUpsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            int oReturn = 0;

            if (response.ScalarResult != null)
            {
                oReturn = Convert.ToInt32(response.ScalarResult);
            }

            return oReturn;
        }

        public int BOProviderUpsert(string ProviderPublicId, string ProviderName, string IdentificationType, string IdentificationNumber)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderName", ProviderName));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", IdentificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "AP_BOProviderUpsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            int oReturn = 0;

            if (response.ScalarResult != null)
            {
                oReturn = Convert.ToInt32(response.ScalarResult);
            }

            return oReturn;
        }

        public void AP_AsociateProviderUpsert(string BOProviderPublicId, string DMProviderPublicId, string Email)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vDM_ProviderPublicId", DMProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vBO_ProviderPublicId", BOProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vUserEmail", Email));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "AP_AsociateProviderUpsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });            
        }

        #endregion        
    }
}
