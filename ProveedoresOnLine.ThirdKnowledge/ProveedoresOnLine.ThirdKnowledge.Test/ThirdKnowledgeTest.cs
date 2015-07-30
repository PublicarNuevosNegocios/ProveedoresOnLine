using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.ThirdKnowledge.Test
{
    [TestClass]
    public class ThirdKnowledgeTest
    {
        [TestMethod]
        public void SimpleRequest()
        {
            ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.SimpleRequest("70041053", "Alvaro uribe Velez");


        }
    }
}
