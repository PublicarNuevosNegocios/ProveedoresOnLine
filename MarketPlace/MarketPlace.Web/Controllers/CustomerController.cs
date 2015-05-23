using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class CustomerController : BaseController
    {
        public virtual ActionResult Index()
        {
            //temporaly redirect to provider search
            return RedirectToRoute
                (MarketPlace.Models.General.Constants.C_Routes_Default,
                new
                {
                    controller = MVC.Provider.Name,
                    action = MVC.Provider.ActionNames.Index
                });            
        }
    }
}