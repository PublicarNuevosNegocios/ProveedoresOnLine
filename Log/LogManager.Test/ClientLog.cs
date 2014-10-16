using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogManager.Test
{

    public class TestLogObject
    {
        public string objId { get; set; }
        public double objValue { get; set; }

        public System.Collections.Generic.List<string> strlst { get; set; }
    }

    [TestClass]
    public class ClientLog
    {
        [TestMethod]
        public void AddLog()
        {
            LogManager.ClientLog.AddLog(new Models.LogModel()
            {
                User = "usuario",
                Application = "Application",
                Source = "Source",
                IsSuccess = true,
                Message = "Message",

                LogObject = "este es el log object",

                RelatedLogInfo = new System.Collections.Generic.List<Models.LogInfoModel>()
                {
                    new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "tipo 1",
                        Value = "Valor 1"
                    },
                    new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "tipo 2",
                        Value = "Valor 2"
                    }
                }
            });

            LogManager.ClientLog.AddLog(new Models.LogModel()
            {
                User = "usuario",
                Application = "Application",
                Source = "Source",
                IsSuccess = true,
                Message = "Message",

                LogObject = Convert.ToInt32(454545),

                RelatedLogInfo = new System.Collections.Generic.List<Models.LogInfoModel>()
                {
                    new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "tipo 1",
                        Value = "Valor 1"
                    },
                    new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "tipo 2",
                        Value = "Valor 2"
                    }
                }
            });


            LogManager.ClientLog.AddLog(new Models.LogModel()
            {
                User = "usuario",
                Application = "Application",
                Source = "Source",
                IsSuccess = true,
                Message = "Message",

                LogObject = new TestLogObject() { objId = "el id ", objValue = 45545.252, strlst = new System.Collections.Generic.List<string>() { "a", "b" } },

                RelatedLogInfo = new System.Collections.Generic.List<Models.LogInfoModel>()
                {
                    new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "tipo 1",
                        Value = "Valor 1"
                    },
                    new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "tipo 2",
                        Value = "Valor 2"
                    }
                }
            });

            System.Threading.Thread.Sleep(70000);
        }
    }
}
