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
            //Get related customers list
            //List<CompanyCustomer.Models.Customer.CustomerModel> oReturn =
            //    CompanyCustomer.Controller.Customer.GetCustomerByProvider("1F2BF1AD", 1);

            //Get related customers by Provider
            List<CompanyCustomer.Models.Customer.CustomerModel> oReturn =
                CompanyCustomer.Controller.CompanyCustomer.GetCustomerByProvider("1F2BF1AD", 0);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void GetCustomerInfoByProvider()
        {
            List<CompanyCustomer.Models.Customer.CustomerModel> oReturn =
                CompanyCustomer.Controller.CompanyCustomer.GetCustomerInfoByProvider(2);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }
    }
}
