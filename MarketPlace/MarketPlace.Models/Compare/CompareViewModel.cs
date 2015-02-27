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

        public List<MarketPlace.Models.Provider.ProviderLiteViewModel> RelatedProvider { get; set; }

        public int TotalRows { get; set; }

        public string CompareId { get { return RelatedCompare.CompareId.ToString(); } }

        public string CompareName { get { return RelatedCompare.CompareName; } }

        public string LastModify { get { return RelatedCompare.LastModify.ToString("yyyy-MM-dd"); } }

        public CompareViewModel(ProveedoresOnLine.CompareModule.Models.CompareModel oRelatedCompare)
        {
            if (oRelatedCompare != null)
                RelatedCompare = oRelatedCompare;
            else
                RelatedCompare = new ProveedoresOnLine.CompareModule.Models.CompareModel();

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
