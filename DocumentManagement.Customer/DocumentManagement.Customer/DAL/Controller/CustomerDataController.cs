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

        #endregion

        #region Form

        public string FormUpsert(string FormPublicId, string CustomerPublicId, string Name, string TermsAndConditions, string Logo)
        {
            return DataFactory.FormUpsert(FormPublicId, CustomerPublicId, Name, TermsAndConditions, Logo);
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

        #endregion
    }
}
