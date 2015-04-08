using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.Process.Test
{
    [TestClass]
    public class SurveySendProcessJobTest
    {
        [TestMethod]
        public void SurveySendProcessJob_Execute()
        {
            ProveedoresOnLine.Process.Implement.SurveySendProcessJob SBJOb = new Implement.SurveySendProcessJob();
            SBJOb.Execute(null);
        }
    }
}
