using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.Company.Test
{
    [TestClass]
    public class CompanyTest
    {
        [TestMethod]
        public void CategorySearchByGeography()
        {
            int oTotalCount;
            List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography
                ("cu", null, 0, 20, out oTotalCount);

            Assert.AreEqual(true, oReturn.Count >= 10);
        }

        [TestMethod]
        public void ContactGetBasicInfo()
        {
            List<Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.ContactGetBasicInfo
                ("1D9B9580", null);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void CategorySearchByRule()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByRules
                ("iso", 0, 0);

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

        [TestMethod]
        public void CategoryGetFinantialAccounts()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategoryGetFinantialAccounts();

            Assert.AreEqual(true, oReturn.Count >= 10);
        }

        [TestMethod]
        public void CurrencyExchangeGetByMoneyType()
        {
            List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetByMoneyType
                (108002, 108001, null);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void CompanyGetBasicInfo()
        {
            ProveedoresOnLine.Company.Models.Company.CompanyModel oReturn =
                ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo
                ("2F8EF68D");

            Assert.IsNotNull(oReturn);
        }

        [TestMethod]
        public void CompanySearchFilter()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CompanySearchFilter
                ("202001,202003", null, null);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void CompanySearch()
        {
            int oTotalRows;

            List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CompanySearch
                ("202001,202003", null, null, 1, 5, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }

        [TestMethod]
        public void CategorySearchByGeographyAdmin()
        {
            int oTotalRows;
             
            List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeographyAdmin
                ("Colo", 0, 5, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }

        [TestMethod]
        public void CategorySearchByBankAdmin()
        {
            int oTotalRows;

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByBankAdmin("Colo", 0, 5, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }        

        [TestMethod]
        public void CategorySearchByCompanyRuleAdmin()
        {
            int oTotalRows;
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByCompanyRulesAdmin
                ("icon", 0, 5, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }

        [TestMethod]
        public void CategorySearchByRulesAdmin()
        {
            int oTotalRows;
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByRulesAdmin
                ("", 0, 5, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }

        [TestMethod]
        public void CategorySearcgByResolutionAdmin()
        {
            int oTotalRows;
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByResolutionAdmin
                ("", 0, 5, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }

        [TestMethod]
        public void CategorySearchByEcoActivityAdmin()
        {
            int oTotalRows;
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByEcoActivityAdmin
                ("", 0, 5, 4, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }
    }
}
