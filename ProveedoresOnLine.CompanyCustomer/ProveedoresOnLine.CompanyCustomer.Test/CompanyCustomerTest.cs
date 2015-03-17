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
                CompanyCustomer.Controller.CompanyCustomer.GetCustomerByProvider("1F2BF1AD", 0, true);

            Assert.AreEqual(true, oReturn.RelatedProvider.Count >= 1 && oReturn.RelatedProvider != null);
        }

        [TestMethod]
        public void GetCustomerInfoByProvider()
        {
            CompanyCustomer.Models.Customer.CustomerModel oReturn =
                CompanyCustomer.Controller.CompanyCustomer.GetCustomerInfoByProvider(387);

            Assert.AreEqual(true, oReturn.RelatedProvider.Count >= 1);
        }
    }
}
