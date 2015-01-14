using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        // GET: Home
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}