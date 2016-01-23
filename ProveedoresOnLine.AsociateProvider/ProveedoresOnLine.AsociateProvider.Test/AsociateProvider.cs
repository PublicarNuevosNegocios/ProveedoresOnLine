using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.AsociateProvider.Test
{
    [TestClass]
    public class AsociateProvider
    {
        [TestMethod]
        public void BOProviderUpsert() 
        {
            int oReturn;

            ProveedoresOnLine.AsociateProvider.Client.Models.AsociateProviderModel BOPRovider = new Client.Models.AsociateProviderModel()
            {
                RelatedProviderBO = new Client.Models.RelatedProviderModel()
                {
                    ProviderPublicId = "4CD75091",
                    ProviderName = "FISHER CONTROLS INTERNATIONAL LLC",
                    IdentificationType = "201003",
                    IdentificationNumber = "431156463",
                },
            };

            oReturn = ProveedoresOnLine.AsociateProvider.Client.Controller.AsociateProviderClient.ProviderUpsertBO(BOPRovider);

            Assert.AreEqual(true, oReturn != null);
        }

        [TestMethod]
        public void DMProviderUpsert()
        {
            int oReturn;

            ProveedoresOnLine.AsociateProvider.Client.Models.AsociateProviderModel DMProvider = new Client.Models.AsociateProviderModel()
            {
                RelatedProviderDM = new Client.Models.RelatedProviderModel()
                {
                    ProviderPublicId = "abc12021",
                    ProviderName = "Prueba Log",
                    IdentificationType = "103",
                    IdentificationNumber = "0001",
                },
            };

            oReturn = ProveedoresOnLine.AsociateProvider.Client.Controller.AsociateProviderClient.ProviderUpsertDM(DMProvider);

            Assert.AreEqual(true, oReturn != null);
        }

        [TestMethod]
        public void AsociateProviderUpsert()
        {
            ProveedoresOnLine.AsociateProvider.Client.Models.AsociateProviderModel AsociateProviderToUpsert = new Client.Models.AsociateProviderModel()
            {
                RelatedProviderBO = new Client.Models.RelatedProviderModel()
                {
                    ProviderPublicId = "4CD75091",
                    ProviderName = "FISHER CONTROLS INTERNATIONAL LLC",
                    IdentificationType = "201003",
                    IdentificationNumber = "431156463",
                },
                RelatedProviderDM = new Client.Models.RelatedProviderModel()
                {
                    ProviderPublicId = "A1F3B1B6",
                    ProviderName = "FISHER CONTROLS INTERNATIONAL LLC",
                    IdentificationType = "103",
                    IdentificationNumber = "431156463",
                },
                Email = "d.alonsomt@gmail.com",
            };

            ProveedoresOnLine.AsociateProvider.Client.Controller.AsociateProviderClient.AsociateProvider(AsociateProviderToUpsert);
        }

        [TestMethod]
        public void GetAllAsociateProvider()
        {
            int TotalRows = 0;

            List<ProveedoresOnLine.AsociateProvider.Interfaces.Models.AsociateProvider.AsociateProviderModel> oReturn =
                ProveedoresOnLine.AsociateProvider.DAL.Controller.AsociateProviderDataController.Instance.GetAllAsociateProvider("", 0, 100, out TotalRows);
        }

        [TestMethod]
        public void GetAsociateProviderByProviderPublicId()
        {
            List<ProveedoresOnLine.AsociateProvider.Interfaces.Models.AsociateProvider.AsociateProviderModel> oReturn =
            ProveedoresOnLine.AsociateProvider.DAL.Controller.AsociateProviderDataController.Instance.GetAsociateProviderByProviderPublicId("", "1CA3A147");

        }

        [TestMethod]
        public void client_GetAsociateProviderByProviderPublicId()
        {
            ProveedoresOnLine.AsociateProvider.Client.Models.AsociateProviderModel oReturn =
            ProveedoresOnLine.AsociateProvider.Client.Controller.AsociateProviderClient.GetAsociateProviderByProviderPublicId("", "1CA3A147");
            Assert.AreEqual(true, oReturn != null);
        }

        [TestMethod]
        public void AsociateProviderUpsertEmail()
        {
            int oReturn = 0;
            oReturn = ProveedoresOnLine.AsociateProvider.DAL.Controller.AsociateProviderDataController.Instance.AsociateProviderUpsertEmail(43, "d.alonsomt@gmail.com");
            Assert.AreEqual(true, oReturn != null);
        }

        [TestMethod]
        public void GetHomologateItemBySourceID()
        {
            ProveedoresOnLine.AsociateProvider.Client.Models.HomologateModel oReturn = null;
            oReturn = ProveedoresOnLine.AsociateProvider.Client.Controller.AsociateProviderClient.GetHomologateItemBySourceID(372);
            Assert.IsNotNull(oReturn);
        }

        [TestMethod]
        public void AsociateCustomerProvider()
        {
            ProveedoresOnLine.AsociateProvider.Client.Controller.AsociateProviderClient.AP_AsociateRelatedCustomerProvider("1EA5A78A", "3247FC17", false);
        }
    }
}
