using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class ProjectConfigViewModel
    {
        public ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel RelatedProjectProvider { get; private set; }

        public int TotalRows { get; set; }

        public string ProjectProviderId { get; set; }
        public string ProjectProviderName { get; set; }
        public bool ProjectProviderEnable { get; set; }

        public ProjectConfigViewModel() { }

        public ProjectConfigViewModel(ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel oRelatedProjectProvider)
        {
            RelatedProjectProvider = oRelatedProjectProvider;

            ProjectProviderId = oRelatedProjectProvider.ItemId.ToString();

            ProjectProviderName = oRelatedProjectProvider.ItemName;

            ProjectProviderEnable = oRelatedProjectProvider.Enable;
        }

        #region Methods

        public List<EvaluationItemViewModel> GetEvaluationCriteria()
        {
            List<EvaluationItemViewModel> oReturn =
                RelatedProjectProvider.RelatedEvaluationItem.
                    Where(ei => ei.ItemType.ItemId == (int)BackOffice.Models.General.enumEvaluationItemType.EvaluationCriteria &&
                          ei.ParentItem != null &&
                          ei.ParentItem.ItemId != null).
                    Select(ei => new EvaluationItemViewModel(ei)).
                    ToList();

            if (oReturn == null)
                oReturn = new List<EvaluationItemViewModel>();

            return oReturn;
        }

        #endregion
    }
}
