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
        {
            try
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
                            oQuery.RelatedQueryInfoModel.FirstOrDefault().Value = oQuery.RelatedQueryInfoModel.FirstOrDefault().Value.Replace(oQuery.RelatedQueryInfoModel.FirstOrDefault().Value.Split('.').LastOrDefault(), "xml");

                            string uri = "ftp://" + ftpServerIP + "/" + uploadToFolder + "/" + "Res_" + oQuery.RelatedQueryInfoModel.FirstOrDefault().Value;
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

                            //Set results to model
                            CurrentXMLAnswer.Descendants("Resultados").All(
                                x =>
                                {
                                    #region QueryInfo
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                                                    {
                                                                        QueryPublicId = oQuery.QueryPublicId,
                                                                        ItemInfoType = new TDCatalogModel()
                                                                        {
                                                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.QueryId,
                                                                        },
                                                                        Value = x.Element("NumeroConsulta").Value,
                                                                        Enable = true,
                                                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.IdNumberRequest,
                                        },
                                        Value = x.Element("IdentificacionConsulta").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.RequestName,
                                        },
                                        Value = x.Element("NombreConsulta").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.GroupNumber,
                                        },
                                        Value = x.Element("IdGrupoLista").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.GroupName,
                                        },
                                        Value = x.Element("NombreGrupoLista").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.Priotity,
                                        },
                                        Value = x.Element("Prioridad").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.TypeDocument,
                                        },
                                        Value = x.Element("TipoDocumento").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.IdNumberResult,
                                        },
                                        Value = x.Element("DocumentoIdentidad").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.NameResult,
                                        },
                                        Value = x.Element("NombreCompleto").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.IdList,
                                        },
                                        Value = x.Element("IdTipoLista").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.ListName,
                                        },
                                        Value = x.Element("NombreTipoLista").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.Alias,
                                        },
                                        Value = x.Element("Alias").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.Offense,
                                        },
                                        Value = x.Element("CargoDelito").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.Peps,
                                        },
                                        Value = x.Element("Peps").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.Zone,
                                        },
                                        Value = x.Element("Zona").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.Link,
                                        },
                                        Value = x.Element("Link").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.MoreInfo,
                                        },
                                        Value = x.Element("OtraInformacion").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.RegisterDate,
                                        },
                                        Value = x.Element("FechaRegistro").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.LastModifyDate,
                                        },
                                        Value = x.Element("FechaActualizacion").Value,
                                        Enable = true,
                                    });
                                    oQuery.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                                    {
                                        QueryPublicId = oQuery.QueryPublicId,
                                        ItemInfoType = new TDCatalogModel()
                                        {
                                            ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeColls.Status,
                                        },
                                        Value = x.Element("Estado").Value,
                                        Enable = true,
                                    });
                                    #endregion
                                    return true;
                                });

                            //Update Status query
                            oQuery.QueryStatus = new TDCatalogModel()
                            {
                                ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeQueryStatus.Finalized,
                            };

                            //TODO: Acá va la notificacion de respuesta DAVID
                            //TODO: Acá va el envio del Email JOSE
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
    }
}
