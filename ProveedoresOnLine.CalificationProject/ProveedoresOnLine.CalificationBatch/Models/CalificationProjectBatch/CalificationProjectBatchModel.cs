﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.CalificationProject.Models.CalificationProject;

namespace ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch
{
    public class CalificationProjectBatchModel
    {
        public int CalificationProjectId { get; set; }

        public string CalificationProjectPublicId { get; set; }

        public CalificationProjectConfigModel ProjectConfigModel { get; set; }

        public CompanyModel RelatedProvider { get; set; }
        
        public int TotalScore { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel> CalificationProjectItemBatchModel { get; set; }

        

    }
}
