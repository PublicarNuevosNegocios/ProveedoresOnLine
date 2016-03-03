using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BackOffice.Models.General;
using BackOffice.Models.Provider;
using LinqToExcel;
using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;

namespace BackOffice.Web.Controllers
{
    public partial class AdministratorController : BaseController
    {
        public virtual ActionResult Index()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            //get provider menu
            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminUserUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            //get provider menu
            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminGeoUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            //get provider menu
            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminBankUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            //get provider menu
            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminCompanyRulesUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminRulesUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminResolutionUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminEcoActivityUpsert(int TreeId, string TreeName)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminEcoGroupUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);

            return View(oModel);
        }

        public virtual ActionResult AdminTreeUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);
            return View(oModel);
        }

        public virtual ActionResult AdminTRMUpsert()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);
            return View(oModel);
        }

        public virtual ActionResult AdminRLUploadProvider(HttpPostedFileBase ExcelFile)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            if (ExcelFile != null)
            {
                string strFolder = System.Web.HttpContext.Current.Server.MapPath
                    (BackOffice.Models.General.InternalSettings.Instance
                    [BackOffice.Models.General.Constants.C_Settings_File_TempDirectory].Value);

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                //get File
                string strFile = strFolder.TrimEnd('\\') +
                    DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                string ErrorFilePath = strFile.Replace(".xls", "_log.csv");
                ExcelFile.SaveAs(strFile);

                //load file to s3
                string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                    (strFile,
                    BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_File_ExcelDirectory].Value);

                //update file into db
                string logFile = this.ProcessProviderFile(strFile, ErrorFilePath, strRemoteFile, ExcelFile.FileName);

                //remove temporal file
                if (System.IO.File.Exists(strFile))
                    System.IO.File.Delete(strFile);

                //ViewData.Add(strRemoteFile);
                List<string> urlList = new List<string>();
                urlList.Add(strRemoteFile);
                urlList.Add(logFile);

                ViewData["UrlReturn"] = urlList;
            }
            else
            {
                ViewData["UrlReturn"] = "No has seleccionado ningún archivo";
            }

            oModel.ProviderMenu = GetAdminMenu(oModel);
            return View(oModel);
        }

        public virtual ActionResult AdminRole()
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);
            return View(oModel);
        }

        public virtual ActionResult AdminModule(int RoleCompanyId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedRole = ProveedoresOnLine.Company.Controller.Company.GetRoleCompanyByRoleCompanyId(RoleCompanyId),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);
            return View(oModel);
        }

        public virtual ActionResult AdminProviderMenu(int RoleCompanyId, int RoleModuleId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedRole = new ProveedoresOnLine.Company.Models.Role.RoleCompanyModel()
                {
                    RoleCompanyId = RoleCompanyId,
                    RoleModule = new List<ProveedoresOnLine.Company.Models.Role.RoleModuleModel>()
                    {
                        new ProveedoresOnLine.Company.Models.Role.RoleModuleModel()
                        {
                            RoleModuleId = RoleModuleId,
                        },
                    },
                },
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);
            return View(oModel);
        }

        public virtual ActionResult AdminSelectionOption(int RoleCompanyId, int RoleModuleId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedRole = new ProveedoresOnLine.Company.Models.Role.RoleCompanyModel()
                {
                    RoleCompanyId = RoleCompanyId,
                    RoleModule = new List<ProveedoresOnLine.Company.Models.Role.RoleModuleModel>()
                    {
                        new ProveedoresOnLine.Company.Models.Role.RoleModuleModel()
                        {
                            RoleModuleId = RoleModuleId,
                        },
                    },
                },
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);
            return View(oModel);
        }

        public virtual ActionResult AdminReports(int RoleCompanyId)
        {
            BackOffice.Models.Provider.ProviderViewModel oModel = new ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                RelatedRole = ProveedoresOnLine.Company.Controller.Company.GetRoleCompanyByRoleCompanyId(RoleCompanyId),
            };

            oModel.ProviderMenu = GetAdminMenu(oModel);
            return View(oModel);
        }

        #region Menu

        private List<BackOffice.Models.General.GenericMenu> GetAdminMenu
            (BackOffice.Models.Provider.ProviderViewModel vAdminInfo)
        {
            List<BackOffice.Models.General.GenericMenu> oReturn = new List<Models.General.GenericMenu>();

            //get current controller action
            string oCurrentController = BackOffice.Web.Controllers.BaseController.CurrentControllerName;
            string oCurrentAction = BackOffice.Web.Controllers.BaseController.CurrentActionName;

            #region Administration

            //header
            BackOffice.Models.General.GenericMenu oMenuAux = new Models.General.GenericMenu()
            {
                Name = "Administración",
                Position = 0,
                ChildMenu = new List<Models.General.GenericMenu>(),
            };

            //Users
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Usuarios",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminUserUpsert,
                    MVC.Administrator.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminUserUpsert &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //Geolocalization
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Geolocalización",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminGeoUpsert,
                    MVC.Administrator.Name),
                Position = 2,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminGeoUpsert &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //Standart Economy Activity standar
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Maestras Estandar",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminEcoActivityUpsert,
                    MVC.Administrator.Name, new { TreeId = 0, TreeName = "" }),
                Position = 3,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminEcoActivityUpsert &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //Arboles
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Maestras Personalizadas",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminTreeUpsert,
                    MVC.Administrator.Name),
                Position = 4,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminTreeUpsert &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //Stantart Group
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Grupos",
                Url = Url.Action
                            (MVC.Administrator.ActionNames.AdminEcoGroupUpsert,
                            MVC.Administrator.Name),
                Position = 5,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminEcoGroupUpsert &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //Banks
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Bancos",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminBankUpsert,
                    MVC.Administrator.Name),
                Position = 6,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminBankUpsert &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //Certificactions Companies
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Empresas Certificadoras",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminCompanyRulesUpsert,
                    MVC.Administrator.Name),
                Position = 7,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminCompanyRulesUpsert &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //Certificados
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Certificados",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminRulesUpsert,
                    MVC.Administrator.Name),
                Position = 8,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminRulesUpsert &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //Resoluciones
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Resoluciones",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminResolutionUpsert,
                    MVC.Administrator.Name),
                Position = 9,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminResolutionUpsert &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //TRM
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "TRM",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminTRMUpsert,
                    MVC.Administrator.Name),
                Position = 10,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminTRMUpsert &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //add menu
            oReturn.Add(oMenuAux);

            #endregion Administration

            #region Restrictive List

            //header
            oMenuAux = new Models.General.GenericMenu()
            {
                Name = "Listas Restrictivas",
                Position = 1,
                ChildMenu = new List<Models.General.GenericMenu>(),
            };

            //UpLoad File
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Carga de Provedores",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminRLUploadProvider,
                    MVC.Administrator.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminRLUploadProvider &&
                    oCurrentController == MVC.Administrator.Name),
            });

            //get is selected menu
            oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

            //add menu
            oReturn.Add(oMenuAux);

            #endregion Restrictive List

            #region Admin Role

            //header
            oMenuAux = new Models.General.GenericMenu()
            {
                Name = "Administración de Roles",
                Position = 2,
                ChildMenu = new List<Models.General.GenericMenu>(),
            };

            //Admin Role
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Administrar Roles",
                Url = Url.Action
                    (MVC.Administrator.ActionNames.AdminRole,
                    MVC.Administrator.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Administrator.ActionNames.AdminRole &&
                    oCurrentController == MVC.Administrator.Name),
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

            #endregion last next menu

            return oReturn;
        }

        #endregion Menu

        #region Private Functions

        private string ProcessProviderFile(string FilePath, string ErrorFilePath, string StrRemoteFile, string FileName)
        {
            var excel = new ExcelQueryFactory(FilePath);

            //get excel rows
            LinqToExcel.ExcelQueryFactory XlsInfo = new LinqToExcel.ExcelQueryFactory(FilePath);

            List<ProviderExcelModel> oPrvToProcess =
                (from x in XlsInfo.Worksheet<ProviderExcelModel>(0)
                 select x).ToList();

            List<ProviderExcelResultModel> oPrvToProcessResult = new List<ProviderExcelResultModel>();
            string ActualProvider = "";
            List<string> ProvidersId = new List<string>();

            FileName = FileName.Replace(Path.GetExtension(FileName), "");
            var page = excel.GetWorksheetNames();
            List<string> Columns = excel.GetColumnNames(page.FirstOrDefault()).ToList();

            ProvidersId = oPrvToProcess.Where(x => x.ProviderPublicId != null).Select(x => x.ProviderPublicId).Distinct().ToList();

            foreach (string ProviderPublicId in ProvidersId)
            {
                ProviderModel oProvider = new ProviderModel();
                oProvider.RelatedCompany = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(ProviderPublicId);

                if (ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListClearProvider(ProviderPublicId) && oProvider.RelatedCompany != null)
                {
                    int AlertId = oProvider.RelatedCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCompanyInfoType.UpdateAlert).
                    Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault();

                    oProvider.RelatedCompany.CompanyInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = AlertId,
                        ItemInfoType = new CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCompanyInfoType.UpdateAlert,
                        },
                        Enable = true,
                        Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    });

                    int StatusAlertId = oProvider.RelatedCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCompanyInfoType.Alert).
                        Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault();

                    oProvider.RelatedCompany.CompanyInfo.Add(new GenericItemInfoModel()
                    {
                        ItemInfoId = StatusAlertId,
                        ItemInfoType = new CatalogModel()
                        {
                            ItemId = (int)BackOffice.Models.General.enumCompanyInfoType.Alert,
                        },
                        Enable = true,
                        Value = ((int)BackOffice.Models.General.enumBlackList.BL_DontShowAlert).ToString(),
                    });

                    //Save DateTime of last Update Data
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.ProviderUpsert(oProvider);                        
                }
            }

            #region New Data

            oPrvToProcess = oPrvToProcess.GroupBy(x => x.IdentificationNumber).Select(grp => grp.First()).ToList();

            oPrvToProcess.Where(prv => (!string.IsNullOrEmpty(prv.ProviderPublicId)) &&
                (prv.BlackListStatus == "SI" || prv.BlackListStatus == "si" || prv.BlackListStatus == "Si") && (prv.Estado == "Activo")).All(prv =>
                {
                    try
                    {
                        #region Operation

                        ProviderModel oProviderToInsert = new ProviderModel();
                        oProviderToInsert.RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel();
                        oProviderToInsert.RelatedCompany.CompanyInfo = new List<GenericItemInfoModel>();
                        oProviderToInsert.RelatedCompany.CompanyPublicId = prv.ProviderPublicId;
                        oProviderToInsert.RelatedBlackList = new List<BlackListModel>();

                        CompanyModel BasicInfo = new CompanyModel();
                        BasicInfo = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(prv.ProviderPublicId);
                        oProviderToInsert.RelatedBlackList.Add(new BlackListModel
                        {
                            BlackListStatus = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumBlackList.BL_ShowAlert,
                            },
                            User = SessionModel.CurrentLoginUser.Name + "_" + SessionModel.CurrentLoginUser.LastName,
                            FileUrl = StrRemoteFile,
                            BlackListInfo = new List<GenericItemInfoModel>()
                        });

                        var Rows = from c in excel.Worksheet(page.FirstOrDefault())
                                   where c["ProviderPublicId"] == prv.ProviderPublicId && c["IdentificationNumber"] == prv.IdentificationNumber
                                   select c;

                                //Load the BlackList info
                                foreach (string item in Columns)
                        {
                            int indexCollumn = Columns.IndexOf(item);
                            oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemName = item,
                                },
                                Value = Rows.First()[indexCollumn].Value.ToString(),
                                Enable = true,
                            });
                        }

                        List<ProviderModel> oProviderResultList = new List<ProviderModel>();
                        oProviderResultList.Add(ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListInsert(oProviderToInsert));

                        var idResult = oProviderResultList.FirstOrDefault().RelatedBlackList.Where(x => x.BlackListInfo != null).Select(x => x.BlackListInfo.Select(y => y.ItemInfoId)).FirstOrDefault();

                                #region Set Provider Info

                                oProviderToInsert.RelatedCompany.CompanyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = BasicInfo.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.Alert)
                                        .Select(x => x.ItemInfoId).FirstOrDefault() != 0 ? BasicInfo.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.Alert)
                                        .Select(x => x.ItemInfoId).FirstOrDefault() : 0,
                            ItemInfoType = new CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumCompanyInfoType.Alert,
                            },
                            Value = ((int)BackOffice.Models.General.enumBlackList.BL_ShowAlert).ToString(),
                            Enable = true,
                        });

                                //Set large value With the items found
                                oProviderToInsert.RelatedCompany.CompanyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = BasicInfo.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.ListId)
                                        .Select(x => x.ItemInfoId).FirstOrDefault() != 0 ? BasicInfo.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.ListId)
                                        .Select(x => x.ItemInfoId).FirstOrDefault() : 0,
                            ItemInfoType = new CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumCompanyInfoType.ListId,
                            },
                            LargeValue = string.Join(",", idResult),
                            Enable = true,
                        });

                                #endregion Set Provider Info

                                ProveedoresOnLine.Company.Controller.Company.CompanyInfoUpsert(oProviderToInsert.RelatedCompany);

                        oPrvToProcessResult.Add(new ProviderExcelResultModel()
                        {
                            PrvModel = prv,
                            Success = true,
                            Error = "Se ha validado el Proveedor '" + oProviderToInsert.RelatedCompany.CompanyPublicId + "'",
                        });

                        ActualProvider = prv.ProviderPublicId;
                        FileName = FileName + ".xls";

                                #endregion Operation
                            }
                    catch (Exception err)
                    {
                        oPrvToProcessResult.Add(new ProviderExcelResultModel()
                        {
                            PrvModel = prv,
                            Success = false,
                            Error = "Error :: " + err.Message + " :: " +
                                        err.StackTrace +
                                        (err.InnerException == null ? string.Empty :
                                        " :: " + err.InnerException.Message + " :: " +
                                        err.InnerException.StackTrace),
                        });
                    }
                    return true;
                });

            #endregion New Data

            //save log file

            #region Error log file

            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(ErrorFilePath))
                {
                    string strSep = ";";

                    sw.WriteLine
                            (
                            "\"ProviderPublicId\"" + strSep +
                            "\"BlackListStatus\"" + strSep +

                            "\"Success\"" + strSep +
                            "\"Message\"");

                    oPrvToProcessResult.All(lg =>
                    {
                        sw.WriteLine
                        (
                        "\"" + lg.PrvModel.ProviderPublicId + "\"" + strSep +
                        "\"" + lg.PrvModel.BlackListStatus + "\"" + strSep +

                        "\"" + lg.Success + "\"" + strSep +
                        "\"" + lg.Error + "\"");

                        return true;
                    });

                    sw.Flush();
                    sw.Close();
                }

                //load file to s3
                string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                    (ErrorFilePath,
                    BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_File_ExcelDirectory].Value);
                //remove temporal file
                if (System.IO.File.Exists(ErrorFilePath))
                    System.IO.File.Delete(ErrorFilePath);

                return strRemoteFile;
            }
            catch { }

            return null;

            #endregion Error log file
        }

        #endregion Private Functions
    }
}