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

        #endregion
    }
}
