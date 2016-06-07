using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.DAL.Controller 
{
    internal class CalificationProjectBatchDataController : ProveedoresOnLine.CalificationBatch.Interfaces.ICalificationProjectBatchData
    {
        #region singleton instance

        private static ProveedoresOnLine.CalificationBatch.Interfaces.ICalificationProjectBatchData oInstance;

        internal static ProveedoresOnLine.CalificationBatch.Interfaces.ICalificationProjectBatchData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new CalificationProjectBatchDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.CalificationBatch.Interfaces.ICalificationProjectBatchData DataFactory;

        #endregion

         #region constructor

        public CalificationProjectBatchDataController()
        {
            CalificationBatch.DAL.Controller.CalificationProjectBatchDataFactory factory = new CalificationBatch.DAL.Controller.CalificationProjectBatchDataFactory();
            DataFactory = factory.GetCalificationProjectBatchInstance();
        }

        #endregion
    }
}
