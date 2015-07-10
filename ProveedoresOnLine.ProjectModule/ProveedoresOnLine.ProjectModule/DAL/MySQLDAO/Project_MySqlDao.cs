using ProveedoresOnLine.ProjectModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProveedoresOnLine.Company.Models.Util;

namespace ProveedoresOnLine.ProjectModule.DAL.MySQLDAO
{
    internal class Project_MySqlDao : ProveedoresOnLine.ProjectModule.Interfaces.IProjectData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Project_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.ProjectModule.Models.Constants.C_POL_ProjectConnectionName);
        }

        #region Project Config

        public int ProjectConfigUpsert(string CustomerPublicId, int? ProjectConfigId, string ProjectName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigId", ProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectName", ProjectName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_ProjectConfig_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int EvaluationItemUpsert(int? EvaluationItemId, int ProjectConfigId, string EvaluationItemName, int EvaluationItemType, int? ParentEvaluationItem, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();


            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigId", ProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemId", EvaluationItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemName", EvaluationItemName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemTypeId", EvaluationItemType));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentEvaluationItemId", ParentEvaluationItem));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_EvaluationItem_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
            throw new NotImplementedException();
        }

        public int EvaluationItemInfoUpsert(int? EvaluationItemInfoId, int EvaluationItemId, int EvaluationItemInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemInfoId", EvaluationItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemId", EvaluationItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemInfoTypeId", EvaluationItemInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_EvaluationItemInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);

        }

        public List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> GetAllProjectConfigByCustomerPublicId(string CustomerPublicId, string SearchParam, bool ViewEnable, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vViewEnable", ViewEnable == true ? 1 : 0));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_ProjectConfig_GetByCustomerProvider",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> oReturn = null;
            TotalRows = 0;


            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from pc in response.DataTableResult.AsEnumerable()
                     where !pc.IsNull("ProjectConfigId")
                     group pc by new
                     {
                         ProjectConfigId = pc.Field<int>("ProjectConfigId"),
                         ProjectConfigName = pc.Field<string>("ProjectConfigName"),
                         CompanyPublicId = pc.Field<string>("CompanyPublicId"),
                         ProjectConfigEnable = pc.Field<UInt64>("ProjectConfigEnable") == 1 ? true : false,
                     } into pcg
                     select new ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel()
                     {
                         ItemId = pcg.Key.ProjectConfigId,
                         ItemName = pcg.Key.ProjectConfigName,
                         Enable = pcg.Key.ProjectConfigEnable,
                         RelatedCustomer = new CompanyCustomer.Models.Customer.CustomerModel()
                         {
                             RelatedCompany = new Company.Models.Company.CompanyModel()
                             {
                                 CompanyPublicId = pcg.Key.CompanyPublicId,
                             },
                         },
                     }).ToList();
            }

            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> GetAllEvaluationItemByProjectConfig(int ProjectConfigId, string SearchParam, int EvaluationItemType, int? ParentEvaluationItem, bool ViewEnable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigId", ProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemType", EvaluationItemType));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentEvaluationItem", ParentEvaluationItem));
            lstParams.Add(DataInstance.CreateTypedParameter("vViewEnable", ViewEnable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_EvaluationItem_GetByProjectConfig",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ei in response.DataTableResult.AsEnumerable()
                     where !ei.IsNull("EvaluationItemId")
                     group ei by new
                    {
                        EvaluationItemId = ei.Field<int>("EvaluationItemId"),
                        EvaluationItemName = ei.Field<string>("EvaluationItemName"),
                        EvaluationItemTypeId = ei.Field<int>("EvaluationItemTypeId"),
                        EvaluationItemTypeName = ei.Field<string>("EvaluationItemTypeName"),
                        ParentEvaluationItem = ei.Field<int?>("ParentEvaluationItem"),
                        EvaluationItemEnable = ei.Field<UInt64>("EvaluationItemEnable") == 1 ? true : false,
                        EvaluationItemLastModify = ei.Field<DateTime>("EvaluationItemLastModify"),
                        EvaluationItemCreateDate = ei.Field<DateTime>("EvaluationItemCreateDate"),
                    } into eig
                     select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                     {
                         ItemId = eig.Key.EvaluationItemId,
                         ItemName = eig.Key.EvaluationItemName,
                         ItemType = new Company.Models.Util.CatalogModel()
                         {
                             ItemId = eig.Key.EvaluationItemTypeId,
                             ItemName = eig.Key.EvaluationItemTypeName,
                         },
                         ParentItem = eig.Key.ParentEvaluationItem == null ? null :
                             new Company.Models.Util.GenericItemModel()
                             {
                                 ItemId = eig.Key.ParentEvaluationItem.Value,
                             },
                         ItemInfo =
                            (from eiinf in response.DataTableResult.AsEnumerable()
                             where !eiinf.IsNull("EvaluationItemInfoId") &&
                                    eiinf.Field<int>("EvaluationItemId") == eig.Key.EvaluationItemId
                             group eiinf by new
                             {
                                 EvaluationItemInfoId = eiinf.Field<int>("EvaluationItemInfoId"),
                                 EvaluationItemInfoTypeId = eiinf.Field<int>("EvaluationItemInfoTypeId"),
                                 EvaluationItemInfoTypeName = eiinf.Field<string>("EvaluationItemInfoTypeName"),
                                 EvaluationItemInfoValue = eiinf.Field<string>("EvaluationItemInfoValue"),
                                 EvaluationItemInfoLargeValue = eiinf.Field<string>("EvaluationItemInfoLargeValue"),
                                 EvaluationItemInfoEnable = eiinf.Field<UInt64>("EvaluationItemInfoEnable") == 1 ? true : false,
                                 EvaluationItemInfoCreateDate = eiinf.Field<DateTime>("EvaluationItemInfoCreateDate"),
                                 EvaluationItemInfoLastModify = eiinf.Field<DateTime>("EvaluationItemInfoLastModify"),
                             }
                                 into eiinfg
                                 select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                 {
                                     ItemInfoId = eiinfg.Key.EvaluationItemInfoId,
                                     ItemInfoType = new Company.Models.Util.CatalogModel()
                                     {
                                         ItemId = eiinfg.Key.EvaluationItemInfoTypeId,
                                         ItemName = eiinfg.Key.EvaluationItemInfoTypeName,
                                     },
                                     Value = eiinfg.Key.EvaluationItemInfoValue,
                                     LargeValue = eiinfg.Key.EvaluationItemInfoLargeValue,
                                     Enable = eiinfg.Key.EvaluationItemInfoEnable,
                                     CreateDate = eiinfg.Key.EvaluationItemInfoCreateDate,
                                     LastModify = eiinfg.Key.EvaluationItemInfoLastModify,
                                 }).ToList(),
                         Enable = eig.Key.EvaluationItemEnable,
                         CreateDate = eig.Key.EvaluationItemCreateDate,
                         LastModify = eig.Key.EvaluationItemLastModify,
                     }).ToList();
            }

            return oReturn;
        }

        public List<ProjectConfigModel> MPProjectConfigGetByCustomer(string CustomerPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CC_ProjectConfig_GetByCustomer",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProjectConfigModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from pjc in response.DataTableResult.AsEnumerable()
                     select new ProjectConfigModel()
                     {
                         ItemId = pjc.Field<int>("ProjectConfigId"),
                         ItemName = pjc.Field<string>("ProjectConfigName"),
                         LastModify = pjc.Field<DateTime>("LastModify"),
                     }).ToList();
            }

            return oReturn;
        }

        public ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel ProjectConfigGetById(int ProjectConfigId, bool ViewEnable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigId", ProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vViewEnable", ViewEnable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_ProjectConfig_GetById",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel oReturn = new ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from pc in response.DataTableResult.AsEnumerable()
                     where !pc.IsNull("ProjectConfigId")
                     group pc by new
                     {
                         ProjectConfigId = pc.Field<int>("ProjectConfigId"),
                         ProjectConfigName = pc.Field<string>("ProjectConfigName"),
                         ProjectConfigEnable = pc.Field<UInt64>("ProjectConfigEnable") == 1 ? true : false,
                     }
                         into pcg
                         select new ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel()
                         {
                             ItemId = pcg.Key.ProjectConfigId,
                             ItemName = pcg.Key.ProjectConfigName,
                             Enable = pcg.Key.ProjectConfigEnable,
                             RelatedEvaluationItem =
                             (from ei in response.DataTableResult.AsEnumerable()
                              where !ei.IsNull("EvaluationItemId") &&
                                    ei.Field<int>("ProjectConfigId") == pcg.Key.ProjectConfigId
                              group ei by new
                              {
                                  EvaluationItemId = ei.Field<int>("EvaluationItemId"),
                                  EvaluationItemName = ei.Field<string>("EvaluationItemName"),
                                  EvaluationItemTypeId = ei.Field<int>("EvaluationItemTypeId"),
                                  EvaluationItemTypeName = ei.Field<string>("EvaluationItemTypeName"),
                                  ParentEvaluationItem = ei.Field<int?>("ParentEvaluationItem"),
                                  EvaluationItemEnable = ei.Field<UInt64>("EvaluationItemEnable") == 1 ? true : false,
                              }
                                  into eig
                                  select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                  {
                                      ItemId = eig.Key.EvaluationItemId,
                                      ItemName = eig.Key.EvaluationItemName,
                                      ItemType = new CatalogModel()
                                      {
                                          ItemId = eig.Key.EvaluationItemTypeId,
                                          ItemName = eig.Key.EvaluationItemTypeName,
                                      },
                                      ParentItem = eig.Key.ParentEvaluationItem == null ? null : new GenericItemModel()
                                      {
                                          ItemId = (int)eig.Key.ParentEvaluationItem,
                                      },
                                      Enable = eig.Key.EvaluationItemEnable,
                                      ItemInfo =
                                      (from inf in response.DataTableResult.AsEnumerable()
                                       where !inf.IsNull("EvaluationItemInfoId") &&
                                             inf.Field<int>("EvaluationItemId") == eig.Key.EvaluationItemId
                                       group inf by new
                                       {
                                           EvaluationItemInfoId = inf.Field<int>("EvaluationItemInfoId"),
                                           EvaluationItemInfoTypeId = inf.Field<int>("EvaluationItemInfoTypeId"),
                                           EvaluationItemInfoTypeName = inf.Field<string>("EvaluationItemInfoTypeName"),
                                           Value = inf.Field<string>("Value"),
                                           LargeValue = inf.Field<string>("LargeValue"),
                                           EvaluationItemInfoEnable = inf.Field<UInt64>("EvaluationItemInfoEnable") == 1 ? true : false,
                                       }
                                           into infg
                                           select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                           {
                                               ItemInfoId = infg.Key.EvaluationItemInfoId,
                                               ItemInfoType = new CatalogModel()
                                               {
                                                   ItemId = infg.Key.EvaluationItemInfoTypeId,
                                                   ItemName = infg.Key.EvaluationItemInfoTypeName,
                                               },
                                               Value = infg.Key.Value,
                                               LargeValue = infg.Key.LargeValue,
                                           }).ToList(),
                                  }).ToList(),
                         }).FirstOrDefault();
            }

            return oReturn;
        }

        #endregion

        #region Project

        public string ProjectUpsert(string ProjectPublicId, string ProjectName, int ProjectConfigId, int ProjectStatus, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectPublicId", ProjectPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectName", ProjectName));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigId", ProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectStatus", ProjectStatus));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CC_Project_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
        }

        public int ProjectInfoUpsert(int? ProjectInfoId, string ProjectPublicId, int ProjectInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectInfoId", ProjectInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectPublicId", ProjectPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectInfoType", ProjectInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CC_ProjectInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int ProjectCompanyUpsert(string ProjectPublicId, string CompanyPublicId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectPublicId", ProjectPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CP_ProjectCompany_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int ProjectCompanyInfoUpsert(int? ProjectCompanyInfoId, int ProjectCompanyId, int? EvaluationItemId, int ProjectCompanyInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectCompanyInfoId", ProjectCompanyInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectCompanyId", ProjectCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemId", EvaluationItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectCompanyInfoType", ProjectCompanyInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CP_ProjectCompanyInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }


        public List<ProjectModel> ProjectSearch(string CustomerPublicId, string SearchParam, int? ProjectStatus, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectStatus", ProjectStatus));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CC_Project_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProjectModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from pj in response.DataTableResult.AsEnumerable()
                     where !pj.IsNull("ProjectPublicId")
                     group pj by new
                     {
                         ProjectPublicId = pj.Field<string>("ProjectPublicId"),
                         ProjectName = pj.Field<string>("ProjectName"),
                         ProjectStatusId = pj.Field<int>("ProjectStatusId"),
                         ProjectStatusName = pj.Field<string>("ProjectStatusName"),
                         ProjectLastModify = pj.Field<DateTime>("ProjectLastModify"),

                         ProjectConfigId = pj.Field<int>("ProjectConfigId"),
                         ProjectConfigName = pj.Field<string>("ProjectConfigName"),
                         CustomerPublicId = pj.Field<string>("CustomerPublicId"),
                     } into pjg
                     select new ProjectModel()
                     {
                         ProjectPublicId = pjg.Key.ProjectPublicId,
                         ProjectName = pjg.Key.ProjectName,
                         ProjectStatus = new Company.Models.Util.CatalogModel()
                         {
                             ItemId = pjg.Key.ProjectStatusId,
                             ItemName = pjg.Key.ProjectStatusName,
                         },
                         LastModify = pjg.Key.ProjectLastModify,

                         RelatedProjectConfig = new ProjectConfigModel()
                         {
                             ItemId = pjg.Key.ProjectConfigId,
                             ItemName = pjg.Key.ProjectConfigName,
                             RelatedCustomer = new CompanyCustomer.Models.Customer.CustomerModel()
                             {
                                 RelatedCompany = new Company.Models.Company.CompanyModel()
                                 {
                                     CompanyPublicId = pjg.Key.CustomerPublicId,
                                 }
                             }
                         },

                         ProjectInfo =
                            (from pjinf in response.DataTableResult.AsEnumerable()
                             where !pjinf.IsNull("ProjectInfoId") &&
                                    pjinf.Field<string>("ProjectPublicId") == pjg.Key.ProjectPublicId
                             group pjinf by new
                             {
                                 ProjectInfoId = pjinf.Field<int>("ProjectInfoId"),
                                 ProjectInfoTypeId = pjinf.Field<int>("ProjectInfoTypeId"),
                                 ProjectInfoTypeName = pjinf.Field<string>("ProjectInfoTypeName"),
                                 ProjectInfoValue = pjinf.Field<string>("ProjectInfoValue"),
                                 ProjectInfoLargeValue = pjinf.Field<string>("ProjectInfoLargeValue"),
                             } into pjinfg
                             select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                             {
                                 ItemInfoId = pjinfg.Key.ProjectInfoId,
                                 ItemInfoType = new Company.Models.Util.CatalogModel()
                                 {
                                     ItemId = pjinfg.Key.ProjectInfoTypeId,
                                     ItemName = pjinfg.Key.ProjectInfoTypeName,
                                 },
                                 Value = pjinfg.Key.ProjectInfoValue,
                                 LargeValue = pjinfg.Key.ProjectInfoLargeValue,
                             }).ToList(),
                     }).ToList();
            }

            return oReturn;
        }

        public ProjectModel ProjectGetById(string ProjectPublicId, string CustomerPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectPublicId", ProjectPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "MP_CC_Project_GetById",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            ProjectModel oReturn = null;

            if (response.DataSetResult != null &&
                response.DataSetResult.Tables.Count > 3 &&
                response.DataSetResult.Tables[0] != null &&
                response.DataSetResult.Tables[0].Rows.Count > 0 &&
                response.DataSetResult.Tables[1] != null &&
                response.DataSetResult.Tables[2] != null &&
                response.DataSetResult.Tables[3] != null)
            {
                oReturn = new ProjectModel()
                {
                    ProjectPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId"),
                    ProjectName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectName"),
                    ProjectStatus = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = (int)response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectStatusId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectStatusName"),
                    },
                    LastModify = response.DataSetResult.Tables[0].Rows[0].Field<DateTime>("ProjectLastModify"),

                    #region Project Info

                    ProjectInfo =
                        (from pjinf in response.DataSetResult.Tables[0].AsEnumerable()
                         where !pjinf.IsNull("ProjectInfoId") &&
                                pjinf.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group pjinf by new
                         {
                             ProjectInfoId = pjinf.Field<int>("ProjectInfoId"),
                             ProjectInfoTypeId = pjinf.Field<int>("ProjectInfoTypeId"),
                             ProjectInfoTypeName = pjinf.Field<string>("ProjectInfoTypeName"),
                             ProjectInfoValue = pjinf.Field<string>("ProjectInfoValue"),
                             ProjectInfoLargeValue = pjinf.Field<string>("ProjectInfoLargeValue"),
                         } into pjinfg
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                         {
                             ItemInfoId = pjinfg.Key.ProjectInfoId,
                             ItemInfoType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = pjinfg.Key.ProjectInfoTypeId,
                                 ItemName = pjinfg.Key.ProjectInfoTypeName,
                             },
                             Value = pjinfg.Key.ProjectInfoValue,
                             LargeValue = pjinfg.Key.ProjectInfoLargeValue,
                             ValueName = pjinfg.Key.ProjectInfoTypeId == 1407005 || pjinfg.Key.ProjectInfoTypeId == 1407006 ?
                                string.Join(";",
                                (from pjinfv in response.DataSetResult.Tables[0].AsEnumerable()
                                 where !pjinfv.IsNull("ProjectInfoValueName") &&
                                        pjinfv.Field<int>("ProjectInfoId") == pjinfg.Key.ProjectInfoId
                                 select pjinfv.Field<string>("ProjectInfoValueName"))) : string.Empty,
                         }).ToList(),

                    #endregion

                    #region Project Config

                    RelatedProjectConfig = new ProjectConfigModel()
                    {
                        ItemId = (int)response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectConfigId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectConfigName"),
                        RelatedCustomer = new CompanyCustomer.Models.Customer.CustomerModel()
                        {
                            RelatedCompany = new Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("CustomerPublicId"),
                            },
                        },

                        RelatedEvaluationItem =
                            (from pjei in response.DataSetResult.Tables[2].AsEnumerable()
                             where !pjei.IsNull("EvaluationItemId") &&
                                    pjei.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                    pjei.Field<Int64>("ProjectConfigId") == response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectConfigId")
                             group pjei by new
                             {
                                 EvaluationItemId = pjei.Field<int>("EvaluationItemId"),
                                 EvaluationItemName = pjei.Field<string>("EvaluationItemName"),
                                 EvaluationItemTypeId = pjei.Field<int>("EvaluationItemTypeId"),
                                 EvaluationItemTypeName = pjei.Field<string>("EvaluationItemTypeName"),
                                 ParentEvaluationItem = pjei.Field<int?>("ParentEvaluationItem"),
                             } into pjeig
                             select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                             {
                                 ItemId = pjeig.Key.EvaluationItemId,
                                 ItemName = pjeig.Key.EvaluationItemName,
                                 ItemType = new Company.Models.Util.CatalogModel()
                                 {
                                     ItemId = pjeig.Key.EvaluationItemTypeId,
                                     ItemName = pjeig.Key.EvaluationItemTypeName
                                 },
                                 ParentItem = pjeig.Key.ParentEvaluationItem == null ? null :
                                    new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = pjeig.Key.ParentEvaluationItem.Value,
                                    },

                                 ItemInfo =
                                     (from pjeiinf in response.DataSetResult.Tables[2].AsEnumerable()
                                      where !pjeiinf.IsNull("EvaluationItemInfoId") &&
                                             pjeiinf.Field<int>("EvaluationItemId") == pjeig.Key.EvaluationItemId
                                      group pjeiinf by new
                                      {
                                          EvaluationItemInfoId = pjeiinf.Field<int>("EvaluationItemInfoId"),
                                          EvaluationItemInfoTypeId = pjeiinf.Field<int>("EvaluationItemInfoTypeId"),
                                          EvaluationItemInfoTypeName = pjeiinf.Field<string>("EvaluationItemInfoTypeName"),
                                          EvaluationItemInfoValue = pjeiinf.Field<string>("EvaluationItemInfoValue"),
                                          EvaluationItemInfoLargeValue = pjeiinf.Field<string>("EvaluationItemInfoLargeValue"),
                                      } into pjeiinfg
                                      select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                      {
                                          ItemInfoId = pjeiinfg.Key.EvaluationItemInfoId,
                                          ItemInfoType = new Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = pjeiinfg.Key.EvaluationItemInfoTypeId,
                                              ItemName = pjeiinfg.Key.EvaluationItemInfoTypeName,
                                          },
                                          Value = pjeiinfg.Key.EvaluationItemInfoValue,
                                          LargeValue = pjeiinfg.Key.EvaluationItemInfoLargeValue,
                                      }).ToList(),
                             }).ToList(),


                    },

                    #endregion

                    #region Project Provider

                    RelatedProjectProvider =
                        (from rpp in response.DataSetResult.Tables[1].AsEnumerable()
                         where !rpp.IsNull("ProjectCompanyId") &&
                                 rpp.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group rpp by new
                         {
                             ProjectCompanyId = rpp.Field<int>("ProjectCompanyId"),
                             ProjectCompanyLastModify = rpp.Field<DateTime>("ProjectCompanyLastModify"),
                         } into rppg
                         select new ProjectProviderModel()
                         {
                             ProjectCompanyId = rppg.Key.ProjectCompanyId,
                             LastModify = rppg.Key.ProjectCompanyLastModify,
                             ItemInfo =
                              (from rppinf in response.DataSetResult.Tables[3].AsEnumerable()
                               where !rppinf.IsNull("ProjectCompanyInfoId") &&
                                       rppinf.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                       rppinf.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId
                               group rppinf by new
                               {
                                   ProjectCompanyInfoId = rppinf.Field<int>("ProjectCompanyInfoId"),
                                   EvaluationItemId = rppinf.Field<int?>("EvaluationItemId"),
                                   ProjectCompanyInfoTypeId = rppinf.Field<int>("ProjectCompanyInfoTypeId"),
                                   ProjectCompanyInfoTypeName = rppinf.Field<string>("ProjectCompanyInfoTypeName"),
                                   ProjectCompanyInfoValue = rppinf.Field<string>("ProjectCompanyInfoValue"),
                                   ProjectCompanyInfoLargeValue = rppinf.Field<string>("ProjectCompanyInfoLargeValue"),
                               } into rppinfg
                               select new ProjectProviderInfoModel()
                               {
                                   ItemInfoId = rppinfg.Key.ProjectCompanyInfoId,
                                   RelatedEvaluationItem = rppinfg.Key.EvaluationItemId == null ? null :
                                        new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = rppinfg.Key.EvaluationItemId.Value,
                                        },
                                   ItemInfoType = new Company.Models.Util.CatalogModel()
                                   {
                                       ItemId = rppinfg.Key.ProjectCompanyInfoTypeId,
                                       ItemName = rppinfg.Key.ProjectCompanyInfoTypeName,
                                   },
                                   Value = rppinfg.Key.ProjectCompanyInfoValue,
                                   LargeValue = rppinfg.Key.ProjectCompanyInfoLargeValue,
                               }).ToList(),

                             RelatedProvider =
                                 (from prv in response.DataSetResult.Tables[1].AsEnumerable()
                                  where !prv.IsNull("ProviderPublicId") &&
                                         prv.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId
                                  group prv by new
                                  {
                                      ProviderPublicId = prv.Field<string>("ProviderPublicId"),
                                      CompanyName = prv.Field<string>("CompanyName"),
                                      IdentificationTypeId = prv.Field<int>("IdentificationTypeId"),
                                      IdentificationTypeName = prv.Field<string>("IdentificationTypeName"),
                                      IdentificationNumber = prv.Field<string>("IdentificationNumber"),
                                      CompanyTypeId = prv.Field<int>("CompanyTypeId"),
                                      CompanyTypeName = prv.Field<string>("CompanyTypeName"),
                                  } into prvg
                                  select new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                                  {
                                      RelatedCompany = new Company.Models.Company.CompanyModel()
                                      {
                                          CompanyPublicId = prvg.Key.ProviderPublicId,
                                          CompanyName = prvg.Key.CompanyName,
                                          IdentificationType = new Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = prvg.Key.CompanyTypeId,
                                              ItemName = prvg.Key.CompanyTypeName,
                                          },
                                          IdentificationNumber = prvg.Key.IdentificationNumber,
                                          CompanyType = new Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = prvg.Key.CompanyTypeId,
                                              ItemName = prvg.Key.CompanyTypeName,
                                          },

                                          CompanyInfo =
                                            (from prvinf in response.DataSetResult.Tables[1].AsEnumerable()
                                             where !prvinf.IsNull("CompanyInfoId") &&
                                                    prvinf.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                                    prvinf.Field<string>("ProviderPublicId") == prvg.Key.ProviderPublicId
                                             group prvinf by new
                                             {
                                                 CompanyInfoId = prvinf.Field<int>("CompanyInfoId"),
                                                 CompanyInfoTypeId = prvinf.Field<int>("CompanyInfoTypeId"),
                                                 CompanyInfoTypeName = prvinf.Field<string>("CompanyInfoTypeName"),
                                                 CompanyInfoValue = prvinf.Field<string>("CompanyInfoValue"),
                                                 CompanyInfoLargeValue = prvinf.Field<string>("CompanyInfoLargeValue"),
                                             } into prvinfg
                                             select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                             {
                                                 ItemInfoId = prvinfg.Key.CompanyInfoId,
                                                 ItemInfoType = new Company.Models.Util.CatalogModel()
                                                 {
                                                     ItemId = prvinfg.Key.CompanyInfoTypeId,
                                                     ItemName = prvinfg.Key.CompanyInfoTypeName,
                                                 },
                                                 Value = prvinfg.Key.CompanyInfoValue,
                                                 LargeValue = prvinfg.Key.CompanyInfoLargeValue,
                                             }).ToList(),
                                      },
                                  }).FirstOrDefault(),
                         }).ToList(),

                    #endregion
                };
            }

            return oReturn;
        }

        public ProjectModel ProjectGetByIdLite(string ProjectPublicId, string CustomerPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectPublicId", ProjectPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "MP_CC_Project_GetById",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            ProjectModel oReturn = null;

            if (response.DataSetResult != null &&
                response.DataSetResult.Tables.Count > 1 &&
                response.DataSetResult.Tables[0] != null &&
                response.DataSetResult.Tables[0].Rows.Count > 0 &&
                response.DataSetResult.Tables[1] != null)
            {
                oReturn = new ProjectModel()
                {
                    ProjectPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId"),
                    ProjectName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectName"),
                    ProjectStatus = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = (int)response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectStatusId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectStatusName"),
                    },
                    LastModify = response.DataSetResult.Tables[0].Rows[0].Field<DateTime>("ProjectLastModify"),

                    ProjectInfo =
                        (from pjinf in response.DataSetResult.Tables[0].AsEnumerable()
                         where !pjinf.IsNull("ProjectInfoId") &&
                                pjinf.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group pjinf by new
                         {
                             ProjectInfoId = pjinf.Field<int>("ProjectInfoId"),
                             ProjectInfoTypeId = pjinf.Field<int>("ProjectInfoTypeId"),
                             ProjectInfoTypeName = pjinf.Field<string>("ProjectInfoTypeName"),
                             ProjectInfoValue = pjinf.Field<string>("ProjectInfoValue"),
                             ProjectInfoLargeValue = pjinf.Field<string>("ProjectInfoLargeValue"),
                         } into pjinfg
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                         {
                             ItemInfoId = pjinfg.Key.ProjectInfoId,
                             ItemInfoType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = pjinfg.Key.ProjectInfoTypeId,
                                 ItemName = pjinfg.Key.ProjectInfoTypeName,
                             },
                             Value = pjinfg.Key.ProjectInfoValue,
                             LargeValue = pjinfg.Key.ProjectInfoLargeValue,
                         }).ToList(),

                    RelatedProjectConfig = new ProjectConfigModel()
                    {
                        ItemId = (int)response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectConfigId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectConfigName"),
                        RelatedCustomer = new CompanyCustomer.Models.Customer.CustomerModel()
                        {
                            RelatedCompany = new Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("CustomerPublicId"),
                            },
                        },
                    },

                    RelatedProjectProvider =
                        (from rpp in response.DataSetResult.Tables[1].AsEnumerable()
                         where !rpp.IsNull("ProjectCompanyId") &&
                                 rpp.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group rpp by new
                         {
                             ProjectCompanyId = rpp.Field<int>("ProjectCompanyId"),
                             ProjectCompanyLastModify = rpp.Field<DateTime>("ProjectCompanyLastModify"),
                         } into rppg
                         select new ProjectProviderModel()
                         {
                             ProjectCompanyId = rppg.Key.ProjectCompanyId,
                             LastModify = rppg.Key.ProjectCompanyLastModify,

                             RelatedProvider =
                                 (from prv in response.DataSetResult.Tables[1].AsEnumerable()
                                  where !prv.IsNull("ProviderPublicId") &&
                                         prv.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId
                                  group prv by new
                                  {
                                      ProviderPublicId = prv.Field<string>("ProviderPublicId"),
                                      CompanyName = prv.Field<string>("CompanyName"),
                                      IdentificationTypeId = prv.Field<int>("IdentificationTypeId"),
                                      IdentificationTypeName = prv.Field<string>("IdentificationTypeName"),
                                      IdentificationNumber = prv.Field<string>("IdentificationNumber"),
                                      CompanyTypeId = prv.Field<int>("CompanyTypeId"),
                                      CompanyTypeName = prv.Field<string>("CompanyTypeName"),
                                  } into prvg
                                  select new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                                  {
                                      RelatedCompany = new Company.Models.Company.CompanyModel()
                                      {
                                          CompanyPublicId = prvg.Key.ProviderPublicId,
                                          CompanyName = prvg.Key.CompanyName,
                                          IdentificationType = new Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = prvg.Key.CompanyTypeId,
                                              ItemName = prvg.Key.CompanyTypeName,
                                          },
                                          IdentificationNumber = prvg.Key.IdentificationNumber,
                                          CompanyType = new Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = prvg.Key.CompanyTypeId,
                                              ItemName = prvg.Key.CompanyTypeName,
                                          },

                                          CompanyInfo =
                                            (from prvinf in response.DataSetResult.Tables[1].AsEnumerable()
                                             where !prvinf.IsNull("CompanyInfoId") &&
                                                    prvinf.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                                    prvinf.Field<string>("ProviderPublicId") == prvg.Key.ProviderPublicId
                                             group prvinf by new
                                             {
                                                 CompanyInfoId = prvinf.Field<int>("CompanyInfoId"),
                                                 CompanyInfoTypeId = prvinf.Field<int>("CompanyInfoTypeId"),
                                                 CompanyInfoTypeName = prvinf.Field<string>("CompanyInfoTypeName"),
                                                 CompanyInfoValue = prvinf.Field<string>("CompanyInfoValue"),
                                                 CompanyInfoLargeValue = prvinf.Field<string>("CompanyInfoLargeValue"),
                                             } into prvinfg
                                             select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                             {
                                                 ItemInfoId = prvinfg.Key.CompanyInfoId,
                                                 ItemInfoType = new Company.Models.Util.CatalogModel()
                                                 {
                                                     ItemId = prvinfg.Key.CompanyInfoTypeId,
                                                     ItemName = prvinfg.Key.CompanyInfoTypeName,
                                                 },
                                                 Value = prvinfg.Key.CompanyInfoValue,
                                                 LargeValue = prvinfg.Key.CompanyInfoLargeValue,
                                             }).ToList(),
                                      },
                                  }).FirstOrDefault(),
                         }).ToList(),
                };
            }

            return oReturn;
        }

        public ProjectModel ProjectGetByIdCalculate(string ProjectPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectPublicId", ProjectPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "MP_CC_Project_GetById_Calculate",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            ProjectModel oReturn = null;

            if (response.DataSetResult != null &&
                response.DataSetResult.Tables.Count > 7 &&
                response.DataSetResult.Tables[0] != null &&
                response.DataSetResult.Tables[0].Rows.Count > 0 &&
                response.DataSetResult.Tables[1] != null &&
                response.DataSetResult.Tables[2] != null &&
                response.DataSetResult.Tables[3] != null &&
                response.DataSetResult.Tables[4] != null &&
                response.DataSetResult.Tables[5] != null &&
                response.DataSetResult.Tables[6] != null &&
                response.DataSetResult.Tables[7] != null)
            {
                oReturn = new ProjectModel()
                {
                    ProjectPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId"),
                    ProjectStatus = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = response.DataSetResult.Tables[0].Rows[0].Field<int>("ProjectStatusId"),
                    },

                    #region ProjectInfo

                    ProjectInfo =
                        (from pjinf in response.DataSetResult.Tables[0].AsEnumerable()
                         where !pjinf.IsNull("ProjectInfoId") &&
                                pjinf.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group pjinf by new
                         {
                             ProjectInfoId = pjinf.Field<int>("ProjectInfoId"),
                             ProjectInfoTypeId = pjinf.Field<int>("ProjectInfoTypeId"),
                             ProjectInfoValue = pjinf.Field<string>("ProjectInfoValue"),
                             ProjectInfoLargeValue = pjinf.Field<string>("ProjectInfoLargeValue"),
                         } into pjinfg
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                         {
                             ItemInfoId = pjinfg.Key.ProjectInfoId,
                             ItemInfoType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = pjinfg.Key.ProjectInfoTypeId,
                             },
                             Value = pjinfg.Key.ProjectInfoValue,
                             LargeValue = pjinfg.Key.ProjectInfoLargeValue,
                         }).ToList(),

                    #endregion

                    #region Project Config

                    RelatedProjectConfig = new ProjectConfigModel()
                    {
                        ItemId = (int)response.DataSetResult.Tables[1].Rows[0].Field<Int64>("ProjectConfigId"),

                        RelatedEvaluationItem =
                            (from pjei in response.DataSetResult.Tables[1].AsEnumerable()
                             where !pjei.IsNull("EvaluationItemId") &&
                                    pjei.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                    pjei.Field<Int64>("ProjectConfigId") == response.DataSetResult.Tables[1].Rows[0].Field<Int64>("ProjectConfigId")
                             group pjei by new
                             {
                                 EvaluationItemId = pjei.Field<int>("EvaluationItemId"),
                                 EvaluationItemTypeId = pjei.Field<int>("EvaluationItemTypeId"),
                                 ParentEvaluationItem = pjei.Field<int?>("ParentEvaluationItem"),
                             } into pjeig
                             select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                             {
                                 ItemId = pjeig.Key.EvaluationItemId,
                                 ItemType = new Company.Models.Util.CatalogModel()
                                 {
                                     ItemId = pjeig.Key.EvaluationItemTypeId,
                                 },
                                 ParentItem = pjeig.Key.ParentEvaluationItem == null ? null :
                                    new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = pjeig.Key.ParentEvaluationItem.Value,
                                    },

                                 ItemInfo =
                                     (from pjeiinf in response.DataSetResult.Tables[1].AsEnumerable()
                                      where !pjeiinf.IsNull("EvaluationItemInfoId") &&
                                             pjeiinf.Field<int>("EvaluationItemId") == pjeig.Key.EvaluationItemId
                                      group pjeiinf by new
                                      {
                                          EvaluationItemInfoId = pjeiinf.Field<int>("EvaluationItemInfoId"),
                                          EvaluationItemInfoTypeId = pjeiinf.Field<int>("EvaluationItemInfoTypeId"),
                                          EvaluationItemInfoValue = pjeiinf.Field<string>("EvaluationItemInfoValue"),
                                          EvaluationItemInfoLargeValue = pjeiinf.Field<string>("EvaluationItemInfoLargeValue"),
                                      } into pjeiinfg
                                      select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                      {
                                          ItemInfoId = pjeiinfg.Key.EvaluationItemInfoId,
                                          ItemInfoType = new Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = pjeiinfg.Key.EvaluationItemInfoTypeId,
                                          },
                                          Value = pjeiinfg.Key.EvaluationItemInfoValue,
                                          LargeValue = pjeiinfg.Key.EvaluationItemInfoLargeValue,
                                      }).ToList(),
                             }).ToList(),


                    },

                    #endregion

                    #region Project Provider

                    RelatedProjectProvider =
                        (from rpp in response.DataSetResult.Tables[2].AsEnumerable()
                         where !rpp.IsNull("ProjectCompanyId") &&
                                 rpp.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group rpp by new
                         {
                             ProjectCompanyId = rpp.Field<int>("ProjectCompanyId"),
                             ProviderPublicId = rpp.Field<string>("ProviderPublicId"),
                         } into rppg
                         select new ProjectProviderModel()
                         {
                             ProjectCompanyId = rppg.Key.ProjectCompanyId,
                             ItemInfo =
                              (from rppinf in response.DataSetResult.Tables[2].AsEnumerable()
                               where !rppinf.IsNull("ProjectCompanyInfoId") &&
                                       rppinf.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                       rppinf.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                               group rppinf by new
                               {
                                   ProjectCompanyInfoId = rppinf.Field<int>("ProjectCompanyInfoId"),
                                   EvaluationItemId = rppinf.Field<int?>("EvaluationItemId"),
                                   ProjectCompanyInfoTypeId = rppinf.Field<int>("ProjectCompanyInfoTypeId"),
                                   ProjectCompanyInfoValue = rppinf.Field<string>("ProjectCompanyInfoValue"),
                                   ProjectCompanyInfoLargeValue = rppinf.Field<string>("ProjectCompanyInfoLargeValue"),
                               } into rppinfg
                               select new ProjectProviderInfoModel()
                               {
                                   ItemInfoId = rppinfg.Key.ProjectCompanyInfoId,
                                   RelatedEvaluationItem = rppinfg.Key.EvaluationItemId == null ? null :
                                        new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = rppinfg.Key.EvaluationItemId.Value,
                                        },
                                   ItemInfoType = new Company.Models.Util.CatalogModel()
                                   {
                                       ItemId = rppinfg.Key.ProjectCompanyInfoTypeId,
                                   },
                                   Value = rppinfg.Key.ProjectCompanyInfoValue,
                                   LargeValue = rppinfg.Key.ProjectCompanyInfoLargeValue,
                               }).ToList(),

                             #region ProviderInfo

                             RelatedProvider = new CompanyProvider.Models.Provider.ProviderModel()
                             {
                                 RelatedCompany = new Company.Models.Company.CompanyModel()
                                 {
                                     CompanyPublicId = rppg.Key.ProviderPublicId,
                                 },

                                 #region Commercial

                                 RelatedCommercial =
                                   (from prvcm in response.DataSetResult.Tables[3].AsEnumerable()
                                    where !prvcm.IsNull("CommercialId") &&
                                           prvcm.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvcm.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                                    group prvcm by new
                                    {
                                        CommercialId = prvcm.Field<int>("CommercialId"),
                                        CommercialType = prvcm.Field<int>("CommercialType"),
                                    } into prvcmg
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvcmg.Key.CommercialId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvcmg.Key.CommercialType,
                                        },
                                        ItemInfo =
                                           (from prvcminf in response.DataSetResult.Tables[3].AsEnumerable()
                                            where !prvcminf.IsNull("CommercialInfoId") &&
                                                   prvcminf.Field<int>("CommercialId") == prvcmg.Key.CommercialId
                                            group prvcminf by new
                                            {
                                                CommercialInfoId = prvcminf.Field<int>("CommercialInfoId"),
                                                CommercialInfoType = prvcminf.Field<int>("CommercialInfoType"),
                                                CommercialInfoValue = prvcminf.Field<string>("CommercialInfoValue"),
                                                CommercialInfoLargeValue = prvcminf.Field<string>("CommercialInfoLargeValue"),
                                            } into prvcminfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvcminfg.Key.CommercialInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvcminfg.Key.CommercialInfoType,
                                                },
                                                Value = prvcminfg.Key.CommercialInfoValue,
                                                LargeValue = prvcminfg.Key.CommercialInfoLargeValue,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region Certification

                                 RelatedCertification =
                                   (from prvcr in response.DataSetResult.Tables[4].AsEnumerable()
                                    where !prvcr.IsNull("CertificationId") &&
                                           prvcr.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvcr.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                                    group prvcr by new
                                    {
                                        CertificationId = prvcr.Field<int>("CertificationId"),
                                        CertificationType = prvcr.Field<int>("CertificationType"),
                                    } into prvcrg
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvcrg.Key.CertificationId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvcrg.Key.CertificationType,
                                        },
                                        ItemInfo =
                                           (from prvcrinf in response.DataSetResult.Tables[4].AsEnumerable()
                                            where !prvcrinf.IsNull("CertificationInfoId") &&
                                                   prvcrinf.Field<int>("CertificationId") == prvcrg.Key.CertificationId
                                            group prvcrinf by new
                                            {
                                                CertificationInfoId = prvcrinf.Field<int>("CertificationInfoId"),
                                                CertificationInfoType = prvcrinf.Field<int>("CertificationInfoType"),
                                                CertificationInfoValue = prvcrinf.Field<string>("CertificationInfoValue"),
                                            } into prvcrinfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvcrinfg.Key.CertificationInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvcrinfg.Key.CertificationInfoType,
                                                },
                                                Value = prvcrinfg.Key.CertificationInfoValue,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region Financial

                                 RelatedFinantial =
                                   (from prvfi in response.DataSetResult.Tables[5].AsEnumerable()
                                    where !prvfi.IsNull("FinancialId") &&
                                           prvfi.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvfi.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                           prvfi.Field<int>("FinancialType") != 501001
                                    group prvfi by new
                                    {
                                        FinancialId = prvfi.Field<int>("FinancialId"),
                                        FinancialType = prvfi.Field<int>("FinancialType"),
                                    } into prvfig
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvfig.Key.FinancialId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvfig.Key.FinancialType,
                                        },
                                        ItemInfo =
                                           (from prvfiinf in response.DataSetResult.Tables[5].AsEnumerable()
                                            where !prvfiinf.IsNull("FinancialInfoId") &&
                                                   prvfiinf.Field<int>("FinancialId") == prvfig.Key.FinancialId
                                            group prvfiinf by new
                                            {
                                                FinancialInfoId = prvfiinf.Field<int>("FinancialInfoId"),
                                                FinancialInfoType = prvfiinf.Field<int>("FinancialInfoType"),
                                                FinancialInfoValue = prvfiinf.Field<string>("FinancialInfoValue"),
                                            } into prvfiinfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvfiinfg.Key.FinancialInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvfiinfg.Key.FinancialInfoType,
                                                },
                                                Value = prvfiinfg.Key.FinancialInfoValue,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region BalanceSheet

                                 RelatedBalanceSheet =
                                   (from prvfi in response.DataSetResult.Tables[5].AsEnumerable()
                                    where !prvfi.IsNull("FinancialId") &&
                                           prvfi.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvfi.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                           prvfi.Field<int>("FinancialType") == 501001
                                    group prvfi by new
                                    {
                                        FinancialId = prvfi.Field<int>("FinancialId"),
                                        FinancialType = prvfi.Field<int>("FinancialType"),
                                    } into prvfig
                                    select new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel()
                                    {
                                        ItemId = prvfig.Key.FinancialId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvfig.Key.FinancialType,
                                        },
                                        ItemInfo =
                                           (from prvfiinf in response.DataSetResult.Tables[5].AsEnumerable()
                                            where !prvfiinf.IsNull("FinancialInfoId") &&
                                                   prvfiinf.Field<int>("FinancialId") == prvfig.Key.FinancialId
                                            group prvfiinf by new
                                            {
                                                FinancialInfoId = prvfiinf.Field<int>("FinancialInfoId"),
                                                FinancialInfoType = prvfiinf.Field<int>("FinancialInfoType"),
                                                FinancialInfoValue = prvfiinf.Field<string>("FinancialInfoValue"),
                                            } into prvfiinfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvfiinfg.Key.FinancialInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvfiinfg.Key.FinancialInfoType,
                                                },
                                                Value = prvfiinfg.Key.FinancialInfoValue,
                                            }).ToList(),

                                        BalanceSheetInfo =
                                            (from prvbs in response.DataSetResult.Tables[6].AsEnumerable()
                                             where !prvbs.IsNull("BalanceSheetId") &&
                                                    prvbs.Field<int>("FinancialId") == prvfig.Key.FinancialId
                                             group prvbs by new
                                             {
                                                 BalanceSheetId = prvbs.Field<int>("BalanceSheetId"),
                                                 Account = prvbs.Field<int>("Account"),
                                                 BalanceSheetValue = prvbs.Field<decimal>("BalanceSheetValue"),
                                             } into prvbsg
                                             select new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                             {
                                                 BalanceSheetId = prvbsg.Key.BalanceSheetId,
                                                 RelatedAccount = new Company.Models.Util.GenericItemModel()
                                                 {
                                                     ItemId = prvbsg.Key.Account,
                                                 },
                                                 Value = prvbsg.Key.BalanceSheetValue,
                                             }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region Legal

                                 RelatedLegal =
                                   (from prvlg in response.DataSetResult.Tables[7].AsEnumerable()
                                    where !prvlg.IsNull("LegalId") &&
                                           prvlg.Field<int>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvlg.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                                    group prvlg by new
                                    {
                                        LegalId = prvlg.Field<int>("LegalId"),
                                        LegalType = prvlg.Field<int>("LegalType"),
                                    } into prvlgg
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvlgg.Key.LegalId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvlgg.Key.LegalType,
                                        },
                                        ItemInfo =
                                           (from prvlginf in response.DataSetResult.Tables[7].AsEnumerable()
                                            where !prvlginf.IsNull("LegalInfoId") &&
                                                   prvlginf.Field<int>("LegalId") == prvlgg.Key.LegalId
                                            group prvlginf by new
                                            {
                                                LegalInfoId = prvlginf.Field<int>("LegalInfoId"),
                                                LegalInfoType = prvlginf.Field<int>("LegalInfoType"),
                                                LegalInfoValue = prvlginf.Field<string>("LegalInfoValue"),
                                            } into prvlginfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvlginfg.Key.LegalInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvlginfg.Key.LegalInfoType,
                                                },
                                                Value = prvlginfg.Key.LegalInfoValue,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion
                             },

                             #endregion

                         }).ToList(),

                    #endregion
                };
            }

            return oReturn;
        }

        public ProjectModel ProjectGetByIdProviderDetail(string ProjectPublicId, string CustomerPublicId, string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectPublicId", ProjectPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "MP_CC_Project_GetById_ProviderDetail",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            ProjectModel oReturn = null;

            if (response.DataSetResult != null &&
                response.DataSetResult.Tables.Count > 8 &&
                response.DataSetResult.Tables[0] != null &&
                response.DataSetResult.Tables[0].Rows.Count > 0 &&
                response.DataSetResult.Tables[1] != null &&
                response.DataSetResult.Tables[2] != null &&
                response.DataSetResult.Tables[3] != null &&
                response.DataSetResult.Tables[4] != null &&
                response.DataSetResult.Tables[5] != null &&
                response.DataSetResult.Tables[6] != null &&
                response.DataSetResult.Tables[7] != null &&
                response.DataSetResult.Tables[8] != null)
            {
                oReturn = new ProjectModel()
                {
                    ProjectPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId"),
                    ProjectName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectName"),
                    ProjectStatus = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = (int)response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectStatusId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectStatusName"),
                    },
                    LastModify = response.DataSetResult.Tables[0].Rows[0].Field<DateTime>("ProjectLastModify"),

                    #region ProjectInfo

                    ProjectInfo =
                        (from pjinf in response.DataSetResult.Tables[0].AsEnumerable()
                         where !pjinf.IsNull("ProjectInfoId") &&
                                pjinf.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group pjinf by new
                         {
                             ProjectInfoId = pjinf.Field<int>("ProjectInfoId"),
                             ProjectInfoTypeId = pjinf.Field<int>("ProjectInfoTypeId"),
                             ProjectInfoTypeName = pjinf.Field<string>("ProjectInfoTypeName"),
                             ProjectInfoValue = pjinf.Field<string>("ProjectInfoValue"),
                             ProjectInfoLargeValue = pjinf.Field<string>("ProjectInfoLargeValue"),
                             ProjectInfoValueName = pjinf.Field<string>("ProjectInfoValueName"),
                         } into pjinfg
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                         {
                             ItemInfoId = pjinfg.Key.ProjectInfoId,
                             ItemInfoType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = pjinfg.Key.ProjectInfoTypeId,
                                 ItemName = pjinfg.Key.ProjectInfoTypeName,
                             },
                             Value = pjinfg.Key.ProjectInfoValue,
                             LargeValue = pjinfg.Key.ProjectInfoLargeValue,
                             ValueName = pjinfg.Key.ProjectInfoValueName,
                         }).ToList(),

                    #endregion

                    #region Project Config

                    RelatedProjectConfig = new ProjectConfigModel()
                    {
                        ItemId = (int)response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectConfigId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectConfigName"),

                        RelatedCustomer = new CompanyCustomer.Models.Customer.CustomerModel()
                        {
                            RelatedCompany = new Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("CustomerPublicId"),
                            },
                        },

                        RelatedEvaluationItem =
                            (from pjei in response.DataSetResult.Tables[2].AsEnumerable()
                             where !pjei.IsNull("EvaluationItemId") &&
                                    pjei.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                    pjei.Field<Int64>("ProjectConfigId") == response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectConfigId")
                             group pjei by new
                             {
                                 EvaluationItemId = pjei.Field<int>("EvaluationItemId"),
                                 EvaluationItemName = pjei.Field<string>("EvaluationItemName"),
                                 EvaluationItemTypeId = pjei.Field<int>("EvaluationItemTypeId"),
                                 EvaluationItemTypeName = pjei.Field<string>("EvaluationItemTypeName"),
                                 ParentEvaluationItem = pjei.Field<int?>("ParentEvaluationItem"),
                             } into pjeig
                             select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                             {
                                 ItemId = pjeig.Key.EvaluationItemId,
                                 ItemName = pjeig.Key.EvaluationItemName,
                                 ItemType = new Company.Models.Util.CatalogModel()
                                 {
                                     ItemId = pjeig.Key.EvaluationItemTypeId,
                                     ItemName = pjeig.Key.EvaluationItemTypeName,
                                 },
                                 ParentItem = pjeig.Key.ParentEvaluationItem == null ? null :
                                    new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = pjeig.Key.ParentEvaluationItem.Value,
                                    },

                                 ItemInfo =
                                     (from pjeiinf in response.DataSetResult.Tables[2].AsEnumerable()
                                      where !pjeiinf.IsNull("EvaluationItemInfoId") &&
                                             pjeiinf.Field<int>("EvaluationItemId") == pjeig.Key.EvaluationItemId
                                      group pjeiinf by new
                                      {
                                          EvaluationItemInfoId = pjeiinf.Field<int>("EvaluationItemInfoId"),
                                          EvaluationItemInfoTypeId = pjeiinf.Field<int>("EvaluationItemInfoTypeId"),
                                          EvaluationItemInfoTypeName = pjeiinf.Field<string>("EvaluationItemInfoTypeName"),
                                          EvaluationItemInfoValue = pjeiinf.Field<string>("EvaluationItemInfoValue"),
                                          EvaluationItemInfoLargeValue = pjeiinf.Field<string>("EvaluationItemInfoLargeValue"),
                                      } into pjeiinfg
                                      select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                      {
                                          ItemInfoId = pjeiinfg.Key.EvaluationItemInfoId,
                                          ItemInfoType = new Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = pjeiinfg.Key.EvaluationItemInfoTypeId,
                                              ItemName = pjeiinfg.Key.EvaluationItemInfoTypeName,
                                          },
                                          Value = pjeiinfg.Key.EvaluationItemInfoValue,
                                          LargeValue = pjeiinfg.Key.EvaluationItemInfoLargeValue,
                                      }).ToList(),
                             }).ToList(),
                    },

                    #endregion

                    #region Project Provider

                    RelatedProjectProvider =
                        (from rpp in response.DataSetResult.Tables[1].AsEnumerable()
                         where !rpp.IsNull("ProjectCompanyId") &&
                                 rpp.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group rpp by new
                         {
                             ProjectCompanyId = (int)rpp.Field<Int64>("ProjectCompanyId"),
                             ProjectCompanyLastModify = rpp.Field<DateTime>("ProjectCompanyLastModify"),

                             ProviderPublicId = rpp.Field<string>("ProviderPublicId"),
                             CompanyName = rpp.Field<string>("CompanyName"),
                             IdentificationTypeId = (int)rpp.Field<Int64>("IdentificationTypeId"),
                             IdentificationTypeName = rpp.Field<string>("IdentificationTypeName"),
                             IdentificationNumber = rpp.Field<string>("IdentificationNumber"),
                             CompanyTypeId = (int)rpp.Field<Int64>("CompanyTypeId"),
                             CompanyTypeName = rpp.Field<string>("CompanyTypeName"),

                         } into rppg
                         select new ProjectProviderModel()
                         {
                             ProjectCompanyId = rppg.Key.ProjectCompanyId,
                             LastModify = rppg.Key.ProjectCompanyLastModify,
                             ItemInfo =
                              (from rppinf in response.DataSetResult.Tables[3].AsEnumerable()
                               where !rppinf.IsNull("ProjectCompanyInfoId") &&
                                       rppinf.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId
                               group rppinf by new
                               {
                                   ProjectCompanyInfoId = rppinf.Field<int>("ProjectCompanyInfoId"),
                                   EvaluationItemId = rppinf.Field<int?>("EvaluationItemId"),
                                   ProjectCompanyInfoTypeId = rppinf.Field<int>("ProjectCompanyInfoTypeId"),
                                   ProjectCompanyInfoTypeName = rppinf.Field<string>("ProjectCompanyInfoTypeName"),
                                   ProjectCompanyInfoValue = rppinf.Field<string>("ProjectCompanyInfoValue"),
                                   ProjectCompanyInfoLargeValue = rppinf.Field<string>("ProjectCompanyInfoLargeValue"),
                               } into rppinfg
                               select new ProjectProviderInfoModel()
                               {
                                   ItemInfoId = rppinfg.Key.ProjectCompanyInfoId,
                                   RelatedEvaluationItem = rppinfg.Key.EvaluationItemId == null ? null :
                                        new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = rppinfg.Key.EvaluationItemId.Value,
                                        },
                                   ItemInfoType = new Company.Models.Util.CatalogModel()
                                   {
                                       ItemId = rppinfg.Key.ProjectCompanyInfoTypeId,
                                       ItemName = rppinfg.Key.ProjectCompanyInfoTypeName,
                                   },
                                   Value = rppinfg.Key.ProjectCompanyInfoValue,
                                   LargeValue = rppinfg.Key.ProjectCompanyInfoLargeValue,
                               }).ToList(),

                             #region ProviderInfo

                             RelatedProvider = new CompanyProvider.Models.Provider.ProviderModel()
                             {
                                 RelatedCompany = new Company.Models.Company.CompanyModel()
                                 {
                                     CompanyPublicId = rppg.Key.ProviderPublicId,
                                     CompanyName = rppg.Key.CompanyName,
                                     IdentificationType = new Company.Models.Util.CatalogModel()
                                     {
                                         ItemId = rppg.Key.IdentificationTypeId,
                                         ItemName = rppg.Key.IdentificationTypeName,
                                     },
                                     IdentificationNumber = rppg.Key.IdentificationNumber,
                                     CompanyType = new Company.Models.Util.CatalogModel()
                                     {
                                         ItemId = rppg.Key.CompanyTypeId,
                                         ItemName = rppg.Key.CompanyTypeName,
                                     },

                                     CompanyInfo =
                                        (from prvinf in response.DataSetResult.Tables[1].AsEnumerable()
                                         where !prvinf.IsNull("CompanyInfoId") &&
                                                 prvinf.Field<string>("ProviderPublicId") == rppg.Key.ProviderPublicId
                                         group prvinf by new
                                         {
                                             CompanyInfoId = prvinf.Field<int>("CompanyInfoId"),
                                             CompanyInfoTypeId = prvinf.Field<int>("CompanyInfoTypeId"),
                                             CompanyInfoTypeName = prvinf.Field<string>("CompanyInfoTypeName"),
                                             CompanyInfoValue = prvinf.Field<string>("CompanyInfoValue"),
                                             CompanyInfoLargeValue = prvinf.Field<string>("CompanyInfoLargeValue"),
                                         } into prvinfg
                                         select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                         {
                                             ItemInfoId = prvinfg.Key.CompanyInfoId,
                                             ItemInfoType = new Company.Models.Util.CatalogModel()
                                             {
                                                 ItemId = prvinfg.Key.CompanyInfoTypeId,
                                                 ItemName = prvinfg.Key.CompanyInfoTypeName,
                                             },
                                             Value = prvinfg.Key.CompanyInfoValue,
                                             LargeValue = prvinfg.Key.CompanyInfoLargeValue,
                                         }).ToList()
                                 },

                                 #region Commercial

                                 RelatedCommercial =
                                   (from prvcm in response.DataSetResult.Tables[4].AsEnumerable()
                                    where !prvcm.IsNull("CommercialId") &&
                                           prvcm.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvcm.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                                    group prvcm by new
                                    {
                                        CommercialId = prvcm.Field<int>("CommercialId"),
                                        CommercialType = prvcm.Field<int>("CommercialType"),
                                    } into prvcmg
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvcmg.Key.CommercialId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvcmg.Key.CommercialType,
                                        },
                                        ItemInfo =
                                           (from prvcminf in response.DataSetResult.Tables[4].AsEnumerable()
                                            where !prvcminf.IsNull("CommercialInfoId") &&
                                                   prvcminf.Field<int>("CommercialId") == prvcmg.Key.CommercialId
                                            group prvcminf by new
                                            {
                                                CommercialInfoId = prvcminf.Field<int>("CommercialInfoId"),
                                                CommercialInfoTypeId = prvcminf.Field<int>("CommercialInfoTypeId"),
                                                CommercialInfoValue = prvcminf.Field<string>("CommercialInfoValue"),
                                                CommercialInfoLargeValue = prvcminf.Field<string>("CommercialInfoLargeValue"),
                                            } into prvcminfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvcminfg.Key.CommercialInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvcminfg.Key.CommercialInfoTypeId,
                                                },
                                                Value = prvcminfg.Key.CommercialInfoValue,
                                                LargeValue = prvcminfg.Key.CommercialInfoLargeValue,
                                                ValueName = prvcminfg.Key.CommercialInfoTypeId == 302013 ||
                                                            prvcminfg.Key.CommercialInfoTypeId == 302014 ?
                                                    string.Join(";",
                                                        (from prvcminfn in response.DataSetResult.Tables[4].AsEnumerable()
                                                         where !prvcminfn.IsNull("CommercialInfoValueName") &&
                                                                prvcminfn.Field<int>("CommercialInfoId") == prvcminfg.Key.CommercialInfoId &&
                                                                !string.IsNullOrEmpty(prvcminfn.Field<string>("CommercialInfoValueName"))
                                                         group prvcminfn by new
                                                         {
                                                             CommercialInfoValueName = prvcminfn.Field<string>("CommercialInfoValueName"),
                                                         } into prvcminfng
                                                         select prvcminfng.Key.CommercialInfoValueName).ToList())
                                                    : string.Empty,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region Certification

                                 RelatedCertification =
                                   (from prvcr in response.DataSetResult.Tables[5].AsEnumerable()
                                    where !prvcr.IsNull("CertificationId") &&
                                           prvcr.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvcr.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                                    group prvcr by new
                                    {
                                        CertificationId = prvcr.Field<int>("CertificationId"),
                                        CertificationType = prvcr.Field<int>("CertificationType"),
                                    } into prvcrg
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvcrg.Key.CertificationId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvcrg.Key.CertificationType,
                                        },
                                        ItemInfo =
                                           (from prvcrinf in response.DataSetResult.Tables[5].AsEnumerable()
                                            where !prvcrinf.IsNull("CertificationInfoId") &&
                                                   prvcrinf.Field<int>("CertificationId") == prvcrg.Key.CertificationId
                                            group prvcrinf by new
                                            {
                                                CertificationInfoId = prvcrinf.Field<int>("CertificationInfoId"),
                                                CertificationInfoType = prvcrinf.Field<int>("CertificationInfoType"),
                                                CertificationInfoValue = prvcrinf.Field<string>("CertificationInfoValue"),
                                                CertificationInfoLargeValue = prvcrinf.Field<string>("CertificationInfoLargeValue"),
                                            } into prvcrinfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvcrinfg.Key.CertificationInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvcrinfg.Key.CertificationInfoType,
                                                },
                                                Value = prvcrinfg.Key.CertificationInfoValue,
                                                LargeValue = prvcrinfg.Key.CertificationInfoLargeValue,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region Financial

                                 RelatedFinantial =
                                   (from prvfi in response.DataSetResult.Tables[6].AsEnumerable()
                                    where !prvfi.IsNull("FinancialId") &&
                                           (int)prvfi.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvfi.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                           prvfi.Field<int>("FinancialType") != 501001
                                    group prvfi by new
                                    {
                                        FinancialId = prvfi.Field<int>("FinancialId"),
                                        FinancialType = prvfi.Field<int>("FinancialType"),
                                    } into prvfig
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvfig.Key.FinancialId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvfig.Key.FinancialType,
                                        },
                                        ItemInfo =
                                           (from prvfiinf in response.DataSetResult.Tables[6].AsEnumerable()
                                            where !prvfiinf.IsNull("FinancialInfoId") &&
                                                   prvfiinf.Field<int>("FinancialId") == prvfig.Key.FinancialId
                                            group prvfiinf by new
                                            {
                                                FinancialInfoId = prvfiinf.Field<int>("FinancialInfoId"),
                                                FinancialInfoType = prvfiinf.Field<int>("FinancialInfoType"),
                                                FinancialInfoValue = prvfiinf.Field<string>("FinancialInfoValue"),
                                                FinancialInfoLargeValue = prvfiinf.Field<string>("FinancialInfoLargeValue"),
                                            } into prvfiinfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvfiinfg.Key.FinancialInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvfiinfg.Key.FinancialInfoType,
                                                },
                                                Value = prvfiinfg.Key.FinancialInfoValue,
                                                LargeValue = prvfiinfg.Key.FinancialInfoLargeValue,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region BalanceSheet

                                 RelatedBalanceSheet =
                                   (from prvfi in response.DataSetResult.Tables[6].AsEnumerable()
                                    where !prvfi.IsNull("FinancialId") &&
                                           prvfi.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvfi.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                           prvfi.Field<int>("FinancialType") == 501001
                                    group prvfi by new
                                    {
                                        FinancialId = prvfi.Field<int>("FinancialId"),
                                        FinancialType = prvfi.Field<int>("FinancialType"),
                                    } into prvfig
                                    select new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel()
                                    {
                                        ItemId = prvfig.Key.FinancialId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvfig.Key.FinancialType,
                                        },
                                        ItemInfo =
                                           (from prvfiinf in response.DataSetResult.Tables[6].AsEnumerable()
                                            where !prvfiinf.IsNull("FinancialInfoId") &&
                                                   prvfiinf.Field<int>("FinancialId") == prvfig.Key.FinancialId
                                            group prvfiinf by new
                                            {
                                                FinancialInfoId = prvfiinf.Field<int>("FinancialInfoId"),
                                                FinancialInfoType = prvfiinf.Field<int>("FinancialInfoType"),
                                                FinancialInfoValue = prvfiinf.Field<string>("FinancialInfoValue"),
                                                FinancialInfoLargeValue = prvfiinf.Field<string>("FinancialInfoLargeValue"),
                                            } into prvfiinfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvfiinfg.Key.FinancialInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvfiinfg.Key.FinancialInfoType,
                                                },
                                                Value = prvfiinfg.Key.FinancialInfoValue,
                                                LargeValue = prvfiinfg.Key.FinancialInfoLargeValue,
                                            }).ToList(),

                                        BalanceSheetInfo =
                                            (from prvbs in response.DataSetResult.Tables[7].AsEnumerable()
                                             where !prvbs.IsNull("BalanceSheetId") &&
                                                    prvbs.Field<int>("FinancialId") == prvfig.Key.FinancialId
                                             group prvbs by new
                                             {
                                                 BalanceSheetId = prvbs.Field<int>("BalanceSheetId"),
                                                 Account = prvbs.Field<int>("Account"),
                                                 BalanceSheetValue = prvbs.Field<decimal>("BalanceSheetValue"),
                                             } into prvbsg
                                             select new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                             {
                                                 BalanceSheetId = prvbsg.Key.BalanceSheetId,
                                                 RelatedAccount = new Company.Models.Util.GenericItemModel()
                                                 {
                                                     ItemId = prvbsg.Key.Account,
                                                 },
                                                 Value = prvbsg.Key.BalanceSheetValue,
                                             }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region Legal

                                 RelatedLegal =
                                   (from prvlg in response.DataSetResult.Tables[8].AsEnumerable()
                                    where !prvlg.IsNull("LegalId") &&
                                           prvlg.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvlg.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                                    group prvlg by new
                                    {
                                        LegalId = prvlg.Field<int>("LegalId"),
                                        LegalType = prvlg.Field<int>("LegalType"),
                                    } into prvlgg
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvlgg.Key.LegalId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvlgg.Key.LegalType,
                                        },
                                        ItemInfo =
                                           (from prvlginf in response.DataSetResult.Tables[8].AsEnumerable()
                                            where !prvlginf.IsNull("LegalInfoId") &&
                                                   prvlginf.Field<int>("LegalId") == prvlgg.Key.LegalId
                                            group prvlginf by new
                                            {
                                                LegalInfoId = prvlginf.Field<int>("LegalInfoId"),
                                                LegalInfoType = prvlginf.Field<int>("LegalInfoType"),
                                                LegalInfoValue = prvlginf.Field<string>("LegalInfoValue"),
                                                LegalInfoLargeValue = prvlginf.Field<string>("LegalInfoLargeValue"),
                                            } into prvlginfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvlginfg.Key.LegalInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvlginfg.Key.LegalInfoType,
                                                },
                                                Value = prvlginfg.Key.LegalInfoValue,
                                                LargeValue = prvlginfg.Key.LegalInfoLargeValue,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion
                             },

                             #endregion

                         }).ToList(),

                    #endregion
                };
            }

            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> ProjectSearchFilter(string SearchParam)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Project_SearchFilter",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from sf in response.DataTableResult.AsEnumerable()
                     where !sf.IsNull("ItemId")
                     select new ProveedoresOnLine.Company.Models.Util.GenericFilterModel()
                     {
                         FilterType = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                         {
                             ItemId = (int)sf.Field<Int32>("ItemId"),
                             ItemName = sf.Field<string>("Name"),
                         },
                     }).ToList();
            }
            return oReturn;
        }


        #endregion

        #region Utils

        public List<Company.Models.Util.CatalogModel> CatalogGetProjectConfigOptions()
        {
            List<Company.Models.Util.CatalogModel> oReturn = null;

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Catalog_GetProjectConfigOptions",
                CommandType = CommandType.StoredProcedure,
            });

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from opt in response.DataTableResult.AsEnumerable()
                     where !string.IsNullOrEmpty("CatalogId")
                     group opt by new
                     {
                         CatalogId = opt.Field<int>("CatalogId"),
                         CatalogName = opt.Field<string>("CatalogName"),
                         ItemId = opt.Field<int>("ItemId"),
                         ItemName = opt.Field<string>("ItemName"),
                     } into optg
                     select new Company.Models.Util.CatalogModel()
                     {
                         CatalogId = optg.Key.CatalogId,
                         CatalogName = optg.Key.CatalogName,
                         ItemId = optg.Key.ItemId,
                         ItemName = optg.Key.ItemName,
                     }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region ProjectCharts

        public List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetProjectByState(string CustomerPublicId, DateTime Year)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCurrentDate", Year));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_ProjectCharts_GetByState",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericChartsModelInfo> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                   (from sv in response.DataTableResult.AsEnumerable()
                    where !sv.IsNull("Count")
                    select new GenericChartsModelInfo()
                    {
                        Title = "Proceso de Selección por estado",
                        CountX = (int)sv.Field<Int32>("ProjectStatus"),
                        AxisX = sv.Field<string>("Name"),
                        Count = (int)sv.Field<UInt64>("Count"),
                    }).ToList();
            }

            return oReturn;
        }


        public List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetProjectByMonth(string CustomerPublicId, DateTime Year)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCurrentDate", Year));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_ProjectCharts_GetByMonth",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericChartsModelInfo> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                   (from sv in response.DataTableResult.AsEnumerable()
                    where !sv.IsNull("Count")
                    select new GenericChartsModelInfo()
                    {
                        CountX = (int)sv.Field<Int32>("CurrentMonth"),
                        AxisX = sv.Field<string>("LabelMonth"),
                        Title = sv.Field<string>("State"),
                        AxisY = sv.Field<string>("Responsible"),
                        Count = (int)sv.Field<UInt64>("Count"),
                        Date = sv.Field<DateTime>("ProjectDate")
                    }).ToList();
            }

            return oReturn;
        }

        #endregion
    }
}
