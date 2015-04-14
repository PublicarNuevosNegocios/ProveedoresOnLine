using ProveedoresOnLine.ProjectModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

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
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_ProjectConfig_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int EvaluationItemUpsert(int? EvaluationItemId, int ProjectConfigId, string EvaluationItemName, int EvaluationItemType, int? ParentEvaluationItem, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemId", EvaluationItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigId", ProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemName", EvaluationItemName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemType", EvaluationItemType));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentEvaluationItem", ParentEvaluationItem));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_EvaluationItemUpsert",
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
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemInfoType", EvaluationItemInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "EvaluationItemInfoUpsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);

        }

        //public List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> GetProjectConfigByCustomer(string CustomerPublicId, int PageNumber, int RowCount, out int TotalRows)
        //{
        //    List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> oReturn = null;

        //    List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

        //    lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

        //    ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
        //    {
        //        CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
        //        CommandText = "",
        //        CommandType = System.Data.CommandType.StoredProcedure,
        //        Parameters = lstParams,
        //    });

        //    if (response.DataTableResult != null &&
        //        response.DataTableResult.Rows.Count > 0)
        //    {
        //        oReturn =
        //            (from )
        //    }

        //    return oReturn;
        //}

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


        public List<ProjectModel> ProjectSearch(string CustomerPublicId, string SearchParam, int ProjectStatus, int PageNumber, int RowCount, out int TotalRows)
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
                        ItemId = response.DataSetResult.Tables[0].Rows[0].Field<int>("ProjectStatusId"),
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
                        ItemId = response.DataSetResult.Tables[0].Rows[0].Field<int>("ProjectConfigId"),
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
                                    pjei.Field<int>("ProjectConfigId") == response.DataSetResult.Tables[0].Rows[0].Field<int>("ProjectConfigId")
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

                    RelatedProjectProvider =
                        (from rpp in response.DataSetResult.Tables[1].AsEnumerable()
                         where !rpp.IsNull("ProjectCompanyId") &&
                                 rpp.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group rpp by new
                         {
                             ProjectCompanyId = rpp.Field<int>("ProjectCompanyId"),
                             ProjectCompanyLastModify = rpp.Field<DateTime>("ProjectCompanyId"),
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
                        ItemId = response.DataSetResult.Tables[0].Rows[0].Field<int>("ProjectStatusId"),
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
                        ItemId = response.DataSetResult.Tables[0].Rows[0].Field<int>("ProjectConfigId"),
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
                             ProjectCompanyLastModify = rpp.Field<DateTime>("ProjectCompanyId"),
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

        #endregion

    }
}
