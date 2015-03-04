using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Controller
{
    public class MessageProcess
    {
        public static void StartProcess()
        {
            try
            {
                //get message to send
                List<MessageModule.Interfaces.Models.MessageModel> lstMessageToSend =
                    MessageModule.DAL.Controller.MessageDataController.Instance.MessageQueueGetMessageToProcess();

                if (lstMessageToSend != null && lstMessageToSend.Count > 0)
                {
                    //log file
                    LogFile("Start send " + lstMessageToSend.Count.ToString());

                    lstMessageToSend.All(mts =>
                    {
                        try
                        {
                            if (MessageModule.Interfaces.Models.MessageConfig.AgentConfig.ContainsKey(mts.Agent))
                            {
                                //get message instance
                                string strAssemblie = MessageModule.Interfaces.Models.MessageConfig.
                                    AgentConfig
                                    [mts.Agent]
                                    [MessageModule.Interfaces.General.Constants.C_Agent_Assemblie];
                                MessageModule.Interfaces.IMessageAgent oConcretAgent = MessageFactory(strAssemblie);

                                //send message
                                mts = oConcretAgent.SendMessage(mts);
                            }
                        }
                        catch (Exception err)
                        {
                            mts.IsSuccess = false;
                            mts.ProcessResult = "Error::" + err.Message + " - " + err.StackTrace;
                        }
                        finally
                        {
                            //register process message
                            mts.QueueProcessId =
                                MessageModule.DAL.Controller.MessageDataController.Instance.QueueProcessProcessMessage
                                (mts.MessageQueueId,
                                mts.IsSuccess,
                                mts.ProcessResult);
                        }

                        return true;
                    });

                    //log file
                    LogFile("End send " + lstMessageToSend.Count.ToString());
                }
            }
            catch (Exception err)
            {
                //log file for fatal error
                LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }
        }

        #region Message factory

        private static MessageModule.Interfaces.IMessageAgent MessageFactory(string Assemblie)
        {
            Type typetoreturn = Type.GetType(Assemblie);
            MessageModule.Interfaces.IMessageAgent oRetorno = (MessageModule.Interfaces.IMessageAgent)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }

        #endregion

        #region Log File

        private static void LogFile(string LogMessage)
        {
            try
            {
                //get file Log
                string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                    System.Configuration.ConfigurationManager.AppSettings[MessageModule.Interfaces.General.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

                if (!System.IO.Directory.Exists(LogFile))
                    System.IO.Directory.CreateDirectory(LogFile);

                LogFile += "\\" + "Log_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

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
