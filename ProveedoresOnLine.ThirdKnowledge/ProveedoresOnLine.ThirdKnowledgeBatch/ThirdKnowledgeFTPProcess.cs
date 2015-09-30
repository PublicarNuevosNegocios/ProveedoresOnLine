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
    public static class ThirdKnowledgeFTPProcess
    {
        public static void StartProcess()
        {
            try
            {
                //Get queries to process
                List<TDQueryModel> oQueryResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetQueriesInProgress();
                if (oQueryResult != null)
                {
                    //Set access
                    string ftpServerIP = ThirdKnowledgeBatch.Models.InternalSettings.Instance[Constants.C_Settings_FTPServerIP].Value;
                    string uploadToFolder = ThirdKnowledgeBatch.Models.InternalSettings.Instance[Constants.C_Settings_UploadFTPFileName].Value;
                    string UserName = ThirdKnowledgeBatch.Models.InternalSettings.Instance[Constants.C_Settings_FTPUserName].Value;
                    string UserPass = ThirdKnowledgeBatch.Models.InternalSettings.Instance[Constants.C_Settings_FTPPassworUser].Value;

                    string uri = "ftp://" + ftpServerIP + "/" + uploadToFolder + "/";

                    oQueryResult.All(oQuery =>
                    {
                        try
                        {
                            byte[] buffer = new byte[1024];
                            uri = uri + "Res_" + oQuery.RelatedQueryInfoModel.FirstOrDefault().Value;

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
                                    return true;
                                });
                            
                            //Update Status query
                            oQuery.QueryStatus = new TDCatalogModel()
                            {
                                ItemId = (int)ProveedoresOnLine.ThirdKnowledgeBatch.Models.Enumerations.enumThirdKnowledgeQueryStatus.Finalized,
                            };

                            ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.QueryUpsert(oQuery);
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                        return true;
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
