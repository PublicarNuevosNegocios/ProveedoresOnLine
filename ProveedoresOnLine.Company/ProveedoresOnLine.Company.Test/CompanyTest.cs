using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.Company.Test
{
    [TestClass]
    public class CompanyTest
    {
        [TestMethod]
        public void TreeGetByType()
        {
            List<ProveedoresOnLine.Company.Models.Util.TreeModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.TreeGetByType(114005);

            Assert.AreEqual(true, oReturn.Count > 0);
        }

        [TestMethod]
        public void TreeGetFullByType()
        {
            List<ProveedoresOnLine.Company.Models.Util.TreeModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.TreeGetFullByType(114014);

            Assert.AreEqual(true, oReturn.Count > 0);
        }

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
                ("186C3052", null, true);

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
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByBankAdmin(null, 0, 0, out oTotalRows);

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
        public void RoleCompanyGetByPublicId()
        {
            ProveedoresOnLine.Company.Models.Company.CompanyModel oReturn =
                ProveedoresOnLine.Company.Controller.Company.RoleCompany_GetByPublicId
                ("DA5C572E");

            Assert.AreEqual(true, oReturn.RelatedRole.Count >= 1);
        }

        [TestMethod]
        public void RoleCompanyGetUsersByPublicId()
        {
            List<ProveedoresOnLine.Company.Models.Company.UserCompany> oReturn =
                ProveedoresOnLine.Company.Controller.Company.RoleCompany_GetUsersByPublicId
                ("DA5C572E", true);

            Assert.AreEqual(true, oReturn.Count >= 1);
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
                ("", 0, 20, 4, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }

        [TestMethod]
        public void CategorySearchByEcoGroupAdmin()
        {
            int oTotalRows;
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByEcoGroupAdmin
                ("", 0, 20, 4, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }

        [TestMethod]
        public void MP_RoleCompanyGetByUser()
        {
            List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.MP_RoleCompanyGetByUser
                ("noexiste1@correo.com");

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void CategorySearchByTreeAdmin()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByTreeAdmin
                    ("", 0, 20);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void CategorySearchByICA()
        {
            int oTotalRows = 0;

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByICA
                    ("", 0, 20, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void BlackListGetByCompanyPublicId()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.BlackListGetByCompanyPublicId
                ("1D9B9580");

            Assert.AreEqual(true, oReturn.Count >= 10);
        }

        [TestMethod]
        public void CategorySearchByConutryAdmin()
        {
            int oTotalRows;

            List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByCountryAdmin
                ("Colo", 0, 20, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }

        [TestMethod]
        public void CategorySearchByStateAdmin()
        {
            int oTotalRows;

            List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchByStateAdmin
                ("Colo", "", 0, 20, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);

            Assert.AreEqual(true, oTotalRows > 0);
        }

        [TestMethod]
        public void CategorySearchBySurveyGroup()
        {
            int oTotalRows;

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.Company.Controller.Company.CategorySearchBySurveyGroup
                    (20, "", 0, 20, out oTotalRows);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void MinimumWageSearchByYear()
        {   
            ProveedoresOnLine.Company.Models.Util.MinimumWageModel oReturn = ProveedoresOnLine.Company.Controller.Company.MinimumWageSearchByYear(2015,988);

            Assert.AreEqual(true, oReturn != null);
        }

        [TestMethod]
        public void UserRoleSearchByUser()
        {
            List<ProveedoresOnLine.Company.Models.Company.UserCompany> oReturn =
                ProveedoresOnLine.Company.Controller.Company.UserRoleSearchByUser("DA5C572E", "d.al", 0, 0);

            Assert.AreEqual(true, oReturn != null);
        }
    }
}
