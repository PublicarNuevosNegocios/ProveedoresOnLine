using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListVerificator.Test
{
    [TestClass]
    public class RestrictiveListValidatorTest
    {
        [TestMethod]
        public void GenerateXLSByProviderStatus()
        {
            var buffer = ProveedoresOnLine.RestrictiveListVerificator.Controller.RestrictiveListVerificator.GenerateXLSByStatus();
        }
    }
}
