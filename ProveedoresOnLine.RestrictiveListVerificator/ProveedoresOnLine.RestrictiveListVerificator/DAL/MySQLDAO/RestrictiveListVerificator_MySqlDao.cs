using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListVerificator.Interfaces;
using ProveedoresOnLine.RestrictiveListVerificator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProveedoresOnLine.RestrictiveListVerificator.DAL.MyQSLDAO
{
    internal class RestrictiveListVerificator_MySqlDao : IRestrictiveListVerificatorData
    {
        private ADO.Interfaces.IADO DataInstance;
        
        public RestrictiveListVerificator_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.RestrictiveListVerificator.Models.Constants.C_POL_CompanyProviderConnection);
        }

        public List<ProviderModel> GetProviderByStatus(int Status, string CustomerPublicId)
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
            
            List<ProviderModel> oReturn = null;


            /* Obtengo la informacion básica de todos los proveedores */
            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {
                oReturn.Add(new ProviderModel
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(CustomerPublicId),
                });                
            }
            

            List<ProviderStatusModel> oReturnProviders = null;
            
            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {
                oReturnProviders =
                    (
                        from cm in response.DataTableResult.AsEnumerable()
                        where !cm.IsNull("ProviderId")
                        group cm by new
                        {
                            CompanyId = cm.Field<string>("ProviderId"),
                            CompanyPublicId = cm.Field<string>("ProviderPublicId"),
                            CompanyName = cm.Field<string>("ProviderName"),
                            IdentificationTypeId = cm.Field<int>("ProviderIdentificationTypeId"),
                            IdentificationTypeName = cm.Field<string>("ProviderIdentificationTypeName"),
                            IdentificationNumber = cm.Field<string>("ProviderIdentificationNumber"),
                            StatusId = cm.Field<int>("StatusId"),
                            StatusName = cm.Field<string>("StatusName"),
                        } into cmg
                        select new ProviderStatusModel()
                        {
                            RelatedProvider = new Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = cmg.Key.CompanyPublicId,
                                CompanyName = cmg.Key.CompanyName,
                                IdentificationType = new Company.Models.Util.CatalogModel()
                                {
                                    ItemId = cmg.Key.IdentificationTypeId,
                                    ItemName = cmg.Key.IdentificationTypeName,
                                },
                                IdentificationNumber = cmg.Key.IdentificationNumber,
                            },
                            RelatedStatus = new Company.Models.Util.CatalogModel() { 
                                ItemId = cmg.Key.StatusId,
                                ItemName = cmg.Key.StatusName,
                            },
                        }).ToList();
            }

            return oReturn;
        }
    }
}
