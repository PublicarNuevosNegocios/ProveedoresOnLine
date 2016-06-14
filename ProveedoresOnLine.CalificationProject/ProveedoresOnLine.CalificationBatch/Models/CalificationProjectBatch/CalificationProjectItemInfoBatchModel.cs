using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProveedoresOnLine.CalificationProject.Models.CalificationProject;

namespace ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch
{
    public class CalificationProjectItemInfoBatchModel
    {
        public int CalificationProjectItemInfoId { get; set; }

        public ConfigItemInfoModel CalificationProjectConfigItemInfoModel { get; set; }

        public int ItemInfoScore { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
