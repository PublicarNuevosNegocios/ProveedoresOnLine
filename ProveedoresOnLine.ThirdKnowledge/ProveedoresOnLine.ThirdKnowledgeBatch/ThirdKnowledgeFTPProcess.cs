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

                    oQueryResult.All(query =>
                    {
                        try
                        {
                            byte[] buffer = new byte[1024];
                            uri = uri + "Res_" + query.RelatedQueryInfoModel.FirstOrDefault().Value;

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
                                    BatchXMLResultModel item = new BatchXMLResultModel()
                                    {
                                        NumeroConsulta = x.Element("NumeroConsulta").Value,
                                        IdentificacionConsulta = x.Element("IdentificacionConsulta").Value,
                                        NombreConsulta = x.Element("NombreConsulta").Value,
                                        IdGrupoLista = x.Element("IdGrupoLista").Value,
                                        NombreGrupoLista = x.Element("NombreGrupoLista").Value,
                                        Prioridad = x.Element("Prioridad").Value,
                                        TipoDocumento = x.Element("TipoDocumento").Value,
                                        DocumentoIdentidad = x.Element("DocumentoIdentidad").Value,
                                        NombreCompleto = x.Element("NombreCompleto").Value,
                                        IdTipoLista = x.Element("IdTipoLista").Value,
                                        NombreTipoLista = x.Element("NombreTipoLista").Value,
                                        Alias = x.Element("Alias").Value,
                                        CargoDelito = x.Element("CargoDelito").Value,
                                        Peps = x.Element("Peps").Value,
                                        Zona = x.Element("Zona").Value,
                                        Link = x.Element("Link").Value,
                                        OtraInformacion = x.Element("OtraInformacion").Value,
                                        FechaRegistro = x.Element("FechaRegistro").Value,
                                        FechaActualizacion = x.Element("FechaActualizacion").Value,
                                        Estado = x.Element("Estado").Value,
                                    };
                                    oResult.Add(item);
                                    return true;
                                });
                        }
                        catch (Exception)
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

        private static TDQueryInfoModel UpsertQueryInfo(List<TDQueryInfoModel> oQueryInfoToUpsert)
        {
            oQueryInfoToUpsert.All(x =>
                {
                    //ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.
                    return true;
                });
            return null;
        }
    }
}
