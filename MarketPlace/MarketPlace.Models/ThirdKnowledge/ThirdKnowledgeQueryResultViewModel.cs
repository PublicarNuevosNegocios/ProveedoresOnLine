using MarketPlace.Models.General;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Company
{
    public class ThirdKnowledgeQueryResultViewModel
    {
        public TDQueryModel oRelatedInfo { get; set; }
        //Cosl      

        public ThirdKnowledgeQueryResultViewModel(TDQueryModel oCollumnsResult)
        {
            oRelatedInfo = oCollumnsResult;
        }

        public ThirdKnowledgeQueryResultViewModel()
        {

        }
    }
}
