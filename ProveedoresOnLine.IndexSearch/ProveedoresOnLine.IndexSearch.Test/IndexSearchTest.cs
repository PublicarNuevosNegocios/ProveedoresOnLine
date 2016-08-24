using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ProveedoresOnLine.IndexSearch.Models;

namespace ProveedoresOnLine.IndexSearch.Test
{
    [TestClass]
    public class IndexSearchTest
    {
        [TestMethod]
        public void GetAllCompanyIndexSearch()
        {
            List<IndexSearchModel> oReturn =
                Controller.IndexSearch.GetCompanyIndex();

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }
    }
}
