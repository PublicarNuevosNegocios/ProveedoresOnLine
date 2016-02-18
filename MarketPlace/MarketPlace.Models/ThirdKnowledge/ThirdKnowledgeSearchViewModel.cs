﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.ThirdKnowledge
{
    public class ThirdKnowledgeSearchViewModel
    {
        public List<ThirdKnowledgeViewModel> SurveySearchResult { get; set; }

        public bool OrderOrientation { get; set; }
                
        public int PageNumber { get; set; }

        public int RowCount { get { return Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim()); } }

        public int TotalRows { get; set; }

        public int TotalPages { get { return (int)Math.Ceiling((decimal)((decimal)TotalRows / (decimal)RowCount)); } }

        #region Methods

        public Tuple<int, int> GetStartEndPage()
        {
            int ItemsxPage = Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Paginator_PagesLimit].Value.Trim()); ;

            int oStart = (int)((PageNumber - (ItemsxPage / 2)) > 0 ? (PageNumber - (ItemsxPage / 2)) : 1);
            int oEnd = (int)(TotalPages >= oStart + (ItemsxPage / 2) ? oStart + (ItemsxPage / 2) : TotalPages);

            return new Tuple<int, int>(oStart, oEnd);
        }

        #endregion
    }
}
