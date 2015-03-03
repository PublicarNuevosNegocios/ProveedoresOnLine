using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageModule.Test
{
    [TestClass]
    public class ServerTest
    {
        [TestMethod]
        public void ProcessMessage()
        {
            MessageModule.Controller.MessageProcess.StartProcess();
        }
    }
}
