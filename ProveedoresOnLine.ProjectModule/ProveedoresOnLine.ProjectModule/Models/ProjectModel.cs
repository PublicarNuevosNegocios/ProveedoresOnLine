using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ProjectModule.Models
{
    public class ProjectModel
    {
        public string ProjectPublicId { get; set; }

        public string ProjectName { get; set; }

        public ProjectConfigModel RelatedProjectConfig { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }


        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> ProjectInfo { get; set; }

        public List<ProjectProviderModel> RelatedProjectProvider { get; set; }
    }
}
