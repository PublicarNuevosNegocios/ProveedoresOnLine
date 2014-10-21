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

            CustomerProviderInfo.ProviderInfoType = new CatalogModel() { ItemId = 201 };
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

        //[TestMethod]
        //public void LoadFile()
        //{
        //    string FilePath = @"D:\Proyectos\Github\ProveedoresOnLine\DocumentManagement.Provider\DocumentManagement.Provider_Test\Jellyfish.jpg";

        //    string oReturn = DocumentManagement.Provider.Controller.Provider.LoadFile(FilePath, "\\tmp\\");

        //    Assert.AreEqual(true, !string.IsNullOrEmpty(oReturn));
        //}
    }
}
