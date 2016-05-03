using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlatform.DAL.Controller
{
    internal class IntegrationPlatformDataController : IntegrationPlatform.Interfaces.IIntegrationPlatformData
    {
        #region singleton instance

        private static IntegrationPlatform.Interfaces.IIntegrationPlatformData oInstance;

        internal static IntegrationPlatform.Interfaces.IIntegrationPlatformData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new IntegrationPlatformDataController();
                return oInstance;
            }
        }

        private IntegrationPlatform.Interfaces.IIntegrationPlatformData DataFactory;

        #endregion

        #region Constructor

        public IntegrationPlatformDataController()
        {
            IntegrationPlatformDataFactory factory = new IntegrationPlatformDataFactory();
            DataFactory = factory.GetIntegrationPlatformInstance();
        }

        #endregion
    }
}
