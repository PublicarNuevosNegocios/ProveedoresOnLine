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

        public bool RenderScripts { get; set; }

        public int TotalRows { get; set; }

        public string ProjectPublicId { get { return RelatedProject.ProjectPublicId; } }

        public string ProjectName { get { return RelatedProject.ProjectName; } }

        public string LastModify { get { return RelatedProject.LastModify.ToString("yyyy-MM-dd"); } }

        #region Project Info

        public int? ProjectCompareId
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Compare &&
                                   !string.IsNullOrEmpty(pjinf.Value)).
                    Select(pjinf => (int?)Convert.ToInt32(pjinf.Value.Replace(" ", ""))).
                    DefaultIfEmpty(null).
                    FirstOrDefault();
            }
        }

        public decimal ProjectAmmount
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Ammount &&
                                   !string.IsNullOrEmpty(pjinf.Value)).
                    Select(pjinf => Convert.ToDecimal(pjinf.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public string ProjectExperienceYears
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.ExperienceYears &&
                                   !string.IsNullOrEmpty(pjinf.Value)).
                    Select(pjinf => pjinf.Value.Replace(" ", "")).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public int? ProjectExperienceQuantity
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.ExperienceQuantity &&
                                   !string.IsNullOrEmpty(pjinf.Value)).
                    Select(pjinf => (int?)Convert.ToInt32(pjinf.Value.Replace(" ", ""))).
                    DefaultIfEmpty(null).
                    FirstOrDefault();
            }
        }

        public List<Tuple<int, string>> ProjectDefaultEconomicActivity
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.DefaultEconomicActivity &&
                                   !string.IsNullOrEmpty(pjinf.ValueName)).
                    Select(pjinf => pjinf.ValueName).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault().
                    Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                    Where(cat => !string.IsNullOrEmpty(cat) &&
                                cat.Split(',').Length >= 2).
                    Select(cat => new Tuple<int, string>(Convert.ToInt32(cat.Split(',')[0].Replace(" ", "")), cat.Split(',')[1])).
                    OrderBy(cat => cat.Item2).
                    ToList();
            }
        }

        public List<Tuple<int, string>> ProjectCustomEconomicActivity
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.CustomEconomicActivity &&
                                   !string.IsNullOrEmpty(pjinf.ValueName)).
                    Select(pjinf => pjinf.ValueName).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault().
                    Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                    Where(cat => !string.IsNullOrEmpty(cat) &&
                                cat.Split(',').Length >= 2).
                    Select(cat => new Tuple<int, string>(Convert.ToInt32(cat.Split(',')[0].Replace(" ", "")), cat.Split(',')[1])).
                    OrderBy(cat => cat.Item2).
                    ToList();
            }
        }

        public int ProjectCurrencyTypeId
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.CurrencyType &&
                                   !string.IsNullOrEmpty(pjinf.Value)).
                    Select(pjinf => Convert.ToInt32(pjinf.Value.Replace(" ", ""))).
                    DefaultIfEmpty(Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value)).
                    FirstOrDefault();
            }
        }

        public string ProjectCurrencyTypeName
        {
            get
            {
                return MarketPlace.Models.Company.CompanyUtil.ProviderOptions.
                    Where(cprv => cprv.CatalogId == 108 && cprv.ItemId == ProjectCurrencyTypeId).
                    Select(cprv => cprv.ItemName).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string ProjectResponsible
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Responsible &&
                                   !string.IsNullOrEmpty(pjinf.Value)).
                    Select(pjinf => pjinf.Value.Replace(" ", "")).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        #endregion

        #region Project Config Info

        public int ProjectConfigId { get { return RelatedProject.RelatedProjectConfig.ItemId; } }

        public string ProjectConfigName { get { return RelatedProject.RelatedProjectConfig.ItemName; } }

        private ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel oProjectConfigExperience;
        public ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel ProjectConfigExperience
        {
            get
            {
                if (oProjectConfigExperience == null)
                {
                    string strConfig = RelatedProject.RelatedProjectConfig.RelatedEvaluationItem.
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
                            YearsEnable = false,
                        };
                    }
                }

                return oProjectConfigExperience;
            }
        }

        #endregion

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
