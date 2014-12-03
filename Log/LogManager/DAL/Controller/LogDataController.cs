﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogManager.DAL.Controller
{
    internal class LogDataController : LogManager.Interfaces.ILogData
    {
        #region Singleton instance

        private static LogManager.Interfaces.ILogData oInstance;
        public static LogManager.Interfaces.ILogData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new LogDataController();
                return oInstance;
            }
        }

        private LogManager.Interfaces.ILogData DataFactory;

        #endregion

        #region constructor

        public LogDataController()
        {
            LogDataFactory factory = new LogDataFactory();
            DataFactory = factory.GetDataInstance();
        }

        #endregion

        #region Generic Log

        public int LogCreate(string User, string Application, string Source, bool IsSuccess, string Message)
        {
            return DataFactory.LogCreate(User, Application, Source, IsSuccess, Message);
        }

        public void LogInfoCreate(int LogId, string LogInfoType, string Value)
        {
            DataFactory.LogInfoCreate(LogId, LogInfoType, Value);
        }

        public List<Models.LogModel> LogSearch(string LogInfoType, string Value)
        {
            return DataFactory.LogSearch(LogInfoType, Value);
        }

        #endregion

        #region File

        public void FileUploadCreate(string Url)
        {
            DataFactory.FileUploadCreate(Url);
        }

        public void FileUsedCreate(string Url)
        {
            DataFactory.FileUsedCreate(Url);
        }

        #endregion
    }
}
