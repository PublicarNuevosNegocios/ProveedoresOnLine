using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.CalificationProject.Test
{
    /// <summary>
    /// Summary description for CalificationProjectBatchTest
    /// </summary>
    [TestClass]
    public class CalificationProjectBatchTest
    {
        [TestMethod]
        public void StartProcess()
        {
            ProveedoresOnLine.CalificationBatch.CalificationProcess.StartProcess();
        }

        [TestMethod]
        public void CalificationProject_GetByCustomer()
        {
            List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> oReturn = new List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel>();
            oReturn = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProject_GetByCustomer("1B40C887","4CD75091", true);

            Assert.AreEqual(true, oReturn.Count > 0);
        }
    }
}