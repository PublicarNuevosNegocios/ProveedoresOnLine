using LinqToExcel;
using NetOffice.ExcelApi;
using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess;
using ProveedoresOnLine.RestrictiveListProcessBatch.Models;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProveedoresOnLine.RestrictiveListProcessBatch
{
    public class RestrictiveListReadProcess
    {
        public static void StartProcess()
        {
            try
            {
                //Start Process
                //Get all BlackListProcess
                List<RestrictiveListProcessModel> oProcessToValidate = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.GetAllProvidersInProcess();
                if (oProcessToValidate != null)
                {
                    string strFolder = ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_Settings_File_TempDirectory].Value;

                    oProcessToValidate.All(Process =>
                    {
                        //Instance App to read excel
                        Application app = new Application();

                        string FileName = Process.FilePath.Split('/').LastOrDefault();
                        //Call Function to get Coincidences
                        List<TDQueryInfoModel> oCoincidences = GetCoincidences(FileName);

                        if (oCoincidences != null)
                        {
                            //Download Current File
                            using (WebClient webClient = new WebClient())
                            {
                                //Get file from S3 using File Name           
                                webClient.DownloadFile(Process.FilePath, strFolder + FileName);

                                //Open File DownLoaded
                                Workbook book = app.Workbooks.Open(strFolder + FileName,
                                       Missing.Value, Missing.Value, Missing.Value,
                                       Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                       Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                       Missing.Value, Missing.Value, Missing.Value);

                                //Save As .xls
                                book.SaveAs(strFolder + FileName.Replace("xlsx", "xls"), fileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8);
                                book.Close();
                            }

                            ExcelQueryFactory XlsInfo = new ExcelQueryFactory(strFolder + FileName.Replace("xlsx", "xls"));
                            //Set model Params
                            List<ExcelModel> oExcelS3 =
                            (from x in XlsInfo.Worksheet<ExcelModel>(0)
                             select x).ToList();

                            //Get Provider by Status                        
                            Process.RelatedProvider = new List<ProviderModel>();
                            
                            //Compare Company
                            List<ProviderModel> oProvidersToCompare = new List<ProviderModel>();
                            
                            //Compare Persons                        
                            List<ProviderModel> oCompanyPersonToCompare = new List<ProviderModel>();

                            //Get Providers By Status
                            Process.RelatedProvider = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.GetProviderByStatus(Convert.ToInt32(Process.ProviderStatus), ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_Settings_PublicarPublicId].Value);

                            oCoincidences.All(p =>
                            {
                                oProvidersToCompare.Add(Process.RelatedProvider.Where(y => y.RelatedCompany.IdentificationNumber == p.IdentificationResult && y.RelatedCompany.CompanyName == p.NameResult).FirstOrDefault());

                                Process.RelatedProvider.All(m =>
                                    {
                                        if (m.RelatedLegal.Where(l => l.ItemInfo.Where(inf => inf.ItemInfoType.ItemId == (int)enumLegalDesignationsInfoType.CD_PartnerIdentificationNumber).Select(inf => inf.Value).FirstOrDefault() == p.IdentificationResult
                                                                && l.ItemInfo.Where(inf => inf.ItemInfoType.ItemId == (int)enumLegalDesignationsInfoType.CD_PartnerIdentificationNumber).Select(inf => inf.Value).FirstOrDefault() == p.NameResult) != null)
                                        {
                                            oCompanyPersonToCompare.Add(m);
                                        }

                                        return true;
                                    });
                                return true;
                            });

                            //Creacte Private Function to update blackList
                            if (oProvidersToCompare.Count > 0)
                                CreateBlackListProcess(oProvidersToCompare, oCoincidences);

                            if (oCompanyPersonToCompare.Count > 0)
                                CreateBlackListProcess(oCompanyPersonToCompare, oCoincidences);


                            //Remove all Files
                            //remove temporal file
                            if (System.IO.File.Exists(strFolder + FileName))
                                System.IO.File.Delete(strFolder + FileName);

                            //remove temporal file
                            if (System.IO.File.Exists(strFolder + FileName.Replace("xlsx", "xls")))
                                System.IO.File.Delete(strFolder + FileName.Replace("xlsx", "xls"));

                        }

                        return true;
                    });
                }
            }
            catch (Exception err)
            {
                LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }
        }

        private static List<TDQueryInfoModel> GetCoincidences(string FileName)
        {
            //Set access
            string ftpServerIP = ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_FTPServerIP].Value;
            string uploadToFolder = ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_UploadFTPFileName].Value;
            string UserName = ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_FTPUserName].Value;
            string UserPass = ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_FTPPassworUser].Value;

            FileName = FileName.Replace(FileName.Split('.').LastOrDefault(), "xml");

            string uri = "ftp://" + ftpServerIP + "/" + uploadToFolder + "/" + "Res_" + FileName;
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

            List<TDQueryInfoModel> oReturn = new List<TDQueryInfoModel>();
            //Set results to model
            CurrentXMLAnswer.Descendants("Resultado").All(
                x =>
                {
                    if (x.Element("NumeroConsulta").Value != "No existen registros asociados a los parámetros de consulta."
                        && x.Element("Estado").Value.ToLower() == "true")
                    {
                        #region QueryInfo
                        TDQueryInfoModel oInfoCreate = new TDQueryInfoModel();
                        oInfoCreate.Alias = x.Element("Alias").Value;
                        oInfoCreate.IdentificationResult = x.Element("IdentificacionConsulta").Value;
                        oInfoCreate.NameResult = x.Element("NombreConsulta").Value;
                        oInfoCreate.Offense = x.Element("CargoDelito").Value;
                        oInfoCreate.Peps = x.Element("Peps").Value;
                        oInfoCreate.Priority = x.Element("Prioridad").Value;
                        oInfoCreate.Status = x.Element("Estado").Value;

                        oInfoCreate.DetailInfo = new List<TDQueryDetailInfoModel>();

                        #region Detail Info
                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                        {
                            ItemInfoType = new TDCatalogModel()
                            {
                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.IdList,
                            },
                            Value = !string.IsNullOrEmpty(x.Element("IdTipoLista").Value) ? x.Element("IdTipoLista").Value : string.Empty,
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
                            Value = !string.IsNullOrEmpty(x.Element("Prioridad").Value) ? x.Element("Prioridad").Value : string.Empty,
                            Enable = true,
                        });
                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                        {
                            ItemInfoType = new TDCatalogModel()
                            {
                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.RegisterDate,
                            },
                            Value = !string.IsNullOrEmpty(x.Element("FechaRegistro").Value) ? x.Element("FechaRegistro").Value : string.Empty,
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
                            Value = !string.IsNullOrEmpty(x.Element("IdGrupoLista").Value) &&
                                    x.Element("IdGrupoLista").Value == "1" ? !string.IsNullOrEmpty(x.Element("NombreGrupoLista").Value) ? x.Element("NombreGrupoLista").Value + " - Criticidad Alta" : string.Empty :
                                    x.Element("IdGrupoLista").Value == "2" ? !string.IsNullOrEmpty(x.Element("NombreGrupoLista").Value) ? x.Element("NombreGrupoLista").Value + " - Criticidad Media" : string.Empty :
                                    x.Element("IdGrupoLista").Value == "3" ? !string.IsNullOrEmpty(x.Element("NombreGrupoLista").Value) ? x.Element("NombreGrupoLista").Value + " - Criticidad Media" : string.Empty :
                                    x.Element("IdGrupoLista").Value == "4" ? !string.IsNullOrEmpty(x.Element("NombreGrupoLista").Value) ? x.Element("NombreGrupoLista").Value + " - Criticidad Baja" : string.Empty :
                                    string.Empty,
                            Enable = true,
                        });
                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                        {
                            ItemInfoType = new TDCatalogModel()
                            {
                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.GroupId,
                            },
                            Value = !string.IsNullOrEmpty(x.Element("IdGrupoLista").Value) ? x.Element("IdGrupoLista").Value : string.Empty,
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
                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                        {
                            ItemInfoType = new TDCatalogModel()
                            {
                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.RequestName,
                            },
                            Value = x.Element("NombreConsulta").Value,
                            Enable = true,
                        });
                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                        {
                            ItemInfoType = new TDCatalogModel()
                            {
                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.IdNumberRequest,
                            },
                            Value = x.Element("IdentificacionConsulta").Value,
                            Enable = true,
                        });
                        oInfoCreate.DetailInfo.Add(new TDQueryDetailInfoModel()
                        {
                            ItemInfoType = new TDCatalogModel()
                            {
                                ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.TypeDocument,
                            },
                            Value = x.Element("TipoDocumento").Value,
                            Enable = true,
                        });
                        #endregion

                        oReturn.Add(oInfoCreate);
                        #endregion
                    }
                    return true;
                });
            return oReturn;
        }

        private static bool CreateBlackListProcess(List<ProviderModel> oProvidersToUpdate, List<TDQueryInfoModel> oCoincidences)
        {
            try
            {
                if (oProvidersToUpdate.Count > 0)
                {
                    List<TDQueryInfoModel> oCurrentCoincidences = new List<TDQueryInfoModel>();

                    //For each  provider create de black List
                    oProvidersToUpdate.All(prv =>
                    {
                        if (ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListClearProvider(prv.RelatedCompany.CompanyPublicId) && prv.RelatedCompany != null)
                        {
                            ProviderModel oProvider = new ProviderModel();
                            oProvider.RelatedCompany = new CompanyModel();
                            oProvider.RelatedCompany.CompanyInfo = new List<GenericItemInfoModel>();

                            oProvider.RelatedCompany.CompanyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = prv.RelatedCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.RestrictiveListProcessBatch.Models.enumCompanyInfoType.UpdateAlert).
                                        Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault(),
                                ItemInfoType = new CatalogModel()
                                {
                                    ItemId = (int)ProveedoresOnLine.RestrictiveListProcessBatch.Models.enumCompanyInfoType.UpdateAlert,
                                },
                                Enable = true,
                                Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            });

                            oProvider.RelatedCompany.CompanyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = prv.RelatedCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.RestrictiveListProcessBatch.Models.enumCompanyInfoType.Alert).
                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault(),
                                ItemInfoType = new CatalogModel()
                                {
                                    ItemId = (int)ProveedoresOnLine.RestrictiveListProcessBatch.Models.enumCompanyInfoType.Alert,
                                },
                                Enable = true,
                                Value = ((int)ProveedoresOnLine.RestrictiveListProcessBatch.Models.enumBlackList.BL_DontShowAlert).ToString(),
                            });

                            //Save DateTime of last Update Data
                            ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.ProviderUpsert(oProvider);

                            //Get coincidences for current provider
                            oCurrentCoincidences = oCoincidences.Where(x => x.NameResult == prv.RelatedCompany.CompanyName && x.IdentificationResult == prv.RelatedCompany.IdentificationNumber).Select(x => x).ToList();

                            oCurrentCoincidences.All(c =>
                            {
                                #region Operation

                                ProviderModel oProviderToInsert = new ProviderModel();
                                oProviderToInsert.RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel();
                                oProviderToInsert.RelatedCompany.CompanyInfo = new List<GenericItemInfoModel>();
                                oProviderToInsert.RelatedCompany.CompanyPublicId = prv.RelatedCompany.CompanyPublicId;
                                oProviderToInsert.RelatedBlackList = new List<BlackListModel>();

                                CompanyModel BasicInfo = new CompanyModel();
                                BasicInfo = ProveedoresOnLine.Company.Controller.Company.CompanyGetBasicInfo(prv.RelatedCompany.CompanyPublicId);
                                oProviderToInsert.RelatedBlackList.Add(new BlackListModel
                                {
                                    BlackListStatus = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)ProveedoresOnLine.RestrictiveListProcessBatch.Models.enumBlackList.BL_ShowAlert,
                                    },
                                    User = "Proveedores OnLine Process",
                                    FileUrl = "",
                                    BlackListInfo = new List<GenericItemInfoModel>()
                                });

                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Alias",
                                    },
                                    Value = c.Alias,
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Nombre Consultado",
                                    },
                                    Value = c.NameResult,
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Identificación Consultada",
                                    },
                                    Value = c.IdentificationResult,
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Cargo o Delito",
                                    },
                                    Value = c.Offense,
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Peps",
                                    },
                                    Value = c.Peps,
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Prioridad",
                                    },
                                    Value = c.Priority,
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Estado",
                                    },
                                    Value = c.Status,
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Fecha Registro",
                                    },
                                    Value = c.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.RegisterDate).Select(x => x.Value).FirstOrDefault(),
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Documento de Identidad",
                                    },
                                    Value = c.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.IdentificationNumberResult).Select(x => x.Value).FirstOrDefault(),
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Fecha de Actualizacion",
                                    },
                                    Value = c.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.LastModifyDate).Select(x => x.Value).FirstOrDefault(),
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Nombre del Grupo",
                                    },
                                    Value = c.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.GroupName).Select(x => x.Value).FirstOrDefault(),
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Nombre Completo",
                                    },
                                    Value = c.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.NameResult).Select(x => x.Value).FirstOrDefault(),
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Nombre de la Lista",
                                    },
                                    Value = c.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.ListName).Select(x => x.Value).FirstOrDefault(),
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Otra Información",
                                    },
                                    Value = c.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.MoreInfo).Select(x => x.Value).FirstOrDefault(),
                                    Enable = true,
                                });
                                oProviderToInsert.RelatedBlackList.FirstOrDefault().BlackListInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemName = "Zona",
                                    },
                                    Value = c.DetailInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Zone).Select(x => x.Value).FirstOrDefault(),
                                    Enable = true,
                                });

                                List<ProviderModel> oProviderResultList = new List<ProviderModel>();
                                oProviderResultList.Add(ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListInsert(oProviderToInsert));

                                var idResult = oProviderResultList.FirstOrDefault().RelatedBlackList.Where(x => x.BlackListInfo != null).Select(x => x.BlackListInfo.Select(y => y.ItemInfoId)).FirstOrDefault();

                                #region Set Provider Info

                                oProviderToInsert.RelatedCompany.CompanyInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = BasicInfo.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.Alert)
                                                .Select(x => x.ItemInfoId).FirstOrDefault() != 0 ? BasicInfo.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.Alert)
                                                .Select(x => x.ItemInfoId).FirstOrDefault() : 0,
                                    ItemInfoType = new CatalogModel()
                                    {
                                        ItemId = (int)enumCompanyInfoType.Alert,
                                    },
                                    Value = ((int)enumBlackList.BL_ShowAlert).ToString(),
                                    Enable = true,
                                });

                                //Set large value With the items found
                                //oProviderToInsert.RelatedCompany.CompanyInfo.Add(new GenericItemInfoModel()
                                //{
                                //    ItemInfoId = BasicInfo.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.ListId)
                                //                .Select(x => x.ItemInfoId).FirstOrDefault() != 0 ? BasicInfo.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.ListId)
                                //                .Select(x => x.ItemInfoId).FirstOrDefault() : 0,
                                //    ItemInfoType = new CatalogModel()
                                //    {
                                //        ItemId = (int)enumCompanyInfoType.ListId,
                                //    },
                                //    LargeValue = string.Join(",", idResult),
                                //    Enable = true,
                                //});

                                #endregion Set Provider Info
                                ProveedoresOnLine.Company.Controller.Company.CompanyInfoUpsert(oProviderToInsert.RelatedCompany);
                                #endregion Operation
                                return true;
                            });
                        }
                        return true;
                    });
                }

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }           
            
        }
        #region Log File

        private static void LogFile(string LogMessage)
        {
            try
            {
                //get file Log
                string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                    System.Configuration.ConfigurationManager.AppSettings[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

                if (!System.IO.Directory.Exists(LogFile))
                    System.IO.Directory.CreateDirectory(LogFile);

                LogFile += "\\" + "Log_BlacListReadProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

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
