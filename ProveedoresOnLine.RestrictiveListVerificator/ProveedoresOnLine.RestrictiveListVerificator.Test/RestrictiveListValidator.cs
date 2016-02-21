using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListVerificator.Test
{
    [TestClass]
    class RestrictiveListValidator
    {
        [TestMethod]
        public void GenerateXLSByProviderStatus()
        {
            byte[] buffer = ProveedoresOnLine.RestrictiveListVerificator.Controller.RestrictiveListVerificator.GenerateXLSByStatus();
            
        }
    }
}
