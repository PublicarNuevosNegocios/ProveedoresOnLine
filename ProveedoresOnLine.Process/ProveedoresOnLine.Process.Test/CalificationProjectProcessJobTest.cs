using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.Process.Test
{
    [TestClass]
    public class CalificationProjectProcessJobTest
    {
        [TestMethod]
        public void CalificationProjectProcessJob()
        {
            ProveedoresOnLine.Process.Implement.CalificationProjectProcessJob CalificationProjectBatch = new Implement.CalificationProjectProcessJob();

            CalificationProjectBatch.Execute(null);
        }
    }
}
