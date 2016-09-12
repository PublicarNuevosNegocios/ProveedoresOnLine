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
            oReturn = IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetInfoByProvider("A24EB150");

            Assert.AreEqual(true, oReturn.Count > 0);
        }

        [TestMethod]
        public void GetComercialInfo_ByProvider()
        {
            List<SanofiComercialInfoModel> oReturn = new List<SanofiComercialInfoModel>();
            oReturn = IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetComercialInfoByProvider("A24EB150");

            Assert.AreEqual(true, oReturn.Count > 0);
        }

        [TestMethod]
        public void GetContableInfo_ByProvider()
        {
            List<SanofiContableInfoModel> oReturn = new List<SanofiContableInfoModel>();
            oReturn = IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetContableInfoByProvider("A24EB150");

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
            oReturn = IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.SanofiProcessLogInsert(oReturn);

            Assert.AreEqual(true, oReturn.SanofiProcessLogId > 0);
        }

        [TestMethod]
        public void GetSanofiProcessLog()
        {
            List<SanofiProcessLogModel> oReturn = new List<SanofiProcessLogModel>();
            oReturn = IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetSanofiProcessLog(false);

            Assert.AreEqual(true, oReturn.Count > 0);
        }

        [TestMethod]
        public void GetSanofiLastProcessLog()
        {
            Assert.IsNotNull(IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetSanofiLastProcessLog());
        }
    }
}
