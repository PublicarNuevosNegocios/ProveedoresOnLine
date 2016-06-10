﻿using ProveedoresOnLine.CalificationBatch.Interfaces;
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

        #region CalificationBatch

        public List<Models.CalificationProjectBatch.CalificationProjectBatchModel> CalificationProject_GetByCustomer(string vCustomerPublicid, string vProviderPublicId, bool vEnable)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<IDbDataParameter>();
            lstparams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", vCustomerPublicid));
            lstparams.Add(DataInstance.CreateTypedParameter("vProviderPublicId",vProviderPublicId));
            lstparams.Add(DataInstance.CreateTypedParameter("vEnable", (vEnable == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_CalificationProject_GetByCustomer",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstparams,
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
                                        CreateDate = cpbg.Key.ItemCreateDate,
                                        //CalificationProjectItemInfoBatchModel
                                        CalificatioProjectItemInfoModel = new Models.CalificationProjectBatch.CalificationProjectItemInfoBatchModel()
                                        {
                                            CalificationProjectItemInfoId = cpbg.Key.CalificationProjectItemInfoId,
                                            ItemInfoScore = cpbg.Key.ItemInfoScore,
                                            Enable = cpbg.Key.ItemInfoEnable,
                                            LastModify = cpbg.Key.ItemInfoLastModify,
                                            CreateDate = cpbg.Key.ItemInfoCreateDate,
                                        }
                                    },
                                                                        
                                }
                    ).ToList();
            }
            return oReturn;
        }

        public int CalificationProjectUpsert(int vCalificationProjectId, string vCalificatonProjectPublicId, int vCalificationProjectConfigId, int vCompanyId, int vTotalScore, bool vEnable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectId", vCalificationProjectId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificatonProjectPublicId", vCalificatonProjectPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", vCalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyId", vCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyId", vTotalScore));            
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", (vEnable == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CP_CalificationProject_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });
            return Convert.ToInt32(response.ScalarResult);
        }

        public int CalificationProjectItemUpsert(int vCalificationProjectItemId, int vCalificationProjectId, int vCalificationProjectConfigItemId, int vItemScore, bool vEnable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectItemId", vCalificationProjectItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectId", vCalificationProjectId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemId", vCalificationProjectConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vItemScore", vItemScore));            
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", (vEnable == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CP_CalificationProjectItem_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });
            return Convert.ToInt32(response.ScalarResult);
        }

        public int CalificationProjectItemInfoUpsert(int vCalificationProjectItemInfoId, int vCalificationProjectItemId, int vCalificationProjectConfigItemInfoId, int vItemInfoScore, bool vEnable)
        {
            throw new NotImplementedException();
        }
        
       
        #endregion

        #region CalificationProjectBatchUtil

        #region LegalModule

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel LegalModuleInfo(string CompanyPublicId, int LegalInfoType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalInfoType", LegalInfoType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CPB_GetLegalByCompany",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            ProveedoresOnLine.Company.Models.Util.GenericItemModel oReturn = new Company.Models.Util.GenericItemModel();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from l in response.DataTableResult.AsEnumerable()
                     where !l.IsNull("LegalId")
                     group l by new
                     {
                         LegalId = l.Field<int>("LegalId"),
                         LegalTypeId = l.Field<int>("LegalTypeId"),
                         LegalTypeName = l.Field<string>("LegalTypeName"),
                         LegalEnable = l.Field<UInt64>("LegalEnable") == 1 ? true : false,
                         LegalLastModify = l.Field<DateTime>("LegalLastModify"),
                         LegalCreateDate = l.Field<DateTime>("LegalCreateDate"),
                     }
                         into lg
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                         {
                             ItemId = lg.Key.LegalId,
                             ItemType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = lg.Key.LegalTypeId,
                                 ItemName = lg.Key.LegalTypeName,
                             },
                             Enable = lg.Key.LegalEnable,
                             LastModify = lg.Key.LegalLastModify,
                             CreateDate = lg.Key.LegalCreateDate,
                             ItemInfo =
                                (from linf in response.DataTableResult.AsEnumerable()
                                 where !linf.IsNull("LegalInfoId") &&
                                     linf.Field<int>("LegalId") == lg.Key.LegalId
                                 group linf by new
                                 {
                                     LegalInfoId = linf.Field<int>("LegalInfoId"),
                                     LegalInfoTypeId = linf.Field<int>("LegalItemInfoTypeId"),
                                     LegalInfoTypeName = linf.Field<string>("LegalItemInfoTypeName"),
                                     LegalInfoValue = linf.Field<string>("LegalInfoValue"),
                                     LegalInfoLargeValue = linf.Field<string>("LegalInfoLargeValue"),
                                     LegalInfoEnable = linf.Field<UInt64>("LegalInfoEnable") == 1 ? true : false,
                                     LegalInfoLastModify = linf.Field<DateTime>("LegalInfoLastModify"),
                                     LegalInfoCreateDate = linf.Field<DateTime>("LegalInfoCreateDate"),
                                 }
                                     into linfg
                                     select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                     {
                                         ItemInfoId = linfg.Key.LegalInfoId,
                                         ItemInfoType = new Company.Models.Util.CatalogModel()
                                         {
                                             ItemId = linfg.Key.LegalInfoTypeId,
                                             ItemName = linfg.Key.LegalInfoTypeName,
                                         },
                                         Value = linfg.Key.LegalInfoValue,
                                         LargeValue = linfg.Key.LegalInfoLargeValue,
                                         Enable = linfg.Key.LegalInfoEnable,
                                         LastModify = linfg.Key.LegalInfoLastModify,
                                         CreateDate = linfg.Key.LegalInfoCreateDate,
                                     }).ToList(),
                         }).FirstOrDefault();
            }

            return oReturn;
        }

        #endregion

        #endregion

       
       
    }
}
