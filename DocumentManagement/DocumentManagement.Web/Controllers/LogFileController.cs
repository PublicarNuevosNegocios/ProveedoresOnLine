using DocumentManagement.Models.Provider;
using DocumentManagement.Provider.Models.Provider;
using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Web.Controllers
{
    public partial class LogFileController : BaseController
    {
        public virtual ActionResult Index(string ProviderInfoId)
        {
            List<LogManager.Models.LogModel> oLogList = LogManager.ClientLog.LogSearch("ProviderInfoId", ProviderInfoId);

            List<ProviderFileLogModel> oModel = new List<ProviderFileLogModel>();

            if (oLogList != null && oLogList.Count > 0)
            {
                oLogList.All(ol =>
                {
                    oModel.Add(new ProviderFileLogModel(ol));
                    return true;
                });
            }

            return View(oModel);
        }
    }
}