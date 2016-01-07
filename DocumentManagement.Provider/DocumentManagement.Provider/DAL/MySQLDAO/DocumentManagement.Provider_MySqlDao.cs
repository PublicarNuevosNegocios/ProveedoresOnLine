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

        public int ProviderInfoUpsert(string ProviderPublicId, int ProviderInfoId, int ProviderInfoTypeId, string Value, string LargeValue)
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
                CommandText = "P_ProviderInfo_Upsert",
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
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderCustomerInfoId", ProviderCustomerInfoId == 0 ? null : ProviderCustomerInfoId));
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

        public List<ProviderModel> ProviderSearch(string SearchParam, string CustomerPublicId, string FormPublicId, int PageNumber, int RowCount, out int TotalRows, bool isUnique)
        {
            TotalRows = 0;
            int oUnique = isUnique == true ? 1 : 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFormPublicId", FormPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));
            lstParams.Add(DataInstance.CreateTypedParameter("vIsSingleCustomer", oUnique));

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
                     group c.Field<int>("ProviderCustomerInfoTypeId") == 403 by
                     new
                     {
                         ProviderPublicId = c.Field<string>("ProviderPublicId"),
                         Name = c.Field<string>("Name"),
                         Email = c.Field<string>("Email"),
                         IdentificationTypeId = c.Field<int>("IdentificationTypeId"),
                         IdentificationTypeName = c.Field<string>("IdentificationTypeName"),
                         IdentificationNumber = c.Field<string>("IdentificationNumber"),
                         FormPublicId = c.Field<string>("FormPublicId"),
                         FormName = c.Field<string>("FormName"),
                         CustomerPublicId = c.Field<string>("CustomerPublicId"),
                         CustomerName = c.Field<string>("CustomerName"),
                         CustomerCount = c.Field<Int64>("CustomerCount"),
                         logUser = c.Field<string>("LogUser"),
                         LogCreateDate = !c.IsNull("LogCreateDate") ? c.Field<DateTime>("LogCreateDate").ToString("yyyy-MM-dd HH:mm") : string.Empty,
                     } into prov
                     select new ProviderModel()
                     {
                         ProviderPublicId = prov.Key.ProviderPublicId,
                         Name = prov.Key.Name,
                         Email = prov.Key.Email,
                         IdentificationType = new CatalogModel()
                         {
                             ItemId = prov.Key.IdentificationTypeId,
                             ItemName = prov.Key.IdentificationTypeName,
                         },
                         IdentificationNumber = prov.Key.IdentificationNumber,
                         FormName = prov.Key.FormName,
                         FormPublicId = prov.Key.FormPublicId,
                         CustomerName = prov.Key.CustomerName,
                         CustomerPublicId = prov.Key.CustomerPublicId,
                         CustomerCount = prov.Key.CustomerCount,
                         LogUser = prov.Key.logUser,
                         LogCreateDate = prov.Key.LogCreateDate,

                         RelatedProviderCustomerInfo =
                            (from pci in response.DataTableResult.AsEnumerable()
                             where !pci.IsNull("ProviderCustomerInfoId") && pci.Field<string>("ProviderPublicId") == prov.Key.ProviderPublicId
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
                     }).ToList();
            }

            return oReturn;
        }

        public List<LogManager.Models.LogModel> ProviderLog(string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                    CommandText = "P_ProviderLog_ByPublicId",
                    CommandType = CommandType.StoredProcedure,
                    Parameters = lstParams,
                });

            List<LogManager.Models.LogModel> oReturn = null;

            if (response.DataTableResult != null && 
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from l in response.DataTableResult.AsEnumerable()
                     where !l.IsNull("LogId")
                     group l by new
                     {
                         LogId = l.Field<int>("LogId"),
                         User = l.Field<string>("User"),
                         Source = l.Field<string>("Source"),
                         LogInfoId = l.Field<int>("LogInfoId"),
                         LogInfoType = l.Field<string>("LogInfoType"),
                         Name = l.Field<string>("Name"),
                         CreateDate = l.Field<DateTime>("CreateDate"),
                     }
                         into li
                         select new LogManager.Models.LogModel()
                         {
                             LogId = li.Key.LogId,
                             User = li.Key.User,
                             Source = li.Key.Source,
                             CreateDate = li.Key.CreateDate,
                             RelatedLogInfo = new List<LogManager.Models.LogInfoModel>()
                             {
                                 new LogManager.Models.LogInfoModel()
                                 {
                                     LogInfoId = li.Key.LogInfoId,
                                     LogInfoType = li.Key.LogInfoType,
                                     Value = li.Key.Name,
                                 }
                             }
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
                             LastModify = pci.Field<DateTime>("ProviderCustomerInfoLastModify"),
                             CreateDate = pci.Field<DateTime>("ProviderCustomerInfoCreateDate"),
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
                             CreateDate = pci.Field<DateTime>("ProviderInfoCreateDate"),
                             LastModify = pci.Field<DateTime>("ProviderInfoLastModify"),
                         }).ToList(),
                };
            }

            return oReturn;
        }

        public Dictionary<int, List<CatalogModel>> CatalogGetProviderOptions()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Catalog_GetProviderOptions",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            Dictionary<int, List<CatalogModel>> oReturn = new Dictionary<int, List<CatalogModel>>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("ProviderInfoType")
                     group c by new
                     {
                         ProviderInfoType = (int)c.Field<Int64>("ProviderInfoType")
                     } into cg
                     select new
                     {
                         ProviderInfoType = cg.Key.ProviderInfoType,

                         CatalogInfo =
                             (from ci in response.DataTableResult.AsEnumerable()
                              where !ci.IsNull("CatalogId") && (int)ci.Field<Int64>("ProviderInfoType") == cg.Key.ProviderInfoType
                              group ci by new
                              {
                                  CatalogId = ci.Field<int>("CatalogId"),
                                  CatalogName = ci.Field<string>("CatalogName"),
                                  ItemId = ci.Field<int>("ItemId"),
                                  ItemName = ci.Field<string>("ItemName"),
                              } into cig
                              select new CatalogModel()
                              {
                                  CatalogId = cig.Key.CatalogId,
                                  CatalogName = cig.Key.CatalogName,
                                  ItemId = cig.Key.ItemId,
                                  ItemName = cig.Key.ItemName,
                              }).ToList(),
                     }).ToDictionary(k => k.ProviderInfoType, v => v.CatalogInfo);
            }
            return oReturn;
        }

        public ProviderModel ProviderGetByInfoType(string IdentificationNumber, int IdentificationTypeId, int ProviderInfoTypeId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdenificationTypeId", IdentificationTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vInfoType", ProviderInfoTypeId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "P_Provider_GetByIdAndType",
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
                    Email = response.DataTableResult.Rows[0].Field<string>("Email")
                   
                };
            }
            return oReturn;
        }

        #region ChangesControl

        //ChangesUpsert
        public string ChangesControlUpsert(string ChangesPublicId, int ProviderInfoId, string FormUrl, int Status, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();
                        
            lstParams.Add(DataInstance.CreateTypedParameter("vChangesPublicId", ChangesPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderInfoId", ProviderInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFormUrl", FormUrl));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatus", Status));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == false ? 0 : 1));            

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_ChangesControl_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();            
        }

        public List<ChangesControlModel> ChangesControlSearch(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;           

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));            
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));            

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "P_ChagesControl_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<ChangesControlModel> oReturn = new List<ChangesControlModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("ProviderPublicId")
                     select new ChangesControlModel()                     
                     {
                         ProviderPublicId = c.Field<string>("ProviderPublicId"),                                                 
                         Name = c.Field<string>("ProviderName"),                                                 
                         IdentificationNumber = c.Field<string>("IdentificationNumber"),
                         IdentificationType = new CatalogModel()
                         {
                            ItemId = c.Field<int>("IdentificationTypeId"),
                            ItemName = c.Field<string>("IdentificationType"),
                         },
                         Status = new CatalogModel()
                         {
                             ItemId = c.Field<int>("StatusId"),
                             ItemName = c.Field<string>("Status"),
                         },
                         FormUrl = c.Field<string>("FormPublicId"),
                         Enable = c.Field<UInt64>("ChangeEnable") == 1 ? true : false,
                          
                         LastModify = c.Field<DateTime>("LastModify"),                     
                     }).ToList();
            }
            return oReturn;
        }

        public List<ChangesControlModel> ChangesControlGetByProviderPublicId(string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));         

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "P_ChagesControl_GetByProviderPublicId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<ChangesControlModel> oReturn = new List<ChangesControlModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("ProviderPublicId")
                     select new ChangesControlModel()
                     {
                         ChangesPublicId = c.Field<string>("ChangesPublicId"),
                         ProviderPublicId = c.Field<string>("ProviderPublicId"),
                         Name = c.Field<string>("ProviderName"),
                         IdentificationNumber = c.Field<string>("IdentificationNumber"),
                         ProviderInfoId = c.Field<int>("ProviderInfoId"),     
                         Status = new CatalogModel()
                         {
                             ItemId = c.Field<int>("StatusId"),
                             ItemName = c.Field<string>("Status"),
                         },
                         FormUrl = c.Field<string>("FormPublicId"),
                         Enable = c.Field<UInt64>("ChangeEnable") == 1 ? true : false,

                         LastModify = c.Field<DateTime>("LastModify"),
                     }).ToList();
            }
            return oReturn;
        }
        #endregion
    }
}
