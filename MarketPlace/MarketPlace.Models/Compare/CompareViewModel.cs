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

        public List<string> CompareColumns
        {
            get
            {
                if (oCompareColumns == null)
                {
                    oCompareColumns = new List<string>() { "EvaluationArea" };
                    oCompareColumns.AddRange(RelatedProvider.
                        Where(x => x.RelatedProvider != null && x.RelatedProvider.RelatedCompany != null).
                        Select(x => x.RelatedProvider.RelatedCompany.CompanyPublicId));
                }
                return oCompareColumns;
            }
        }
        private List<string> oCompareColumns;

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
            //switch (CompareType)
            //{
            //    case General.enumCompareType.Commercial:
            //        CompareDetail = GetCompareDetailCommercial();
            //        break;
            //    case General.enumCompareType.Certifications:
            //        CompareDetail = GetCompareDetailCertifications();
            //        break;
            //    case General.enumCompareType.Financial:
            //        CompareDetail = GetCompareDetailFinancial(null);
            //        break;
            //    default:
            //        break;
            //}
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

        #region Public methods

        private System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public string GetJsonColumns()
        {
            StringBuilder oReturn = new StringBuilder();

            oReturn.AppendLine("[");

            CompareColumns.All(col =>
            {
                oReturn.AppendLine("{");


                oReturn.AppendLine("width:'200px',");
                if (col == "EvaluationArea")
                {
                    oReturn.AppendLine("field:'" + col + "',");
                    oReturn.AppendLine("locked:true,");
                    oReturn.AppendLine("title:'Area de evaluación'");
                }
                else
                {
                    oReturn.AppendLine("field:'_" + col + "',");
                    oReturn.AppendLine("headerTemplate:Compare_DetailObject.GetHeaderTemplate('" + col + "'),");
                    oReturn.AppendLine("template:Compare_DetailObject.GetItemTemplate('" + col + "'),");
                }
                oReturn.AppendLine("},");
                return true;
            });

            return oReturn.ToString().Trim().TrimEnd(new char[] { ',' }) + "]";
        }

        public string GetJsonData()
        {
            StringBuilder oReturn = new StringBuilder();

            oReturn.AppendLine("[");

            switch (CompareType)
            {
                case General.enumCompareType.Commercial:
                    CompareDetail = GetCompareDetailCommercial();
                    break;
                case General.enumCompareType.Certifications:
                    CompareDetail = GetCompareDetailCertifications();
                    break;
                case General.enumCompareType.Financial:
                    oReturn.AppendLine(GetJsonDataFinancial(null));
                    break;
                default:
                    break;
            }


            //if (CompareDetail != null && CompareDetail.Count > 0)
            //{
            //    CompareDetail.All(reg =>
            //    {
            //        CompareColumns.All(col =>
            //        {
            //            oReturn.AppendLine("{");

            //            if (col == "EvaluationArea")
            //            {
            //                oReturn.AppendLine("EvaluationArea:'" + reg.RelatedEvaluationArea.ItemName + "',");
            //            }
            //            else
            //            {
            //                //oReturn.AppendLine("_" + col + ":" + reg.RelatedCompanyInfo.Where(comp => comp.RelatedCompany.CompanyPublicId == col).Select(comp => comp.CompareDetail.FirstOrDefault().Value.FirstOrDefault().Item1));
            //            }

            //            oReturn.AppendLine("},");

            //            return true;
            //        });

            //        return true;
            //    });
            //}

            return oReturn.ToString().Trim().TrimEnd(new char[] { ',' }) + "]";
        }

        public string GetJsonDataFinancial(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAccount)
        {
            StringBuilder oReturn = new StringBuilder();

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
                    //get child values
                    oReturn.AppendLine(GetJsonDataFinancial(ac));

                    //get account unit
                    string oAccountUnit = ac.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_Unit).
                        Select(y => y.Value.Replace(" ", "")).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    //get max value count
                    int oMaxValueCount = RelatedCompare.RelatedProvider.
                            Where(rp => rp.RelatedCompany != null &&
                                !string.IsNullOrEmpty(rp.RelatedCompany.CompanyPublicId) &&
                                rp.CompareDetail != null &&
                                rp.CompareDetail.Count > 0).
                            Max(rp =>
                                rp.CompareDetail.
                                    Where(cd => //cd.EvaluationAreaId == ac.ItemId &&
                                        cd.Value != null &&
                                        cd.Value.Count > 0).
                                    Max(cd => cd.Value.Count)
                            );


                    oReturn.AppendLine("{");

                    CompareColumns.All(col =>
                    {
                        if (col == "EvaluationArea")
                        {
                            oReturn.AppendLine("EvaluationArea:'" + ac.ItemName + "',");
                        }
                        else if (col != "EvaluationArea" && RelatedCompare.RelatedProvider != null)
                        {
                            oReturn.AppendLine("_" + col + ":{");

                            for (int i = 1; i <= oMaxValueCount; i++)
                            {
                                Tuple<int, string, string> ValItem = null;// = cd.Value.Where(val => val.Item1 == i).FirstOrDefault();

                                RelatedCompare.RelatedProvider.
                                    Where(rp => rp.RelatedCompany != null &&
                                        rp.RelatedCompany.CompanyPublicId == col &&
                                        rp.CompareDetail != null &&
                                        rp.CompareDetail.Count > 0).
                                    All(rp =>
                                    {
                                        rp.CompareDetail.
                                            Where(cd => cd.EvaluationAreaId == ac.ItemId &&
                                            cd.Value != null &&
                                            cd.Value.Count > 0).
                                                All(cd =>
                                                {
                                                    ValItem = cd.Value.Where(val => val.Item1 == i).FirstOrDefault();
                                                    return true;
                                                });

                                        return true;
                                    });

                                if (ValItem != null)
                                {
                                    oReturn.AppendLine("Value" +
                                        i.ToString() +
                                        ":'" +
                                        ValItem.Item2 +
                                        "',Unit" +
                                        i.ToString() + ":'" +
                                        ValItem.Item3 +
                                        "',");
                                }
                                else
                                {
                                    oReturn.AppendLine("Value" +
                                            i.ToString() +
                                            ":'',Unit" +
                                            i.ToString() + ":'',");
                                }

                            }
                            oReturn.AppendLine("},");
                        }
                        return true;
                    });

                    oReturn.AppendLine("},");

                    return true;
                });

            return oReturn.ToString();
        }

        #endregion
    }
}
