using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.Process.Test
{
    [TestClass]
    public class SurveyReminderProcessJobTest
    {
        [TestMethod]
        public void SurveyReminderProcessJob_Execute()
        {
            ProveedoresOnLine.Process.Implement.SurveyReminderProcessJob SBJOb = new Implement.SurveyReminderProcessJob();
            SBJOb.Execute(null);
        }
    }
}
