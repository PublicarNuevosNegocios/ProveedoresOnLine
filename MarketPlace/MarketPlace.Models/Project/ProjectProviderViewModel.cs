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

        public MarketPlace.Models.General.enumApprovalStatus? GetApprovalStatusByArea(int vEvaluationItemId)
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItemId &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus &&
                                !string.IsNullOrEmpty(pjpvinf.Value)).
                Select(pjpvinf => (MarketPlace.Models.General.enumApprovalStatus?)Convert.ToInt32(pjpvinf.Value.Replace(" ", ""))).
                DefaultIfEmpty(null).
                FirstOrDefault();
        }

        /// <summary>
        /// validate all approval status for provider
        /// </summary>
        /// <returns>null not request, enum values for any status</returns>
        public MarketPlace.Models.General.enumApprovalStatus? GetApprovalStatus()
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem == null &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus &&
                                !string.IsNullOrEmpty(pjpvinf.Value)).
                Select(pjpvinf => (MarketPlace.Models.General.enumApprovalStatus?)Convert.ToInt32(pjpvinf.Value.Replace(" ", ""))).
                DefaultIfEmpty(null).
                FirstOrDefault();
        }

        public string GetApprovalText(int vEvaluationItemId)
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItemId &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalText &&
                                !string.IsNullOrEmpty(pjpvinf.LargeValue)).
                Select(pjpvinf => pjpvinf.LargeValue).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
        }

        #endregion
    }
}
