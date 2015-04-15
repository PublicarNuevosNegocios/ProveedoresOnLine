﻿using System;
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
            ProveedoresOnLine.ProjectModule.Controller.ProjectModule.GetAllProjectConfigByCustomerPublicId("DA5C572E", true, 1000, 0, out TotalRows);

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void GetAllEvaluationItemByProjectConfig()
        {
            int TotalRows = 0;

            List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> oReturn =
                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.GetAllEvaluationItemByProjectConfig("", true, 0, 1000, out TotalRows);

            Assert.AreEqual(1, 1);
        }


        [TestMethod]
        public void ProjectGetById()
        {
            ProveedoresOnLine.ProjectModule.Models.ProjectModel oReturn =
                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetById("1EC791B6", "DA5C572E");

            Assert.IsNotNull(oReturn);
        }

        [TestMethod]
        public void ProjectGetByIdLite()
        {
            ProveedoresOnLine.ProjectModule.Models.ProjectModel oReturn =
                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetByIdLite("1EC791B6", "DA5C572E");

            Assert.IsNotNull(oReturn);
        }
    }
}
