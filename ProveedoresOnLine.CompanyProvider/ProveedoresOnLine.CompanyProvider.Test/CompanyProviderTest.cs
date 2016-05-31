using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System.Linq;
using ProveedoresOnLine.Company.Models.Company;

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
        public void BalanceSheetUpsert()
        {
            ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel ProviderToUpsert = GetBalanceSheetUpsertModel();

            ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetUpsert
                (ProviderToUpsert);
        }

        [TestMethod]
        public void AditionalDocumentGetByType()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = new List<GenericItemModel>();

            oReturn = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentGetByType
                ("5CD41647",
                1701001,
                true);

            Assert.AreEqual(true, oReturn.Count > 0);
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

        [TestMethod]
        public void AddQuestions()
        {
            List<string> oProviderPublicId = new List<string>();

            #region ProviderList

            //oProviderPublicId.Add("1882BED8");
            oProviderPublicId.Add("1250DA28");
            oProviderPublicId.Add("165490C9");
            oProviderPublicId.Add("1435D2EA");
            oProviderPublicId.Add("20D4BF66");
            oProviderPublicId.Add("21633AAC");
            oProviderPublicId.Add("22251703");
            oProviderPublicId.Add("72FFEF86");
            oProviderPublicId.Add("15BD9EC3");
            oProviderPublicId.Add("429B1C5C");
            oProviderPublicId.Add("340E25B4");
            //oProviderPublicId.Add("AD638C88");
            //oProviderPublicId.Add("DCDA13A1");
            //oProviderPublicId.Add("E8C8B871");
            //oProviderPublicId.Add("2A762DCE");
            //oProviderPublicId.Add("12B6313F");
            //oProviderPublicId.Add("202075A2");
            //oProviderPublicId.Add("176506B8");
            //oProviderPublicId.Add("2D6F780A");
            //oProviderPublicId.Add("5102B05A");
            //oProviderPublicId.Add("1E07C733");
            //oProviderPublicId.Add("1269D88B");
            //oProviderPublicId.Add("80FBBE1B");
            //oProviderPublicId.Add("1758E4AB");
            //oProviderPublicId.Add("231EB383");
            //oProviderPublicId.Add("90DB5704");
            //oProviderPublicId.Add("15AC4A73");
            //oProviderPublicId.Add("16E833B4");
            //oProviderPublicId.Add("8B845F7A");
            //oProviderPublicId.Add("7354C447");
            //oProviderPublicId.Add("29F7F855");
            //oProviderPublicId.Add("4EFD0B97");
            //oProviderPublicId.Add("421686F4");
            //oProviderPublicId.Add("7F0C6ABE");
            //oProviderPublicId.Add("1C757486");
            //oProviderPublicId.Add("7CC6ED17");
            //oProviderPublicId.Add("1E5514CE");
            //oProviderPublicId.Add("11080CED");
            //oProviderPublicId.Add("901100F8");
            //oProviderPublicId.Add("232A1476");
            //oProviderPublicId.Add("12FF44F2");
            //oProviderPublicId.Add("2A007FF5");
            //oProviderPublicId.Add("DAFDDB1D");
            //oProviderPublicId.Add("1508ECCB");
            //oProviderPublicId.Add("211680E4");
            //oProviderPublicId.Add("225CEA6B");
            //oProviderPublicId.Add("1818D40B");
            //oProviderPublicId.Add("DBC2BD55");
            //oProviderPublicId.Add("19F43DF2");
            //oProviderPublicId.Add("1C8E63AC");
            //oProviderPublicId.Add("1770E6A2");
            //oProviderPublicId.Add("27216CCC");
            //oProviderPublicId.Add("2F327B54");
            //oProviderPublicId.Add("100DD42C");
            //oProviderPublicId.Add("135BD603");
            //oProviderPublicId.Add("2460E1AC");
            //oProviderPublicId.Add("D1294CBC");
            //oProviderPublicId.Add("A4D057C0");
            //oProviderPublicId.Add("58AE7197");
            //oProviderPublicId.Add("15A234BF");
            //oProviderPublicId.Add("1FE6AA91");
            //oProviderPublicId.Add("F58F387E");
            //oProviderPublicId.Add("1AA7DED7");
            //oProviderPublicId.Add("126457FE");
            //oProviderPublicId.Add("EBDAE5D9");
            //oProviderPublicId.Add("2EDAAFDA");
            //oProviderPublicId.Add("9C727E6C");
            //oProviderPublicId.Add("10A22F76");
            //oProviderPublicId.Add("22B8ED65");
            //oProviderPublicId.Add("11420F8E");
            //oProviderPublicId.Add("1FAF80D7");
            //oProviderPublicId.Add("15772F1E");
            //oProviderPublicId.Add("1F151B81");
            //oProviderPublicId.Add("33763928");
            //oProviderPublicId.Add("D94B0C0D");
            //oProviderPublicId.Add("15E85CAD");
            //oProviderPublicId.Add("13AEEC93");
            //oProviderPublicId.Add("20BB9A27");
            //oProviderPublicId.Add("1904C10B");
            //oProviderPublicId.Add("7DBBA454");
            //oProviderPublicId.Add("16DDE2CB");
            //oProviderPublicId.Add("34C3CDBD");
            //oProviderPublicId.Add("BEED759C");
            //oProviderPublicId.Add("11C065C1");
            //oProviderPublicId.Add("28D1440D");
            //oProviderPublicId.Add("15243175");
            //oProviderPublicId.Add("18EA1B5C");
            //oProviderPublicId.Add("898585C9");
            //oProviderPublicId.Add("1B4EF664");
            //oProviderPublicId.Add("15DADFF7");
            //oProviderPublicId.Add("A905FABC");
            //oProviderPublicId.Add("370D3568");
            //oProviderPublicId.Add("1C9188F9");
            //oProviderPublicId.Add("3058F3F8");
            //oProviderPublicId.Add("1D87D2ED");
            //oProviderPublicId.Add("203A94EB");
            //oProviderPublicId.Add("AF34CE8B");
            //oProviderPublicId.Add("170A1055");
            //oProviderPublicId.Add("20AACF20");
            //oProviderPublicId.Add("1F9D4C05");
            //oProviderPublicId.Add("922F5E7B");
            //oProviderPublicId.Add("1BC63880");
            //oProviderPublicId.Add("E7A2293A");
            //oProviderPublicId.Add("14792B22");
            //oProviderPublicId.Add("1DE1CCD2");
            //oProviderPublicId.Add("53B1A7BC");
            //oProviderPublicId.Add("9CCF2522");
            //oProviderPublicId.Add("E7559B04");
            //oProviderPublicId.Add("9792B569");
            //oProviderPublicId.Add("1EF1E71C");
            //oProviderPublicId.Add("94C8546F");
            //oProviderPublicId.Add("ABD8BC92");
            //oProviderPublicId.Add("289853E4");
            //oProviderPublicId.Add("9D737633");
            //oProviderPublicId.Add("283AAE5C");
            //oProviderPublicId.Add("E3D0160D");
            //oProviderPublicId.Add("18F95F44");
            //oProviderPublicId.Add("229DF4FE");
            //oProviderPublicId.Add("1C835522");
            //oProviderPublicId.Add("14AA63B0");
            //oProviderPublicId.Add("13457DE0");
            //oProviderPublicId.Add("D776C455");
            //oProviderPublicId.Add("1F4CE586");
            //oProviderPublicId.Add("877F89C1");
            //oProviderPublicId.Add("96D9CEB8");
            //oProviderPublicId.Add("234CE493");
            //oProviderPublicId.Add("1DE7FFB6");
            //oProviderPublicId.Add("16C2EA5C");
            //oProviderPublicId.Add("E6B6780B");
            //oProviderPublicId.Add("19EF9D83");
            //oProviderPublicId.Add("1B8CA06B");
            //oProviderPublicId.Add("8C7CFA03");
            //oProviderPublicId.Add("18D3FE68");
            //oProviderPublicId.Add("12C007E3");
            //oProviderPublicId.Add("1962B4E5");
            //oProviderPublicId.Add("21C16F33");
            //oProviderPublicId.Add("2ED1540E");
            //oProviderPublicId.Add("9998D267");
            //oProviderPublicId.Add("CFB8AFFA");
            //oProviderPublicId.Add("221DAD63");
            //oProviderPublicId.Add("1F2A4461");
            //oProviderPublicId.Add("C93C7FF6");
            //oProviderPublicId.Add("2A5AB362");
            //oProviderPublicId.Add("16EC39D4");
            //oProviderPublicId.Add("18DFD61E");
            //oProviderPublicId.Add("10A9A1F3");
            //oProviderPublicId.Add("1B2BD9A9");
            //oProviderPublicId.Add("128B1018");
            //oProviderPublicId.Add("2F3806B8");
            //oProviderPublicId.Add("934A7B29");
            //oProviderPublicId.Add("EEFF09D4");
            //oProviderPublicId.Add("2AE7CB67");
            //oProviderPublicId.Add("4AE4D328");
            //oProviderPublicId.Add("3CA734E4");
            //oProviderPublicId.Add("AB11A1A4");
            //oProviderPublicId.Add("1EEA15B2");
            //oProviderPublicId.Add("19FB3A04");
            //oProviderPublicId.Add("1EB79D65");
            //oProviderPublicId.Add("294DEDCF");
            //oProviderPublicId.Add("23DC921D");
            //oProviderPublicId.Add("1EA42F80");
            //oProviderPublicId.Add("28490578");
            //oProviderPublicId.Add("28CE402C");
            //oProviderPublicId.Add("81F2E301");
            //oProviderPublicId.Add("15E8C095");
            //oProviderPublicId.Add("15EF69EA");
            //oProviderPublicId.Add("2A1B530E");
            //oProviderPublicId.Add("D26A851E");
            //oProviderPublicId.Add("31F12E47");
            //oProviderPublicId.Add("1F3D6641");
            //oProviderPublicId.Add("76739C35");
            //oProviderPublicId.Add("166DFB54");
            //oProviderPublicId.Add("1674A4A8");
            //oProviderPublicId.Add("167B4DFF");
            //oProviderPublicId.Add("4C1D2CA5");
            //oProviderPublicId.Add("2FD4D94C");
            //oProviderPublicId.Add("DB28C678");
            //oProviderPublicId.Add("893C1970");
            //oProviderPublicId.Add("15FFCB55");
            //oProviderPublicId.Add("1F8D564D");
            //oProviderPublicId.Add("8D235201");
            //oProviderPublicId.Add("1FEA9907");
            //oProviderPublicId.Add("354BC20E");
            //oProviderPublicId.Add("E4519D42");
            //oProviderPublicId.Add("E4BC328C");
            //oProviderPublicId.Add("335B909E");
            //oProviderPublicId.Add("57C583BD");
            //oProviderPublicId.Add("371E0FBB");
            //oProviderPublicId.Add("175078CA");
            //oProviderPublicId.Add("2083CFC8");
            //oProviderPublicId.Add("93E74DBD");
            //oProviderPublicId.Add("9429EB0B");
            //oProviderPublicId.Add("17BB0E30");
            //oProviderPublicId.Add("17C1B787");
            //oProviderPublicId.Add("20ABC7CF");
            //oProviderPublicId.Add("3450B506");
            //oProviderPublicId.Add("17F058E2");
            //oProviderPublicId.Add("17F70239");
            //oProviderPublicId.Add("2123AFE0");
            //oProviderPublicId.Add("184D9B9C");
            //oProviderPublicId.Add("185444F1");
            //oProviderPublicId.Add("213E5539");
            //oProviderPublicId.Add("9B30842C");
            //oProviderPublicId.Add("9B73217A");
            //oProviderPublicId.Add("187C3CF8");
            //oProviderPublicId.Add("6C2C1C2F");
            //oProviderPublicId.Add("1896E251");
            //oProviderPublicId.Add("189D8BA8");
            //oProviderPublicId.Add("A064CF9F");
            //oProviderPublicId.Add("27ED080F");
            //oProviderPublicId.Add("21DE3552");
            //oProviderPublicId.Add("48CFDD11");
            //oProviderPublicId.Add("2837A3D8");
            //oProviderPublicId.Add("76FF486B");
            //oProviderPublicId.Add("7769DDD9");
            //oProviderPublicId.Add("1060AE23");
            //oProviderPublicId.Add("4E46C5D4");
            //oProviderPublicId.Add("4E896339");
            //oProviderPublicId.Add("19AEAA7B");
            //oProviderPublicId.Add("4FD6761B");
            //oProviderPublicId.Add("50191380");
            //oProviderPublicId.Add("AA058E58");
            //oProviderPublicId.Add("548586C5");
            //oProviderPublicId.Add("1A4137E7");
            //oProviderPublicId.Add("2331F184");
            //oProviderPublicId.Add("55D299A8");
            //oProviderPublicId.Add("5615370D");
            //oProviderPublicId.Add("AF39D9CC");
            //oProviderPublicId.Add("1A6FD943");
            //oProviderPublicId.Add("2359E98B");
            //oProviderPublicId.Add("236092E2");
            //oProviderPublicId.Add("57A4E73E");
            //oProviderPublicId.Add("1C53DD5B");
            //oProviderPublicId.Add("1AE117FF");
            //oProviderPublicId.Add("10A7255C");
            //oProviderPublicId.Add("5BCEBD34");
            //oProviderPublicId.Add("12185667");
            //oProviderPublicId.Add("1B0266AF");
            //oProviderPublicId.Add("1B091004");
            //oProviderPublicId.Add("439CAA67");
            //oProviderPublicId.Add("9563E260");
            //oProviderPublicId.Add("EFB0BFB0");
            //oProviderPublicId.Add("1D3E5938");
            //oProviderPublicId.Add("1B31080B");
            //oProviderPublicId.Add("1B37B162");
            //oProviderPublicId.Add("567E6C10");
            //oProviderPublicId.Add("61CAE0D7");
            //oProviderPublicId.Add("1B94F41A");
            //oProviderPublicId.Add("62D55654");
            //oProviderPublicId.Add("1BBCEC21");
            //oProviderPublicId.Add("126B34EF");
            //oProviderPublicId.Add("A10C3977");
            //oProviderPublicId.Add("1BE4E426");
            //oProviderPublicId.Add("F6A39A6B");
            //oProviderPublicId.Add("A74AFA60");
            //oProviderPublicId.Add("31966639");
            //oProviderPublicId.Add("2D414D21");
            //oProviderPublicId.Add("10B74C89");
            //oProviderPublicId.Add("69DBEF5E");
            //oProviderPublicId.Add("13800E9C");
            //oProviderPublicId.Add("1386B7F3");
            //oProviderPublicId.Add("139AB3F5");
            //oProviderPublicId.Add("1C84C43E");
            //oProviderPublicId.Add("B148FBE2");
            //oProviderPublicId.Add("13FEA006");
            //oProviderPublicId.Add("1CE8B04D");
            //oProviderPublicId.Add("16F60D7A");
            //oProviderPublicId.Add("701AB050");
            //oProviderPublicId.Add("705D4DB5");
            //oProviderPublicId.Add("1885BDC2");
            //oProviderPublicId.Add("18C85B10");
            //oProviderPublicId.Add("71ECFDE6");
            //oProviderPublicId.Add("1D7B3DB9");
            //oProviderPublicId.Add("1CAF93B8");
            //oProviderPublicId.Add("BC86BD8C");
            //oProviderPublicId.Add("BCF152FA");
            //oProviderPublicId.Add("14B92578");
            //oProviderPublicId.Add("1DA335BE");
            //oProviderPublicId.Add("1DA9DF15");
            //oProviderPublicId.Add("1E81E14E");
            //oProviderPublicId.Add("77A68424");
            //oProviderPublicId.Add("1DD1D71C");
            //oProviderPublicId.Add("22ABB744");
            //oProviderPublicId.Add("1545098B");
            //oProviderPublicId.Add("154BB2E2");
            //oProviderPublicId.Add("1E35C32B");
            //oProviderPublicId.Add("23F8CA27");
            //oProviderPublicId.Add("1404B543");
            //oProviderPublicId.Add("C899AA12");
            //oProviderPublicId.Add("1573AAE9");
            //oProviderPublicId.Add("1E5DBB30");
            //oProviderPublicId.Add("25887A58");
            //oProviderPublicId.Add("7EAD1D2D");
            //oProviderPublicId.Add("7EEFBA92");
            //oProviderPublicId.Add("159BA2EE");
            //oProviderPublicId.Add("1EC85095");
            //oProviderPublicId.Add("29B2504E");
            //oProviderPublicId.Add("82D6F324");
            //oProviderPublicId.Add("15FF8EFD");
            //oProviderPublicId.Add("2ABCC5CC");
            //oProviderPublicId.Add("2AFF6331");
            //oProviderPublicId.Add("84240606");
            //oProviderPublicId.Add("1620DDAD");
            //oProviderPublicId.Add("1F0AEDF5");
            //oProviderPublicId.Add("2C4C7613");
            //oProviderPublicId.Add("857118D3");
            //oProviderPublicId.Add("16422C5D");
            //oProviderPublicId.Add("1F6EDA04");
            //oProviderPublicId.Add("3033AEA4");
            //oProviderPublicId.Add("169F6F15");
            //oProviderPublicId.Add("16A6186C");
            //oProviderPublicId.Add("1F9028B4");
            //oProviderPublicId.Add("3180C187");
            //oProviderPublicId.Add("DDD56D3E");
            //oProviderPublicId.Add("16C7671C");
            //oProviderPublicId.Add("1FB17764");
            //oProviderPublicId.Add("32CDD453");
            //oProviderPublicId.Add("16E8B5CC");
            //oProviderPublicId.Add("571D7F56");
            //oProviderPublicId.Add("E5BE83DF");
            //oProviderPublicId.Add("1745F884");
            //oProviderPublicId.Add("377CE513");
            //oProviderPublicId.Add("E768D975");
            //oProviderPublicId.Add("17609DDD");
            //oProviderPublicId.Add("204AAE26");
            //oProviderPublicId.Add("38C9F7F6");
            //oProviderPublicId.Add("3991D00F");
            //oProviderPublicId.Add("92B672E4");
            //oProviderPublicId.Add("345FE16A");
            //oProviderPublicId.Add("61F0AB92");
            //oProviderPublicId.Add("3D7908A0");
            //oProviderPublicId.Add("17F32B4A");
            //oProviderPublicId.Add("17F9D49E");
            //oProviderPublicId.Add("20E3E4E7");
            //oProviderPublicId.Add("3EC61B82");
            //oProviderPublicId.Add("F311308D");
            //oProviderPublicId.Add("181B234F");
            //oProviderPublicId.Add("34D51F59");
            //oProviderPublicId.Add("40132E65");
            //oProviderPublicId.Add("F5261B6D");
            //oProviderPublicId.Add("2162764F");
            //oProviderPublicId.Add("21691FA6");
            //oProviderPublicId.Add("FAFA470B");
            //oProviderPublicId.Add("1899B4B7");
            //oProviderPublicId.Add("2183C4FF");
            //oProviderPublicId.Add("218A6E56");
            //oProviderPublicId.Add("FD0F320F");
            //oProviderPublicId.Add("18BB0367");
            //oProviderPublicId.Add("21A513AF");
            //oProviderPublicId.Add("4651EF56");
            //oProviderPublicId.Add("46948CA5");
            //oProviderPublicId.Add("FF8EB25E");
            //oProviderPublicId.Add("2208FFBE");
            //oProviderPublicId.Add("76C1D973");
            //oProviderPublicId.Add("10562DDF");
            //oProviderPublicId.Add("2223A517");
            //oProviderPublicId.Add("C0AD180E");
            //oProviderPublicId.Add("1070D339");
            //oProviderPublicId.Add("195AE37F");
            //oProviderPublicId.Add("2244F3C8");
            //oProviderPublicId.Add("4C90B048");
            //oProviderPublicId.Add("109221E7");
            //oProviderPublicId.Add("197C322F");
            //oProviderPublicId.Add("22664278");
            //oProviderPublicId.Add("10EF64A1");
            //oProviderPublicId.Add("22BCDBD8");
            //oProviderPublicId.Add("14CCBCA8");
            //oProviderPublicId.Add("110A09FA");
            //oProviderPublicId.Add("19F41A40");
            //oProviderPublicId.Add("22DE2A89");
            //oProviderPublicId.Add("84148620");
            //oProviderPublicId.Add("1A0EBF9A");
            //oProviderPublicId.Add("1A22BB9E");
            //oProviderPublicId.Add("56B6A9CA");
            //oProviderPublicId.Add("2A5BBB32");
            //oProviderPublicId.Add("23636548");
            //oProviderPublicId.Add("8C68320D");
            //oProviderPublicId.Add("11B09369");
            //oProviderPublicId.Add("1A9AA3AF");
            //oProviderPublicId.Add("1AB54909");
            //oProviderPublicId.Add("5A18A7A8");
            //oProviderPublicId.Add("1B193517");
            //oProviderPublicId.Add("1B2D311C");
            //oProviderPublicId.Add("385ED6DF");
            //oProviderPublicId.Add("125DC62D");
            //oProviderPublicId.Add("402D48B3");
            //oProviderPublicId.Add("12786B86");
            //oProviderPublicId.Add("4A95DF9B");
            //oProviderPublicId.Add("128C6788");
            //oProviderPublicId.Add("6F03EF57");
            //oProviderPublicId.Add("A0643511");
            //oProviderPublicId.Add("1BD3BA89");
            //oProviderPublicId.Add("C2473D31");
            //oProviderPublicId.Add("1BEE5FE2");
            //oProviderPublicId.Add("1BFBB290");
            //oProviderPublicId.Add("1C09053B");
            //oProviderPublicId.Add("1EC2907E");
            //oProviderPublicId.Add("1F4276F7");
            //oProviderPublicId.Add("C4E2CC85");
            //oProviderPublicId.Add("CA17178A");
            //oProviderPublicId.Add("14BABD24");
            //oProviderPublicId.Add("153FF7D8");
            //oProviderPublicId.Add("18A1F5B5");
            //oProviderPublicId.Add("283EB3FF");
            //oProviderPublicId.Add("2913DEB8");
            //oProviderPublicId.Add("2ED02CF2");
            //oProviderPublicId.Add("1A74434B");
            //oProviderPublicId.Add("1AF97E15");
            //oProviderPublicId.Add("1D636AC5");
            //oProviderPublicId.Add("1BC1562E");
            //oProviderPublicId.Add("1DC756D6");
            //oProviderPublicId.Add("14FE953D");
            //oProviderPublicId.Add("20F5A1A2");
            //oProviderPublicId.Add("C4326512");
            //oProviderPublicId.Add("7DBEDFA4");
            //oProviderPublicId.Add("1583CFFC");
            //oProviderPublicId.Add("1597CBFF");
            //oProviderPublicId.Add("7FD3CA9F");
            //oProviderPublicId.Add("CD5B3BDB");
            //oProviderPublicId.Add("CE306694");
            //oProviderPublicId.Add("CF059171");
            //oProviderPublicId.Add("2BA0D5EE");
            //oProviderPublicId.Add("84C578C4");
            //oProviderPublicId.Add("2D308620");
            //oProviderPublicId.Add("31EF8D6A");
            //oProviderPublicId.Add("498D63B1");
            //oProviderPublicId.Add("167A4977");
            //oProviderPublicId.Add("32223445");
            //oProviderPublicId.Add("1FB449CA");
            //oProviderPublicId.Add("1FC845CC");
            //oProviderPublicId.Add("52B63A57");
            //oProviderPublicId.Add("1FDC41D1");
            //oProviderPublicId.Add("53F5FA7E");
            //oProviderPublicId.Add("8D9E5F64");
            //oProviderPublicId.Add("55A05014");
            //oProviderPublicId.Add("176A199B");
            //oProviderPublicId.Add("E940555E");
            //oProviderPublicId.Add("177E159E");
            //oProviderPublicId.Add("92900D73");
            //oProviderPublicId.Add("179211A0");
            //oProviderPublicId.Add("179F644E");
            //oProviderPublicId.Add("20897495");
            //oProviderPublicId.Add("94E795D3");
            //oProviderPublicId.Add("F33E56E0");
            //oProviderPublicId.Add("181DF5B6");
            //oProviderPublicId.Add("F47E1707");
            //oProviderPublicId.Add("1831F1B9");
            //oProviderPublicId.Add("9996A693");
            //oProviderPublicId.Add("1845EDBB");
            //oProviderPublicId.Add("18B2F58C");
            //oProviderPublicId.Add("1859E9C0");
            //oProviderPublicId.Add("6A06E886");
            //oProviderPublicId.Add("F8A7ED0F");
            //oProviderPublicId.Add("45E8EC96");
            //oProviderPublicId.Add("18CB287A");
            //oProviderPublicId.Add("21B538C2");
            //oProviderPublicId.Add("9FD56785");
            //oProviderPublicId.Add("18E5CDD3");
            //oProviderPublicId.Add("21CFDE1C");
            //oProviderPublicId.Add("47FDD791");
            //oProviderPublicId.Add("1900732C");
            //oProviderPublicId.Add("21EA8375");
            //oProviderPublicId.Add("74DA14E5");
            //oProviderPublicId.Add("195DB5E6");
            //oProviderPublicId.Add("4C6A4AD6");
            //oProviderPublicId.Add("41230F22");
            //oProviderPublicId.Add("225BC231");
            //oProviderPublicId.Add("4D74C054");
            //oProviderPublicId.Add("198C5742");
            //oProviderPublicId.Add("2276678B");
            //oProviderPublicId.Add("4E7F35D2");
            //oProviderPublicId.Add("19A6FC9C");
            //oProviderPublicId.Add("4F89AB4F");
            //oProviderPublicId.Add("1A043F55");
            //oProviderPublicId.Add("52EBA92D");
            //oProviderPublicId.Add("AC104C02");
            //oProviderPublicId.Add("23024B9E");
            //oProviderPublicId.Add("53F61EAA");
            //oProviderPublicId.Add("1A32E0B1");
            //oProviderPublicId.Add("55009428");
            //oProviderPublicId.Add("1A4D860B");
            //oProviderPublicId.Add("23379651");
            //oProviderPublicId.Add("89AB42A3");
            //oProviderPublicId.Add("1C4753F9");
            //oProviderPublicId.Add("596D0783");
            //oProviderPublicId.Add("1ABEC4C7");
            //oProviderPublicId.Add("845D3905");
            //oProviderPublicId.Add("11EF59D8");
            //oProviderPublicId.Add("1AD96A20");
            //oProviderPublicId.Add("5B3F5519");
            //oProviderPublicId.Add("1209FF31");
            //oProviderPublicId.Add("1F738D91");
            //oProviderPublicId.Add("121DFB33");
            //oProviderPublicId.Add("1B080B7C");
            //oProviderPublicId.Add("5FABC874");
            //oProviderPublicId.Add("1B5EA4DF");
            //oProviderPublicId.Add("4BB00F21");
            //oProviderPublicId.Add("1B794A38");
            //oProviderPublicId.Add("9BFCF011");
            //oProviderPublicId.Add("12A9DF49");
            //oProviderPublicId.Add("5DE716FF");
            //oProviderPublicId.Add("65B588D3");
            //oProviderPublicId.Add("635063A1");
            //oProviderPublicId.Add("D8DBEA96");
            //oProviderPublicId.Add("A4509BFE");
            //oProviderPublicId.Add("1C1280FA");
            //oProviderPublicId.Add("A5905C25");
            //oProviderPublicId.Add("1C267CFC");
            //oProviderPublicId.Add("F600C55C");
            //oProviderPublicId.Add("1357120D");
            //oProviderPublicId.Add("1C412255");
            //oProviderPublicId.Add("694C8743");
            //oProviderPublicId.Add("1371B766");
            //oProviderPublicId.Add("ADE40812");
            //oProviderPublicId.Add("13C850C9");
            //oProviderPublicId.Add("149457C9");
            //oProviderPublicId.Add("AF8E5DA7");
            //oProviderPublicId.Add("13E2F623");
            //oProviderPublicId.Add("159ECD47");
            //oProviderPublicId.Add("6EC37006");
            //oProviderPublicId.Add("1CE1026E");
            //oProviderPublicId.Add("6F8B4835");
            //oProviderPublicId.Add("1411977E");
            //oProviderPublicId.Add("17711ADD");
            //oProviderPublicId.Add("146830E1");
            //oProviderPublicId.Add("1D52412A");
            //oProviderPublicId.Add("B98C5F2A");
            //oProviderPublicId.Add("1482D63B");
            //oProviderPublicId.Add("1BDD8E38");
            //oProviderPublicId.Add("BB36B4BF");
            //oProviderPublicId.Add("1CE803B6");
            //oProviderPublicId.Add("14B17797");
            //oProviderPublicId.Add("1D9B87DF");
            //oProviderPublicId.Add("76D47EA4");
            //oProviderPublicId.Add("1DF22142");
            //oProviderPublicId.Add("138DD677");
            //oProviderPublicId.Add("1E061D44");
            //oProviderPublicId.Add("C4CA20F7");
            //oProviderPublicId.Add("1536B255");
            //oProviderPublicId.Add("22E42742");
            //oProviderPublicId.Add("7C08CA18");
            //oProviderPublicId.Add("1E48BAA5");
            //oProviderPublicId.Add("CD1DCCE3");
            //oProviderPublicId.Add("1E9F5406");
            //oProviderPublicId.Add("281872B6");
            //oProviderPublicId.Add("22E641C1");
            //oProviderPublicId.Add("1EB9F95F");
            //oProviderPublicId.Add("8204EDA4");
            //oProviderPublicId.Add("15EA8E72");
            //oProviderPublicId.Add("29EAC04C");
            //oProviderPublicId.Add("15FE8A75");
            //oProviderPublicId.Add("1EE89ABD");
            //oProviderPublicId.Add("D2F1F85D");
            //oProviderPublicId.Add("1F3F341E");
            //oProviderPublicId.Add("D85B8E8D");
            //oProviderPublicId.Add("166FC931");
            //oProviderPublicId.Add("4B64DF9A");
            //oProviderPublicId.Add("1683C533");
            //oProviderPublicId.Add("1F6DD57C");
            //oProviderPublicId.Add("DB45A44A");
            //oProviderPublicId.Add("30F15956");
            //oProviderPublicId.Add("16B2668F");
            //oProviderPublicId.Add("34535734");
            //oProviderPublicId.Add("1708FFF2");
            //oProviderPublicId.Add("54F84BAD");
            //oProviderPublicId.Add("8E3FD222");
            //oProviderPublicId.Add("20070C3D");
            //oProviderPublicId.Add("E4D9105E");
            //oProviderPublicId.Add("1737A14E");
            //oProviderPublicId.Add("36ED7CF9");
            //oProviderPublicId.Add("174B9D50");
            //oProviderPublicId.Add("37B55511");
            //oProviderPublicId.Add("90D9F7E7");
            //oProviderPublicId.Add("208C46FC");
            //oProviderPublicId.Add("943BF5AE");
            //oProviderPublicId.Add("20A042FE");
            //oProviderPublicId.Add("EE6C7C95");
            //oProviderPublicId.Add("17D0D80F");
            //oProviderPublicId.Add("3CE9A085");
            //oProviderPublicId.Add("20C83B06");
            //oProviderPublicId.Add("F0EBFCE4");
            //oProviderPublicId.Add("17F8D016");
            //oProviderPublicId.Add("3E7950B6");
            //oProviderPublicId.Add("184F6979");
            //oProviderPublicId.Add("41DB4E94");
            //oProviderPublicId.Add("1863657C");
            //oProviderPublicId.Add("6A9EA448");
            //oProviderPublicId.Add("1877617E");
            //oProviderPublicId.Add("6BDE6492");
            //oProviderPublicId.Add("188B5D81");
            //oProviderPublicId.Add("189F5985");
            //oProviderPublicId.Add("218969CC");
            //oProviderPublicId.Add("18F5F2E6");
            //oProviderPublicId.Add("485CACEA");
            //oProviderPublicId.Add("1909EEEB");
            //oProviderPublicId.Add("21F3FF31");
            //oProviderPublicId.Add("103A83FC");
            //oProviderPublicId.Add("2207FB36");
            //oProviderPublicId.Add("A310FFF1");
            //oProviderPublicId.Add("10627C01");
            //oProviderPublicId.Add("A73AD5E7");
            //oProviderPublicId.Add("22868C9E");
            //oProviderPublicId.Add("A802AE00");
            //oProviderPublicId.Add("229A88A0");
            //oProviderPublicId.Add("10E10D6B");
            //oProviderPublicId.Add("19CB1DB1");
            //oProviderPublicId.Add("811A27BE");
            //oProviderPublicId.Add("10FBB2C4");
            //oProviderPublicId.Add("8259E7E6");
            //oProviderPublicId.Add("110FAEC6");
            //oProviderPublicId.Add("54DA2ECD");
            //oProviderPublicId.Add("55A206E6");
            //oProviderPublicId.Add("117A442C");
            //oProviderPublicId.Add("8A42FE64");
            //oProviderPublicId.Add("1A71A720");
            //oProviderPublicId.Add("5731B72D");
            //oProviderPublicId.Add("11A23C31");
            //oProviderPublicId.Add("236FB36B");
            //oProviderPublicId.Add("11B63835");
            //oProviderPublicId.Add("922C1506");
            //oProviderPublicId.Add("1AF03888");
            //oProviderPublicId.Add("5C23653C");
            //oProviderPublicId.Add("1220CD9B");
            //oProviderPublicId.Add("285C2EEB");
            //oProviderPublicId.Add("302AA0C0");
            //oProviderPublicId.Add("1248C5A0");
            //oProviderPublicId.Add("1B32D5E8");
            //oProviderPublicId.Add("1C0DDC6F");
            //oProviderPublicId.Add("121548CA");
            //oProviderPublicId.Add("23DC4E43");
            //oProviderPublicId.Add("122944CD");
            //oProviderPublicId.Add("2BAABF39");
            //oProviderPublicId.Add("123D40D1");
            //oProviderPublicId.Add("33793030");
            //oProviderPublicId.Add("12513CD4");
            //oProviderPublicId.Add("126538D6");
            //oProviderPublicId.Add("6231936B");
            //oProviderPublicId.Add("12BBD239");
            //oProviderPublicId.Add("64E9FCC4");
            //oProviderPublicId.Add("12CFCE3C");
            //oProviderPublicId.Add("ADF3E428");
            //oProviderPublicId.Add("12E3CA3E");
            //oProviderPublicId.Add("BA7165B2");
            //oProviderPublicId.Add("12F7C643");
            //oProviderPublicId.Add("7C555085");
            //oProviderPublicId.Add("130BC245");
            //oProviderPublicId.Add("A784B603");
            //oProviderPublicId.Add("1C45C29A");
            //oProviderPublicId.Add("697AC9DA");
            //oProviderPublicId.Add("1C59BE9C");
            //oProviderPublicId.Add("ADC61D1A");
            //oProviderPublicId.Add("1F43B915");
            //oProviderPublicId.Add("B5948EEE");
            //oProviderPublicId.Add("139E4FB0");
            //oProviderPublicId.Add("6BD2523A");
            //oProviderPublicId.Add("6F345018");
            //oProviderPublicId.Add("1CEC4C07");
            //oProviderPublicId.Add("B32D0D1B");
            //oProviderPublicId.Add("1D00480B");
            //oProviderPublicId.Add("70C40049");
            //oProviderPublicId.Add("143E2FC8");
            //oProviderPublicId.Add("192F0DBB");
            //oProviderPublicId.Add("14522BCC");
            //oProviderPublicId.Add("19F6E5D4");
            //oProviderPublicId.Add("1D8C2C1F");
            //oProviderPublicId.Add("12EAB4A9");
            //oProviderPublicId.Add("1DA02824");
            //oProviderPublicId.Add("7702C13A");
            //oProviderPublicId.Add("1DB42426");
            //oProviderPublicId.Add("BFAA8EEC");
            //oProviderPublicId.Add("1DC82028");
            //oProviderPublicId.Add("C0EA4F36");
            //oProviderPublicId.Add("2FC69378");
            //oProviderPublicId.Add("795A499A");
            //oProviderPublicId.Add("C729101F");
            //oProviderPublicId.Add("1E40083C");
            //oProviderPublicId.Add("7D41822C");
            //oProviderPublicId.Add("1E54043E");
            //oProviderPublicId.Add("C9A8906E");
            //oProviderPublicId.Add("1E680041");
            //oProviderPublicId.Add("CAE850B9");
            //oProviderPublicId.Add("15989551");
            //oProviderPublicId.Add("26B7051B");
            //oProviderPublicId.Add("1ED295A6");
            //oProviderPublicId.Add("15FC8162");
            //oProviderPublicId.Add("2A9E3DAD");
            //oProviderPublicId.Add("16107D65");
            //oProviderPublicId.Add("2B6615C5");
            //oProviderPublicId.Add("236D8F0C");
            //oProviderPublicId.Add("46AFE320");
            //oProviderPublicId.Add("1638756A");
            //oProviderPublicId.Add("23AD824A");
            //oProviderPublicId.Add("8939C95B");
            //oProviderPublicId.Add("1F8671C1");
            //oProviderPublicId.Add("1614BDAC");
            //oProviderPublicId.Add("1F9A6DC5");
            //oProviderPublicId.Add("8AC9798C");
            //oProviderPublicId.Add("50ADE4A2");
            //oProviderPublicId.Add("16D85582");
            //oProviderPublicId.Add("51EDA4CA");
            //oProviderPublicId.Add("16EC5186");
            //oProviderPublicId.Add("1742EAE0");
            //oProviderPublicId.Add("375E5CB2");
            //oProviderPublicId.Add("203A4DD7");
            //oProviderPublicId.Add("91083A3B");
            //oProviderPublicId.Add("204E49D9");
            //oProviderPublicId.Add("91D01253");
            //oProviderPublicId.Add("206245DC");
            //oProviderPublicId.Add("EA8CAA6A");
            //oProviderPublicId.Add("33F06964");
            //oProviderPublicId.Add("935FC29B");
            //oProviderPublicId.Add("F0CB6B53");
            //oProviderPublicId.Add("20DA2DEF");
            //oProviderPublicId.Add("180419A9");
            //oProviderPublicId.Add("3EEA3085");
            //oProviderPublicId.Add("181815AB");
            //oProviderPublicId.Add("A30F7239");
            //oProviderPublicId.Add("182C11B0");
            //oProviderPublicId.Add("4079E0B7");
            //oProviderPublicId.Add("18400DB2");
            //oProviderPublicId.Add("188FFDBF");
            //oProviderPublicId.Add("44611948");
            //oProviderPublicId.Add("218760B3");
            //oProviderPublicId.Add("9E0AF6E7");
            //oProviderPublicId.Add("219B5CB5");
            //oProviderPublicId.Add("FE1E1800");
            //oProviderPublicId.Add("46760443");
            //oProviderPublicId.Add("10033030");
            //oProviderPublicId.Add("4A5D3CEB");
            //oProviderPublicId.Add("369A8BED");
            //oProviderPublicId.Add("10671C3E");
            //oProviderPublicId.Add("22349379");
            //oProviderPublicId.Add("107B1841");
            //oProviderPublicId.Add("22488F7B");
            //oProviderPublicId.Add("19727B35");
            //oProviderPublicId.Add("4D39FFFE");
            //oProviderPublicId.Add("2269DE2B");
            //oProviderPublicId.Add("A6E3DD88");
            //oProviderPublicId.Add("22C0778C");
            //oProviderPublicId.Add("19EA6348");
            //oProviderPublicId.Add("51E910A8");
            //oProviderPublicId.Add("19FE5F4B");
            //oProviderPublicId.Add("52B0E8D7");
            //oProviderPublicId.Add("29B6F87C");
            //oProviderPublicId.Add("113C4709");
            //oProviderPublicId.Add("2309BE43");
            //oProviderPublicId.Add("AD229E79");
            //oProviderPublicId.Add("11A03318");
            //oProviderPublicId.Add("236DAA52");
            //oProviderPublicId.Add("11B42F1A");
            //oProviderPublicId.Add("58AD0C64");
            //oProviderPublicId.Add("1AAB9211");
            //oProviderPublicId.Add("5974E47C");
            //oProviderPublicId.Add("1ABF8E13");
            //oProviderPublicId.Add("11E979CD");
            //oProviderPublicId.Add("1DFF1EE9");
            //oProviderPublicId.Add("11FD75D2");
            //oProviderPublicId.Add("5E23F53C");
            //oProviderPublicId.Add("1B377624");
            //oProviderPublicId.Add("97DFAEEF");
            //oProviderPublicId.Add("41961AD3");
            //oProviderPublicId.Add("12755DE3");
            //oProviderPublicId.Add("756DAC75");
            //oProviderPublicId.Add("1B6CC0D7");
            //oProviderPublicId.Add("6100B850");
            //oProviderPublicId.Add("1B80BCDB");
            //oProviderPublicId.Add("12FA98A1");
            //oProviderPublicId.Add("656D2BAB");
            //oProviderPublicId.Add("1BF1FB95");
            //oProviderPublicId.Add("A3880607");
            //oProviderPublicId.Add("DD8391DA");
            //oProviderPublicId.Add("132FE354");
            //oProviderPublicId.Add("9240AC1E");
            //oProviderPublicId.Add("1343DF57");
            //oProviderPublicId.Add("1393CF63");
            //oProviderPublicId.Add("128749C7");
            //oProviderPublicId.Add("2DAB83C2");
            //oProviderPublicId.Add("13B51E13");
            //oProviderPublicId.Add("13D45CAA");
            //oProviderPublicId.Add("4967B0B6");
            //oProviderPublicId.Add("AF305D1E");
            //oProviderPublicId.Add("15216F8C");
            //oProviderPublicId.Add("13EA68C6");
            //oProviderPublicId.Add("2EA0A82A");
            //oProviderPublicId.Add("207D5487");
            //oProviderPublicId.Add("19D08036");
            //oProviderPublicId.Add("146250D9");
            //oProviderPublicId.Add("B8C3C932");
            //oProviderPublicId.Add("1D59B3CD");
            //oProviderPublicId.Add("14839F87");
            //oProviderPublicId.Add("1BE56B31");
            //oProviderPublicId.Add("2F95CC93");
            //oProviderPublicId.Add("14E78B98");
            //oProviderPublicId.Add("C117751F");
            //oProviderPublicId.Add("1DDEEE8C");
            //oProviderPublicId.Add("C2573546");
            //oProviderPublicId.Add("2119B6A5");
            //oProviderPublicId.Add("21C7BD44");
            //oProviderPublicId.Add("21E18EBE");
            //oProviderPublicId.Add("1530D24D");
            //oProviderPublicId.Add("7B8B6C47");
            //oProviderPublicId.Add("25C8C765");
            //oProviderPublicId.Add("1594BE5B");
            //oProviderPublicId.Add("CBEAA17E");
            //oProviderPublicId.Add("1E8C214F");
            //oProviderPublicId.Add("CD2A61A5");
            //oProviderPublicId.Add("27DDB260");
            //oProviderPublicId.Add("15CA090E");
            //oProviderPublicId.Add("28A58A79");
            //oProviderPublicId.Add("1EC16C02");
            //oProviderPublicId.Add("47479E77");
            //oProviderPublicId.Add("1641F121");
            //oProviderPublicId.Add("D6BDCDB9");
            //oProviderPublicId.Add("1F395415");
            //oProviderPublicId.Add("16633FCF");
            //oProviderPublicId.Add("D8D2B8BD");
            //oProviderPublicId.Add("1F5AA2C6");
            //oProviderPublicId.Add("DA1278E4");
            //oProviderPublicId.Add("16DB27E2");
            //oProviderPublicId.Add("E05139CD");
            //oProviderPublicId.Add("1FD28AD7");
            //oProviderPublicId.Add("16FC7693");
            //oProviderPublicId.Add("349DD192");
            //oProviderPublicId.Add("1FF3D987");
            //oProviderPublicId.Add("E3A5E4F8");
            //oProviderPublicId.Add("35EAE474");
            //oProviderPublicId.Add("33BFA289");
            //oProviderPublicId.Add("1781B151");
            //oProviderPublicId.Add("92B42276");
            //oProviderPublicId.Add("20791446");
            //oProviderPublicId.Add("EBF990E5");
            //oProviderPublicId.Add("F229B9FD");
            //oProviderPublicId.Add("209A62F6");
            //oProviderPublicId.Add("17C44EB0");
            //oProviderPublicId.Add("3C6C42CA");
            //oProviderPublicId.Add("17D84AB2");
            //oProviderPublicId.Add("98B04603");
            //oProviderPublicId.Add("40537B5C");
            //oProviderPublicId.Add("183C36C3");
            //oProviderPublicId.Add("F66227D5");
            //oProviderPublicId.Add("213399B7");
            //oProviderPublicId.Add("185D8571");
            //oProviderPublicId.Add("6A40A3BF");
            //oProviderPublicId.Add("2154E867");
            //oProviderPublicId.Add("F9B6D301");
            //oProviderPublicId.Add("464F9EE8");
            //oProviderPublicId.Add("18D56D84");
            //oProviderPublicId.Add("FFF593E9");
            //oProviderPublicId.Add("479CB1CB");
            //oProviderPublicId.Add("18F6BC34");
            //oProviderPublicId.Add("73D40FD2");
            //oProviderPublicId.Add("21EE1F29");
            //oProviderPublicId.Add("19180AE2");
            //oProviderPublicId.Add("75E8FAD6");
            //oProviderPublicId.Add("22520B37");
            //oProviderPublicId.Add("197BF6F3");
            //oProviderPublicId.Add("A67ADAC8");
            //oProviderPublicId.Add("227359E7");
            //oProviderPublicId.Add("199D45A1");
            //oProviderPublicId.Add("C9FAA46B");
            //oProviderPublicId.Add("2294A898");
            //oProviderPublicId.Add("10DB2D60");
            //oProviderPublicId.Add("5032FB1C");
            //oProviderPublicId.Add("22F894A6");
            //oProviderPublicId.Add("1A228060");
            //oProviderPublicId.Add("541A33AD");
            //oProviderPublicId.Add("2319E356");
            //oProviderPublicId.Add("1A43CF10");
            //oProviderPublicId.Add("88A53DB3");
            //oProviderPublicId.Add("233B3204");
            //oProviderPublicId.Add("AF112419");
            //oProviderPublicId.Add("56B45972");
            //oProviderPublicId.Add("11E5A2DD");
            //oProviderPublicId.Add("90F8E9A0");
            //oProviderPublicId.Add("1673C8C9");
            //oProviderPublicId.Add("1206F18E");
            //oProviderPublicId.Add("930DD4A4");
            //oProviderPublicId.Add("1228403E");
            //oProviderPublicId.Add("4A7EBB4F");
            //oProviderPublicId.Add("1B6F933E");
            //oProviderPublicId.Add("9B618090");
            //oProviderPublicId.Add("57817828");
            //oProviderPublicId.Add("2FD09703");
            //oProviderPublicId.Add("9D766B71");
            //oProviderPublicId.Add("64843501");
            //oProviderPublicId.Add("1E17A911");
            //oProviderPublicId.Add("9F8B5675");
            //oProviderPublicId.Add("DF471117");
            //oProviderPublicId.Add("1C161CAD");
            //oProviderPublicId.Add("13400867");
            //oProviderPublicId.Add("A69F423A");
            //oProviderPublicId.Add("2D257892");
            //oProviderPublicId.Add("13615717");
            //oProviderPublicId.Add("108E96D6");
            //oProviderPublicId.Add("1C58BA0B");
            //oProviderPublicId.Add("1382A5C8");
            //oProviderPublicId.Add("6D57D4D8");
            //oProviderPublicId.Add("1CBCA61A");
            //oProviderPublicId.Add("13E691D6");
            //oProviderPublicId.Add("13F3E482");
            //oProviderPublicId.Add("23A694C9");
            //oProviderPublicId.Add("1CEB4778");
            //oProviderPublicId.Add("14153332");
            //oProviderPublicId.Add("11FE97E4");
            //oProviderPublicId.Add("1D0C9626");
            //oProviderPublicId.Add("745E6DE2");
            //oProviderPublicId.Add("1C01A33B");
            //oProviderPublicId.Add("149A6DF1");
            //oProviderPublicId.Add("BC459AD4");
            //oProviderPublicId.Add("2EE45673");
            //oProviderPublicId.Add("1D9F2393");
            //oProviderPublicId.Add("14C90F4C");
            //oProviderPublicId.Add("1518FF59");
            //oProviderPublicId.Add("C42EB152");
            //oProviderPublicId.Add("2240642C");
            //oProviderPublicId.Add("153A4E09");
            //oProviderPublicId.Add("C6439C56");
            //oProviderPublicId.Add("238D76F9");
            //oProviderPublicId.Add("1E3F03AB");
            //oProviderPublicId.Add("1568EF65");
            //oProviderPublicId.Add("3AF742F9");
            //oProviderPublicId.Add("1EA2EFB9");
            //oProviderPublicId.Add("15CCDB75");
            //oProviderPublicId.Add("81A3C7DD");
            //oProviderPublicId.Add("2946FD36");
            //oProviderPublicId.Add("1ED19115");
            //oProviderPublicId.Add("82F0DAC0");
            //oProviderPublicId.Add("2A941019");
            //oProviderPublicId.Add("1EF2DFC5");
            //oProviderPublicId.Add("161CCB81");
            //oProviderPublicId.Add("875D4E1B");
            //oProviderPublicId.Add("2F00835E");
            //oProviderPublicId.Add("1F641E82");
            //oProviderPublicId.Add("88AA60FD");
            //oProviderPublicId.Add("892F9BB1");
            //oProviderPublicId.Add("30D2D10A");
            //oProviderPublicId.Add("16AF58EC");
            //oProviderPublicId.Add("DD944A86");
            //oProviderPublicId.Add("8D9C0F0C");
            //oProviderPublicId.Add("E3687600");
            //oProviderPublicId.Add("8EA6848A");
            //oProviderPublicId.Add("20115145");
            //oProviderPublicId.Add("3B78EE19");
            //oProviderPublicId.Add("3796CCAF");
            //oProviderPublicId.Add("203FF2A3");
            //oProviderPublicId.Add("9140AA39");
            //oProviderPublicId.Add("ED667782");
            //oProviderPublicId.Add("3C03400B");
            //oProviderPublicId.Add("20B1315D");
            //oProviderPublicId.Add("EF7B6286");
            //oProviderPublicId.Add("3D5052ED");
            //oProviderPublicId.Add("20D2800E");
            //oProviderPublicId.Add("17FC6BCA");
            //oProviderPublicId.Add("977F6B2A");
            //oProviderPublicId.Add("41BCC632");
            //oProviderPublicId.Add("2715DDA0");
            //oProviderPublicId.Add("F9796409");
            //oProviderPublicId.Add("438F13DE");
            //oProviderPublicId.Add("21726026");
            //oProviderPublicId.Add("9D38F168");
            //oProviderPublicId.Add("44DC26AB");
            //oProviderPublicId.Add("2193AED6");
            //oProviderPublicId.Add("21E39EE2");
            //oProviderPublicId.Add("190D8A9C");
            //oProviderPublicId.Add("10377658");
            //oProviderPublicId.Add("2212403E");
            //oProviderPublicId.Add("193C2BFA");
            //oProviderPublicId.Add("4B1AE79C");
            //oProviderPublicId.Add("22338EEE");
            //oProviderPublicId.Add("2240E19A");
            //oProviderPublicId.Add("19AD6AB4");
            //oProviderPublicId.Add("10D75670");
            //oProviderPublicId.Add("801422CF");
            //oProviderPublicId.Add("22B22056");
            //oProviderPublicId.Add("19DC0C12");
            //oProviderPublicId.Add("1105F7CC");
            //oProviderPublicId.Add("51DEE357");
            //oProviderPublicId.Add("22E0C1B2");
            //oProviderPublicId.Add("1A4D4ACC");
            //oProviderPublicId.Add("1A5A9D7A");
            //oProviderPublicId.Add("11848936");
            //oProviderPublicId.Add("8AE74F0A");
            //oProviderPublicId.Add("235F531C");
            //oProviderPublicId.Add("1A893ED6");
            //oProviderPublicId.Add("B0FFA9B9");
            //oProviderPublicId.Add("58A2DEFC");
            //oProviderPublicId.Add("1B5FF1AC");
            //oProviderPublicId.Add("941025D3");
            //oProviderPublicId.Add("29C5010B");
            //oProviderPublicId.Add("1B1BCC43");
            //oProviderPublicId.Add("1245B7FC");
            //oProviderPublicId.Add("96FA3B91");
            //oProviderPublicId.Add("3BFC08E8");
            //oProviderPublicId.Add("1B4A6D9E");
            //oProviderPublicId.Add("1B9A5DAA");
            //oProviderPublicId.Add("1BA7B056");
            //oProviderPublicId.Add("12D19C12");
            //oProviderPublicId.Add("6D6CD65B");
            //oProviderPublicId.Add("4721452F");
            //oProviderPublicId.Add("12F2EAC2");
            //oProviderPublicId.Add("652060DF");
            //oProviderPublicId.Add("1BF7A062");
            //oProviderPublicId.Add("1C47906E");
            //oProviderPublicId.Add("1F1BF9DD");
            //oProviderPublicId.Add("A9B67E6E");
            //oProviderPublicId.Add("11B54447");
            //oProviderPublicId.Add("1C7631CC");
            //oProviderPublicId.Add("13A01D86");
            //oProviderPublicId.Add("11434204");
            //oProviderPublicId.Add("138791DD");
            //oProviderPublicId.Add("39FD4DAA");
            //oProviderPublicId.Add("1CF4C334");
            //oProviderPublicId.Add("141EAEF0");
            //oProviderPublicId.Add("B489AAA9");
            //oProviderPublicId.Add("18794003");
            //oProviderPublicId.Add("1D236490");
            //oProviderPublicId.Add("144D504C");
            //oProviderPublicId.Add("B773C066");
            //oProviderPublicId.Add("1A4B8D99");
            //oProviderPublicId.Add("2F117CE9");
            //oProviderPublicId.Add("1DA1F5F8");
            //oProviderPublicId.Add("BF5CD708");
            //oProviderPublicId.Add("C03201C1");
            //oProviderPublicId.Add("1FC27671");
            //oProviderPublicId.Add("1DDDEA01");
            //oProviderPublicId.Add("1507D5BD");
            //oProviderPublicId.Add("7C8BB473");
            //oProviderPublicId.Add("C81B183F");
            //oProviderPublicId.Add("24B42480");
            //oProviderPublicId.Add("1E5C7B69");
            //oProviderPublicId.Add("15866725");
            //oProviderPublicId.Add("CB052E1F");
            //oProviderPublicId.Add("26867217");
            //oProviderPublicId.Add("1E8B1CC7");
            //oProviderPublicId.Add("2A2B0D59");
            //oProviderPublicId.Add("44B7D583");
            //oProviderPublicId.Add("458D003C");
            //oProviderPublicId.Add("1F09AE2F");
            //oProviderPublicId.Add("2385C312");
            //oProviderPublicId.Add("15626F6F");
            //oProviderPublicId.Add("2D4A6DD2");
            //oProviderPublicId.Add("1F384F8B");
            //oProviderPublicId.Add("1F883F97");
            //oProviderPublicId.Add("1F959245");
            //oProviderPublicId.Add("16BF7DFF");
            //oProviderPublicId.Add("8B1E2151");
            //oProviderPublicId.Add("165793D7");
            //oProviderPublicId.Add("3346915E");
            //oProviderPublicId.Add("33CBCC12");
            //oProviderPublicId.Add("1FDED8FA");
            //oProviderPublicId.Add("202EC906");
            //oProviderPublicId.Add("1758B4C2");
            //oProviderPublicId.Add("911A44DE");
            //oProviderPublicId.Add("38BD7A37");
            //oProviderPublicId.Add("5B9DEE45");
            //oProviderPublicId.Add("206ABD10");
            //oProviderPublicId.Add("1794A8CA");
            //oProviderPublicId.Add("9371CD3E");
            //oProviderPublicId.Add("F0E84924");
            //oProviderPublicId.Add("9716686A");
            //oProviderPublicId.Add("979BA334");
            //oProviderPublicId.Add("65315A58");
            //oProviderPublicId.Add("2103F3D1");
            //oProviderPublicId.Add("2111467D");
            //oProviderPublicId.Add("183B3239");
            //oProviderPublicId.Add("F651DF54");
            //oProviderPublicId.Add("9D128C0D");
            //oProviderPublicId.Add("18AC70F5");
            //oProviderPublicId.Add("9E5F9ED9");
            //oProviderPublicId.Add("70D9B170");
            //oProviderPublicId.Add("21BE7942");
            //oProviderPublicId.Add("18E864FF");
            //oProviderPublicId.Add("221BBBFA");
            //oProviderPublicId.Add("22290EA8");
            //oProviderPublicId.Add("1952FA64");
            //oProviderPublicId.Add("19604D10");
            //oProviderPublicId.Add("196D9FBD");
            //oProviderPublicId.Add("197AF269");
            //oProviderPublicId.Add("10A4DE25");
            //oProviderPublicId.Add("7CEC9DF7");
            //oProviderPublicId.Add("51334333");
            //oProviderPublicId.Add("51B87DFC");
            //oProviderPublicId.Add("22DCEAC2");
            //oProviderPublicId.Add("22EA3D70");
            //oProviderPublicId.Add("1A14292A");
            //oProviderPublicId.Add("AC6CD103");
            //oProviderPublicId.Add("86800A0A");
            //oProviderPublicId.Add("875534E7");
            //oProviderPublicId.Add("57B4A189");
            //oProviderPublicId.Add("23762184");
            //oProviderPublicId.Add("1AA00D40");
            //oProviderPublicId.Add("1AAD5FEE");
            //oProviderPublicId.Add("11D74BA8");
            //oProviderPublicId.Add("90137642");
            //oProviderPublicId.Add("90E8A0FB");
            //oProviderPublicId.Add("91BDCBD8");
            //oProviderPublicId.Add("5E35FFDF");
            //oProviderPublicId.Add("3A7C135B");
            //oProviderPublicId.Add("3FB05F3E");
            //oProviderPublicId.Add("2BB9755E");
            //oProviderPublicId.Add("1B613C08");
            //oProviderPublicId.Add("128B27C2");
            //oProviderPublicId.Add("6112C2F3");
            //oProviderPublicId.Add("9C2662A4");
            //oProviderPublicId.Add("64B75E1F");
            //oProviderPublicId.Add("C5A93791");
            //oProviderPublicId.Add("80BE0DBF");
            //oProviderPublicId.Add("1BFA72C9");
            //oProviderPublicId.Add("1C07C577");
            //oProviderPublicId.Add("1331B131");
            //oProviderPublicId.Add("1C291425");
            //oProviderPublicId.Add("1256B705");
            //oProviderPublicId.Add("1C8656DF");
            //oProviderPublicId.Add("1C93A98B");
            //oProviderPublicId.Add("13BD9547");
            //oProviderPublicId.Add("6D0B0A0C");
            //oProviderPublicId.Add("6D9044D6");
            //oProviderPublicId.Add("6E157F89");
            //oProviderPublicId.Add("22C1216B");
            //oProviderPublicId.Add("18D8155B");
            //oProviderPublicId.Add("195D500F");
            //oProviderPublicId.Add("1D3A32FA");
            //oProviderPublicId.Add("1D4785A8");
            //oProviderPublicId.Add("14717161");
            //oProviderPublicId.Add("147EC40F");
            //oProviderPublicId.Add("7496DDE0");
            //oProviderPublicId.Add("BB602776");
            //oProviderPublicId.Add("783B7922");
            //oProviderPublicId.Add("C13452F0");
            //oProviderPublicId.Add("2063E92F");
            //oProviderPublicId.Add("20E923E3");
            //oProviderPublicId.Add("1DFB61C2");
            //oProviderPublicId.Add("15254D7C");
            //oProviderPublicId.Add("1532A02A");
            //oProviderPublicId.Add("7B9D76EA");
            //oProviderPublicId.Add("11D8C680");
            //oProviderPublicId.Add("11E6192D");
            //oProviderPublicId.Add("5AA0310E");
            //oProviderPublicId.Add("91D57959");
            //oProviderPublicId.Add("92AAA412");
            //oProviderPublicId.Add("12220D37");
            //oProviderPublicId.Add("122F5FE3");
            //oProviderPublicId.Add("127F4FEF");
            //oProviderPublicId.Add("128CA29C");
            //oProviderPublicId.Add("9C3E1049");
            //oProviderPublicId.Add("622C04E1");
            //oProviderPublicId.Add("6218464F");
            //oProviderPublicId.Add("674C9154");
            //oProviderPublicId.Add("1BB95043");
            //oProviderPublicId.Add("DF910316");
            //oProviderPublicId.Add("1C1692FB");
            //oProviderPublicId.Add("F0385AEF");
            //oProviderPublicId.Add("1C313854");
            //oProviderPublicId.Add("1C3E8B02");
            //oProviderPublicId.Add("1C4BDDAE");
            //oProviderPublicId.Add("1C59305B");
            //oProviderPublicId.Add("1437D122");
            //oProviderPublicId.Add("14BD0BD6");
            //oProviderPublicId.Add("2E060935");
            //oProviderPublicId.Add("1CD1186C");
            //oProviderPublicId.Add("1CDE6B1A");
            //oProviderPublicId.Add("140856D4");
            //oProviderPublicId.Add("1415A982");
            //oProviderPublicId.Add("1422FC2D");
            //oProviderPublicId.Add("1472EC3A");
            //oProviderPublicId.Add("14803EE7");
            //oProviderPublicId.Add("74A5AA51");
            //oProviderPublicId.Add("BB77D4F7");
            //oProviderPublicId.Add("75B01FCE");
            //oProviderPublicId.Add("BD222A8D");
            //oProviderPublicId.Add("2FC0E62C");
            //oProviderPublicId.Add("30961108");
            //oProviderPublicId.Add("217D2B1E");
            //oProviderPublicId.Add("220265D1");
            //oProviderPublicId.Add("1E1781F4");
            //oProviderPublicId.Add("303AEDCF");
            //oProviderPublicId.Add("1E32274D");
            //oProviderPublicId.Add("155C1309");
            //oProviderPublicId.Add("156965B5");
            //oProviderPublicId.Add("1E9CBCB2");
            //oProviderPublicId.Add("1EAA0F60");
            //oProviderPublicId.Add("1EB7620C");
            //oProviderPublicId.Add("1EC4B4B9");
            //oProviderPublicId.Add("15EEA073");
            //oProviderPublicId.Add("15FBF321");
            //oProviderPublicId.Add("2B1DE9ED");
            //oProviderPublicId.Add("2E3D4A65");
            //oProviderPublicId.Add("2EC28519");
            //oProviderPublicId.Add("2F47BFE3");
            //oProviderPublicId.Add("2FCCFA97");
            //oProviderPublicId.Add("1F7890D4");
            //oProviderPublicId.Add("1F85E380");
            //oProviderPublicId.Add("1F93362D");
            //oProviderPublicId.Add("329A748F");
            //oProviderPublicId.Add("E3054592");
            //oProviderPublicId.Add("17210DF8");
            //oProviderPublicId.Add("E4AF9B28");
            //oProviderPublicId.Add("8F72FBC2");
            //oProviderPublicId.Add("E659F0BD");
            //oProviderPublicId.Add("907D7140");
            //oProviderPublicId.Add("9102ABF4");
            //oProviderPublicId.Add("17B39B62");
            //oProviderPublicId.Add("17C0EE10");
            //oProviderPublicId.Add("17CE40BC");
            //oProviderPublicId.Add("17DB936A");
            //oProviderPublicId.Add("17E8E615");
            //oProviderPublicId.Add("17F638C3");
            //oProviderPublicId.Add("F20247D5");
            //oProviderPublicId.Add("97C6A7AF");
            //oProviderPublicId.Add("3E68762B");
            //oProviderPublicId.Add("186E20D4");
            //oProviderPublicId.Add("187B7382");
            //oProviderPublicId.Add("1888C62D");
            //oProviderPublicId.Add("189618DB");
            //oProviderPublicId.Add("45236330");
            //oProviderPublicId.Add("739E05DA");
            //oProviderPublicId.Add("74733094");
            //oProviderPublicId.Add("75485B70");
            //oProviderPublicId.Add("49D273DA");
            //oProviderPublicId.Add("4A57AEA3");
            //oProviderPublicId.Add("4ADCE957");
            //oProviderPublicId.Add("4B622421");
            //oProviderPublicId.Add("223AAE93");
            //oProviderPublicId.Add("A7E8C4BE");
            //oProviderPublicId.Add("10D7CCC0");
            //oProviderPublicId.Add("10E51F6C");
            //oProviderPublicId.Add("80F0B288");
            //oProviderPublicId.Add("511BAA49");
            //oProviderPublicId.Add("51A0E512");
            //oProviderPublicId.Add("111A6A1F");
            //oProviderPublicId.Add("AE2785AF");
            //oProviderPublicId.Add("1177ACD8");
            //oProviderPublicId.Add("1A686676");
            //oProviderPublicId.Add("1A75B923");
            //oProviderPublicId.Add("1A830BCF");
            //oProviderPublicId.Add("11ACF78B");
            //oProviderPublicId.Add("11BA4A37");
            //oProviderPublicId.Add("11C79CE5");
            //oProviderPublicId.Add("12178CF1");
            //oProviderPublicId.Add("1224DF9C");
            //oProviderPublicId.Add("1232324A");
            //oProviderPublicId.Add("123F84F6");
            //oProviderPublicId.Add("1D47BF6C");
            //oProviderPublicId.Add("125A2A4F");
            //oProviderPublicId.Add("12677CFD");
            //oProviderPublicId.Add("1B9AD3FA");
            //oProviderPublicId.Add("1BA826A6");
            //oProviderPublicId.Add("1BB57954");
            //oProviderPublicId.Add("1BC2CBFF");
            //oProviderPublicId.Add("BBDBFDB5");
            //oProviderPublicId.Add("7A9DC996");
            //oProviderPublicId.Add("CC83558E");
            //oProviderPublicId.Add("694ED5F5");
            //oProviderPublicId.Add("A9534DDC");
            //oProviderPublicId.Add("1385EE7D");
            //oProviderPublicId.Add("13934128");
            //oProviderPublicId.Add("13A093D6");
            //oProviderPublicId.Add("1C97F6CA");
            //oProviderPublicId.Add("2445B58A");
            //oProviderPublicId.Add("1730CC40");
            //oProviderPublicId.Add("70980C64");
            //oProviderPublicId.Add("B4FBA4F3");
            //oProviderPublicId.Add("B5D0CFD0");
            //oProviderPublicId.Add("B6A5FA89");
            //oProviderPublicId.Add("B77B2566");
            //oProviderPublicId.Add("14AB0954");
            //oProviderPublicId.Add("14B85BFF");
            //oProviderPublicId.Add("1DA9159F");
            //oProviderPublicId.Add("1DB6684A");
            //oProviderPublicId.Add("1DC3BAF8");
            //oProviderPublicId.Add("2FB4E2A0");
            //oProviderPublicId.Add("1DDE6051");
            //oProviderPublicId.Add("236BB0BE");
            //oProviderPublicId.Add("23F0EB71");
            //oProviderPublicId.Add("3A56A392");
            //oProviderPublicId.Add("5EAC7D46");
            //oProviderPublicId.Add("25809BB9");
            //oProviderPublicId.Add("2605D66D");
            //oProviderPublicId.Add("CBE1BDD8");
            //oProviderPublicId.Add("CCB6E892");
            //oProviderPublicId.Add("15FEC586");
            //oProviderPublicId.Add("160C1834");
            //oProviderPublicId.Add("16196AE0");
            //oProviderPublicId.Add("1626BD8E");
            //oProviderPublicId.Add("16341039");
            //oProviderPublicId.Add("2D4F0CDC");
            //oProviderPublicId.Add("895072C5");
            //oProviderPublicId.Add("16ABF84C");
            //oProviderPublicId.Add("16B94AF8");
            //oProviderPublicId.Add("16C69DA6");
            //oProviderPublicId.Add("1FB75743");
            //oProviderPublicId.Add("1FC4A9F1");
            //oProviderPublicId.Add("1FD1FC9C");
            //oProviderPublicId.Add("36EFCBAB");
            //oProviderPublicId.Add("90570BCF");
            //oProviderPublicId.Add("172D7BEF");
            //oProviderPublicId.Add("9161814C");
            //oProviderPublicId.Add("91E6BC16");
            //oProviderPublicId.Add("1787CC6C");
            //oProviderPublicId.Add("17951F1A");
            //oProviderPublicId.Add("17A271C5");
            //oProviderPublicId.Add("20D5C8C3");
            //oProviderPublicId.Add("3E7B9F7E");
            //oProviderPublicId.Add("3F00DA32");
            //oProviderPublicId.Add("3F8614FC");
            //oProviderPublicId.Add("400B4FB0");
            //oProviderPublicId.Add("A5491094");
            //oProviderPublicId.Add("6822D515");
            //oProviderPublicId.Add("FB5844F1");
            //oProviderPublicId.Add("9D9C65E0");
            //oProviderPublicId.Add("9E21A094");
            //oProviderPublicId.Add("9EA6DB5E");
            //oProviderPublicId.Add("9F2C1612");
            //oProviderPublicId.Add("18DB88A1");
            //oProviderPublicId.Add("18E8DB4C");
            //oProviderPublicId.Add("221C324A");
            //oProviderPublicId.Add("4B3BBEC6");
            //oProviderPublicId.Add("4BC0F97A");
            //oProviderPublicId.Add("A528399E");
            //oProviderPublicId.Add("1974BF62");
            //oProviderPublicId.Add("1982120E");
            //oProviderPublicId.Add("198F64BB");
            //oProviderPublicId.Add("22C2BBB9");
            //oProviderPublicId.Add("51BD1D06");
            //oProviderPublicId.Add("524257D0");
            //oProviderPublicId.Add("52C79284");
            //oProviderPublicId.Add("534CCD4E");
            //oProviderPublicId.Add("ACB40D72");
            //oProviderPublicId.Add("1A3C977F");
            //oProviderPublicId.Add("38B3172E");
            //oProviderPublicId.Add("237D412B");
            //oProviderPublicId.Add("8DD0C38E");
            //oProviderPublicId.Add("8F45B065");
            //oProviderPublicId.Add("5A10C8F3");
            //oProviderPublicId.Add("90F005FB");
            //oProviderPublicId.Add("11FFB9FC");
            //oProviderPublicId.Add("1B3310FA");
            //oProviderPublicId.Add("2B9A390D");
            //oProviderPublicId.Add("1B4DB654");
            //oProviderPublicId.Add("1B5B0901");
            //oProviderPublicId.Add("4CE15278");
            //oProviderPublicId.Add("52159E5B");
            //oProviderPublicId.Add("DF7588A4");
            //oProviderPublicId.Add("A12CC865");
            //oProviderPublicId.Add("1E6C09D2");
            //oProviderPublicId.Add("1310D8D1");
            //oProviderPublicId.Add("131E2B7C");
            //oProviderPublicId.Add("1C0EE51C");
            //oProviderPublicId.Add("1C1C37C7");
            //oProviderPublicId.Add("2D0F43EF");
            //oProviderPublicId.Add("125B5625");
            //oProviderPublicId.Add("AC6A8A0F");
            //oProviderPublicId.Add("6C47D113");
            //oProviderPublicId.Add("13C4B4EB");
            //oProviderPublicId.Add("1FB6728F");
            //oProviderPublicId.Add("13DF5A45");
            //oProviderPublicId.Add("189A1716");
            //oProviderPublicId.Add("191F51CA");
            //oProviderPublicId.Add("72869204");
            //oProviderPublicId.Add("B812E127");
            //oProviderPublicId.Add("1471E7B1");
            //oProviderPublicId.Add("147F3A5D");
            //oProviderPublicId.Add("148C8D0B");
            //oProviderPublicId.Add("1D7D46A8");
            //oProviderPublicId.Add("1FA0B020");
            //oProviderPublicId.Add("2025EAEA");
            //oProviderPublicId.Add("C27B7817");
            //oProviderPublicId.Add("7A1265D8");

            #endregion

            ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider = new ProviderModel();

            oProviderPublicId.All(x =>
            {
                oProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = x,
                    },
                    RelatedAditionalDocuments = new List<GenericItemModel>()
                    {
                        new GenericItemModel()
                        {
                            ItemId = 0,
                            ItemType = new CatalogModel()
                            {
                                ItemId = 1701002,
                            },
                            ItemName = "a) ¿CUÁLES SON LOS DIAS Y HORARIOS DEFINIDOS POR SU EMPRESA PARA LA RECEPCION DEL PRODUCTO?",
                            Enable = true,
                            ItemInfo = new List<GenericItemInfoModel>(),
                        },
                    },
                };

                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.Add(
                    new GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new CatalogModel()
                        {
                            ItemId = 1703004, //Related User
                        },
                        Value = "david.moncayo@publicar.com",
                        Enable = true,
                    });

                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.Add(
                        new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new CatalogModel()
                            {
                                ItemId = 1703003, //Related customer
                            },
                            Value = "10A0C1D5", //Clientes - Polipropileno
                            Enable = true,
                        });

                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.Add(
                        new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new CatalogModel()
                            {
                                ItemId = 1703001, //DataType
                            },
                            Value = "1901003",
                            Enable = true,
                        });

                //Upsert 1 Question
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                //Upsert 2 Question
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemId = 0;
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.All(y =>
                {
                    y.ItemInfoId = 0;
                    return true;
                });
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemName = "b) ¿CUÁLES SON LOS DOCUMENTOS REQUERIDOS POR SU EMPRESA, PARA REALIZAR LA ENTREGA DEL PRODUCTO?";
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                //Upsert 3 Question
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemId = 0;
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.All(y =>
                {
                    y.ItemInfoId = 0;
                    return true;
                });
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemName = "c) ¿ES NECESARIO REALIZAR SOLICITUD DE CITA PREVIA PARA LA ENTREGA DEL PRODUCTO? CON CUÁNTO TIEMPO DE ANTICIPACIÓN A LA FECHA DE ENTREGA SE DEBE SOLICITAR LA CITA Y CON QUIEN?";
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                //Upsert 4 Question
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemId = 0;
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.All(y =>
                {
                    y.ItemInfoId = 0;
                    return true;
                });
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemName = "d) ¿EN LA ENTREGA DEL PRODUCTO, USTEDES REQUIEREN AUXILIARES DE DESCARGUE? CUÁNTOS?";
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                //Upsert 5 Question
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemId = 0;
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.All(y =>
                {
                    y.ItemInfoId = 0;
                    return true;
                });
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemName = "e) ¿CUÁLES SON LOS ELEMENTOS DE SEGURIDAD QUE DEBEN TENER LOS AUXILIARES DE DESCARGUE? QUÉ DOCUMENTOS SON REQUERIDOS DEL AUXILIAR?";
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                //Upsert 6 Question
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemId = 0;
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemInfo.All(y =>
                {
                    y.ItemInfoId = 0;
                    return true;
                });
                oProvider.RelatedAditionalDocuments.FirstOrDefault().ItemName = "f) ¿CUÁL ES EL DIA DE CIERRE CONTABLE EN SU EMPRESA? DESPUÉS DE ESTE DÍA USTEDES RECIBEN PRODUCTO?";
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.AditionalDocumentsUpsert(oProvider);

                return true;
            });

            Assert.AreEqual(true, oProvider.RelatedAditionalDocuments != null);

        }

        #region MarketPlace

        #region SearchProviders

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
        public void MPProviderSearchNew()
        {
            int TotalRows;

            List<Models.Provider.ProviderModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchNew
                ("1EA5A78A",
                false,
                null,
                null,
                113002,
                false,
                0,
                60000,
                out TotalRows);

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
        public void MPProviderSearchFilterNew()
        {
            List<Company.Models.Util.GenericFilterModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchFilterNew
                    ("1EA5A78A",
                    "",
                    null,
                    true);

            Assert.AreEqual(true, oResult.Count > 0);

        }

        [TestMethod]
        public void GetAllProvidersByCustomerPublicId()
        {
            List<CompanyModel> oCompanyList = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicId("7BC27832");
            Assert.IsNotNull(oCompanyList);
        }
        #endregion        

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
                ("1D9B9580", 2014, 108001);

            Assert.AreEqual(true, oResult.Count > 0);
        }

        [TestMethod]
        public void MPCustomerProviderGetTracking()
        {
            List<GenericItemModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCustomerProviderGetTracking
                ("1A9863BD", "1D9B9580");

            Assert.AreEqual(true, oResult.Count > 0);
        }

        [TestMethod]
        public void MPFinancialGetLastyearInfoDeta()
        {
            List<GenericItemModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetLastyearInfoDeta
                ("1D9B9580");

            Assert.AreEqual(true, oResult.Count > 0);
        }

        [TestMethod]
        public void MPCertificationGetSpecificCert()
        {
            List<GenericItemInfoModel> oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetSpecificCert
                ("1D9B9580");

            Assert.AreEqual(true, oResult.Count > 0);
        }

        [TestMethod]
        public void MPCustomerProviderGetAllTracking()
        {
            GenericItemModel oResult =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCustomerProviderGetAllTracking
                ("DA5C572E", "18A8F37B");

            Assert.AreEqual(true, oResult != null && oResult.ItemInfo.Count > 0);
        }

        [TestMethod]
        public void MPLegalInfoGetBasicInfo()
        {
            List<Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPReportGetBasicInfo("1D9B9580", null);
            Assert.AreEqual(true, oReturn.Count >= 1);
        }

        [TestMethod]
        public void MPBlackListGetBasicInfo()
        {
            List<BlackListModel> oReturn =
               ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListGetBasicInfo("50583E9D");
            Assert.AreEqual(true, oReturn.Count >= 1);
        }
        #endregion

        #region Charts

        [TestMethod]
        public void GetProvidersByState()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> nazi = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetProvidersByState("26D388E3");
        }

        #endregion
    }
}
