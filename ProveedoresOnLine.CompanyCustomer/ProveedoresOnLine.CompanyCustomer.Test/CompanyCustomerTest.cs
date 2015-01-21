using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.CompanyCustomer.Test
{
    [TestClass]
    public class CompanyCustomerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void GetCustomerByProvider()
        {
            List<CompanyCustomer.Models.Customer.CustomerModel> oReturn =
                CompanyCustomer.Controller.Customer.GetCustomerByProvider("1A9863BD");

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void GetCustomerInfoByProvider()
        {
            List<CompanyCustomer.Models.Customer.CustomerModel> oReturn =
                CompanyCustomer.Controller.Customer.GetCustomerInfoByProvider(2);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }
    }
}
