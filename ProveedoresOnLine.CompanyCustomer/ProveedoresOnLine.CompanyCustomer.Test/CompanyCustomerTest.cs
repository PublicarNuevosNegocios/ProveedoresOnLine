using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.CompanyCustomer.Test
{
    [TestClass]
    public class CompanyCustomerTest
    {
        [TestMethod]
        public void GetCustomerByProvider()
        {
            //Get related customers by Provider
            CompanyCustomer.Models.Customer.CustomerModel oReturn =
                CompanyCustomer.Controller.CompanyCustomer.GetCustomerByProvider("1CA3A147", null);

            Assert.AreEqual(true, oReturn.RelatedProvider.Count >= 1 && oReturn.RelatedProvider != null);
        }

        [TestMethod]
        public void GetCustomerInfoByProvider()
        {
            int TotalRows = 0;

            CompanyCustomer.Models.Customer.CustomerModel oReturn =
                CompanyCustomer.Controller.CompanyCustomer.GetCustomerInfoByProvider(118, true, 0, 1000000, out TotalRows);

            Assert.AreEqual(true, oReturn.RelatedProvider.Count >= 1 && TotalRows > 0);
        }
    }
}
