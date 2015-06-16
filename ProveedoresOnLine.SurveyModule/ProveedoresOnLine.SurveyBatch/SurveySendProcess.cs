using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.SurveyBatch
{
    public class SurveySendProcess
    {
        public static void StartProcess()
        {
            try
            {
                List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> lstSurveyToSend =
                    ProveedoresOnLine.SurveyModule.Controller.SurveyModule.BPSurveyGetNotification();

                if (lstSurveyToSend != null && lstSurveyToSend.Count > 0)
                {
                    //log file
                    LogFile("Start send " + lstSurveyToSend.Count.ToString());

                    lstSurveyToSend.All(sv =>
                    {
                        try
                        {
                            if (sv.SurveyInfo != null)
                            {
                                List<string> EvaluatorRol = new List<string>();

                                sv.SurveyInfo.All(x =>
                                    {
                                        if (x.ItemInfoType.ItemId == 1204003)
                                        {
                                            EvaluatorRol.Add(x.Value);
                                        }
                                        return true;
                                    });
                                EvaluatorRol.All(r =>
                                    {
                                        MessageModule.Client.Models.ClientMessageModel oMessageToUpsert = GetMessage(sv, r);

                                        //create message
                                        MessageModule.Client.Controller.ClientController.CreateMessage(oMessageToUpsert);
                                        return true;
                                    }
                                );                                                               

                                //update survey status
                                ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurveyToUpsert = new SurveyModule.Models.SurveyModel()
                                {
                                    SurveyPublicId = sv.SurveyPublicId,
                                    SurveyInfo = new List<Company.Models.Util.GenericItemInfoModel>()
                                    {
                                        new Company.Models.Util.GenericItemInfoModel()
                                        {
                                            ItemInfoId = sv.SurveyInfo.
                                                Where(svinf=>svinf.ItemInfoType.ItemId == 1204004).
                                                Select(svinf=>svinf.ItemInfoId).
                                                DefaultIfEmpty(0).
                                                FirstOrDefault(),
                                            ItemInfoType = new Company.Models.Util.CatalogModel()
                                            {
                                                ItemId = 1204004
                                            },
                                            Value = "1206002",
                                            Enable = true,
                                        }
                                    }
                                };

                                oSurveyToUpsert = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyInfoUpsert(oSurveyToUpsert);
                            }
                            else
                            {
                                throw new Exception("La evaluación con id '" + sv.SurveyPublicId + "' no tienen información para enviar el correo.");
                            }
                        }
                        catch (Exception err)
                        {
                            LogFile("Error:: SurveyPublicId '" + sv.SurveyPublicId + "' :: " + err.Message);
                        }

                        return true;
                    });

                    //log file
                    LogFile("End send " + lstSurveyToSend.Count.ToString());
                }
            }
            catch (Exception err)
            {
                //log file for fatal error
                LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }
        }

        #region MessageFabric

        private static MessageModule.Client.Models.ClientMessageModel GetMessage(ProveedoresOnLine.SurveyModule.Models.SurveyModel vSurvey, string EvaluatorRol)
        {
            //Create message object
            MessageModule.Client.Models.ClientMessageModel oReturn = new MessageModule.Client.Models.ClientMessageModel()
            {
                Agent = ProveedoresOnLine.SurveyBatch.Models.Constants.C_POL_SurveyWriteBackNotification_Mail_Agent,
                User = vSurvey.SurveyInfo.
                    Where(svinf => svinf.ItemInfoType.ItemId == 1204002).
                    Select(svinf => svinf.Value).
                    DefaultIfEmpty("SystemUser").
                    FirstOrDefault(),
                ProgramTime = DateTime.Now,
                MessageQueueInfo = new List<Tuple<string, string>>(),
            };

            //get to address
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("To",EvaluatorRol));

            //get customer info
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("CustomerLogo",
                vSurvey.RelatedSurveyConfig.RelatedCustomer.RelatedCompany.CompanyInfo.
                    Where(prvinf => prvinf.ItemInfoType.ItemId == 203005).
                    Select(prvinf => prvinf.Value).
                    DefaultIfEmpty(ProveedoresOnLine.SurveyBatch.Models.InternalSettings.Instance
                                [ProveedoresOnLine.SurveyBatch.Models.Constants.C_Settings_Company_DefaultLogoUrl].Value).
                    FirstOrDefault()));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("CustomerName",
                vSurvey.RelatedSurveyConfig.RelatedCustomer.RelatedCompany.CompanyName));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("CustomerIdentificationTypeName",
                vSurvey.RelatedSurveyConfig.RelatedCustomer.RelatedCompany.IdentificationType.ItemName));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("CustomerIdentificationNumber",
                vSurvey.RelatedSurveyConfig.RelatedCustomer.RelatedCompany.IdentificationNumber));

            //get provider info
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProviderLogo",
                vSurvey.RelatedProvider.RelatedCompany.CompanyInfo.
                    Where(prvinf => prvinf.ItemInfoType.ItemId == 203005).
                    Select(prvinf => prvinf.Value).
                    DefaultIfEmpty(ProveedoresOnLine.SurveyBatch.Models.InternalSettings.Instance
                                [ProveedoresOnLine.SurveyBatch.Models.Constants.C_Settings_Company_DefaultLogoUrl].Value).
                    FirstOrDefault()));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProviderName",
                vSurvey.RelatedProvider.RelatedCompany.CompanyName));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProviderIdentificationTypeName",
                vSurvey.RelatedProvider.RelatedCompany.IdentificationType.ItemName));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProviderIdentificationNumber",
                vSurvey.RelatedProvider.RelatedCompany.IdentificationNumber));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("ProviderLink",
                ProveedoresOnLine.SurveyBatch.Models.InternalSettings.Instance
                                [ProveedoresOnLine.SurveyBatch.Models.Constants.C_Settings_Provider_ProviderUrl].Value.
                Replace("{ProviderPublicId}", vSurvey.RelatedProvider.RelatedCompany.CompanyPublicId)));

            //get survey info
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("SurveyName",
                vSurvey.RelatedSurveyConfig.ItemInfo.
                    Where(scinf => scinf.ItemInfoType.ItemId == 1201001).
                    Select(svinf => string.IsNullOrEmpty(svinf.ValueName) ? string.Empty : svinf.ValueName + " - ").
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault() +
                    vSurvey.RelatedSurveyConfig.ItemName));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("SurveyIssueDate",
                vSurvey.SurveyInfo.
                    Where(svinf => svinf.ItemInfoType.ItemId == 1204001).
                    Select(svinf => svinf.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault()));

            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("SurveyLink",
                ProveedoresOnLine.SurveyBatch.Models.InternalSettings.Instance
                                [ProveedoresOnLine.SurveyBatch.Models.Constants.C_Settings_Survey_SurveyUrl].Value.
                Replace("{SurveyPublicId}", vSurvey.SurveyPublicId)));

            return oReturn;
        }

        #endregion

        #region Log File

        private static void LogFile(string LogMessage)
        {
            try
            {
                //get file Log
                string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                    System.Configuration.ConfigurationManager.AppSettings[ProveedoresOnLine.SurveyBatch.Models.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

                if (!System.IO.Directory.Exists(LogFile))
                    System.IO.Directory.CreateDirectory(LogFile);

                LogFile += "\\" + "Log_SurveySendProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

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
