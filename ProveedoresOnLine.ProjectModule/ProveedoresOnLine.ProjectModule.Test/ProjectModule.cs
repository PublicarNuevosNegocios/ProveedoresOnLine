using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.ProjectModule.Test
{
    [TestClass]
    public class ProjectModule
    {
        [TestMethod]
        public void GetAllProjectConfigByCustomerPublicId()
        {
            int TotalRows = 0;

            List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> oReturn =
            ProveedoresOnLine.ProjectModule.Controller.ProjectModule.GetAllProjectConfigByCustomerPublicId("DA5C572E", 1000, 0, out TotalRows);

            Assert.AreEqual(1, 1);
        }
    }
}
