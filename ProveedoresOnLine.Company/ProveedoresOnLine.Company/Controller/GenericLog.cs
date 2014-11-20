using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Controller
{
    public class GenericLog
    {
        public static LogManager.Models.LogModel GetGenericLogModel()
        {
            LogManager.Models.LogModel oReturn = new LogManager.Models.LogModel()
            {
                RelatedLogInfo = new List<LogManager.Models.LogInfoModel>(),
            };

            try
            {
                System.Diagnostics.StackTrace oStack = new System.Diagnostics.StackTrace();

                if (System.Web.HttpContext.Current != null)
                {
                    //get user info
                    if (SessionManager.SessionController.Auth_UserLogin != null)
                    {
                        oReturn.User = SessionManager.SessionController.Auth_UserLogin.Email;
                    }
                    else
                    {
                        oReturn.User = System.Web.HttpContext.Current.Request.UserHostAddress;
                    }

                    //get source invocation
                    oReturn.Source = System.Web.HttpContext.Current.Request.Url.ToString();
                }
                else if (oStack.FrameCount > 0)
                {
                    oReturn.Source = oStack.GetFrame(oStack.FrameCount - 1).GetMethod().GetType().FullName;
                }

                //get appname
                oReturn.Application = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                if (oStack.FrameCount > 1)
                {
                    oReturn.Application += " - " + oStack.GetFrame(1).GetMethod().Name;
                }
            }
            catch { }

            return oReturn;
        }

    }
}
