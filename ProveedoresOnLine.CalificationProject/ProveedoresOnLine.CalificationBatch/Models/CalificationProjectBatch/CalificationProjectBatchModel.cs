using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProveedoresOnLine.Company.Models.Company;

namespace ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch
{
    public class CalificationProjectBatchModel
    {
        public int CalificationProjectId { get; set; }

        public string CalificationProjectPublicId { get; set; }

        public CompanyModel Company { get; set; }
        
        public int TotalScore { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel CalificationProjectItemBatchModel { get; set; }

        public ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemInfoBatchModel CalificationProjectItemInfoBatchModel { get; set; }

    }
}
