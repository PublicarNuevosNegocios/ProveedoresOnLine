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