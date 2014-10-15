using Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DAL.Controller
{
    internal class AuthDataFactory
    {
        public IAuthData GetDataInstance()
        {
            Type typetoreturn = Type.GetType("Auth.DAL.MySqlDao.Auth_MySqlDao,Auth.DAL");
            IAuthData oRetorno = (IAuthData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
