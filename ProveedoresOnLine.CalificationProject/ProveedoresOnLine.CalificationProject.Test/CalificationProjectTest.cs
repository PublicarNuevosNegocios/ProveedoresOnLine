using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProveedoresOnLine.CalificationProject.Models.CalificationProject;
using System.Collections.Generic;

namespace ProveedoresOnLine.CalificationProject.Test
{
    [TestClass]
    public class CalificationProjectTest
    {
        [TestMethod]
        public void CalificationProjectConfigGetByCompanyId()
        {
            List<CalificationProjectConfigModel> oReturn = new List<CalificationProjectConfigModel>();
            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigGetByCompanyId
                ("1B40C887",
                true);

            Assert.AreEqual(true, oReturn.Count > 0);
        }
    }
}
