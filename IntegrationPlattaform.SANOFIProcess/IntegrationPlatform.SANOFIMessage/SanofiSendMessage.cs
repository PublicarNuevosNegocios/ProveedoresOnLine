using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlatform.SANOFIMessage
{
    public class SanofiSendMessage
    {
        public static void StartProcess()
        {
            try
            {
                LogFile("StartProccess::");

                try
                {
                    LogFile("StartProccess:: get all log process");

                    List<IntegrationPlattaform.SANOFIProcess.Models.SanofiProcessLogModel> oLogModel =
                    IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetSanofiProcessLogBySendStatus(false);

                    LogFile("StartProccess:: log process :: count " + oLogModel.Count);

                    if (oLogModel != null && oLogModel.Count > 0)
                    {
                        LogFile("StartProccess:: get message");

                        MessageModule.Client.Models.ClientMessageModel oMessageModel =
                            GetMessage(oLogModel, IntegrationPlatform.SANOFIMessage.Models.InternalSettings.Instance
                                    [IntegrationPlatform.SANOFIMessage.Models.Constants.C_POL_SanofiMessage_To].Value.ToString());

                        LogFile("StartProccess:: upsert message");

                        //create message
                        int oMessageCreate = MessageModule.Client.Controller.ClientController.CreateMessage(oMessageModel);

                        LogFile("StartProccess:: message saved with code: " + oMessageCreate);

                        if (oMessageCreate > 0)
                        {
                            LogFile("StartProccess:: update log send status");

                            oLogModel.All(log =>
                            {
                                //update status send message
                                log.SendStatus = true;
                                IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.SanofiProcessLogUpsert(log);

                                return true;
                            });

                            LogFile("StartProccess:: finally update send status");
                        }
                        else
                        {
                            throw new Exception("Sanofi process error:: Send Queue");
                        }
                    }
                    else
                    {
                        throw new Exception("Sanofi process error:: Sanofi log list is empty.");
                    }
                }
                catch (Exception err)
                {
                    LogFile("Error:: SanofiMessageProccess '" + err.Message);
                }
            }
            catch { }
        }

        #region Message Fabric

        private static MessageModule.Client.Models.ClientMessageModel GetMessage(List<IntegrationPlattaform.SANOFIProcess.Models.SanofiProcessLogModel> oLogModel, string toMessage)
        {
            //Create Message Object
            MessageModule.Client.Models.ClientMessageModel oReturn = new MessageModule.Client.Models.ClientMessageModel()
            {
                Agent = IntegrationPlatform.SANOFIMessage.Models.Constants.C_POL_SANOFI_Mail_Agent,
                User = "SANOFIProccess",
                ProgramTime = DateTime.Now,
                MessageQueueInfo = new List<Tuple<string, string>>(),
            };

            //get to address
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("To", toMessage));

            //Add Sanofi logo to email
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("CustomerLogo", IntegrationPlatform.SANOFIMessage.Models.InternalSettings.Instance
                                    [IntegrationPlatform.SANOFIMessage.Models.Constants.C_Settings_Company_DefaultLogoUrl].Value));
            
            //get Sanofi document

            if (oLogModel != null &&
                oLogModel.Count > 0)
            {
                #region General Info

                if (oLogModel.Any(x => x.ProcessName == "GeneralInfo"))
                {
                    //Add general info doc to html
                    oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                        ("SANOFIGeneralInfo", IntegrationPlatform.SANOFIMessage.Models.InternalSettings.Instance
                                    [IntegrationPlatform.SANOFIMessage.Models.Constants.C_POL_SANOFI_FileMessage].Value.
                                    Replace("{InformationType}", "Información General").
                                    Replace("{InfoFileUrl}", oLogModel.Where(log => log.ProcessName == "GeneralInfo" && !string.IsNullOrEmpty(log.FileName)).FirstOrDefault().FileName).
                                    Replace("{FileCreateDate}", oLogModel.Where(log => log.ProcessName == "GeneralInfo" && !string.IsNullOrEmpty(log.FileName)).FirstOrDefault().CreateDate.ToString())));
                }
                else
                {
                    oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                        ("SANOFIGeneralInfo", string.Empty));

                    LogFile("Error:: General info file is empty or null");
                }

                #endregion

                #region Commercial Info

                if (oLogModel.Any(x => x.ProcessName == "ComercialInfo"))
                {
                    //Add commercial info doc to html
                    oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                        ("SANOFIComercialInfo", IntegrationPlatform.SANOFIMessage.Models.InternalSettings.Instance
                                    [IntegrationPlatform.SANOFIMessage.Models.Constants.C_POL_SANOFI_FileMessage].Value.
                                    Replace("{InformationType}", "Información Comercial").
                                    Replace("{InfoFileUrl}", oLogModel.Where(log => log.ProcessName == "ComercialInfo" && !string.IsNullOrEmpty(log.FileName)).FirstOrDefault().FileName).
                                    Replace("{FileCreateDate}", oLogModel.Where(log => log.ProcessName == "ComercialInfo" && !string.IsNullOrEmpty(log.FileName)).FirstOrDefault().CreateDate.ToString())));
                }
                else
                {
                    oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                        ("SANOFIComercialInfo", string.Empty));

                    LogFile("Error:: Commercial info file is empty or null");
                }

                #endregion

                #region Contable Info

                if (oLogModel.Any(x => x.ProcessName == "ContableInfo"))
                {
                    //Add contable info doc to html
                    oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                        ("SANOFIContableInfo", IntegrationPlatform.SANOFIMessage.Models.InternalSettings.Instance
                                    [IntegrationPlatform.SANOFIMessage.Models.Constants.C_POL_SANOFI_FileMessage].Value.
                                    Replace("{InformationType}", "Información Contable").
                                    Replace("{InfoFileUrl}", oLogModel.Where(log => log.ProcessName == "ContableInfo" && !string.IsNullOrEmpty(log.FileName)).FirstOrDefault().FileName).
                                    Replace("{FileCreateDate}", oLogModel.Where(log => log.ProcessName == "ContableInfo" && !string.IsNullOrEmpty(log.FileName)).FirstOrDefault().CreateDate.ToString())));
                }
                else
                {
                    oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                        ("SANOFIContableInfo", string.Empty));

                    LogFile("Error:: Contable info file is empty or null");
                }

                #endregion
            }

            return oReturn;
        }

        #endregion

        #region LogFile

        private static void LogFile(string LogMessage)
        {
            try
            {
                //get the file log
                string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                    System.Configuration.ConfigurationManager.AppSettings[IntegrationPlatform.SANOFIMessage.Models.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

                if (!System.IO.Directory.Exists(LogFile))
                    System.IO.Directory.CreateDirectory(LogFile);

                LogFile += "\\" + "Log_SANOFISendProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

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
