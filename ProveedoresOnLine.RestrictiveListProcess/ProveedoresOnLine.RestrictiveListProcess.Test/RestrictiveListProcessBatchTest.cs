using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.RestrictiveListProcess.Test
{
    [TestClass]
    public class RestrictiveListProcessBatchTest
    {
        [TestMethod]
        public void StartProcessTest()
        {
            ProveedoresOnLine.RestrictiveListProcessBatch.RestrictiveListSendProcess.StartProcess();
        }
    }
}
