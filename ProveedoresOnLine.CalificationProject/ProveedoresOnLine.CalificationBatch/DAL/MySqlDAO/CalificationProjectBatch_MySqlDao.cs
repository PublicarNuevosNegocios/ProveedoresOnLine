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
                        }
                            into cpbg
                            select new Models.CalificationProjectBatch.CalificationProjectBatchModel()
                                {
                                    //CalificationProjectItemBatch
                                    CalificationProjectId = cpbg.Key.CalificationProjectId,
                                    CalificationProjectPublicId = cpbg.Key.CalificationProjectPublicId,
                                    ProjectConfigModel = new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                                    {
                                        CalificationProjectConfigId = cpbg.Key.CalificationProjectConfigId,
                                    },
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
                                                 CalificationProjectConfigItem = new CalificationProject.Models.CalificationProject.ConfigItemModel()
                                                 {
                                                     CalificationProjectConfigItemId = cpitg.Key.CalificationProjectConfigItemId,
                                                 },
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
                                                          CalificationProjectConfigItemInfoModel = new CalificationProject.Models.CalificationProject.ConfigItemInfoModel()
                                                          {
                                                              CalificationProjectConfigItemInfoId = cpitinfg.Key.CalificationProjectConfigItemInfoId,
                                                          },
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

        public int CalificationProjectUpsert(int vCalificationProjectId, string vCalificatonProjectPublicId, int vCalificationProjectConfigId, string vCompanyPublicId, int vTotalScore, bool vEnable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectId", vCalificationProjectId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificatonProjectPublicId", vCalificatonProjectPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", vCalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", vCompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vTotalScore", vTotalScore));
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
            return Convert.ToInt32(response.ScalarResult);
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
