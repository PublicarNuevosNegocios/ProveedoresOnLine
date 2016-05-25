using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.DAL.Controller
{
    internal class CalificationProjectDataController : ProveedoresOnLine.CalificationProject.Interfaces.ICalificationProjectData
    {
        #region singleton instance

        private static ProveedoresOnLine.CalificationProject.Interfaces.ICalificationProjectData oInstance;

        internal static ProveedoresOnLine.CalificationProject.Interfaces.ICalificationProjectData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new CalificationProjectDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.CalificationProject.Interfaces.ICalificationProjectData DataFactory;

        #endregion

        #region constructor

        public CalificationProjectDataController()
        {
            CalificationProject.DAL.Controller.CalificationProjectDataFactory factory = new CalificationProject.DAL.Controller.CalificationProjectDataFactory();
            DataFactory = factory.GetCalificationProjectInstance();
        }

        #endregion

        #region ProjectConfig
        public int CalificationProjectConfigUpsert(int CalificationProjectConfigId, string Company, string CalificationProjectConfigName, bool Enable) 
        {
            return DataFactory.CalificationProjectConfigUpsert(CalificationProjectConfigId, Company, CalificationProjectConfigName, Enable);
        }
        #endregion

        #region ConfigItem

        public int CalificationProjectConfigItemUpsert(int CalificationProjectConfigId, int CalificationProjectConfigItemId, string CalificationProjectConfigItemName, int CalificationProjectConfigItemType, bool Enable)
        {
            return DataFactory.CalificationProjectConfigItemUpsert(CalificationProjectConfigId, CalificationProjectConfigItemId, CalificationProjectConfigItemName, CalificationProjectConfigItemType, Enable);
        }

        #endregion
        
    }
}
