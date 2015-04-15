using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Project
{
    public class ProjectConfigViewModel
    {
        public ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel RelatedProjectConfig { get; private set; }

        public int ProjectConfigId { get { return RelatedProjectConfig.ItemId; } }

        public string ProjectConfigName { get { return RelatedProjectConfig.ItemName; } }

        public ProjectConfigViewModel(ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel oRelatedProjectConfig)
        {
            RelatedProjectConfig = oRelatedProjectConfig;
        }
    }
}
