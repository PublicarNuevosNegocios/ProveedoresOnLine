using ProveedoresOnLine.AsociateProvider.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ProveedoresOnLine.AsociateProvider.Client.DAL.MySQLDAO
{
    internal class AsociateProviderClient_MySqlDao : IAsociateProviderClientData
    {
        private ADO.Interfaces.IADO DataInstance;

        public AsociateProviderClient_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.AsociateProvider.Client.Models.Constants.POL_AsociateProviderClientConnectionName);
        }

        #region AsociateProvider

        public int DMProviderUpsert(string ProviderPublicId, string ProviderName, string IdentificationType, string IdentificationNumber)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderName", ProviderName));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", IdentificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "AP_DMProviderUpsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            int oReturn = 0;

            if (response.ScalarResult != null)
            {
                oReturn = Convert.ToInt32(response.ScalarResult);
            }

            return oReturn;
        }

        public int BOProviderUpsert(string ProviderPublicId, string ProviderName, string IdentificationType, string IdentificationNumber)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderName", ProviderName));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", IdentificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "AP_BOProviderUpsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            int oReturn = 0;

            if (response.ScalarResult != null)
            {
                oReturn = Convert.ToInt32(response.ScalarResult);
            }

            return oReturn;
        }

        public void AP_AsociateProviderUpsert(string BOProviderPublicId, string DMProviderPublicId, string Email)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vDM_ProviderPublicId", DMProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vBO_ProviderPublicId", BOProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vUserEmail", Email));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "AP_AsociateProviderUpsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });            
        }

        public ProveedoresOnLine.AsociateProvider.Client.Models.HomologateModel GetHomologateItemBySourceID(Int32 SourceCode)
        {

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();
            lstParams.Add(DataInstance.CreateTypedParameter("vSource", SourceCode));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "AP_GetHomologateItemBySourceId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            ProveedoresOnLine.AsociateProvider.Client.Models.HomologateModel oReturn = null;

            if (response.DataTableResult != null &&
               response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new ProveedoresOnLine.AsociateProvider.Client.Models.HomologateModel()
                {
                    HomologateId = response.DataTableResult.Rows[0].Field<int>("HomologateId"),
                    HomologateType = new ProveedoresOnLine.AsociateProvider.Client.Models.CatalogModel() 
                    {
                        CatalogId = response.DataTableResult.Rows[0].Field<int>("HomologateTypecatalogId"),
                        CatalogName = response.DataTableResult.Rows[0].Field<string>("HomologateTypecatalogName"),
                        ItemId = response.DataTableResult.Rows[0].Field<int>("HomologateTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("HomologateTypeName"),
                    },

                    Source = new ProveedoresOnLine.AsociateProvider.Client.Models.CatalogModel()
                    {

                        CatalogId = response.DataTableResult.Rows[0].Field<int>("SourceCatalogId"),
                        CatalogName = response.DataTableResult.Rows[0].Field<string>("SourceCatalogName"),
                        ItemId = response.DataTableResult.Rows[0].Field<int>("SourceTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("SourceTypeName"),
                    },

                    Target = new ProveedoresOnLine.AsociateProvider.Client.Models.CatalogModel()
                    {

                        CatalogId = response.DataTableResult.Rows[0].Field<int>("TargetCatalogId"),
                        CatalogName = response.DataTableResult.Rows[0].Field<string>("TargetCatalogName"),
                        ItemId = response.DataTableResult.Rows[0].Field<int>("TargetTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("TargetTypeName"),
                    },

                    CreateDate = response.DataTableResult.Rows[0].Field<DateTime>("CreateDate"),
                    LastModify = response.DataTableResult.Rows[0].Field<DateTime>("LastModify"),

                };                
            }
            
            return oReturn;
        }

        public ProveedoresOnLine.AsociateProvider.Client.Models.AsociateProviderModel GetAsociateProviderByProviderPublicId(string vProviderPublicIdDM, string vProviderPublicIdBO)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicIdDM", vProviderPublicIdDM));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicIdBO", vProviderPublicIdBO));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "AP_GetAsociateProviderByProviderPublicId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });
            ProveedoresOnLine.AsociateProvider.Client.Models.AsociateProviderModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ap in response.DataTableResult.AsEnumerable()
                     where !ap.IsNull("AsociateProviderId")
                     select new ProveedoresOnLine.AsociateProvider.Client.Models.AsociateProviderModel()
                     {
                         AsociateProviderId = ap.Field<int>("AsociateProviderId"),
                         RelatedProviderBO = new ProveedoresOnLine.AsociateProvider.Client.Models.RelatedProviderModel()
                         {
                             ProviderPublicId = ap.Field<string>("BO_ProviderPublicId"),
                             ProviderName = ap.Field<string>("BO_ProviderName"),
                             IdentificationType = ap.Field<int>("BO_IdentificationType").ToString(),
                             IdentificationNumber = ap.Field<string>("BO_IdentificationNumber"),
                         },
                         RelatedProviderDM = new ProveedoresOnLine.AsociateProvider.Client.Models.RelatedProviderModel()
                         {
                             ProviderPublicId = ap.Field<string>("DM_ProviderPublicId"),
                             ProviderName = ap.Field<string>("DM_ProviderName"),
                             IdentificationType = ap.Field<int>("DM_IdentificationType").ToString(),
                             IdentificationNumber = ap.Field<string>("DM_IdentificationNumber")
                         },
                         Email = ap.Field<string>("UserEmail"),
                         CreateDate = ap.Field<DateTime>("CreateDate"),
                         LastModify = ap.Field<DateTime>("LastModify"),
                     }).FirstOrDefault();
            }
            return oReturn;
        }

        #endregion        
    }
}
