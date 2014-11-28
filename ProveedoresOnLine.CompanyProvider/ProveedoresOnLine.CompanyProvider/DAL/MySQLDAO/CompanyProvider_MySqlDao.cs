using ProveedoresOnLine.CompanyProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.Company.Models.Util;

namespace ProveedoresOnLine.CompanyProvider.DAL.MySQLDAO
{
    internal class CompanyProvider_MySqlDao : ICompanyProviderData
    {
        private ADO.Interfaces.IADO DataInstance;

        public CompanyProvider_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.CompanyProvider.Models.Constants.C_POL_CompanyProviderConnectionName);
        }

        #region Provider Experience

        public int ProviderCategoryUpsert(string CompanyPublicId, int CategoryId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryId", CategoryId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_CompanyCategory_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int ExperienceUpsert(string CompanyPublicId, int? ExperienceId, string ExperienceName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vExperienceId", ExperienceId));
            lstParams.Add(DataInstance.CreateTypedParameter("vExperienceName", ExperienceName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_Experience_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int ExperienceInfoUpsert(int ExperienceId, int? ExperienceInfoId, int ExperienceInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vExperienceId", ExperienceId));
            lstParams.Add(DataInstance.CreateTypedParameter("vExperienceInfoId", ExperienceInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vExperienceInfoTypeId", ExperienceInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_ExperienceInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion

        #region Provider Certification

        public int CertificationUpsert(string CompanyPublicId, int? CertificationId, int CertificationTypeId, string CertificationName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationId", CertificationId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationTypeId", CertificationTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationName", CertificationName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_Certification_UpSert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int CertificationInfoUpsert(int CertificationId, int? CertificationInfoId, int CertificationInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationId", CertificationId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationInfoId", CertificationInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationInfoTypeId", CertificationInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_CertificationInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<GenericItemModel> CertificationGetBasicInfo(string CompanyPublicId, int? CertificationType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationType", CertificationType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CP_Certification_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("CertificationId")
                     group c by new
                     {
                         CertificationId = c.Field<int>("CertificationId"),
                         CertificationTypeId = c.Field<int>("CertificationTypeId"),
                         CertificationTypeName = c.Field<string>("CertificationTypeName"),
                         CertificationName = c.Field<string>("CertificationName"),
                         CertificationEnable = c.Field<UInt64>("CertificationEnable") == 1 ? true : false,
                         CertificationLastModify = c.Field<DateTime>("CertificationLastModify"),
                         CertificationCreateDate = c.Field<DateTime>("CertificationCreateDate"),
                     }
                         into cog
                         select new GenericItemModel()
                         {
                             ItemId = cog.Key.CertificationId,
                             ItemType = new CatalogModel()
                             {
                                 ItemId = cog.Key.CertificationTypeId,
                                 ItemName = cog.Key.CertificationTypeName
                             },
                             ItemName = cog.Key.CertificationName,
                             Enable = cog.Key.CertificationEnable,
                             LastModify = cog.Key.CertificationLastModify,
                             CreateDate = cog.Key.CertificationCreateDate,
                             ItemInfo =
                                 (from coinf in response.DataTableResult.AsEnumerable()
                                  where !coinf.IsNull("CertificationInfoId") &&
                                          coinf.Field<int>("CertificationId") == cog.Key.CertificationId
                                  select new GenericItemInfoModel()
                                  {
                                      ItemInfoId = coinf.Field<int>("CertificationInfoId"),
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemId = coinf.Field<int>("CertificationInfoTypeId"),
                                          ItemName = coinf.Field<string>("CertificationInfoTypeName"),
                                      },
                                      Value = coinf.Field<string>("Value"),
                                      LargeValue = coinf.Field<string>("LargeValue"),
                                      Enable = coinf.Field<UInt64>("CertificationInfoEnable") == 1 ? true : false,
                                      LastModify = coinf.Field<DateTime>("CertificationInfoLastModify"),
                                      CreateDate = coinf.Field<DateTime>("CertificationInfoCreateDate"),
                                  }).ToList(),

                         }).ToList();
            }
            return oReturn;
        }
        
        #endregion

        #region Provider financial

        public int FinancialUpsert(string CompanyPublicId, int? FinancialId, int FinancialTypeId, string FinancialName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFunancualId", FinancialId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialTypeId", FinancialTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialName", FinancialName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_Financial_UpSert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int FinancialInfoUpsert(int FinancialId, int? FinancialInfoId, int FinancialInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialId", FinancialId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialInfoId", FinancialInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialInfoTypeId", FinancialInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_FinancialInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int BalanceSheetUpsert(int FinancialId, int? BalanceSheetId, int AccountId, decimal Value, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialId", FinancialId));
            lstParams.Add(DataInstance.CreateTypedParameter("vBalanceSheetId", BalanceSheetId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAccountId", AccountId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_BalanceSheet_UpSert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion

        #region Provider Legal

        public int LegalUpsert(string CompanyPublicId, int? LegalId, int LegalTypeId, string LegalName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalId", LegalId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalTypeId", LegalTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalName", LegalName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_Legal_UpSert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int LegalInfoUpsert(int LegalId, int? LegalInfoId, int LegalInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vLegalId", LegalId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalInfoId", LegalInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLelalInfoTypeId", LegalInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_Legal_UpSert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<GenericItemModel> LegalGetBasicInfo(string CompanyPublicId, int? LegalType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalType", LegalType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CP_Legal_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

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
                         LegalName = l.Field<string>("LegalName"),
                         LegalEnable = l.Field<UInt64>("LegalEnable") == 1 ? true : false,
                         LegalLastModify = l.Field<DateTime>("LegalLastModify"),
                         LegalCreateDate = l.Field<DateTime>("LegalCreateDate"),
                     }
                         into cog
                         select new GenericItemModel()
                         {
                             ItemId = cog.Key.LegalId,
                             ItemType = new CatalogModel()
                             {
                                 ItemId = cog.Key.LegalTypeId,
                                 ItemName = cog.Key.LegalTypeName
                             },
                             ItemName = cog.Key.LegalName,
                             Enable = cog.Key.LegalEnable,
                             LastModify = cog.Key.LegalLastModify,
                             CreateDate = cog.Key.LegalCreateDate,
                             ItemInfo =
                                 (from coinf in response.DataTableResult.AsEnumerable()
                                  where !coinf.IsNull("LegalInfoId") &&
                                          coinf.Field<int>("LegalId") == cog.Key.LegalId
                                  select new GenericItemInfoModel()
                                  {
                                      ItemInfoId = coinf.Field<int>("LegalInfoId"),
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemId = coinf.Field<int>("LegalInfoTypeId"),
                                          ItemName = coinf.Field<string>("LegalInfoTypeName"),
                                      },
                                      Value = coinf.Field<string>("Value"),
                                      LargeValue = coinf.Field<string>("LargeValue"),
                                      Enable = coinf.Field<UInt64>("LegalInfoEnable") == 1 ? true : false,
                                      LastModify = coinf.Field<DateTime>("LegalInfoLastModify"),
                                      CreateDate = coinf.Field<DateTime>("LegalInfoCreateDate"),
                                  }).ToList(),

                         }).ToList();
            }
            return oReturn;
        }
        #endregion

        #region Util

        public List<Company.Models.Util.CatalogModel> CatalogGetProviderOptions()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Catalog_GetProviderOptions",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            List<ProveedoresOnLine.Company.Models.Util.CatalogModel> oReturn = new List<ProveedoresOnLine.Company.Models.Util.CatalogModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("ItemId")
                     select new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                     {
                         CatalogId = c.Field<int>("CatalogId"),
                         CatalogName = c.Field<string>("CatalogName"),
                         ItemId = c.Field<int>("ItemId"),
                         ItemName = c.Field<string>("ItemName"),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion
    }
}
