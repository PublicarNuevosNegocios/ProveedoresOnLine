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

        public List<ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel> SurveyConfigSearch(string CustomerPublicId, string SearchParam, bool Enable, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.SurveyConfigSearch(CustomerPublicId, SearchParam, Enable, PageNumber, RowCount, out TotalRows);
        }

        public Models.SurveyConfigModel SurveyConfigGetById(int SurveyConfigId)
        {
            return DataFactory.SurveyConfigGetById(SurveyConfigId);
        }

        public List<Company.Models.Util.GenericItemModel> SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? ParentSurveyConfigItem, int? SurveyItemType, bool Enable)
        {
            return DataFactory.SurveyConfigItemGetBySurveyConfigId(SurveyConfigId, ParentSurveyConfigItem, SurveyItemType, Enable);
        }

        public List<Models.SurveyConfigModel> MP_SurveyConfigSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount)
        {
            return DataFactory.MP_SurveyConfigSearch(CustomerPublicId, SearchParam, PageNumber, RowCount);
        }

        public List<Company.Models.Util.GenericItemModel> MP_SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? SurveyItemType)
        {
            return DataFactory.MP_SurveyConfigItemGetBySurveyConfigId(SurveyConfigId, SurveyItemType);
        }
        #endregion

        #region Survey

        public string SurveyUpsert(string SurveyPublicId, string ProviderPublicId, int SurveyConfigId, bool Enable)
        {
            return DataFactory.SurveyUpsert(SurveyPublicId, ProviderPublicId, SurveyConfigId, Enable);
        }

        public int SurveyInfoUpsert(int? SurveyInfoId, string SurveyPublicId, int SurveyInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.SurveyInfoUpsert(SurveyInfoId, SurveyPublicId, SurveyInfoType, Value, LargeValue, Enable);
        }

        public int SurveyItemUpsert(int? SurveyItemId, string SurveyPublicId, int SurveyConfigItemId, int EvaluatorRolId, bool Enable)
        {
            return DataFactory.SurveyItemUpsert(SurveyItemId, SurveyPublicId, SurveyConfigItemId, EvaluatorRolId, Enable);
        }

        public int SurveyItemInfoUpsert(int? SurveyItemInfoId, int SurveyItemId, int SurveyItemInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.SurveyItemInfoUpsert(SurveyItemInfoId, SurveyItemId, SurveyItemInfoType, Value, LargeValue, Enable);
        }

        public List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> SurveySearch(string CustomerPublicId, string ProviderPublicId, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.SurveySearch(CustomerPublicId, ProviderPublicId, SearchOrderType, OrderOrientation, PageNumber, RowCount, out  TotalRows);
        }

        public Models.SurveyModel SurveyGetById(string SurveyPublicId)
        {
            return DataFactory.SurveyGetById(SurveyPublicId);
        }

        public SurveyModule.Models.SurveyModel SurveyGetByUser(string ParentSurveyPublicId, string User)
        {
            return DataFactory.SurveyGetByUser(ParentSurveyPublicId, User);
        }

        public List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> SurveyGetByCustomerProvider(string CustomerPublicId, string ProviderPublicId)
        {
            return DataFactory.SurveyGetByCustomerProvider(CustomerPublicId, ProviderPublicId);
        }

        #endregion

        #region SurveyBatch

        public List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> BPSurveyGetNotification()
        {
            return DataFactory.BPSurveyGetNotification();
        }

        #endregion

        #region SurveyCharts

        public List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetSurveyByResponsable(string CustomerPublicId, string ResponsableEmail, DateTime Year)
        {
            return DataFactory.GetSurveyByResponsable(CustomerPublicId, ResponsableEmail, Year);
        }
        public List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetSurveyByEvaluator(string CustomerPublicId, DateTime Year)
        {
            return DataFactory.GetSurveyByEvaluator(CustomerPublicId, Year);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetSurveyByMonth(string CustomerPublicId, DateTime Year)
        {
            return DataFactory.GetSurveyByMonth(CustomerPublicId, Year);
        }
        #endregion        
    }
}
