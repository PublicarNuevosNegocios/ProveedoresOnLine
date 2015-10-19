using ProveedoresOnLine.ThirdKnowledgeBatch.Models;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace ProveedoresOnLine.ThirdKnowledgeBatch
{
    public class ThirdKnowledgeFTPProcess
    {
        public static void StartProcess()
        {try
            {
                //Get queries to process
                List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryModel> oQueryResult = new List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryModel>();
                oQueryResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetQueriesInProgress();
                if (oQueryResult != null)
                {
                    //Set access
                    string ftpServerIP = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_FTPServerIP].Value;
                    string uploadToFolder = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_UploadFTPFileName].Value;
                    string UserName = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_FTPUserName].Value;
                    string UserPass = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_FTPPassworUser].Value;
                    
                    oQueryResult.All(oQuery =>
                    {
                        try
                        {
                            oQuery.FileName = oQuery.FileName.Replace(oQuery.FileName.Split('.').LastOrDefault(), "xml");

                            string uri = "ftp://" + ftpServerIP + "/" + uploadToFolder + "/" + "Res_" + oQuery.FileName;
                            byte[] buffer = new byte[1024];

                            FtpWebRequest request = ((FtpWebRequest)WebRequest.Create(new Uri(uri)));
                            request.Credentials = new NetworkCredential(UserName, UserPass, ftpServerIP);
                            request.UsePassive = true;
                            request.UseBinary = true;
                            request.KeepAlive = true;
                            request.Method = WebRequestMethods.Ftp.DownloadFile;
                            //Connect to ftp
                            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                            //Download xml result
                            Stream responseStream = response.GetResponseStream();
                            StreamReader reader = new StreamReader(responseStream);
                            string xml = reader.ReadToEnd();
                            XDocument CurrentXMLAnswer = XDocument.Parse(xml);
                            List<BatchXMLResultModel> oResult = new List<BatchXMLResultModel>();
                            oQuery.RelatedQueryBasicInfoModel = new List<TDQueryInfoModel>();
                            //Set results to model
                            CurrentXMLAnswer.Descendants("Resultado").All(
                                x =>
                                {
                                    if (x.Element("NumeroConsulta").Value != "No existen registros asociados a los parámetros de consulta.")
                                    {
                                        #region QueryInfo
                                        TDQueryInfoModel oInfoCreate = new TDQueryInfoModel();
                                        oInfoCreate.Alias = x.Element("Alias").Value;
                                        oInfoCreate.IdentificationResult = x.Element("DocumentoIdentidad").Value;
                                        oInfoCreate.NameResult = x.Element("NombreCompleto").Value;
                                        oInfoCreate.Offense = x.Element("CargoDelito").Value;
                                        oInfoCreate.Peps = x.Element("Peps").Value;
                                        oInfoCreate.Priority = x.Element("Prioridad").Value;
                                        oInfoCreate.Status = x.Element("Estado").Value;
                                        oInfoCreate.Enable = true;
                                        oInfoCreate.QueryPublicId = oQuery.QueryPublicId;
                                        oInfoCreate.DetailInfo = new List<TDQueryDetailInfoModel>();

                                        #region Detail Info
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.IdNumberRequest,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("IdentificacionConsulta").Value) ? x.Element("IdentificacionConsulta").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Alias,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("Alias").Value) ? x.Element("Alias").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Priotity,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("Prioridad").Value) ? x.Element("Alias").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.RegisterDate,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("FechaRegistro").Value) ? x.Element("Alias").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Offense,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("CargoDelito").Value) ? x.Element("CargoDelito").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.IdentificationNumberResult,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("DocumentoIdentidad").Value) ? x.Element("DocumentoIdentidad").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Status,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("Estado").Value) ? x.Element("Estado").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.LastModifyDate,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("FechaActualizacion").Value) ? x.Element("FechaActualizacion").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.QueryId,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("NumeroConsulta").Value) ? x.Element("NumeroConsulta").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.GroupName,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("NombreGrupoLista").Value) ? x.Element("NombreGrupoLista").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Link,
                                            },
                                            Value = !string.IsNullOrEmpty(x.Element("Link").Value) ? x.Element("Link").Value : string.Empty,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.NameResult,
                                            },
                                            Value = x.Element("NombreCompleto").Value,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.ListName,
                                            },
                                            Value = x.Element("NombreTipoLista").Value,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.MoreInfo,
                                            },
                                            Value = x.Element("OtraInformacion").Value,
                                            Enable = true,
                                        });
                                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                                        {
                                            ItemInfoType = new TDCatalogModel()
                                            {
                                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Zone,
                                            },
                                            Value = x.Element("Zona").Value,
                                            Enable = true,
                                        });
                                        #endregion

                                        oQuery.RelatedQueryBasicInfoModel.Add(oInfoCreate);
                                        #endregion
                                    }                                   
                                    return true;
                                });

                            //Update Status query
                            oQuery.QueryStatus = new TDCatalogModel()
                            {
                                ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeQueryStatus.Finalized,
                            };
                            
                            CreateReadyResultNotification(oQuery);
                            ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.QueryUpsert(oQuery);

                            LogFile("Success:: QueryPublicId '" + oQuery.QueryPublicId + "' :: Validation is success");
                        }
                        catch (Exception err)
                        {
                            LogFile("Error:: QueryPublicId '" + oQuery.QueryPublicId + "' :: " + err.Message);
                        }                      

                        return true;
                    });
                }
                else
                {
                    //log file
                    LogFile("End Process No Files to Vaildate");
                }
            }
            catch (Exception err)
            {
                //log file for fatal error
                LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }
        }

        #region Log File

        private static void LogFile(string LogMessage)
        {
            try
            {
                //get file Log
                string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                    System.Configuration.ConfigurationManager.AppSettings[ProveedoresOnLine.ThirdKnowledgeBacth.Models.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

                if (!System.IO.Directory.Exists(LogFile))
                    System.IO.Directory.CreateDirectory(LogFile);

                LogFile += "\\" + "Log_ThirdKnowledgeProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                using (System.IO.StreamWriter sw = System.IO.File.AppendText(LogFile))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "::" + LogMessage);
                    sw.Close();
                }
            }
            catch { }
        }

        #endregion

        #region Message

        public static void CreateReadyResultNotification(TDQueryModel oQuery)
        {
            #region Email
            //Create message object

            MessageModule.Client.Models.ClientMessageModel oMessageToSend = new MessageModule.Client.Models.ClientMessageModel()
            {
                Agent = ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_TK_ReadyResultAgent].Value,
                User = oQuery.User,
                ProgramTime = DateTime.Now,
                MessageQueueInfo = new List<Tuple<string, string>>(),
            };

            oMessageToSend.MessageQueueInfo.Add(new Tuple<string, string>("To", oQuery.User));

            oMessageToSend.MessageQueueInfo.Add(new Tuple<string, string>
                ("URLToRedirect", ThirdKnowledge.Models.InternalSettings.Instance[Constants.C_Settings_TK_QueryUrl].Value.Replace("{QueryPublicId}", oQuery.QueryPublicId)));


            MessageModule.Client.Controller.ClientController.CreateMessage(oMessageToSend);
            #endregion
        }

        #endregion
    }
}
