using ProveedoresOnLine.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.DAL.MySQLDAO
{
    internal class Reports_MySqlDao : IReportData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Reports_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.Reports.Models.Constants.R_POL_ReportsConnectionName);
        }

        //public string ReportsUpsert(string Name, string Attributes)
        //{

        //}
    }
}
