using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.Company.Test
{
    [TestClass]
    public class CompanyTest
    {
        [TestMethod]
        public void ContactGetBasicInfo()
        {
            List<Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.ContactGetBasicInfo
                ("1D9B9580", null);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void CategorySearchByGeography()
        {
            List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography
                ("cu", null, 0, 20);

            Assert.AreEqual(true, oReturn.Count >= 10);
        }

        [TestMethod]
        public void CategorySearchByActivity()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByActivity
                ("cu", 0, 20);

            Assert.AreEqual(true, oReturn.Count >= 10);
        }

        [TestMethod]
        public void CategorySearchByCustomActivity()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByCustomActivity
                ("cu", 0, 20);

            Assert.AreEqual(true, oReturn.Count >= 10);
        }
    }
}
