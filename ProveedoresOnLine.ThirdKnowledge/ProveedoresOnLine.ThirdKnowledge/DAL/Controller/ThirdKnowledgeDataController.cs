﻿using ProveedoresOnLine.ThirdKnowledge.Interfaces;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion

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

        #endregion

        #region MarketPlace

        public List<Models.PlanModel> GetCurrenPeriod(string CustomerPublicId, bool Enable)
        {
            return DataFactory.GetCurrenPeriod(CustomerPublicId, Enable);
        }

        public List<Models.TDQueryModel> ThirdKnoledgeSearch(string CustomerPublicId, int SearchType, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.ThirdKnoledgeSearch(CustomerPublicId, SearchType, SearchOrderType, OrderOrientation, PageNumber, RowCount, out TotalRows);
        }

        #endregion

        #region Queries

        public string QueryInsert(string PeriodPublicId, int SearchType, string User, bool isSuccess, bool Enable)
        {
            return DataFactory.QueryInsert(PeriodPublicId, SearchType, User, isSuccess, Enable);
        }

        public int QueryInfoInsert(string QueryPublicId, int ItemInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.QueryInfoInsert(QueryPublicId, ItemInfoType, Value, LargeValue, Enable);
        }

        #endregion

        #region Utils
        public List<TDCatalogModel> CatalogGetThirdKnowledgeOptions()
        {
            return DataFactory.CatalogGetThirdKnowledgeOptions();
        } 
        #endregion       
    }
}
