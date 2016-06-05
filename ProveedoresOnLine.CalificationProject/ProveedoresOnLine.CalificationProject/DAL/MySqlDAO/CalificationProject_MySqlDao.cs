using ProveedoresOnLine.CalificationProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProveedoresOnLine.CalificationProject.Models.CalificationProject;
namespace ProveedoresOnLine.CalificationProject.DAL.MySqlDAO
{
    internal class CalificationProject_MySqlDao : ICalificationProjectData
    {
        private ADO.Interfaces.IADO DataInstance;

        public CalificationProject_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.CalificationProject.Models.Constants.C_POL_CalificatioProjectConnectionName);
        }

        #region ProjectConfig

        public int CalificationProjectConfigUpsert(int CalificationProjectConfigId, string CompanyPublicId, string CalificationProjectConfigName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", CalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigName", CalificationProjectConfigName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", (Enable == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_CalificationProjectConfig_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });
            return Convert.ToInt32(response.ScalarResult);
        }

        public List<Models.CalificationProject.CalificationProjectConfigModel> CalificationProjectConfig_GetByCompanyId(string CompanyPublicId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstparams.Add(DataInstance.CreateTypedParameter("vEnable", (Enable == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_CalificationProjectConfig_GetByCompanyId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<CalificationProject.Models.CalificationProject.CalificationProjectConfigModel> oReturn = new List<Models.CalificationProject.CalificationProjectConfigModel>();

            if (response.DataTableResult != null & response.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from cpc in response.DataTableResult.AsEnumerable()
                           where !cpc.IsNull("CalificationProjectConfigId")
                           group cpc by new
                           {
                               CalificationProjectConfigId = cpc.Field<int>("CalificationProjectConfigId"),
                               CalificationProjectConfigName = cpc.Field<string>("CalificationProjectConfigName"),
                               Enable = cpc.Field<UInt64>("Enable") == 1 ? true : false,
                               LastModify = cpc.Field<DateTime>("LastModify"),
                               CreateDate = cpc.Field<DateTime>("CreateDate"),
                           }
                               into cpcg
                               select new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                               {
                                   CalificationProjectConfigId = cpcg.Key.CalificationProjectConfigId,
                                   CalificationProjectConfigName = cpcg.Key.CalificationProjectConfigName,
                                   Enable = cpcg.Key.Enable,
                                   Company = new Company.Models.Company.CompanyModel()
                                   {
                                       CompanyPublicId = CompanyPublicId
                                   },
                                   LastModify = cpcg.Key.LastModify,
                                   CreateDate = cpcg.Key.CreateDate,
                               }).ToList();
            }
            return oReturn;
        }

        public List<Models.CalificationProject.CalificationProjectConfigModel> CalificationProjectConfig_GetAll()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_CalificationProjectConfig_GetAll",
                CommandType = CommandType.StoredProcedure
            });

            List<Models.CalificationProject.CalificationProjectConfigModel> oReturn = new List<Models.CalificationProject.CalificationProjectConfigModel>();

            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (
                        from cpm in response.DataTableResult.AsEnumerable()
                        where !cpm.IsNull("CalificationProjectConfigId")
                        group cpm by new
                        {
                            CalificationProjectConfigId = cpm.Field<int>("CalificationProjectConfigId"),
                            CompanyPublicId = cpm.Field<string>("CompanyPublicId"),
                            CalificationProjectConfigName = cpm.Field<string>("CalificationProjectConfigName"),
                            Enable = cpm.Field<UInt64>("Enable") == 1 ? true : false,
                            LastModify = cpm.Field<DateTime>("LastModify"),
                            CreateDate = cpm.Field<DateTime>("CreateDate"),
                            CalificatonProjectConfigItemId = cpm.Field<int>("CalificationProjectConfigItemId"),
                            ConfigItemTypeName = cpm.Field<string>("ConfigItemTypeName"),
                            ConfigItemTypeId = cpm.Field<int>("ConfigItemTypeId"),
                            CalificationProjectConfigItemInfoId = cpm.Field<int>("CalificationProjectConfigItemInfoId"),
                            Question = cpm.Field<int>("Question"),
                            RuleName = cpm.Field<string>("RuleName"),
                            RuleId = cpm.Field<int>("RuleId"),
                            ValueName = cpm.Field<string>("ValueName"),
                            ValueId = cpm.Field<int>("ValueId"),
                            Value = cpm.Field<string>("Value"),
                            Score = cpm.Field<string>("Score"),
                            CalificationProjectConfigValidateId = cpm.Field<int>("CalificationProjectConfigValidateId"),
                            OperatorName = cpm.Field<string>("OperatorName"),
                            OperatorId = cpm.Field<int>("OperatorId"),
                            ValidateValue = cpm.Field<string>("ValidateValue"),
                            ValidateResult = cpm.Field<string>("ValidateResult")
                        }
                            into cpmg
                            select new Models.CalificationProject.CalificationProjectConfigModel()
                            {
                                CalificationProjectConfigId = cpmg.Key.CalificationProjectConfigId,
                                Company = new Company.Models.Company.CompanyModel()
                                {
                                     CompanyPublicId= cpmg.Key.CompanyPublicId
                                },
                                CalificationProjectConfigName = cpmg.Key.CalificationProjectConfigName,
                                Enable = cpmg.Key.Enable,
                                LastModify = cpmg.Key.LastModify,
                                CreateDate = cpmg.Key.CreateDate,
                                ConfigItemModel = new List<Models.CalificationProject.ConfigItemModel>()
                                {
                                     new Models.CalificationProject.ConfigItemModel() 
                                    {
                                        CalificationProjectConfigItemId = cpmg.Key.CalificatonProjectConfigItemId,
                                        CalificationProjectConfigItemName = cpmg.Key.ConfigItemTypeName,
                                        CalificationProjectConfigItemType = new Company.Models.Util.CatalogModel
                                        {
                                            ItemId = cpmg.Key.ConfigItemTypeId
                                        },
                                        CalificationProjectConfigItemInfoModel = new List<Models.CalificationProject.ConfigItemInfoModel>()
                                        {
                                            new Models.CalificationProject.ConfigItemInfoModel()
                                            {
                                                CalificationProjectConfigItemInfoId = cpmg.Key.CalificationProjectConfigItemInfoId,
                                                Question = cpmg.Key.Question,
                                                Rule = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemName = cpmg.Key.RuleName,
                                                    ItemId = cpmg.Key.RuleId
                                                },
                                                ValueType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemName = cpmg.Key.ValueName,
                                                    ItemId = cpmg.Key.ValueId
                                                },
                                                Value = cpmg.Key.Value,
                                                Score = cpmg.Key.Score,


                                            }
                                        }
                                    }

                                },
                                ConfigValidateModel = new List<Models.CalificationProject.ConfigValidateModel>()
                                {
                                    new Models.CalificationProject.ConfigValidateModel()
                                    {
                                        CalificationProjectConfigValidateId = cpmg.Key.CalificationProjectConfigValidateId,
                                        Operator = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemName = cpmg.Key.OperatorName,
                                            ItemId = cpmg.Key.OperatorId
                                        },
                                        Value = cpmg.Key.ValidateValue,
                                        Result = cpmg.Key.ValidateResult
                                    }
                                }

                            }

                    ).ToList();
            }
            return oReturn;
        }

        public Models.CalificationProject.CalificationProjectConfigModel CalificationProjectConfig_GetByCalificationProjectConfigId(int CalificationProjectConfigId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", CalificationProjectConfigId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_CalificationProjectConfig_GetByCalificationProjectConfigId",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            Models.CalificationProject.CalificationProjectConfigModel oReturn = new CalificationProjectConfigModel();

            if (response.DataTableResult != null &&                
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from cpc in response.DataTableResult.AsEnumerable()
                           where !cpc.IsNull("CalificationProjectConfigId")
                           group cpc by new
                           {
                               CalificationProjectConfigId = cpc.Field<int>("CalificationProjectConfigId"),
                               CalificationProjectConfigName = cpc.Field<string>("CalificationProjectConfigName"),
                               CompanyPublicId = cpc.Field<string>("CompanyPublicId"),
                               Enable = cpc.Field<UInt64>("Enable") == 1 ? true : false,
                               LastModify = cpc.Field<DateTime>("LastModify"),
                               CreateDate = cpc.Field<DateTime>("CreateDate"),
                           }
                               into cpcg
                               select new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                               {
                                   CalificationProjectConfigId = cpcg.Key.CalificationProjectConfigId,
                                   CalificationProjectConfigName = cpcg.Key.CalificationProjectConfigName,
                                   Enable = cpcg.Key.Enable,
                                   Company = new Company.Models.Company.CompanyModel()
                                   {
                                       CompanyPublicId = cpcg.Key.CompanyPublicId,
                                   },
                                   LastModify = cpcg.Key.LastModify,
                                   CreateDate = cpcg.Key.CreateDate,
                               }).FirstOrDefault();
            }

            return oReturn;
        }

        #endregion

        #region ConfigItem

        public int CalificationProjectConfigItemUpsert(int CalificationProjectConfigId, int CalificationProjectConfigItemId, string CalificationProjectConfigItemName, int CalificationProjectConfigItemType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", CalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemId", CalificationProjectConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemName", CalificationProjectConfigItemName));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemType", CalificationProjectConfigItemType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_CalificationProjectConfigItem_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<Models.CalificationProject.ConfigItemModel> CalificationProjectConfigItem_GetByCalificationProjectConfigId(int CalificationProjectConfigId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", CalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_CalificationProjectConfigItem_GetByConfigId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<Models.CalificationProject.ConfigItemModel> oReturn = new List<Models.CalificationProject.ConfigItemModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cit in response.DataTableResult.AsEnumerable()
                     where !cit.IsNull("CalificationProjectConfigId")
                     group cit by new
                     {
                         CalificationProjectConfigItemId = cit.Field<int>("CalificationProjectConfigItemId"),
                         CalificationProjectConfigId = cit.Field<int>("CalificationProjectConfigId"),
                         CalificationProjectConfigItemName = cit.Field<string>("CalificationProjectConfigItemName"),
                         CalicationProjecConfigItemTypeId = cit.Field<int>("CalicationProjecConfigItemTypeId"),
                         CalicationProjecConfigItemTypeName = cit.Field<string>("CalicationProjecConfigItemTypeName"),
                         Enable = cit.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = cit.Field<DateTime>("LastModify"),
                         CreateDate = cit.Field<DateTime>("CreateDate"),
                     }
                         into citg
                         select new Models.CalificationProject.ConfigItemModel()
                         {
                             CalificationProjectConfigItemId = citg.Key.CalificationProjectConfigItemId,
                             CalificationProjectConfigItemName = citg.Key.CalificationProjectConfigItemName,
                             CalificationProjectConfigItemType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = citg.Key.CalicationProjecConfigItemTypeId,
                                 ItemName = citg.Key.CalicationProjecConfigItemTypeName,
                             },
                             Enable = citg.Key.Enable,
                             LastModify = citg.Key.LastModify,
                             CreateDate = citg.Key.CreateDate,
                         }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region ConfigItemInfo

        public int CalificationProjectConfigItemInfoUpsert(int CalificationProjectConfigItemId, int CalificationProjectConfigItemInfoId, int Question, int Rule, int ValueType, string Value, string Score, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemId", CalificationProjectConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemInfoId", CalificationProjectConfigItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vQuestion", Question));
            lstParams.Add(DataInstance.CreateTypedParameter("vRule", Rule));
            lstParams.Add(DataInstance.CreateTypedParameter("vValueType", ValueType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vScore", Score));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_CalificationProjectConfigItemInfo_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<Models.CalificationProject.ConfigItemInfoModel> CalificationProjectConfigItemInfo_GetByCalificationProjectConfigItemId(int CalificationProjectConfigItemId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemId", CalificationProjectConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_CalificationProjectConfigItemInfo_GetByProjectConfigItem",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<Models.CalificationProject.ConfigItemInfoModel> oReturn = new List<Models.CalificationProject.ConfigItemInfoModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cinf in response.DataTableResult.AsEnumerable()
                     where !cinf.IsNull("CalificationProjectConfigItemInfoId")
                     group cinf by new
                     {
                         CalificationProjectConfigItemInfoId = cinf.Field<int>("CalificationProjectConfigItemInfoId"),
                         Question = cinf.Field<int>("Question"),
                         RuleId = cinf.Field<int>("RuleId"),
                         RuleName = cinf.Field<string>("RuleName"),
                         ValueTypeId = cinf.Field<int>("ValueTypeId"),
                         ValueTypeName = cinf.Field<string>("ValueTypeName"),
                         Value = cinf.Field<string>("Value"),
                         Score = cinf.Field<string>("Score"),
                         Enable = cinf.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = cinf.Field<DateTime>("LastModify"),
                         CreateDate = cinf.Field<DateTime>("CreateDate"),
                     }
                         into cinfg
                         select new Models.CalificationProject.ConfigItemInfoModel()
                         {
                             CalificationProjectConfigItemInfoId = cinfg.Key.CalificationProjectConfigItemInfoId,
                             Question = cinfg.Key.Question,
                             Rule = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = cinfg.Key.RuleId,
                                 ItemName = cinfg.Key.RuleName,
                             },
                             ValueType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = cinfg.Key.ValueTypeId,
                                 ItemName = cinfg.Key.ValueTypeName,
                             },
                             Value = cinfg.Key.Value,
                             Score = cinfg.Key.Score,
                             Enable = cinfg.Key.Enable,
                             LastModify = cinfg.Key.LastModify,
                             CreateDate = cinfg.Key.CreateDate,
                         }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region ConfigValidate

        public int CalificationProjectConfigValidateUpsert(int CalificationProjectConfigValidateId, int CalificationProjectConfigId, int Operator, string Value, string Result, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigValidateId", CalificationProjectConfigValidateId));
            lstparams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", CalificationProjectConfigId));
            lstparams.Add(DataInstance.CreateTypedParameter("vOperator", Operator));
            lstparams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstparams.Add(DataInstance.CreateTypedParameter("vResult", Result));
            lstparams.Add(DataInstance.CreateTypedParameter("vEnable", (Enable == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_CalificationProjectConfigValidate_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstparams,

            });

            return Convert.ToInt32(response.ScalarResult);
        }
        public List<Models.CalificationProject.ConfigValidateModel> CalificationProjectConfigValidate_GetByProjectConfigId(int CalificationProjectConfigId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", CalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_CalificationProjectConfigValidate_GetByProjectConfigId",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<Models.CalificationProject.ConfigValidateModel> oReturn = new List<Models.CalificationProject.ConfigValidateModel>();

            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from cvm in response.DataTableResult.AsEnumerable()
                           where !cvm.IsNull("CalificationProjectConfigValidateId")
                           group cvm by new
                           {
                               CalificationProjectConfigValidateId = cvm.Field<int>("CalificationProjectConfigValidateId"),
                               CalificationProjectConfigId = cvm.Field<int>("CalificationProjectConfigId"),
                               OperatorId = cvm.Field<int>("OperatorId"),
                               OperatorName = cvm.Field<string>("OperatorName"),
                               Value = cvm.Field<string>("Value"),
                               Result = cvm.Field<string>("Result"),
                               Enable = cvm.Field<UInt64>("Enable") == 1 ? true : false,
                               CreateDate = cvm.Field<DateTime>("CreateDate"),
                               LastModify = cvm.Field<DateTime>("LastModify")
                           }
                               into cvmf
                               select new Models.CalificationProject.ConfigValidateModel()
                               {
                                   CalificationProjectConfigValidateId = cvmf.Key.CalificationProjectConfigValidateId,
                                   CalificationProjectConfigId = cvmf.Key.CalificationProjectConfigId,
                                   Operator = new Company.Models.Util.CatalogModel
                                   {
                                       ItemId = cvmf.Key.OperatorId,
                                       ItemName = cvmf.Key.OperatorName
                                   },
                                   Value = cvmf.Key.Value,
                                   Result = cvmf.Key.Result,
                                   Enable = cvmf.Key.Enable,
                                   CreateDate = cvmf.Key.CreateDate,
                                   LastModify = cvmf.Key.LastModify

                               }).ToList();
            }
            return oReturn;
        }
        #endregion

        #region CalificationProjectconfigOptions

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> CalificationProjectConfigOptions()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Catalog_GetCalificationProjectConfigOptions",
                CommandType = CommandType.StoredProcedure,
            });

            List<ProveedoresOnLine.Company.Models.Util.CatalogModel> oReturn = new List<Company.Models.Util.CatalogModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ci in response.DataTableResult.AsEnumerable()
                     where !ci.IsNull("CatalogId")
                     group ci by new
                     {
                         CatalogId = ci.Field<Int64>("CatalogId"),
                         CatalogName = ci.Field<string>("CatalogName"),
                         CatalogEnable = ci.Field<UInt64>("CatalogEnable") == 1 ? true : false,
                         ItemId = ci.Field<int>("ItemId"),
                         ItemName = ci.Field<string>("ItemName"),
                         ItemEnable = ci.Field<UInt64>("ItemEnable") == 1 ? true : false,
                     }
                         into cig
                         select new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                         {
                             CatalogId = Convert.ToInt32(cig.Key.CatalogId),
                             CatalogName = cig.Key.CatalogName,
                             CatalogEnable = cig.Key.CatalogEnable,
                             ItemId = cig.Key.ItemId,
                             ItemName = cig.Key.ItemName,
                             ItemEnable = cig.Key.ItemEnable,
                         }).ToList();
            }

            return oReturn;
        }

        public List<Models.CalificationProject.CalificationProjectCategoryModel> CalificationProjectConfigCategoryOptions()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Catalog_GetCalificationProjectConfigCategoryOptions",
                CommandType = CommandType.StoredProcedure,
            });

            List<Models.CalificationProject.CalificationProjectCategoryModel> oReturn = new List<Models.CalificationProject.CalificationProjectCategoryModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cm in response.DataTableResult.AsEnumerable()
                     where !cm.IsNull("TreeId")
                     group cm by new
                     {
                         TreeId = cm.Field<int>("TreeId"),
                         TreeName = cm.Field<string>("TreeName"),
                         TreeEnable = cm.Field<UInt64>("TreeEnable") == 1 ? true : false,
                         CategoryId = cm.Field<int>("CategoryId"),
                         CategoryName = cm.Field<string>("CategoryName"),
                         CategoryEnable = cm.Field<UInt64>("CategoryEnable") == 1 ? true : false,
                     }
                         into cmg
                         select new Models.CalificationProject.CalificationProjectCategoryModel()
                         {
                             TreeId = cmg.Key.TreeId,
                             TreeName = cmg.Key.TreeName,
                             TreeEnable = cmg.Key.TreeEnable,
                             CategoryId = cmg.Key.CategoryId,
                             CategoryName = cmg.Key.CategoryName,
                             CategoryEnable = cmg.Key.CategoryEnable,
                         }).ToList();
            }

            return oReturn;
        }

        #endregion
    }
}
