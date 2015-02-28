using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.CompareModule.Test
{
    [TestClass]
    public class CompareModuleTest
    {
        [TestMethod]
        public void CompareGetDetailByType()
        {
            var oResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.CompareGetDetailByType
                (5, 2, "noexiste1@correo.com", null, "1A9863BD");

            Assert.AreEqual(true, oResult.RelatedProvider.Count > 0);

            oResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.CompareGetDetailByType
               (3, 2, "noexiste1@correo.com", null, "1A9863BD");

            Assert.AreEqual(true, oResult.RelatedProvider.Count > 0);

            oResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.CompareGetDetailByType
               (7, 2, "noexiste1@correo.com", null, "1A9863BD");

            Assert.AreEqual(true, oResult.RelatedProvider.Count > 0);

        }
    }
}
