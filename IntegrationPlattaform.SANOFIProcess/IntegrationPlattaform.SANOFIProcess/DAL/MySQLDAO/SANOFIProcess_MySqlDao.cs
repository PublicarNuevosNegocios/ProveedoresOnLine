﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IntegrationPlattaform.SANOFIProcess.Interfaces;
using IntegrationPlattaform.SANOFIProcess.Models;
using ADO.MYSQL;


namespace IntegrationPlattaform.SANOFIProcess.DAL.MySQLDAO
{
    internal class SANOFIProcess_MySqlDao : IIntegrationPlatformSANOFIProcessData
    {
        private ADO.Interfaces.IADO DataInstance;

        public SANOFIProcess_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(IntegrationPlattaform.SANOFIProcess.Models.Constants.C_SettingsModuleName);
        }

        public List<SanofiGeneralInfoModel> GetInfoByProvider(string vProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<System.Data.IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", vProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "Sanofi_GetGeneralInfo",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<SanofiGeneralInfoModel> oReturn = new List<SanofiGeneralInfoModel>();

            if (response.DataTableResult != null & response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (
                        from sgi in response.DataTableResult.AsEnumerable()
                        where !sgi.IsNull("CompanyId")
                        group sgi by new
                        {
                            CompanyId = sgi.Field<int>("CompanyId"),
                            CompanyName = sgi.Field<string>("CompanyName"),
                            ComercialName = sgi.Field<string>("ComercialName"),
                            NaturalPersonName = sgi.Field<string>("NaturalPersonName"),
                            IdentificationNumber = sgi.Field<string>("IdentificationNumber"),
                            FiscalNumber = sgi.Field<string>("FiscalNumber"),
                            Address = sgi.Field<string>("Address"),
                            City = sgi.Field<string>("City"),
                            Region = sgi.Field<string>("Region"),
                            Country = sgi.Field<string>("Country"),
                            PhoneNumber = sgi.Field<string>("PhoneNumber"),
                            Fax = sgi.Field<string>("Fax"),
                            Email_OC = sgi.Field<string>("Email_OC"),
                            Email_P = sgi.Field<string>("Email_P"),
                            Email_Cert = sgi.Field<string>("Email_Cert"),
                            Comentaries = sgi.Field<DateTime>("Comentaries"),
                        }
                            into sgig
                            select new SanofiGeneralInfoModel()
                            {
                                CompanyId = sgig.Key.CompanyId,
                                CompanyName = sgig.Key.CompanyName,
                                ComercialName = sgig.Key.ComercialName,
                                NaturalPersonName = sgig.Key.NaturalPersonName,
                                IdentificationNumber = sgig.Key.IdentificationNumber,
                                FiscalNumber = sgig.Key.FiscalNumber,
                                Address = sgig.Key.Address,
                                City = sgig.Key.City,
                                Region = sgig.Key.Region,
                                Country = sgig.Key.Country,
                                PhoneNumber = sgig.Key.PhoneNumber,
                                Fax = sgig.Key.Fax,
                                Email_OC = sgig.Key.Email_OC,
                                Email_P = sgig.Key.Email_P,
                                Email_Cert = sgig.Key.Email_Cert,
                                Comentaries = sgig.Key.Comentaries,
                            }).ToList();
            }
            return oReturn;
        }


        public List<SanofiComercialInfoModel> GetComercialInfoByProvider(string vProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<System.Data.IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", vProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "Sanofi_GetComercialInfo",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<SanofiComercialInfoModel> oReturn = new List<SanofiComercialInfoModel>();
            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                (
                    from sci in response.DataTableResult.AsEnumerable()
                    where !sci.IsNull("CompanyId")
                    group sci by new
                    {
                        CompanyId = sci.Field<int>("CompanyId"),
                        CompanyName = sci.Field<string>("CompanyName"),
                        IdentificationNumber = sci.Field<string>("IdentificationNumber"),
                        FiscalNumber = sci.Field<string>("FiscalNumber"),
                        NIF_Type = sci.Field<string>("NIF_Type"),
                        CountsGroupItemId = sci.Field<string>("CountsGroupItemId"),
                        CountsGroupItemName = sci.Field<string>("CountsGroupItemName"),
                        Ramo = sci.Field<string>("Ramo"),
                        PayCondition = sci.Field<string>("PayCondition"),
                        TaxClassId = sci.Field<string>("TaxClassId"),
                        TaxClassName = sci.Field<string>("TaxClassName"),
                        CurrencyId = sci.Field<string>("CurrencyId"),
                        CurrencyName = sci.Field<string>("CurrencyName"),
                        GroupSchemaProvider = sci.Field<int>("GroupSchemaProvider"),
                        ContactName = sci.Field<string>("ContactName"),
                        ComprasCod = sci.Field<string>("ComprasCod"),
                    }
                        into scig
                        select new SanofiComercialInfoModel()
                        {
                            CompanyId = scig.Key.CompanyId,
                            CompanyName = scig.Key.CompanyName,
                            IdentificationNumber = scig.Key.IdentificationNumber,
                            FiscalNumber = scig.Key.FiscalNumber,
                            NIFType = scig.Key.NIF_Type,
                            CountsGroupItemId = scig.Key.CountsGroupItemId,
                            CountsGroupItemName = scig.Key.CountsGroupItemName,
                            Ramo = scig.Key.Ramo,
                            PayCondition = scig.Key.PayCondition,
                            TaxClassId = scig.Key.TaxClassId,
                            TaxClassName = scig.Key.TaxClassName,
                            CurrencyId = scig.Key.CurrencyId,
                            CurrencyName = scig.Key.CurrencyName,
                            GroupSchemaProvider = scig.Key.GroupSchemaProvider,
                            ContactName = scig.Key.ContactName,
                            BuyCod = scig.Key.ComprasCod

                        }).ToList();
            }
            return oReturn;
        }


        public List<SanofiContableInfoModel> GetContableInfoByProvider(string vProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<System.Data.IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", vProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "Sanofi_GetContableInfo",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<SanofiContableInfoModel> oReturn = new List<SanofiContableInfoModel>();

            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (
                      from sconi in response.DataTableResult.AsEnumerable()
                      where !sconi.IsNull("CompanyId")
                      group sconi by new
                      {
                          CompanyId = sconi.Field<int>("CompanyId"),
                          CompanyName = sconi.Field<string>("CompanyName"),
                          FiscalNumber = sconi.Field<string>("FiscalNumber"),
                          IdentificationNumber = sconi.Field<string>("IdentificationNumber"),
                          Country = sconi.Field<string>("Country"),
                          BankPassword = sconi.Field<int>("BankPassword"),
                          BankCountNumber = sconi.Field<string>("BankCountNumber"),
                          CountType = sconi.Field<int>("CountType"),
                          IBAN = sconi.Field<string>("IBAN"),
                          PayWay = sconi.Field<string>("PayWay"),
                          PayCondition = sconi.Field<string>("PayCondition"),
                          AssociatedCount = sconi.Field<string>("AssociatedCount")
                      }
                          into sconig
                          select new SanofiContableInfoModel()
                          {
                              CompanyId = sconig.Key.CompanyId,
                              CompanyName = sconig.Key.CompanyName,
                              FiscalNumber = sconig.Key.FiscalNumber,
                              IdentificationNumber = sconig.Key.IdentificationNumber,
                              Country = sconig.Key.Country,
                              BankPassword = sconig.Key.BankPassword,
                              BankCountNumber = sconig.Key.BankCountNumber,
                              CountType = sconig.Key.CountType,
                              IBAN = sconig.Key.IBAN,
                              PayWay = sconig.Key.PayWay,
                              PayCondition = sconig.Key.PayCondition,
                              AssociatedCount = sconig.Key.AssociatedCount
                          }).ToList();
            }
            return oReturn;
        }


        public int SanofiProcessLogInsert(string ProviderPublicId, string ProcessName, bool IsSuccess, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<System.Data.IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstparams.Add(DataInstance.CreateTypedParameter("vProcessName", ProcessName));
            lstparams.Add(DataInstance.CreateTypedParameter("vIsSuccess", (IsSuccess == true) ? 1 : 0));
            lstparams.Add(DataInstance.CreateTypedParameter("vEnable", (Enable == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "Sanofi_ProcessLog_Insert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });
            return Convert.ToInt32(response.ScalarResult);
        }


        public List<SanofiProcessLogModel> GetSanofiProcessLog(bool IsSuccess)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vIsSuccess", (IsSuccess == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "Sanofi_ProcessLog_Get",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<SanofiProcessLogModel> oReturn = new List<SanofiProcessLogModel>();

            if (response.DataTableResult != null && response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (
                        from spl in response.DataTableResult.AsEnumerable()
                        where !spl.IsNull("SanofiProcessLogId")
                        group spl by new
                        {
                            SanofiProcessLogId = spl.Field<int>("SanofiProcessLogId"),
                            ProviderPublicId = spl.Field<string>("ProviderPublicId"),
                            ProcessName = spl.Field<string>("ProcessName"),
                            IsSuccess = spl.Field<UInt64>("IsSuccess") == 1 ? true : false,
                            CreateDate = spl.Field<DateTime>("CreateDate"),
                            LastModify = spl.Field<DateTime>("LastModify"),
                            Enable = spl.Field<UInt64>("Enable") == 1 ? true : false
                        }
                            into splg
                            select new SanofiProcessLogModel
                            {
                                SanofiProcessLogId = splg.Key.SanofiProcessLogId,
                                ProviderPublicId = splg.Key.ProviderPublicId,
                                ProcessName = splg.Key.ProcessName,
                                IsSucces = splg.Key.IsSuccess,
                                CreateDate = splg.Key.CreateDate,
                                LastModify = splg.Key.LastModify
                            }).ToList();
            }
            return oReturn;
        }
    }
}
