using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using IntegrationPlattaform.SANOFIProcess.Models;

namespace IntegrationPlatform.SANOFIProcess.Test
{
    [TestClass]
    public class IntegrationPlatformSANOFIProcessTest
    {
        [TestMethod]
        public void StartProcess()
        {
            IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.StartProcess();
        }

        [TestMethod]
        public void GetInfo_ByProvider() 
        {
            List<SanofiGeneralInfoModel> oReturn = new List<SanofiGeneralInfoModel>();
            oReturn = IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetInfo_ByProvider("A24EB150");

            Assert.AreEqual(true, oReturn.Count > 0);
        }
        [TestMethod]
        public void GetComercialInfo_ByProvider() 
        {
            List<SanofiComercialInfoModel> oReturn = new List<SanofiComercialInfoModel>();
            oReturn = IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetComercialInfo_ByProvider("A24EB150");

            Assert.AreEqual(true, oReturn.Count > 0);
        }

        [TestMethod]
        public void GetContableInfo_ByProvider() 
        {
            List<SanofiContableInfoModel> oReturn = new List<SanofiContableInfoModel>();
            oReturn = IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetContableInfo_ByProvider("A24EB150");

            Assert.AreEqual(true, oReturn.Count > 0);
        }
        [TestMethod]
        public void SanofiProcessLog_Insert() 
        {
            SanofiProcessLogModel oReturn = new SanofiProcessLogModel()
            {
                ProviderPublicId = "A24EB150",
                ProcessName = "PruebaLog 4",
                IsSucces = true,
                Enable = true
            };
            oReturn = IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.SanofiProcessLog_Insert(oReturn);

            Assert.AreEqual(true, oReturn.SanofiProcessLogId > 0);
        }
    }
}
