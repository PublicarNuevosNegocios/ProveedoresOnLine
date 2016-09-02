using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Process.Test
{
    [TestClass]
    public class CompanyIndexProcessJobTest
    {
        [TestMethod]
        public void CompanyIndexProcessJob_Execute()
        {
            ProveedoresOnLine.Process.Implement.CompanyIndexProcessJob SBJOb = new Implement.CompanyIndexProcessJob();
            SBJOb.Execute(null);
        }
    }    
}
