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

        #region Commercial

        public int CommercialUpsert(string CompanyPublicId, int? CommercialId, int CommercialTypeId, string CommercialName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCommercialId", CommercialId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCommercialTypeId", CommercialTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCommercialName", CommercialName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_Commercial_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int CommercialInfoUpsert(int CommercialId, int? CommercialInfoId, int CommercialInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCommercialId", CommercialId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCommercialInfoId", CommercialInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCommercialInfoTypeId", CommercialInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_CommercialInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<GenericItemModel> CommercialGetBasicInfo(string CompanyPublicId, int? CommercialType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCommercialType", CommercialType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CP_Commercial_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cm in response.DataTableResult.AsEnumerable()
                     where !cm.IsNull("CommercialId")
                     group cm by new
                     {
                         CommercialId = cm.Field<int>("CommercialId"),
                         CommercialTypeId = cm.Field<int>("CommercialTypeId"),
                         CommercialTypeName = cm.Field<string>("CommercialTypeName"),
                         CommercialName = cm.Field<string>("CommercialName"),
                         CommercialEnable = cm.Field<UInt64>("CommercialEnable") == 1 ? true : false,
                         CommercialLastModify = cm.Field<DateTime>("CommercialLastModify"),
                         CommercialCreateDate = cm.Field<DateTime>("CommercialCreateDate"),
                     } into cmg
                     select new GenericItemModel()
                     {
                         ItemId = cmg.Key.CommercialId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = cmg.Key.CommercialTypeId,
                             ItemName = cmg.Key.CommercialTypeName
                         },
                         ItemName = cmg.Key.CommercialName,
                         Enable = cmg.Key.CommercialEnable,
                         LastModify = cmg.Key.CommercialLastModify,
                         CreateDate = cmg.Key.CommercialCreateDate,
                         ItemInfo =
                             (from cminf in response.DataTableResult.AsEnumerable()
                              where !cminf.IsNull("CommercialInfoId") &&
                                      cminf.Field<int>("CommercialId") == cmg.Key.CommercialId
                              group cminf by new
                              {
                                  CommercialInfoId = cminf.Field<int>("CommercialInfoId"),
                                  CommercialInfoTypeId = cminf.Field<int>("CommercialInfoTypeId"),
                                  CommercialInfoTypeName = cminf.Field<string>("CommercialInfoTypeName"),
                                  Value = cminf.Field<string>("Value"),
                                  LargeValue = cminf.Field<string>("LargeValue"),
                                  CommercialInfoEnable = cminf.Field<UInt64>("CommercialInfoEnable") == 1 ? true : false,
                                  CommercialInfoLastModify = cminf.Field<DateTime>("CommercialInfoLastModify"),
                                  CommercialInfoCreateDate = cminf.Field<DateTime>("CommercialInfoCreateDate"),
                              } into cminfg
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = cminfg.Key.CommercialInfoId,
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = cminfg.Key.CommercialInfoTypeId,
                                      ItemName = cminfg.Key.CommercialInfoTypeName,
                                  },
                                  Value = cminfg.Key.Value,
                                  LargeValue = cminfg.Key.LargeValue,
                                  ValueName = string.Join(";",
                                    (from cminfvn in response.DataTableResult.AsEnumerable()
                                     where !cminfvn.IsNull("CommercialInfoId") &&
                                            cminfvn.Field<int>("CommercialInfoId") == cminfg.Key.CommercialInfoId &&
                                            !cminfvn.IsNull("ValueName")
                                     select cminfvn.Field<string>("ValueName")).ToList()),
                                  Enable = cminfg.Key.CommercialInfoEnable,
                                  LastModify = cminfg.Key.CommercialInfoLastModify,
                                  CreateDate = cminfg.Key.CommercialInfoCreateDate,
                              }).ToList(),

                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region Certification

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

        public List<GenericItemModel> CertificationGetBasicInfo(string CompanyPublicId, int? CertificationType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationType", CertificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

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
                                  ValueName = coinf.Field<string>("ValueName"),
                                  Enable = coinf.Field<UInt64>("CertificationInfoEnable") == 1 ? true : false,
                                  LastModify = coinf.Field<DateTime>("CertificationInfoLastModify"),
                                  CreateDate = coinf.Field<DateTime>("CertificationInfoCreateDate"),
                              }).ToList(),

                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region Financial

        public int FinancialUpsert(string CompanyPublicId, int? FinancialId, int FinancialTypeId, string FinancialName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialId", FinancialId));
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

        public List<GenericItemModel> FinancialGetBasicInfo(string CompanyPublicId, int? FinancialType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialType", FinancialType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CP_Financial_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from fi in response.DataTableResult.AsEnumerable()
                     where !fi.IsNull("FinancialId")
                     group fi by new
                     {
                         FinancialId = fi.Field<int>("FinancialId"),
                         FinancialTypeId = fi.Field<int>("FinancialTypeId"),
                         FinancialTypeName = fi.Field<string>("FinancialTypeName"),
                         FinancialName = fi.Field<string>("FinancialName"),
                         FinancialEnable = fi.Field<UInt64>("FinancialEnable") == 1 ? true : false,
                         FinancialLastModify = fi.Field<DateTime>("FinancialLastModify"),
                         FinancialCreateDate = fi.Field<DateTime>("FinancialCreateDate"),
                     } into fig
                     select new GenericItemModel()
                     {
                         ItemId = fig.Key.FinancialId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = fig.Key.FinancialTypeId,
                             ItemName = fig.Key.FinancialTypeName
                         },
                         ItemName = fig.Key.FinancialName,
                         Enable = fig.Key.FinancialEnable,
                         LastModify = fig.Key.FinancialLastModify,
                         CreateDate = fig.Key.FinancialCreateDate,
                         ItemInfo =
                             (from fiinf in response.DataTableResult.AsEnumerable()
                              where !fiinf.IsNull("FinancialInfoId") &&
                                      fiinf.Field<int>("FinancialId") == fig.Key.FinancialId
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = fiinf.Field<int>("FinancialInfoId"),
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = fiinf.Field<int>("FinancialInfoTypeId"),
                                      ItemName = fiinf.Field<string>("FinancialInfoTypeName"),
                                  },
                                  Value = fiinf.Field<string>("Value"),
                                  LargeValue = fiinf.Field<string>("LargeValue"),
                                  Enable = fiinf.Field<UInt64>("FinancialInfoEnable") == 1 ? true : false,
                                  LastModify = fiinf.Field<DateTime>("FinancialInfoLastModify"),
                                  CreateDate = fiinf.Field<DateTime>("FinancialInfoCreateDate"),
                              }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<BalanceSheetDetailModel> BalanceSheetGetByFinancial(int FinancialId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialId", FinancialId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CP_BalanceSheet_GetByFinancial",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<BalanceSheetDetailModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from bs in response.DataTableResult.AsEnumerable()
                     where !bs.IsNull("BalanceSheetId")
                     select new BalanceSheetDetailModel()
                     {
                         BalanceSheetId = bs.Field<int>("BalanceSheetId"),
                         RelatedAccount = new GenericItemModel()
                         {
                             ItemId = bs.Field<int>("AccountId"),
                             ItemName = bs.Field<string>("AccountName"),
                         },
                         Value = bs.Field<decimal>("Value"),
                         Enable = bs.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = bs.Field<DateTime>("LastModify"),
                         CreateDate = bs.Field<DateTime>("CreateDate"),
                     }).ToList();
            }
            return oReturn;
        }

        public List<BalanceSheetDetailModel> BalanceSheetGetCompanyAverage(string CompanyPublicId, int Year)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vYear", Year));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CP_BalanceSheet_GetCompanyAverage",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<BalanceSheetDetailModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from bs in response.DataTableResult.AsEnumerable()
                     where !bs.IsNull("AccountId")
                     select new BalanceSheetDetailModel()
                     {
                         RelatedAccount = new GenericItemModel()
                         {
                             ItemId = bs.Field<int>("AccountId"),
                             ItemName = bs.Field<string>("AccountName"),
                         },
                         Value = bs.Field<decimal>("Value"),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region Legal

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
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalInfoTypeId", LegalInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_LegalInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<GenericItemModel> LegalGetBasicInfo(string CompanyPublicId, int? LegalType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalType", LegalType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

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
                                  ValueName = coinf.Field<string>("ValueName"),
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

        #region Black List

        public int BlackListInsert(string CompanyPublicId, int BlackListStatus, string User, string FileUrl)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vBlackListStatus", BlackListStatus));
            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));
            lstParams.Add(DataInstance.CreateTypedParameter("vFileUrl", FileUrl));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_BlackList_Insert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int BlackListInfoInsert(int BlackListId, string BlackListInfoType, string Value)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vBlackListId", BlackListId));
            lstParams.Add(DataInstance.CreateTypedParameter("vBlackListInfoType", BlackListInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_BlackListInfo_Insert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<BlackListModel> BlackListGetBasicInfo(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_BlackList_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<BlackListModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from l in response.DataTableResult.AsEnumerable()
                     where !l.IsNull("BlackListId")
                     group l by new
                     {
                         BlackListId = l.Field<int>("BlackListId"),
                         BlackListSatusTypeId = l.Field<int>("BlackListSatusTypeId"),
                         BlackListSatusTypeName = l.Field<string>("BlackListSatusTypeName"),
                         User = l.Field<string>("User"),
                         FileUrl = l.Field<string>("FileUrl"),

                         BlackListCreateDate = l.Field<DateTime>("BlackListCreateDate"),
                     }
                         into cog
                     select new BlackListModel()
                     {
                         BlackListId = cog.Key.BlackListId,
                         BlackListStatus = new CatalogModel()
                         {
                             ItemId = cog.Key.BlackListSatusTypeId,
                             ItemName = cog.Key.BlackListSatusTypeName,
                         },
                         User = cog.Key.User,
                         FileUrl = cog.Key.FileUrl,
                         CreateDate = cog.Key.BlackListCreateDate,
                         BlackListInfo =
                             (from coinf in response.DataTableResult.AsEnumerable()
                              where !coinf.IsNull("BlackListInfoId") &&
                                      coinf.Field<int>("BlackListId") == cog.Key.BlackListId
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = coinf.Field<int>("BlackListinfoId"),
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemName = coinf.Field<string>("BlacklistInfoType"),
                                  },
                                  Value = coinf.Field<string>("Value"),
                                  CreateDate = coinf.Field<DateTime>("CreateDate"),
                              }).ToList(),

                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region Aditional Documents

        public int AditionalDocumentUpsert(string CompanyPublicId, int? AditionalDocumentId, int AditionalDocumentTypeId, string AditionalDocumentName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentId", AditionalDocumentId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentTypeId", AditionalDocumentTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentName", AditionalDocumentName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_AditionalDocument_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int AditionalDocumentInfoUpsert(int? AditionalDocumentId, int? AditionalDocumentInfoId, int AditionalDocumentInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();
            
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentId", AditionalDocumentId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentInfoId", AditionalDocumentInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentInfoTypeId", AditionalDocumentInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_AditionalDocumnetInfo_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> AditionalDocumentGetBasicInfo(string CompanyPublicId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CP_AditionalDocument_GetBasicInfo",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("AditionalDocumentId")
                     group c by new
                     {
                         AditionalDocumentId = c.Field<int>("AditionalDocumentId"),
                         AditionalDocumentTypeId = c.Field<int>("AditionalDocumentTypeId"),
                         AditionalDocumentTypeName = c.Field<string>("AditionalDocumentTypeName"),
                         AditionalDocumentName = c.Field<string>("AditionalDocumentName"),
                         AditionalDocumentEnable = c.Field<UInt64>("AditionalDocumentEnable") == 1 ? true : false,
                         AditionalDocumentLastModify = c.Field<DateTime>("AditionalDocumentLastModify"),
                         AditionalDocumentCreateDate = c.Field<DateTime>("AditionalDocumentCreateDate"),
                     }
                         into cog
                     select new GenericItemModel()
                     {
                         ItemId = cog.Key.AditionalDocumentId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = cog.Key.AditionalDocumentTypeId,
                             ItemName = cog.Key.AditionalDocumentTypeName
                         },
                         ItemName = cog.Key.AditionalDocumentName,
                         Enable = cog.Key.AditionalDocumentEnable,
                         LastModify = cog.Key.AditionalDocumentLastModify,
                         CreateDate = cog.Key.AditionalDocumentCreateDate,
                         ItemInfo =
                             (from coinf in response.DataTableResult.AsEnumerable()
                              where !coinf.IsNull("AditionalDocumentInfoId") &&
                                      coinf.Field<int>("AditionalDocumentId") == cog.Key.AditionalDocumentId
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = coinf.Field<int>("AditionalDocumentInfoId"),
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = coinf.Field<int>("AditionalDocumentInfoTypeId"),
                                      ItemName = coinf.Field<string>("AditionalDocumentInfoTypeName"),
                                  },
                                  Value = coinf.Field<string>("Value"),
                                  LargeValue = coinf.Field<string>("LargeValue"),
                                  Enable = coinf.Field<UInt64>("AditionalDocumentInfoEnable") == 1 ? true : false,
                                  LastModify = coinf.Field<DateTime>("AditionalDocumentInfoLastModify"),
                                  CreateDate = coinf.Field<DateTime>("AditionalDocumentInfoCreateDate"),
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

        #region MarketPlace

        #region SearchProviders

        public List<ProviderModel> MPProviderSearch(string CustomerPublicId, string SearchParam, string SearchFilter, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchFilter", SearchFilter));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchOrderType", SearchOrderType));
            lstParams.Add(DataInstance.CreateTypedParameter("vOrderOrientation", OrderOrientation));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Provider_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<ProviderModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from p in response.DataTableResult.AsEnumerable()
                     where !p.IsNull("CompanyPublicId")
                     group p by new
                     {
                         CompanyPublicId = p.Field<string>("CompanyPublicId"),
                         CompanyName = p.Field<string>("CompanyName"),
                         IdentificationTypeId = p.Field<int>("IdentificationTypeId"),
                         IdentificationTypeName = p.Field<string>("IdentificationTypeName"),
                         IdentificationNumber = p.Field<string>("IdentificationNumber"),
                         CompanyTypeId = p.Field<int>("CompanyTypeId"),
                         CompanyTypeName = p.Field<string>("CompanyTypeName"),
                     } into pg
                     select new ProviderModel()
                     {
                         RelatedCompany = new Company.Models.Company.CompanyModel()
                         {
                             CompanyPublicId = pg.Key.CompanyPublicId,
                             CompanyName = pg.Key.CompanyName,
                             IdentificationType = new CatalogModel()
                             {
                                 ItemId = pg.Key.IdentificationTypeId,
                                 ItemName = pg.Key.IdentificationTypeName,
                             },
                             IdentificationNumber = pg.Key.IdentificationNumber,
                             CompanyType = new CatalogModel()
                             {
                                 ItemId = pg.Key.CompanyTypeId,
                                 ItemName = pg.Key.CompanyTypeName
                             },

                             CompanyInfo =
                                 (from pi in response.DataTableResult.AsEnumerable()
                                  where !pi.IsNull("CompanyInfoId") && pi.Field<string>("CompanyPublicId") == pg.Key.CompanyPublicId
                                  group pi by new
                                  {
                                      CompanyInfoId = pi.Field<int>("CompanyInfoId"),
                                      CompanyInfoTypeId = pi.Field<int>("CompanyInfoTypeId"),
                                      CompanyInfoTypeName = pi.Field<string>("CompanyInfoTypeName"),
                                      CompanyInfoValue = pi.Field<string>("CompanyInfoValue"),
                                      CompanyInfoLargeValue = pi.Field<string>("CompanyInfoLargeValue"),
                                  } into pig
                                  select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                  {
                                      ItemInfoId = pig.Key.CompanyInfoId,
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemId = pig.Key.CompanyInfoTypeId,
                                          ItemName = pig.Key.CompanyInfoTypeName,
                                      },
                                      Value = pig.Key.CompanyInfoValue,
                                      LargeValue = pig.Key.CompanyInfoLargeValue,
                                  }).ToList(),
                         },
                         RelatedCustomerInfo =
                            (from rc in response.DataTableResult.AsEnumerable()
                             where !rc.IsNull("CustomerProviderId") && rc.Field<string>("CompanyPublicId") == pg.Key.CompanyPublicId
                             group rc by new
                             {
                                 CustomerPublicId = rc.Field<string>("CustomerPublicId"),
                                 CustomerProviderId = rc.Field<int>("CustomerProviderId"),
                                 CustomerProviderStatusId = rc.Field<int>("CustomerProviderStatusId"),
                                 CustomerProviderStatusName = rc.Field<string>("CustomerProviderStatusName"),
                             } into rcg
                             select new
                             {
                                 oKey = rcg.Key.CustomerPublicId,
                                 oValue = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                 {
                                     ItemId = rcg.Key.CustomerProviderId,
                                     ItemType = new CatalogModel()
                                     {
                                         ItemId = rcg.Key.CustomerProviderStatusId,
                                         ItemName = rcg.Key.CustomerProviderStatusName,
                                     },
                                     ItemInfo =
                                        (from rci in response.DataTableResult.AsEnumerable()
                                         where !rci.IsNull("CustomerProviderInfoId") && rci.Field<int>("CustomerProviderId") == rcg.Key.CustomerProviderId
                                         group rci by new
                                         {
                                             CustomerProviderInfoId = rci.Field<int>("CustomerProviderInfoId"),
                                             CustomerProviderInfoTypeId = rci.Field<int>("CustomerProviderInfoTypeId"),
                                             CustomerProviderInfoTypeName = rci.Field<string>("CustomerProviderInfoTypeName"),
                                             CustomerProviderInfoValue = rci.Field<string>("CustomerProviderInfoValue"),
                                             CustomerProviderInfoLargeValue = rci.Field<string>("CustomerProviderInfoLargeValue"),
                                         } into rcig
                                         select new GenericItemInfoModel()
                                         {
                                             ItemInfoId = rcig.Key.CustomerProviderInfoId,
                                             ItemInfoType = new CatalogModel()
                                             {
                                                 ItemId = rcig.Key.CustomerProviderInfoTypeId,
                                                 ItemName = rcig.Key.CustomerProviderInfoTypeName,
                                             },
                                             Value = rcig.Key.CustomerProviderInfoValue,
                                             LargeValue = rcig.Key.CustomerProviderInfoLargeValue,
                                         }).ToList()
                                 }
                             }).ToDictionary(k => k.oKey, v => v.oValue),
                     }).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> MPProviderSearchNew(string CustomerPublicId, bool OtherProviders, string SearchParam, string SearchFilter, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vOtherProviders", OtherProviders == true ? 1 : 0));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchFilter", SearchFilter));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchOrderType", SearchOrderType));
            lstParams.Add(DataInstance.CreateTypedParameter("vOrderOrientation", OrderOrientation == true ? 1 : 0));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Provider_Search_New",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProviderModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from p in response.DataTableResult.AsEnumerable()
                     where !p.IsNull("CompanyPublicId")
                     group p by new
                     {
                         CompanyPublicId = p.Field<string>("CompanyPublicId"),
                         CompanyName = p.Field<string>("CompanyName"),
                         IdentificationTypeId = p.Field<int>("IdentificationTypeId"),
                         IdentificationTypeName = p.Field<string>("IdentificationTypeName"),
                         IdentificationNumber = p.Field<string>("IdentificationNumber"),
                         CompanyTypeId = p.Field<int>("CompanyTypeId"),
                         CompanyTypeName = p.Field<string>("CompanyTypeName"),
                     } into pg
                     select new ProviderModel()
                     {
                         RelatedCompany = new Company.Models.Company.CompanyModel()
                         {
                             CompanyPublicId = pg.Key.CompanyPublicId,
                             CompanyName = pg.Key.CompanyName,
                             IdentificationType = new CatalogModel()
                             {
                                 ItemId = pg.Key.IdentificationTypeId,
                                 ItemName = pg.Key.IdentificationTypeName,
                             },
                             IdentificationNumber = pg.Key.IdentificationNumber,
                             CompanyType = new CatalogModel()
                             {
                                 ItemId = pg.Key.CompanyTypeId,
                                 ItemName = pg.Key.CompanyTypeName
                             },

                             CompanyInfo =
                                 (from pi in response.DataTableResult.AsEnumerable()
                                  where !pi.IsNull("CompanyInfoId") && pi.Field<string>("CompanyPublicId") == pg.Key.CompanyPublicId
                                  group pi by new
                                  {
                                      CompanyInfoId = pi.Field<int>("CompanyInfoId"),
                                      CompanyInfoTypeId = pi.Field<int>("CompanyInfoTypeId"),
                                      CompanyInfoTypeName = pi.Field<string>("CompanyInfoTypeName"),
                                      CompanyInfoValue = pi.Field<string>("CompanyInfoValue"),
                                      CompanyInfoLargeValue = pi.Field<string>("CompanyInfoLargeValue"),
                                  } into pig
                                  select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                  {
                                      ItemInfoId = pig.Key.CompanyInfoId,
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemId = pig.Key.CompanyInfoTypeId,
                                          ItemName = pig.Key.CompanyInfoTypeName,
                                      },
                                      Value = pig.Key.CompanyInfoValue,
                                      LargeValue = pig.Key.CompanyInfoLargeValue,
                                  }).ToList(),
                         },
                         RelatedCustomerInfo =
                            (from rc in response.DataTableResult.AsEnumerable()
                             where !rc.IsNull("CustomerProviderId") && rc.Field<string>("CompanyPublicId") == pg.Key.CompanyPublicId
                             group rc by new
                             {
                                 CustomerPublicId = rc.Field<string>("CustomerPublicId"),
                                 CustomerProviderId = rc.Field<int>("CustomerProviderId"),
                                 CustomerProviderStatusId = rc.Field<int>("CustomerProviderStatusId"),
                                 CustomerProviderStatusName = rc.Field<string>("CustomerProviderStatusName"),
                             } into rcg
                             select new
                             {
                                 oKey = rcg.Key.CustomerPublicId,
                                 oValue = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                 {
                                     ItemId = rcg.Key.CustomerProviderId,
                                     ItemType = new CatalogModel()
                                     {
                                         ItemId = rcg.Key.CustomerProviderStatusId,
                                         ItemName = rcg.Key.CustomerProviderStatusName,
                                     },
                                     ItemInfo =
                                        (from rci in response.DataTableResult.AsEnumerable()
                                         where !rci.IsNull("CustomerProviderInfoId") && rci.Field<int>("CustomerProviderId") == rcg.Key.CustomerProviderId
                                         group rci by new
                                         {
                                             CustomerProviderInfoId = rci.Field<int>("CustomerProviderInfoId"),
                                             CustomerProviderInfoTypeId = rci.Field<int>("CustomerProviderInfoTypeId"),
                                             CustomerProviderInfoTypeName = rci.Field<string>("CustomerProviderInfoTypeName"),
                                             CustomerProviderInfoValue = rci.Field<string>("CustomerProviderInfoValue"),
                                             CustomerProviderInfoLargeValue = rci.Field<string>("CustomerProviderInfoLargeValue"),
                                         } into rcig
                                         select new GenericItemInfoModel()
                                         {
                                             ItemInfoId = rcig.Key.CustomerProviderInfoId,
                                             ItemInfoType = new CatalogModel()
                                             {
                                                 ItemId = rcig.Key.CustomerProviderInfoTypeId,
                                                 ItemName = rcig.Key.CustomerProviderInfoTypeName,
                                             },
                                             Value = rcig.Key.CustomerProviderInfoValue,
                                             LargeValue = rcig.Key.CustomerProviderInfoLargeValue,
                                         }).ToList()
                                 }
                             }).ToDictionary(k => k.oKey, v => v.oValue),
                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericFilterModel> MPProviderSearchFilter(string CustomerPublicId, string SearchParam, string SearchFilter)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchFilter", SearchFilter));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Provider_SearchFilter",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericFilterModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from sf in response.DataTableResult.AsEnumerable()
                     where !sf.IsNull("FilterTypeId")
                     select new GenericFilterModel()
                     {
                         FilterType = new GenericItemModel()
                         {
                             ItemId = (int)sf.Field<Int64>("FilterTypeId"),
                             ItemName = sf.Field<string>("FilterTypeName"),
                         },
                         FilterValue = new GenericItemModel()
                         {
                             ItemId = Convert.ToInt32(sf.Field<string>("FilterValueId")),
                             ItemName = sf.Field<string>("FilterValueName"),
                         },
                         Quantity = Convert.ToInt32(sf.Field<Int64>("Quantity")),
                         CustomerPublicId = sf.Field<string>("CustomerPublicId"),
                     }).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> MPProviderSearchFilterNew(string CustomerPublicId, string SearchParam, string SearchFilter, bool OtherProviders)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchFilter", SearchFilter));
            lstParams.Add(DataInstance.CreateTypedParameter("vOtherProviders", OtherProviders == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Provider_SearchFilter_New",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericFilterModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from sf in response.DataTableResult.AsEnumerable()
                     where !sf.IsNull("FilterTypeId")
                     select new GenericFilterModel()
                     {
                         FilterType = new GenericItemModel()
                         {
                             ItemId = (int)sf.Field<Int64>("FilterTypeId"),
                             ItemName = sf.Field<string>("FilterTypeName"),
                         },
                         FilterValue = new GenericItemModel()
                         {
                             ItemId = Convert.ToInt32(sf.Field<string>("FilterValueId")),
                             ItemName = sf.Field<string>("FilterValueName"),
                         },
                         Quantity = Convert.ToInt32(sf.Field<Int64>("Quantity")),
                         CustomerPublicId = sf.Field<string>("CustomerPublicId"),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        public List<ProviderModel> MPProviderSearchById(string CustomerPublicId, string lstProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vlstProviderPublicId", lstProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Provider_SearchById",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<ProviderModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from p in response.DataTableResult.AsEnumerable()
                     where !p.IsNull("CompanyPublicId")
                     group p by new
                     {
                         CompanyPublicId = p.Field<string>("CompanyPublicId"),
                         CompanyName = p.Field<string>("CompanyName"),
                         IdentificationTypeId = p.Field<int>("IdentificationTypeId"),
                         IdentificationTypeName = p.Field<string>("IdentificationTypeName"),
                         IdentificationNumber = p.Field<string>("IdentificationNumber"),
                         CompanyTypeId = p.Field<int>("CompanyTypeId"),
                         CompanyTypeName = p.Field<string>("CompanyTypeName"),
                     } into pg
                     select new ProviderModel()
                     {
                         RelatedCompany = new Company.Models.Company.CompanyModel()
                         {
                             CompanyPublicId = pg.Key.CompanyPublicId,
                             CompanyName = pg.Key.CompanyName,
                             IdentificationType = new CatalogModel()
                             {
                                 ItemId = pg.Key.IdentificationTypeId,
                                 ItemName = pg.Key.IdentificationTypeName,
                             },
                             IdentificationNumber = pg.Key.IdentificationNumber,
                             CompanyType = new CatalogModel()
                             {
                                 ItemId = pg.Key.CompanyTypeId,
                                 ItemName = pg.Key.CompanyTypeName
                             },

                             CompanyInfo =
                                 (from pi in response.DataTableResult.AsEnumerable()
                                  where !pi.IsNull("CompanyInfoId") && pi.Field<string>("CompanyPublicId") == pg.Key.CompanyPublicId
                                  group pi by new
                                  {
                                      CompanyInfoId = pi.Field<int>("CompanyInfoId"),
                                      CompanyInfoTypeId = pi.Field<int>("CompanyInfoTypeId"),
                                      CompanyInfoTypeName = pi.Field<string>("CompanyInfoTypeName"),
                                      CompanyInfoValue = pi.Field<string>("CompanyInfoValue"),
                                      CompanyInfoLargeValue = pi.Field<string>("CompanyInfoLargeValue"),
                                  } into pig
                                  select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                  {
                                      ItemInfoId = pig.Key.CompanyInfoId,
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemId = pig.Key.CompanyInfoTypeId,
                                          ItemName = pig.Key.CompanyInfoTypeName,
                                      },
                                      Value = pig.Key.CompanyInfoValue,
                                      LargeValue = pig.Key.CompanyInfoLargeValue,
                                  }).ToList(),
                         },
                         RelatedCustomerInfo =
                            (from rc in response.DataTableResult.AsEnumerable()
                             where !rc.IsNull("CustomerProviderId") && rc.Field<string>("CompanyPublicId") == pg.Key.CompanyPublicId
                             group rc by new
                             {
                                 CustomerPublicId = rc.Field<string>("CustomerPublicId"),
                                 CustomerProviderId = rc.Field<int>("CustomerProviderId"),
                                 CustomerProviderStatusId = rc.Field<int>("CustomerProviderStatusId"),
                                 CustomerProviderStatusName = rc.Field<string>("CustomerProviderStatusName"),
                             } into rcg
                             select new
                             {
                                 oKey = rcg.Key.CustomerPublicId,
                                 oValue = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                 {
                                     ItemId = rcg.Key.CustomerProviderId,
                                     ItemType = new CatalogModel()
                                     {
                                         ItemId = rcg.Key.CustomerProviderStatusId,
                                         ItemName = rcg.Key.CustomerProviderStatusName,
                                     },
                                     ItemInfo =
                                        (from rci in response.DataTableResult.AsEnumerable()
                                         where !rci.IsNull("CustomerProviderInfoId") && rci.Field<int>("CustomerProviderId") == rcg.Key.CustomerProviderId
                                         group rci by new
                                         {
                                             CustomerProviderInfoId = rci.Field<int>("CustomerProviderInfoId"),
                                             CustomerProviderInfoTypeId = rci.Field<int>("CustomerProviderInfoTypeId"),
                                             CustomerProviderInfoTypeName = rci.Field<string>("CustomerProviderInfoTypeName"),
                                             CustomerProviderInfoValue = rci.Field<string>("CustomerProviderInfoValue"),
                                             CustomerProviderInfoLargeValue = rci.Field<string>("CustomerProviderInfoLargeValue"),
                                         } into rcig
                                         select new GenericItemInfoModel()
                                         {
                                             ItemInfoId = rcig.Key.CustomerProviderInfoId,
                                             ItemInfoType = new CatalogModel()
                                             {
                                                 ItemId = rcig.Key.CustomerProviderInfoTypeId,
                                                 ItemName = rcig.Key.CustomerProviderInfoTypeName,
                                             },
                                             Value = rcig.Key.CustomerProviderInfoValue,
                                             LargeValue = rcig.Key.CustomerProviderInfoLargeValue,
                                         }).ToList()
                                 }
                             }).ToDictionary(k => k.oKey, v => v.oValue),
                     }).ToList();
            }
            return oReturn;
        }

        public Company.Models.Company.CompanyModel MPCompanyGetBasicInfo(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_C_Company_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            Company.Models.Company.CompanyModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = response.DataTableResult.Rows[0].Field<string>("CompanyPublicId"),
                    CompanyName = response.DataTableResult.Rows[0].Field<string>("CompanyName"),
                    IdentificationType = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = response.DataTableResult.Rows[0].Field<int>("IdentificationTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("IdentificationTypeName"),
                    },
                    IdentificationNumber = response.DataTableResult.Rows[0].Field<string>("IdentificationNumber"),
                    CompanyType = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = response.DataTableResult.Rows[0].Field<int>("CompanyTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("CompanyTypeName"),
                    },
                    Enable = response.DataTableResult.Rows[0].Field<UInt64>("CompanyEnable") == 1 ? true : false,
                    LastModify = response.DataTableResult.Rows[0].Field<DateTime>("CompanyLastModify"),
                    CreateDate = response.DataTableResult.Rows[0].Field<DateTime>("CompanyCreateDate"),

                    CompanyInfo =
                        (from ci in response.DataTableResult.AsEnumerable()
                         where !ci.IsNull("CompanyInfoId")
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                         {
                             ItemInfoId = ci.Field<int>("CompanyInfoId"),
                             ItemInfoType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = ci.Field<int>("CompanyInfoTypeId"),
                                 ItemName = ci.Field<string>("CompanyInfoTypeName"),
                             },
                             Value = ci.Field<string>("Value"),
                             LargeValue = ci.Field<string>("LargeValue"),
                             Enable = ci.Field<UInt64>("CompanyInfoEnable") == 1 ? true : false,
                             LastModify = ci.Field<DateTime>("CompanyInfoLastModify"),
                             CreateDate = ci.Field<DateTime>("CompanyInfoCreateDate"),
                         }).ToList(),
                };
            }
            return oReturn;
        }

        public List<GenericItemModel> MPContactGetBasicInfo(string CompanyPublicId, int? ContactType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vContactType", ContactType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_C_Contact_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from co in response.DataTableResult.AsEnumerable()
                     where !co.IsNull("ContactId")
                     group co by new
                     {
                         ContactId = co.Field<int>("ContactId"),
                         ContactTypeId = co.Field<int>("ContactTypeId"),
                         ContactTypeName = co.Field<string>("ContactTypeName"),
                         ContactName = co.Field<string>("ContactName"),
                         ContactEnable = co.Field<UInt64>("ContactEnable") == 1 ? true : false,
                         ContactLastModify = co.Field<DateTime>("ContactLastModify"),
                         ContactCreateDate = co.Field<DateTime>("ContactCreateDate"),
                     } into cog
                     select new GenericItemModel()
                     {
                         ItemId = cog.Key.ContactId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = cog.Key.ContactTypeId,
                             ItemName = cog.Key.ContactTypeName
                         },
                         ItemName = cog.Key.ContactName,
                         Enable = cog.Key.ContactEnable,
                         LastModify = cog.Key.ContactLastModify,
                         CreateDate = cog.Key.ContactCreateDate,
                         ItemInfo =
                             (from coinf in response.DataTableResult.AsEnumerable()
                              where !coinf.IsNull("ContactInfoId") &&
                                      coinf.Field<int>("ContactId") == cog.Key.ContactId
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = coinf.Field<int>("ContactInfoId"),
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = coinf.Field<int>("ContactInfoTypeId"),
                                      ItemName = coinf.Field<string>("ContactInfoTypeName"),
                                  },
                                  Value = coinf.Field<string>("Value"),
                                  LargeValue = coinf.Field<string>("LargeValue"),
                                  Enable = coinf.Field<UInt64>("ContactInfoEnable") == 1 ? true : false,
                                  LastModify = coinf.Field<DateTime>("ContactInfoLastModify"),
                                  CreateDate = coinf.Field<DateTime>("ContactInfoCreateDate"),
                              }).ToList(),

                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericItemModel> MPCommercialGetBasicInfo(string CompanyPublicId, int? CommercialType, string CustomerPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCommercialType", CommercialType));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Commercial_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cm in response.DataTableResult.AsEnumerable()
                     where !cm.IsNull("CommercialId")
                     group cm by new
                     {
                         CommercialId = cm.Field<int>("CommercialId"),
                         CommercialTypeId = cm.Field<int>("CommercialTypeId"),
                         CommercialTypeName = cm.Field<string>("CommercialTypeName"),
                         CommercialName = cm.Field<string>("CommercialName"),
                         CommercialEnable = cm.Field<UInt64>("CommercialEnable") == 1 ? true : false,
                         CommercialLastModify = cm.Field<DateTime>("CommercialLastModify"),
                         CommercialCreateDate = cm.Field<DateTime>("CommercialCreateDate"),
                     } into cmg
                     select new GenericItemModel()
                     {
                         ItemId = cmg.Key.CommercialId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = cmg.Key.CommercialTypeId,
                             ItemName = cmg.Key.CommercialTypeName
                         },
                         ItemName = cmg.Key.CommercialName,
                         Enable = cmg.Key.CommercialEnable,
                         LastModify = cmg.Key.CommercialLastModify,
                         CreateDate = cmg.Key.CommercialCreateDate,
                         ItemInfo =
                             (from cminf in response.DataTableResult.AsEnumerable()
                              where !cminf.IsNull("CommercialInfoId") &&
                                      cminf.Field<int>("CommercialId") == cmg.Key.CommercialId
                              group cminf by new
                              {
                                  CommercialInfoId = cminf.Field<int>("CommercialInfoId"),
                                  CommercialInfoTypeId = cminf.Field<int>("CommercialInfoTypeId"),
                                  CommercialInfoTypeName = cminf.Field<string>("CommercialInfoTypeName"),
                                  Value = cminf.Field<string>("Value"),
                                  LargeValue = cminf.Field<string>("LargeValue"),
                                  CommercialInfoEnable = cminf.Field<UInt64>("CommercialInfoEnable") == 1 ? true : false,
                                  CommercialInfoLastModify = cminf.Field<DateTime>("CommercialInfoLastModify"),
                                  CommercialInfoCreateDate = cminf.Field<DateTime>("CommercialInfoCreateDate"),
                              } into cminfg
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = cminfg.Key.CommercialInfoId,
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = cminfg.Key.CommercialInfoTypeId,
                                      ItemName = cminfg.Key.CommercialInfoTypeName,
                                  },
                                  Value = cminfg.Key.Value,
                                  LargeValue = cminfg.Key.LargeValue,
                                  ValueName = string.Join(";",
                                    (from cminfvn in response.DataTableResult.AsEnumerable()
                                     where !cminfvn.IsNull("CommercialInfoId") &&
                                            cminfvn.Field<int>("CommercialInfoId") == cminfg.Key.CommercialInfoId &&
                                            !cminfvn.IsNull("ValueName")
                                     select cminfvn.Field<string>("ValueName")).ToList()),
                                  Enable = cminfg.Key.CommercialInfoEnable,
                                  LastModify = cminfg.Key.CommercialInfoLastModify,
                                  CreateDate = cminfg.Key.CommercialInfoCreateDate,
                              }).ToList(),

                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericItemModel> MPCertificationGetBasicInfo(string CompanyPublicId, int? CertificationType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationType", CertificationType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Certification_GetBasicInfo",
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
                                  ValueName = coinf.Field<string>("ValueName"),
                                  Enable = coinf.Field<UInt64>("CertificationInfoEnable") == 1 ? true : false,
                                  LastModify = coinf.Field<DateTime>("CertificationInfoLastModify"),
                                  CreateDate = coinf.Field<DateTime>("CertificationInfoCreateDate"),
                              }).ToList(),

                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericItemModel> MPFinancialGetBasicInfo(string CompanyPublicId, int? FinancialType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialType", FinancialType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Financial_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from fi in response.DataTableResult.AsEnumerable()
                     where !fi.IsNull("FinancialId")
                     group fi by new
                     {
                         FinancialId = fi.Field<int>("FinancialId"),
                         FinancialTypeId = fi.Field<int>("FinancialTypeId"),
                         FinancialTypeName = fi.Field<string>("FinancialTypeName"),
                         FinancialName = fi.Field<string>("FinancialName"),
                         FinancialEnable = fi.Field<UInt64>("FinancialEnable") == 1 ? true : false,
                         FinancialLastModify = fi.Field<DateTime>("FinancialLastModify"),
                         FinancialCreateDate = fi.Field<DateTime>("FinancialCreateDate"),
                     } into fig
                     select new GenericItemModel()
                     {
                         ItemId = fig.Key.FinancialId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = fig.Key.FinancialTypeId,
                             ItemName = fig.Key.FinancialTypeName
                         },
                         ItemName = fig.Key.FinancialName,
                         Enable = fig.Key.FinancialEnable,
                         LastModify = fig.Key.FinancialLastModify,
                         CreateDate = fig.Key.FinancialCreateDate,
                         ItemInfo =
                             (from fiinf in response.DataTableResult.AsEnumerable()
                              where !fiinf.IsNull("FinancialInfoId") &&
                                      fiinf.Field<int>("FinancialId") == fig.Key.FinancialId
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = fiinf.Field<int>("FinancialInfoId"),
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = fiinf.Field<int>("FinancialInfoTypeId"),
                                      ItemName = fiinf.Field<string>("FinancialInfoTypeName"),
                                  },
                                  Value = fiinf.Field<string>("Value"),
                                  LargeValue = fiinf.Field<string>("LargeValue"),
                                  Enable = fiinf.Field<UInt64>("FinancialInfoEnable") == 1 ? true : false,
                                  LastModify = fiinf.Field<DateTime>("FinancialInfoLastModify"),
                                  CreateDate = fiinf.Field<DateTime>("FinancialInfoCreateDate"),
                              }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<BalanceSheetModel> MPBalanceSheetGetByYear(string CompanyPublicId, int? Year)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vYear", Year));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_BalanceSheet_GetByYear",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<BalanceSheetModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from fi in response.DataTableResult.AsEnumerable()
                     where !fi.IsNull("FinancialId")
                     group fi by new
                     {
                         FinancialId = fi.Field<int>("FinancialId"),
                     } into fig
                     select new BalanceSheetModel()
                     {
                         ItemId = fig.Key.FinancialId,
                         BalanceSheetInfo =
                            (from bsd in response.DataTableResult.AsEnumerable()
                             where !bsd.IsNull("BalanceSheetId") &&
                                    bsd.Field<int>("FinancialId") == fig.Key.FinancialId
                             group bsd by new
                             {
                                 BalanceSheetId = bsd.Field<int>("BalanceSheetId"),
                                 AccountId = bsd.Field<int>("AccountId"),
                                 AccountName = bsd.Field<string>("AccountName"),
                                 Value = bsd.Field<decimal>("Value"),
                                 AccountUnit = bsd.Field<string>("AccountUnit"),
                             } into bsdg
                             select new BalanceSheetDetailModel()
                             {
                                 BalanceSheetId = bsdg.Key.BalanceSheetId,
                                 RelatedAccount = new GenericItemModel()
                                 {
                                     ItemId = bsdg.Key.AccountId,
                                     ItemName = bsdg.Key.AccountName,
                                     ItemInfo = new List<GenericItemInfoModel>()
                                     {
                                        new GenericItemInfoModel()
                                        {
                                            ItemInfoType = new CatalogModel()
                                            {
                                                ItemId = 109007
                                            },
                                            Value = bsdg.Key.AccountUnit
                                        }
                                     },
                                 },
                                 Value = bsdg.Key.Value,
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericItemModel> MPLegalGetBasicInfo(string CompanyPublicId, int? LegalType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalType", LegalType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Legal_GetBasicInfo",
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

        public List<GenericItemModel> MPCustomerProviderGetTracking(string CustomerPublicId, string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_CustomerProvider_GetTracking",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from bl in response.DataTableResult.AsEnumerable()
                     where !bl.IsNull("CustomerProviderInfoId")
                     group bl by new
                     {
                         CustomerProviderInfoId = bl.Field<int>("CustomerProviderInfoId"),
                         Seguimiento = bl.Field<string>("Seguimiento"),
                         CreateDate = bl.Field<DateTime>("CreateDate"),
                         Status = bl.Field<string>("Status"),
                     } into blg
                     select new GenericItemModel()
                     {
                         ItemId = blg.Key.CustomerProviderInfoId,
                         ItemType = new CatalogModel()
                         {
                             ItemName = blg.Key.Status
                         },
                         CreateDate = blg.Key.CreateDate,
                         ItemName = blg.Key.Seguimiento,
                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericItemModel> MPFinancialGetLastyearInfoDeta(string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Financial_GetLastyearInfoDeta",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from bl in response.DataTableResult.AsEnumerable()
                     where !bl.IsNull("CompanyId")
                     group bl by new
                     {
                         CompanyId = bl.Field<int>("CompanyId"),
                         Year = bl.Field<string>("Year"),
                     } into blg
                     select new GenericItemModel()
                     {
                         ItemId = blg.Key.CompanyId,
                         ItemName = blg.Key.Year,
                         ItemInfo =
                              (from blinf in response.DataTableResult.AsEnumerable()
                               where !blinf.IsNull("FinancialInfoId") &&
                                       blinf.Field<int>("CompanyId") == blg.Key.CompanyId
                               select new GenericItemInfoModel()
                               {
                                   ItemInfoId = blinf.Field<int>("FinancialInfoId"),
                                   ItemInfoType = new CatalogModel()
                                   {
                                       ItemId = blinf.Field<int>("Account"),
                                   },
                                   Value = blinf.Field<decimal>("Value").ToString(),
                                   ValueName = blinf.Field<string>("Currency"),
                               }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericItemInfoModel> MPCertificationGetSpecificCert(string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_CertificationGetSpecificCert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemInfoModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cerinf in response.DataTableResult.AsEnumerable()
                     where !cerinf.IsNull("CompanyId")
                     select new GenericItemInfoModel()
                     {
                         ItemInfoId = cerinf.Field<int>("CompanyId"),
                         ItemInfoType = new CatalogModel()
                         {
                             ItemId = cerinf.Field<int>("CategoryId"),
                             ItemName = cerinf.Field<string>("CategoryName"),
                         },
                         Value = cerinf.Field<string>("CCS"),
                         LargeValue = cerinf.Field<string>("IsCertified"),
                     }).ToList();
            }
            return oReturn;
        }

        public GenericItemModel MPCustomerProviderGetAllTracking(string CustomerPublicId, string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_CustomerProvider_GetAllTracking",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            GenericItemModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from tr in response.DataTableResult.AsEnumerable()
                     where !tr.IsNull("CustomerProviderId")
                     group tr by new
                     {
                         CustomerProviderId = tr.Field<int>("CustomerProviderId"),
                         StatusName = tr.Field<string>("StatusName"),
                     }
                         into trg
                     select new GenericItemModel()
                     {
                         ItemId = trg.Key.CustomerProviderId,
                         ItemType = new CatalogModel()
                         {
                             ItemName = trg.Key.StatusName
                         },
                         ItemInfo =
                             (from trinf in response.DataTableResult.AsEnumerable()
                              where !trinf.IsNull("CustomerProviderInfoId") &&
                                    trinf.Field<int>("CustomerProviderId") == trg.Key.CustomerProviderId
                              group trinf by new
                              {
                                  CustomerProviderInfoId = trinf.Field<int>("CustomerProviderInfoId"),
                                  Tracking = trinf.Field<string>("Tracking"),
                                  CreateDate = trinf.Field<DateTime>("CreateDate"),
                              }
                                  into trinfg
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = trinfg.Key.CustomerProviderInfoId,
                                  LargeValue = trinfg.Key.Tracking,
                                  CreateDate = trinfg.Key.CreateDate,
                              }).ToList()
                     }).FirstOrDefault();
            }
            return oReturn;
        }

        public List<GenericItemModel> MPReportGetBasicInfo(string CompanyPublicId, int? ReportType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vReportType", ReportType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Report_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from l in response.DataTableResult.AsEnumerable()
                     where !l.IsNull("ReportId")
                     group l by new
                     {
                         ReportId = l.Field<int>("ReportId"),
                         ReportTypeId = l.Field<int>("ReportTypeId"),
                         ReportTypeName = l.Field<string>("ReportTypeName"),
                         ReportName = l.Field<string>("ReportName"),
                         ReportEnable = l.Field<UInt64>("ReportEnable") == 1 ? true : false,
                         ReportLastModify = l.Field<DateTime>("ReportLastModify"),
                         ReportCreateDate = l.Field<DateTime>("ReportCreateDate"),
                     }
                         into cog
                     select new GenericItemModel()
                     {
                         ItemId = cog.Key.ReportId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = cog.Key.ReportTypeId,
                             ItemName = cog.Key.ReportTypeName
                         },
                         ItemName = cog.Key.ReportName,
                         Enable = cog.Key.ReportEnable,
                         LastModify = cog.Key.ReportLastModify,
                         CreateDate = cog.Key.ReportCreateDate,
                         ItemInfo =
                             (from coinf in response.DataTableResult.AsEnumerable()
                              where !coinf.IsNull("ReportInfoId") &&
                                      coinf.Field<int>("ReportId") == cog.Key.ReportId
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = coinf.Field<int>("ReportInfoId"),
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = coinf.Field<int>("ReportInfoTypeId"),
                                      ItemName = coinf.Field<string>("ReportInfoTypeName"),
                                  },
                                  Value = coinf.Field<string>("Value"),
                                  LargeValue = coinf.Field<string>("LargeValue"),
                                  Enable = coinf.Field<UInt64>("ReportInfoEnable") == 1 ? true : false,
                                  LastModify = coinf.Field<DateTime>("ReportInfoLastModify"),
                                  CreateDate = coinf.Field<DateTime>("ReportInfoCreateDate"),
                              }).ToList(),

                     }).ToList();
            }
            return oReturn;
        }

        public int MPReportUpsert(string CompanyPublicId, int? ReportId, int ReportTypeId, string ReportName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vReportId", ReportId));
            lstParams.Add(DataInstance.CreateTypedParameter("vReportTypeId", ReportTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vReportName", ReportName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_Report_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int MPReportInfoUpsert(int ReportId, int? ReportInfoId, int ReportInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vReportId", ReportId));
            lstParams.Add(DataInstance.CreateTypedParameter("vReportInfoId", ReportInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vReportInfoTypeId", ReportInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_ReportInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<GenericItemModel> MPAditionalDocumentGetBasicInfo(string CustomerPublicId, string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_AditionalDocument_GetBasicInfo",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("AditionalDocumentId")
                     group c by new
                     {
                         AditionalDocumentId = c.Field<int>("AditionalDocumentId"),
                         AditionalDocumentTypeId = c.Field<int>("AditionalDocumentTypeId"),
                         AditionalDocumentTypeName = c.Field<string>("AditionalDocumentTypeName"),
                         AditionalDocumentName = c.Field<string>("AditionalDocumentName"),
                         AditionalDocumentEnable = c.Field<UInt64>("AditionalDocumentEnable") == 1 ? true : false,
                         AditionalDocumentLastModify = c.Field<DateTime>("AditionalDocumentLastModify"),
                         AditionalDocumentCreateDate = c.Field<DateTime>("AditionalDocumentCreateDate"),
                     }
                         into cog
                     select new GenericItemModel()
                     {
                         ItemId = cog.Key.AditionalDocumentId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = cog.Key.AditionalDocumentTypeId,
                             ItemName = cog.Key.AditionalDocumentTypeName
                         },
                         ItemName = cog.Key.AditionalDocumentName,
                         Enable = cog.Key.AditionalDocumentEnable,
                         LastModify = cog.Key.AditionalDocumentLastModify,
                         CreateDate = cog.Key.AditionalDocumentCreateDate,
                         ItemInfo =
                             (from coinf in response.DataTableResult.AsEnumerable()
                              where !coinf.IsNull("AditionalDocumentInfoId") &&
                                      coinf.Field<int>("AditionalDocumentId") == cog.Key.AditionalDocumentId
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = coinf.Field<int>("AditionalDocumentInfoId"),
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = coinf.Field<int>("AditionalDocumentInfoTypeId"),
                                      ItemName = coinf.Field<string>("AditionalDocumentInfoTypeName"),
                                  },
                                  Value = coinf.Field<string>("Value"),
                                  LargeValue = coinf.Field<string>("LargeValue"),
                                  Enable = coinf.Field<UInt64>("AditionalDocumentInfoEnable") == 1 ? true : false,
                                  LastModify = coinf.Field<DateTime>("AditionalDocumentInfoLastModify"),
                                  CreateDate = coinf.Field<DateTime>("AditionalDocumentInfoCreateDate"),
                              }).ToList(),

                     }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region BatchProcess

        /// <summary>
        /// Get all providers to calculate the Recruitment K
        /// </summary>
        /// <returns>ProviderPublicId List</returns>
        public List<ProviderModel> BPGetRecruitmentProviders()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "BP_CP_Financial_GetRecruitmentProviders",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            List<ProviderModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from p in response.DataTableResult.AsEnumerable()
                     where !p.IsNull("CompanyPublicId")
                     group p by new
                     {
                         CompanyPublicId = p.Field<string>("CompanyPublicId"),
                     } into pg
                     select new ProviderModel()
                     {
                         RelatedCompany = new Company.Models.Company.CompanyModel()
                         {
                             CompanyPublicId = pg.Key.CompanyPublicId,
                         },
                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region Charts

        public List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetProvidersByState(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_ProvidersCharts_GetByState",
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
                        Title = "Estado de mis proveedores",
                        ItemType = sv.Field<string>("StateId"),
                        ItemName = sv.Field<string>("State"),
                        Count = (int)sv.Field<Int64>("Count"),
                    }).ToList();
            }

            return oReturn;
        }

        #endregion        
    }
}
