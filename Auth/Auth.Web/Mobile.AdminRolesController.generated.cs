// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Auth.Web.Areas.Mobile.Controllers
{
    public partial class AdminRolesController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AdminRolesController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected AdminRolesController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult AutorizationUpsert()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AutorizationUpsert);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AdminRolesController Actions { get { return MVC.Mobile.AdminRoles; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Mobile";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "AdminRoles";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "AdminRoles";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string AutorizationUpsert = "AutorizationUpsert";
            public readonly string AutorizationDelete = "AutorizationDelete";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string AutorizationUpsert = "AutorizationUpsert";
            public const string AutorizationDelete = "AutorizationDelete";
        }


        static readonly ActionParamsClass_AutorizationUpsert s_params_AutorizationUpsert = new ActionParamsClass_AutorizationUpsert();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AutorizationUpsert AutorizationUpsertParams { get { return s_params_AutorizationUpsert; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AutorizationUpsert
        {
            public readonly string AplicationId = "AplicationId";
            public readonly string RoleId = "RoleId";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string Index = "Index";
            }
            public readonly string Index = "~/Areas/Mobile/Views/AdminRoles/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_AdminRolesController : Auth.Web.Areas.Mobile.Controllers.AdminRolesController
    {
        public T4MVC_AdminRolesController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void AutorizationUpsertOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, SessionManager.Models.Auth.enumApplication AplicationId, SessionManager.Models.Auth.enumRole RoleId);

        [NonAction]
        public override System.Web.Mvc.ActionResult AutorizationUpsert(SessionManager.Models.Auth.enumApplication AplicationId, SessionManager.Models.Auth.enumRole RoleId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AutorizationUpsert);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "AplicationId", AplicationId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "RoleId", RoleId);
            AutorizationUpsertOverride(callInfo, AplicationId, RoleId);
            return callInfo;
        }

        [NonAction]
        partial void AutorizationDeleteOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult AutorizationDelete()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AutorizationDelete);
            AutorizationDeleteOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
