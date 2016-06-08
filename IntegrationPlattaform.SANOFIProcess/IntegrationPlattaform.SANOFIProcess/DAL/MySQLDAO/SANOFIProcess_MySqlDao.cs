using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationPlattaform.SANOFIProcess.Interfaces;
using IntegrationPlattaform.SANOFIProcess.Models;

namespace IntegrationPlattaform.SANOFIProcess.DAL.MySQLDAO
{
    internal class SANOFIProcess_MySqlDao : IIntegrationPlatformSANOFIProcessData
    {
        private ADO.Interfaces.IADO DataInstance;

        public SANOFIProcess_MySqlDao() 
        {
            DataInstance = new ADO.MYSQL.MySqlImplement("");
        }

        public List<Models.SanofiGeneralInfoModel> GetInfo_ByProviderAndLastModify(string vProviderPublicId)
        {
            throw new NotImplementedException();
        }
    }
}
