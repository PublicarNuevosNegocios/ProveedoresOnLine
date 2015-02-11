using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.CompanyProvider.Test
{
    [TestClass]
    public class CompanyProviderTest
    {
        [TestMethod]
        public void CommercialGetBasicInfo()
        {
            List<Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CommercialGetBasicInfo
                ("1D9B9580", null, false);

            Assert.AreEqual(true, oReturn != null && oReturn.Count >= 1);
        }

        [TestMethod]
        public void FinancialGetBasicInfo()
        {
            List<Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.FinancialGetBasicInfo
                ("1D9B9580", null, true);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void BalanceSheetGetByFinancial()
        {
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel> oReturn =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetGetByFinancial
                (1);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void LegalInfoGetByLegalType()
        {
            // List<CatalogModel> oReturn = new List<CatalogModel>();
            //return (LegalId, LegalInfoId, LegalInfoTypeId, Value, LargeValue, Enable);
        }

        #region MarketPlace

        [TestMethod]
        public void MPProviderSearch()
        {
            int oTotalRows;

            List<Models.Provider.ProviderModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearch
                    ("1A9863BD",
                    "a",
                    "",
                    113002,
                    false,
                    0,
                    20,
                    out oTotalRows);

            Assert.AreEqual(true, oResult.Count > 0);

        }

        [TestMethod]
        public void MPProviderSearchFilter()
        {
            List<Company.Models.Util.GenericFilterModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchFilter
                    ("1A9863BD",
                    "a",
                    null);

            Assert.AreEqual(true, oResult.Count > 0);

        }


        [TestMethod]
        public void MPProviderSearchById()
        {
            List<Models.Provider.ProviderModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                    ("1A9863BD",
                    "C44A0CBF,2F8EF68D,");

            Assert.AreEqual(true, oResult.Count > 0);
        }


        #endregion
    }
}
