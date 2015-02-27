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
                    if (oProvider.RelatedCertification == null)
                    {
                        oProvider.RelatedCertification = CrawlerInfo.CertificationInfo.GetCertificationInfo(ParId, oProvider.RelatedCompany == null ? PublicId : oProvider.RelatedCompany.CompanyPublicId);
                    }
                    else
                    {
                        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCertifications = CrawlerInfo.CertificationInfo.GetCertificationInfo(ParId, oProvider.RelatedCompany == null ? PublicId : oProvider.RelatedCompany.CompanyPublicId);

                        oCertifications.All(x =>
                        {
                            oProvider.RelatedCertification.Add(x);
                            return true;
                        });
                    }
                }
                else if (item.ToString() == enumMenu.HSE.ToString())
                {
                    if (oProvider.RelatedCertification == null)
                    {
                        oProvider.RelatedCertification = CrawlerInfo.HSEQInfo.GetHSEQInfo(ParId, oProvider.RelatedCompany == null ? PublicId : oProvider.RelatedCompany.CompanyPublicId);
                    }
                    else
                    {
                        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCertifications = CrawlerInfo.HSEQInfo.GetHSEQInfo(ParId, oProvider.RelatedCompany == null ? PublicId : oProvider.RelatedCompany.CompanyPublicId);

                        oCertifications.All(x =>
                            {
                                oProvider.RelatedCertification.Add(x);
                                return true;
                            });
                    }
                }
                else if (item.ToString() == enumMenu.Finantial.ToString())
                {
                    oProvider.RelatedFinantial = CrawlerInfo.BalancesInfo.GetFinantialInfo(ParId, oProvider.RelatedCompany == null ? PublicId : oProvider.RelatedCompany.CompanyPublicId);
                }
                else if (item.ToString() == enumMenu.LegalInfo.ToString())
                {
                    oProvider.RelatedLegal = CrawlerInfo.LegalInfo.GetLegalInfo(ParId, oProvider.RelatedCompany == null ? PublicId : oProvider.RelatedCompany.CompanyPublicId);
                }
                else if (item.ToString() == enumMenu.Experience.ToString())
                {
                    oProvider.RelatedCommercial = WebCrawler.Manager.CrawlerInfo.ExperiencesInfo.GetExperiencesInfo(ParId, oProvider.RelatedCompany == null ? PublicId : oProvider.RelatedCompany.CompanyPublicId);
                }
                else if (item.ToString() == enumMenu.GeneralBalance.ToString())
                {
                    WebCrawler.Manager.CrawlerInfo.BalancesInfo.GetBalancesInfo(ParId, oProvider.RelatedCompany == null ? PublicId : oProvider.RelatedCompany.CompanyPublicId);
                }
            }

            try
            {
                if (oProvider.RelatedCompany == null)
                {
                    oProvider.RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(PublicId);
                }

                //Provider upsert
                oProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.ProviderUpsert(oProvider);
                Console.WriteLine("\nSe agregó el proveedor " + oProvider.RelatedCompany.CompanyName + "\n");

                //Relation Provider with Publicar
                SetCompanyProvider(oProvider.RelatedCompany.CompanyPublicId);

            }
            catch (System.Exception e)
            {
                Console.WriteLine("\nError, " + e.Message + "\n");
            }


            //Update search filters
            ProveedoresOnLine.Company.Controller.Company.CompanySearchFill(oProvider.RelatedCompany.CompanyPublicId);
            ProveedoresOnLine.Company.Controller.Company.CompanyFilterFill(oProvider.RelatedCompany.CompanyPublicId);


            Console.WriteLine("\nFinalizó el proceso. " + DateTime.Now.ToString() + "\n");
            Console.WriteLine("\n-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-\n");
        }

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

                ProveedoresOnLine.Company.Models.Util.GenericItemModel oGeneralInfo = null;

                foreach (HtmlNode node in HtmlDoc.DocumentNode.SelectNodes("//input"))
                {
                    oGeneralInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
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
                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_Telephone),
                                    Enable = true,
                                });

                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                });
                        }
                        else if (AttId.Value == "postal")
                        {
                            oGeneralInfo.ItemInfo.Add(
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
                            );

                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                });
                        }
                        else if (AttId.Value == "tel")
                        {
                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_Cellphone),
                                    Enable = true,
                                });

                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                });
                        }
                        else if (AttId.Value == "fax")
                        {
                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_Fax),
                                    Enable = true,
                                });

                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                });
                        }
                        else if (AttId.Value == "telefono_3")
                        {
                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_Telephone),
                                    Enable = true,
                                });

                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                });
                        }
                        else if (AttId.Value == "web")
                        {
                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_WebPage),
                                    Enable = true,
                                });

                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = AttValue.Value.ToString(),
                                    Enable = true,
                                });
                        }
                    }
                    //Add Contact Info
                    if (oGeneralInfo.ItemInfo != null && oGeneralInfo.ItemInfo.Count > 0)
                    {
                        oCompany.RelatedContact.Add(oGeneralInfo);
                    }
                }

                //Upsert Provider
                oCompany = ProveedoresOnLine.Company.Controller.Company.CompanyUpsert(oCompany);
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
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oContactInfo = null;

                    for (int i = 1; i < rowsTable1.Count; i++)
                    {
                        oContactInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
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

                        HtmlNodeCollection cols = rowsTable1[i].SelectNodes(".//td");

                        if (cols[3].InnerText.ToString() == "E-mail Empresarial")
                        {
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

                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_CompanyContactType,
                                    },
                                    Value = Convert.ToString((int)enumCategoryInfoType.CC_Email),
                                    Enable = true,
                                });

                            oGeneralInfo.ItemInfo.Add(
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CC_Value,
                                    },
                                    Value = cols[8].InnerText.ToString() != "&nbsp;" ? cols[8].InnerText.ToString() : string.Empty,
                                    Enable = true,
                                });

                            if (oGeneralInfo.ItemInfo != null && oGeneralInfo.ItemInfo.Count > 0)
                            {
                                oCompany.RelatedContact.Add(oGeneralInfo);
                            }
                        }
                        else
                        {
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
                                    Value = cols[0].InnerText.ToString() != "&nbsp;" ? cols[0].InnerText.ToString() : string.Empty,
                                    Enable = true,
                                });

                            oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CP_IdentificationCity,
                                    },
                                    Value = cols[5].InnerText.ToString() != "&nbsp;" ? cols[5].InnerText.ToString() : string.Empty,
                                    Enable = true,
                                });

                            if (cols[9].InnerHtml.Contains("href"))
                            {
                                string urlDownload = WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_UrlDownload].Value;
                                string urlS3 = string.Empty;

                                if (cols[9].ChildNodes["a"].Attributes["href"].Value.Contains("../"))
                                {
                                    urlDownload = cols[9].ChildNodes["a"].Attributes["href"].Value.Replace("..", urlDownload);
                                }

                                urlS3 = UploadFile(urlDownload, enumContactType.PersonContact.ToString(), oCompany.CompanyPublicId);

                                oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CP_IdentificationFile,
                                    },
                                    Value = urlS3,
                                    Enable = true,
                                });
                            }

                            oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CP_Phone,
                                    },
                                    Value = cols[4].InnerText.ToString() != "&nbsp;" ? cols[4].InnerText.ToString() : string.Empty,
                                    Enable = true,
                                });

                            oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CP_Email,
                                    },
                                    Value = cols[8].InnerText.ToString() != "&nbsp;" ? cols[8].InnerText.ToString() : string.Empty,
                                    Enable = true,
                                });

                            oContactInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumContactInfoType.CP_Negotiation,
                                    },
                                    Value = cols[6].InnerText.ToString() != "&nbsp;" ? cols[6].InnerText.ToString() : string.Empty,
                                    Enable = true,
                                });

                            oContactInfo.ItemName = cols[1].InnerText.ToString() != "&nbsp;" ? cols[1].InnerText.ToString() : string.Empty;
                        }

                        //Add Conctact Person Info
                        if (oContactInfo.ItemInfo != null && oContactInfo.ItemInfo.Count > 0)
                        {
                            oCompany.RelatedContact.Add(oContactInfo);
                        }
                    }
                }

                Console.WriteLine("\nLocations Info\n");

                HtmlNodeCollection rowsTable2 = tables[2].SelectNodes(".//tr");

                if (rowsTable2 != null)
                {
                    ProveedoresOnLine.Company.Models.Util.GenericItemModel oLocationsInfo = null;

                    for (int i = 1; i < rowsTable2.Count; i++)
                    {
                        oLocationsInfo = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
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

                        HtmlNodeCollection cols = rowsTable2[i].SelectNodes(".//td");

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.BR_Representative,
                                },
                                Value = cols[5].InnerText.ToString() != "&nbsp;" ? cols[5].InnerText.ToString() : string.Empty,
                                Enable = true,
                            });

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumContactInfoType.BR_Address,
                                },
                                Value = cols[1].InnerText.ToString() != "&nbsp;" ? cols[1].InnerText.ToString() : string.Empty,
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
                            Value = cols[3].InnerText.ToString() != "&nbsp;" ? cols[3].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumContactInfoType.BR_Fax,
                            },
                            Value = cols[4].InnerText.ToString() != "&nbsp;" ? cols[4].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumContactInfoType.BR_Email,
                            },
                            Value = cols[6].InnerText.ToString() != "&nbsp;" ? cols[6].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        oLocationsInfo.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumContactInfoType.BR_Website,
                            },
                            Value = cols[7].InnerText.ToString() != "&nbsp;" ? cols[7].InnerText.ToString() : string.Empty,
                            Enable = true,
                        });

                        //Add Location Info
                        if (oLocationsInfo.ItemInfo != null && oLocationsInfo.ItemInfo.Count > 0)
                        {
                            oCompany.RelatedContact.Add(oLocationsInfo);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("la sección " + enumMenu.GeneralInfo.ToString() + " no tiene información para descargar." + "\n");
            }

            return oCompany;
        }

        #endregion

        #region Get Html Document

        public static MyWebClient oWebClient = new MyWebClient();

        public static HtmlDocument GetHtmlDocumnet(string ParId, string Setting)
        {
            if (!oWebClient.Headers.AllKeys.Contains("Cookie"))
            {
                oWebClient.Headers.Add("Cookie", WebCrawler.Manager.General.InternalSettings.Instance
                                                [Constants.C_Settings_SessionKey].
                                                Value);
            }

            string oParURL = WebCrawler.Manager.General.InternalSettings.Instance
                    ["CrawlerURL_" + Setting].
                    Value.
                    Replace("{{ParProviderId}}", ParId);

            string strHtml = oWebClient.DownloadString(oParURL);

            HtmlDocument htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(strHtml);

            return htmlDoc;
        }

        #endregion

        #region Upload File

        public static string UploadFile(
            string UrlFile,
            string Setting,
            string ProviderPublicId)
        {
            string strRemoteFile = string.Empty;
            string folderSave = string.Empty;

            //Create folder save
            folderSave = WebCrawler.Manager.General.InternalSettings.Instance
                                                                [WebCrawler.Manager.General.Constants.C_Settings_FolderSave].
                                                                Value
                                                                + "\\"
                                                                + ProviderPublicId
                                                                + "\\"
                                                                + Setting;

            if (!File.Exists(folderSave))
            {
                System.IO.Directory.CreateDirectory(folderSave);
            }

            //Get file name
            string[] files = UrlFile.Split(new char[] { '&' });
            string fileName = files[1].ToString();

            fileName = fileName.Substring(fileName.IndexOf("=") + 1, fileName.Length - fileName.IndexOf("=") - 1).Replace(@"\", @"").Replace('"', ' ');
            try
            {
                //Download file
                Console.WriteLine("Start download file -> " + DateTime.Now.ToString("dd/MM/yy HH:mm:ss"));
                oWebClient.DownloadFile(UrlFile, folderSave + "//" + fileName);
                Console.WriteLine("End download file -> " + DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + "\n");

                //Upload S3
                string strFile = folderSave.TrimEnd('\\') +
                               "\\CompanyFile_" +
                               ProviderPublicId + "_" +
                               DateTime.Now.ToString("yyyyMMddHHmmss") + "." +
                               fileName.Split('.').DefaultIfEmpty("pdf").LastOrDefault();

                File.Copy(folderSave + "//" + fileName, strFile);

                //load file to s3
                strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                    (strFile,
                    WebCrawler.Manager.General.InternalSettings.Instance
                    [Constants.C_Settings_File_RemoteDirectory].Value.TrimEnd('\\') +
                        "\\CompanyFile\\" + ProviderPublicId + "\\");

                //remove temporal file
                if (System.IO.File.Exists(strFile))
                    System.IO.File.Delete(strFile);


                //Timer Sleep
                System.Threading.Thread.Sleep(
                                        Convert.ToInt32(WebCrawler.Manager.General.InternalSettings.Instance
                                        [WebCrawler.Manager.General.Constants.C_Settings_TimerSleep].Value));
            }
            catch (System.Exception e)
            {
                Console.WriteLine("\nError al descargar el archivo ::" + UrlFile + "::" + e.Message + "\n");
                strRemoteFile = string.Empty;
            }

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
