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
            oReturn = IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetInfo_ByProvider();

            Assert.AreEqual(true, oReturn.Count > 0);
        }
    }
}
