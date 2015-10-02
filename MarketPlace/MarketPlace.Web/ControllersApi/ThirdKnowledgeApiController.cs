﻿using MarketPlace.Models.Company;
using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using OfficeOpenXml;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MarketPlace.Web.ControllersApi
{
    public class ThirdKnowledgeApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public ProviderViewModel TKSingleSearch(string TKSingleSearch)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.RelatedThirdKnowledge = new MarketPlace.Models.Company.ThirdKnowledgeViewModel();
            List<PlanModel> oCurrentPeriodList = new List<PlanModel>();

            try
            {
                //Get The Active Plan By Customer
                oCurrentPeriodList = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetCurrenPeriod(SessionModel.CurrentCompany.CompanyPublicId, true);

                if (oCurrentPeriodList != null && oCurrentPeriodList.Count > 0)
                {
                    oModel.RelatedThirdKnowledge.HasPlan = true;

                    //Get The Most Recently Period When Plan is More Than One
                    oModel.RelatedThirdKnowledge.CurrentPlanModel = oCurrentPeriodList.OrderByDescending(x => x.CreateDate).First();

                    #region Upsert Process

                    if (System.Web.HttpContext.Current.Request["UpsertRequest"] == "true")
                    {
                        //Set Current Sale
                        if (oModel.RelatedThirdKnowledge != null)
                        {
                            //Save Query
                            TDQueryModel oQueryToCreate = new TDQueryModel()
                            {
                                IsSuccess = true,
                                PeriodPublicId = oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                                SearchType = new TDCatalogModel()
                                {
                                    ItemId = (int)enumThirdKnowledgeQueryType.Simple,
                                },
                                User = SessionModel.CurrentLoginUser.Email,
                                QueryStatus = new TDCatalogModel()
                                {
                                    ItemId = (int)enumThirdKnowledgeQueryStatus.Finalized
                                },
                                RelatedQueryInfoModel = new List<TDQueryInfoModel>(),
                            };
                            TDQueryModel oQueryResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.SimpleRequest(oCurrentPeriodList.FirstOrDefault().
                                            RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                                           System.Web.HttpContext.Current.Request["IdentificationNumber"], System.Web.HttpContext.Current.Request["Name"], oQueryToCreate);
                            if (oQueryResult != null && oQueryResult.RelatedQueryInfoModel != null)
                            {
                                oModel.CollsResult = this.SetCollumnsResult(oQueryResult);
                            }
                            if (oQueryResult.IsSuccess == true)
                            {
                            //Set New Score
                            oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().TotalQueries = (oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault().TotalQueries + 1);

                            //Period Upsert
                            oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().PeriodPublicId = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.PeriodoUpsert(
                                oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault());
                        }
                        }
                        else
                        {
                            TDQueryModel oQueryToCreate = new TDQueryModel()
                            {
                                IsSuccess = false,
                                PeriodPublicId = oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                                SearchType = new TDCatalogModel()
                                {
                                    ItemId = (int)enumThirdKnowledgeQueryType.Simple,
                                },
                                User = SessionModel.CurrentLoginUser.Email,
                            };
                        }
                    }

                    #endregion Upsert Process
                }
                else
                {
                    oModel.RelatedThirdKnowledge.HasPlan = false;
                }
                return oModel;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpPost]
        [HttpGet]
        public FileModel TKLoadFile(string TKLoadFile, string CompanyPublicId, string PeriodPublicId)
        {
            FileModel oReturn = new FileModel();

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Length > 0)
            {
                //get folder
                string strFolder = System.Web.HttpContext.Current.Server.MapPath
                    (Models.General.InternalSettings.Instance
                    [Models.General.Constants.C_Settings_File_TempDirectory].Value);

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                //get File
                var UploadFile = System.Web.HttpContext.Current.Request.Files["ThirdKnowledge_FileUpload"];

                if (UploadFile != null && !string.IsNullOrEmpty(UploadFile.FileName))
                {
                    string oFileName = "ThirdKnowledgeFile_" +
                            CompanyPublicId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." +
                        UploadFile.FileName.Split('.').DefaultIfEmpty("xls").LastOrDefault();
                    string strFile = strFolder.TrimEnd('\\') +
                    "\\ThirdKnowledgeFile_" +
                        CompanyPublicId + "_" + oFileName;

                    UploadFile.SaveAs(strFile);

                    Tuple<bool, string> oVerifyResult = this.FileVerify(strFile, oFileName, PeriodPublicId);
                    bool isValidFile = oVerifyResult.Item1;

                    string strRemoteFile = string.Empty;
                    if (isValidFile)
                    {
                        //load file to s3
                        strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                            (strFile,
                            Models.General.InternalSettings.Instance
                                [Models.General.Constants.C_Settings_File_RemoteDirectory].Value.TrimEnd('\\') +
                                 CompanyPublicId + "_" + DateTime.Now + "\\");

                        TDQueryModel oQueryToCreate = new TDQueryModel()
                        {
                            IsSuccess = isValidFile,
                            PeriodPublicId = PeriodPublicId,
                            QueryStatus = new TDCatalogModel()
                            {
                                ItemId = (int)enumThirdKnowledgeQueryStatus.InProcess
                            },
                            SearchType = new TDCatalogModel()
                            {
                                ItemId = (int)enumThirdKnowledgeQueryType.Masive,
                            },
                            User = SessionModel.CurrentLoginUser.Email,
                            RelatedQueryInfoModel = new List<TDQueryInfoModel>(),
                        };
                        oQueryToCreate.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                        {
                            ItemInfoType = new TDCatalogModel()
                            {
                                ItemId = (int)enumThirdKnowledgeColls.FileURL,
                                ItemName = strRemoteFile,
                            },
                            Value = oFileName,
                            LargeValue = strRemoteFile,
                        });

                        oQueryToCreate = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.QueryUpsert(oQueryToCreate);

                        //Send Message
                        MessageModule.Client.Models.NotificationModel oDataMessage = new MessageModule.Client.Models.NotificationModel();
                        oDataMessage.CompanyPublicId = CompanyPublicId;
                        oDataMessage.User = SessionModel.CurrentLoginUser.Email;
                        oDataMessage.CompanyLogo = SessionModel.CurrentCompany_CompanyLogo;
                        oDataMessage.CompanyName = SessionModel.CurrentCompany.CompanyName;
                        oDataMessage.IdentificationType = SessionModel.CurrentCompany.IdentificationType.ItemName;
                        oDataMessage.IdentificationNumber = SessionModel.CurrentCompany.IdentificationNumber;

                        #region Notification

                        oDataMessage.Label = Models.General.InternalSettings.Instance
                                [Models.General.Constants.N_ThirdKnowledgeUploadMassiveMessage].Value;
                        oDataMessage.Url = Models.General.InternalSettings.Instance
                                [Models.General.Constants.N_UrlThirdKnowledgeQuery].Value.Replace("{{QueryPublicId}}", oQueryToCreate.QueryPublicId);
                        oDataMessage.NotificationType = (int)MarketPlace.Models.General.enumNotificationType.ThirdKnowledgeNotification;
                        oDataMessage.Enable = true;

                        #endregion

                        ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.CreateUploadNotification(oDataMessage);
                    }

                    //remove temporal file
                    if (System.IO.File.Exists(strFile))
                        System.IO.File.Delete(strFile);

                    oReturn = new FileModel()
                    {
                        FileName = UploadFile.FileName,
                        ServerUrl = strRemoteFile,
                        LoadMessage = isValidFile ? "El Archivo " + UploadFile.FileName + " es correto, en unos momentos recibirá un correo con el respectivo resultado de la validación." :
                                                    "El Archivo " + UploadFile.FileName + " no es correto, por favor verifique el nombre de las columnas y el formato.",
                        AdditionalInfo = oVerifyResult.Item2,
                    };
                }
            }
            return oReturn;
        }

        #region ThirdKnowledge Charts

        [HttpPost]
        [HttpGet]
        public List<Tuple<string, int, int>> GetPeriodsByPlan(string GetPeriodsByPlan)
        {
            //Get Charts By Module
            List<GenericChartsModel> oResult = new List<GenericChartsModel>();
            GenericChartsModel oRelatedChart = null;

            oRelatedChart = new GenericChartsModel()
            {
                ChartModuleType = ((int)enumCategoryInfoType.CH_ThirdKnowledgeModule).ToString(),
                GenericChartsInfoModel = new List<GenericChartsModelInfo>(),
            };

            List<ProveedoresOnLine.ThirdKnowledge.Models.PlanModel> oPlanModel = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetAllPlanByCustomer(SessionModel.CurrentCompany.CompanyPublicId, true);

            List<Tuple<string, int, int>> oReturn = new List<Tuple<string, int, int>>();
            if (oPlanModel != null)
            {
                oPlanModel.All(x =>
                {
                    x.RelatedPeriodModel.All(y =>
                    {
                        oReturn.Add(Tuple.Create(y.InitDate.ToString("dd/MM/yy") + " - " + y.EndDate.ToString("dd/MM/yy")
                            , y.TotalQueries, y.AssignedQueries));
                        return true;
                    });
                    return true;
                });
            }

            return oReturn;
        }

        #endregion ThirdKnowledge Charts

        #region Private Functions

        [HttpPost]
        [HttpGet]
        public Tuple<bool, string> FileVerify(string FilePath, string FileName, string PeriodPublicId)
        {
            var Excel = new FileInfo(FilePath);
            List<PlanModel> oCurrentPeriodList = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetCurrenPeriod(SessionModel.CurrentCompany.CompanyPublicId, true);
            using (var package = new ExcelPackage(Excel))
            {
                // Get the work book in the file
                ExcelWorkbook workBook = package.Workbook;
                                
                if (workBook != null)
                {                    
                    object[,] values = (object[,])workBook.Worksheets.First().Cells["A1:C1"].Value;

                    string UncodifiedObj = new JavaScriptSerializer().Serialize(values);
                    if (UncodifiedObj.Contains(Models.General.InternalSettings.Instance
                                    [Models.General.Constants.MP_CP_ColPersonType].Value)
                        && UncodifiedObj.Contains(Models.General.InternalSettings.Instance
                                    [Models.General.Constants.MP_CP_ColIdNumber].Value)
                        && UncodifiedObj.Contains(Models.General.InternalSettings.Instance
                                    [Models.General.Constants.MP_CP_ColIdName].Value))
                    {                        
                        bool isLoaded = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.AccessFTPClient(FileName, FilePath, PeriodPublicId);
                        if (isLoaded)
                        {
                            //Get The Active Plan By Customer                            
                            oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault().TotalQueries += (workBook.Worksheets[1].Dimension.End.Row - 1);
                            ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.PeriodoUpsert(oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault());
                        }

                        return new Tuple<bool, string>(true, oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault().TotalQueries.ToString());
                    }
                    else
                    {
                        return new Tuple<bool, string>(true, oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault().TotalQueries.ToString());
                    }
                }
            }
            return new Tuple<bool, string>(true, oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault().TotalQueries.ToString()); ;
        }


        public List<Tuple<string, string>> SetCollumnsResult(TDQueryModel oCollumnsResult)
        {
            List<Tuple<string, string>> oReturn = new List<Tuple<string, string>>();

            if (oCollumnsResult != null && oCollumnsResult.RelatedQueryInfoModel != null)
            {
                oCollumnsResult.RelatedQueryInfoModel.All(inf =>
                    {
                        #region Set Collumns
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.RequestName)
                        {
                            oReturn.Add(new Tuple<string, string>("Nombre Consultado", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdNumberRequest)
                        {
                            oReturn.Add(new Tuple<string, string>("Identificación Consultada", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.QueryId)
                        {
                            oReturn.Add(new Tuple<string, string>("Id Consulta", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Priotity)
                        {
                            oReturn.Add(new Tuple<string, string>("Prioridad", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.TypeDocument)
                        {
                            oReturn.Add(new Tuple<string, string>("Tipo De Documento", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdNumberResult)
                        {
                            oReturn.Add(new Tuple<string, string>("númer de Identificación", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.NameResult)
                        {
                            oReturn.Add(new Tuple<string, string>("Nombre Completo", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.ListName)
                        {
                            oReturn.Add(new Tuple<string, string>("Nombre Lista", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Alias)
                        {
                            oReturn.Add(new Tuple<string, string>("Alias", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Offense)
                        {
                            oReturn.Add(new Tuple<string, string>("Cargo o Delito", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Peps)
                        {
                            oReturn.Add(new Tuple<string, string>("Peps", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Status)
                        {
                            oReturn.Add(new Tuple<string, string>("Estado", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Zone)
                        {
                            oReturn.Add(new Tuple<string, string>("Zona", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.MoreInfo)
                        {
                            oReturn.Add(new Tuple<string, string>("Otra Información", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.RegisterDate)
                        {
                            oReturn.Add(new Tuple<string, string>("Fecha de Registro", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.LastModifyDate)
                        {
                            oReturn.Add(new Tuple<string, string>("Fecha Última Modificación", inf.Value));
                        }
                        if (inf.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Link)
                        {
                            oReturn.Add(new Tuple<string, string>("Link", inf.Value));
                        }   
                        #endregion                      
                        return true;
                    });
            }
            return oReturn;
        }
        #endregion Private Functions
    }
}
