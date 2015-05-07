using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
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
            string SurveyPublicId)
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
                                "\\ProjectFile\\" + SurveyPublicId + "\\");

                        //remove temporal file
                        if (System.IO.File.Exists(strLocalFile))
                            System.IO.File.Delete(strLocalFile);

                        //add file to project
                        //ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
                        //{
                        //    ProjectPublicId = SurveyPublicId,
                        //    ProjectInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>() 
                        //    {
                        //        new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        //        {
                        //            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        //            {
                        //                ItemId = (int)MarketPlace.Models.General.enumProjectInfoType.File,
                        //            },
                        //            LargeValue = oFile.ServerUrl + "," + oFile.FileName,
                        //            Enable = true,
                        //        },
                        //    },
                        //};

                        //oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectInfoUpsert(oProjectToUpsert);

                        //oFile.FileObjectId = oProjectToUpsert.ProjectInfo.Where
                        //    (pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.File &&
                        //            pjinf.ItemInfoId > 0).
                        //            Select(pjinf => pjinf.ItemInfoId.ToString()).
                        //            DefaultIfEmpty(string.Empty).
                        //            FirstOrDefault();

                        //lstUsedFiles.Add(oFile.ServerUrl);

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
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oProject = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetByIdLite
                    (SurveyPublicId,
                    MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

                //create object to upsert
                //ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
                //{
                //    ProjectPublicId = ProjectPublicId,
                //    ProjectInfo = oProject.ProjectInfo.
                //        Where(pjinf => pjinf.ItemInfoId == ProjectInfoId).
                //        Select(pjinf => new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                //        {
                //            ItemInfoId = pjinf.ItemInfoId,
                //            ItemInfoType = pjinf.ItemInfoType,
                //            Value = pjinf.Value,
                //            LargeValue = pjinf.LargeValue,
                //            Enable = false,
                //        }).ToList(),
                //};

                //upsert project info
                //oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectInfoUpsert(oProjectToUpsert);

                oReturn = true;
            }
            return oReturn;
        }


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
