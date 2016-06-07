using ProveedoresOnLine.CalificationBatch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.DAL.MySqlDAO
{
    internal class CalificationProjectBatch_MySqlDao : ICalificationProjectBatchData
    {
        private ADO.Interfaces.IADO DataInstance;

        public CalificationProjectBatch_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.CalificationBatch.Models.Constants.C_POL_CalificatioProjectConnectionName);
        }
    }
}
