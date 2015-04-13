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

        public List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> GetAllProjectConfigByCustomerPublicId(string CustomerPublicId, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CP_ProjectConfig_GetByCustomerProvider",
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
                         RelatedEvaluationItem =
                            (from ei in response.DataTableResult.AsEnumerable()
                             where !ei.IsNull("EvaluationItemId") &&
                                    ei.Field<int>("ProjectConfigId") == pcg.Key.ProjectConfigId
                             group ei by new
                             {
                                 EvaluationItemId = ei.Field<int>("EvaluationItemId"),
                                 EvaluationItemName = ei.Field<string>("EvaluationItemName"),
                                 EvaluationTypeId = ei.Field<int>("EvaluationTypeId"),
                                 EvaluationTypeName = ei.Field<string>("EvaluationTypeName"),
                                 EvaluationItemEnable = ei.Field<UInt64>("EvaluationItemEnable") == 1 ? true : false,
                             } into eig
                             select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                             {
                                 ItemId = eig.Key.EvaluationItemId,
                                 ItemName = eig.Key.EvaluationItemName,
                                 ItemType = new Company.Models.Util.CatalogModel()
                                 {
                                     ItemId = eig.Key.EvaluationTypeId,
                                     ItemName = eig.Key.EvaluationTypeName
                                 },
                                 Enable = eig.Key.EvaluationItemEnable,
                                 ItemInfo =
                                    (from eiinf in response.DataTableResult.AsEnumerable()
                                     where !eiinf.IsNull("EvaluationItemInfoId") &&
                                            eiinf.Field<int>("EvaluationItemId") == eig.Key.EvaluationItemId
                                     group eiinf by new
                                     {
                                         EvaluationItemInfoId = eiinf.Field<int>("EvaluationItemInfoId"),
                                         EvaluationItemInfoTypeId = eiinf.Field<int>("EvaluationItemInfoTypeId"),
                                         EvaluationItemInfoTypeName = eiinf.Field<string>("EvaluationItemInfoTypeName"),
                                         Value = eiinf.Field<string>("Value"),
                                         LargeValue = eiinf.Field<string>("LargeValue"),
                                         EvaluationItemInfoEnable = eiinf.Field<UInt64>("EvaluationItemInfoEnable") == 1 ? true : false,
                                     } into eiinfg
                                     select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                     {
                                         ItemInfoId = eiinfg.Key.EvaluationItemInfoId,
                                         ItemInfoType = new Company.Models.Util.CatalogModel()
                                         {
                                             ItemId = eiinfg.Key.EvaluationItemInfoTypeId,
                                             ItemName = eiinfg.Key.EvaluationItemInfoTypeName,
                                         },
                                         Value = eiinfg.Key.Value,
                                         LargeValue = eiinfg.Key.LargeValue,
                                         Enable = eiinfg.Key.EvaluationItemInfoEnable,
                                     }).ToList(),
                             }).ToList(),
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

        #endregion
    }
}
