using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.SurveyModule.Test
{
    [TestClass]
    public class SurveySendProcessTest
    {
        [TestMethod]
        public void SurveySendProcess_StartProcess()
        {
            ProveedoresOnLine.SurveyBatch.SurveySendProcess.StartProcess();

            Assert.AreEqual(1, 1);
        }
    }
}
