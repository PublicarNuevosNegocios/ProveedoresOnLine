using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class ProviderApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> GITrackingInfo
            (string GITrackingInfo, string ProviderPublicId)
        {
            if (GITrackingInfo == "true")
            {
                ProveedoresOnLine.Company.Models.Util.GenericItemModel oTrakingInf =
                   ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCustomerProviderGetAllTracking(
                   SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

                List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> oReturn = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>();

                if (oTrakingInf != null && oTrakingInf.ItemInfo.Count > 0)
                {
                    oTrakingInf.ItemInfo.All(x =>
                    {
                        TrackingDetailViewModel oTracking = new TrackingDetailViewModel();
                        oTracking = (TrackingDetailViewModel)(new System.Web.Script.Serialization.JavaScriptSerializer()).
                            Deserialize(x.LargeValue, typeof(TrackingDetailViewModel));

                        oReturn.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            LargeValue = oTracking.Description,
                            CreateDate = x.CreateDate,
                            Value = oTrakingInf.ItemType.ItemName,
                        });
                        return true;
                    });
                }
                else
                {
                    oReturn.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        Value = oTrakingInf.ItemType.ItemName,
                    });
                }
                return oReturn;
            }
            return null;
        }
        
        #region Providers Charts

        [HttpPost]
        [HttpGet]
        public Dictionary<string, int> GetProvidersByState(string GetProvidersByState)
        {
            //Get Charts By Module
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;


            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_ProvidersStateModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };

            //Get Providers of the Company
            oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetProvidersByState(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

            Dictionary<string, int> oReturn = new Dictionary<string, int>();

            if (oRelatedChart.GenericChartsInfoModel != null && oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    oReturn.Add(x.ItemName, x.Count);
                    return true;
                });
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public Dictionary<string, int> GetNationalProvidersByState(string GetNationalProvidersByState)
        {
            //Get Charts By Module
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;


            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_ProvidersStateModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };

            List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> oCurrentCharts = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetProvidersByState(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);
            oCurrentCharts.All(x =>
            {
                if (
                    x.ItemType.CompareTo("902001") == 0 ||
                    x.ItemType.CompareTo("902002") == 0 ||
                    x.ItemType.CompareTo("902003") == 0 ||
                    x.ItemType.CompareTo("902004") == 0 ||
                    x.ItemType.CompareTo("902005") == 0 ||
                    x.ItemType.CompareTo("902009") == 0 ||
                    x.ItemType.CompareTo("902011") == 0
                    )
                {
                    oRelatedChart.GenericChartsInfoModel.Add(x);
                }

                    return true;
            });

            Dictionary<string, int> oReturn = new Dictionary<string, int>();

            if (oRelatedChart.GenericChartsInfoModel != null && oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    oReturn.Add(x.ItemName, x.Count);
                    return true;
                });
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public Dictionary<string, int> GetAlienProvidersByState(string GetAlienProvidersByState)
        {
            //Get Charts By Module 
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;


            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_ProvidersStateModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };

            List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> oCurrentChart = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetProvidersByState(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

            oCurrentChart.All(x =>
            {
                if (
                    x.ItemType.CompareTo("902006") == 0 ||
                    x.ItemType.CompareTo("902007") == 0 ||
                    x.ItemType.CompareTo("902008") == 0 ||
                    x.ItemType.CompareTo("902010") == 0 ||
                    x.ItemType.CompareTo("902012") == 0
                    )
                {
                    oRelatedChart.GenericChartsInfoModel.Add(x);
                }

                return true;
            });

            Dictionary<string, int> oReturn = new Dictionary<string, int>();

            if (oRelatedChart.GenericChartsInfoModel != null && oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    oReturn.Add(x.ItemName, x.Count);
                    return true;
                });
            }

            return oReturn;
        }

        #endregion        

        #region General Reports

        [HttpPost]
        [HttpGet]
        public string ReportGeneralCompare(string ReportGeneralCompare, string SearchParam, string SearchFilter)
        {
            if (ReportGeneralCompare == "true")
            {
                if (SessionModel.CurrentURL != null)
                    SessionModel.CurrentURL = null;

                ProviderSearchViewModel oModel = null;

                List<ProviderModel> oProviderResult;

                if (SessionModel.CurrentCompany != null &&
                    !string.IsNullOrEmpty(SessionModel.CurrentCompany.CompanyPublicId))
                {
                    //get basic search model
                    oModel = new ProviderSearchViewModel()
                    {
                        ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                        SearchParam = SearchParam,
                        SearchFilter = SearchFilter == null ? null : (SearchFilter.Trim(new char[] { ',' }).Length > 0 ? SearchFilter.Trim(new char[] { ',' }) : null),
                        SearchOrderType = MarketPlace.Models.General.enumSearchOrderType.Relevance,
                        OrderOrientation = false,
                        PageNumber = 0,
                        ProviderSearchResult = new List<ProviderLiteViewModel>(),
                    };

                    #region Providers

                    //search providers
                    int oTotalRowsAux;
                    oProviderResult =
                        ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchNew
                        (SessionModel.CurrentCompany.CompanyPublicId,
                        SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false,
                        oModel.SearchParam,
                        oModel.SearchFilter,
                        (int)oModel.SearchOrderType,
                        oModel.OrderOrientation,
                        oModel.PageNumber,
                        //oModel.RowCount,
                        65000,
                        out oTotalRowsAux);

                    oModel.TotalRows = oTotalRowsAux;

                    List<GenericFilterModel> oFilterModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchFilterNew
                        (SessionModel.CurrentCompany.CompanyPublicId,
                        oModel.SearchParam,
                        oModel.SearchFilter,
                        SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false);

                    if (oFilterModel != null)
                    {
                        oModel.ProviderFilterResult = oFilterModel.Where(x => x.CustomerPublicId == SessionModel.CurrentCompany.CompanyPublicId).ToList();
                    }

                    //Branch Info


                    //parse view model
                    if (oProviderResult != null && oProviderResult.Count > 0)
                    {
                        oProviderResult.All(prv =>
                        {
                            ProviderModel response = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPGetBasicInfo(prv.RelatedCompany.CompanyPublicId);
                            prv.RelatedFinantial = response.RelatedFinantial;
                            prv.RelatedCommercial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(prv.RelatedCompany.CompanyPublicId, (int)enumContactType.Brach);
                            prv.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(prv.RelatedCompany.CompanyPublicId, (int)enumLegalType.RUT);

                            return true;
                        });
                    }

                    #endregion Providers

                    #region Crete Excel

                    //Write the document
                    StringBuilder data = new StringBuilder();
                    string strSep = ";";
                    string strProvidersName = "\"" + "PROVEEDOR" + "\"";
                    string Address = string.Empty;
                    string Telephone = string.Empty;
                    string Mail = string.Empty;
                    string Representative = string.Empty;
                    string Country = string.Empty;
                    int CityId = 0;
                    string City = string.Empty;
                    string State = string.Empty;
                    string AERut = string.Empty;
                    string Income = string.Empty;
                    string Utility = string.Empty;
                    string Etibda = string.Empty;

                    oProviderResult.All(x =>
                    {
                        if (!string.IsNullOrEmpty(x.RelatedCompany.CompanyName))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + x.RelatedCompany.CompanyName + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }

                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "TIPO DE IDENTIFICACIÓN" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (!string.IsNullOrEmpty(x.RelatedCompany.IdentificationType.ItemName))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + x.RelatedCompany.IdentificationType.ItemName + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }
                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "NÚMERO DE IDENTIFICACIÓN" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (!string.IsNullOrEmpty(x.RelatedCompany.IdentificationNumber))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + x.RelatedCompany.IdentificationNumber + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }
                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "PAÍS" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedCommercial != null)
                        {
                            x.RelatedCommercial.Where(y => y != null).All(y =>
                            {
                                bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                                z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                                if (isPrincipal)
                                {
                                    int oTotalRows = 0;
                                    CityId = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).Select(z => Convert.ToInt32(z.Value)).DefaultIfEmpty(0).FirstOrDefault();
                                    List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oGeographyModel = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, CityId, 0, 10000, out oTotalRows);

                                    Country = oGeographyModel.FirstOrDefault().Country.ItemName;
                                }
                                return true;
                            });
                        }
                        if (!string.IsNullOrEmpty(Country))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + Country + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }

                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "CIUDAD" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedCommercial != null)
                        {
                            x.RelatedCommercial.Where(y => y != null).All(y =>
                            {
                                bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                                z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                                if (isPrincipal)
                                {
                                    int oTotalRows = 0;
                                    CityId = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).Select(z => Convert.ToInt32(z.Value)).DefaultIfEmpty(0).FirstOrDefault();
                                    List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oGeographyModel = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, CityId, 0, 10000, out oTotalRows);

                                    City = oGeographyModel.FirstOrDefault().City.ItemName;
                                }
                                return true;
                            });
                        }
                        if (!string.IsNullOrEmpty(City))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + City + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }

                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "DEPARTAMENTO" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedCommercial != null)
                        {
                            x.RelatedCommercial.Where(y => y != null).All(y =>
                            {
                                bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                                z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                                if (isPrincipal)
                                {
                                    int oTotalRows = 0;
                                    CityId = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).Select(z => Convert.ToInt32(z.Value)).DefaultIfEmpty(0).FirstOrDefault();
                                    List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oGeographyModel = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, CityId, 0, 10000, out oTotalRows);

                                    State = oGeographyModel.FirstOrDefault().State.ItemName;
                                }
                                return true;
                            });
                        }
                        if (!string.IsNullOrEmpty(State))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + State + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }

                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "REPRESENTANTE LEGAL" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedCommercial != null)
                        {
                            x.RelatedCommercial.Where(y => y != null).All(y =>
                            {
                                bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                                z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                                if (isPrincipal)
                                {
                                    Representative = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Representative).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                                }
                                return true;
                            });
                        }
                        if (!string.IsNullOrEmpty(Representative))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + Representative + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }

                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "TELÉFONO" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedCommercial != null)
                        {
                            x.RelatedCommercial.Where(y => y != null).All(y =>
                            {
                                bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                                z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                                if (isPrincipal)
                                {
                                    Telephone = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Phone).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                                }
                                return true;
                            });
                        }
                        if (!string.IsNullOrEmpty(Telephone))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + Telephone + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }

                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "CORREO" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedCommercial != null)
                        {
                            x.RelatedCommercial.Where(y => y != null).All(y =>
                            {
                                bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                                z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                                if (isPrincipal)
                                {
                                    Mail = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Email).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                                }
                                return true;
                            });
                        }
                        if (!string.IsNullOrEmpty(Mail))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + Mail + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }

                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "DIRECCIÓN" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedCommercial != null)
                        {
                            x.RelatedCommercial.Where(y => y != null).All(y =>
                            {
                                bool isPrincipal = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal &&
                                z.Value == "1").Select(z => z.Value).FirstOrDefault() == "1" ? true : false;

                                if (isPrincipal)
                                {
                                    Address = y.ItemInfo.Where(z => z.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Address).Select(z => z.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
                                }
                                return true;
                            });
                        }
                        if (!string.IsNullOrEmpty(Address))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + Address + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }

                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "ACTIVIDAD ECONOMICA" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedLegal != null)
                        {
                            List<ProviderLegalViewModel> RelatedLegalInfo = new List<ProviderLegalViewModel>();

                            x.RelatedLegal.All(z =>
                            {
                                RelatedLegalInfo.Add(new ProviderLegalViewModel(z));
                                return true;
                            });
                            AERut = RelatedLegalInfo.Select(r => r.R_ICAName).DefaultIfEmpty(string.Empty).FirstOrDefault();

                        }
                        if (!string.IsNullOrEmpty(AERut))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + AERut + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }

                        return true;
                    });
                    data.AppendLine(strProvidersName);


                    strProvidersName = "\"" + "INGRESOS" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedFinantial != null)
                        {

                            List<ProviderFinancialBasicInfoViewModel> RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                            decimal oExchange;
                            oExchange = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(
                                        Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemInfo.FirstOrDefault().ValueName),
                                        Convert.ToInt32(Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_CurrencyExchange_COP].Value),
                                        Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemName));

                            x.RelatedFinantial.All(z =>
                            {
                                RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(z, oExchange));
                                return true;
                            });
                            Income = RelatedFinancialBasicInfo.Select(r => r.BI_OperationIncome).DefaultIfEmpty(string.Empty).FirstOrDefault();

                        }
                        if (!string.IsNullOrEmpty(Income))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + Income + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }

                        return true;
                    });
                    data.AppendLine(strProvidersName);


                    strProvidersName = "\"" + "UTILIDAD NETA" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedFinantial != null)
                        {

                            List<ProviderFinancialBasicInfoViewModel> RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                            decimal oExchange;
                            oExchange = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(
                                        Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemInfo.FirstOrDefault().ValueName),
                                        Convert.ToInt32(Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_CurrencyExchange_COP].Value),
                                        Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemName));

                            x.RelatedFinantial.All(z =>
                            {
                                RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(z, oExchange));
                                return true;
                            });
                            Utility = RelatedFinancialBasicInfo.Select(r => r.BI_IncomeBeforeTaxes).DefaultIfEmpty(string.Empty).FirstOrDefault();

                        }
                        if (!string.IsNullOrEmpty(Utility))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + Utility + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }
                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    strProvidersName = "\"" + "EBITDA" + "\"";
                    oProviderResult.All(x =>
                    {
                        if (x.RelatedFinantial != null)
                        {

                            List<ProviderFinancialBasicInfoViewModel> RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                            decimal oExchange;
                            oExchange = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(
                                        Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemInfo.FirstOrDefault().ValueName),
                                        Convert.ToInt32(Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_CurrencyExchange_COP].Value),
                                        Convert.ToInt32(x.RelatedFinantial.FirstOrDefault().ItemName));

                            x.RelatedFinantial.All(z =>
                            {
                                RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(z, oExchange));
                                return true;
                            });
                            Etibda = RelatedFinancialBasicInfo.Select(r => r.BI_EBITDA).DefaultIfEmpty(string.Empty).FirstOrDefault();

                        }
                        if (!string.IsNullOrEmpty(Etibda))
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + Etibda + "\"";
                        }
                        else
                        {
                            strProvidersName = strProvidersName + strSep + "\"" + "N/D" + "\"";
                        }
                        return true;
                    });
                    data.AppendLine(strProvidersName);

                    byte[] buffer = Encoding.Default.GetBytes(data.ToString().ToCharArray());

                    #endregion Crete Excel
                    //Create 
                    byte[] bytes = Encoding.Default.GetBytes(data.ToString().ToCharArray());

                    //get folder
                    string strFolder = System.Web.HttpContext.Current.Server.MapPath
                        (Models.General.InternalSettings.Instance
                        [Models.General.Constants.C_Settings_File_TempDirectory].Value);

                    if (!System.IO.Directory.Exists(strFolder))
                        System.IO.Directory.CreateDirectory(strFolder);

                    string oFileName = "GeneralReport_" + SessionModel.CurrentCompany.CompanyPublicId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".";                          
                    oFileName = oFileName +".csv";
                    string strFilePath = strFolder.TrimEnd('\\') + "\\" + oFileName;

                    File.WriteAllBytes(strFilePath, buffer);
                    string strRemoteFile = string.Empty;

                    strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                           (strFilePath, Models.General.InternalSettings.Instance
                               [Models.General.Constants.C_Settings_File_RemoteDirectory].Value.TrimEnd('\\') +
                                "_" + DateTime.Now + "\\");

                    //remove temporal file
                    if (System.IO.File.Exists(strFilePath))
                        System.IO.File.Delete(strFilePath);
                    return strRemoteFile;
                    //return File(buffer, "application/csv", "Proveedores_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv");
                }
                return null;

            }

            return null;
        }
        
        #endregion
    }
}
