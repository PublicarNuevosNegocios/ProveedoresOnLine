using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.SurveyModule.Interfaces
{
    internal interface ISurveyData
    {
        #region Survey Config

        int SurveyConfigUpsert(string CustomerPublicId, int? SurveyConfigId, string SurveyName, bool Enable);

        int SurveyConfigInfoUpsert(int? SurveyConfigInfoId, int SurveyConfigId, int SurveyConfigInfoType, string Value, string LargeValue, bool Enable);

        int SurveyConfigItemUpsert(int? SurveyConfigItemId, int SurveyConfigId, string SurveyConfigItemName, int SurveyConfigItemType, int? ParentSurveyConfigItem, bool Enable);

        int SurveyConfigItemInfoUpsert(int? SurveyConfigItemInfoId, int SurveyConfigItemId, int SurveyConfigItemInfoType, string Value, string LargeValue, bool Enable);

        List<ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel> SurveyConfigSearch(string CustomerPublicId, string SearchParam, bool Enable, int PageNumber, int RowCount, out int TotalRows);

        ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel SurveyConfigGetById(int SurveyConfigId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? ParentSurveyConfigItem, int? SurveyItemType, bool Enable);

        List<ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel> MP_SurveyConfigSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount);

        List<GenericItemModel> MP_SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? SurveyItemType);
        #endregion

        #region Survey

        string SurveyUpsert(string SurveyPublicId, string ProviderPublicId, int SurveyConfigId, bool Enable);

        int SurveyInfoUpsert(int? SurveyInfoId, string SurveyPublicId, int SurveyInfoType, string Value, string LargeValue, bool Enable);

        int SurveyItemUpsert(int? SurveyItemId, string SurveyPublicId, int SurveyConfigItemId, int EvaluatorRolId, bool Enable);

        int SurveyItemInfoUpsert(int? SurveyItemInfoId, int SurveyItemId, int SurveyItemInfoType, string Value, string LargeValue, bool Enable);

        List<SurveyModule.Models.SurveyModel> SurveySearch(string CustomerPublicId, string ProviderPublicId, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows);
                
        SurveyModule.Models.SurveyModel SurveyGetById(string SurveyPublicId);

        List<SurveyModule.Models.SurveyModel> SurveyGetByCustomerProvider(string CustomerPublicId, string ProviderPublicId);

        #endregion

        #region SurveyBatch

        List<SurveyModule.Models.SurveyModel> BPSurveyGetNotification();

        #endregion

        #region SurveyCharts

        List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetSurveyByResponsable(string CustomerPublicId, string ResponsableEmail, DateTime Year);
        List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetSurveyByMonth(string CustomerPublicId, DateTime Year);

        #endregion
    }
}
