using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch
{
    public class CalificationProjectItemInfoBatchModel
    {
        public int CalificationProjectItemInfoId { get; set; }

        public int CalificationProjectItemId { get; set; }

        public ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemInfoModel CalificationProjectConfigItemInfo { get; set; }

        public int ItemInfoScore { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
