using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch;

namespace ProveedoresOnLine.CalificationBatch.Controller
{
    public class CalificationProjectBatch
    {
        public static List<Models.CalificationProjectBatch.CalificationProjectBatchModel> CalificationProject_GetByCustomer(string vCustomerPublicid, string vProviderPublicId, bool Enable)
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.CalificationProject_GetByCustomer(vCustomerPublicid,vProviderPublicId,Enable);
        }
    }
}
