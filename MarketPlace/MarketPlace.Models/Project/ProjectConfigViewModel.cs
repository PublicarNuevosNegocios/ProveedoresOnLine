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

        private ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel oProjectConfigExperience;
        public ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel ProjectConfigExperience
        {
            get
            {
                if (oProjectConfigExperience == null)
                {
                    string strConfig = RelatedProjectConfig.RelatedEvaluationItem.
                        Where(ei => ei.ItemInfo.
                            Any(eiinf => eiinf.ItemInfoType != null &&
                                        eiinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumEvaluationItemInfoType.EvaluationExperienceConfig &&
                                        !string.IsNullOrEmpty(eiinf.LargeValue))).
                        Select(ei => ei.ItemInfo.
                            Where(eiinf => eiinf.ItemInfoType != null &&
                                        eiinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumEvaluationItemInfoType.EvaluationExperienceConfig &&
                                        !string.IsNullOrEmpty(eiinf.LargeValue)).
                            Select(eiinf => eiinf.LargeValue).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    if (!string.IsNullOrEmpty(strConfig))
                    {
                        oProjectConfigExperience = (ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel)
                            (new System.Web.Script.Serialization.JavaScriptSerializer()).
                            Deserialize(strConfig, typeof(ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel));
                    }
                    else
                    {
                        oProjectConfigExperience = new ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel()
                        {
                            AmmounEnable = false,
                            CurrencyEnable = false,
                            CustomAcitvityEnable = false,
                            DefaultAcitvityEnable = false,
                        };
                    }
                }

                return oProjectConfigExperience;
            }
        }

        public ProjectConfigViewModel(ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel oRelatedProjectConfig)
        {
            RelatedProjectConfig = oRelatedProjectConfig;
        }

        #region Methods

        public List<EvaluationItemViewModel> GetEvaluationAreas()
        {
            List<EvaluationItemViewModel> oReturn =
                RelatedProjectConfig.RelatedEvaluationItem.
                Where(ei => ei.ItemType.ItemId == (int)MarketPlace.Models.General.enumEvaluationItemType.EvaluationArea).
                OrderBy(ei => ei.ItemInfo.
                    Where(eiinf => eiinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumEvaluationItemInfoType.EvaluationOrder &&
                                    !string.IsNullOrEmpty(eiinf.Value)).
                    Select(eiinf => Convert.ToInt32(eiinf.Value.Replace(" ", ""))).
                    DefaultIfEmpty(0).
                    FirstOrDefault()).
                Select(ei => new EvaluationItemViewModel(ei)).
                ToList();

            if (oReturn == null)
                oReturn = new List<EvaluationItemViewModel>();

            return oReturn;
        }

        public List<EvaluationItemViewModel> GetEvaluationCriteria(int oEvaluationAreaId)
        {
            List<EvaluationItemViewModel> oReturn =
                RelatedProjectConfig.RelatedEvaluationItem.
                Where(ei => ei.ItemType.ItemId == (int)MarketPlace.Models.General.enumEvaluationItemType.EvaluationCriteria &&
                            ei.ParentItem != null &&
                            ei.ParentItem.ItemId == oEvaluationAreaId).
                OrderBy(ei => ei.ItemInfo.
                    Where(eiinf => eiinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumEvaluationItemInfoType.EvaluationOrder &&
                                    !string.IsNullOrEmpty(eiinf.Value)).
                    Select(eiinf => Convert.ToInt32(eiinf.Value.Replace(" ", ""))).
                    DefaultIfEmpty(0).
                    FirstOrDefault()).
                Select(ei => new EvaluationItemViewModel(ei)).
                ToList();

            if (oReturn == null)
                oReturn = new List<EvaluationItemViewModel>();

            return oReturn;
        }

        #endregion
    }
}
