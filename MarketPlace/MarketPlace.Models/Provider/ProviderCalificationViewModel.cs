using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderCalificationViewModel
    {
        public List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> ProRelatedCalificationProject { get; set; }

        public string TotalCalification { get; set; }

        public int TotalScore { get; set; }
    }
}
