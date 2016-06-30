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
            lstparams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", vProviderPublicId));
            lstparams.Add(DataInstance.CreateTypedParameter("vEnable", (vEnable == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_CalificationProject_GetByCustomer",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> oReturn = null;

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
                        }
                            into cpbg
                            select new Models.CalificationProjectBatch.CalificationProjectBatchModel()
                                {
                                    //CalificationProjectItemBatch
                                    CalificationProjectId = cpbg.Key.CalificationProjectId,
                                    CalificationProjectPublicId = cpbg.Key.CalificationProjectPublicId,
                                    ProjectConfigModel =
                                        (from cpc in response.DataTableResult.AsEnumerable()
                                         where !cpc.IsNull("CalificationProjectConfigId") &&
                                                cpc.Field<int>("CalificationProjectId") == cpbg.Key.CalificationProjectId &&
                                                cpc.Field<int>("CalificationProjectConfigId") == cpbg.Key.CalificationProjectConfigId
                                         group cpc by new
                                         {
                                             CalificationProjectConfigId = cpc.Field<int>("CalificationProjectConfigId"),
                                             CustomerPublicId = cpc.Field<string>("CustomerPublicId"),
                                             CalificationProjectconfigName = cpc.Field<string>("CalificationProjectconfigName"),
                                             ProjectConfigEnable = cpc.Field<UInt64>("ProjectConfigEnable") == 1 ? true : false,
                                             ProjectConfigLastModify = cpc.Field<DateTime>("ProjectConfigLastModify"),
                                             ProjectConfigCreateDate = cpc.Field<DateTime>("ProjectConfigCreateDate"),
                                         }
                                             into cpcg
                                             select new ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                                             {
                                                 CalificationProjectConfigId = cpcg.Key.CalificationProjectConfigId,
                                                 Company = new Company.Models.Company.CompanyModel()
                                                 {
                                                     CompanyPublicId = cpcg.Key.CustomerPublicId,
                                                 },
                                                 CalificationProjectConfigName = cpcg.Key.CalificationProjectconfigName,
                                                 Enable = cpcg.Key.ProjectConfigEnable,
                                                 LastModify = cpcg.Key.ProjectConfigLastModify,
                                                 CreateDate = cpcg.Key.ProjectConfigCreateDate,
                                             }).FirstOrDefault(),
                                    RelatedProvider = new Company.Models.Company.CompanyModel
                                    {
                                        CompanyPublicId = cpbg.Key.CompanyPublicId
                                    },
                                    TotalScore = cpbg.Key.TotalScore,
                                    Enable = cpbg.Key.Enable,
                                    LastModify = cpbg.Key.LastModify,
                                    CreateDate = cpbg.Key.CreateDate,

                                    //CalificationProjectItemBatchModel

                                    CalificationProjectItemBatchModel =
                                        (from cpit in response.DataTableResult.AsEnumerable()
                                         where !cpit.IsNull("CalificationProjectItemId") &&
                                                cpit.Field<int>("CalificationProjectId") == cpbg.Key.CalificationProjectId
                                         group cpit by new
                                         {
                                             //CalificationProjectItemBatch

                                             CalificationProjectItemId = cpit.Field<int>("CalificationProjectItemId"),
                                             CalificationProjectConfigItemId = cpit.Field<int>("CalificationProjectConfigItemId"),
                                             ItemScore = cpit.Field<int>("ItemScore"),
                                             ItemEnable = cpit.Field<UInt64>("ItemEnable") == 1 ? true : false,
                                             ItemLastModify = cpit.Field<DateTime>("ItemLastModify"),
                                             ItemCreateDate = cpit.Field<DateTime>("ItemCreateDate"),
                                         }
                                             into cpitg
                                             select new Models.CalificationProjectBatch.CalificationProjectItemBatchModel()
                                             {
                                                 CalificationProjectItemId = cpitg.Key.CalificationProjectItemId,
                                                 CalificationProjectConfigItem =
                                                    (from cpci in response.DataTableResult.AsEnumerable()
                                                     where !cpci.IsNull("CalificationProjectConfigItemId") &&
                                                            cpci.Field<int>("CalificationProjectItemId") == cpitg.Key.CalificationProjectItemId &&
                                                            cpci.Field<int>("CalificationProjectConfigItemId") == cpitg.Key.CalificationProjectConfigItemId
                                                     group cpci by new
                                                     {
                                                         CalificationProjectConfigItemId = cpci.Field<int>("CalificationProjectConfigItemId"),
                                                         CalificationProjectConfigItemName = cpci.Field<string>("CalificationProjectConfigItemName"),
                                                         CalificationProjectConfigItemType = cpci.Field<int>("CalificationProjectConfigItemType"),
                                                         CalificationProjectConfigItemTypeName = cpci.Field<string>("CalificationProjectConfigItemTypeName"),
                                                         ProjectConfigItemEnable = cpci.Field<UInt64>("ProjectConfigItemEnable") == 1 ? true : false,
                                                         ProjectConfigItemLastModify = cpci.Field<DateTime>("ProjectConfigItemLastModify"),
                                                         ProjectConfigItemCreateDate = cpci.Field<DateTime>("ProjectConfigItemCreateDate")
                                                     }
                                                         into cpcig
                                                         select new ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemModel()
                                                             {
                                                                 CalificationProjectConfigItemId = cpcig.Key.CalificationProjectConfigItemId,
                                                                 CalificationProjectConfigItemName = cpcig.Key.CalificationProjectConfigItemName,
                                                                 CalificationProjectConfigItemType = new Company.Models.Util.CatalogModel()
                                                                 {
                                                                     ItemId = cpcig.Key.CalificationProjectConfigItemType,
                                                                     ItemName = cpcig.Key.CalificationProjectConfigItemTypeName,
                                                                 },
                                                                 Enable = cpcig.Key.ProjectConfigItemEnable,
                                                                 LastModify = cpcig.Key.ProjectConfigItemLastModify,
                                                                 CreateDate = cpcig.Key.ProjectConfigItemCreateDate
                                                             }).FirstOrDefault(),
                                                 ItemScore = cpitg.Key.ItemScore,
                                                 Enable = cpitg.Key.ItemEnable,
                                                 LastModify = cpitg.Key.ItemLastModify,
                                                 CreateDate = cpitg.Key.ItemCreateDate,
                                                 CalificatioProjectItemInfoModel =
                                                     (from cpitinf in response.DataTableResult.AsEnumerable()
                                                      where !cpitinf.IsNull("CalificationProjectItemInfoId") &&
                                                            cpitinf.Field<int>("CalificationProjectItemId") == cpitg.Key.CalificationProjectItemId
                                                      group cpitinf by new
                                                      {
                                                          //CalificationProjectItemInfoBatch

                                                          CalificationProjectItemInfoId = cpitinf.Field<int>("CalificationProjectItemInfoId"),
                                                          CalificationProjectConfigItemInfoId = cpitinf.Field<int>("CalificationProjectConfigItemInfoId"),
                                                          ItemInfoScore = cpitinf.Field<int>("ItemInfoScore"),
                                                          ItemInfoEnable = cpitinf.Field<UInt64>("ItemInfoEnable") == 1 ? true : false,
                                                          ItemInfoLastModify = cpitinf.Field<DateTime>("ItemInfoLastModify"),
                                                          ItemInfoCreateDate = cpitinf.Field<DateTime>("ItemInfoCreateDate")
                                                      }
                                                          into cpitinfg
                                                          select new Models.CalificationProjectBatch.CalificationProjectItemInfoBatchModel()
                                                          {
                                                              CalificationProjectItemInfoId = cpitinfg.Key.CalificationProjectItemInfoId,
                                                              CalificationProjectConfigItemInfoModel =
                                                                  (from cpcii in response.DataTableResult.AsEnumerable()
                                                                   where !cpcii.IsNull("CalificationProjectConfigItemInfoId")
                                                                        && cpcii.Field<int>("CalificationProjectItemInfoId") == cpitinfg.Key.CalificationProjectItemInfoId
                                                                        && cpcii.Field<int>("CalificationProjectConfigItemInfoId") == cpitinfg.Key.CalificationProjectConfigItemInfoId
                                                                   group cpcii by new
                                                                   {
                                                                       CalificationProjectConfigItemInfoId = cpcii.Field<int>("CalificationProjectConfigItemInfoId"),
                                                                       Question = cpcii.Field<int>("Question"),
                                                                       QuestionName = cpcii.Field<string>("QuestionName"),
                                                                       ConfigItemInfoRule = cpcii.Field<int>("ConfigItemInfoRule"),
                                                                       ConfigItemInfoRuleName = cpcii.Field<string>("ConfigItemInfoRuleName"),
                                                                       ValueType = cpcii.Field<int>("ValueType"),
                                                                       ValueTypeName = cpcii.Field<string>("ValueTypeName"),
                                                                       ConfigitemInfoValue = cpcii.Field<string>("ConfigitemInfoValue"),
                                                                       Score = cpcii.Field<string>("Score"),
                                                                       ConfigItemInfoEnable = cpcii.Field<UInt64>("ConfigItemInfoEnable") == 1 ? true : false,
                                                                       ConfigItemInfoLastModify = cpcii.Field<DateTime>("ConfigItemInfoLastModify"),
                                                                       ConfigItemInfoCreateDate = cpcii.Field<DateTime>("ConfigItemInfoCreateDate"),
                                                                   }
                                                                       into cpciig
                                                                       select new ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemInfoModel()
                                                                           {
                                                                               CalificationProjectConfigItemInfoId = cpciig.Key.CalificationProjectConfigItemInfoId,
                                                                               Question = new Company.Models.Util.CatalogModel()
                                                                               {
                                                                                   ItemId = cpciig.Key.Question,
                                                                                   ItemName = cpciig.Key.QuestionName,
                                                                               },
                                                                               Rule = new Company.Models.Util.CatalogModel
                                                                               {
                                                                                   ItemId = cpciig.Key.ConfigItemInfoRule,
                                                                                   ItemName = cpciig.Key.ConfigItemInfoRuleName,
                                                                               },
                                                                               ValueType = new Company.Models.Util.CatalogModel()
                                                                               {
                                                                                   ItemId = cpciig.Key.ValueType,
                                                                                   ItemName = cpciig.Key.ValueTypeName,
                                                                               },
                                                                               Value = cpciig.Key.ConfigitemInfoValue,
                                                                               Score = cpciig.Key.Score,
                                                                               Enable = cpciig.Key.ConfigItemInfoEnable,
                                                                               LastModify = cpciig.Key.ConfigItemInfoLastModify,
                                                                               CreateDate = cpciig.Key.ConfigItemInfoCreateDate,
                                                                           }).FirstOrDefault(),
                                                              ItemInfoScore = cpitinfg.Key.ItemInfoScore,
                                                              Enable = cpitinfg.Key.ItemInfoEnable,
                                                              LastModify = cpitinfg.Key.ItemInfoLastModify,
                                                              CreateDate = cpitinfg.Key.ItemInfoCreateDate,
                                                          }).ToList(),
                                             }).ToList(),
                                }
                    ).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> CalificationProject_GetProviderByCustomer(string CustomerPublicId, string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_CalificationProject_GetProviderByCustomer",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cp in response.DataTableResult.AsEnumerable()
                     where !cp.IsNull("CalificationProjectId")
                     group cp by new
                     {
                         CalificationProjectId = cp.Field<int>("CalificationProjectId"),
                         CalificationProjectPublicId = cp.Field<string>("CalificationProjectPublicId"),
                         CompanyPublicId = cp.Field<string>("CompanyPublicId"),
                         CalificationProjectConfigId = cp.Field<int>("CalificationProjectConfigId"),
                         TotalScore = cp.Field<int>("TotalScore"),
                         Enable = cp.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = cp.Field<DateTime>("LastModify"),
                         CreateDate = cp.Field<DateTime>("CreateDate"),
                     }
                         into cpg
                         select new ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel()
                         {
                             CalificationProjectId = cpg.Key.CalificationProjectId,
                             CalificationProjectPublicId = cpg.Key.CalificationProjectPublicId,
                             ProjectConfigModel = new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                             {
                                 CalificationProjectConfigId = cpg.Key.CalificationProjectConfigId,
                             },
                             RelatedProvider = new Company.Models.Company.CompanyModel()
                             {
                                 CompanyPublicId = cpg.Key.CalificationProjectPublicId,
                             },
                             TotalScore = cpg.Key.TotalScore,
                             Enable = cpg.Key.Enable,
                             LastModify = cpg.Key.LastModify,
                             CreateDate = cpg.Key.CreateDate,
                             CalificationProjectItemBatchModel =
                                (from cpi in response.DataTableResult.AsEnumerable()
                                 where !cpi.IsNull("CalificationProjectItemId") &&
                                        cpi.Field<int>("CalificationProjectId") == cpg.Key.CalificationProjectId
                                 group cpi by new
                                 {
                                     CalificationProjectItemId = cpi.Field<int>("CalificationProjectItemId"),
                                     CalificationProjectId = cpi.Field<int>("CalificationProjectId"),
                                     CalificationProjectConfigItemId = cpi.Field<int>("CalificationProjectConfigItemId"),
                                     ItemScore = cpi.Field<int>("ItemScore"),
                                     ItemEnable = cpi.Field<UInt64>("ItemEnable") == 1 ? true : false,
                                     ItemLastModify = cpi.Field<DateTime>("ItemLastModify"),
                                     ItemCreateDate = cpi.Field<DateTime>("ItemCreateDate"),
                                 }
                                     into cpig
                                     select new ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel()
                                     {
                                         CalificationProjectItemId = cpig.Key.CalificationProjectItemId,
                                         CalificationProjectConfigItem = new CalificationProject.Models.CalificationProject.ConfigItemModel()
                                         {
                                             CalificationProjectConfigItemId = cpig.Key.CalificationProjectConfigItemId,
                                         },
                                         ItemScore = cpig.Key.ItemScore,
                                         LastModify = cpig.Key.ItemLastModify,
                                         CreateDate = cpig.Key.ItemCreateDate,
                                         CalificatioProjectItemInfoModel =
                                            (from cpiinf in response.DataTableResult.AsEnumerable()
                                             where !cpiinf.IsNull("CalificationProjectItemInfoId") &&
                                                   cpiinf.Field<int>("CalificationProjectItemId") == cpig.Key.CalificationProjectItemId
                                             group cpiinf by new
                                             {
                                                 CalificationProjectItemInfoId = cpiinf.Field<int>("CalificationProjectItemInfoId"),
                                                 CalificationProjectConfigItemInfoId = cpiinf.Field<int>("CalificationProjectConfigItemInfoId"),
                                                 ItemInfoScore = cpiinf.Field<int>("ItemInfoScore"),
                                                 ItemInfoEnable = cpiinf.Field<UInt64>("ItemInfoEnable") == 1 ? true : false,
                                                 ItemInfoLastModify = cpiinf.Field<DateTime>("ItemInfoLastModify"),
                                                 ItemInfoCreateDate = cpiinf.Field<DateTime>("ItemInfoCreateDate"),
                                             }
                                                 into cpiinfg
                                                 select new ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemInfoBatchModel()
                                                 {
                                                     CalificationProjectItemInfoId = cpiinfg.Key.CalificationProjectItemInfoId,
                                                     CalificationProjectConfigItemInfoModel = new CalificationProject.Models.CalificationProject.ConfigItemInfoModel()
                                                     {
                                                         CalificationProjectConfigItemInfoId = cpiinfg.Key.CalificationProjectConfigItemInfoId,
                                                     },
                                                     ItemInfoScore = cpiinfg.Key.ItemInfoScore,
                                                     Enable = cpiinfg.Key.ItemInfoEnable,
                                                     LastModify = cpiinfg.Key.ItemInfoLastModify,
                                                     CreateDate = cpiinfg.Key.ItemInfoCreateDate,
                                                 }).ToList(),
                                     }).ToList(),
                         }).ToList();
            }

            return oReturn;
        }

        public int CalificationProjectUpsert(int vCalificationProjectId, string vCalificatonProjectPublicId, int vCalificationProjectConfigId, string vCompanyPublicId, int vTotalScore, bool vEnable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectId", vCalificationProjectId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificatonProjectPublicId", vCalificatonProjectPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", vCalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", vCompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vTotalScore", vTotalScore));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", vEnable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CP_CalificationProject_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return int.Parse(response.ScalarResult.ToString());
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

            return int.Parse(response.ScalarResult.ToString());
        }

        public int CalificationProjectItemInfoUpsert(int vCalificationProjectItemInfoId, int vCalificationProjectItemId, int vCalificationProjectConfigItemInfoId, int vItemInfoScore, bool vEnable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectItemInfoId", vCalificationProjectItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectItemId", vCalificationProjectItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemInfoId", vCalificationProjectConfigItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vItemInfoScore", vItemInfoScore));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", (vEnable == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CP_CalificationProjectItemInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return int.Parse(response.ScalarResult.ToString());
        }


        #endregion

        #region MarketPlace

        public List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel> CalificationProjectConfig_GetByCustomerPublicId(string CustomerPublicId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_CaficationProjectConfig_GetByCustomerPublicId",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cpc in response.DataTableResult.AsEnumerable()
                     where !cpc.IsNull("CalificationProjectConfigId")
                     group cpc by new
                     {
                         CalificationProjectConfigId = cpc.Field<int>("CalificationProjectConfigId"),
                         CompanyPublicId = cpc.Field<string>("CompanyPublicId"),
                         CalificationProjectConfigName = cpc.Field<string>("CalificationProjectConfigName"),
                         CalificationProjectConfigEnable = cpc.Field<UInt64>("CalificationProjectConfigEnable") == 1 ? true : false,
                         CalificationProjectConfigLastModify = cpc.Field<DateTime>("CalificationProjectConfigLastModify"),
                         CalificationProjectConfigCreateDate = cpc.Field<DateTime>("CalificationProjectConfigCreateDate"),
                     }
                         into cpcg
                         select new ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                         {
                             CalificationProjectConfigId = cpcg.Key.CalificationProjectConfigId,
                             Company = new Company.Models.Company.CompanyModel()
                             {
                                 CompanyPublicId = cpcg.Key.CompanyPublicId,
                             },
                             CalificationProjectConfigName = cpcg.Key.CalificationProjectConfigName,
                             Enable = cpcg.Key.CalificationProjectConfigEnable,
                             LastModify = cpcg.Key.CalificationProjectConfigLastModify,
                             CreateDate = cpcg.Key.CalificationProjectConfigCreateDate,
                             ConfigItemModel =
                                (from cpcit in response.DataTableResult.AsEnumerable()
                                 where !cpcit.IsNull("CalificationProjectConfigItemId") &&
                                       cpcit.Field<int>("CalificationProjectConfigId") == cpcg.Key.CalificationProjectConfigId
                                 group cpcit by new
                                 {
                                     CalificationProjectConfigItemId = cpcit.Field<int>("CalificationProjectConfigItemId"),
                                     CalificationProjectConfigItemName = cpcit.Field<string>("CalificationProjectConfigItemName"),
                                     CalificationProjectConfigItemTypeId = cpcit.Field<int>("CalificationProjectConfigItemTypeId"),
                                     CalificationProjectConfigItemTypeName = cpcit.Field<string>("CalificationProjectConfigItemTypeName"),
                                     CalificationProjectConfigItemEnable = cpcit.Field<UInt64>("CalificationProjectConfigItemEnable") == 1 ? true : false,
                                     CalificationProjectConfigItemLastModify = cpcit.Field<DateTime>("CalificationProjectConfigItemLastModify"),
                                     CalificationProjectConfigItemCreateDate = cpcit.Field<DateTime>("CalificationProjectConfigItemCreateDate"),
                                 }
                                     into cpcitg
                                     select new ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemModel()
                                     {
                                         CalificationProjectConfigItemId = cpcitg.Key.CalificationProjectConfigItemId,
                                         CalificationProjectConfigItemName = cpcitg.Key.CalificationProjectConfigItemName,
                                         CalificationProjectConfigItemType = new Company.Models.Util.CatalogModel()
                                         {
                                             ItemId = cpcitg.Key.CalificationProjectConfigItemTypeId,
                                             ItemName = cpcitg.Key.CalificationProjectConfigItemTypeName,
                                         },
                                         Enable = cpcitg.Key.CalificationProjectConfigItemEnable,
                                         LastModify = cpcitg.Key.CalificationProjectConfigItemLastModify,
                                         CreateDate = cpcitg.Key.CalificationProjectConfigItemCreateDate,
                                         CalificationProjectConfigItemInfoModel =
                                            (from cpcinf in response.DataTableResult.AsEnumerable()
                                             where !cpcinf.IsNull("CalificationProjectConfigItemInfoId") &&
                                                   cpcinf.Field<int>("CalificationProjectConfigItemId") == cpcitg.Key.CalificationProjectConfigItemId
                                             group cpcinf by new
                                             {
                                                 CalificationProjectConfigItemInfoId = cpcinf.Field<int>("CalificationProjectConfigItemInfoId"),
                                                 QuestionId = cpcinf.Field<int>("QuestionId"),
                                                 QuestionName = cpcinf.Field<string>("QuestionName"),
                                                 RuleId = cpcinf.Field<int>("RuleId"),
                                                 RuleName = cpcinf.Field<string>("RuleName"),
                                                 ValueTypeId = cpcinf.Field<int>("ValueTypeId"),
                                                 ValueTypeName = cpcinf.Field<string>("ValueTypeName"),
                                                 Value = cpcinf.Field<string>("Value"),
                                                 Score = cpcinf.Field<string>("Score"),
                                                 CalificationProjectConfigItemInfoEnable = cpcinf.Field<UInt64>("CalificationProjectConfigItemInfoEnable") == 1 ? true : false,
                                                 CalificationProjectConfigItemInfoLastModify = cpcinf.Field<DateTime>("CalificationProjectConfigItemInfoLastModify"),
                                                 CalificationProjectConfigItemInfoCreateDate = cpcinf.Field<DateTime>("CalificationProjectConfigItemInfoCreateDate"),
                                             }
                                                 into cpcinfg
                                                 select new ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemInfoModel()
                                                 {
                                                     CalificationProjectConfigItemInfoId = cpcinfg.Key.CalificationProjectConfigItemInfoId,
                                                     Question = new Company.Models.Util.CatalogModel()
                                                     {
                                                         ItemId = cpcinfg.Key.QuestionId,
                                                         ItemName = cpcinfg.Key.QuestionName,
                                                     },
                                                     Rule = new Company.Models.Util.CatalogModel()
                                                     {
                                                         ItemId = cpcinfg.Key.RuleId,
                                                         ItemName = cpcinfg.Key.RuleName,
                                                     },
                                                     ValueType = new Company.Models.Util.CatalogModel()
                                                     {
                                                         ItemId = cpcinfg.Key.ValueTypeId,
                                                         ItemName = cpcinfg.Key.ValueTypeName,
                                                     },
                                                     Value = cpcinfg.Key.Value,
                                                     Score = cpcinfg.Key.Score,
                                                     Enable = cpcinfg.Key.CalificationProjectConfigItemInfoEnable,
                                                     LastModify = cpcinfg.Key.CalificationProjectConfigItemInfoLastModify,
                                                     CreateDate = cpcinfg.Key.CalificationProjectConfigItemInfoCreateDate,
                                                 }).ToList(),
                                     }).ToList(),
                             ConfigValidateModel =
                                 (from cpcv in response.DataTableResult.AsEnumerable()
                                  where !cpcv.IsNull("CalificationProjectConfigValidateId") &&
                                        cpcv.Field<int>("CalificationProjectConfigId") == cpcg.Key.CalificationProjectConfigId
                                  group cpcv by new
                                  {
                                      CalificationProjectConfigValidateId = cpcv.Field<int>("CalificationProjectConfigValidateId"),
                                      OperatorId = cpcv.Field<int>("OperatorId"),
                                      OperatorName = cpcv.Field<string>("OperatorName"),
                                      CalificationProjectConfigValidateValue = cpcv.Field<string>("CalificationProjectConfigValidateValue"),
                                      CalificationProjectConfigValidateResult = cpcv.Field<string>("CalificationProjectConfigValidateResult"),
                                      CalificationProjectConfigValidateEnable = cpcv.Field<UInt64>("CalificationProjectConfigValidateEnable") == 1 ? true : false,
                                      CalificationProjectConfigValidateLastModify = cpcv.Field<DateTime>("CalificationProjectConfigValidateLastModify"),
                                      CalificationProjectConfigValidateCreateDate = cpcv.Field<DateTime>("CalificationProjectConfigValidateCreateDate"),
                                  }
                                      into cpcvg
                                      select new ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigValidateModel()
                                      {
                                          CalificationProjectConfigId = cpcvg.Key.CalificationProjectConfigValidateId,
                                          Operator = new Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = cpcvg.Key.OperatorId,
                                              ItemName = cpcvg.Key.OperatorName,
                                          },
                                          Value = cpcvg.Key.CalificationProjectConfigValidateValue,
                                          Result = cpcvg.Key.CalificationProjectConfigValidateResult,
                                          Enable = cpcvg.Key.CalificationProjectConfigValidateEnable,
                                          LastModify = cpcvg.Key.CalificationProjectConfigValidateLastModify,
                                          CreateDate = cpcvg.Key.CalificationProjectConfigValidateCreateDate,
                                      }).ToList(),
                         }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region Calification Project Batch Util

        #region Legal Module

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> LegalModuleInfo(string CompanyPublicId, int LegalInfoType)
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

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = new List<Company.Models.Util.GenericItemModel>();

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
                         }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region Financial Module

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> FinancialModuleInfo(string CompanyPublicId, int FinancialInfoType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialInfoType", FinancialInfoType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CPB_GetFinancialByCompany",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = new List<Company.Models.Util.GenericItemModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from f in response.DataTableResult.AsEnumerable()
                     where !f.IsNull("FinancialId")
                     group f by new
                     {
                         FinancialId = f.Field<int>("FinancialId"),
                         FinancialTypeId = f.Field<int>("FinancialTypeId"),
                         FinancialTypeName = f.Field<string>("FinancialTypeName"),
                         FinancialName = f.Field<string>("FinancialName"),
                         FinancialEnable = f.Field<UInt64>("FinancialEnable") == 1 ? true : false,
                         FinancialLastModify = f.Field<DateTime>("FinancialLastModify"),
                         FinancialCreateDate = f.Field<DateTime>("FinancialCreateDate"),
                     }
                         into fg
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                         {
                             ItemId = fg.Key.FinancialId,
                             ItemName = fg.Key.FinancialName,
                             ItemType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = fg.Key.FinancialTypeId,
                                 ItemName = fg.Key.FinancialTypeName,
                             },
                             Enable = fg.Key.FinancialEnable,
                             LastModify = fg.Key.FinancialLastModify,
                             CreateDate = fg.Key.FinancialCreateDate,
                             ItemInfo =
                                (from finf in response.DataTableResult.AsEnumerable()
                                 where !finf.IsNull("FinancialInfoId") &&
                                        finf.Field<int>("FinancialId") == fg.Key.FinancialId
                                 group finf by new
                                 {
                                     FinancialInfoId = finf.Field<int>("FinancialInfoId"),
                                     FinancialInfoTypeId = finf.Field<int>("FinancialInfoTypeId"),
                                     FinancialInfoTypeName = finf.Field<string>("FinancialInfoTypeName"),
                                     FinancialInfoValue = finf.Field<string>("FinancialInfoValue"),
                                     FinancialInfoLargeValue = finf.Field<string>("FinancialInfoLargeValue"),
                                     FinancialInfoEnable = finf.Field<UInt64>("FinancialInfoEnable") == 1 ? true : false,
                                     FinancialInfoLastModify = finf.Field<DateTime>("FinancialInfoLastModify"),
                                     FinancialInfoCreateDate = finf.Field<DateTime>("FinancialInfoCreateDate"),
                                 }
                                     into finfg
                                     select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                     {
                                         ItemInfoId = finfg.Key.FinancialInfoId,
                                         ItemInfoType = new Company.Models.Util.CatalogModel()
                                         {
                                             ItemId = finfg.Key.FinancialInfoTypeId,
                                             ItemName = finfg.Key.FinancialInfoTypeName,
                                         },
                                         Value = finfg.Key.FinancialInfoValue,
                                         LargeValue = finfg.Key.FinancialInfoLargeValue,
                                         Enable = finfg.Key.FinancialInfoEnable,
                                         LastModify = finfg.Key.FinancialInfoLastModify,
                                         CreateDate = finfg.Key.FinancialInfoCreateDate,
                                     }).ToList(),
                         }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region Commercial Module

        public List<Company.Models.Util.GenericItemModel> CommercialModuleInfo(string CompanyPublicId, int CommercialInfoType)
        {
            List<IDbDataParameter> lstparams = new List<IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstparams.Add(DataInstance.CreateTypedParameter("vCommercialInfoType", CommercialInfoType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CPB_GetCommercialByCompany",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = new List<Company.Models.Util.GenericItemModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (
                        from cial in response.DataTableResult.AsEnumerable()
                        where !cial.IsNull("CommercialId")
                        group cial by new
                        {
                            CommercialId = cial.Field<int>("CommercialId"),
                            CommercialTypeId = cial.Field<int>("CommercialTypeId"),
                            CommercialTypeName = cial.Field<string>("CommercialTypeName"),
                            CommercialEnable = cial.Field<UInt64>("CommercialEnable") == 1 ? true : false,
                            CommercialLastModify = cial.Field<DateTime>("CommercialLastModify"),
                            CommercialCreateDate = cial.Field<DateTime>("CommercialCreateDate"),
                        }
                            into cialg
                            select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                            {
                                ItemId = cialg.Key.CommercialId,
                                ItemType = new Company.Models.Util.CatalogModel()
                                {
                                    ItemId = cialg.Key.CommercialTypeId,
                                    ItemName = cialg.Key.CommercialTypeName,
                                },
                                Enable = cialg.Key.CommercialEnable,
                                LastModify = cialg.Key.CommercialLastModify,
                                CreateDate = cialg.Key.CommercialCreateDate,
                                ItemInfo =
                                   (from cialinf in response.DataTableResult.AsEnumerable()
                                    where !cialinf.IsNull("CommercialInfoId") &&
                                        cialinf.Field<int>("CommercialId") == cialg.Key.CommercialId
                                    group cialinf by new
                                    {
                                        CommercialInfoId = cialinf.Field<int>("CommercialInfoId"),
                                        CommercialInfoTypeId = cialinf.Field<int>("CommercialItemInfoTypeId"),
                                        CommerciallInfoTypeName = cialinf.Field<string>("CommercialItemInfoTypeName"),
                                        CommercialInfoLargeValue = cialinf.Field<string>("CommercialInfoLargeValue"),
                                        CommercialInfoValue = cialinf.Field<string>("CommercialInfoValue"),
                                        CommercialInfoEnable = cialinf.Field<UInt64>("CommercialInfoEnable") == 1 ? true : false,
                                        CommercialInfoLastModify = cialinf.Field<DateTime>("CommercialInfoLastModify"),
                                        CommercialInfoCreateDate = cialinf.Field<DateTime>("CommercialInfoCreateDate"),
                                    }
                                        into cialinfg
                                        select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                        {
                                            ItemInfoId = cialinfg.Key.CommercialInfoId,
                                            ItemInfoType = new Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = cialinfg.Key.CommercialInfoTypeId,
                                                ItemName = cialinfg.Key.CommerciallInfoTypeName,
                                            },
                                            Value = cialinfg.Key.CommercialInfoValue,
                                            LargeValue = cialinfg.Key.CommercialInfoLargeValue,
                                            Enable = cialinfg.Key.CommercialInfoEnable,
                                            LastModify = cialinfg.Key.CommercialInfoLastModify,
                                            CreateDate = cialinfg.Key.CommercialInfoCreateDate,
                                        }).ToList(),
                            }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region HSEQ Module

        public List<Company.Models.Util.GenericItemModel> CertificationModuleInfo(string CompanyPublicId, int CertificationInfoType)
        {
            List<IDbDataParameter> lstparams = new List<IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstparams.Add(DataInstance.CreateTypedParameter("vCertificationInfoType", CertificationInfoType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CPB_GetCertificationByCompany",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = new List<Company.Models.Util.GenericItemModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (
                        from cert in response.DataTableResult.AsEnumerable()
                        where !cert.IsNull("CertificationId")
                        group cert by new
                        {
                            CertificationId = cert.Field<int>("CertificationId"),
                            CertificationTypeId = cert.Field<int>("CertificationTypeId"),
                            CertificationTypeName = cert.Field<string>("CertificationTypeName"),
                            CertificationEnable = cert.Field<UInt64>("CertificationEnable") == 1 ? true : false,
                            CertificationLastModify = cert.Field<DateTime>("CertificationLastModify"),
                            CertificationCreateDate = cert.Field<DateTime>("CertificationCreateDate"),
                        }
                            into certg
                            select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                            {
                                ItemId = certg.Key.CertificationId,
                                ItemType = new Company.Models.Util.CatalogModel()
                                {
                                    ItemId = certg.Key.CertificationTypeId,
                                    ItemName = certg.Key.CertificationTypeName,
                                },
                                Enable = certg.Key.CertificationEnable,
                                LastModify = certg.Key.CertificationLastModify,
                                CreateDate = certg.Key.CertificationCreateDate,
                                ItemInfo =
                                   (from certinf in response.DataTableResult.AsEnumerable()
                                    where !certinf.IsNull("CertificationInfoId") &&
                                        certinf.Field<int>("CertificationId") == certg.Key.CertificationId
                                    group certinf by new
                                    {
                                        CertificationInfoId = certinf.Field<int>("CertificationInfoId"),
                                        CertificationItemInfoTypeId = certinf.Field<int>("CertificationItemInfoTypeId"),
                                        CertificationItemInfoTypeName = certinf.Field<string>("CertificationItemInfoTypeName"),
                                        CertificationInfoLargeValue = certinf.Field<string>("CertificationInfoLargeValue"),
                                        CertificationInfoValue = certinf.Field<string>("CertificationInfoValue"),
                                        CertificationInfoEnable = certinf.Field<UInt64>("CertificationInfoEnable") == 1 ? true : false,
                                        CertificationInfoLastModify = certinf.Field<DateTime>("CertificationInfoLastModify"),
                                        CertificationInfoCreateDate = certinf.Field<DateTime>("CertificationInfoCreateDate"),
                                    }
                                        into certingg
                                        select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                        {
                                            ItemInfoId = certingg.Key.CertificationInfoId,
                                            ItemInfoType = new Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = certingg.Key.CertificationItemInfoTypeId,
                                                ItemName = certingg.Key.CertificationItemInfoTypeName,
                                            },
                                            Value = certingg.Key.CertificationInfoValue,
                                            LargeValue = certingg.Key.CertificationInfoLargeValue,
                                            Enable = certingg.Key.CertificationInfoEnable,
                                            LastModify = certingg.Key.CertificationInfoLastModify,
                                            CreateDate = certingg.Key.CertificationInfoCreateDate,
                                        }).ToList(),
                            }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region Balance Module

        public List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> BalanceModuleInfo(string CompanyPublicId, int BalanceAccount)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vBalanceAccount", BalanceAccount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CPB_GetBalanceByCompany",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from f in response.DataTableResult.AsEnumerable()
                     where !f.IsNull("FinancialId")
                     group f by new
                     {
                         FinancialId = f.Field<int>("FinancialId"),
                         FinancialTypeId = f.Field<int>("FinancialTypeId"),
                         FinancialTypeName = f.Field<string>("FinancialTypeName"),
                         FinancialEnable = f.Field<UInt64>("FinancialEnable") == 1 ? true : false,
                         FinancialLastModify = f.Field<DateTime>("FinancialLastModify"),
                         FinancialCreateDate = f.Field<DateTime>("FinancialCreateDate"),
                     }
                         into fg
                         select new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel()
                         {
                             ItemId = fg.Key.FinancialId,
                             ItemType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = fg.Key.FinancialTypeId,
                                 ItemName = fg.Key.FinancialTypeName,
                             },
                             Enable = fg.Key.FinancialEnable,
                             LastModify = fg.Key.FinancialLastModify,
                             CreateDate = fg.Key.FinancialCreateDate,
                             BalanceSheetInfo =
                                (from bs in response.DataTableResult.AsEnumerable()
                                 where !bs.IsNull("BalanceSheetId") &&
                                        bs.Field<int>("FinancialId") == fg.Key.FinancialId
                                 group bs by new
                                 {
                                     BalanceSheetId = bs.Field<int>("BalanceSheetId"),
                                     AccountId = bs.Field<int>("AccountId"),
                                     AccountName = bs.Field<string>("AccountName"),
                                     BalanceValue = bs.Field<decimal>("BalanceValue"),
                                     BalanceEnable = bs.Field<UInt64>("BalanceEnable") == 1 ? true : false,
                                     BalanceLastModify = bs.Field<DateTime>("BalanceLastModify"),
                                     BalanceCreateDate = bs.Field<DateTime>("BalanceCreateDate"),
                                 }
                                     into bsg
                                     select new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                     {
                                         BalanceSheetId = bsg.Key.BalanceSheetId,
                                         RelatedAccount = new Company.Models.Util.GenericItemModel()
                                         {
                                             ItemId = bsg.Key.AccountId,
                                             ItemName = bsg.Key.AccountName,
                                         },
                                         Value = bsg.Key.BalanceValue,
                                         Enable = bsg.Key.BalanceEnable,
                                         LastModify = bsg.Key.BalanceLastModify,
                                         CreateDate = bsg.Key.BalanceCreateDate,
                                     }).ToList(),
                         }).ToList();
            }

            return oReturn;
        }

        #endregion

        #endregion
    }
}
