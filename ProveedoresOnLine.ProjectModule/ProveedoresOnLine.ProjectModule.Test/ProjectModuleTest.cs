using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.ProjectModule.Test
{
    [TestClass]
    public class ProjectModuleTest
    {
        [TestMethod]
        public void GetAllProjectConfigByCustomerPublicId()
        {
            int TotalRows = 0;

            List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> oReturn =
            ProveedoresOnLine.ProjectModule.Controller.ProjectModule.GetAllProjectConfigByCustomerPublicId
                ("DA5C572E", null, true, 1000, 0, out TotalRows);

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void GetAllEvaluationItemByProjectConfig()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.GetAllEvaluationItemByProjectConfig(1, null,1401001, null, true);

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void ProjectGetById()
        {
            ProveedoresOnLine.ProjectModule.Models.ProjectModel oReturn =
                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetById("1BC311CC", "DA5C572E");

            Assert.IsNotNull(oReturn);
        }

        [TestMethod]
        public void ProjectGetByIdLite()
        {
            ProveedoresOnLine.ProjectModule.Models.ProjectModel oReturn =
                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetByIdLite("1EC791B6", "DA5C572E");

            Assert.IsNotNull(oReturn);
        }

        [TestMethod]
        public void ProjectGetByIdCalculate()
        {
            ProveedoresOnLine.ProjectModule.Models.ProjectModel oReturn =
                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetByIdCalculate("1EC791B6");

            Assert.IsNotNull(oReturn);
        }

        [TestMethod]
        public void ProjectCalculate()
        {
            ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectCalculate("1BC311CC");
            Assert.AreEqual(1, 1);
        }

    }
}
