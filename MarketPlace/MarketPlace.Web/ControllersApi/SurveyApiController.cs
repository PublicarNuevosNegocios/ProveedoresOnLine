using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.SurveyModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MarketPlace.Web.ControllersApi
{
    public class SurveyApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<Tuple<int, string>> SurveyConfigSearchAC
            (string SurveyConfigSearchAC,
            string SearchParam)
        {
            List<Tuple<int, string>> oReturn = new List<Tuple<int, string>>();

            if (SurveyConfigSearchAC == "true")
            {
                List<ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel> SearchResult = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.MP_SurveyConfigSearch
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    SearchParam,
                    0,
                    Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_AutoCompleteRow].Value.Trim()));

                if (SearchResult != null && SearchResult.Count > 0)
                {
                    oReturn = SearchResult.
                        OrderBy(x => x.ItemName).
                        Select(x => new Tuple<int, string>
                            (x.ItemId,
                            x.ItemInfo.
                                Where(y => y.ItemInfoType.ItemId == (int)enumSurveyConfigInfoType.Group).
                                Select(y => string.IsNullOrEmpty(y.ValueName) ? string.Empty : y.ValueName + " - ").
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault() + x.ItemName)).
                        ToList();
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public MarketPlace.Models.Survey.SurveyViewModel SurveyUpsert
            (string SurveyUpsert)
        {
            MarketPlace.Models.Survey.SurveyViewModel oReturn = null;

            if (SurveyUpsert == "true")
            {
                ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyToUpsert = GetSurveyUpsertRequest();

                SurveyToUpsert = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyUpsert(SurveyToUpsert);

                oReturn = new Models.Survey.SurveyViewModel(SurveyToUpsert);
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<MarketPlace.Models.General.FileModel> SurveyUploadFile
            (string SurveyUploadFile,
            string SurveyPublicId,
            string SurveyConfigInfoId,
            string ProviderPublicId)
        {
            List<MarketPlace.Models.General.FileModel> oReturn = new List<MarketPlace.Models.General.FileModel>();

            if (SurveyUploadFile == "true" &&
                !string.IsNullOrEmpty(SurveyPublicId) &&
                System.Web.HttpContext.Current.Request.Files.AllKeys.Length > 0)
            {
                List<string> lstUsedFiles = new List<string>();

                //get folder
                string strFolder = System.Web.HttpContext.Current.Server.MapPath
                    (MarketPlace.Models.General.InternalSettings.Instance
                    [MarketPlace.Models.General.Constants.C_Settings_File_TempDirectory].Value);

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                System.Web.HttpContext.Current.Request.Files.AllKeys.All(reqFile =>
                {
                    MarketPlace.Models.General.FileModel oFile = new MarketPlace.Models.General.FileModel();

                    //get File request
                    var oUploadFile = System.Web.HttpContext.Current.Request.Files[reqFile];

                    if (oUploadFile != null && !string.IsNullOrEmpty(oUploadFile.FileName))
                    {
                        oFile.FileName = oUploadFile.FileName.Replace(",", "");

                        //save local file
                        string strLocalFile = strFolder.TrimEnd('\\') +
                            "\\SurveyFile_" +
                            SurveyPublicId + "_" +
                            DateTime.Now.ToString("yyyyMMddHHmmss") + "." +
                            oFile.FileExtension;

                        oUploadFile.SaveAs(strLocalFile);

                        //load file to s3
                        oFile.ServerUrl = ProveedoresOnLine.FileManager.FileController.LoadFile
                            (strLocalFile,
                            MarketPlace.Models.General.InternalSettings.Instance
                                [MarketPlace.Models.General.Constants.C_Settings_File_RemoteDirectory].Value.TrimEnd('\\') +
                                "\\SurveyFile\\" + SurveyPublicId + "\\");

                        //remove temporal file
                        if (System.IO.File.Exists(strLocalFile))
                            System.IO.File.Delete(strLocalFile);


                        ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurvey =
                        ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId);

                        //add file to Survey                        
                        ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurveyToUpsert = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
                        {
                            SurveyPublicId = SurveyPublicId,

                            RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                            {
                                RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                                {
                                    CompanyPublicId = ProviderPublicId,
                                }
                            },
                            RelatedSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel()
                            {
                                ItemId = Convert.ToInt32(SurveyConfigInfoId),
                            },

                            SurveyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>() 
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)MarketPlace.Models.General.enumSurveyInfoType.File,
                                    },
                                    LargeValue = oFile.ServerUrl + "," + oFile.FileName,
                                    Enable = true,
                                },
                            },
                            Enable = true,
                        };
                        ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyUpsert(oSurveyToUpsert);


                        oFile.FileObjectId = oSurveyToUpsert.SurveyInfo.Where
                            (pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.File &&
                                    pjinf.ItemInfoId > 0).
                                    Select(pjinf => pjinf.ItemInfoId.ToString()).
                                    DefaultIfEmpty(string.Empty).
                                    FirstOrDefault();

                        lstUsedFiles.Add(oFile.ServerUrl);

                        //add result to response                        
                        oReturn.Add(oFile);
                    }
                    return true;
                });

                //register used files
                LogManager.ClientLog.FileUsedCreate(lstUsedFiles);
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public bool SurveyRemoveFile
            (string SurveyRemoveFile,
            string SurveyPublicId,
            int SurveyInfoId)
        {
            bool oReturn = false;

            if (SurveyRemoveFile == "true" &&
                !string.IsNullOrEmpty(SurveyPublicId))
            {
                //get project basic info
                ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurvey = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById
                    (SurveyPublicId);

                //create object to upsert
                ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurveyToUpsert = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
                {
                    SurveyPublicId = SurveyPublicId,
                    RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                    {
                        RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                        {
                            CompanyPublicId = oSurvey.RelatedProvider.RelatedCompany.CompanyPublicId,
                        }
                    },
                    RelatedSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel(),
                    SurveyInfo = oSurvey.SurveyInfo.
                        Where(pjinf => pjinf.ItemInfoId == SurveyInfoId).
                        Select(pjinf => new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = pjinf.ItemInfoId,
                            ItemInfoType = pjinf.ItemInfoType,
                            Value = pjinf.Value,
                            LargeValue = pjinf.LargeValue,
                            Enable = false,
                        }).ToList(),
                };

                //upsert project info
                oSurveyToUpsert = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyInfoUpsert(oSurveyToUpsert);

                oReturn = true;
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<MarketPlace.Models.Survey.SurveyConfigItemViewModel> SCSurveyConfigItemGetBySurveyConfigId
            (string SCSurveyConfigItemGetBySurveyConfigId,
            string SurveyConfigId,
            string SurveyConfigItemType)
        {
            List<MarketPlace.Models.Survey.SurveyConfigItemViewModel> oReturn = new List<Models.Survey.SurveyConfigItemViewModel>();

            if (SCSurveyConfigItemGetBySurveyConfigId == "true" && SurveyConfigId != null)
            {
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oSearchResult =
                    ProveedoresOnLine.SurveyModule.Controller.SurveyModule.MP_SurveyConfigItemGetBySurveyConfigId
                    (Convert.ToInt32(SurveyConfigId.Trim()),
                    Convert.ToInt32(SurveyConfigItemType));

                if (oSearchResult != null && oSearchResult.Count > 0)
                {
                    oSearchResult.All(x =>
                    {
                        oReturn.Add(new MarketPlace.Models.Survey.SurveyConfigItemViewModel(x));
                        return true;
                    });
                }
            }

            return oReturn;
        }

        #region Survey Charts

        [HttpPost]
        [HttpGet]
        public List<Tuple<string, int,int>> GetSurveyByResponsable(string GetSurveyByResponsable)
        {
            //Get Charts By Module
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;

            #region Survey
            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_SurveyModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };

            //new function
            if (SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().RelatedCompanyRole.ParentRoleCompany == null)
                oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByResponsable(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId, string.Empty, DateTime.Now);
            else
                oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByResponsable(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId, SessionModel.CurrentLoginUser.Email, DateTime.Now);
                List<Tuple<string, int, int>> oReturn = new List<Tuple<string, int, int>>();
            

            if (oRelatedChart.GenericChartsInfoModel != null && oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    oReturn.Add(Tuple.Create(x.ItemName, x.Count,x.Year));
                    return true;
                });
            }
            #endregion
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<Tuple<int, string, int, int>> GetSurveyByName(string GetSurveyByName)
        {
            //Get Charts By Module
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;

            #region Survey
            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_SurveyModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };

            if (SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().RelatedCompanyRole.ParentRoleCompany == null)
            {
                oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByName(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId, string.Empty);
            }
            else
            {
                oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByName(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId, SessionModel.CurrentLoginUser.Email);
            }


            List<Tuple<int, string, int, int>> oReturn = new List<Tuple<int, string, int, int>>();

            if (oRelatedChart.GenericChartsInfoModel != null && oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    oReturn.Add(Tuple.Create(x.CountX, x.ItemName, x.Count, x.Year));
                    return true;
                });
            }
            #endregion
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<Tuple<string, string, string, int, int, int>> GetSurveyByEvaluators(string GetSurveyByEvaluators)
        {
            //Get Charts By Module
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;

            #region Survey
            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_SurveyModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };

            if (SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().RelatedCompanyRole.ParentRoleCompany == null)
            {
                oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByEvaluator(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId, string.Empty);
            }
            else
            {
                oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByEvaluator(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId, SessionModel.CurrentLoginUser.Email);
            }

            List<Tuple<string, string, string, int, int, int>> oReturn = new List<Tuple<string, string, string, int, int, int>>();

            if (oRelatedChart.GenericChartsInfoModel != null && oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    oReturn.Add(Tuple.Create(x.AxisY, x.ItemName, x.AxisX, x.Count, x.CountX, x.Year));
                    return true;
                });
            }
            #endregion
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<Tuple<string, int, string, string, int>> GetSurveyByMonth(string GetSurveyByMonth)
        {
            //Get Charts By Module
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;

            #region Survey
            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_SurveyModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };
            //Get By Year
            if (SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().RelatedCompanyRole.ParentRoleCompany == null)
            {
                oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByMonth(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId, string.Empty);
            }
            else
            {
                oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByMonth(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId, SessionModel.CurrentLoginUser.Email);
            }


            // Se repite el estado porque es necesario para el tooltip de la gráfica
            List<Tuple<string, int, string, string,int>> oReturn = new List<Tuple<string, int, string, string,int>>();


            if (oRelatedChart.GenericChartsInfoModel != null && oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    oReturn.Add(Tuple.Create(x.ItemName, x.Count, x.AxisX, x.ItemType,x.Year));
                    return true;
                });
            }
            #endregion
            return oReturn;
        }


        #endregion

        #region Private Methods

        private ProveedoresOnLine.SurveyModule.Models.SurveyModel GetSurveyUpsertRequest()
        {
            //Obtener El reponsable
            //Obetener todos los evaluadores

            List<Tuple<string, int, int>> EvaluatorsRoleObj = new List<Tuple<string, int, int>>();
            List<string> EvaluatorsEmail = new List<string>();

            #region Parent Survey
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oReturn = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
            {
                ChildSurvey = new List<ProveedoresOnLine.SurveyModule.Models.SurveyModel>(),

                SurveyPublicId = System.Web.HttpContext.Current.Request["SurveyPublicId"],
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = System.Web.HttpContext.Current.Request["ProviderPublicId"],
                    }
                },
                RelatedSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel()
                {
                    ItemId = Convert.ToInt32(System.Web.HttpContext.Current.Request["SurveyConfigId"].Trim()),
                },
                Enable = true,
                User = SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().User, //Responsable               
                SurveyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
            };

            System.Web.HttpContext.Current.Request.Form.AllKeys.Where(x => x.Contains("SurveyInfo_")).All(req =>
            {
                string[] strSplit = req.Split('_');

                //Set Parent Survey Info
                if (strSplit.Length >= 3)
                {
                    oReturn.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = !string.IsNullOrEmpty(strSplit[2]) ? Convert.ToInt32(strSplit[2].Trim()) : 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = Convert.ToInt32(strSplit[1].Trim())
                        },
                        Value = System.Web.HttpContext.Current.Request[req],
                        //LargeValue = Convert.ToInt32(strSplit[1].Trim()) == (int)enumSurveyInfoType.Evaluator ? strSplit[4].Trim() : string.Empty,
                        Enable = true,
                    });

                    if (Convert.ToInt32(strSplit[1].Trim()) == (int)enumSurveyInfoType.Evaluator)
                        EvaluatorsRoleObj.Add(new Tuple<string, int, int>(System.Web.HttpContext.Current.Request[req], Convert.ToInt32(strSplit[4].Trim()), Convert.ToInt32(strSplit[2].Trim())));
                }
                return true;
            });
            #endregion

            if (EvaluatorsRoleObj != null && EvaluatorsRoleObj.Count > 0)
            {
                EvaluatorsEmail = new List<string>();
                EvaluatorsEmail = EvaluatorsRoleObj.GroupBy(x => x.Item1).Select(grp => grp.First().Item1).ToList();

                #region Child Survey
                //Set survey by evaluators
                EvaluatorsEmail.All(x =>
                {
                    oReturn.ChildSurvey.Add(new SurveyModel()
                    {
                        SurveyPublicId = System.Web.HttpContext.Current.Request["SurveyPublicId"],
                        RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                        {
                            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = System.Web.HttpContext.Current.Request["ProviderPublicId"],
                            }
                        },
                        RelatedSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel()
                        {
                            ItemId = Convert.ToInt32(System.Web.HttpContext.Current.Request["SurveyConfigId"].Trim()),
                        },
                        Enable = true,
                        User = x,//Evaluator,                     
                        SurveyInfo = new List<GenericItemInfoModel>()
                    });
                    return true;
                });

                //Set SurveyChild Info
                oReturn.ChildSurvey.All(inf =>
                {
                    List<Tuple<int, int>> AreaIdList = new List<Tuple<int, int>>();
                    AreaIdList.AddRange(EvaluatorsRoleObj.Where(y => y.Item1 == inf.User).Select(y => new Tuple<int, int>(y.Item2, y.Item3)).ToList());
                    if (AreaIdList != null)
                    {
                        AreaIdList.All(a =>
                        {
                            inf.SurveyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = a.Item2 != null ? a.Item2 : 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyInfoType.CurrentArea
                                },
                                Value = a.Item1.ToString(),
                                Enable = true,
                            });
                            inf.SurveyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyInfoType.IssueDate
                                },
                                Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.IssueDate).Select(x => x.Value).FirstOrDefault(),
                                Enable = true,
                            });
                            inf.SurveyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyInfoType.Contract
                                },
                                Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Contract).Select(x => x.Value).FirstOrDefault(),
                                Enable = true,
                            });
                            inf.SurveyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyInfoType.Status
                                },
                                Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).Select(x => x.Value).FirstOrDefault(),
                                Enable = true,
                            });
                            inf.SurveyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyInfoType.Comments
                                },
                                Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Comments).Select(x => x.Value).FirstOrDefault(),
                                Enable = true,
                            });
                            inf.SurveyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyInfoType.Responsible
                                },
                                Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Responsible).Select(x => x.Value).FirstOrDefault(),
                                Enable = true,
                            });
                            inf.SurveyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyInfoType.ExpirationDate
                                },
                                Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.ExpirationDate).Select(x => x.Value).FirstOrDefault(),
                                Enable = true,
                            });

                            List<GenericItemInfoModel> oEvaluators = new List<GenericItemInfoModel>();
                            oEvaluators = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Evaluator).Select(x => x).ToList();

                            if (oEvaluators.Count > 0)
                            {
                                oEvaluators.All(x =>
                                {
                                    inf.SurveyInfo.Add(new GenericItemInfoModel()
                                    {
                                        ItemInfoId = 0,
                                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = (int)enumSurveyInfoType.Evaluator
                                        },
                                        Value = x.Value,
                                        Enable = true,
                                    });
                                    return true;
                                });
                            }
                            return true;
                        });
                    }
                    return true;
                });
                #endregion
            }
            return oReturn;
        }

        #endregion
    }
}
