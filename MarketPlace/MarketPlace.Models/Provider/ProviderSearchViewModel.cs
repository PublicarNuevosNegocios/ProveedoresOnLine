﻿using System;
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

        #region methods

        public List<Tuple<string, string>> GetlstSearchFilter()
        {
            List<Tuple<string, string>> oReturn = new List<Tuple<string, string>>();

            if (!string.IsNullOrEmpty(SearchFilter))
            {
                oReturn = SearchFilter.Replace(" ", "").
                    Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                    Where(x => x.IndexOf(';') >= 0).
                    Select(x => new Tuple<string, string>(x.Split(';')[0], x.Split(';')[1])).
                    Where(x => !string.IsNullOrEmpty(x.Item1) && !string.IsNullOrEmpty(x.Item2)).
                    ToList();
            }

            return oReturn;
        }

        public Tuple<int, int> GetStartEndPage()
        {
            int ItemsxPage = 10;

            decimal oTotalPages = Math.Ceiling((decimal)((decimal)TotalRows / (decimal)RowCount));

            int oStart = (int)((PageNumber - (ItemsxPage / 2)) > 0 ? (PageNumber - (ItemsxPage / 2)) : 1);
            int oEnd = (int)(oTotalPages >= oStart + (ItemsxPage / 2) ? oStart + (ItemsxPage / 2) : oTotalPages);

            return new Tuple<int, int>(oStart, oEnd);
        }

        #endregion
    }
}
