using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DocumentManagement.Customer.Test
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void CustomerSearch()
        {
            int oTotalRows;
            List<DocumentManagement.Customer.Models.Customer.CustomerModel> oResult =
                DocumentManagement.Customer.Controller.Customer.CustomerSearch
                    (null, 0, 20, out oTotalRows);

            Assert.AreEqual(true, oResult.Count > 0);
        }

        [TestMethod]
        public void CustomerGetById()
        {
            DocumentManagement.Customer.Models.Customer.CustomerModel oResult =
                DocumentManagement.Customer.Controller.Customer.CustomerGetById("aaa");

            Assert.IsNotNull(oResult);
        }
    }
}
