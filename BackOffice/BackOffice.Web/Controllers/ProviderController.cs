using BackOffice.Models.General;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class ProviderController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        #region General Info

        public virtual ActionResult GIProviderUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            //eval upsert action
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                //get provider request info
                ProveedoresOnLine.Company.Models.Company.CompanyModel CompanyToUpsert = GetProviderRequest();

                //upsert provider
                CompanyToUpsert = ProveedoresOnLine.Company.Controller.Company.CompanyUpsert(CompanyToUpsert);

                //eval redirect url
                if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "next" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.NextMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.NextMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.NextMenu.Url);
                }
                else if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                    Request["StepAction"].ToLower().Trim() == "last" &&
                    oModel.CurrentSubMenu != null &&
                    oModel.CurrentSubMenu.LastMenu != null &&
                    !string.IsNullOrEmpty(oModel.CurrentSubMenu.LastMenu.Url))
                {
                    return Redirect(oModel.CurrentSubMenu.LastMenu.Url);
                }
                else
                {
                    return RedirectToAction(MVC.Provider.ActionNames.GIProviderUpsert, MVC.Provider.Name, new { ProviderPublicId = CompanyToUpsert.CompanyPublicId });
                }
            }

            return View(oModel);
        }

        public virtual ActionResult GICompanyContactUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult GIPersonContactUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult GIBranchUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult GIDistributorUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

            //get provider menu
            oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        #region Private methods

        private ProveedoresOnLine.Company.Models.Company.CompanyModel GetProviderRequest()
        {
            //get company
            ProveedoresOnLine.Company.Models.Company.CompanyModel oReturn = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
            {
                CompanyPublicId = Request["ProviderPublicId"],
                IdentificationType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = Convert.ToInt32(Request["IdentificationType"]),
                },
                IdentificationNumber = Request["IdentificationNumber"],
                CompanyName = Request["CompanyName"],
                CompanyType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = Convert.ToInt32(Request["CompanyType"]),
                },
                Enable = !string.IsNullOrEmpty(Request["Enable"]),
                CompanyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            };

            //get company info
            Request.Form.AllKeys.Where(x => x.Contains("CompanyInfoType_")).All(req =>
            {
                string[] strSplit = req.Split('_');

                if (strSplit.Length >= 3)
                {
                    oReturn.CompanyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = !string.IsNullOrEmpty(strSplit[2]) ? Convert.ToInt32(strSplit[2].Trim()) : 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = Convert.ToInt32(strSplit[1].Trim())
                        },
                        Value = Request[req],
                        Enable = true,
                    });
                }
                return true;
            });

            return oReturn;
        }

        private GenericItemModel GetChaimberOfCommerceInfoRequest()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
               && bool.Parse(Request["UpsertAction"]))
                {
                    //get ChaimberInfo                
                    GenericItemModel RelatedLegal = new GenericItemModel
                    {
                        ItemId = Convert.ToInt32(Request["NameInfoId"]),
                        ItemType = new CatalogModel()
                        {
                            ItemId = Convert.ToInt32(enumLegalType.ChaimberOfCommerce),
                        },
                        ItemName = Request["ChaimberName"],
                        Enable = Request["Enable"] == "true" ? true : false,
   
                        ItemInfo = new List<GenericItemInfoModel>
                        {   
                            new GenericItemInfoModel()
                            {
                                ItemInfoId = int.Parse(Request["ConstitutionDateId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_ConstitutionDate
                                },
                                Value = Request["ConstitutionDate"]
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =  Convert.ToInt32(Request["ValidityDateId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_ConstitutionEndDate
                                },
                                Value = Request["ValidityDate"]
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =  Convert.ToInt32(Request["SelectedCityId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_InscriptionCity
                                },
                                Value = Request["SelectedCity"]
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =  Convert.ToInt32(Request["InscriptionNumberId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_InscriptionNumber
                                },
                                Value = Request["InscriptionNumber"]
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =  Convert.ToInt32(Request["CertificateURLId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_ExistenceAndLegalPersonCertificate
                                },
                                Value = Request["CertificateURL"]
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =  Convert.ToInt32(Request["ExpeditionCertificatedDateId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_CertificateExpeditionDate
                                },
                                Value = Request["ExpeditionCertificatedDate"]
                            },
                            new GenericItemInfoModel()
                            {
                                ItemInfoId =   Convert.ToInt32(Request["SocialObjectId"]),
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumLegalInfoType.CP_SocialObject
                                },
                                Value = Request["SocialObject"]
                            },
                        }
                    };
                    return RelatedLegal;
                }
            return null;
        }
        #endregion

        #endregion

        #region Commercial Info

        public virtual ActionResult CIExperiencesUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                },
            };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);

            return View(oModel);
        }

        #endregion

        #region HSEQ Info

        public virtual ActionResult HICertificationsUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertficationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.Certifications),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
            //return View(new Models.Provider.ProviderViewModel());
        }

        public virtual ActionResult HIHealtyPoliticUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertficationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.CompanyHealtyPolitic),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
            //return View(new Models.Provider.ProviderViewModel());
        }

        public virtual ActionResult HIRiskPoliciesUpsert(string ProviderPublicId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CertficationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.CompanyRiskPolicies),
                };

                //get provider menu
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
            //return View(new Models.Provider.ProviderViewModel());
        }

        #endregion

        #region Finantial Info
        #endregion

        #region Legal Info

        public virtual ActionResult LIChaimberOfCommerceUpsert(string ProviderPublicId)
        {
            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                oModel.RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId),
                    RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.ChaimberOfCommerce),

                };
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            //eval upsert action
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                ProviderModel ProviderToUpsert = new ProviderModel();
                ProviderToUpsert.RelatedLegal = new List<GenericItemModel>();
                ProviderToUpsert.RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel();

                ProviderToUpsert.RelatedCompany.CompanyPublicId = ProviderPublicId;               

                //get request Info
                ProviderToUpsert.RelatedLegal.Add(this.GetChaimberOfCommerceInfoRequest());

                //upsert provider
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalUpsert(ProviderToUpsert);               

                ////eval redirect url
                //if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                //    Request["StepAction"].ToLower().Trim() == "next" &&
                //    oModel.CurrentSubMenu != null &&
                //    oModel.CurrentSubMenu.NextMenu != null &&
                //    !string.IsNullOrEmpty(oModel.CurrentSubMenu.NextMenu.Url))
                //{
                //    return Redirect(oModel.CurrentSubMenu.NextMenu.Url);
                //}
                //else if (!string.IsNullOrEmpty(Request["StepAction"]) &&
                //    Request["StepAction"].ToLower().Trim() == "last" &&
                //    oModel.CurrentSubMenu != null &&
                //    oModel.CurrentSubMenu.LastMenu != null &&
                //    !string.IsNullOrEmpty(oModel.CurrentSubMenu.LastMenu.Url))
                //{
                //    return Redirect(oModel.CurrentSubMenu.LastMenu.Url);
                //}
                //else
                //{
                //    return RedirectToAction(MVC.Provider.ActionNames.GIProviderUpsert, MVC.Provider.Name, new { ProviderPublicId = CompanyToUpsert.CompanyPublicId });
                //}
            }

            return View(oModel);
        }

        #endregion

        #region Menu

        private List<BackOffice.Models.General.GenericMenu> GetProviderMenu
            (BackOffice.Models.Provider.ProviderViewModel vProviderInfo)
        {
            List<BackOffice.Models.General.GenericMenu> oReturn = new List<Models.General.GenericMenu>();

            if (vProviderInfo.RelatedProvider != null &&
                vProviderInfo.RelatedProvider.RelatedCompany != null &&
                !string.IsNullOrEmpty(vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId))
            {
                //get current controller action
                string oCurrentController = BackOffice.Web.Controllers.BaseController.CurrentControllerName;
                string oCurrentAction = BackOffice.Web.Controllers.BaseController.CurrentActionName;

                #region General Info

                //header
                BackOffice.Models.General.GenericMenu oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información general",
                    Position = 0,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Basic info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información básica",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company contact info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información de contacto de empresa",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GICompanyContactUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GICompanyContactUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Person contact info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información de personas de contacto",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIPersonContactUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIPersonContactUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });


                //Branch info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Sucursales",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIBranchUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIBranchUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Distributor info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Distribuidores",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIDistributorUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 4,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIDistributorUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Commercial Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información comercial",
                    Position = 1,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Experience
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Experiencias",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.CIExperiencesUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.CIExperiencesUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region HSEQ Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "HSEQ",
                    Position = 2,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Certifications
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Certificaciones",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.HICertificationsUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.HICertificationsUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company healty politic
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Salud, medio ambiente y seguridad",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.HIHealtyPoliticUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.HIHealtyPoliticUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company risk policies
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Sistema de riesgos laborales",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.HIRiskPoliciesUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.HIRiskPoliciesUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Financial Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información financiera",
                    Position = 3,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Balancesheet info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Balances financieros",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //tax info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Impuestos",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //money laundering
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Lavado de activos",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //income statement
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Declaración de renta",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //bank info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información bancaria",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 4,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Legal Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información Legal",
                    Position = 4,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //chaimber of commerce
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Camara y comercio",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.LIChaimberOfCommerceUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LIChaimberOfCommerceUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //RUT
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Registro unico tributario",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //CIFIN
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "CIFIN",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //SARLAFT
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "SARLAFT",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Resolution
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Resoluciones",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderUpsert,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 4,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderUpsert &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region last next menu

                BackOffice.Models.General.GenericMenu MenuAux = null;

                oReturn.OrderBy(x => x.Position).All(pm =>
                {
                    pm.ChildMenu.OrderBy(x => x.Position).All(sm =>
                    {
                        if (MenuAux != null)
                        {
                            MenuAux.NextMenu = sm;
                        }
                        sm.LastMenu = MenuAux;
                        MenuAux = sm;

                        return true;
                    });

                    return true;
                });

                #endregion
            }


            return oReturn;
        }

        #endregion
    }
}