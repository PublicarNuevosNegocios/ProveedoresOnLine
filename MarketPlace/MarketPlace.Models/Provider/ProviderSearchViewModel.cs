using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderSearchViewModel
    {
        public bool RenderScripts { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> ProviderOptions { get; set; }

        public List<ProviderLiteViewModel> ProviderSearchResult { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> ProviderFilterResult { get; set; }

        public string SearchParam { get; set; }

        public string SearchFilter { get; set; }

        public MarketPlace.Models.General.enumSearchOrderType SearchOrderType { get; set; }

        public bool OrderOrientation { get; set; }

        public int PageNumber { get; set; }

        public int RowCount { get { return Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim()); } }

        public int TotalRows { get; set; }
    }
}
