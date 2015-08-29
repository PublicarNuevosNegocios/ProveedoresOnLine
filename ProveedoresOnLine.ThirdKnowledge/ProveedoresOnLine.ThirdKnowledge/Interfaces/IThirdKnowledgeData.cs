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

        #region Utils

        List<TDCatalogModel> CatalogGetThirdKnowledgeOptions();

        #endregion
    }
}
