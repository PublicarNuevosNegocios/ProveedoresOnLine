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
        
        public List<SanofiGeneralInfoModel> GetInfo_ByProvider(string vProviderPublicId)
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


        public List<SanofiComercialInfoModel> GetComercialInfo_ByProvider(string vProviderPublicId)
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
                        NIF_Type = sci.Field<int>("NIF_Type"),
                        CountsGroupItemId = sci.Field<string>("CountsGroupItemId"),
                        CountsGroupItemName = sci.Field<string>("CountsGroupItemName"),
                        TaxClassId = sci.Field<int>("TaxClassId"),
                        TaxClassName = sci.Field<string>("TaxClassName"),
                        CurrencyId = sci.Field<string>("CurrencyId"),
                        CurrencyName = sci.Field<string>("CurrencyName"),
                        GroupSchemaProvider = sci.Field<string>("GroupSchemaProvider"),
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
                            NIF_Type = scig.Key.NIF_Type,
                            CountsGroupItemId = scig.Key.CountsGroupItemId,
                            CountsGroupItemName = scig.Key.CountsGroupItemName,
                            TaxClassId = scig.Key.TaxClassId,
                            TaxClassName = scig.Key.TaxClassName,
                            CurrencyId = scig.Key.CurrencyId,
                            CurrencyName = scig.Key.CurrencyName,
                            GroupSchemaProvider = scig.Key.GroupSchemaProvider,
                            ContactName = scig.Key.ContactName,
                            ComprasCod = scig.Key.ComprasCod

                        }).ToList();
	        }
            return oReturn;
        }
    }
}
