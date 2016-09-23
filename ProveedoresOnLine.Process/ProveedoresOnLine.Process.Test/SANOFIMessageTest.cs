using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.Process.Test
{
    /// <summary>
    /// Summary description for SANOFIMessageTest
    /// </summary>
    [TestClass]
    public class SANOFIMessageTest
    {
        [TestMethod]
        public void SANOFIMessage_Execute()
        {
            ProveedoresOnLine.Process.Implement.SANOFIProcessMessage Msj = new Implement.SANOFIProcessMessage();
            Msj.Execute(null);
        }
    }
}
