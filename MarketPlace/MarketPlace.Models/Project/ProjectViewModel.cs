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

        public ProjectConfigViewModel RelatedProjectConfig { get; private set; }

        public List<ProjectProviderViewModel> RelatedProjectProvider { get; private set; }

        public ProjectProviderViewModel CurrentProjectProvider { get; private set; }

        public bool RenderScripts { get; set; }

        public int TotalRows { get; set; }

        #region Project Info

        public string ProjectPublicId { get { return RelatedProject.ProjectPublicId; } }

        public string ProjectName { get { return RelatedProject.ProjectName; } }

        public MarketPlace.Models.General.enumProjectStatus ProjectStatus { get { return (MarketPlace.Models.General.enumProjectStatus)RelatedProject.ProjectStatus.ItemId; } }

        public string LastModify { get { return RelatedProject.LastModify.ToString("yyyy-MM-dd"); } }

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

        public int ProjectAmmountId
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Ammount &&
                                   !string.IsNullOrEmpty(pjinf.Value)).
                    Select(pjinf => pjinf.ItemInfoId).
                    DefaultIfEmpty(0).
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

        public int ProjectExperienceYearsId
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.ExperienceYears).
                    Select(pjinf => pjinf.ItemInfoId).
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

        public string ProjectExperienceYearsValueName
        {
            get
            {
                string strExpYear = ProjectExperienceYears;
                string oReturn = string.Empty;

                if (!string.IsNullOrEmpty(strExpYear))
                {
                    string[] strSplit = strExpYear.Split('_');
                    if (strSplit.Length >= 3)
                    {
                        if (strSplit[1].Replace(" ", "") != "0")
                        {
                            switch ((MarketPlace.Models.General.enumProjectOperator)Convert.ToInt32(strSplit[2].Replace(" ", "")))
                            {
                                case MarketPlace.Models.General.enumProjectOperator.Equal:
                                    oReturn = "Igual a " + strSplit[1];
                                    break;
                                case MarketPlace.Models.General.enumProjectOperator.Higher:
                                    oReturn = "Mayor a " + strSplit[1];
                                    break;
                                case MarketPlace.Models.General.enumProjectOperator.GreaterOrEqual:
                                    oReturn = "Mayor o igual a " + strSplit[1];
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                return oReturn;
            }
        }

        public int ProjectExperienceQuantityId
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.ExperienceQuantity).
                    Select(pjinf => pjinf.ItemInfoId).
                    DefaultIfEmpty(0).
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

        public int ProjectDefaultEconomicActivityId
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.DefaultEconomicActivity).
                    Select(pjinf => pjinf.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public List<MarketPlace.Models.General.EconomicActivityViewModel> ProjectDefaultEconomicActivity
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
                    Select(cat => new MarketPlace.Models.General.EconomicActivityViewModel()
                    {
                        EconomicActivityId = cat.Split(',')[0].Replace(" ", ""),
                        ActivityName = cat.Split(',')[1],
                    }).
                    OrderBy(cat => cat.ActivityName).
                    ToList();
            }
        }

        public int ProjectCustomEconomicActivityId
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.CustomEconomicActivity).
                    Select(pjinf => pjinf.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public List<MarketPlace.Models.General.EconomicActivityViewModel> ProjectCustomEconomicActivity
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
                    Select(cat => new MarketPlace.Models.General.EconomicActivityViewModel()
                    {
                        EconomicActivityId = cat.Split(',')[0].Replace(" ", ""),
                        ActivityName = cat.Split(',')[1],
                    }).
                    OrderBy(cat => cat.ActivityName).
                    ToList();
            }
        }

        public int ProjectCurrencyId
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.CurrencyType).
                    Select(pjinf => pjinf.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
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

        public int ProjectInternalProcessNumberId
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.InternalProcessNumber &&
                                   !string.IsNullOrEmpty(pjinf.Value)).
                    Select(pjinf => pjinf.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public string ProjectInternalProcessNumber
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.InternalProcessNumber &&
                                   !string.IsNullOrEmpty(pjinf.Value)).
                    Select(pjinf => pjinf.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string ProjectCloseText
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.CloseText &&
                                   !string.IsNullOrEmpty(pjinf.LargeValue)).
                    Select(pjinf => pjinf.LargeValue).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string ProjectAwardText
        {
            get
            {
                return RelatedProject.ProjectInfo.
                    Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.AwardText &&
                                   !string.IsNullOrEmpty(pjinf.LargeValue)).
                    Select(pjinf => pjinf.LargeValue).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        List<MarketPlace.Models.General.FileModel> oProjectFile;
        public List<MarketPlace.Models.General.FileModel> ProjectFile
        {
            get
            {
                if (oProjectFile == null)
                {
                    oProjectFile = RelatedProject.ProjectInfo.
                        Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.File &&
                                       !string.IsNullOrEmpty(pjinf.LargeValue) &&
                                       pjinf.LargeValue.Split(',').Length >= 2).
                        Select(pjinf => new MarketPlace.Models.General.FileModel()
                        {
                            FileObjectId = pjinf.ItemInfoId.ToString(),
                            ServerUrl = pjinf.LargeValue.Split(',')[0],
                            FileName = pjinf.LargeValue.Split(',')[1]
                        }).
                        ToList();

                    if (oProjectFile == null)
                        oProjectFile = new List<MarketPlace.Models.General.FileModel>();
                }
                return oProjectFile;
            }
        }

        #endregion

        #region Constructors

        public ProjectViewModel(ProveedoresOnLine.ProjectModule.Models.ProjectModel oRelatedProject)
        {
            GenericConstructor(oRelatedProject, null);
        }

        public ProjectViewModel(ProveedoresOnLine.ProjectModule.Models.ProjectModel oRelatedProject, string ProviderPublicId)
        {
            GenericConstructor(oRelatedProject, ProviderPublicId);
        }

        #endregion

        #region Methods

        private void GenericConstructor(ProveedoresOnLine.ProjectModule.Models.ProjectModel oRelatedProject, string ProviderPublicId)
        {
            //get project info
            RelatedProject = oRelatedProject;

            //get project config info
            RelatedProjectConfig = new ProjectConfigViewModel(RelatedProject.RelatedProjectConfig);

            //get project provider info
            RelatedProjectProvider = new List<ProjectProviderViewModel>();

            if (RelatedProject.RelatedProjectProvider != null && RelatedProject.RelatedProjectProvider.Count > 0)
            {
                RelatedProject.RelatedProjectProvider.
                    OrderBy(pjpv => pjpv.RelatedProvider.RelatedCompany.CompanyName).
                    All(rp =>
                {
                    RelatedProjectProvider.Add(new ProjectProviderViewModel(rp));

                    if (!string.IsNullOrEmpty(ProviderPublicId) &&
                        rp.RelatedProvider.RelatedCompany.CompanyPublicId == ProviderPublicId)
                    {
                        CurrentProjectProvider = new ProjectProviderViewModel(rp);
                    }

                    return true;
                });
            }
        }

        public bool IsApprovalUser()
        {
            bool oReturn = false;

            if (ProjectStatus == MarketPlace.Models.General.enumProjectStatus.Approval &&
                CurrentProjectProvider.ApprovalStatus != null &&
                CurrentProjectProvider.ApprovalStatus == MarketPlace.Models.General.enumApprovalStatus.Pending &&
                RelatedProjectConfig.CurrentEvaluationArea != null)
            {
                //get approval users for current area
                List<string> oAprovalEmails = RelatedProjectConfig.CurrentEvaluationArea.GetEvaluatorsEmails();

                oReturn = oAprovalEmails != null &&
                        oAprovalEmails.Count > 0 &&
                        oAprovalEmails.Any(ae => ae.Replace(" ", "").ToLower() == MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email.Replace(" ", "").ToLower());
            }

            return oReturn;
        }

        #endregion
    }
}
