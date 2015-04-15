using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ProjectController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult ProjectDetail(string ProjectPublicId)
        {
            return View();
        }

        public virtual ActionResult ProjectProviderDetail(string ProjectPublicId, string ProviderPublicId)
        {
            return View();
        }

    }
}