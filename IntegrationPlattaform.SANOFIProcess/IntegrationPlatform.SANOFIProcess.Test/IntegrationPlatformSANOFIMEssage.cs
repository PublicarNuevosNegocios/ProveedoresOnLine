using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationPlatform.SANOFIProcess.Test
{
    [TestClass]
    public class IntegrationPlatformSANOFIMEssage
    {
        [TestMethod]
        public void SanofiMessage()
        {
            SANOFIMessage.SanofiSendMessage.StartProcess();
        }
    }
}
