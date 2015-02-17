using BackOffice.Models.General;
using BackOffice.Models.Provider;
using LinqToExcel;
using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class AdminController : BaseController
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
                string strFolder = Server.MapPath(BackOffice.Models.General.Constants.C_Settings_File_TempDirectory);

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                //get File
                string strFile = strFolder.TrimEnd('\\') +
                    "\\ProviderUploadFile_" +
                    SessionModel.CurrentLoginUser + "_" +
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
                    (MVC.Admin.ActionNames.AdminUserUpsert,
                    MVC.Admin.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminUserUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });


            //Geolocalization
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Geolocalización",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminGeoUpsert,
                    MVC.Admin.Name),
                Position = 2,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminGeoUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Standart Economy Activity standar
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Maestras Estandar",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminEcoActivityUpsert,
                    MVC.Admin.Name, new { TreeId = 0, TreeName = "" }),
                Position = 3,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminEcoActivityUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Arboles
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Maestras Personalizadas",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminTreeUpsert,
                    MVC.Admin.Name),
                Position = 4,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminTreeUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Stantart Group
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Grupos",
                    Url = Url.Action
                            (MVC.Admin.ActionNames.AdminEcoGroupUpsert,
                            MVC.Admin.Name),
                    Position = 5,
                    IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminEcoGroupUpsert &&
                    oCurrentController == MVC.Admin.Name),
                });

            //Banks
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Bancos",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminBankUpsert,
                    MVC.Admin.Name),
                Position = 6,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminBankUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Certificactions Companies
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Empresas Certificadoras",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminCompanyRulesUpsert,
                    MVC.Admin.Name),
                Position = 7,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminCompanyRulesUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Certificados
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Certificados",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminRulesUpsert,
                    MVC.Admin.Name),
                Position = 8,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminRulesUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //Resoluciones
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "Resoluciones",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminResolutionUpsert,
                    MVC.Admin.Name),
                Position = 9,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminResolutionUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //TRM
            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
            {
                Name = "TRM",
                Url = Url.Action
                    (MVC.Admin.ActionNames.AdminTRMUpsert,
                    MVC.Admin.Name),
                Position = 10,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminTRMUpsert &&
                    oCurrentController == MVC.Admin.Name),
            });

            //add menu
            oReturn.Add(oMenuAux);

            #endregion

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
                    (MVC.Admin.ActionNames.AdminRLUploadProvider,
                    MVC.Admin.Name),
                Position = 1,
                IsSelected =
                    (oCurrentAction == MVC.Admin.ActionNames.AdminRLUploadProvider &&
                    oCurrentController == MVC.Admin.Name),
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

            return oReturn;
        }

        #endregion

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

            oPrvToProcess.Where(prv => !string.IsNullOrEmpty(prv.ProviderPublicId)).All(prv =>
            {
                try
                {
                    FileName = FileName.Replace(Path.GetExtension(FileName), "");
                    List<string> Columns = excel.GetColumnNames(FileName).ToList();

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
                            ItemId = prv.BlackListStatus == "si" ? (int)BackOffice.Models.General.enumBlackList.BL_ShowAlert : (int)BackOffice.Models.General.enumBlackList.BL_DontShowAlert,
                        },
                        User = SessionModel.CurrentLoginUser.Name + "_" + SessionModel.CurrentLoginUser.LastName,
                        FileUrl = StrRemoteFile,                        
                        BlackListInfo = new List<GenericItemInfoModel>()
                    });

                    var Rows = from c in excel.Worksheet(FileName)
                               where c["ProviderPublicId"] == prv.ProviderPublicId
                               select c;

                    //Load the BlackList info
                    foreach (string item in Columns)
                    {
                        int index = Columns.IndexOf(item);
                        oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemName = item,
                            },
                            Value = Rows.First()[index].Value.ToString(),
                            Enable = true,
                        });
                    }

                    if (prv.BlackListStatus == "si")
                    {
                        
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
                    }
                    else
                    {
                        
                        oProviderToInsert.RelatedCompany.CompanyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = BasicInfo.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.Alert)
                                        .Select(x => x.ItemInfoId).FirstOrDefault() != 0 ? BasicInfo.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.Alert)
                                        .Select(x => x.ItemInfoId).FirstOrDefault() : 0,
                            ItemInfoType = new CatalogModel()
                            {
                                ItemId = (int)BackOffice.Models.General.enumCompanyInfoType.Alert,
                            },
                            Value = ((int)BackOffice.Models.General.enumBlackList.BL_DontShowAlert).ToString(),
                            Enable = true,
                        });
                    }
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListInsert(oProviderToInsert);
                    ProveedoresOnLine.Company.Controller.Company.CompanyInfoUpsert(oProviderToInsert.RelatedCompany);


                    oPrvToProcessResult.Add(new ProviderExcelResultModel()
                    {
                        PrvModel = prv,
                        Success = true,
                        Error = "Se ha validado el Proveedor '" + oProviderToInsert.RelatedCompany.CompanyPublicId + "'",
                    });
                    #endregion
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
            #endregion            
        }
        #endregion
    }
}