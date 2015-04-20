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
            ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel oSerialize = new ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel()
            {
                AmmounEnable = true,
                YearsEnable = true,
                DefaultAcitvityEnable = true,
                CustomAcitvityEnable = true,
                CurrencyEnable = true,
                YearsInterval = new List<Tuple<string, string>>() 
                {
                    new Tuple<int, string>("302003_0_1409001","Seleccione"),
                    new Tuple<int, string>("302003_1_1409001","1"),
                    new Tuple<int, string>("302003_2_1409001","2"),
                    new Tuple<int, string>("302003_3_1409001","3"),
                    new Tuple<int, string>("302003_4_1409001","4"),
                    new Tuple<int, string>("302003_5_1409003","5"),
                },
                QuantityInterval = new List<Tuple<int, string>>()
                {
                    new Tuple<int, string>(0,"Seleccione"),
                    new Tuple<int, string>(1,"1"),
                    new Tuple<int, string>(2,"2"),
                    new Tuple<int, string>(3,"3"),
                    new Tuple<int, string>(4,"4"),
                    new Tuple<int, string>(10,"10"),
                }
            };

            string oResult = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(oSerialize);








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
                (ProjectPublicId,
                MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

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