using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch
{
    public class CalificationProjectItemBatchModel
    {
        public int CalificationProjectItemId { get; set; }

        public int CalificationProjectId { get; set; }

        public ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemModel CalificationProjectConfigItem { get; set; }

        public int ItemScore { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public CalificationProjectItemInfoBatchModel CalificatioProjectItemInfoModel { get; set; }

    }
}
