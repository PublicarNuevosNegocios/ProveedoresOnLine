using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Interfaces
{
    interface IThirdKnowledgeData
    {
        #region Config
        List<PlanModel> GetAllPlanByCustomer(string CustomerPublicId, bool Enable);

        string PlanUpsert(string PlanPublicId, string CompanyPublicId, int QueriesByPeriod, int DaysByPeriod, TDCatalogModel Status, DateTime InitDate, DateTime EndDate, bool Enable);

        string PeriodUpsert(string PeriodPublicId, string PlanPublicId, int AssignedQueries, int TotalQueries, DateTime InitDate, DateTime EndDate, bool Enable);

        #endregion

        #region MarketPlace

        List<Models.PlanModel> GetCurrenPeriod(string CustomerPublicId, bool Enable);

        List<Models.TDQueryModel> ThirdKnoledgeSearch(string CustomerPublicId, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows);

        #endregion

        #region Queries

        string QueryInsert(string PeriodPublicId, int SearchType, string User, bool isSuccess, bool Enable);

        int QueryInfoInsert(string QueryPublicId, int ItemInfoType, string Value, string LargeValue, bool Enable);

        #endregion

        #region Utils

        List<TDCatalogModel> CatalogGetThirdKnowledgeOptions();

        List<PeriodModel> GetPeriodByPlanPublicId(string PlanPublicId, bool Enable);
        List<TDQueryModel> GetQueriesByPeriodPublicId(string PeriodPublicId, bool Enable);
        #endregion
    }
}
