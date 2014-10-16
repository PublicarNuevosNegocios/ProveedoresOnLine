using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogManager.DAL.Controller
{
    internal class LogDataFactory
    {
        public LogManager.Interfaces.ILogData GetDataInstance()
        {
            Type typetoreturn = Type.GetType("LogManager.DAL.MySqlDao.Log_MySqlDao,LogManager");
            LogManager.Interfaces.ILogData oRetorno = (LogManager.Interfaces.ILogData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }

    }
}
