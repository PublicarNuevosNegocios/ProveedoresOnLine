using Auth.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Web.Controllers
{
    public partial class AdminRolesController : BaseController
    {
        public virtual ActionResult Index()
        {
            //Listar roles
            List<AdminRolesModel> Model = new List<AdminRolesModel>();
            Model = DAL.Controller.AuthDataController.Instance.ListUserRoles();

            if (Model.Select(x => x.RelatedRole).ToList().ToString() == null)
            {
                Model = new List<AdminRolesModel>();
            }

            return View(Model);
        }

        public virtual ActionResult AutorizationUpsert(SessionManager.Models.Auth.enumRole Rol, SessionManager.Models.Auth.enumApplication Aplication)
        {
            string result = "";
            
            string correo = Request["UserEmail"].Trim();

            result = DAL.Controller.AuthDataController.Instance.CreateUserRolesUpsert(Convert.ToInt32(Aplication), Convert.ToInt32(Rol), correo);
            
            return RedirectToAction(MVC.AdminRoles.ActionNames.Index, MVC.AdminRoles.Name);
        }

        public virtual ActionResult AutorizationDelete()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                int userRole = int.Parse(Request["RoleId"].ToString().Trim());
                DAL.Controller.AuthDataController.Instance.DeleteUserRoles(userRole);
            }

            return RedirectToAction(MVC.AdminRoles.ActionNames.Index, MVC.AdminRoles.Name);
        }
    }
}