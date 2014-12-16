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
                ("1D9B9580", null);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void FinancialGetBasicInfo()
        {
            List<Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.FinancialGetBasicInfo
                ("1D9B9580", null);

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
    }
}
