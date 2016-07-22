using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchProjectDevTest
{
    [TestClass]
    public class IndexTest
    {
        [TestMethod]
        public void Index()
        {
            ElasticSearchProjectDev.ElasticSearchProcess.ElasticSearchInit();
        }


        [TestMethod]
        public void SearchPerson()
        {
            ElasticSearchProjectDev.ElasticSearchProcess.SearchPerson("Sebastian");
        }
    }
}
