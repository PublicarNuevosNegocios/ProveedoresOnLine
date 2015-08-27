using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.DAL.MySQLDAO
{
    internal class ThirdKnowledge_MySqlDao : ProveedoresOnLine.ThirdKnowledge.Interfaces.IThirdKnowledgeData
    {
        private ADO.Interfaces.IADO DataInstance;

        public ThirdKnowledge_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_POL_ThirdKnowledgeConnectionName);
        }

        #region Config

        public List<Models.PlanModel> GetAllPlanByCustomer(string CustomerPublicId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("CustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "TD_GetAllPlanByCustomer",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<Models.PlanModel> oReturn = null;

            if (response.DataTableResult != null &&
               response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cm in response.DataTableResult.AsEnumerable()
                     where !cm.IsNull("CommercialId")
                     group cm by new
                     {
                         PlanId = cm.Field<int>("PlanId"),
                         PlanPublicId = cm.Field<string>("PlanPublicId"),
                         CompanyPublicId = cm.Field<string>("CompanyPublicId"),
                         QueriesByPeriod = cm.Field<int>("QueriesByPeriod"),
                         InitDate = cm.Field<DateTime>("InitDate"),
                         EndDate = cm.Field<DateTime>("EndDate"),

                         StatusId = cm.Field<int>("StatusId"),
                         StatusName = cm.Field<string>("StatusName"),

                         DaysByPeriod = cm.Field<int>("DaysByPeriod"),
                         Enable = cm.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = cm.Field<DateTime>("LastModify"),
                         CreateDate = cm.Field<DateTime>("CreateDate"),
                     } into cmg
                     select new PlanModel()
                     {
                         CompanyPublicId = CustomerPublicId,
                         DaysByPeriod = cmg.Key.DaysByPeriod,
                         PlanPublicId = cmg.Key.PlanPublicId,
                         QueriesByPeriod = cmg.Key.QueriesByPeriod,
                         InitDate = cmg.Key.InitDate,
                         EndDate = cmg.Key.EndDate,
                         Status = new CatalogModel()
                         {
                             ItemId = cmg.Key.StatusId,
                             ItemName = cmg.Key.StatusName,
                         },
                         Enable = cmg.Key.Enable,
                         CreateDate = cmg.Key.CreateDate,
                         LastModify = cmg.Key.LastModify,
                         RelatedPeriodoModel = 
                            (from pinf in response.DataTableResult.AsEnumerable()
                                 where !pinf.IsNull("PeriodPublicId") && 
                                 pinf.Field<int>("PlanId") == cmg.Key.PlanId
                                 group pinf by new 
                                 {
                                    PeriodPublicId = pinf.Field<string>("PeriodPublicId"),                                     
		                            AssignedQueries = pinf.Field<int>("AssignedQueries"),                                     
		                            InitDate = pinf.Field<DateTime>("InitDate"),                                     
                                    EndDate = pinf.Field<DateTime>("EndDate"),                                     
		                            TotalQueries = pinf.Field<int>("TotalQueries"),                                     
                                    Enable = pinf.Field<UInt64>("PeriodEnable") == 1 ? true : false,
                                    LastModify = pinf.Field<DateTime>("LastModify"),                                     
                                    CreateDate = pinf.Field<DateTime>("CreateDate"),                                     		                        
                                 } into pinfgr
                                 select new PeriodModel()                                 
                                 {
                                     AssignedQueries = pinfgr.Key.AssignedQueries,
                                     PeriodPublicId = pinfgr.Key.PeriodPublicId,
                                     InitDate = pinfgr.Key.InitDate,
                                     EndDate = pinfgr.Key.EndDate,
                                     TotalQueries = pinfgr.Key.TotalQueries ,
                                     Enable = pinfgr.Key.Enable,
                                     LastModify = pinfgr.Key.LastModify,
                                     CreateDate = pinfgr.Key.CreateDate,
                                 }).ToList(),                         
                         
                     }).ToList();
            }
            return oReturn;
        }

        public string PlanUpsert(string PlanPublicId, string CompanyPublicId, int QueriesByPeriod,int DaysByPeriod, CatalogModel Status, DateTime InitDate, DateTime EndDate, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPlanPublicId", PlanPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vQueriesByPeriod", QueriesByPeriod));
            lstParams.Add(DataInstance.CreateTypedParameter("vDaysByPeriod", DaysByPeriod));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", Status.ItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vInitDate", InitDate));
            lstParams.Add(DataInstance.CreateTypedParameter("vEndDate", EndDate));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "TD_PlanUpsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            if (response.ScalarResult != null)
                return response.ScalarResult.ToString();
            else
                return null;
        }

        public string PeriodUpsert(string PeriodPublicId, string PlanPublicId, int AssignedQueries, int TotalQueries, DateTime InitDate, DateTime EndDate, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPeriodPublicId", PeriodPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vPlanPublicId", PlanPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAssignedQueries", AssignedQueries));
            lstParams.Add(DataInstance.CreateTypedParameter("vTotalQueries", TotalQueries));
            lstParams.Add(DataInstance.CreateTypedParameter("vInitDate", InitDate));
            lstParams.Add(DataInstance.CreateTypedParameter("vEndDate", EndDate));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));


            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "TD_PeriodUpsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            if (response.ScalarResult != null)
                return response.ScalarResult.ToString();
            else
                return null;
        }
        
        #endregion
    }
}
