﻿using System;
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

                return oReturn;
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

        List<Tuple<string, string>> oProjectFile;
        public List<Tuple<string, string>> ProjectFile
        {
            get
            {
                if (oProjectFile == null)
                {
                    oProjectFile = RelatedProject.ProjectInfo.
                        Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.File &&
                                       !string.IsNullOrEmpty(pjinf.LargeValue) &&
                                       pjinf.LargeValue.Split(',').Length >= 2).
                        Select(pjinf => new Tuple<string, string>(pjinf.LargeValue.Split(',')[0], pjinf.LargeValue.Split(',')[1])).
                        ToList();

                    if (oProjectFile == null)
                        oProjectFile = new List<Tuple<string, string>>();
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

        #endregion
    }
}
