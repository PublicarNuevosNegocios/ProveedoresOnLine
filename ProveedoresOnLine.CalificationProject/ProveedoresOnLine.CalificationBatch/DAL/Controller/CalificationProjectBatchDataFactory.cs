using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.DAL.Controller
{
    internal class CalificationProjectBatchDataFactory
    {
        public CalificationBatch.Interfaces.ICalificationProjectBatchData GetCalificationProjectBatchInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.CalificationBatch.DAL.MySqlDAO.CalificationProjectBatch_MySqlDao,ProveedoresOnLine.CalificationProjectBatch");
            ProveedoresOnLine.CalificationBatch.Interfaces.ICalificationProjectBatchData oRetorno = (ProveedoresOnLine.CalificationBatch.Interfaces.ICalificationProjectBatchData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
