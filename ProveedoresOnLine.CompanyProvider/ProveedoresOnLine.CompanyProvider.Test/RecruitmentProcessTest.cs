using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ProveedoresOnLine.Company.Models.Util;

namespace ProveedoresOnLine.CompanyProvider.Test
{
    [TestClass]
    public class RecruitmentProcessTest
    {
        [TestMethod]
        public void RecruitmentProcess_StartProcess()
        {
            ProveedoresOnLine.CompanyProviderBatch.RecruitmentProcess.StartProcess();

            Assert.AreEqual(1, 1);
        }
    }
}
