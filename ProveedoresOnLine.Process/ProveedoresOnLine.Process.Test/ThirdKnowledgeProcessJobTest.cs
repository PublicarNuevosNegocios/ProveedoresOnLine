﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.Process.Test
{
    [TestClass]
    public class ThirdKnowledgeProcessJobTest
    {
        [TestMethod]
        public void SurveySendProcessJob_Execute()
        {
            ProveedoresOnLine.Process.Implement.ThirdKnowledgeProcessJob SBJOb = new Implement.ThirdKnowledgeProcessJob();
            SBJOb.Execute(null);
        }
    }
}
