using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.Test
{
    [TestClass]
    public class RestrictiveListProcessTest
    {
        [TestMethod]
        public void TestDll()
        {
            string test = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.DllTest();
        }

    }
}
