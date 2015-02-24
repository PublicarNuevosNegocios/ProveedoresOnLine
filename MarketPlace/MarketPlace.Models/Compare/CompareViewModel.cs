using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Compare
{
    public class CompareViewModel
    {
        public ProveedoresOnLine.CompareModule.Models.CompareModel RelatedCompare { get; private set; }

        public int TotalRows { get; set; }

        public List<MarketPlace.Models.Provider.ProviderLiteViewModel> RelatedProvider { get; set; }

        public string CompareId { get { return RelatedCompare.CompareId.ToString(); } }

        public string CompareName { get { return RelatedCompare.CompareName; } }

        public string LastModify { get { return RelatedCompare.LastModify.ToString("yyyy-MM-dd"); } }

        #region Compare Controller attributes

        public bool RenderScripts { get; set; }

        public List<MarketPlace.Models.General.GenericMenu> CompareMenu { get; set; }

        public string CompareCurrency { get; set; }

        #endregion

        public CompareViewModel(ProveedoresOnLine.CompareModule.Models.CompareModel oRelatedCompare)
        {
            RelatedCompare = oRelatedCompare;

            RelatedProvider = new List<Provider.ProviderLiteViewModel>();

            if (RelatedCompare.RelatedProvider != null && RelatedCompare.RelatedProvider.Count > 0)
            {
                RelatedCompare.RelatedProvider.All(rp =>
                {
                    RelatedProvider.Add(new Provider.ProviderLiteViewModel(rp));
                    return true;
                });

            }
        }
    }
}
