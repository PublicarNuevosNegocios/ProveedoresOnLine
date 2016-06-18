using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.Process.Test
{
    /// <summary>
    /// Summary description for SANOFIIntegrationProcessJobTest
    /// </summary>
    [TestClass]
    public class SANOFIIntegrationProcessJobTest
    {
        [TestMethod]
        public void SANOFI_StartProcessJob()
        {
            ProveedoresOnLine.Process.Implement.SANOFIProcess SBJOb = new Implement.SANOFIProcess();
            SBJOb.Execute(null);
        }
    }
}
