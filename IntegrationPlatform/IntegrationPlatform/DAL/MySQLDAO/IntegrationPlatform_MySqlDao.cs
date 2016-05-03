using IntegrationPlatform.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlatform.DAL.MySQLDAO
{
    internal class IntegrationPlatform_MySqlDao : IIntegrationPlatformData
    {
        private ADO.Interfaces.IADO DataInstance;

        public IntegrationPlatform_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(IntegrationPlatform.Models.Constants.C_POL_IntegrationPlatformConnectionName);
        }
    }
}
