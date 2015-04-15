using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Project
{
    public class ProjectViewModel
    {
        public ProveedoresOnLine.ProjectModule.Models.ProjectModel RelatedProject { get; private set; }

        public List<MarketPlace.Models.Provider.ProviderLiteViewModel> RelatedProvider { get; set; }

        public int TotalRows { get; set; }

        public string ProjectPublicId { get { return RelatedProject.ProjectPublicId; } }

        public string ProjectName { get { return RelatedProject.ProjectName; } }

        public string LastModify { get { return RelatedProject.LastModify.ToString("yyyy-MM-dd"); } }

        public ProjectViewModel(ProveedoresOnLine.ProjectModule.Models.ProjectModel oRelatedProject)
        {
            RelatedProject = oRelatedProject;

            RelatedProvider = new List<Provider.ProviderLiteViewModel>();

            if (RelatedProject.RelatedProjectProvider != null && RelatedProject.RelatedProjectProvider.Count > 0)
            {
                RelatedProject.RelatedProjectProvider.All(rp =>
                {
                    RelatedProvider.Add(new Provider.ProviderLiteViewModel(rp.RelatedProvider));
                    return true;
                });
            }
        }
    }
}
