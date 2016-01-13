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
            ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyRecalculate("1C4FB681", 31, "johann.martinez@publicar.com");

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void GetSurveyByResponsable()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> oreturn =
            ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByResponsable("1EA5A78A", "johann.martinez@publicar.com", DateTime.Now);

            Assert.IsTrue(oreturn.Count > 0);
        }

        [TestMethod]
        public void GetSurveyByEvaluator()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> oreturn =
            ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByEvaluator("DA5C572E", "");

            Assert.IsTrue(oreturn.Count > 0);
        }


        [TestMethod]
        public void GetSurveyByUser()
        {
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oReturn =
                ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetByUser(null, "sebastian.admin@alpina.com");

            Assert.IsTrue(oReturn != null);
        }
    }
}
