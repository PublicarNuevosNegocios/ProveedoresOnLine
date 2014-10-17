using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Customer.DAL.Controller
{
    internal class CustomerDataController : DocumentManagement.Customer.Interfaces.ICustomerData
    {
        #region singleton instance

        private static DocumentManagement.Customer.Interfaces.ICustomerData oInstance;
        internal static DocumentManagement.Customer.Interfaces.ICustomerData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new CustomerDataController();
                return oInstance;
            }
        }

        private DocumentManagement.Customer.Interfaces.ICustomerData DataFactory;

        #endregion

        #region Constructor

        public CustomerDataController()
        {
            CustomerDataFactory factory = new CustomerDataFactory();
            DataFactory = factory.GetCustomerInstance();
        }

        #endregion

        #region Customer

        public string CustomerUpsert(string CustomerPublicId, string Name, int IdentificationType, string IdentificationNumber)
        {
            return DataFactory.CustomerUpsert(CustomerPublicId, Name, IdentificationType, IdentificationNumber);
        }

        public List<Models.Customer.CustomerModel> CustomerSearch(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CustomerSearch(SearchParam, PageNumber, RowCount, out  TotalRows);
        }

        public Models.Customer.CustomerModel CustomerGetByFormId(string FormPublicId)
        {
            return DataFactory.CustomerGetByFormId(FormPublicId);
        }

        public Models.Customer.CustomerModel CustomerGetById(string CustomerPublicId)
        {
            return DataFactory.CustomerGetById(CustomerPublicId);
        }

        #endregion

        #region Form

        public string FormUpsert(string FormPublicId, string CustomerPublicId, string Name, string TermsAndConditions)
        {
            return DataFactory.FormUpsert(FormPublicId, CustomerPublicId, Name, TermsAndConditions);
        }

        public void FormUpsertLogo(string FormPublicId, string Logo)
        {
            DataFactory.FormUpsertLogo(FormPublicId, Logo);
        }


        public int StepCreate(string FormPublicId, string Name, int Position)
        {
            return DataFactory.StepCreate(FormPublicId, Name, Position);
        }

        public void StepDelete(int StepId)
        {
            DataFactory.StepDelete(StepId);
        }

        public int FieldCreate(int StepId, string Name, int ProviderInfoType, int FieldType, bool IsRequired, int Position)
        {
            return DataFactory.FieldCreate(StepId, Name, ProviderInfoType, FieldType, IsRequired, Position);
        }

        public void FieldDelete(int FieldId)
        {
            DataFactory.FieldDelete(FieldId);
        }

        public List<Models.Form.FormModel> FormSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.FormSearch(CustomerPublicId, SearchParam, PageNumber, RowCount, out  TotalRows);
        }

        #endregion

        #region Util

        public List<Models.Util.CatalogModel> CatalogGetCustomerOptions()
        {
            return DataFactory.CatalogGetCustomerOptions();
        }

        #endregion
    }
}
