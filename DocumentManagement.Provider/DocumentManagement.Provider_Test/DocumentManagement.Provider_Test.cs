using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocumentManagement.Provider.Models.Provider;
using System.Collections.Generic;
using DocumentManagement.Provider.Models.Util;

namespace DocumentManagement.Provider_Test
{
    [TestClass]
    public class ProviderTest
    {
        [TestMethod]
        public void ProviderUpsert()
        {
            List<ProviderInfoModel> ListCustomerProviderInfo = new List<ProviderInfoModel>();
            ProviderInfoModel CustomerProviderInfo = new ProviderInfoModel();

            CustomerProviderInfo.ProviderInfoType = new CatalogModel() { ItemId = 401 };
            CustomerProviderInfo.Value = "201";
            ListCustomerProviderInfo.Add(CustomerProviderInfo);
            //Create Provider
            ProviderModel ProviderToCreate = new ProviderModel()
            {
                CustomerPublicId = "1C17DDCD",
                Name = "ProveedorTest",
                IdentificationType = new Provider.Models.Util.CatalogModel() { ItemId = 101 },
                IdentificationNumber = "1030544789",
                Email = "pruebaemail.email.com",
                RelatedProviderCustomerInfo = ListCustomerProviderInfo

            };
            string result = DocumentManagement.Provider.Controller.Provider.ProviderUpsert(ProviderToCreate);
            //Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProviderGetByIdentification()
        {
            DocumentManagement.Provider.Models.Provider.ProviderModel oReturn =
                DocumentManagement.Provider.Controller.Provider.ProviderGetByIdentification
                    ("16290", 102, "1C17DDCD");

            Assert.IsNotNull(oReturn);
        }

        [TestMethod]
        public void ProviderGetById()
        {
            DocumentManagement.Provider.Models.Provider.ProviderModel oReturn =
                DocumentManagement.Provider.Controller.Provider.ProviderGetById
                    ("", 1);

            Assert.IsNotNull(oReturn);
        }

        [TestMethod]
        public void CatalogGetProviderOptions()
        {
            Dictionary<DocumentManagement.Provider.Models.Util.CatalogModel, List<DocumentManagement.Provider.Models.Util.CatalogModel>> oReturn =
                DocumentManagement.Provider.Controller.Provider.CatalogGetProviderOptions();

            Assert.AreEqual(true, oReturn.Count > 0);
        }

    }
}
