using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.Process.Test
{
    [TestClass]
    public class RestrictiveListReadProcessJobTest
    {
        [TestMethod]
        public void RestrictiveListReadProcessJob_Execute()
        {
            ProveedoresOnLine.Process.Implement.RestrictiveListReadProcess SBJOb = new Implement.RestrictiveListReadProcess();
            SBJOb.Execute(null);
        }
    }
}
