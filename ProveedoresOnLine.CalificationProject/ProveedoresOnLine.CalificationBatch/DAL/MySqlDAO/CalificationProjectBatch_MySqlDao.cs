using ProveedoresOnLine.CalificationBatch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ProveedoresOnLine.CalificationBatch.DAL.MySqlDAO
{
    internal class CalificationProjectBatch_MySqlDao : ICalificationProjectBatchData
    {
        private ADO.Interfaces.IADO DataInstance;

        public CalificationProjectBatch_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.CalificationBatch.Models.Constants.C_POL_CalificatioProjectConnectionName);
        }

        public List<Models.CalificationProjectBatch.CalificationProjectBatchModel> CalificationProject_GetByCustomer(string vCustomerPublicid, string vProviderPublicId, bool Enable)
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_CalificationProject_GetByCustomer",
                CommandType = System.Data.CommandType.StoredProcedure
            });

            List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> oReturn = new List<Models.CalificationProjectBatch.CalificationProjectBatchModel>();

            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0) 
            {
                oReturn =
                    (
                        from cpb in response.DataTableResult.AsEnumerable()
                        where !cpb.IsNull("CalificationProjectId")
                        group cpb
                        by new
                        {
                            //CalificationProjectBatch

                            CalificationProjectId = cpb.Field<int>("CalificationProjectId"),
                            CalificationProjectPublicId = cpb.Field<string>("CalificationProjectPublicId"),
                            CompanyPublicId = cpb.Field<string>("CompanyPublicId"),
                            CalificationProjectConfigId = cpb.Field<int>("CalificationProjectConfigId"),
                            TotalScore = cpb.Field<int>("TotalScore"),
                            Enable = cpb.Field<UInt64>("Enable") == 1 ? true : false,
                            LastModify = cpb.Field<DateTime>("LastModify"),
                            CreateDate = cpb.Field<DateTime>("CreateDate"),

                            //CalificationProjectItemBatch

                            CalificationProjectItemId = cpb.Field<int>("CalificationProjectItemId"),
                            ItemScore = cpb.Field<int>("ItemScore"),
                            ItemEnable = cpb.Field<UInt64>("ItemEnable") == 1 ? true : false,
                            ItemLastModify = cpb.Field<DateTime>("ItemLastModify"),
                            ItemCreateDate = cpb.Field<DateTime>("ItemCreateDate"),

                            //CalificationProjectItemInfoBatch

                            CalificationProjectItemInfoId = cpb.Field<int>("CalificationProjectItemInfoId"),
                            ItemInfoScore = cpb.Field<int>("ItemInfoScore"),
                            ItemInfoEnable = cpb.Field<UInt64>("ItemInfoEnable") == 1 ? true : false,
                            ItemInfoLastModify = cpb.Field<DateTime>("ItemInfoLastModify"),
                            ItemInfoCreateDate = cpb.Field<DateTime>("ItemInfoCreateDate")
                        }
                            into cpbg
                            select new Models.CalificationProjectBatch.CalificationProjectBatchModel()
                                {
                                    //CalificationProjectItemBatch
                                    CalificationProjectId = cpbg.Key.CalificationProjectId,
                                    CalificationProjectPublicId = cpbg.Key.CalificationProjectPublicId,
                                    Company = new Company.Models.Company.CompanyModel
                                    {
                                        CompanyPublicId = cpbg.Key.CompanyPublicId
                                    },
                                    TotalScore = cpbg.Key.TotalScore,
                                    Enable = cpbg.Key.Enable,
                                    LastModify = cpbg.Key.LastModify,
                                    CreateDate = cpbg.Key.CreateDate,

                                    //CalificationProjectItemBatchModel

                                    CalificationProjectItemBatchModel = new Models.CalificationProjectBatch.CalificationProjectItemBatchModel ()
                                    {
                                        CalificationProjectItemId = cpbg.Key.CalificationProjectItemId,
                                        ItemScore = cpbg.Key.ItemScore,
                                        Enable = cpbg.Key.ItemEnable,
                                        LastModify = cpbg.Key.ItemLastModify,
                                        CreateDate = cpbg.Key.ItemCreateDate
                                    },
                                    //CalificationProjectItemInfoBatchModel

                                    CalificationProjectItemInfoBatchModel = new Models.CalificationProjectBatch.CalificationProjectItemInfoBatchModel()
                                    {
                                        CalificationProjectItemInfoId = cpbg.Key.CalificationProjectItemInfoId,
                                        ItemInfoScore = cpbg.Key.ItemInfoScore,
                                        Enable = cpbg.Key.ItemInfoEnable,
                                        LastModify = cpbg.Key.ItemInfoLastModify,
                                        CreateDate = cpbg.Key.ItemInfoCreateDate
                                    }
                                }
                    ).ToList();
            }
            return oReturn;
        }
        
    }
}
