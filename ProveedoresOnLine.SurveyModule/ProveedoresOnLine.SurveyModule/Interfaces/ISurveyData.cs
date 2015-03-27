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

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? ParentSurveyConfigItem, bool Enable);

        #endregion

        #region Survey

        string SurveyUpsert(string SurveyPublicId, string ProviderPublicId, int SurveyConfigId, bool Enable);

        int SurveyInfoUpsert(int? SurveyInfoId, string SurveyPublicId, int SurveyInfoType, string Value, string LargeValue, bool Enable);

        int SurveyItemUpsert(int? SurveyItemId, string SurveyPublicId, int SurveyConfigItemId, bool Enable);

        int SurveyItemInfoUpsert(int? SurveyItemInfoId, int SurveyItemId, int SurveyItemInfoType, string Value, string LargeValue, bool Enable);

        List<SurveyModule.Models.SurveyModel> SurveySearch(string CustomerPublicId, string ProviderPublicId, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows);

        #endregion
    }
}
