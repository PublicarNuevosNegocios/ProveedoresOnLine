using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Interfaces;
using ProveedoresOnLine.RestrictiveListProcess.Models;
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
    }
}
