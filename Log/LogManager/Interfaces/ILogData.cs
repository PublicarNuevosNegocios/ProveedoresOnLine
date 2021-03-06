﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogManager.Interfaces
{
    internal interface ILogData
    {
        int LogCreate(string User, string Application, string Source, bool IsSuccess, string Message);

        void LogInfoCreate(int LogId, string LogInfoType, string Value);

        List<LogManager.Models.LogModel> LogSearch(string LogInfoType, string Value);

        void FileUploadCreate(string Url);

        void FileUsedCreate(string Url);
    }
}
