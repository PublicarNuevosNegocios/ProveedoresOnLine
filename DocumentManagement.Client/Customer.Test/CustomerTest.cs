using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DocumentManagementClient.Manager.Models;


namespace Customer.Test
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void CustomerUpsert()
        {
            string result = DocumentManagementClient.Manager.Controller.Customer.CustomerUpsert(null, "SebastianTes", enumIdentificationType.Nit, "5555555555");
            Assert.IsNotNull(result);
        }
    }
}
