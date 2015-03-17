using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.SurveyModule.DAL.Controller
{
    internal class SurveyDataController : ProveedoresOnLine.SurveyModule.Interfaces.ISurveyData
    {
        #region singleton instance

        private static ProveedoresOnLine.SurveyModule.Interfaces.ISurveyData oInstance;
        internal static ProveedoresOnLine.SurveyModule.Interfaces.ISurveyData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new SurveyDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.SurveyModule.Interfaces.ISurveyData DataFactory;

        #endregion

        #region Constructor

        public SurveyDataController()
        {
            SurveyDataFactory factory = new SurveyDataFactory();
            DataFactory = factory.GetSurveyInstance();
        }

        #endregion

        #region Survey Config

        public int SurveyConfigUpsert(string CustomerPublicId, int? SurveyConfigId, string SurveyName, bool Enable)
        {
            return DataFactory.SurveyConfigUpsert(CustomerPublicId, SurveyConfigId, SurveyName, Enable);
        }

        public int SurveyConfigInfoUpsert(int? SurveyConfigInfoId, int SurveyConfigId, int SurveyConfigInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.SurveyConfigInfoUpsert(SurveyConfigInfoId, SurveyConfigId, SurveyConfigInfoType, Value, LargeValue, Enable);
        }

        public int SurveyConfigItemUpsert(int? SurveyConfigItemId, int SurveyConfigId, string SurveyConfigItemName, int SurveyConfigItemType, int? ParentSurveyConfigItem, bool Enable)
        {
            return DataFactory.SurveyConfigItemUpsert(SurveyConfigItemId, SurveyConfigId, SurveyConfigItemName, SurveyConfigItemType, ParentSurveyConfigItem, Enable);
        }

        public int SurveyConfigItemInfoUpsert(int? SurveyConfigItemInfoId, int SurveyConfigItemId, int SurveyConfigItemInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.SurveyConfigItemInfoUpsert(SurveyConfigItemInfoId, SurveyConfigItemId, SurveyConfigItemInfoType, Value, LargeValue, Enable);
        }

        #endregion
    }
}
