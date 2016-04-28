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

        [TestMethod]
        public void AditionalDocumentsUpsert()
        {
            ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel oModel = new Models.Customer.CustomerModel();
            ProveedoresOnLine.Company.Models.Util.GenericItemModel oGenericModel = new ProveedoresOnLine.Company.Models.Util.GenericItemModel();

            oModel.RelatedCompany = new Company.Models.Company.CompanyModel();
            oModel.RelatedCompany.CompanyPublicId = "DA5C572E";

            oModel.AditionalDocuments = new List<Company.Models.Util.GenericItemModel>();

            oGenericModel = new Company.Models.Util.GenericItemModel()
            {
                ItemId = 0,
                ItemName = "Archivo adicional de prueba",
                Enable = true,
                ItemInfo = new List<Company.Models.Util.GenericItemInfoModel>(),
            };

            oGenericModel.ItemInfo.Add(
                    new Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new Company.Models.Util.CatalogModel()
                        {
                            ItemId = 220003,
                        },
                        Value = "1701001",
                        Enable = true,
                    });

            oGenericModel.ItemInfo.Add(
                    new Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new Company.Models.Util.CatalogModel()
                        {
                            ItemId = 220004,
                        },
                        Value = "701001",
                        Enable = true,
                    }
                    );

            oModel.AditionalDocuments.Add(oGenericModel);

            oModel = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.AditionalDocumentsUpsert(oModel);

            Assert.AreEqual(true, oModel != null && oModel.AditionalDocuments != null);
        }

        [TestMethod]
        public void GetAllAditionalDocuments()
        {
            int Total = 0;

            ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel oReturn =
                   ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.GetAditionalDocumentsByCompany
                   ("DA5C572E", true, 0, 12000, out Total);

            Assert.AreEqual(true, oReturn != null && oReturn.AditionalDocuments != null);
        }

        [TestMethod]
        public void GetCustomerProviderByCustomData()
        {
            List<ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel> oReturn =
                ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.GetCustomerProviderByCustomData("1BD9AD1B");

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }
    }
}
