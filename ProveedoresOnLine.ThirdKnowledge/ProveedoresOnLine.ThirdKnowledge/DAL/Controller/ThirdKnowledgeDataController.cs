using ProveedoresOnLine.ThirdKnowledge.Interfaces;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;

namespace ProveedoresOnLine.ThirdKnowledge.DAL.Controller
{
    internal class ThirdKnowledgeDataController : IThirdKnowledgeData
    {
        private static ProveedoresOnLine.ThirdKnowledge.Interfaces.IThirdKnowledgeData oInstance;

        internal static ProveedoresOnLine.ThirdKnowledge.Interfaces.IThirdKnowledgeData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new ThirdKnowledgeDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.ThirdKnowledge.Interfaces.IThirdKnowledgeData DataFactory;

        #region Constructor

        public ThirdKnowledgeDataController()
        {
            ThirdKnowledgeDataFactory factory = new ThirdKnowledgeDataFactory();
            DataFactory = factory.GetThirdKnowledgeInstance();
        }

        #endregion Constructor

        #region Config

        public List<Models.PlanModel> GetAllPlanByCustomer(string CustomerPublicId, bool Enable)
        {
            return DataFactory.GetAllPlanByCustomer(CustomerPublicId, Enable);
        }

        public string PlanUpsert(string PlanPublicId, string CompanyPublicId, int QueriesByPeriod, int DaysByPeriod, TDCatalogModel Status, DateTime InitDate, DateTime EndDate, bool Enable)
        {
            return DataFactory.PlanUpsert(PlanPublicId, CompanyPublicId, QueriesByPeriod, DaysByPeriod, Status, InitDate, EndDate, Enable);
        }

        public string PeriodUpsert(string PeriodPublicId, string PlanPublicId, int AssignedQueries, int TotalQueries, DateTime InitDate, DateTime EndDate, bool Enable)
        {
            return DataFactory.PeriodUpsert(PeriodPublicId, PlanPublicId, AssignedQueries, TotalQueries, InitDate, EndDate, Enable);
        }

        public List<PeriodModel> GetPeriodByPlanPublicId(string PlanPublicId, bool Enable)
        {
            return DataFactory.GetPeriodByPlanPublicId(PlanPublicId, Enable);
        }

        public List<TDQueryModel> GetQueriesByPeriodPublicId(string PeriodPublicId, bool Enable)
        {
            return DataFactory.GetQueriesByPeriodPublicId(PeriodPublicId, Enable);
        }

        #endregion Config

        #region MarketPlace

        public List<Models.PlanModel> GetCurrenPeriod(string CustomerPublicId, bool Enable)
        {
            return DataFactory.GetCurrenPeriod(CustomerPublicId, Enable);
        }

        public List<Models.TDQueryModel> ThirdKnowledgeSearch(string CustomerPublicId, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.ThirdKnowledgeSearch(CustomerPublicId, SearchOrderType, OrderOrientation, PageNumber, RowCount, out TotalRows);
        }

        public List<Models.TDQueryModel> ThirdKnowledgeSearchByPublicId(string CustomerPublicId, string QueryPublic, bool Enable)
        {
            return DataFactory.ThirdKnowledgeSearchByPublicId(CustomerPublicId, QueryPublic, Enable);
        }

        #endregion MarketPlace

        #region Queries

        public string QueryUsert(string QueryPublicId, string PeriodPublicId, int SearchType, string User, bool isSuccess, int QueryStatusId, bool Enable)
        {
            return DataFactory.QueryUsert(QueryPublicId, PeriodPublicId, SearchType, User, isSuccess, QueryStatusId, Enable);
        }

        public int QueryInfoInsert(string QueryPublicId, int ItemInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.QueryInfoInsert(QueryPublicId, ItemInfoType, Value, LargeValue, Enable);
        }

        #endregion Queries

        #region Utils

        public List<TDCatalogModel> CatalogGetThirdKnowledgeOptions()
        {
            return DataFactory.CatalogGetThirdKnowledgeOptions();
        }

        #endregion Utils

        #region BatchProcess

        public List<TDQueryModel> GetQueriesInProgress()
        {
            return DataFactory.GetQueriesInProgress();
        }
        
        #endregion

    }
}