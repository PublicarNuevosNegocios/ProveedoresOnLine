using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.SurveyModule.Test
{
    [TestClass]
    public class SurveyReminderProcessTest
    {
        [TestMethod]
        public void SurveyReminderProcess_StartProcess()
        {
            ProveedoresOnLine.SurveyBatch.SurveyReminderProcess.StartProcess();

            Assert.AreEqual(1, 1);
        }
    }
}
