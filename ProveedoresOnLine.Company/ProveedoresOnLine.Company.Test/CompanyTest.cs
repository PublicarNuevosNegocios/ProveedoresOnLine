﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.Company.Test
{
    [TestClass]
    public class CompanyTest
    {
        //[TestMethod]
        //public void CategorySearchByGeography()
        //{
        //    List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oReturn =
        //        ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography
        //        ("cu", null, 0, 20);

        //    Assert.AreEqual(true, oReturn.Count >= 10);
        //}

        [TestMethod]
        public void CategorySearchByRule()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByRules
                ("iso", 0, 0);

            Assert.AreEqual(true, oReturn.Count >= 10);
        }
    }
}
