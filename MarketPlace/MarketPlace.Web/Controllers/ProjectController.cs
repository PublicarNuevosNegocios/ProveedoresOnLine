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
        public virtual ActionResult Index(
                string ProjectStatus,
                string SearchParam,
                string SearchFilter,
                string SearchOrderType,
                string OrderOrientation,
                string PageNumber
            )
        {
            MarketPlace.Models.Provider.ProviderSearchViewModel oModel = null;
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

            //get basic search model
            oModel = new Models.Provider.ProviderSearchViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                SearchParam = SearchParam,
                SearchFilter = SearchFilter == null ? null : (SearchFilter.Trim(new char[] { ',' }).Length > 0 ? SearchFilter.Trim(new char[] { ',' }) : null),
                SearchOrderType = string.IsNullOrEmpty(SearchOrderType) ? MarketPlace.Models.General.enumSearchOrderType.Relevance : (MarketPlace.Models.General.enumSearchOrderType)Convert.ToInt32(SearchOrderType),
                OrderOrientation = string.IsNullOrEmpty(OrderOrientation) ? false : ((OrderOrientation.Trim().ToLower() == "1") || (OrderOrientation.Trim().ToLower() == "true")),
                PageNumber = 0,
                RelatedSearchProject = new List<Models.Project.ProjectSearchViewModel>(),
            };

            if (MarketPlace.Models.General.SessionModel.CurrentCompany != null &&
                !string.IsNullOrEmpty(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId))
            {
                //get basic search model
                int oTotalRows;                
                List<ProveedoresOnLine.ProjectModule.Models.ProjectModel> oProjects =
                    ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectSearch(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    SearchParam, null, Convert.ToInt32(!string.IsNullOrEmpty(PageNumber) ? PageNumber : "0"), 100, out oTotalRows);

                if (oProjects != null && oProjects.Count > 0)
                {
                    oProjects.All(x =>
                    {
                        oModel.RelatedSearchProject.Add(new MarketPlace.Models.Project.ProjectSearchViewModel(x));
                        return true;
                    });
                }
            }
            return View(oModel);
        }

        public virtual ActionResult ProjectDetail(string ProjectPublicId)
        {
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

        public virtual ActionResult ProjectProviderDetail
            (string ProjectPublicId,
            string ProviderPublicId,
            string EvaluationAreaId)
        {
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

            ProveedoresOnLine.ProjectModule.Models.ProjectModel oCurrentProject = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.
                ProjectGetByIdProviderDetail
                (ProjectPublicId,
                MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                ProviderPublicId);

            MarketPlace.Models.Project.ProjectViewModel oModel = new Models.Project.ProjectViewModel(oCurrentProject, ProviderPublicId);

            //get current evaluation area
            if (!string.IsNullOrEmpty(EvaluationAreaId))
            {
                oModel.RelatedProjectConfig.SetCurrentEvaluationArea(Convert.ToInt32(EvaluationAreaId.Replace(" ", "")));
            }

            if (oModel.RelatedProjectConfig.CurrentEvaluationArea == null)
            {
                oModel.RelatedProjectConfig.SetCurrentEvaluationArea(null);
            }

            return View(oModel);
        }
    }
}