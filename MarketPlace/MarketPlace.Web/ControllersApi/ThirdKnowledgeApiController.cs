using MarketPlace.Models.Company;
using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using NetOffice.ExcelApi;
using NetOffice.ExcelApi.Enums;
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
                            };
                            oModel.RelatedThidKnowledgeSearch = new ThirdKnowledgeViewModel();
                            oModel.RelatedThidKnowledgeSearch.CollumnsResult = new TDQueryModel();

                            //Get Reqsult
                            oModel.RelatedThidKnowledgeSearch.CollumnsResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.SimpleRequest(oCurrentPeriodList.FirstOrDefault().
                                            RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                                           System.Web.HttpContext.Current.Request["IdentificationNumber"], System.Web.HttpContext.Current.Request["Name"], oQueryToCreate);
                            //Init Finally Tuple, Group by ItemGroup Name
                            List<Tuple<string, List<TDQueryInfoModel>>> Group = new List<Tuple<string, List<TDQueryInfoModel>>>();
                            List<string> Item1 = new List<string>();
                            if (oModel.RelatedThidKnowledgeSearch.CollumnsResult != null && oModel.RelatedThidKnowledgeSearch.CollumnsResult.IsSuccess)
                            {
                                oModel.RelatedThidKnowledgeSearch.CollumnsResult.RelatedQueryBasicInfoModel.All(x =>
                                {
                                    Item1.Add(x.DetailInfo.Where(y => y.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.GroupName).Select(y => y.Value).FirstOrDefault());
                                    return true;
                                });
                                Item1 = Item1.GroupBy(x => x).Select(grp => grp.Last()).ToList();

                                List<TDQueryInfoModel> oItem2 = new List<TDQueryInfoModel>();
                                Tuple<string, List<TDQueryInfoModel>> oTupleItem = new Tuple<string, List<TDQueryInfoModel>>("", new List<TDQueryInfoModel>());

                                Item1.All(x =>
                                {
                                    if (oModel.RelatedThidKnowledgeSearch.CollumnsResult.RelatedQueryBasicInfoModel.Where(td => td.DetailInfo.Any(inf => inf.Value == x)) != null)
                                    {
                                        oItem2 = oModel.RelatedThidKnowledgeSearch.CollumnsResult.RelatedQueryBasicInfoModel.Where(td => td.DetailInfo.Any(inf => inf.Value == x)).Select(td => td).ToList();
                                        oTupleItem = new Tuple<string, List<TDQueryInfoModel>>(x, oItem2);
                                        Group.Add(oTupleItem);
                                    }
                                    return true;
                                });
                                if (Group != null)                                
                                    oModel.RelatedSingleSearch = Group;
                                

                                if (oModel.RelatedThidKnowledgeSearch.CollumnsResult.QueryPublicId != null)
                                {
                                    //Set New Score
                                    oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().TotalQueries = (oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault().TotalQueries + 1);

                                    //Period Upsert
                                    oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().PeriodPublicId = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.PeriodoUpsert(
                                        oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault());
                                }
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
        public ProviderViewModel TKSingleDetail(string QueryPublicId)
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
                            };
                            oModel.RelatedThidKnowledgeSearch = new ThirdKnowledgeViewModel();
                            oModel.RelatedThidKnowledgeSearch.CollumnsResult = new TDQueryModel();
                            oModel.RelatedThidKnowledgeSearch.CollumnsResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.SimpleRequest(oCurrentPeriodList.FirstOrDefault().
                                            RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                                           System.Web.HttpContext.Current.Request["IdentificationNumber"], System.Web.HttpContext.Current.Request["Name"], oQueryToCreate);
                            if (oModel.RelatedThidKnowledgeSearch.CollumnsResult.IsSuccess == true)
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
                        UploadFile.FileName.Split('.').DefaultIfEmpty("xlsx").LastOrDefault();
                    oFileName = oFileName.Split('.').LastOrDefault() == "xls" ? oFileName.Replace("xls", "xlsx") : oFileName;
                    string strFilePath = strFolder.TrimEnd('\\') + "\\" + oFileName;

                    UploadFile.SaveAs(strFilePath);

                    Tuple<bool, string> oVerifyResult = this.FileVerify(strFilePath, oFileName, PeriodPublicId);
                    bool isValidFile = oVerifyResult.Item1;

                    string strRemoteFile = string.Empty;
                    if (isValidFile)
                    {
                        //load file to s3
                        strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                            (strFilePath,
                            Models.General.InternalSettings.Instance[Models.General.Constants.C_Settings_File_ThirdKnowledgeRemoteDirectory].Value);

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
                            FileName = oFileName,
                        };

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
                    if (System.IO.File.Exists(strFilePath))
                        System.IO.File.Delete(strFilePath);

                    oReturn = new FileModel()
                    {
                        FileName = UploadFile.FileName,
                        ServerUrl = strRemoteFile,
                        LoadMessage = isValidFile ? "El Archivo " + UploadFile.FileName + " es correcto, en unos momentos recibirá un correo con el respectivo resultado de la validación." :
                                                    "El Archivo " + UploadFile.FileName + " no es correcto, por favor verifique el nombre de las columnas y el formato.",
                        AdditionalInfo = oVerifyResult.Item2,
                    };
                }
            }
            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public ProviderViewModel TKThirdKnowledgeDetail(
              string TKThirdKnowledgeDetail
            , string QueryPublicId
            , string InitDate
            , string EndDate
            , string Enable
            , string IsSuccess
            , int PageNumber
            ) {

                ProviderViewModel oModel = new ProviderViewModel();
                oModel.RelatedThidKnowledgeSearch = new ThirdKnowledgeViewModel();
                oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult = new List<TDQueryModel>();
                int TotalRows = 0;
                List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryModel> oQueryResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.ThirdKnowledgeSearchByPublicId
                    (SessionModel.CurrentCompany.CompanyPublicId
                    , QueryPublicId
                    , Enable == "1" ? true : false
                    , PageNumber
                    , Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim())
                    , out TotalRows
                    );
                oModel.RelatedThidKnowledgeSearch.TotalRows = TotalRows;
                oModel.RelatedThidKnowledgeSearch.TotalPages = (int)Math.Ceiling((decimal)((decimal)oModel.RelatedThidKnowledgeSearch.TotalRows / (decimal)oModel.RelatedThidKnowledgeSearch.RowCount));

                if (oQueryResult != null && oQueryResult.Count > 0)
                    oModel.RelatedThidKnowledgeSearch.ThirdKnowledgeResult = oQueryResult;
                else if (IsSuccess == "Finalizado")
                    oModel.RelatedThidKnowledgeSearch.Message = "La búsqueda no arrojó resultados.";

                if (!string.IsNullOrEmpty(InitDate) && !string.IsNullOrEmpty(EndDate))
                {
                    oModel.RelatedThidKnowledgeSearch.InitDate = Convert.ToDateTime(InitDate);
                    oModel.RelatedThidKnowledgeSearch.EndDate = Convert.ToDateTime(EndDate);
                }

            return oModel;
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

        #endregion Private Functions
    }
}
