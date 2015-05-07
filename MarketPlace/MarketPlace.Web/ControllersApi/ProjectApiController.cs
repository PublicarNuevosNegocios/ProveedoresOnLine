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
        #region Project

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
                if (string.IsNullOrEmpty(oProjectToUpsert.ProjectPublicId) &&
                    oProjectToUpsert.ProjectInfo.
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
        public bool ProjectClose
            (string ProjectClose)
        {
            if (ProjectClose == "true")
            {
                //get project request
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = GetProjectUpsertRequest();

                //upsert project
                oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectUpsert(oProjectToUpsert);

                return true;
            }
            return false;
        }

        [HttpPost]
        [HttpGet]
        public bool ProjectAward
            (string ProjectAward)
        {
            if (ProjectAward == "true")
            {
                //get project request
                ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = GetProjectUpsertRequest();

                //upsert project
                oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectUpsert(oProjectToUpsert);

                return true;
            }
            return false;
        }

        #endregion

        #region Project Company

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

        #endregion

        #region Project files

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

        #endregion

        #region Project Approval

        [HttpPost]
        [HttpGet]
        public bool ProjectRequestApproval
            (string ProjectRequestApproval,
            string ProjectPublicId,
            string ProviderPublicId)
        {
            bool oReturn = false;

            if (ProjectRequestApproval == "true" &&
                !string.IsNullOrEmpty(ProjectPublicId) &&
                !string.IsNullOrEmpty(ProviderPublicId))
            {
                //get project basic info
                MarketPlace.Models.Project.ProjectViewModel oProject = new Models.Project.ProjectViewModel
                    (ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetById
                        (ProjectPublicId,
                        MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId));

                //validate project status
                if (oProject.ProjectStatus == MarketPlace.Models.General.enumProjectStatus.Open ||
                    oProject.ProjectStatus == MarketPlace.Models.General.enumProjectStatus.OpenRefusal)
                {
                    //get provider for approval
                    MarketPlace.Models.Project.ProjectProviderViewModel oProjectProvider = oProject.RelatedProjectProvider.
                        Where(pjpv => pjpv.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId == ProviderPublicId).
                        FirstOrDefault();

                    //validate provider and status
                    if (oProjectProvider != null &&
                        oProjectProvider.ApprovalStatus == null)
                    {
                        //create project to upsert
                        ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
                        {
                            ProjectPublicId = oProject.ProjectPublicId,
                            ProjectName = oProject.ProjectName,
                            RelatedProjectConfig = new ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel()
                            {
                                ItemId = oProject.RelatedProjectConfig.ProjectConfigId,
                            },
                            ProjectStatus = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)MarketPlace.Models.General.enumProjectStatus.Approval,
                            },
                            Enable = true,
                            RelatedProjectProvider = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel>() 
                            { 
                                new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                                {
                                    RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                                    {
                                        RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                                        {
                                            CompanyPublicId = oProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                                        },
                                    },

                                    ItemInfo = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel>()
                                    {
                                        new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
                                        {
                                            RelatedEvaluationItem = null,
                                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus,
                                            },
                                            Value = ((int)MarketPlace.Models.General.enumApprovalStatus.Pending).ToString(),
                                            Enable = true,
                                        },
                                    },
                                    Enable = true,
                                },
                            },
                        };

                        List<MessageModule.Client.Models.ClientMessageModel> oMessageToSend = new List<MessageModule.Client.Models.ClientMessageModel>();

                        //loop for all evaluation areas
                        List<MarketPlace.Models.Project.EvaluationItemViewModel> oEvaluationAreas = oProject.RelatedProjectConfig.GetEvaluationAreas();
                        oEvaluationAreas.All(ea =>
                        {
                            #region Project and provider status

                            oProjectToUpsert.RelatedProjectProvider.Add(
                                new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                                {
                                    RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                                    {
                                        RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                                        {
                                            CompanyPublicId = oProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                                        },
                                    },

                                    ItemInfo = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel>()
                                    {
                                        new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
                                        {
                                            RelatedEvaluationItem = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                            {
                                                ItemId = ea.EvaluationItemId,
                                            },
                                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus,
                                            },
                                            Value = ((int)MarketPlace.Models.General.enumApprovalStatus.Pending).ToString(),
                                            Enable = true,
                                        },
                                    },
                                    Enable = true,
                                });

                            #endregion

                            #region Notifications

                            MessageModule.Client.Models.ClientMessageModel oAreaMessage = GetProjectMessage(oProject, oProjectProvider, ea);

                            if (oAreaMessage != null)
                                oMessageToSend.Add(oAreaMessage);

                            #endregion

                            return true;
                        });

                        //upsert project
                        oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectUpsert(oProjectToUpsert);

                        //send messages
                        oMessageToSend.All(msg =>
                        {
                            MessageModule.Client.Controller.ClientController.CreateMessage(msg);
                            return true;
                        });
                    }
                }

                oReturn = true;
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public bool ProjectApproveEvaluationArea
            (string ProjectApproveEvaluationArea,
            string ProjectPublicId,
            string ProviderPublicId,
            string EvaluationAreaId)
        {
            bool oReturn = false;

            if (ProjectApproveEvaluationArea == "true" &&
                !string.IsNullOrEmpty(ProjectPublicId) &&
                !string.IsNullOrEmpty(ProviderPublicId) &&
                !string.IsNullOrEmpty(EvaluationAreaId) &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["ApprovalText"]))
            {
                //get project basic info
                MarketPlace.Models.Project.ProjectViewModel oProject = new Models.Project.ProjectViewModel
                    (ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetById
                        (ProjectPublicId,
                        MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId));

                //set current evaluation area
                oProject.RelatedProjectConfig.SetCurrentEvaluationArea(Convert.ToInt32(EvaluationAreaId.Replace(" ", "")));

                //validate project status
                if (oProject.ProjectStatus == MarketPlace.Models.General.enumProjectStatus.Approval &&
                    oProject.RelatedProjectConfig.CurrentEvaluationArea != null)
                {
                    //get provider for approval
                    MarketPlace.Models.Project.ProjectProviderViewModel oProjectProvider = oProject.RelatedProjectProvider.
                        Where(pjpv => pjpv.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId == ProviderPublicId).
                        FirstOrDefault();

                    //get current area evaluation status
                    MarketPlace.Models.General.enumApprovalStatus? oCurrentEvaluationAreaStatus =
                        oProjectProvider.GetApprovalStatusByArea(oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId);

                    //validate provider and status
                    if (oProjectProvider != null &&
                        oProjectProvider.ApprovalStatus != null &&
                        oProjectProvider.ApprovalStatus == Models.General.enumApprovalStatus.Pending &&

                        oCurrentEvaluationAreaStatus != null &&
                        oCurrentEvaluationAreaStatus == Models.General.enumApprovalStatus.Pending)
                    {
                        //validate status for all areas for status project and provider
                        Dictionary<int, MarketPlace.Models.General.enumApprovalStatus> oProviderAprovalAreas = oProjectProvider.GetApprovalStatusAllAreas();
                        List<MarketPlace.Models.Project.EvaluationItemViewModel> oAreas = oProject.RelatedProjectConfig.GetEvaluationAreas();

                        bool oApproveArea = true, oApproveProvider = true;
                        oAreas.
                            Where(ea => ea.EvaluationItemId != oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId).
                            All(ea =>
                            {
                                if (oProviderAprovalAreas.ContainsKey(ea.EvaluationItemId))
                                {
                                    if (oProviderAprovalAreas[ea.EvaluationItemId] == Models.General.enumApprovalStatus.Pending)
                                    {
                                        //any area pending
                                        oApproveProvider = false;
                                    }
                                    else if (oProviderAprovalAreas[ea.EvaluationItemId] != Models.General.enumApprovalStatus.Approved)
                                    {
                                        //any area rejected or award
                                        oApproveArea = false;
                                        oApproveProvider = false;
                                    }
                                }
                                else
                                {
                                    //any area not referenced
                                    oApproveArea = false;
                                    oApproveProvider = false;
                                }
                                return true;
                            });

                        if (oApproveArea)
                        {
                            //create project to upsert
                            ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
                            {
                                ProjectPublicId = oProject.ProjectPublicId,
                                ProjectName = oProject.ProjectName,
                                RelatedProjectConfig = new ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel()
                                {
                                    ItemId = oProject.RelatedProjectConfig.ProjectConfigId,
                                },
                                ProjectStatus = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = oApproveProvider ? (int)MarketPlace.Models.General.enumProjectStatus.Award :
                                                                (int)MarketPlace.Models.General.enumProjectStatus.Approval,
                                },
                                Enable = true,
                                RelatedProjectProvider = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel>() 
                                { 
                                    new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                                    {
                                        RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                                        {
                                            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                                            {
                                                CompanyPublicId = oProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                                            },
                                        },

                                        ItemInfo = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel>()
                                        {
                                            new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
                                            {
                                                ItemInfoId = oProjectProvider.ApprovalStatusId,
                                                RelatedEvaluationItem = null,
                                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus,
                                                },
                                                Value = oApproveProvider ? ((int)MarketPlace.Models.General.enumApprovalStatus.Approved).ToString() : 
                                                                           ((int)MarketPlace.Models.General.enumApprovalStatus.Pending).ToString(),
                                                Enable = true,
                                            },
                                            new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
                                            {
                                                ItemInfoId = oProjectProvider.GetApprovalStatusIdByArea(oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId),
                                                RelatedEvaluationItem = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                                {
                                                    ItemId = oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId,
                                                },
                                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus,
                                                },
                                                Value = ((int)MarketPlace.Models.General.enumApprovalStatus.Approved).ToString(),
                                                Enable = true,
                                            },
                                            new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
                                            {
                                                ItemInfoId = oProjectProvider.GetApprovalStatusIdByArea(oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId),
                                                RelatedEvaluationItem = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                                {
                                                    ItemId = oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId,
                                                },
                                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalText,
                                                },
                                                LargeValue = System.Web.HttpContext.Current.Request["ApprovalText"],
                                                Enable = true,
                                            },
                                        },
                                        Enable = true,
                                    },
                                },
                            };

                            //upsert project
                            oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectUpsert(oProjectToUpsert);

                            //send approve message
                            MessageModule.Client.Controller.ClientController.CreateMessage(
                                GetProjectAreaMessage(oProject,
                                        oProjectProvider,
                                        oProject.RelatedProjectConfig.CurrentEvaluationArea,
                                        MarketPlace.Models.General.InternalSettings.Instance
                                            [MarketPlace.Models.General.Constants.C_Settings_Project_ApproveArea_MailAgent].Value
                                ));
                        }
                    }
                }

            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public bool ProjectRejectEvaluationArea
            (string ProjectRejectEvaluationArea,
            string ProjectPublicId,
            string ProviderPublicId,
            string EvaluationAreaId)
        {
            bool oReturn = false;

            if (ProjectRejectEvaluationArea == "true" &&
                !string.IsNullOrEmpty(ProjectPublicId) &&
                !string.IsNullOrEmpty(ProviderPublicId) &&
                !string.IsNullOrEmpty(EvaluationAreaId) &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["ApprovalText"]))
            {
                //get project basic info
                MarketPlace.Models.Project.ProjectViewModel oProject = new Models.Project.ProjectViewModel
                    (ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetById
                        (ProjectPublicId,
                        MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId));

                //set current evaluation area
                oProject.RelatedProjectConfig.SetCurrentEvaluationArea(Convert.ToInt32(EvaluationAreaId.Replace(" ", "")));

                //validate project status
                if (oProject.ProjectStatus == MarketPlace.Models.General.enumProjectStatus.Approval &&
                    oProject.RelatedProjectConfig.CurrentEvaluationArea != null)
                {
                    //get provider for approval
                    MarketPlace.Models.Project.ProjectProviderViewModel oProjectProvider = oProject.RelatedProjectProvider.
                        Where(pjpv => pjpv.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId == ProviderPublicId).
                        FirstOrDefault();

                    //get current area evaluation status
                    MarketPlace.Models.General.enumApprovalStatus? oCurrentEvaluationAreaStatus =
                        oProjectProvider.GetApprovalStatusByArea(oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId);

                    //validate provider and status
                    if (oProjectProvider != null &&
                        oProjectProvider.ApprovalStatus != null &&
                        oProjectProvider.ApprovalStatus == Models.General.enumApprovalStatus.Pending &&

                        oCurrentEvaluationAreaStatus != null &&
                        oCurrentEvaluationAreaStatus == Models.General.enumApprovalStatus.Pending)
                    {
                        //create project to upsert
                        ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectToUpsert = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
                        {
                            ProjectPublicId = oProject.ProjectPublicId,
                            ProjectName = oProject.ProjectName,
                            RelatedProjectConfig = new ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel()
                            {
                                ItemId = oProject.RelatedProjectConfig.ProjectConfigId,
                            },
                            ProjectStatus = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)MarketPlace.Models.General.enumProjectStatus.OpenRefusal,
                            },
                            Enable = true,
                            RelatedProjectProvider = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel>() 
                                { 
                                    new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                                    {
                                        RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                                        {
                                            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                                            {
                                                CompanyPublicId = oProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                                            },
                                        },

                                        ItemInfo = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel>()
                                        {
                                            new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
                                            {
                                                ItemInfoId = oProjectProvider.ApprovalStatusId,
                                                RelatedEvaluationItem = null,
                                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus,
                                                },
                                                Value = ((int)MarketPlace.Models.General.enumApprovalStatus.Rejected).ToString(),
                                                Enable = true,
                                            },
                                            new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
                                            {
                                                ItemInfoId = oProjectProvider.GetApprovalStatusIdByArea(oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId),
                                                RelatedEvaluationItem = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                                {
                                                    ItemId = oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId,
                                                },
                                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus,
                                                },
                                                Value = ((int)MarketPlace.Models.General.enumApprovalStatus.Rejected).ToString(),
                                                Enable = true,
                                            },
                                            new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
                                            {
                                                ItemInfoId = oProjectProvider.GetApprovalStatusIdByArea(oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId),
                                                RelatedEvaluationItem = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                                {
                                                    ItemId = oProject.RelatedProjectConfig.CurrentEvaluationArea.EvaluationItemId,
                                                },
                                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalText,
                                                },
                                                LargeValue = System.Web.HttpContext.Current.Request["ApprovalText"],
                                                Enable = true,
                                            },
                                        },
                                        Enable = true,
                                    },
                                },
                        };

                        //upsert project
                        oProjectToUpsert = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectUpsert(oProjectToUpsert);

                        //send approve message
                        MessageModule.Client.Controller.ClientController.CreateMessage(
                            GetProjectAreaMessage(oProject,
                                    oProjectProvider,
                                    oProject.RelatedProjectConfig.CurrentEvaluationArea,
                                    MarketPlace.Models.General.InternalSettings.Instance
                                        [MarketPlace.Models.General.Constants.C_Settings_Project_RejectArea_MailAgent].Value
                            ));
                    }
                }

            }
            return oReturn;
        }

        #endregion

        #region private methods

        private ProveedoresOnLine.ProjectModule.Models.ProjectModel GetProjectUpsertRequest()
        {
            ProveedoresOnLine.ProjectModule.Models.ProjectModel oReturn = new ProveedoresOnLine.ProjectModule.Models.ProjectModel()
            {
                ProjectPublicId = System.Web.HttpContext.Current.Request["ProjectPublicId"],
                ProjectName = System.Web.HttpContext.Current.Request["ProjectName"],
                ProjectStatus = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["ProjectStatus"]) ?
                        (int)MarketPlace.Models.General.enumProjectStatus.Open :
                        Convert.ToInt32(System.Web.HttpContext.Current.Request["ProjectStatus"].Replace(" ", "")),
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
                    string strValue = null, strLargeValue = null;

                    if (Convert.ToInt32(strSplit[1].Trim()) == (int)MarketPlace.Models.General.enumProjectInfoType.CustomEconomicActivity ||
                        Convert.ToInt32(strSplit[1].Trim()) == (int)MarketPlace.Models.General.enumProjectInfoType.DefaultEconomicActivity ||
                        Convert.ToInt32(strSplit[1].Trim()) == (int)MarketPlace.Models.General.enumProjectInfoType.CloseText ||
                        Convert.ToInt32(strSplit[1].Trim()) == (int)MarketPlace.Models.General.enumProjectInfoType.AwardText)
                    {
                        strLargeValue = System.Web.HttpContext.Current.Request[req];
                    }
                    else
                    {
                        strValue = System.Web.HttpContext.Current.Request[req];
                    }

                    oReturn.ProjectInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = !string.IsNullOrEmpty(strSplit[2]) ? Convert.ToInt32(strSplit[2].Trim()) : 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = Convert.ToInt32(strSplit[1].Trim())
                        },
                        Value = strValue,
                        LargeValue = strLargeValue,
                        Enable = true,
                    });
                }
                return true;
            });

            //get project company info
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["ProjectCompanyId"]) &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["ProviderPublicId"]))
            {
                oReturn.RelatedProjectProvider = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel>() 
                {
                    new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                    {
                        ProjectCompanyId = Convert.ToInt32(System.Web.HttpContext.Current.Request["ProjectCompanyId"].Replace(" ","")),
                        RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                        {
                            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = System.Web.HttpContext.Current.Request["ProviderPublicId"].Replace(" ",""),
                            },
                        },
                        Enable = true,
                        ItemInfo = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel>(),
                    },
                };

                System.Web.HttpContext.Current.Request.Form.AllKeys.Where(x => x.Contains("ProjectCompanyInfo_")).All(req =>
                {
                    string[] strSplit = req.Split('_');

                    if (strSplit.Length >= 3)
                    {
                        string strValue = null, strLargeValue = null;

                        if (Convert.ToInt32(strSplit[1].Trim()) == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalText)
                        {
                            strLargeValue = System.Web.HttpContext.Current.Request[req];
                        }
                        else
                        {
                            strValue = System.Web.HttpContext.Current.Request[req];
                        }

                        oReturn.RelatedProjectProvider.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
                        {
                            ItemInfoId = !string.IsNullOrEmpty(strSplit[2]) ? Convert.ToInt32(strSplit[2].Trim()) : 0,
                            RelatedEvaluationItem = null,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = Convert.ToInt32(strSplit[1].Trim())
                            },
                            Value = strValue,
                            LargeValue = strLargeValue,
                            Enable = true,
                        });
                    }
                    return true;
                });
            }

            return oReturn;
        }

        private MessageModule.Client.Models.ClientMessageModel GetProjectMessage
            (MarketPlace.Models.Project.ProjectViewModel vProject,
            MarketPlace.Models.Project.ProjectProviderViewModel vProjectProvider,
            MarketPlace.Models.Project.EvaluationItemViewModel vEvaluationArea)
        {
            //get to for message
            List<string> oTo = vEvaluationArea.GetEvaluatorsEmails();

            if (oTo != null && oTo.Count > 0)
            {

                //Create message object
                MessageModule.Client.Models.ClientMessageModel oReturn = new MessageModule.Client.Models.ClientMessageModel()
                {
                    Agent = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Project_Approval_MailAgent].Value,
                    User = MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                    ProgramTime = DateTime.Now,
                    MessageQueueInfo = new List<Tuple<string, string>>(),
                };

                //get to address
                oTo.All(ovTo =>
                {
                    oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                        ("To", ovTo));
                    return true;
                });

                //get customer info
                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("CustomerLogo", MarketPlace.Models.General.SessionModel.CurrentCompany_CompanyLogo));

                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("CustomerName", MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyName));

                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("CustomerIdentificationTypeName", MarketPlace.Models.General.SessionModel.CurrentCompany.IdentificationType.ItemName));

                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("CustomerIdentificationNumber", MarketPlace.Models.General.SessionModel.CurrentCompany.IdentificationNumber));

                //get provider info
                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("ProviderLogo", vProjectProvider.RelatedProvider.RelatedLiteProvider.ProviderLogoUrl));

                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("ProviderName", vProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName));

                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("ProviderIdentificationTypeName", vProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName));

                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("ProviderIdentificationNumber", vProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber));

                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("ProviderLink",
                    Url.Content(Url.Route(MarketPlace.Models.General.Constants.C_Routes_Default,
                        new
                        {
                            controller = MVC.Provider.Name,
                            action = MVC.Provider.ActionNames.GIProviderInfo,
                            ProviderPublicId = vProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                        }))));

                //get project info
                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("ProjectName", vProject.ProjectName));

                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("ProjectLastModify", vProject.LastModify));

                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("ProjectUrl",
                    Url.Content(Url.Route(MarketPlace.Models.General.Constants.C_Routes_Default,
                        new
                        {
                            controller = MVC.Project.Name,
                            action = MVC.Project.ActionNames.ProjectDetail,
                            ProjectPublicId = vProject.ProjectPublicId,
                        }))));

                //get area info
                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("EvaluationAreaName", vEvaluationArea.EvaluationItemName));

                //get project provider url
                oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                    ("ProjectProviderUrl",
                    Url.Content(Url.Route(MarketPlace.Models.General.Constants.C_Routes_Default,
                        new
                        {
                            controller = MVC.Project.Name,
                            action = MVC.Project.ActionNames.ProjectProviderDetail,
                            ProjectPublicId = vProject.ProjectPublicId,
                            ProviderPublicId = vProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                            EvaluationAreaId = vEvaluationArea.EvaluationItemId.ToString(),
                        }))));
                return oReturn;
            }
            return null;
        }

        private MessageModule.Client.Models.ClientMessageModel GetProjectAreaMessage
            (MarketPlace.Models.Project.ProjectViewModel vProject,
            MarketPlace.Models.Project.ProjectProviderViewModel vProjectProvider,
            MarketPlace.Models.Project.EvaluationItemViewModel vEvaluationArea,
            string vAgent)
        {
            //Create message object
            MessageModule.Client.Models.ClientMessageModel oReturn = new MessageModule.Client.Models.ClientMessageModel()
            {
                Agent = vAgent,
                User = MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                ProgramTime = DateTime.Now,
                MessageQueueInfo = new List<Tuple<string, string>>(),
            };

            //get to address
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("To", MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email));

            //get customer info
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("CustomerLogo", MarketPlace.Models.General.SessionModel.CurrentCompany_CompanyLogo));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("CustomerName", MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyName));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("CustomerIdentificationTypeName", MarketPlace.Models.General.SessionModel.CurrentCompany.IdentificationType.ItemName));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("CustomerIdentificationNumber", MarketPlace.Models.General.SessionModel.CurrentCompany.IdentificationNumber));

            //get provider info
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProviderLogo", vProjectProvider.RelatedProvider.RelatedLiteProvider.ProviderLogoUrl));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProviderName", vProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProviderIdentificationTypeName", vProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProviderIdentificationNumber", vProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProviderLink",
                Url.Content(Url.Route(MarketPlace.Models.General.Constants.C_Routes_Default,
                    new
                    {
                        controller = MVC.Provider.Name,
                        action = MVC.Provider.ActionNames.GIProviderInfo,
                        ProviderPublicId = vProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                    }))));

            //get project info
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProjectName", vProject.ProjectName));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProjectLastModify", vProject.LastModify));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProjectUrl",
                Url.Content(Url.Route(MarketPlace.Models.General.Constants.C_Routes_Default,
                    new
                    {
                        controller = MVC.Project.Name,
                        action = MVC.Project.ActionNames.ProjectDetail,
                        ProjectPublicId = vProject.ProjectPublicId,
                    }))));

            //get area info
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("EvaluationAreaName", vEvaluationArea.EvaluationItemName));

            //get project provider url
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProjectProviderUrl",
                Url.Content(Url.Route(MarketPlace.Models.General.Constants.C_Routes_Default,
                    new
                    {
                        controller = MVC.Project.Name,
                        action = MVC.Project.ActionNames.ProjectProviderDetail,
                        ProjectPublicId = vProject.ProjectPublicId,
                        ProviderPublicId = vProjectProvider.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                        EvaluationAreaId = vEvaluationArea.EvaluationItemId.ToString(),
                    }))));

            return oReturn;
        }

        #endregion
    }
}
