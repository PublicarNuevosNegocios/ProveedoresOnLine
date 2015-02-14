using System;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Manager.General;

namespace WebCrawler.Manager
{
    public static class WebCrawlerManager
    {
        public static void WebCrawlerInfo(string ParId, string PublicId)
        {
            Console.WriteLine("Registro del proveedor: " + PublicId + " y ParId: " + ParId + "\n");
            Console.WriteLine("Start Date: " + DateTime.Now.ToString() + "\n");

            ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel();

            List<string> oSettingsList = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_CrawlerTags].Value.
               Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
               ToList();

            //redirect submenu
            foreach (var item in oSettingsList)
            {
                if (item.ToString() == enumMenu.GeneralInfo.ToString())
                {
                    oProvider.RelatedCompany = GeneralInfo(ParId, PublicId);
                }
                else if (item.ToString() == enumMenu.Certifications.ToString())
                {
                    oProvider.RelatedCertification = CrawlerInfo.CertificationInfo.GetCertificationInfo(ParId, PublicId);
                }
                else if (item.ToString() == enumMenu.LegalInfo.ToString())
                {
                    oProvider.RelatedLegal = CrawlerInfo.LegalInfo.GetLegalInfo(ParId, PublicId);
                }
            }

            try
            {
                //Provider upsert
                
                oProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.ProviderUpsert(oProvider);
                Console.WriteLine("Se agregó el proveedor " + oProvider.RelatedCompany.CompanyName + "\n");

                //Relation Provider with Publicar
                SetCompanyProvider(oProvider.RelatedCompany.CompanyPublicId);

                //Update search filters
                ProveedoresOnLine.Company.Controller.Company.CompanySearchFill(oProvider.RelatedCompany.CompanyPublicId);
                ProveedoresOnLine.Company.Controller.Company.CompanyFilterFill(oProvider.RelatedCompany.CompanyPublicId);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Error, " + e.Message + "\n");
            }

            Console.WriteLine("Finalizó el proceso. " + DateTime.Now.ToString() + "\n");
        }

        #region Get Html Document

        public static HtmlDocument GetHtmlDocumnet(string ParId, string Setting)
        {
            HtmlDocument htmlDoc = new HtmlDocument();

            MyWebClient oWebClient = new MyWebClient();

            oWebClient.Headers.Add("Cookie", WebCrawler.Manager.General.InternalSettings.Instance
                                                [Constants.C_Settings_SessionKey].
                                                Value);

            string oParURL = WebCrawler.Manager.General.InternalSettings.Instance
                    ["CrawlerURL_" + Setting].
                    Value.
                    Replace("{{ParProviderId}}", ParId);

            string strHtml = oWebClient.DownloadString(oParURL);

            htmlDoc.LoadHtml(strHtml);

            return htmlDoc;
        }

        #endregion

        #region General Info

        public static ProveedoresOnLine.Company.Models.Company.CompanyModel GeneralInfo(string ParId, string ProviderPublicId)
        {
            //Create model
            ProveedoresOnLine.Company.Models.Company.CompanyModel oCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                {
                    RelatedContact = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>(),
                    CompanyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                    {
                        new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumCompanyInfoType.ProviderPaymentInfo,
                            },
                            Value = Convert.ToString((int)enumCategoryInfoType.NotApplicate),
                        },
                    },
                    CompanyType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                    {
                        ItemId = (int)enumCategoryInfoType.Provider,
                    },
                    IdentificationType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                    {
                        ItemId = (int)enumCategoryInfoType.NIT,
                    },
                    Enable = true,
                };

            HtmlDocument HtmlDoc = GetHtmlDocumnet(ParId, enumMenu.GeneralInfo.ToString());

            if (HtmlDoc.DocumentNode.SelectNodes("//input") != null) // General Info
            {
                Console.WriteLine("\nGeneral Info\n");

                ProveedoresOnLine.Company.Models.Util.GenericItemModel oGeneralInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                    {
                        ItemId = 0,
                        ItemName = string.Empty,
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumContactType.CompanyContact,
                        },
                        Enable = true,
                        ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                    };

                foreach (HtmlNode node in HtmlDoc.DocumentNode.SelectNodes("//input"))
                {
                    HtmlAttribute AttId = node.Attributes["id"];
                    HtmlAttribute AttValue = node.Attributes["value"];
                    if (AttId != null && AttValue != null)
                    {
                        //get provider nit
                        if (AttId.Value == "nit2")//Nit
                        {
                            oCompany.IdentificationNumber = AttValue.Value.ToString();
                        }
                        else if (AttId.Value == "b")//Dígito de Verificación
                        {
                            oCompany.IdentificationNumber += AttValue.Value.ToString();
                        }
                        else if (AttId.Value == "razon")//Razón Social
                        {
                            oCompany.CompanyName = AttValue.Value.ToString();
                        }
                        else if (AttId.Value == "razon2")//Nombre Comercial
                        {
                            oCompany.CompanyInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumCompanyInfoType.ComercialName,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                }
                            );
                        }
                        else if (AttId.Value == "tel0")
                        {
                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_Telephone),
                                    Enable = true,
                                }
                            };

                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                }
                            };
                        }
                        else if (AttId.Value == "postal")
                        {
                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_PostalCode),
                                    Enable = true,
                                }
                            };

                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                }
                            };
                        }
                        else if (AttId.Value == "tel")
                        {
                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_Cellphone),
                                    Enable = true,
                                }
                            };

                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                }
                            };
                        }
                        else if (AttId.Value == "fax")
                        {
                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_Fax),
                                    Enable = true,
                                }
                            };

                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                }
                            };
                        }
                        else if (AttId.Value == "telefono_3")
                        {
                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_Telephone),
                                    Enable = true,
                                }
                            };

                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                }
                            };
                        }
                        else if (AttId.Value == "web")
                        {
                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                   ItemInfoId = 0,
                                   ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                   {
                                       ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                   },
                                   Value = Convert.ToString((int)enumCategoryInfoType.CC_WebPage),
                                   Enable = true,
                               }
                            };

                            oGeneralInfo.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                }
                            };
                        }
                    }
                }

                //Add General Info
                oCompany.RelatedContact.Add(oGeneralInfo);
            }
            else
            {
                Console.WriteLine("la sección " + enumMenu.GeneralInfo.ToString() + " no tiene información para descargar." + "\n");
            }

            if (HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']/tr") != null)
            {
                HtmlNodeCollection tables = HtmlDoc.DocumentNode.SelectNodes("//table[@class='administrador_tabla_generales']");

                Console.WriteLine("\nContact Info\n");

                HtmlNodeCollection rowsTable1 = tables[1].SelectNodes(".//tr");
                if (rowsTable1 != null)
                {
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oContactInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                    {
                        ItemId = 0,
                        ItemName = string.Empty,
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumContactType.PersonContact,
                        },
                        Enable = true,
                        ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                    };

                    for (int i = 1; i < rowsTable1.Count; i++)
                    {
                        HtmlNodeCollection cols = rowsTable1[i].SelectNodes(".//td");

                        //Get person contact type
                        ProveedoresOnLine.Company.Models.Util.CatalogModel oPersonContactInfo = Util.ProviderOptions_GetByName(210, cols[3].InnerText.ToString());

                        if (oPersonContactInfo != null)
                        {
                            oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.CP_PersonContactType,
                                },
                                Value = oPersonContactInfo.ItemId.ToString(),
                                Enable = true,
                            });
                        }

                        //Get document type
                        ProveedoresOnLine.Company.Models.Util.CatalogModel oDocumentType = Util.ProviderOptions_GetByName(101, cols[2].InnerText.ToString());

                        if (oDocumentType != null)
                        {
                            oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.CP_IdentificationType,
                                },
                                Value = oDocumentType.ItemId.ToString(),
                                Enable = true,
                            });
                        }

                        oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.CP_IdentificationNumber,
                                },
                                Value = cols[0].InnerText.ToString(),
                                Enable = true,
                            });

                        oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.CP_IdentificationCity,
                                },
                                Value = cols[5].InnerText.ToString(),
                                Enable = true,
                            });

                        if (cols[9].InnerHtml.Contains("href"))
                        {
                            if (cols[9].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                            {
                                cols[9].ChildNodes["a"].Attributes["href"].Value = cols[9].ChildNodes["a"].Attributes["href"].Value.Replace("../", "https://www.parservicios.com/parservi/procesos/");
                            }

                            Console.WriteLine("Documento " + cols[9].ChildNodes["a"].Attributes["href"].Value);
                        }

                        oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.CP_Phone,
                                },
                                Value = cols[4].InnerText.ToString(),
                                Enable = true,
                            });

                        oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.CP_Email,
                                },
                                Value = cols[8].InnerText.ToString(),
                                Enable = true,
                            });

                        oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.CP_Negotiation,
                                },
                                Value = cols[6].InnerText.ToString(),
                                Enable = true,
                            });

                        oContactInfo.ItemName = cols[1].InnerText.ToString();
                    }

                    //Add Conctact Person Info
                    oCompany.RelatedContact.Add(oContactInfo);
                }

                Console.WriteLine("\nLocations Info\n");

                HtmlNodeCollection rowsTable2 = tables[2].SelectNodes(".//tr");

                if (rowsTable2 != null)
                {
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oLocationsInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                    {
                        ItemId = 0,
                        ItemName = string.Empty,
                        ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = (int)enumContactType.Brach,
                        },
                        Enable = true,
                        ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                    };

                    for (int i = 1; i < rowsTable2.Count; i++)
                    {
                        HtmlNodeCollection cols = rowsTable2[i].SelectNodes(".//td");

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.BR_Representative,
                                },
                                Value = cols[5].InnerText.ToString(),
                                Enable = true,
                            });

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.BR_Email,
                                },
                                Value = cols[1].InnerText.ToString(),
                                Enable = true,
                            });

                        //Get city
                        ProveedoresOnLine.Company.Models.Util.GeographyModel oGeograghy = Util.Geography_GetByName(cols[2].InnerText.ToString());

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumContactInfoType.BR_City,
                            },
                            Value = oGeograghy.City.ItemId.ToString(),
                            Enable = true,
                        });


                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumContactInfoType.BR_Phone,
                            },
                            Value = cols[3].InnerText.ToString(),
                            Enable = true,
                        });

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumContactInfoType.BR_Fax,
                            },
                            Value = cols[4].InnerText.ToString(),
                            Enable = true,
                        });

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumContactInfoType.BR_Email,
                            },
                            Value = cols[6].InnerText.ToString(),
                            Enable = true,
                        });

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumContactInfoType.BR_Website,
                            },
                            Value = cols[7].InnerText.ToString(),
                            Enable = true,
                        });
                    }

                    //Add Location Info
                    oCompany.RelatedContact.Add(oLocationsInfo);
                }
            }
            else
            {
                Console.WriteLine("la sección " + enumMenu.GeneralInfo.ToString() + " no tiene información para descargar." + "\n");
            }

            return oCompany;
        }

        #endregion

        #region Commercial Info



        #endregion

        #region Finantial Info

        #endregion

        #region BalanceSheet Info

        #endregion

        #region Upload File

        public static string UploadFile(
            string ProviderPublicId,
            string UrlFile,
            string UrlNewFile)
        {
            string strRemoteFile = string.Empty;

            string strFile = string.Empty;

            strFile = UrlNewFile +
                "\\ProviderFile_" +
                ProviderPublicId + "_" +
                "0" + "_" +
                DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            File.Copy(UrlFile, strFile);

            //load file to s3
            strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                        (strFile,
                        WebCrawler.Manager.General.InternalSettings.Instance
                        [WebCrawler.Manager.General.Constants.C_Settings_File_RemoteDirectoryProvider].Value +
                            ProviderPublicId + "\\");

            File.Delete(strFile);

            return strRemoteFile;
        }

        #endregion

        #region Asociate to Customer Publicar

        public static void SetCompanyProvider(string ProviderId)
        {
            //Create Provider By Customer Publicar
            ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel oCustomerModel = new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel();
            oCustomerModel.RelatedProvider = new List<ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerProviderModel>();

            oCustomerModel.RelatedProvider.Add(new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerProviderModel()
            {
                RelatedProvider = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = ProviderId,
                },
                Status = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = Convert.ToInt32(enumProviderCustomerStatus.Creation),
                },
                Enable = true,
            });

            oCustomerModel.RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(WebCrawler.Manager.General.InternalSettings.Instance[WebCrawler.Manager.General.Constants.C_Settings_PublicarPublicId].Value);

            ProveedoresOnLine.CompanyCustomer.Controller.Customer.CustomerProviderUpsert(oCustomerModel);
        }

        #endregion

        public class MyWebClient : System.Net.WebClient
        {
            protected override System.Net.WebRequest GetWebRequest(Uri uri)
            {
                System.Net.WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 20 * 60 * 1000;
                return w;
            }
        }
    }
}
