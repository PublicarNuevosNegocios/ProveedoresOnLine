using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IntegrationPlattaform.SANOFIProcess.Interfaces;
using IntegrationPlattaform.SANOFIProcess.Models;


namespace IntegrationPlattaform.SANOFIProcess.DAL.MySQLDAO
{
    internal class SANOFIProcess_MySqlDao : IIntegrationPlatformSANOFIProcessData
    {
        private ADO.Interfaces.IADO DataInstance;

        public SANOFIProcess_MySqlDao() 
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(IntegrationPlattaform.SANOFIProcess.Models.Constants.C_SettingsModuleName);
        }
        
        public List<SanofiGeneralInfoModel> GetInfo_ByProvider(string vProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<System.Data.IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", vProviderPublicId));
                        
        }
    }
}
