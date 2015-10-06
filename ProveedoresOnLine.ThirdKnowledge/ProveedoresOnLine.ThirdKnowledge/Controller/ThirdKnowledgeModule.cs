using ProveedoresOnLine.ThirdKnowledge.DAL.Controller;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProveedoresOnLine.ThirdKnowledge.Controller
{
    public class ThirdKnowledgeModule
    {
        public static TDQueryModel SimpleRequest(string PeriodPublicId, string IdentificationNumber, string Name, TDQueryModel oQueryToCreate)
        {
            try
            {
                #region Set User Service

                WS_Inspekt.Autenticacion oAuth = new WS_Inspekt.Autenticacion();
                WS_Inspekt.WSInspektorSoapClient oClient = new WS_Inspekt.WSInspektorSoapClient();

                #endregion Set User Service

                List<PlanModel> oPlanModel = new List<PlanModel>();
                PeriodModel oCurrentPeriod = new PeriodModel();

                oAuth.UsuarioNombre = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_AuthServiceUser].Value;
                oAuth.UsuarioClave = "D6-E9$C3S6Q#5WW&5@";

                //WS Request
                var Result = oClient.ConsultaInspektor(oAuth, IdentificationNumber, Name);

                oQueryToCreate.RelatedQueryBasicInfoModel = new List<TDQueryInfoModel>();

                if (Result != null && Result.FirstOrDefault().IdConsulta != "No existen registros asociados a los parámetros de consulta.")
                {
                    #region Answer Procces
                    Result.All(x =>
                                   {
                                       if (x != null)
                                       {
                                           TDQueryInfoModel oInfoCreate = new TDQueryInfoModel();
                                           oInfoCreate.Alias = x.Alias;
                                           oInfoCreate.IdentificationResult = x.DocumentoIdentidad;
                                           oInfoCreate.Offense = x.CargoDelito;
                                           oInfoCreate.NameResult = x.NombreCompleto;
                                           oInfoCreate.Peps = x.Peps;
                                           oInfoCreate.Priority = x.Prioridad;
                                           oInfoCreate.Status = x.Estado;
                                           oInfoCreate.Enable = true;
                                           oInfoCreate.QueryPublicId = oQueryToCreate.QueryPublicId;
                                           oInfoCreate.DetailInfo = new List<TDQueryDetailInfoModel>();

                                           #region Create Detail
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                                                              {
                                                                                  ItemInfoType = new TDCatalogModel()
                                                                                  {
                                                                                      ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.IdNumberRequest,
                                                                                  },
                                                                                  Value = !string.IsNullOrEmpty(IdentificationNumber) ? IdentificationNumber : string.Empty,
                                                                                  Enable = true,
                                                                              });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Alias,
                                               },
                                               Value = !string.IsNullOrEmpty(x.Alias) ? x.Alias : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Offense,
                                               },
                                               Value = !string.IsNullOrEmpty(x.CargoDelito) ? x.CargoDelito : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.IdentificationNumberResult,
                                               },
                                               Value = !string.IsNullOrEmpty(x.DocumentoIdentidad) ? x.DocumentoIdentidad : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Status,
                                               },
                                               Value = !string.IsNullOrEmpty(x.Estado) ? x.Estado : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.LastModifyDate,
                                               },
                                               Value = !string.IsNullOrEmpty(x.FechaActualizacion) ? x.FechaActualizacion : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.QueryId,
                                               },
                                               Value = !string.IsNullOrEmpty(x.IdConsulta) ? x.IdConsulta : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.GroupName,
                                               },
                                               Value = !string.IsNullOrEmpty(x.NombreGrupo) ? x.NombreGrupo : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.IdList,
                                               },
                                               Value = !string.IsNullOrEmpty(x.IdLista) ? x.IdLista : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Link,
                                               },
                                               Value = !string.IsNullOrEmpty(x.Link) ? x.Link : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.NameResult,
                                               },
                                               Value = !string.IsNullOrEmpty(x.NombreCompleto) ? x.NombreCompleto : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.ListName,
                                               },
                                               Value = !string.IsNullOrEmpty(x.NombreTipoLista) ? x.NombreTipoLista : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.MoreInfo,
                                               },
                                               Value = !string.IsNullOrEmpty(x.OtraInformacion) ? x.OtraInformacion : string.Empty,
                                               Enable = true,
                                           });
                                           oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                           {
                                               ItemInfoType = new TDCatalogModel()
                                               {
                                                   ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Zone,
                                               },
                                               Value = !string.IsNullOrEmpty(x.Zona) ? x.Zona : string.Empty,
                                               Enable = true,
                                           });
                                           #endregion

                                           oQueryToCreate.RelatedQueryBasicInfoModel.Add(oInfoCreate);
                                       }
                                       return true;
                                   });
                    #endregion
                    oQueryToCreate.IsSuccess = true;
                    QueryUpsert(oQueryToCreate);
                }
                else
                {
                    oQueryToCreate.IsSuccess = false;
                    QueryUpsert(oQueryToCreate);
                }

                return oQueryToCreate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Config

        public static PlanModel PlanUpsert(PlanModel oPlanModelToUpsert)
        {
            //LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
            try
            {
                if (oPlanModelToUpsert != null)
                {
                    oPlanModelToUpsert.PlanPublicId = ThirdKnowledgeDataController.Instance.PlanUpsert
                                                      (oPlanModelToUpsert.PlanPublicId,
                                                      oPlanModelToUpsert.CompanyPublicId,
                                                      oPlanModelToUpsert.QueriesByPeriod,
                                                      oPlanModelToUpsert.DaysByPeriod,
                                                      oPlanModelToUpsert.Status,
                                                      oPlanModelToUpsert.InitDate,
                                                      oPlanModelToUpsert.EndDate,
                                                      oPlanModelToUpsert.Enable);
                    oPlanModelToUpsert.RelatedPeriodModel = CalculatePeriods(oPlanModelToUpsert);
                }
                return oPlanModelToUpsert;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<PlanModel> GetAllPlanByCustomer(string CustomerPublicId, bool Enable)
        {
            return ThirdKnowledgeDataController.Instance.GetAllPlanByCustomer(CustomerPublicId, Enable);
        }

        public static List<PeriodModel> GetPeriodByPlanPublicId(string PeriodPublicId, bool Enable)
        {
            return ThirdKnowledgeDataController.Instance.GetPeriodByPlanPublicId(PeriodPublicId, Enable);
        }

        public static List<TDQueryModel> GetQueriesByPeriodPublicId(string PeriodPublicId, bool Enable)
        {
            return ThirdKnowledgeDataController.Instance.GetQueriesByPeriodPublicId(PeriodPublicId, Enable);
        }

        #endregion Config

        #region MarketPlace

        public static List<PeriodModel> CalculatePeriods(PlanModel oPlanToReCalculate)
        {
            int DiferenceInDays;
            int TotalPeriods = 0;

            oPlanToReCalculate.RelatedPeriodModel = ThirdKnowledgeDataController.Instance.GetPeriodByPlanPublicId(oPlanToReCalculate.PlanPublicId, true);

            if (oPlanToReCalculate.PlanPublicId != null &&
                oPlanToReCalculate.RelatedPeriodModel != null &&
                oPlanToReCalculate.RelatedPeriodModel.Count > 0)
            {
                oPlanToReCalculate.RelatedPeriodModel.All(x =>
                    {
                        ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.PeriodUpsert(
                                                            x.PeriodPublicId,
                                                            oPlanToReCalculate.PlanPublicId,
                                                            oPlanToReCalculate.QueriesByPeriod,
                                                            x.TotalQueries,
                                                            x.InitDate,
                                                            x.EndDate,
                                                            oPlanToReCalculate.Enable);
                        return true;
                    });
            }
            else
            {
                if (oPlanToReCalculate != null)
                {
                    //Get Days from dates interval
                    DiferenceInDays = (oPlanToReCalculate.EndDate - oPlanToReCalculate.InitDate).Days;

                    TotalPeriods = DiferenceInDays / oPlanToReCalculate.DaysByPeriod;
                    oPlanToReCalculate.RelatedPeriodModel = new List<PeriodModel>();
                }

                DateTime EndPastPeriod = new DateTime();
                for (int i = 0; i < TotalPeriods; i++)
                {
                    if (i == 0)
                    {
                        oPlanToReCalculate.RelatedPeriodModel.Add(new PeriodModel()
                        {
                            AssignedQueries = oPlanToReCalculate.QueriesByPeriod,
                            InitDate = oPlanToReCalculate.InitDate,
                            EndDate = oPlanToReCalculate.InitDate.AddDays(oPlanToReCalculate.DaysByPeriod),
                            CreateDate = DateTime.Now,
                            LastModify = DateTime.Now,
                            PlanPublicId = oPlanToReCalculate.PlanPublicId,
                            TotalQueries = 0,
                        });
                        EndPastPeriod = oPlanToReCalculate.InitDate.AddDays(oPlanToReCalculate.DaysByPeriod);
                    }
                    else
                    {
                        oPlanToReCalculate.RelatedPeriodModel.Add(new PeriodModel()
                        {
                            AssignedQueries = oPlanToReCalculate.QueriesByPeriod,
                            InitDate = EndPastPeriod,
                            EndDate = i == TotalPeriods ? oPlanToReCalculate.EndDate : EndPastPeriod.AddDays(oPlanToReCalculate.DaysByPeriod),
                            CreateDate = DateTime.Now,
                            LastModify = DateTime.Now,
                            PlanPublicId = oPlanToReCalculate.PlanPublicId,
                            TotalQueries = 0,
                        });
                        EndPastPeriod = EndPastPeriod.AddDays(oPlanToReCalculate.DaysByPeriod);
                    }
                }
                oPlanToReCalculate.RelatedPeriodModel.All(x =>
                {
                    ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.PeriodUpsert(
                                                            x.PeriodPublicId,
                                                            x.PlanPublicId,
                                                            x.AssignedQueries,
                                                            x.TotalQueries,
                                                            x.InitDate,
                                                            x.EndDate,
                                                            oPlanToReCalculate.Enable);
                    return true;
                });
            }
            return oPlanToReCalculate.RelatedPeriodModel;
        }

        public static List<TDCatalogModel> CatalogGetThirdKnowledgeOptions()
        {
            return ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.CatalogGetThirdKnowledgeOptions();
        }

        public static List<Models.PlanModel> GetCurrenPeriod(string CustomerPublicId, bool Enable)
        {
            return ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.GetCurrenPeriod(CustomerPublicId, Enable);
        }

        public static List<Models.TDQueryDetailInfoModel> GetQueryDetail(string QueryBasicPublicId)
        {
            return ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.GetQueryDetail(QueryBasicPublicId);
        }

        public static string PeriodoUpsert(PeriodModel oPeriodModel)
        {
            return ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.PeriodUpsert(oPeriodModel.PeriodPublicId,
                       oPeriodModel.PlanPublicId, oPeriodModel.AssignedQueries, oPeriodModel.TotalQueries, oPeriodModel.InitDate, oPeriodModel.EndDate, oPeriodModel.Enable);
        }

        public static List<Models.TDQueryModel> ThirdKnowledgeSearch(string CustomerPublicId, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            return ThirdKnowledgeDataController.Instance.ThirdKnowledgeSearch(CustomerPublicId, SearchOrderType, OrderOrientation, PageNumber, RowCount, out TotalRows);
        }

        public static List<Models.TDQueryModel> ThirdKnowledgeSearchByPublicId(string CustomerPublicId, string QueryPublic, bool Enable)
        {
            return ThirdKnowledgeDataController.Instance.ThirdKnowledgeSearchByPublicId(CustomerPublicId, QueryPublic, Enable);
        }

        public static bool AccessFTPClient(string FileName, string FilePath, string PeriodPublicId)
        {
            string ftpServerIP = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_FTPServerIP].Value;
            string uploadToFolder = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_UploadFTPFileName].Value;
            string UserName = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_FTPUserName].Value;
            string UserPass = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_FTPPassworUser].Value;

            FileInfo FileInf = new FileInfo(FilePath);

            string uri = "ftp://" + ftpServerIP + "/" + uploadToFolder + "/" + FileInf.Name;

            FtpWebRequest request = ((FtpWebRequest)FtpWebRequest.Create(new Uri(uri)));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(UserName, UserPass, ftpServerIP);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;
            request.ContentLength = FileInf.Length;

            int buffLength = 64000;
            byte[] buff = new byte[buffLength];
            int contentLen;

            FileStream fs = FileInf.OpenRead();
            try
            {
                Stream strm = request.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                strm.Close();
                fs.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        #endregion MarketPlace

        #region Queries

        public static TDQueryModel QueryUpsert(TDQueryModel QueryModelToUpsert)
        {
            if (QueryModelToUpsert != null &&
                !string.IsNullOrEmpty(QueryModelToUpsert.PeriodPublicId))
            {
                QueryModelToUpsert.QueryPublicId = ThirdKnowledgeDataController.Instance.QueryUpsert(QueryModelToUpsert.QueryPublicId, QueryModelToUpsert.PeriodPublicId,
                    QueryModelToUpsert.SearchType.ItemId, QueryModelToUpsert.User, QueryModelToUpsert.FileName, QueryModelToUpsert.IsSuccess, QueryModelToUpsert.QueryStatus.ItemId, true);

                if (QueryModelToUpsert.RelatedQueryBasicInfoModel != null)
                {
                    QueryModelToUpsert.RelatedQueryBasicInfoModel.All(qInf =>
                    {
                        LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                        try
                        {
                            qInf.QueryBasicPublicId =
                               ThirdKnowledgeDataController.Instance.QueryBasicInfoInsert
                                (QueryModelToUpsert.QueryPublicId,
                                qInf.NameResult, qInf.IdentificationResult, qInf.Priority, qInf.Peps, qInf.Status, qInf.Alias
                                , qInf.Offense, true);

                            if (qInf.DetailInfo != null)
                            {
                                qInf.DetailInfo.All(det =>
                                {
                                    ThirdKnowledgeDataController.Instance.QueryDetailInfoInsert(qInf.QueryBasicPublicId, det.ItemInfoType.ItemId, det.Value, det.LargeValue, det.Enable);
                                    return true;
                                });
                            }
                            oLog.IsSuccess = true;
                        }
                        catch (Exception err)
                        {
                            oLog.IsSuccess = false;
                            oLog.Message = err.Message + " - " + err.StackTrace;

                            throw err;
                        }
                        finally
                        {
                            oLog.LogObject = qInf;

                            oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                            {
                                LogInfoType = "PeriodPublicId",
                                Value = QueryModelToUpsert.PeriodPublicId,
                            });

                            LogManager.ClientLog.AddLog(oLog);
                        }
                        return true;
                    });
                }
            }

            return QueryModelToUpsert;
        }

        public static TDQueryInfoModel QueryDetailGetByBasicPublicID(string QueryBasicInfoPublicId)
        {
            return ThirdKnowledgeDataController.Instance.QueryDetailGetByBasicPublicID(QueryBasicInfoPublicId);
        }
        #endregion Queries

        #region BatchProcess

        public static List<TDQueryModel> GetQueriesInProgress()
        {
            return ThirdKnowledgeDataController.Instance.GetQueriesInProgress();
        }

        #endregion

        #region Messenger

        public static void CreateUploadNotification(MessageModule.Client.Models.NotificationModel DataMessage)
        {
            try
            {
                #region Email

                //Create message object
                MessageModule.Client.Models.ClientMessageModel oMessageToSend = new MessageModule.Client.Models.ClientMessageModel()
                {
                    Agent = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_TK_UploadSuccessFileAgent].Value,
                    User = DataMessage.User,
                    ProgramTime = DateTime.Now,
                    MessageQueueInfo = new List<Tuple<string, string>>(),
                };

                oMessageToSend.MessageQueueInfo.Add(new Tuple<string, string>("To", DataMessage.User));

                //get customer info
                oMessageToSend.MessageQueueInfo.Add(new Tuple<string, string>
                    ("CustomerLogo", DataMessage.CompanyLogo));

                oMessageToSend.MessageQueueInfo.Add(new Tuple<string, string>
                    ("CustomerName", DataMessage.CompanyName));

                oMessageToSend.MessageQueueInfo.Add(new Tuple<string, string>
                    ("CustomerIdentificationTypeName", DataMessage.IdentificationType));

                oMessageToSend.MessageQueueInfo.Add(new Tuple<string, string>
                    ("CustomerIdentificationNumber", DataMessage.IdentificationNumber));

                MessageModule.Client.Controller.ClientController.CreateMessage(oMessageToSend);

                #endregion

                #region Notification

                DataMessage.NotificationId = MessageModule.Client.Controller.ClientController.NotificationUpsert(DataMessage);

                #endregion
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}