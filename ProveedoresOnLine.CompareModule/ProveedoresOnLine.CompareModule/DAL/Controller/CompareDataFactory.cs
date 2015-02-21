using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompareModule.DAL.Controller
{
    internal class CompareDataFactory
    {
        public ProveedoresOnLine.CompareModule.Interfaces.ICompareData GetCompareInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.CompareModule.DAL.MySQLDAO.Compare_MySqlDao,ProveedoresOnLine.CompareModule");
            ProveedoresOnLine.CompareModule.Interfaces.ICompareData oRetorno = (ProveedoresOnLine.CompareModule.Interfaces.ICompareData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
