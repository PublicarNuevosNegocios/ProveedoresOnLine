using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.Process.Test
{
    [TestClass]
    public class RestrictiveListSendProcessJobTest
    {
        [TestMethod]
        public void RestrictiveListSendProcessJob_Execute()
        {
            ProveedoresOnLine.Process.Implement.RestrictiveListSendProcess SBJOb = new Implement.RestrictiveListSendProcess();
            SBJOb.Execute(null);
        }
    }
}
