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
                    List<IntegrationPlattaform.SANOFIProcess.Models.SanofiProcessLogModel> oLogModel =
                    IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.GetSanofiProcessLogBySendStatus(false);

                    MessageModule.Client.Models.ClientMessageModel oMessageModel =
                        GetMessage(oLogModel, "david.moncayo@publicar.com");

                    //create message
                    int oMessageCreate = MessageModule.Client.Controller.ClientController.CreateMessage(oMessageModel);

                    if (oMessageCreate > 0)
                    {
                        oLogModel.All(log =>
                        {
                            //update status send message
                            log.SendStatus = true;

                            IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.SanofiProcessLogUpsert(log);

                            return true;
                        });
                    }
                    else
                    {
                        throw new Exception ("Sanofi process error:: Send Queue");
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
                User = "",
                ProgramTime = DateTime.Now,
                MessageQueueInfo = new List<Tuple<string, string>>(),
            };

            //get to address
            oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                ("To", toMessage));

            //get Sanofi document
            oLogModel.All(log =>
            {
                if (log.ProcessName == "GeneralInfo")
                {
                    if (!string.IsNullOrEmpty(log.FileName))
                    {
                        //General Document
                        oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                            ("GeneralInfo", log.FileName));

                        oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                            ("GeneralInfoLastDate", log.CreateDate.ToString()));
                    }
                    else
                    {
                        LogFile("Error:: General info file is empty or null");
                    }                    
                }
                else if (log.ProcessName == "ComercialInfo")
                {
                    if (!string.IsNullOrEmpty(log.FileName))
                    {
                        //Commercial Document
                        oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                            ("ComercialInfo", log.FileName));

                        oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                            ("ComercialInfoLastDate", log.CreateDate.ToString()));    
                    }
                    else
                    {
                        LogFile("Error:: Commercial info file is empty or null");
                    }                    
                }
                else
                {
                    if (!string.IsNullOrEmpty(log.FileName))
                    {
                        //Contable Document
                        oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                            ("ContableInfo", log.FileName));

                        oReturn.MessageQueueInfo.Add(new Tuple<string, string>
                            ("ContableInfoLastDate", log.CreateDate.ToString()));                        
                    }
                    else
                    {
                        LogFile("Error:: Contable info file is empty or null");
                    }
                }

                return true;
            });

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
