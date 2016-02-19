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

        public string PlanUpsert(string PlanPublicId, string CompanyPublicId, int QueriesByPeriod, bool IsLimited, int DaysByPeriod, TDCatalogModel Status, DateTime InitDate, DateTime EndDate, bool Enable)
        {
            return DataFactory.PlanUpsert(PlanPublicId, CompanyPublicId, QueriesByPeriod, IsLimited, DaysByPeriod, Status, InitDate, EndDate, Enable);
        }

        public string PeriodUpsert(string PeriodPublicId, string PlanPublicId, int AssignedQueries, bool IsLimited, int TotalQueries, DateTime InitDate, DateTime EndDate, bool Enable)
        {
            return DataFactory.PeriodUpsert(PeriodPublicId, PlanPublicId, AssignedQueries, IsLimited, TotalQueries, InitDate, EndDate, Enable);
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

        public List<Models.TDQueryModel> ThirdKnowledgeSearch(string CustomerPublicId, string StartDate, string EndtDate, int PageNumber, int RowCount, string SearchType, string Status, out int TotalRows)
        {
            return DataFactory.ThirdKnowledgeSearch(CustomerPublicId, StartDate, EndtDate, PageNumber, RowCount, SearchType, Status, out TotalRows);
        }

        public List<Models.TDQueryModel> ThirdKnowledgeSearchByPublicId(string CustomerPublicId, string QueryPublic, bool Enable, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.ThirdKnowledgeSearchByPublicId(CustomerPublicId, QueryPublic, Enable,  PageNumber,  RowCount, out TotalRows);
        }

        #endregion MarketPlace

        #region Query

        public string QueryUpsert(string QueryPublicId, string PeriodPublicId, int SearchType, string User, string FileName, bool isSuccess, int QueryStatusId, bool Enable)
        {
            return DataFactory.QueryUpsert(QueryPublicId, PeriodPublicId, SearchType, User, FileName, isSuccess, QueryStatusId, Enable);
        }

        public string QueryBasicInfoInsert(string QueryPublicId, string NameResult, string IdentificationResult, string Priority, string Peps, string Status, string Alias, string Offense, bool Enable)
        {
            return DataFactory.QueryBasicInfoInsert(QueryPublicId, NameResult, IdentificationResult, Priority, Peps, Status, Alias,Offense, Enable);
        }

        public int QueryDetailInfoInsert(string QueryBasicPublicId, int ItemInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.QueryDetailInfoInsert(QueryBasicPublicId, ItemInfoType, Value, LargeValue, Enable); ;
        }

        public TDQueryInfoModel QueryDetailGetByBasicPublicID(string QueryBasicInfoPublicId)
        {
            return DataFactory.QueryDetailGetByBasicPublicID(QueryBasicInfoPublicId);
        }
        #endregion Query

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