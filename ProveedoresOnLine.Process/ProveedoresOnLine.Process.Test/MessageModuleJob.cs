using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.Process.Test
{
    [TestClass]
    public class MessageModuleJob
    {
        [TestMethod]
        public void MessageModuleJob_Execute()
        {
            ProveedoresOnLine.Process.Implement.MessageModuleJob MMJOb = new Implement.MessageModuleJob();
            MMJOb.Execute(null);
        }
    }
}
