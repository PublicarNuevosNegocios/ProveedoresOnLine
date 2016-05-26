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

            #region ProjectConfig

        public int CalificationProjectConfigUpsert(int CalificationProjectConfigId, string CompanyPublicId, string CalificationProjectConfigName, bool Enable)
            {
                return DataFactory.CalificationProjectConfigUpsert(CalificationProjectConfigId, CompanyPublicId, CalificationProjectConfigName, Enable);
            }
        public List<Models.CalificationProject.CalificationProjectConfigModel> CalificationProjectConfig_GetByCompanyId(string CompanyPublicId, bool Enable)
            {
                return DataFactory.CalificationProjectConfig_GetByCompanyId(CompanyPublicId, Enable);
            }

            #endregion

            #region ConfigItem

            public int CalificationProjectConfigItemUpsert(int CalificationProjectConfigId, int CalificationProjectConfigItemId, string CalificationProjectConfigItemName, int CalificationProjectConfigItemType, bool Enable)
            {
                return DataFactory.CalificationProjectConfigItemUpsert(CalificationProjectConfigId, CalificationProjectConfigItemId, CalificationProjectConfigItemName, CalificationProjectConfigItemType, Enable);
            }

            public List<Models.CalificationProject.ConfigItemModel> CalificationProjectConfigItem_GetByCalificationProjectConfigId(int CalificationProjectConfigId, bool Enable)
            {
                return DataFactory.CalificationProjectConfigItem_GetByCalificationProjectConfigId(CalificationProjectConfigId, Enable);
            }

            #endregion

            #region ConfigItemInfo

        public int CalificationProjectConfigItemInfoUpsert(int CalificationProjectConfigItemId, int CalificationProjectConfigItemInfoId, int Question, int Rule, int ValueType, string Value, string Score, bool Enable)
        {
            return DataFactory.CalificationProjectConfigItemInfoUpsert(CalificationProjectConfigItemId, CalificationProjectConfigItemInfoId, Question, Rule, ValueType, Value, Score, Enable);
        }

        public List<Models.CalificationProject.ConfigItemInfoModel> CalificationProjectConfigItemInfo_GetByCalificationProjectConfigItemId(int CalificationProjectConfigItemId, bool Enable)
        {
            return DataFactory.CalificationProjectConfigItemInfo_GetByCalificationProjectConfigItemId(CalificationProjectConfigItemId, Enable);
        }

        #endregion

            #region ConfigValidate

            public int CalificationProjectConfigValidateUpsert(int CalificationProjectConfigValidateId, int CalificationProjectConfigId, int Operator, string Value, string Result, bool Enable)
            {
                return DataFactory.CalificationProjectConfigValidateUpsert(CalificationProjectConfigValidateId, CalificationProjectConfigId, Operator, Value, Result, Enable);
            }
            public List<Models.CalificationProject.ConfigValidateModel> CalificationProjectConfigValidate_GetByProjectConfigId(int CalificationProjectConfigId, bool Enable)
            {
                return DataFactory.CalificationProjectConfigValidate_GetByProjectConfigId(CalificationProjectConfigId, Enable);
            }

            #endregion

        #endregion
    }
}
