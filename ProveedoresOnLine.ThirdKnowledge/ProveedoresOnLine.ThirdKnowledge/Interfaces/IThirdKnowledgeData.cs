using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;

namespace ProveedoresOnLine.ThirdKnowledge.Interfaces
{
    internal interface IThirdKnowledgeData
    {
        #region Config

        List<PlanModel> GetAllPlanByCustomer(string CustomerPublicId, bool Enable);

        string PlanUpsert(string PlanPublicId, string CompanyPublicId, int QueriesByPeriod, bool IsLimited, int DaysByPeriod, TDCatalogModel Status, DateTime InitDate, DateTime EndDate, bool Enable);

        string PeriodUpsert(string PeriodPublicId, string PlanPublicId, int AssignedQueries, bool IsLimited, int TotalQueries, DateTime InitDate, DateTime EndDate, bool Enable);

        #endregion Config

        #region MarketPlace

        List<Models.PlanModel> GetCurrenPeriod(string CustomerPublicId, bool Enable);        

        List<Models.TDQueryModel> ThirdKnowledgeSearch(string CustomerPublicId, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows);

        List<Models.TDQueryModel> ThirdKnowledgeSearchByPublicId(string CustomerPublicId, string QueryPublic, bool Enable);

        #endregion MarketPlace

        #region Queries

        string QueryUpsert(string QueryPublicId, string PeriodPublicId, int SearchType, string User, string FileName, bool isSuccess, int QueryStatusId, bool Enable);

        string QueryBasicInfoInsert(string QueryPublicId, string NameResult, string IdentificationResult, string Priority, string Peps, string Status, string Alias, string Offense, bool Enable);

        int QueryDetailInfoInsert(string QueryBasicPublicId, int ItemInfoType, string Value, string LargeValue, bool Enable);

        TDQueryInfoModel QueryDetailGetByBasicPublicID(string QueryBasicInfoPublicId);
        #endregion Queries

        #region Utils

        List<TDCatalogModel> CatalogGetThirdKnowledgeOptions();

        List<PeriodModel> GetPeriodByPlanPublicId(string PlanPublicId, bool Enable);

        List<TDQueryModel> GetQueriesByPeriodPublicId(string PeriodPublicId, bool Enable);

        #endregion Utils

        #region BatchProcess

        List<TDQueryModel> GetQueriesInProgress();
        #endregion
    }
}