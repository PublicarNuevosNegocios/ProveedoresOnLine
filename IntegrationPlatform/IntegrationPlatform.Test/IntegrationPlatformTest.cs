using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationPlatform.Test
{
    [TestClass]
    public class IntegrationPlatformTest
    {
        #region Integration

        [TestMethod]
        public void CustomerProvider_GetField()
        {
            List<string> oCustomer = new List<string>();

            string ProviderPublicId = "90AF9E9D";

            List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oRelatedCustomer = new List<ProveedoresOnLine.Company.Models.Company.CompanyModel>()
            {
                new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = "7BC27832",
                    CompanyName = "SANOFI AVENTIS DE COLOMBIA S.A",
                },

                new ProveedoresOnLine.Company.Models.Company.CompanyModel(){
                    CompanyPublicId = "DA5C572E",
                    CompanyName = "PUBLICAR PUBLICIDAD MULTIMEDIA SAS",
                },
            };

            List<IntegrationPlatform.Models.Integration.CustomDataModel> oReturn = IntegrationPlatform.Controller.IntegrationPlatform.CustomerProvider_GetCustomData(oRelatedCustomer, ProviderPublicId);

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }

        #region Sanofi

        [TestMethod]
        public void CustomerProvider_CustomData_Upsert()
        {
            IntegrationPlatform.Models.Integration.CustomDataModel oCustomDataModel = new Models.Integration.CustomDataModel()
            {
                CustomData = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>()
                {
                    new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                    {
                        ItemId = 0,
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = 1,
                        },
                        ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                        {
                            new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                Value = "Texto de prueba dll 1.0",
                                Enable = true,
                            },
                        },
                        Enable = true,
                    }
                },
                RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = "7BC27832",
                },
            };

            oCustomDataModel = IntegrationPlatform.Controller.IntegrationPlatform.CustomerProvider_CustomData_Upsert(oCustomDataModel, "F5112365");

            Assert.AreEqual(true, oCustomDataModel.CustomData != null && oCustomDataModel.CustomData.Count > 0);
        }

        [TestMethod]
        public void CustomerProvider_Sanofi_CustomDataInfo_Upsert()
        {
            ProveedoresOnLine.Company.Models.Util.GenericItemModel oReturn = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
            {
                ItemId = 8,
                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                {
                    new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 8,
                        Value = "201005",
                        Enable  = true,
                    },
                },
            };

            oReturn = IntegrationPlatform.Controller.IntegrationPlatform.CustomerProvider_Sanofi_CustomDataInfo_Upsert(oReturn);

            Assert.AreEqual(true, oReturn != null);
        }

        [TestMethod]
        public void CustomerProvider_Publicar_CustomDataInfo_Upsert()
        {
            ProveedoresOnLine.Company.Models.Util.GenericItemModel oReturn = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
            {
                ItemId = 8,
                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                {
                    new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 8,
                        Value = "201005",
                        Enable  = true,
                    },
                },
            };

            oReturn = IntegrationPlatform.Controller.IntegrationPlatform.CustomerProvider_Publicar_CustomDataInfo_Upsert(oReturn);

            Assert.AreEqual(true, oReturn != null);
        }

        [TestMethod]
        public void CatalogGetSanofiOptions()
        {
            List<ProveedoresOnLine.Company.Models.Util.CatalogModel> oReturn = new List<ProveedoresOnLine.Company.Models.Util.CatalogModel>();

            oReturn = IntegrationPlatform.Controller.IntegrationPlatform.CatalogGetSanofiOptions();

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }

        [TestMethod]
        public void CustomerProvider_GetCustomData()
        {
            List<ProveedoresOnLine.Company.Models.Company.CompanyModel> RelatedCustomer = new List<ProveedoresOnLine.Company.Models.Company.CompanyModel>(){
                new ProveedoresOnLine.Company.Models.Company.CompanyModel(){
                    CompanyPublicId = "7BC27832",
                },
            };

            List<Models.Integration.CustomDataModel> oReturn = IntegrationPlatform.Controller.IntegrationPlatform.CustomerProvider_GetCustomData(RelatedCustomer, "A24EB150");

            Assert.AreEqual(true, oReturn.Count > 0 && oReturn != null);
        }

        #endregion

        #endregion
    }
}
