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

        public int CompareCurrency { get; set; }

        public int? Year { get; set; }

        public MarketPlace.Models.General.enumCompareType CompareType { get; set; }

        public List<CompareDetailViewModel> CompareDetail { get; set; }

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

        #region Private methods

        public void GetCompareDetail()
        {
            switch (CompareType)
            {
                case General.enumCompareType.Commercial:
                    CompareDetail = GetCompareDetailCommercial();
                    break;
                case General.enumCompareType.Certifications:
                    CompareDetail = GetCompareDetailCertifications();
                    break;
                case General.enumCompareType.Financial:
                    CompareDetail = GetCompareDetailFinancial(null);
                    break;
                default:
                    break;
            }

        }

        private List<CompareDetailViewModel> GetCompareDetailCommercial()
        {
            List<CompareDetailViewModel> oReturn = new List<CompareDetailViewModel>();

            if (RelatedCompare.RelatedProvider != null && RelatedCompare.RelatedProvider.Count > 0)
            {
                //get all economic activity to eval
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> olstEvaluationArea = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

                RelatedCompare.RelatedProvider.
                    Where(rp => rp.CompareDetail != null && rp.CompareDetail.Count > 0).
                    All(rp =>
                    {
                        rp.CompareDetail.All(cd =>
                        {
                            if (!olstEvaluationArea.Any(ea => ea.ItemId == cd.EvaluationAreaId))
                            {
                                olstEvaluationArea.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = cd.EvaluationAreaId,
                                    ItemName = cd.EvaluationAreaName,
                                });
                            }
                            return true;
                        });
                        return true;
                    });
                olstEvaluationArea = olstEvaluationArea.OrderBy(x => x.ItemName).ToList();

                //get all compare detail items
                olstEvaluationArea.All(ea =>
                {
                    CompareDetailViewModel oItemToAdd =
                        new CompareDetailViewModel()
                        {
                            RelatedEvaluationArea = ea,
                            Colum = new List<Tuple<int, string>>() 
                            {
                                new Tuple<int,string>(1,"Número de experiencias"),
                                new Tuple<int,string>(2,"Valor"),
                            },
                            RelatedCompanyInfo = new List<ProveedoresOnLine.CompareModule.Models.CompareCompanyModel>(),
                        };

                    RelatedCompare.RelatedProvider.
                        All(rc =>
                        {
                            oItemToAdd.RelatedCompanyInfo.Add(rc);
                            return true;
                        });

                    //add account item
                    oReturn.Add(oItemToAdd);
                    return true;
                });
            }

            return oReturn;
        }

        private List<CompareDetailViewModel> GetCompareDetailFinancial(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAccount)
        {
            List<CompareDetailViewModel> oReturn = new List<CompareDetailViewModel>();

            MarketPlace.Models.Company.CompanyUtil.FinancialAccount.
                Where(ac =>
                    RelatedAccount != null ?
                        (ac.ParentItem != null && ac.ParentItem.ItemId == RelatedAccount.ItemId) :
                        (ac.ParentItem == null)).
                OrderBy(ac => ac.ItemInfo.
                    Where(aci => aci.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_Order).
                    Select(aci => Convert.ToInt32(aci.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault()).
                All(ac =>
                {
                    CompareDetailViewModel oItemToAdd =
                        new CompareDetailViewModel()
                        {
                            RelatedEvaluationArea = ac,
                            Colum = null,
                            RelatedCompanyInfo = new List<ProveedoresOnLine.CompareModule.Models.CompareCompanyModel>(),
                        };

                    //get account unit
                    string oAccountUnit = ac.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_Unit).
                        Select(y => y.Value.Replace(" ", "")).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    if (RelatedCompare.RelatedProvider != null && RelatedCompare.RelatedProvider.Count > 0)
                    {
                        RelatedCompare.RelatedProvider.
                            All(rc =>
                            {
                                //oAccountUnit == "$"
                                oItemToAdd.RelatedCompanyInfo.Add(rc);
                                return true;
                            });
                    }

                    //get child values
                    oItemToAdd.ChildCompareDetail = GetCompareDetailFinancial(ac);

                    //add account item
                    oReturn.Add(oItemToAdd);

                    return true;
                });

            return oReturn;
        }

        private List<CompareDetailViewModel> GetCompareDetailCertifications()
        {
            List<CompareDetailViewModel> oReturn = new List<CompareDetailViewModel>();

            if (RelatedCompare.RelatedProvider != null && RelatedCompare.RelatedProvider.Count > 0)
            {
                //get all economic activity to eval
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> olstEvaluationArea = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

                RelatedCompare.RelatedProvider.FirstOrDefault().CompareDetail.All(cd =>
                {
                    olstEvaluationArea.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                    {
                        ItemId = cd.EvaluationAreaId,
                        ItemName = cd.EvaluationAreaName,
                    });

                    return true;
                });
                olstEvaluationArea = olstEvaluationArea.OrderBy(x => x.ItemName).ToList();

                //get all compare detail items
                olstEvaluationArea.All(ea =>
                {
                    CompareDetailViewModel oItemToAdd =
                        new CompareDetailViewModel()
                        {
                            RelatedEvaluationArea = ea,
                            Colum = new List<Tuple<int, string>>() 
                            {
                                new Tuple<int,string>(1,"Valor"),
                                new Tuple<int,string>(2,"CCS"),
                            },
                            RelatedCompanyInfo = new List<ProveedoresOnLine.CompareModule.Models.CompareCompanyModel>(),
                        };

                    RelatedCompare.RelatedProvider.
                        All(rc =>
                        {
                            oItemToAdd.RelatedCompanyInfo.Add(rc);
                            return true;
                        });

                    //add account item
                    oReturn.Add(oItemToAdd);
                    return true;
                });
            }

            return oReturn;
        }

        #endregion
    }
}
