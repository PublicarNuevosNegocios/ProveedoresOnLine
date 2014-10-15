using DocumentManagementClient.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagementClient.Manager.DAL.Controller
{
    internal class CustomerDataController : ICustomerData
    {
        #region singleton instance
        private static ICustomerData oInstance;
        internal static ICustomerData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new CustomerDataController();
                return oInstance;
            }
        }

        private ICustomerData DataFactory;
        #endregion

        #region Customer
        public CustomerDataController()
        {
            CustomerDataFactory factory = new CustomerDataFactory();
            DataFactory = factory.GetCustomerInstance();
        }
        #endregion

        public string CustomerUpsert(string CustomerPublicId, string Name, Models.enumIdentificationType IdentificationType, string IdentificationNumber)
        {
            return DataFactory.CustomerUpsert(CustomerPublicId, Name, IdentificationType, IdentificationNumber);
        }
    }
}
