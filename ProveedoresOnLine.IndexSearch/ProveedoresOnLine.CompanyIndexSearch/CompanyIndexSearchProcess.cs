using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyIndexSearch
{
    public class CompanyIndexSearchProcess
    {
        public static void StartProcess()
        {
            ProveedoresOnLine.IndexSearch.Controller.IndexSearch.CompanyIndexationFunction();

            //ProveedoresOnLine.IndexSearch.Controller.IndexSearch.CustomerProviderIdexationFunction();
        }
    }
}
