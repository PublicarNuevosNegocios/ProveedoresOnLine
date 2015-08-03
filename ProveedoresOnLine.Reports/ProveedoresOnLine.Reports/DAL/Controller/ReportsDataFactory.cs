using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.DAL.Controller
{
    class ReportsDataFactory
    {
        public ProveedoresOnLine.Reports.Interfaces.IReportData GetReportsInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.Reports.DAL.MySQLDAO.Reports_MySqlDao,ProveedoresOnLine.Reports");
            ProveedoresOnLine.Reports.Interfaces.IReportData oRetorno = (ProveedoresOnLine.Reports.Interfaces.IReportData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
