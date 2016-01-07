﻿using DocumentManagement.Provider.Interfaces;
using DocumentManagement.Provider.Models;
using DocumentManagement.Provider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.DAL.Controller
{
    internal class ProviderDataController : IProviderData
    {
        #region Singleton Instance
        private static IProviderData oInstance;
        internal static IProviderData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new ProviderDataController();
                return oInstance;
            }
        }

        private IProviderData DataFactory;
        #endregion

        #region Constructor
        public ProviderDataController()
        {
            ProviderDataFactory factory = new ProviderDataFactory();
            DataFactory = factory.DocumentManagementProviderInstance();
        }

        #endregion

        public string ProviderUpsert(string ProviderPublicId, string Name, int IdentificationTypeId, string IdentificationNumber, string Email)
        {
            return DataFactory.ProviderUpsert(ProviderPublicId, Name, IdentificationTypeId, IdentificationNumber, Email);
        }

        public int ProviderInfoUpsert(string ProviderPublicId, int ProviderInfoId, int ProviderInfoTypeId, string Value, string LargeValue)
        {
            return DataFactory.ProviderInfoUpsert(ProviderPublicId, ProviderInfoId, ProviderInfoTypeId, Value, LargeValue);
        }

        public int ProviderCustomerInfoUpsert(string ProviderPublicId, string CustomerPublicId, int? ProviderCustomerInfoId, int ProviderCustomerInfoTypeId, string Value, string LargeValue)
        {
            return DataFactory.ProviderCustomerInfoUpsert(ProviderPublicId, CustomerPublicId, ProviderCustomerInfoId, ProviderCustomerInfoTypeId, Value, LargeValue);
        }

        public List<Models.Provider.ProviderModel> ProviderSearch(string SearchParam, string CustomerPublicId, string FormPublicId, int PageNumber, int RowCount, out int TotalRows, bool isUnique)
        {
            return DataFactory.ProviderSearch(SearchParam, CustomerPublicId, FormPublicId, PageNumber, RowCount, out TotalRows, isUnique);
        }

        public List<LogManager.Models.LogModel> ProviderLog(string ProviderPublicId)
        {
            return DataFactory.ProviderLog(ProviderPublicId);
        }

        public Models.Provider.ProviderModel ProviderGetByIdentification(string IdentificationNumber, int IdenificationTypeId, string CustomerPublicId)
        {
            return DataFactory.ProviderGetByIdentification(IdentificationNumber, IdenificationTypeId, CustomerPublicId);
        }

        public Models.Provider.ProviderModel ProviderGetById(string ProviderPublicId, int? StepId)
        {
            return DataFactory.ProviderGetById(ProviderPublicId, StepId);
        }

        public Dictionary<int, List<Models.Util.CatalogModel>> CatalogGetProviderOptions()
        {
            return DataFactory.CatalogGetProviderOptions();
        }

        public Models.Provider.ProviderModel ProviderGetByInfoType(string IdentificationNumber, int IdentificationTypeId, int ProviderInfoTypeId)
        {
            return DataFactory.ProviderGetByInfoType(IdentificationNumber, IdentificationTypeId, ProviderInfoTypeId);
        }

        #region ChangesControl

        public string ChangesControlUpsert(string ChangesPublicId, int ProviderInfoId, string FormUrl, int Status, bool Enable)
        {
            return DataFactory.ChangesControlUpsert(ChangesPublicId, ProviderInfoId, FormUrl, Status, Enable);
        }

        public List<ChangesControlModel> ChangesControlSearch(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.ChangesControlSearch(SearchParam,PageNumber, RowCount, out TotalRows);
        }

        public List<ChangesControlModel> ChangesControlGetByProviderPublicId(string ProviderPublicId)
        {
            return DataFactory.ChangesControlGetByProviderPublicId(ProviderPublicId);
        }
        #endregion
    }
}
