using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class ProjectApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public string ProjectUpsert
            (string ProjectUpsert)
        {
            if (ProjectUpsert == "true")
            {
                //get project request
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = GetProjectUpsertRequest();

                //validate related compare
                if (oProjectToUpsert.ProjectInfo.
                        Any(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Compare &&
                                !string.IsNullOrEmpty(x.Value)))
                {
                    int oCompareId = oProjectToUpsert.ProjectInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.Compare &&
                                    !string.IsNullOrEmpty(x.Value)).
                        Select(x => Convert.ToInt32(x.Value.Replace(" ", ""))).
                        DefaultIfEmpty(0).
                        FirstOrDefault();

                    ProveedoresOnLine.CompareModule.Models.CompareModel oCompare = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                            CompareGetCompanyBasicInfo
                            (oCompareId,
                            MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                            MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

                    if (oCompare != null &&
                        oCompare.RelatedProvider != null &&
                        oCompare.RelatedProvider.Count > 0)
                    {
                        //get compare providers

                        oProjectToUpsert.RelatedProjectProvider = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel>();

                        oCompare.RelatedProvider.All(prv =>
                        {
                            oProjectToUpsert.RelatedProjectProvider.Add(new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                            {
                                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                                {
                                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                                    {
                                        CompanyPublicId = prv.RelatedCompany.CompanyPublicId,
                                    }
                                },
                                Enable = true,
                            });

                            return true;
                        });
                    }

                }

                //upsert project
                oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectUpsert(oProjectToUpsert);

                return oProjectToUpsert.ProjectPublicId;
            }
            return string.Empty;
        }

        [HttpPost]
        [HttpGet]
        public bool ProjectAddCompany
            (string ProjectAddCompany,
            string ProjectPublicId,
            string ProviderPublicId)
        {
            if (ProjectAddCompany == "true")
            {
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
                {
                    ProjectPublicId = ProjectPublicId,
                    RelatedProjectProvider = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel>()
                    {
                        new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                        {
                            Enable = true,
                            RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                            {
                                RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                                {
                                    CompanyPublicId = ProviderPublicId,
                                },
                            },
                        },
                    },
                };

                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectCompanyUpsert(oProjectToUpsert);

                return true;
            }

            return false;
        }

        [HttpPost]
        [HttpGet]
        public bool ProjectRemoveCompany
            (string ProjectRemoveCompany,
            string ProjectPublicId,
            string ProviderPublicId)
        {
            if (ProjectRemoveCompany == "true")
            {
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
                {
                    ProjectPublicId = ProjectPublicId,
                    RelatedProjectProvider = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel>()
                    {
                        new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                        {
                            Enable = false,
                            RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                            {
                                RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                                {
                                    CompanyPublicId = ProviderPublicId,
                                },
                            },
                        },
                    },
                };

                ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectCompanyUpsert(oProjectToUpsert);

                return true;
            }

            return false;
        }

        [HttpPost]
        [HttpGet]
        public MarketPlace.Models.Project.ProjectViewModel ProjectGet
            (string ProjectGet,
            string ProjectPublicId)
        {
            if (ProjectGet == "true")
            {
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectResult = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.
                       ProjectGetByIdLite
                       (ProjectPublicId,
                       MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

                MarketPlace.Models.Project.ProjectViewModel oReturn = new Models.Project.ProjectViewModel(oProjectResult);

                return oReturn;
            }
            return null;
        }


        [HttpPost]
        [HttpGet]
        public List<MarketPlace.Models.General.FileModel> ProjectUploadFile
            (string ProjectUploadFile,
            string ProjectPublicId)
        {
            List<MarketPlace.Models.General.FileModel> oReturn = new List<MarketPlace.Models.General.FileModel>();

            if (ProjectUploadFile == "true" &&
                !string.IsNullOrEmpty(ProjectPublicId) &&
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
                            "\\ProjectFile_" +
                            ProjectPublicId + "_" +
                            DateTime.Now.ToString("yyyyMMddHHmmss") + "." +
                            oFile.FileExtension;

                        oUploadFile.SaveAs(strLocalFile);

                        //load file to s3
                        oFile.ServerUrl = ProveedoresOnLine.FileManager.FileController.LoadFile
                            (strLocalFile,
                            MarketPlace.Models.General.InternalSettings.Instance
                                [MarketPlace.Models.General.Constants.C_Settings_File_RemoteDirectory].Value.TrimEnd('\\') +
                                "\\ProjectFile\\" + ProjectPublicId + "\\");

                        //remove temporal file
                        if (System.IO.File.Exists(strLocalFile))
                            System.IO.File.Delete(strLocalFile);

                        //add file to project
                        ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
                        {
                            ProjectPublicId = ProjectPublicId,
                            ProjectInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>() 
                            {
                                new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                {
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)MarketPlace.Models.General.enumProjectInfoType.File,
                                    },
                                    LargeValue = oFile.ServerUrl + "," + oFile.FileName,
                                    Enable = true,
                                },
                            },
                        };

                        oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectInfoUpsert(oProjectToUpsert);

                        oFile.FileObjectId = oProjectToUpsert.ProjectInfo.Where
                            (pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectInfoType.File &&
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
        public bool ProjectRemoveFile
            (string ProjectRemoveFile,
            string ProjectPublicId,
            int ProjectInfoId)
        {
            bool oReturn = false;

            if (ProjectRemoveFile == "true" &&
                !string.IsNullOrEmpty(ProjectPublicId))
            {
                //get project basic info
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oProject = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetByIdLite
                    (ProjectPublicId,
                    MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

                //create object to upsert
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
                {
                    ProjectPublicId = ProjectPublicId,
                    ProjectInfo = oProject.ProjectInfo.
                        Where(pjinf => pjinf.ItemInfoId == ProjectInfoId).
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
                oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectInfoUpsert(oProjectToUpsert);

                oReturn = true;
            }
            return oReturn;
        }

        #region private methods

        private ProveedoresOnLine.ProjectModule.Models.ProjectModel GetProjectUpsertRequest()
        {
            ProveedoresOnLine.ProjectModule.Models.ProjectModel oReturn = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
            {
                ProjectPublicId = System.Web.HttpContext.Current.Request["ProjectPublicId"],
                ProjectName = System.Web.HttpContext.Current.Request["ProjectName"],
                ProjectStatus = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)MarketPlace.Models.General.enumProjectStatus.Open,
                },
                RelatedProjectConfig = new ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel()
                {
                    ItemId = Convert.ToInt32(System.Web.HttpContext.Current.Request["ProjectConfig"].Replace(" ", "")),
                },
                Enable = true,
                ProjectInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            };

            //get company info
            System.Web.HttpContext.Current.Request.Form.AllKeys.Where(x => x.Contains("ProjectInfo_")).All(req =>
            {
                string[] strSplit = req.Split('_');

                if (strSplit.Length >= 3)
                {
                    oReturn.ProjectInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
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
