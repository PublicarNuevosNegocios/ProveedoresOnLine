using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Interfaces;
using ProveedoresOnLine.RestrictiveListProcess.Models;
using ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.DAL.MySQLDAO
{
    internal class RestrictiveListProcess_MySqlDao : IRestrictiveListProcess
    {
        private ADO.Interfaces.IADO DataInstance;

        public RestrictiveListProcess_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.RestrictiveListProcess.Models.Constants.R_POL_RestrictiveListProcessConnectionName);
        }

        public List<CompanyModel> GetProviderByStatus(int Status, string CustomerPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", Status));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "BP_GetProviderByStatus",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<CompanyModel> oCompanyList = null;

            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {
                oCompanyList = (
                    from cm in response.DataTableResult.AsEnumerable()
                    where !cm.IsNull("ProviderId")
                    group cm by new
                         {
                             CompanyName = cm.Field<string>("ProviderName"),
                             CompanyPublicId = cm.Field<string>("ProviderPublicId"),
                             Enable = cm.Field<UInt64>("Enable") == 1 ? true : false,
                             IdentificationType = new ProveedoresOnLine.Company.Models.Util.CatalogModel() 
                             { 
                                 ItemId = cm.Field<int>("ProviderIdentificationTypeId"), 
                                 ItemName = cm.Field<string>("ProviderIdentificationTypeName") 
                             },
                             IdentificationNumber = cm.Field<string>("ProviderIdentificationNumber"),
                         } into cmg
                    select new CompanyModel()
                    {
                        CompanyName = cmg.Key.CompanyName,
                        CompanyPublicId = cmg.Key.CompanyPublicId,
                        Enable = cmg.Key.Enable,
                        IdentificationType = cmg.Key.IdentificationType,
                        IdentificationNumber = cmg.Key.IdentificationNumber
                    }
                 ).ToList();
            }
            return oCompanyList;
        }

        public List<RestrictiveListProcessModel> GetAllProvidersInProcess()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CP_BlackListFileProcess_GetAllInProcess",
                CommandType = System.Data.CommandType.StoredProcedure
            });

            List<RestrictiveListProcessModel>oProvidersInProcess = null;

            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {
                oProvidersInProcess = (
                        from cm in response.DataTableResult.AsEnumerable()
                        where !cm.IsNull("BlackListProcessId")
                        group cm by new
                                {
                                    //cm.Field<UInt64>("Enable") == 1 ? true : false,
                                    BlackListProcessId = cm.Field<UInt64>("UInt64"),
                                    FilePath = cm.Field<string>("FilePath"),
                                    ProcessStatus = cm.Field<UInt64>("ProcessStatus") == 1 ? true : false,
                                    IsSuccess = cm.Field<UInt64>("ProviderName") == 1 ? true : false,
                                    ProviderStatus = cm.Field<string>("IsSuccess"),
                                    Enable = cm.Field<UInt64>("Enable") == 1 ? true : false,
                                    LastModify = cm.Field<string>("LastModify"),
                                    CreateDate = cm.Field<string>("CreateDate"),
                                } into cmg

                        select new RestrictiveListProcessModel() {
                            BlackListProcessId = cmg.Key.BlackListProcessId,
                            FilePath = cmg.Key.FilePath,
                            ProcessStatus = cmg.Key.ProcessStatus,
                            IsSuccess = cmg.Key.IsSuccess,
                            ProviderStatus = cmg.Key.ProviderStatus,
                            Enable = cmg.Key.Enable,
                            LastModify = cmg.Key.LastModify,
                            CreateDate = cmg.Key.CreateDate
                        }
                    ).ToList();
            }

            return oProvidersInProcess;
        }

        public string BlackListProcessUpsert(int BlackListProcessId, string FilePath, bool ProcessStatus, bool IsSuccess, string ProviderStatus, bool Enable, string LastModify, string CreateDate)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();
            
            lstParams.Add(DataInstance.CreateTypedParameter("vBlackListProcessId", BlackListProcessId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFilePath", FilePath));
            lstParams.Add(DataInstance.CreateTypedParameter("vProcessStatus", ProcessStatus));
            lstParams.Add(DataInstance.CreateTypedParameter("vIsSuccess", IsSuccess));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderStatus", ProviderStatus));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));
            lstParams.Add(DataInstance.CreateTypedParameter("vLastModify", LastModify));
            lstParams.Add(DataInstance.CreateTypedParameter("vCreateDate", CreateDate));
            
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_BlackListProcess_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
        }

    }
}
