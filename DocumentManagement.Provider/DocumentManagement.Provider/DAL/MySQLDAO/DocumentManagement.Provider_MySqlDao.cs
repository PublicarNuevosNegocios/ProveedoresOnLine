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

        #region Comentario
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
        #endregion

        public string ProviderUpsert(string ProviderPublicId, string Name, int IdentificationTypeId, string IdentificationNumber, string Email)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationTypeId", IdentificationTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vEmail", Email));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_Provider_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
        }

        public int ProviderInfoUpsert(string ProviderPublicId, int? ProviderInfoId, int ProviderInfoTypeId, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderInfoId", ProviderInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderInfoTypeId", ProviderInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_Provider_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int ProviderCustomerInfoUpsert(string ProviderPublicId, string CustomerPublicId, int? ProviderCustomerInfoId, int ProviderCustomerInfoTypeId, string Value, string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderCustomerInfoId", ProviderCustomerInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderCustomerInfoTypeId", ProviderCustomerInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_ProviderCustomerInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<ProviderModel> ProviderSearch(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "P_Provider_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<ProviderModel> oReturn = new List<ProviderModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("ProviderPublicId")
                     select new ProviderModel()
                     {
                         CustomerPublicId = c.Field<string>("ProviderPublicId"),
                         Name = c.Field<string>("Name"),
                         IdentificationType = new Models.Util.CatalogModel()
                         {
                             ItemId = c.Field<int>("IdentificationTypeId"),
                             ItemName = c.Field<string>("IdentificationTypeName"),
                         },
                         IdentificationNumber = c.Field<string>("IdentificationNumber"),
                         FormPublicId = c.Field<string>("FormPublicId"),
                         FormName = c.Field<string>("FormName"),
                     }).ToList();
            }

            return oReturn;
        }

        public ProviderModel ProviderGetByIdentification(string IdentificationNumber, int IdenificationTypeId, string CustomerPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdenificationTypeId", IdenificationTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "P_Provider_GetByIdentification",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            ProviderModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new ProviderModel()
                    {
                        ProviderPublicId = response.DataTableResult.Rows[0].Field<string>("ProviderPublicId"),
                        Name = response.DataTableResult.Rows[0].Field<string>("Name"),
                        IdentificationType = new CatalogModel()
                        {
                            ItemId = response.DataTableResult.Rows[0].Field<int>("IdentificationTypeId"),
                            ItemName = response.DataTableResult.Rows[0].Field<string>("IdentificationTypeName"),
                        },
                        IdentificationNumber = response.DataTableResult.Rows[0].Field<string>("IdentificationNumber"),
                        Email = response.DataTableResult.Rows[0].Field<string>("Email"),

                        CustomerPublicId = response.DataTableResult.Rows[0].Field<string>("CustomerPublicId"),
                        CustomerName = response.DataTableResult.Rows[0].Field<string>("CustomerName"),

                        RelatedProviderCustomerInfo =
                            (from pci in response.DataTableResult.AsEnumerable()
                             where !pci.IsNull("ProviderCustomerInfoId")
                             select new ProviderInfoModel()
                             {
                                 ProviderInfoId = pci.Field<int>("ProviderCustomerInfoId"),
                                 ProviderInfoType = new CatalogModel()
                                 {
                                     ItemId = pci.Field<int>("ProviderCustomerInfoTypeId"),
                                     ItemName = pci.Field<string>("ProviderCustomerInfoTypeName"),
                                 },
                                 Value = pci.Field<string>("ProviderCustomerInfoValue"),
                                 LargeValue = pci.Field<string>("ProviderCustomerInfoLargeValue"),
                             }).ToList(),
                    };
            }

            return oReturn;
        }

        public ProviderModel ProviderGetById(string ProviderPublicId, int? StepId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vStepId", StepId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "P_Provider_GetById",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            ProviderModel oReturn = null;

            if (response.DataSetResult != null &&
                response.DataSetResult.Tables != null &&
                response.DataSetResult.Tables.Count > 0 &&
                response.DataSetResult.Tables[0].Rows.Count > 0)
            {
                oReturn = new ProviderModel()
                {
                    ProviderPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProviderPublicId"),
                    Name = response.DataSetResult.Tables[0].Rows[0].Field<string>("Name"),
                    IdentificationType = new CatalogModel()
                    {
                        ItemId = response.DataSetResult.Tables[0].Rows[0].Field<int>("IdentificationTypeId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("IdentificationTypeName"),
                    },
                    IdentificationNumber = response.DataSetResult.Tables[0].Rows[0].Field<string>("IdentificationNumber"),
                    Email = response.DataSetResult.Tables[0].Rows[0].Field<string>("Email"),

                    CustomerPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("CustomerPublicId"),
                    CustomerName = response.DataSetResult.Tables[0].Rows[0].Field<string>("CustomerName"),

                    RelatedProviderCustomerInfo =
                        (from pci in response.DataSetResult.Tables[0].AsEnumerable()
                         where !pci.IsNull("ProviderCustomerInfoId")
                         select new ProviderInfoModel()
                         {
                             ProviderInfoId = pci.Field<int>("ProviderCustomerInfoId"),
                             ProviderInfoType = new CatalogModel()
                             {
                                 ItemId = pci.Field<int>("ProviderCustomerInfoTypeId"),
                                 ItemName = pci.Field<string>("ProviderCustomerInfoTypeName"),
                             },
                             Value = pci.Field<string>("ProviderCustomerInfoValue"),
                             LargeValue = pci.Field<string>("ProviderCustomerInfoLargeValue"),
                         }).ToList(),

                    RelatedProviderInfo = (response.DataSetResult.Tables.Count <= 1 || response.DataSetResult.Tables[1].Rows.Count <= 0) ? new List<ProviderInfoModel>() :
                        (from pci in response.DataSetResult.Tables[1].AsEnumerable()
                         where !pci.IsNull("ProviderInfoId")
                         select new ProviderInfoModel()
                         {
                             ProviderInfoId = pci.Field<int>("ProviderInfoId"),
                             ProviderInfoType = new CatalogModel()
                             {
                                 ItemId = pci.Field<int>("ProviderInfoTypeId"),
                                 ItemName = pci.Field<string>("ProviderInfoTypeName"),
                             },
                             Value = pci.Field<string>("Value"),
                             LargeValue = pci.Field<string>("LargeValue"),
                         }).ToList(),
                };
            }

            return oReturn;
        }
    }
}
