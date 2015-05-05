﻿using System;
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
                        oProjectProvider.GetApprovalStatus() == null)
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
                        Convert.ToInt32(strSplit[1].Trim()) == (int)MarketPlace.Models.General.enumProjectInfoType.DefaultEconomicActivity)
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

            return oReturn;
        }

        private MessageModule.Client.Models.ClientMessageModel GetProjectMessage
            (MarketPlace.Models.Project.ProjectViewModel vProject,
            MarketPlace.Models.Project.ProjectProviderViewModel vProjectProvider,
            MarketPlace.Models.Project.EvaluationItemViewModel vEvaluationArea)
        {
            //get to for message
            List<string> oTo = new List<string>();

            if (vEvaluationArea.EvaluatorType != null &&
                vEvaluationArea.EvaluatorType == Models.General.enumEvaluatorType.SpecificPerson &&
                !string.IsNullOrEmpty(vEvaluationArea.Evaluator) &&
                vEvaluationArea.Evaluator.Any(ev => ev == '@'))
            {
                //specific user
                oTo.Add(vEvaluationArea.Evaluator);
            }
            else if (vEvaluationArea.EvaluatorType != null &&
                    vEvaluationArea.EvaluatorType == Models.General.enumEvaluatorType.AnyInRol &&
                    !string.IsNullOrEmpty(vEvaluationArea.Evaluator))
            {
                //all users in role
                List<ProveedoresOnLine.Company.Models.Company.UserCompany> oUsersTo = ProveedoresOnLine.Company.Controller.Company.MP_UserCompanySearch
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    "@",
                    Convert.ToInt32(vEvaluationArea.Evaluator.Replace(" ", "")),
                    0,
                    20);

                oTo = oUsersTo.Select(usr => usr.User).Distinct().ToList();
            }

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
                        }))));
                return oReturn;
            }
            return null;
        }

        #endregion
    }
}
