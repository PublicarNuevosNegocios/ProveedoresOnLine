using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using MarketPlace.Models.Survey;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.SurveyModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ErrorController : BaseController
    {
        public virtual ActionResult Index()
        {
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

            return View();
        }
    }
}