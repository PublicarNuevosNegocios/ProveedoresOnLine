using IntegrationPlatform.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationPlatform.Models.Integration;
using ProveedoresOnLine.Company.Models.Company;
using System.Data;

namespace IntegrationPlatform.DAL.MySQLDAO
{
    internal class IntegrationPlatform_MySqlDao : IIntegrationPlatformData
    {
        private ADO.Interfaces.IADO DataInstance;

        public IntegrationPlatform_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(IntegrationPlatform.Models.Constants.C_POL_IntegrationPlatformConnectionName);
        }

        #region Integration
        
        public CustomDataModel CustomerProvider_GetCustomData(CompanyModel Customer, string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", Customer.CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "IP_CustomerRedirect_GetAll",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            Models.Integration.CustomDataModel oReturn = new CustomDataModel();

            if (response.DataSetResult.Tables[0] != null &&
                response.DataSetResult.Tables[0].Rows.Count > 0)
            {
                oReturn.CustomField =
                    (from af in response.DataSetResult.Tables[0].AsEnumerable()
                     where !af.IsNull("AditionalFieldId")
                     select new Models.Integration.CustomFieldModel()
                     {
                         AditionalFieldId = af.Field<int>("AditionalFieldId"),
                         AditionalFieldType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                         {
                             ItemId = af.Field<int?>("AditionalFieldTypeId") != null ? af.Field<int>("AditionalFieldTypeId") : 0,
                             ItemName = af.Field<string>("AditionalFieldTypeName"),
                         },
                         AditionalFieldTypeInfo = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                         {
                             ItemId = af.Field<int?>("AditionalFieldTypeInfoId") != null ? af.Field<int>("AditionalFieldTypeInfoId") : 0,
                             ItemName = af.Field<string>("AditionalFieldTypeInfoName"),
                         },
                         Label = af.Field<string>("Label"),
                         Enable = af.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = af.Field<DateTime>("LastModify"),
                         CreateDate = af.Field<DateTime>("CreateDate"),
                     }).ToList();
            }

            if (response.DataSetResult.Tables[1] != null &&
                response.DataSetResult.Tables[1].Rows.Count > 0)
            {
                oReturn.CustomData =
                    (from ad in response.DataSetResult.Tables[1].AsEnumerable()
                     where !ad.IsNull("AditionalDataId")
                     group ad by new
                     {
                         AditionalDataId = ad.Field<int>("AditionalDataId"),
                         ProviderId = ad.Field<int>("ProviderId"),
                         AditionalFieldId = ad.Field<int>("AditionalFieldId"),
                         AditionalDataName = ad.Field<string>("AditionalDataName"),
                         AditionalDataEnable = ad.Field<UInt64>("AditionalDataEnable"),
                         AditionalDataLastModify = ad.Field<DateTime>("AditionalDataLastModify"),
                         AditionalDataCreateDate = ad.Field<DateTime>("AditionalDataCreateDate"),
                     }
                     into adg
                     select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                     {
                         ItemId = adg.Key.AditionalDataId,
                         ItemName = adg.Key.AditionalDataName,
                         ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                         {
                             ItemId = adg.Key.AditionalFieldId,
                         },
                         Enable = adg.Key.AditionalDataEnable == 1 ? true : false,
                         LastModify = adg.Key.AditionalDataLastModify,
                         CreateDate = adg.Key.AditionalDataCreateDate,
                         ItemInfo =
                            (from adinf in response.DataSetResult.Tables[1].AsEnumerable()
                             where !adinf.IsNull("AditionalDataInfoId") &&
                                    adinf.Field<int>("AditionalDataId") == adg.Key.AditionalDataId
                             select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                             {
                                 ItemInfoId = adinf.Field<int>("AditionalDataInfoId"),
                                 ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                 {
                                     ItemId = adinf.Field<int?>("AditionalDataInfoType") != null ? adinf.Field<int>("AditionalDataInfoType") : 0,
                                 },
                                 Value = adinf.Field<string>("AditionalDataInfoValue"),
                                 LargeValue = adinf.Field<string>("AditionalDataInfoLargeValue"),
                                 Enable = adinf.Field<UInt64>("AditionalDataInfoEnable") == 1 ? true : false,
                                 LastModify = adinf.Field<DateTime>("AditionalDataInfoLastModify"),
                                 CreateDate = adinf.Field<DateTime>("AditionalDataInfoCreateDate"),
                             }).ToList(),
                     }).ToList();
            }

            oReturn.RelatedCompany = Customer;

            return oReturn;
        }

        #region Integration Sanofi

        public int Sanofi_AditionalDataInfo_Upsert(int AditionalDataInfoId, int AditionalDataId, int? AditionalDataInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDataInfoId", AditionalDataInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDataId", AditionalDataId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDataInfoType", AditionalDataInfoType != 0 ? AditionalDataInfoType : null));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "Sanofi_AditionalDataInfo_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int Sanofi_AditionalData_Upsert(int AditionalDataId, string ProviderPublicId, int AditionalFieldId, string AditionalDataName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDataId", AditionalDataId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalFieldId", AditionalFieldId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDataName", AditionalDataName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "Sanofi_AditionalData_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> CatalogGetSanofiOptions()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "Sanofi_Catalog_GetProviderOptions",
                CommandType = CommandType.StoredProcedure,
            });

            List<ProveedoresOnLine.Company.Models.Util.CatalogModel> oReturn = new List<ProveedoresOnLine.Company.Models.Util.CatalogModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("CatalogId")
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

        #region Publicar

        public int Publicar_AditionalDataInfo_Upsert(int AditionalDataInfoId, int AditionalDataId, int? AditionalDataInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDataInfoId", AditionalDataInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDataId", AditionalDataId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDataInfoType", AditionalDataInfoType != 0 ? AditionalDataInfoType : null));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "Publicar_AditionalDataInfo_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int Publicar_AditionalData_Upsert(int AditionalDataId, string ProviderPublicId, int AditionalFieldId, string AditionalDataName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDataId", AditionalDataId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalFieldId", AditionalFieldId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDataName", AditionalDataName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "Publicar_AditionalData_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion

        #endregion
    }
}
