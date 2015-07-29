namespace MarketPlace.Models.Compare
{
    public class CompareSearchViewModel
    {
        public ProveedoresOnLine.CompareModule.Models.CompareModel RelatedCompare { get; private set; }

        public string CompareId { get { return RelatedCompare.CompareId.ToString(); } }

        public string CompareName { get { return RelatedCompare.CompareName; } }
        
        public string LastModify { get { return RelatedCompare.LastModify.ToString("yyyy-MM-dd"); } }

        public CompareSearchViewModel(ProveedoresOnLine.CompareModule.Models.CompareModel oRelatedCompare)
        {
            RelatedCompare = oRelatedCompare;
        }
    }
}