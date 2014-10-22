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
                         CustomerPublicId = c.Field<string>("CustomerPublicId"),
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

        public CustomerModel CustomerGetByFormId(string FormPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFormPublicId", FormPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_Customer_GetByFormId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            CustomerModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new CustomerModel()
                {
                    CustomerPublicId = response.DataTableResult.Rows[0].Field<string>("CustomerPublicId"),
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
                                             IsRequired = fi.Field<UInt64>("IsRequired") == 1 ? true : false,
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
                                             IsRequired = fig.Key.IsRequired,
                                             Position = fig.Key.FieldPosition,
                                         }).ToList(),
                                 }).ToList(),
                         }).ToList(),
                };
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
                    CustomerPublicId = response.DataTableResult.Rows[0].Field<string>("CustomerPublicId"),
                    Name = response.DataTableResult.Rows[0].Field<string>("Name"),
                    IdentificationType = new Models.Util.CatalogModel()
                    {
                        ItemId = response.DataTableResult.Rows[0].Field<int>("IdentificationTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("IdentificationTypeName"),
                    },
                    IdentificationNumber = response.DataTableResult.Rows[0].Field<string>("IdentificationNumber"),
                };
            }

            return oReturn;
        }

        #endregion

        #region Form

        public string FormUpsert(string FormPublicId, string CustomerPublicId, string Name)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFormPublicId", FormPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));

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

        public void FormUpsertLogo(string FormPublicId, string Logo)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFormPublicId", FormPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLogo", Logo));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "F_Form_UpsertLogo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
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

        public void StepModify(int StepId, string Name, int Position)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vStepId", StepId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vPosition", Position));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "F_Step_Modify",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
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

        public int FieldCreate(int StepId, string Name, int ProviderInfoType, bool IsRequired, int Position)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vStepId", StepId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderInfoType", ProviderInfoType));
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

        public List<FormModel> FormSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "F_Form_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<FormModel> oReturn = new List<FormModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("FormPublicId")
                     select new FormModel()
                     {
                         FormPublicId = c.Field<string>("FormPublicId"),
                         Name = c.Field<string>("Name"),
                         Logo = c.Field<string>("Logo"),
                     }).ToList();
            }

            return oReturn;
        }

        public List<StepModel> StepGetByFormId(string FormPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFormPublicId", FormPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "F_Step_GetByFormId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<StepModel> oReturn = new List<StepModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from s in response.DataTableResult.AsEnumerable()
                     where !s.IsNull("StepId")
                     select new StepModel()
                     {
                         StepId = s.Field<int>("StepId"),
                         Name = s.Field<string>("StepName"),
                         Position = s.Field<int>("StepPosition"),
                     }).ToList();
            }
            return oReturn;
        }

        public List<FieldModel> FieldGetByStepId(int StepId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vStepId", StepId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "F_Field_GetByStepId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<FieldModel> oReturn = new List<FieldModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from fi in response.DataTableResult.AsEnumerable()
                     where !fi.IsNull("FieldId")
                     select new FieldModel()
                     {
                         FieldId = fi.Field<int>("FieldId"),
                         Name = fi.Field<string>("FieldName"),
                         ProviderInfoType = new CatalogModel()
                         {
                             ItemId = fi.Field<int>("ProviderInfoTypeId"),
                             ItemName = fi.Field<string>("ProviderInfoTypeName"),
                         },
                         IsRequired = fi.Field<UInt64>("IsRequired") == 1 ? true : false,
                         Position = fi.Field<int>("FieldPosition"),
                     }).ToList();
            }
            return oReturn;

        }

        #endregion

        #region Util

        public List<CatalogModel> CatalogGetCustomerOptions()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Catalog_GetCustomerOptions",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            List<CatalogModel> oReturn = new List<CatalogModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("ItemId")
                     select new CatalogModel()
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
