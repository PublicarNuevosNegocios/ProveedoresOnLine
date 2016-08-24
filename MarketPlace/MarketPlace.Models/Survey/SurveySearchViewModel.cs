using MarketPlace.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Survey
{
    public class SurveySearchViewModel
    {
        public bool RenderScripts { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> ProviderOptions { get; set; }

        public List<ProviderLiteViewModel> ProviderSearchResult { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> ProviderFilterResult { get; set; }

        public string SearchParam { get; set; }

        public string SearchFilter { get; set; }
        
        public MarketPlace.Models.General.enumSurveySearchOrderType SearchOrderType { get; set; }

        public List<SurveyViewModel> SurveySearchResult { get; set; }

        public bool OrderOrientation { get; set; }

        public string OrderTypeOrientation { get { return ((int)SearchOrderType).ToString() + "_" + OrderOrientation.ToString(); } }

        public int PageNumber { get; set; }

        public int RowCount { get { return Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim()); } }

        public int TotalRows { get; set; }

        public int TotalPages { get { return (int)Math.Ceiling((decimal)((decimal)TotalRows / (decimal)RowCount)); } }

        #region Methods

        public Tuple<int, int> GetStartEndPage()
        {
            int ItemsxPage = 10;

            int oStart = (int)((PageNumber - (ItemsxPage / 2)) > 0 ? (PageNumber - (ItemsxPage / 2)) : 1);
            int oEnd = (int)(TotalPages >= oStart + (ItemsxPage / 2) ? oStart + (ItemsxPage / 2) : TotalPages);

            return new Tuple<int, int>(oStart, oEnd);
        }

        #endregion

    }
}
