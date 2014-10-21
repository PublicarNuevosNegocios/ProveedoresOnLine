using DocumentManagement.Provider.Interfaces;
using DocumentManagement.Provider.Models;
using DocumentManagement.Provider.Models.Provider;
using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.DAL.MySQLDAO
{
    internal class Provider_MySqlDao : IProviderData
    {
        private ADO.Interfaces.IADO DataInstance;
        public Provider_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Constants.C_DMProviderConnectionName);
        }

        //public string ProviderUpsert(string CustomerPublicId, string ProviderPublicId, string Name, Enumerations.enumIdentificationType IdentificationType, DocumentManagement.Provider.Models.Enumerations.enumProviderCustomerInfoType CustomerProviderInfoType, string IdentificationNumber, string Email, Enumerations.enumProcessStatus Status)
        //{
        //    List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

        //    lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", IdentificationType));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vCustomerProviderInfoType", CustomerProviderInfoType));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vEmail", Email));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vStatus", (int)Status));

        //    ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
        //    {
        //        CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
        //        CommandText = "P_Provider_UpSert",
        //        CommandType = System.Data.CommandType.StoredProcedure,
        //        Parameters = lstParams
        //    });

        //    return response.ScalarResult.ToString();
        //}

        //public string ProviderInfoUpsert(int ProviderInfoId, string ProviderPublicId, Enumerations.enumProviderInfoType ProviderInfoType, string Value, string LargeValue)
        //{
        //    List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

        //    lstParams.Add(DataInstance.CreateTypedParameter("vProviderInfoId", ProviderInfoId));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vProviderInfoType", (int)ProviderInfoType));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

        //    ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
        //    {
        //        CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
        //        CommandText = "P_ProviderInfo_Upsert",
        //        CommandType = System.Data.CommandType.StoredProcedure,
        //        Parameters = lstParams
        //    });

        //    return null;
        //}

        //public string ProviderCustomerInfoUpsert(int ProviderCustomerInfoId, string ProviderPublicId, string CustomerPublicId, Enumerations.enumProviderCustomerInfoType ProviderCustomerInfoType, string Value, string LargeValue)
        //{
        //    List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

        //    lstParams.Add(DataInstance.CreateTypedParameter("vProviderCustomerInfoId", ProviderCustomerInfoId));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vProviderCustomerInfoType", (int)ProviderCustomerInfoType));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

        //    ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
        //    {
        //        CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
        //        CommandText = "P_ProviderCustomerInfo_Upsert",
        //        CommandType = System.Data.CommandType.StoredProcedure,
        //        Parameters = lstParams
        //    });

        //    return null;
        //}

        //public Models.Provider.ProviderModel GetProviderByIdentificationNumberAndDocumentType(string IdentificationNumber, Enumerations.enumIdentificationType IdenificationType)
        //{
        //    List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();
        //    lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", (int)IdenificationType));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));

        //    ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
        //    {
        //        CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
        //        CommandText = "P_Provider_GetByIdNumberAndIdType",
        //        CommandType = System.Data.CommandType.StoredProcedure,
        //        Parameters = lstParams
        //    });

        //    ProviderModel oReturn = null;
        //    if (response.DataTableResult != null &&
        //        response.DataTableResult.Rows.Count > 0)
        //    {
        //        oReturn = new ProviderModel()
        //        {
        //            IdentificationNumber = response.DataTableResult.Rows[0].Field<string>("IdentificationNumber"),
        //            IdentificationType = new Models.Util.CatalogModel()
        //            {
        //                ItemId = response.DataTableResult.Rows[0].Field<int>("IdentificationType"),
        //                ItemName = response.DataTableResult.Rows[0].Field<string>("IdentificationTypeName"),
        //            },
        //            Email = response.DataTableResult.Rows[0].Field<string>("Email"),
        //            Name = response.DataTableResult.Rows[0].Field<string>("Name"),
        //            LastModify = response.DataTableResult.Rows[0].Field<DateTime>("LastModify"),
        //            CreateDate = response.DataTableResult.Rows[0].Field<DateTime>("CreateDate")
        //        };
        //    }
        //    return oReturn;
        //}

        //public bool GetRelationProviderAndCustomer(string CustomerPublicId, string ProviderPublicId)
        //{
        //    List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();
        //    lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
        //    lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

        //    ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
        //    {
        //        CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
        //        CommandText = "P_ProviderCustomer_GetByCustomerAndProvider",
        //        CommandType = System.Data.CommandType.StoredProcedure,
        //        Parameters = lstParams
        //    });

        //    if (response.DataTableResult != null &&
        //        response.DataTableResult.Rows.Count > 0)
        //        return true;
        //    else            
        //        return false;           
        //}

        public string ProviderUpsert(string ProviderPublicId, string Name, int IdentificationTypeId, string IdentificationNumber, string Email)
        {
            throw new NotImplementedException();
        }

        public int ProviderInfoUpsert(string ProviderPublicId, int? ProviderInfoId, int ProviderInfoTypeId, string Value, string LargeValue)
        {
            throw new NotImplementedException();
        }

        public int ProviderCustomerInfoUpsert(string ProviderPublicId, string CustomerPublicId, int? ProviderCustomerInfoId, int ProviderCustomerInfoTypeId, string Value, string LargeValue)
        {
            throw new NotImplementedException();
        }

        public List<ProviderModel> ProviderSearch(string SearchParam, int PageNumber, int RowCount)
        {
            throw new NotImplementedException();
        }

        public ProviderModel ProviderGetByIdentification(string IdentificationNumber, int IdenificationTypeId, string CustomerPublicId)
        {
            throw new NotImplementedException();
        }

        public ProviderModel ProviderGetById(string ProviderPublicId, int? StepId)
        {
            throw new NotImplementedException();
        }
    }
}
