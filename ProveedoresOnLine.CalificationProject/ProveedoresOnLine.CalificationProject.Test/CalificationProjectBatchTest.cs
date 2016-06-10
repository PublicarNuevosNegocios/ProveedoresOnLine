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

        #region CalificationProjectBatchUtil

        #region LegalModule

        [TestMethod]
        public void LegalModuleInfo()
        {
            ProveedoresOnLine.Company.Models.Util.GenericItemModel oReturn =
                ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.LegalModuleInfo("1351D3F3", 603001);

            Assert.AreEqual(true, oReturn != null && oReturn.ItemInfo != null && oReturn.ItemInfo.Count > 0);
        }

        #endregion

        #endregion
    }
}