using ProveedoresOnLine.AsociateProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ProveedoresOnLine.AsociateProvider.DAL.MySqlDao
{
    public class AsociateProvider_MySqlDao : IAsociateProviderData
    {
        private ADO.Interfaces.IADO DataInstance;

        public AsociateProvider_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Interfaces.Constants.C_AsociateProviderConnectionName);
        }

        #region Asociate Provider

        public List<Interfaces.Models.AsociateProvider.AsociateProviderModel> GetAllAsociateProvider(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "AP_GetAllAsociateProviders",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            TotalRows = 0;
            List<Interfaces.Models.AsociateProvider.AsociateProviderModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");
                oReturn =
                    (from ap in response.DataTableResult.AsEnumerable()
                     where !ap.IsNull("AsociateProviderId")
                     select new AsociateProvider.Interfaces.Models.AsociateProvider.AsociateProviderModel()
                     {
                         AsociateProviderId = ap.Field<int>("AsociateProviderId"),
                         RelatedProviderBO = new Interfaces.Models.AsociateProvider.RelatedProviderModel()
                         {
                             ProviderPublicId = ap.Field<string>("BO_ProviderPublicId"),
                             ProviderName = ap.Field<string>("BO_ProviderName"),
                             IdentificationType = ap.Field<int>("BO_IdentificationType").ToString(),
                             IdentificationNumber = ap.Field<string>("BO_IdentificationNumber"),
                         },
                         RelatedProviderDM = new Interfaces.Models.AsociateProvider.RelatedProviderModel()
                         {
                             ProviderPublicId = ap.Field<string>("DM_ProviderPublicId"),
                             ProviderName = ap.Field<string>("DM_ProviderName"),
                             IdentificationType = ap.Field<int>("DM_IdentificationType").ToString(),
                             IdentificationNumber = ap.Field<string>("DM_IdentificationNumber")
                         },
                         Email = ap.Field<string>("UserEmail"),
                         CreateDate = ap.Field<DateTime>("CreateDate"),
                         LastModify = ap.Field<DateTime>("LastModify"),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion
    }
}
