using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentManagement.Provider_Test
{
    [TestClass]
    public class ProviderTest
    {
        [TestMethod]
        public void ProviderUpsert()
        {
            string result = DocumentManagement.Provider.Controller.Provider.ProviderUpsert("", "", "SebastianProvider", DocumentManagement.Provider.Models.Enumerations.enumIdentificationType.Nit, "1030544724", "sebastianmartinez18@yahoo.com.co", DocumentManagement.Provider.Models.Enumerations.enumProcessStatus.New);

            Assert.Fail("");
        }
    }
}
