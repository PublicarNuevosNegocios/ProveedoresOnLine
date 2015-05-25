using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
                    Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim()));

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

        #region Survey Charts

        [HttpPost]
        [HttpGet]
        public List<GenericChartsModel> GetSurveyByResponsable
            (string GetSurveyByResponsable)
        {
            //Get Charts By Module
            List<GenericChartsModel> oReturn = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;

            #region Survey
            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_SurveyModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };
            //Get By Responsable
            oRelatedChart.GenericChartsInfoModel = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.GetSurveyByResponsable(SessionModel.CurrentLoginUser.Email, DateTime.Now);
            if (oRelatedChart.GenericChartsInfoModel.Count > 0)
            {
                oRelatedChart.GenericChartsInfoModel.All(x =>
                {
                    x.ChartModuleInfoType = ((int)enumCategoryInfoType.CH_SurveyStatusByRol).ToString();
                    return true;
                });
            }
            oReturn.Add(oRelatedChart);
            #endregion

            return oReturn;
        }
        #endregion

        #region Private Methods

        private ProveedoresOnLine.SurveyModule.Models.SurveyModel GetSurveyUpsertRequest()
        {
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oReturn = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
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

                SurveyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
            };

            //get company info
            System.Web.HttpContext.Current.Request.Form.AllKeys.Where(x => x.Contains("SurveyInfo_")).All(req =>
            {
                string[] strSplit = req.Split('_');

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
                        Enable = true,
                    });
                }
                return true;
            });

            return oReturn;
        }

        #endregion
    }
}
