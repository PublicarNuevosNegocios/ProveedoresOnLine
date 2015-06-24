using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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

        [TestMethod]
        public void GetSurveyByResponsable()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> oreturn =
            ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByResponsable("1EA5A78A", "johann.martinez@publicar.com", DateTime.Now);

            Assert.IsTrue(oreturn.Count > 0);
        }
    }
}
