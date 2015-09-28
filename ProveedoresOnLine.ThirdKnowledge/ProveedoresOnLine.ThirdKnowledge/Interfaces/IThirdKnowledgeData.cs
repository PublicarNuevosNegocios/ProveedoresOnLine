using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;

namespace ProveedoresOnLine.ThirdKnowledge.Interfaces
{
    internal interface IThirdKnowledgeData
    {
        #region Config

        List<PlanModel> GetAllPlanByCustomer(string CustomerPublicId, bool Enable);

        string PlanUpsert(string PlanPublicId, string CompanyPublicId, int QueriesByPeriod, int DaysByPeriod, TDCatalogModel Status, DateTime InitDate, DateTime EndDate, bool Enable);

        string PeriodUpsert(string PeriodPublicId, string PlanPublicId, int AssignedQueries, int TotalQueries, DateTime InitDate, DateTime EndDate, bool Enable);

        #endregion Config

        #region MarketPlace

        List<Models.PlanModel> GetCurrenPeriod(string CustomerPublicId, bool Enable);

        List<Models.TDQueryModel> ThirdKnowledgeSearch(string CustomerPublicId, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows);

        List<Models.TDQueryModel> ThirdKnowledgeSearchByPublicId(string CustomerPublicId, string QueryPublic, bool Enable);

        #endregion MarketPlace

        #region Queries

        string QueryInsert(string PeriodPublicId, int SearchType, string User, bool isSuccess, int QueryStatusId, bool Enable);

        int QueryInfoInsert(string QueryPublicId, int ItemInfoType, string Value, string LargeValue, bool Enable);

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