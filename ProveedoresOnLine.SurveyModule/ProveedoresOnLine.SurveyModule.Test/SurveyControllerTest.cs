using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.SurveyModule.Test
{
    [TestClass]
    public class SurveyControllerTest
    {
        [TestMethod]
        public void SurveyRecalculate()
        {
            ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyRecalculate("1C4FB681");

            Assert.AreEqual(1, 1);
        }
    }
}
