using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DocumentManagementClient.Manager.Models;
using DocumentManagementClient.Manager.Models.Customer;


namespace Customer.Test
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void CustomerUpsert()
        {
            string result = DocumentManagementClient.Manager.Controller.Customer.CustomerUpsert(null, "SebastianTest", enumIdentificationType.Nit, "5555555555");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CustomerSearch()
        {
            List<CustomerModel> result = DocumentManagementClient.Manager.Controller.Customer.CustomerSearch(string.Empty, string.Empty);
            Assert.IsNotNull(result);
        }
    }
}
