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

        //public decimal ProjectAmmount
        //{
        //    get
        //    {
        //        return RelatedProject.ProjectInfo.
        //            Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Ammount &&
        //                           !string.IsNullOrEmpty(pjinf.Value)).
        //            Select(pjinf => Convert.ToDecimal(pjinf.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
        //            DefaultIfEmpty(0).
        //            FirstOrDefault();
        //    }
        //}

        //public decimal ProjectExperienceYears
        //{
        //    get
        //    {
        //        return RelatedProject.ProjectInfo.
        //            Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Ammount &&
        //                           !string.IsNullOrEmpty(pjinf.Value)).
        //            Select(pjinf => Convert.ToDecimal(pjinf.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
        //            DefaultIfEmpty(0).
        //            FirstOrDefault();
        //    }
        //}

        //public decimal ProjectExperienceQuantity
        //{
        //    get
        //    {
        //        return RelatedProject.ProjectInfo.
        //            Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Ammount &&
        //                           !string.IsNullOrEmpty(pjinf.Value)).
        //            Select(pjinf => Convert.ToDecimal(pjinf.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
        //            DefaultIfEmpty(0).
        //            FirstOrDefault();
        //    }
        //}

        //public decimal ProjectDefaultEconomicActivity
        //{
        //    get
        //    {
        //        return RelatedProject.ProjectInfo.
        //            Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Ammount &&
        //                           !string.IsNullOrEmpty(pjinf.Value)).
        //            Select(pjinf => Convert.ToDecimal(pjinf.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
        //            DefaultIfEmpty(0).
        //            FirstOrDefault();
        //    }
        //}

        //public decimal ProjectCustomEconomicActivity
        //{
        //    get
        //    {
        //        return RelatedProject.ProjectInfo.
        //            Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Ammount &&
        //                           !string.IsNullOrEmpty(pjinf.Value)).
        //            Select(pjinf => Convert.ToDecimal(pjinf.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
        //            DefaultIfEmpty(0).
        //            FirstOrDefault();
        //    }
        //}

        //public decimal ProjectCurrencyType
        //{
        //    get
        //    {
        //        return RelatedProject.ProjectInfo.
        //            Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Ammount &&
        //                           !string.IsNullOrEmpty(pjinf.Value)).
        //            Select(pjinf => Convert.ToDecimal(pjinf.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
        //            DefaultIfEmpty(0).
        //            FirstOrDefault();
        //    }
        //}

        //public decimal ProjectResponsible
        //{
        //    get
        //    {
        //        return RelatedProject.ProjectInfo.
        //            Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Ammount &&
        //                           !string.IsNullOrEmpty(pjinf.Value)).
        //            Select(pjinf => Convert.ToDecimal(pjinf.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
        //            DefaultIfEmpty(0).
        //            FirstOrDefault();
        //    }
        //}

        #endregion

        #region Project Config Info

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
