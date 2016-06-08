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

        public int CalificationProjectPublicId { get; set; }

        public CompanyModel Company { get; set; }

        public ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel CalificationProjectConfigModel { get; set; }

        public int TotalScore { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
