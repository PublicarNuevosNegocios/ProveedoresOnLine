using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
