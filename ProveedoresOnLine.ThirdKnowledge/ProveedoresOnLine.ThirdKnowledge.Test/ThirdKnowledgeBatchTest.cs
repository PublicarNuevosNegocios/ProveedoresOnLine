using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System.Collections.Generic;

namespace ProveedoresOnLine.ThirdKnowledge.Test
{
    [TestClass]
    public class ThirdKnowledgeBatchTest
    {
        [TestMethod]
        public void GetQueriesInProgress()
        {
            ProveedoresOnLine.ThirdKnowledgeBatch.ThirdKnowledgeFTPProcess.StartProcess();            
        }
    }
}
