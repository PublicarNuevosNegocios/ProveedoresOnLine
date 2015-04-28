using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Project
{
    public class ProjectProviderViewModel
    {
        public ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel RelatedProjectProvider { get; private set; }

        public MarketPlace.Models.Provider.ProviderViewModel RelatedProvider { get; private set; }

        public ProjectProviderViewModel(ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel oRelatedProjectProvider)
        {
            RelatedProjectProvider = oRelatedProjectProvider;
            RelatedProvider = new Provider.ProviderViewModel()
            {
                RelatedLiteProvider = new Provider.ProviderLiteViewModel(RelatedProjectProvider.RelatedProvider),
            };

            #region Commercial

            RelatedProvider.RelatedComercialInfo = new List<MarketPlace.Models.Provider.ProviderComercialViewModel>();

            if (RelatedProjectProvider.RelatedProvider != null &&
                RelatedProjectProvider.RelatedProvider.RelatedCommercial != null &&
                RelatedProjectProvider.RelatedProvider.RelatedCommercial.Count > 0)
            {
                RelatedProjectProvider.RelatedProvider.RelatedCommercial.All(cm =>
                {
                    RelatedProvider.RelatedComercialInfo.Add(new MarketPlace.Models.Provider.ProviderComercialViewModel(cm));
                    return true;
                });

            }

            #endregion
        }

        #region Methods

        #region Project Company Info

        public decimal GetRatting(int vEvaluationItemId)
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItemId &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.Ratting &&
                                !string.IsNullOrEmpty(pjpvinf.Value)).
                Select(pjpvinf => Convert.ToDecimal(pjpvinf.Value, System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
                DefaultIfEmpty(0).
                FirstOrDefault();
        }

        public List<int> GetInfoItems(int vEvaluationItemId)
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItemId &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.InfoItems &&
                                !string.IsNullOrEmpty(pjpvinf.LargeValue)).
                Select(pjpvinf => pjpvinf.LargeValue).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault().
                Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                Select(strint => !string.IsNullOrEmpty(strint) ? Convert.ToInt32(strint) : 0).
                ToList();
        }

        #endregion

        #region EvaluationCriteria

        #region Experience

        public Tuple<decimal, int> GetExperienceTotal(int vEvaluationItemId)
        {
            List<int> lstExperienceId = GetInfoItems(vEvaluationItemId);

            decimal ExperienceSum = RelatedProvider.RelatedComercialInfo.
                Where(cm => cm.RelatedCommercialInfo.ItemType.ItemId == (int)MarketPlace.Models.General.enumCommercialType.Experience &&
                            lstExperienceId.Any(cmid => cm.RelatedCommercialInfo.ItemId == cmid)).
                Sum(cm => Convert.ToDecimal(cm.EX_ContractValue, System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")));

            int ExperienceCount = RelatedProvider.RelatedComercialInfo.
                Where(cm => cm.RelatedCommercialInfo.ItemType.ItemId == (int)MarketPlace.Models.General.enumCommercialType.Experience &&
                            lstExperienceId.Any(cmid => cm.RelatedCommercialInfo.ItemId == cmid)).
                Count();

            return new Tuple<decimal, int>(ExperienceSum, ExperienceCount);
        }

        #endregion

        #endregion

        #endregion
    }
}
