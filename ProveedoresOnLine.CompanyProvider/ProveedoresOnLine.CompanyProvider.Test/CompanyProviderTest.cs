using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProveedoresOnLine.CompanyProvider.Test
{
    [TestClass]
    public class CompanyProviderTest
    {
        [TestMethod]
        public void CommercialGetBasicInfo()
        {
            List<Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CommercialGetBasicInfo
                ("1D9B9580", 301001, true);

            Assert.AreEqual(true, oReturn != null && oReturn.Count >= 1);
        }

        [TestMethod]
        public void FinancialGetBasicInfo()
        {
            List<Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.FinancialGetBasicInfo
                ("1D9B9580", null, true);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void BalanceSheetGetByFinancial()
        {
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel> oReturn =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetGetByFinancial
                (1);

            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void LegalInfoGetByLegalType()
        {
            // List<CatalogModel> oReturn = new List<CatalogModel>();
            //return (LegalId, LegalInfoId, LegalInfoTypeId, Value, LargeValue, Enable);
        }

        [TestMethod]
        public void BalanceSheetUpsert()
        {
            ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel ProviderToUpsert = GetBalanceSheetUpsertModel();

            ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetUpsert
                (ProviderToUpsert);
        }

        private ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel GetBalanceSheetUpsertModel()
        {
            ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel ProviderToUpsert = new Models.Provider.ProviderModel()
            {
                RelatedCompany = new Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = "1D9B9580",
                },
                RelatedBalanceSheet = new List<Models.Provider.BalanceSheetModel>()
                {
                    new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel()
                    {
                        ItemId = 3,
                        ItemName = "balance test",
                        Enable = true,
                        ItemType = new Company.Models.Util.CatalogModel()
                        {
                            ItemId = 501001,
                        },
                        ItemInfo = new List<Company.Models.Util.GenericItemInfoModel>()
                        {
                            new Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                {
                                    ItemId = 502001,
                                },
                                Value = "2014",
                                Enable = true,
                            },
                            new Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                {
                                    ItemId = 502002,
                                },
                                Value = "http://devproveedoresonline.s3-website-us-east-1.amazonaws.com/BackOffice/CompanyFile/1D9B9580/CompanyFile_1D9B9580_20141222165937.txt",
                                Enable = true,
                            },
                            new Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                {
                                    ItemId = 502003,
                                },
                                Value = "108002",
                                Enable = true,
                            }
                        },

                        BalanceSheetInfo = new List<Models.Provider.BalanceSheetDetailModel>()
                        {
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3122,
                                },
                                Value = Convert.ToDecimal("209734852"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3123,
                                },
                                Value = Convert.ToDecimal("53010714"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3124,
                                },
                                Value = Convert.ToDecimal("653133858"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3125,
                                },
                                Value = Convert.ToDecimal("0"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3126,
                                },
                                Value = Convert.ToDecimal("0"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3127,
                                },
                                Value = Convert.ToDecimal("0"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3128,
                                },
                                Value = Convert.ToDecimal("260340160"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3129,
                                },
                                Value = Convert.ToDecimal("139176647"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3130,
                                },
                                Value = Convert.ToDecimal("256971776"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3131,
                                },
                                Value = Convert.ToDecimal("284148815"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3132,
                                },
                                Value = Convert.ToDecimal("8523685"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3133,
                                },
                                Value = Convert.ToDecimal("276304645"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3134,
                                },
                                Value = Convert.ToDecimal("1093875"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3135,
                                },
                                Value = Convert.ToDecimal("264454629"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3136,
                                },
                                Value = Convert.ToDecimal("127795747"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3137,
                                },
                                Value = Convert.ToDecimal("0"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3138,
                                },
                                Value = Convert.ToDecimal("0"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3139,
                                },
                                Value = Convert.ToDecimal("0"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3140,
                                },
                                Value = Convert.ToDecimal("0"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3141,
                                },
                                Value = Convert.ToDecimal("600000000"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3142,
                                },
                                Value = Convert.ToDecimal("114741431"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3143,
                                },
                                Value = Convert.ToDecimal("185249516"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3821,
                                },
                                Value = Convert.ToDecimal("3170428588"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3822,
                                },
                                Value = Convert.ToDecimal("0"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3825,
                                },
                                Value = Convert.ToDecimal("2593326724"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3826,
                                },
                                Value = Convert.ToDecimal("89215972"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3827,
                                },
                                Value = Convert.ToDecimal("144027216"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3833,
                                },
                                Value = Convert.ToDecimal("4900025"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3834,
                                },
                                Value = Convert.ToDecimal("31777365"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3835,
                                },
                                Value = Convert.ToDecimal("21619515"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3844,
                                },
                                Value = Convert.ToDecimal("112789000"),
                            },
                            new Models.Provider.BalanceSheetDetailModel()
                            {
                                RelatedAccount = new Company.Models.Util.GenericItemModel()
                                {
                                    ItemId = 3854,
                                },
                                Value = Convert.ToDecimal("0"),
                            },
                        },
                    }
                }
            };


            return ProviderToUpsert;
        }

        #region MarketPlace

        [TestMethod]
        public void MPProviderSearch()
        {
            int oTotalRows;

            List<Models.Provider.ProviderModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearch
                    ("1A9863BD",
                    "a",
                    "",
                    113002,
                    false,
                    0,
                    20,
                    out oTotalRows);

            Assert.AreEqual(true, oResult.Count > 0);

        }

        [TestMethod]
        public void MPProviderSearchFilter()
        {
            List<Company.Models.Util.GenericFilterModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchFilter
                    ("1A9863BD",
                    "a",
                    null);

            Assert.AreEqual(true, oResult.Count > 0);

        }


        [TestMethod]
        public void MPProviderSearchById()
        {
            List<Models.Provider.ProviderModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                    ("1A9863BD",
                    "C44A0CBF,2F8EF68D,");

            Assert.AreEqual(true, oResult.Count > 0);
        }

        [TestMethod]
        public void MPBalanceSheetGetByYear()
        {

            List<Models.Provider.BalanceSheetModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPBalanceSheetGetByYear
                ("1D9B9580", 2013, 108001);


            Assert.AreEqual(true, oResult.Count > 0);
        }


        
        #endregion
    }
}
