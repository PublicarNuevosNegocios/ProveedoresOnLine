﻿using MarketPlace.Models.General;
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
            ProveedoresOnLine.ProjectModule.Models.ProjectModel oCurrentProject = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.
                ProjectGetById
                (ProjectPublicId,
                MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

            MarketPlace.Models.Project.ProjectViewModel oModel = new Models.Project.ProjectViewModel(oCurrentProject);

            return View(oModel);
        }

        public virtual ActionResult ProjectDetailRecalculate(string ProjectPublicId)
        {
            //recalculate project values
            ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectCalculate
                (ProjectPublicId);

            //return redirect to project detail
            return RedirectToRoute
                (MarketPlace.Models.General.Constants.C_Routes_Default,
                new
                {
                    controller = MVC.Project.Name,
                    action = MVC.Project.ActionNames.ProjectDetail,
                    ProjectPublicId = ProjectPublicId,
                });
        }

        public virtual ActionResult ProjectProviderDetail(string ProjectPublicId, string ProviderPublicId)
        {
            return View();
        }

    }
}