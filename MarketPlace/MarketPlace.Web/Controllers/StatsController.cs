using MarketPlace.Models.Compare;
using MarketPlace.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class StatsController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }
      
    }
}