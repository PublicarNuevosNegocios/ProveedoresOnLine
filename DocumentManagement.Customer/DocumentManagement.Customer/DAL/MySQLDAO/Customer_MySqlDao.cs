using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using DocumentManagement.Customer.Interfaces;
using DocumentManagement.Customer.Models;
using DocumentManagement.Customer.Models.Customer;
using DocumentManagement.Customer.Models.Form;
using DocumentManagement.Customer.Models.Util;

namespace DocumentManagement.Customer.DAL.MySQLDAO
{
    internal class Customer_MySqlDao : ICustomerData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Customer_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Constants.C_DMCustomerConnectionName);
        }

        #region Customer

        public string CustomerUpsert(string CustomerPublicId, string Name, int IdentificationType, string IdentificationNumber)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", IdentificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "C_Customer_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            if (response.ScalarResult != null)
                return response.ScalarResult.ToString();
            else
                return null;
        }

        public List<CustomerModel> CustomerSearch(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_Customer_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<CustomerModel> oReturn = new List<CustomerModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("CustomerPublicId")
                     select new CustomerModel()
                     {
                         CustomerPublicId = c.Field<string>("UserInfoId"),
                         Name = c.Field<string>("Name"),
                         IdentificationType = new Models.Util.CatalogModel()
                         {
                             ItemId = c.Field<int>("IdentificationTypeId"),
                             ItemName = c.Field<string>("IdentificationTypeName"),
                         },
                         IdentificationNumber = c.Field<string>("IdentificationNumber"),
                     }).ToList();
            }

            return oReturn;
        }

        public CustomerModel CustomerGetById(string CustomerPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_Customer_GetById",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            CustomerModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new CustomerModel()
                {
                    CustomerPublicId = response.DataTableResult.Rows[0].Field<string>("UserInfoId"),
                    Name = response.DataTableResult.Rows[0].Field<string>("Name"),
                    IdentificationType = new Models.Util.CatalogModel()
                    {
                        ItemId = response.DataTableResult.Rows[0].Field<int>("IdentificationTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("IdentificationTypeName"),
                    },
                    IdentificationNumber = response.DataTableResult.Rows[0].Field<string>("IdentificationNumber"),

                    RelatedForm =
                        (from f in response.DataTableResult.AsEnumerable()
                         where !f.IsNull("FormPublicId")
                         group f by new
                         {
                             FormPublicId = f.Field<string>("FormPublicId"),
                             FormName = f.Field<string>("FormName"),
                             TermsAndConditions = f.Field<string>("TermsAndConditions"),
                             Logo = f.Field<string>("Logo"),
                         } into fg
                         select new FormModel()
                         {
                             FormPublicId = fg.Key.FormPublicId,
                             Name = fg.Key.FormName,
                             TermsAndConditions = fg.Key.TermsAndConditions,
                             Logo = fg.Key.Logo,
                             RelatedStep =
                                (from s in response.DataTableResult.AsEnumerable()
                                 where !s.IsNull("StepId") &&
                                         s.Field<string>("FormPublicId") == fg.Key.FormPublicId
                                 group s by new
                                 {
                                     StepId = s.Field<int>("StepId"),
                                     StepName = s.Field<string>("StepName"),
                                     StepPosition = s.Field<int>("StepPosition"),
                                 } into sg
                                 select new StepModel()
                                 {
                                     StepId = sg.Key.StepId,
                                     Name = sg.Key.StepName,
                                     Position = sg.Key.StepPosition,
                                     RelatedField =
                                        (from fi in response.DataTableResult.AsEnumerable()
                                         where !fi.IsNull("FieldId") &&
                                                fi.Field<int>("StepId") == sg.Key.StepId
                                         group fi by new
                                         {
                                             FieldId = fi.Field<int>("FieldId"),
                                             FieldName = fi.Field<string>("FieldName"),
                                             ProviderInfoTypeId = fi.Field<int>("ProviderInfoTypeId"),
                                             ProviderInfoTypeName = fi.Field<string>("ProviderInfoTypeName"),
                                             FieldTypeId = fi.Field<int>("FieldTypeId"),
                                             FieldTypeName = fi.Field<string>("FieldTypeName"),
                                             IsRequired = fi.Field<bool>("IsRequired"),
                                             FieldPosition = fi.Field<int>("FieldPosition"),
                                         } into fig
                                         select new FieldModel()
                                         {
                                             FieldId = fig.Key.FieldId,
                                             Name = fig.Key.FieldName,
                                             ProviderInfoType = new CatalogModel()
                                             {
                                                 ItemId = fig.Key.ProviderInfoTypeId,
                                                 ItemName = fig.Key.ProviderInfoTypeName,
                                             },
                                             FieldType = new CatalogModel()
                                             {
                                                 ItemId = fig.Key.FieldTypeId,
                                                 ItemName = fig.Key.FieldTypeName,
                                             },
                                             IsRequired = fig.Key.IsRequired,
                                             Position = fig.Key.FieldPosition,
                                         }).ToList(),
                                 }).ToList(),
                         }).ToList(),
                };
            }

            return oReturn;
        }

        #endregion

        #region Form

        public string FormUpsert(string FormPublicId, string CustomerPublicId, string Name, string TermsAndConditions, string Logo)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFormPublicId", FormPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vTermsAndConditions", TermsAndConditions));
            lstParams.Add(DataInstance.CreateTypedParameter("vLogo", Logo));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "F_Form_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            if (response.ScalarResult != null)
                return response.ScalarResult.ToString();
            else
                return null;
        }

        public int StepCreate(string FormPublicId, string Name, int Position)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFormPublicId", FormPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vPosition", Position));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "F_Step_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public void StepDelete(int StepId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vStepId", StepId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "F_Step_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public int FieldCreate(int StepId, string Name, int ProviderInfoType, int FieldType, bool IsRequired, int Position)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vStepId", StepId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderInfoType", ProviderInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vFieldType", FieldType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIsRequired", IsRequired));
            lstParams.Add(DataInstance.CreateTypedParameter("vPosition", Position));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "F_Field_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public void FieldDelete(int FieldId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFieldId", FieldId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "F_Field_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        #endregion
    }
}
