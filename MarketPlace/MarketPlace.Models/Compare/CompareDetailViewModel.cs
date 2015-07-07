using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Compare
{
    public class CompareDetailViewModel
    {
        public ProveedoresOnLine.CompareModule.Models.CompareModel RelatedCompare { get; private set; }

        public List<MarketPlace.Models.Provider.ProviderLiteViewModel> RelatedProvider { get; set; }

        public List<MarketPlace.Models.General.GenericMenu> CompareMenu { get; set; }

        public bool RenderScripts { get; set; }

        #region Request Values

        public int CompareCurrency { get; set; }

        public int? Year { get; set; }

        #endregion

        public MarketPlace.Models.General.enumCompareType CompareType { get; set; }

        #region Compare calculate properties

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

        public int MaxValueCount
        {
            get
            {
                if (oMaxValueCount == null &&
                    RelatedCompare.RelatedProvider != null &&
                    RelatedCompare.RelatedProvider.Any(rp => rp.CompareDetail != null) &&
                    RelatedCompare.RelatedProvider.Any(rp => rp.CompareDetail != null && rp.CompareDetail.Any(cd => cd.Value != null && cd.Value.Count > 0)))
                {
                    oMaxValueCount = RelatedCompare.RelatedProvider.
                            Where(rp => rp.RelatedCompany != null &&
                                !string.IsNullOrEmpty(rp.RelatedCompany.CompanyPublicId) &&
                                rp.CompareDetail != null &&
                                rp.CompareDetail.Count > 0).
                            Max(rp =>
                                rp.CompareDetail.
                                    Where(cd =>
                                        cd.Value != null &&
                                        cd.Value.Count > 0).
                                    Max(cd => cd.Value.Where(val => val != null && !string.IsNullOrEmpty(val.Item2)).ToList().Count)
                            );
                }
                return oMaxValueCount == null ? 0 : oMaxValueCount.Value;
            }
        }
        private int? oMaxValueCount;

        public List<string> UnitNames
        {
            get
            {
                if (oUnitNames == null &&
                    RelatedCompare.RelatedProvider != null &&
                    RelatedCompare.RelatedProvider.Any(rp => rp.CompareDetail != null))
                {
                    oUnitNames = new List<string>();

                    if (CompareType == General.enumCompareType.Commercial)
                    {
                        RelatedCompare.RelatedProvider.
                            Where(rp => rp.RelatedCompany != null &&
                                !string.IsNullOrEmpty(rp.RelatedCompany.CompanyPublicId) &&
                                rp.CompareDetail != null &&
                                rp.CompareDetail.Count > 0).
                            Select(rp =>
                                rp.CompareDetail.
                                    Where(cd =>
                                        cd.Value != null &&
                                        cd.Value.Count > 0).
                                    OrderByDescending(cd => cd.Value.Count).
                                    Select(cd => cd.Value.
                                        All(val =>
                                        {
                                            if (val.Item3 == "$")
                                            {
                                                oUnitNames.Add(MarketPlace.Models.Company.CompanyUtil.ProviderOptions.
                                                    Where(x => x.CatalogId == 108 && x.ItemId == CompareCurrency).
                                                    Select(x => x.ItemName).
                                                    DefaultIfEmpty(string.Empty).
                                                    FirstOrDefault());
                                            }
                                            else
                                            {
                                                oUnitNames.Add(val.Item3);
                                            }

                                            return true;
                                        })).
                                    FirstOrDefault()).
                            FirstOrDefault();
                    }
                }
                return oUnitNames != null ? oUnitNames : new List<string>();
            }
        }
        private List<string> oUnitNames;

        public List<int> YearsToSelect
        {
            get
            {
                if (RelatedCompare.CompareOptions != null &&
                    RelatedCompare.CompareOptions.ContainsKey("Year") &&
                    RelatedCompare.CompareOptions["Year"] != null &&
                    RelatedCompare.CompareOptions["Year"].Count() > 0)
                {
                    return RelatedCompare.CompareOptions["Year"].
                        Select(x => Convert.ToInt32(x.Key.Replace(" ", ""))).
                        OrderByDescending(x => x).
                        ToList();
                }
                else
                {
                    return new List<int>() { DateTime.Now.Year };
                }
            }
        }

        #endregion

        public CompareDetailViewModel(ProveedoresOnLine.CompareModule.Models.CompareModel oRelatedCompare)
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

        #region Json Company Methods

        public string GetJsonCompany()
        {
            return (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(RelatedProvider);
        }

        #endregion

        #region Json Columns

        public string GetJsonColumns()
        {
            StringBuilder oReturn = new StringBuilder();

            oReturn.Append("[");

            CompareColumns.All(col =>
            {
                oReturn.Append("{" +
                    MarketPlace.Models.General.Constants.C_Program_Compare_ColumnItem.
                        Replace("{Width}", "'400px'").
                        Replace("{Field}", "'_" + col + "'").
                        Replace("{HeaderTemplate}", "Compare_DetailObject.GetHeaderTemplate('" + col + "')").
                        Replace("{Template}", "Compare_DetailObject.GetItemTemplate('" + col + "')").
                        Replace("{Locked}", col == "EvaluationArea" ? "true" : "false") +
                    "},");
                return true;
            });

            return oReturn.ToString().Trim().TrimEnd(new char[] { ',' }) + "]";
        }

        #endregion

        #region Json Data

        public string GetJsonData()
        {
            StringBuilder oReturn = new StringBuilder();

            oReturn.Append("[");

            switch (CompareType)
            {
                case General.enumCompareType.Commercial:
                    oReturn.Append(GetJsonDataCommercial());
                    break;
                case General.enumCompareType.Certifications:
                    oReturn.Append(GetJsonDataCertifications());
                    break;
                case General.enumCompareType.Financial:
                    oReturn.Append(GetJsonDataFinancial(null));
                    break;
                default:
                    break;
            }

            return oReturn.ToString().Trim().TrimEnd(new char[] { ',' }) + "]";
        }

        private string GetJsonDataCommercial()
        {
            StringBuilder oReturn = new StringBuilder();

            if (RelatedCompare.RelatedProvider != null && RelatedCompare.RelatedProvider.Count > 0)
            {
                //get all economic activity to eval
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> olstEvaluationArea =
                    new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

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
                    oReturn.Append("{");

                    #region ItemData

                    CompareColumns.All(col =>
                    {
                        if (col == "EvaluationArea")
                        {
                            oReturn.Append("EvaluationArea:{" +
                                MarketPlace.Models.General.Constants.C_Program_Compare_Value_EvaluationArea.
                                Replace("{Name}", "'" + ea.ItemName + "'").
                                Replace("{Type}", "'0'") +
                                "},");
                        }
                        else if (col != "EvaluationArea" && RelatedCompare.RelatedProvider != null)
                        {
                            //get currency rate
                            int oCurrencyFrom = RelatedCompare.RelatedProvider.
                                Where(rp => rp.RelatedCompany != null &&
                                    rp.RelatedCompany.CompanyPublicId == col &&
                                    rp.CompareDetail != null &&
                                    rp.CompareDetail.Count > 0).
                                Select(rp => rp.CompareDetail.
                                        Where(cd => cd.EvaluationAreaId == ea.ItemId &&
                                                    !string.IsNullOrEmpty(cd.Currency)).
                                        Select(cd => Convert.ToInt32(cd.Currency)).
                                        FirstOrDefault()).
                                FirstOrDefault();

                            decimal CurrencyRate = GetCurrencyRateCaching
                                (oCurrencyFrom, CompareCurrency, Year == null ? DateTime.Now.Year : Year.Value);

                            //get company values
                            List<Tuple<int, string, string>> lstValItem = RelatedCompare.RelatedProvider.
                                Where(rp => rp.RelatedCompany != null &&
                                    rp.RelatedCompany.CompanyPublicId == col &&
                                    rp.CompareDetail != null &&
                                    rp.CompareDetail.Count > 0).
                                Select(rp => rp.CompareDetail.
                                        Where(cd => cd.EvaluationAreaId == ea.ItemId &&
                                                    cd.Value != null &&
                                                    cd.Value.Count > 0).
                                        Select(cd => cd.Value).
                                        FirstOrDefault()).
                                FirstOrDefault();

                            oReturn.Append("_" + col + ":{");

                            for (int i = 1; i <= MaxValueCount; i++)
                            {
                                //get current value
                                Tuple<int, string, string> ValItem = null;
                                if (lstValItem != null && lstValItem.Count > 0)
                                {
                                    ValItem = lstValItem.Where(vi => vi.Item1 == i).FirstOrDefault();
                                }
                                oReturn.Append(MarketPlace.Models.General.Constants.C_Program_Compare_Value_Item.
                                    Replace("{i}", i.ToString()).
                                    Replace("{Value}", "'" + (ValItem != null ? (ValItem.Item3 != "$" ? ValItem.Item2 : GetDecimalCurrency(ValItem.Item2, CurrencyRate)) : "0") + "'").
                                    Replace("{After}", "'" + string.Empty + "'").
                                    Replace("{Before}", "'" + (ValItem != null && ValItem.Item3 == "$" ? ValItem.Item3 : string.Empty) + "'"));
                            }

                            oReturn.Append("},");
                        }

                        return true;
                    });

                    #endregion

                    oReturn.Append("},");

                    return true;
                });

            }

            return oReturn.ToString().Trim().TrimEnd(new char[] { ',' });
        }

        private string GetJsonDataCertifications()
        {
            StringBuilder oReturn = new StringBuilder();

            if (RelatedCompare.RelatedProvider != null && RelatedCompare.RelatedProvider.Count > 0)
            {
                //get all economic activity to eval
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> olstEvaluationArea =
                    new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

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
                    oReturn.Append("{");

                    #region ItemData

                    CompareColumns.All(col =>
                    {
                        if (col == "EvaluationArea")
                        {
                            oReturn.Append("EvaluationArea:{" +
                                MarketPlace.Models.General.Constants.C_Program_Compare_Value_EvaluationArea.
                                Replace("{Name}", "'" + ea.ItemName + "'").
                                Replace("{Type}", "'0'") +
                                "},");
                        }
                        else if (col != "EvaluationArea" && RelatedCompare.RelatedProvider != null)
                        {
                            //get company values
                            List<Tuple<int, string, string>> lstValItem = RelatedCompare.RelatedProvider.
                                Where(rp => rp.RelatedCompany != null &&
                                    rp.RelatedCompany.CompanyPublicId == col &&
                                    rp.CompareDetail != null &&
                                    rp.CompareDetail.Count > 0).
                                Select(rp => rp.CompareDetail.
                                        Where(cd => cd.EvaluationAreaId == ea.ItemId &&
                                                    cd.Value != null &&
                                                    cd.Value.Count > 0).
                                        Select(cd => cd.Value).
                                        FirstOrDefault()).
                                FirstOrDefault();

                            oReturn.Append("_" + col + ":{");

                            for (int i = 1; i <= MaxValueCount; i++)
                            {
                                //get current value
                                Tuple<int, string, string> ValItem = null;
                                if (lstValItem != null && lstValItem.Count > 0)
                                {
                                    ValItem = lstValItem.Where(vi => vi.Item1 == i).FirstOrDefault();
                                }
                                oReturn.Append(MarketPlace.Models.General.Constants.C_Program_Compare_Value_Item.
                                    Replace("{i}", i.ToString()).
                                    Replace("{Value}", "'" + (ValItem != null ? ValItem.Item2 : "0") + "'").
                                    Replace("{After}", "'" + string.Empty + "'").
                                    Replace("{Before}", "'" + ValItem.Item3 + "'"));
                            }
                            oReturn.Append("},");
                        }

                        return true;
                    });

                    #endregion

                    oReturn.Append("},");

                    return true;
                });
            }

            return oReturn.ToString().Trim().TrimEnd(new char[] { ',' });
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
                    #region Get Account Values

                    //get account type
                    string strAccountType = ac.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_IsValue).
                        Select(y => y.Value.Replace(" ", "")).
                        DefaultIfEmpty("2").
                        FirstOrDefault();

                    //get account unit
                    string oAccountUnit = ac.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_Unit).
                        Select(y => y.Value.Replace(" ", "")).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    #endregion

                    if (strAccountType != "2")
                    {
                        //get child values
                        oReturn.AppendLine(GetJsonDataFinancial(ac));
                    }

                    oReturn.Append("{");

                    #region Item data

                    CompareColumns.All(col =>
                    {
                        if (col == "EvaluationArea")
                        {
                            oReturn.Append("EvaluationArea:{" +
                                MarketPlace.Models.General.Constants.C_Program_Compare_Value_EvaluationArea.
                                Replace("{Name}", "'" + ac.ItemName + "'").
                                Replace("{Type}", "'" + strAccountType + "'") +
                                "},");
                        }
                        else if (col != "EvaluationArea" && RelatedCompare.RelatedProvider != null)
                        {
                            //get currency rate
                            int oCurrencyFrom = RelatedCompare.RelatedProvider.
                                Where(rp => rp.RelatedCompany != null &&
                                    rp.RelatedCompany.CompanyPublicId == col &&
                                    rp.CompareDetail != null &&
                                    rp.CompareDetail.Count > 0).
                                Select(rp => rp.CompareDetail.
                                        Where(cd => cd.EvaluationAreaId == ac.ItemId &&
                                                    !string.IsNullOrEmpty(cd.Currency)).
                                        Select(cd => Convert.ToInt32(cd.Currency)).
                                        FirstOrDefault()).
                                FirstOrDefault();

                            decimal CurrencyRate = GetCurrencyRateCaching
                                (oCurrencyFrom, CompareCurrency, Year == null ? DateTime.Now.Year : Year.Value);

                            //get company values
                            List<Tuple<int, string, string>> lstValItem = RelatedCompare.RelatedProvider.
                                Where(rp => rp.RelatedCompany != null &&
                                    rp.RelatedCompany.CompanyPublicId == col &&
                                    rp.CompareDetail != null &&
                                    rp.CompareDetail.Count > 0).
                                Select(rp => rp.CompareDetail.
                                        Where(cd => cd.EvaluationAreaId == ac.ItemId &&
                                                    cd.Value != null &&
                                                    cd.Value.Count > 0).
                                        Select(cd => cd.Value).
                                        FirstOrDefault()).
                                FirstOrDefault();

                            oReturn.Append("_" + col + ":{");

                            for (int i = 1; i <= MaxValueCount; i++)
                            {
                                //get current value
                                Tuple<int, string, string> ValItem = null;
                                if (lstValItem != null && lstValItem.Count > 0)
                                {
                                    ValItem = lstValItem.Where(vi => vi.Item1 == i).FirstOrDefault();
                                }
                                oReturn.Append(MarketPlace.Models.General.Constants.C_Program_Compare_Value_Item.
                                    Replace("{i}", i.ToString()).
                                    Replace("{Value}", "'" + (ValItem != null ? (oAccountUnit != "$" ? ValItem.Item2 : GetDecimalCurrency(ValItem.Item2, CurrencyRate)) : (strAccountType == "2" ? " - " : "0")) + "'").
                                    Replace("{After}", "'" + (oAccountUnit == "$" ? string.Empty : (ValItem != null ? ValItem.Item3 : string.Empty)) + "'").
                                    Replace("{Before}", "'" + (oAccountUnit != "$" ? string.Empty : (ValItem != null ? ValItem.Item3 : string.Empty)) + "'"));
                            }

                            oReturn.Append("},");
                        }
                        return true;
                    });

                    #endregion

                    oReturn.Append("},");

                    if (strAccountType == "2")
                    {
                        //get child values
                        oReturn.Append(GetJsonDataFinancial(ac));
                    }

                    return true;
                });

            return oReturn.ToString();
        }

        private string GetDecimalCurrency(string OriginalNumber, decimal CurrencyRate)
        {
            string oReturn = "0";

            if (!string.IsNullOrEmpty(OriginalNumber))
            {
                oReturn = ((decimal)(CurrencyRate * Convert.ToDecimal(OriginalNumber))).ToString("#,0.##");
            }

            return oReturn;
        }

        private decimal GetCurrencyRateCaching
            (int MoneyFrom,
            int MoneyTo,
            int Year)
        {
            if (CurrencyRateCaching == null)
                CurrencyRateCaching = new Dictionary<string, decimal>();

            string oKey = MoneyFrom.ToString() + "_" +
                        MoneyTo.ToString() + "_" +
                        Year.ToString();

            if (!CurrencyRateCaching.ContainsKey(oKey))
            {
                CurrencyRateCaching[oKey] = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate
                    (MoneyFrom, MoneyTo, Year);
            }

            return CurrencyRateCaching[oKey];
        }
        private Dictionary<string, decimal> CurrencyRateCaching;


        #endregion
    }
}
