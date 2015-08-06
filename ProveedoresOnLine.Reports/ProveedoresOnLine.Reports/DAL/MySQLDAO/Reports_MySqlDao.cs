using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.Reports.Interfaces;
using ProveedoresOnLine.SurveyModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.DAL.MySQLDAO
{
    internal class Reports_MySqlDao : IReportData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Reports_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.Reports.Models.Constants.R_POL_ReportsConnectionName);
        }

        public List<SurveyModule.Models.SurveyModel> SurveyGetAllByCustomer(string CustomerPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));


            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Report_Survey_GetAllByCustomer",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<SurveyModule.Models.SurveyModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from sv in response.DataTableResult.AsEnumerable()
                     where !sv.IsNull("SurveyPublicId")
                     group sv by new
                     {
                         SurveyPublicId = sv.Field<string>("SurveyPublicId"),
                         LastModify = sv.Field<DateTime>("LastModify"),
                         SurveyConfigId = sv.Field<int>("SurveyConfigId"),
                         SurveyName = sv.Field<string>("SurveyName"),
                     } into svg
                     select new SurveyModel()
                     {
                         SurveyPublicId = svg.Key.SurveyPublicId,
                         LastModify = svg.Key.LastModify,

                         RelatedSurveyConfig = new SurveyConfigModel()
                         {
                             ItemId = svg.Key.SurveyConfigId,
                             ItemName = svg.Key.SurveyName,

                             ItemInfo =
                                (from scinf in response.DataTableResult.AsEnumerable()
                                 where !scinf.IsNull("SurveyConfigInfoId") &&
                                        scinf.Field<int>("SurveyConfigId") == svg.Key.SurveyConfigId &&
                                        scinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId
                                 group scinf by new
                                 {
                                     SurveyConfigInfoId = scinf.Field<int>("SurveyConfigInfoId"),
                                     SurveyConfigInfoTypeId = scinf.Field<int>("SurveyConfigInfoTypeId"),
                                     SurveyConfigInfoTypeName = scinf.Field<string>("SurveyConfigInfoTypeName"),
                                     SurveyConfigInfoValue = scinf.Field<string>("SurveyConfigInfoValue"),
                                     SurveyConfigInfoLargeValue = scinf.Field<string>("SurveyConfigInfoLargeValue"),
                                     SurveyConfigInfoValueName = scinf.Field<string>("SurveyConfigInfoValueName"),
                                 } into scinfg
                                 select new GenericItemInfoModel()
                                 {
                                     ItemInfoId = scinfg.Key.SurveyConfigInfoId,
                                     ItemInfoType = new CatalogModel()
                                     {
                                         ItemId = scinfg.Key.SurveyConfigInfoTypeId,
                                         ItemName = scinfg.Key.SurveyConfigInfoTypeName,
                                     },
                                     Value = scinfg.Key.SurveyConfigInfoValue,
                                     LargeValue = scinfg.Key.SurveyConfigInfoLargeValue,
                                     ValueName = scinfg.Key.SurveyConfigInfoValueName,
                                 }).ToList(),
                         },
                         SurveyInfo =
                            (from svinf in response.DataTableResult.AsEnumerable()
                             where !svinf.IsNull("SurveyInfoId") &&
                                    svinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId
                             group svinf by new
                             {
                                 SurveyInfoId = svinf.Field<int>("SurveyInfoId"),
                                 SurveyInfoTypeId = svinf.Field<int>("SurveyInfoTypeId"),
                                 SurveyInfoTypeName = svinf.Field<string>("SurveyInfoTypeName"),
                                 SurveyInfoValue = svinf.Field<string>("SurveyInfoValue"),
                                 SurveyInfoLargeValue = svinf.Field<string>("SurveyInfoLargeValue"),
                                 SurveyInfoValueName = svinf.Field<string>("SurveyInfoValueName"),

                             } into svinfg
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = svinfg.Key.SurveyInfoId,
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = svinfg.Key.SurveyInfoTypeId,
                                     ItemName = svinfg.Key.SurveyInfoTypeName,
                                 },
                                 Value = svinfg.Key.SurveyInfoValue,
                                 LargeValue = svinfg.Key.SurveyInfoLargeValue,
                                 ValueName = svinfg.Key.SurveyInfoValueName,
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

    }
}
