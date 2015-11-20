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
namespace MarketPlace.Web.Controllers
{
    public partial class ThirdKnowledgeController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ThirdKnowledgeController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ThirdKnowledgeController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult TKDetailSingleSearch()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TKDetailSingleSearch);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult TKThirdKnowledgeSearch()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TKThirdKnowledgeSearch);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult TKThirdKnowledgeDetail()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TKThirdKnowledgeDetail);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ThirdKnowledgeController Actions { get { return MVC.ThirdKnowledge; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "ThirdKnowledge";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "ThirdKnowledge";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string TKSingleSearch = "TKSingleSearch";
            public readonly string TKMasiveSearch = "TKMasiveSearch";
            public readonly string TKDetailSingleSearch = "TKDetailSingleSearch";
            public readonly string TKThirdKnowledgeSearch = "TKThirdKnowledgeSearch";
            public readonly string TKThirdKnowledgeDetail = "TKThirdKnowledgeDetail";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string TKSingleSearch = "TKSingleSearch";
            public const string TKMasiveSearch = "TKMasiveSearch";
            public const string TKDetailSingleSearch = "TKDetailSingleSearch";
            public const string TKThirdKnowledgeSearch = "TKThirdKnowledgeSearch";
            public const string TKThirdKnowledgeDetail = "TKThirdKnowledgeDetail";
        }


        static readonly ActionParamsClass_TKDetailSingleSearch s_params_TKDetailSingleSearch = new ActionParamsClass_TKDetailSingleSearch();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_TKDetailSingleSearch TKDetailSingleSearchParams { get { return s_params_TKDetailSingleSearch; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_TKDetailSingleSearch
        {
            public readonly string QueryBasicPublicId = "QueryBasicPublicId";
            public readonly string ReturnUrl = "ReturnUrl";
        }
        static readonly ActionParamsClass_TKThirdKnowledgeSearch s_params_TKThirdKnowledgeSearch = new ActionParamsClass_TKThirdKnowledgeSearch();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_TKThirdKnowledgeSearch TKThirdKnowledgeSearchParams { get { return s_params_TKThirdKnowledgeSearch; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_TKThirdKnowledgeSearch
        {
            public readonly string SearchOrderType = "SearchOrderType";
            public readonly string OrderOrientation = "OrderOrientation";
            public readonly string PageNumber = "PageNumber";
            public readonly string InitDate = "InitDate";
            public readonly string EndDate = "EndDate";
        }
        static readonly ActionParamsClass_TKThirdKnowledgeDetail s_params_TKThirdKnowledgeDetail = new ActionParamsClass_TKThirdKnowledgeDetail();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_TKThirdKnowledgeDetail TKThirdKnowledgeDetailParams { get { return s_params_TKThirdKnowledgeDetail; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_TKThirdKnowledgeDetail
        {
            public readonly string QueryPublicId = "QueryPublicId";
            public readonly string InitDate = "InitDate";
            public readonly string EndDate = "EndDate";
            public readonly string Enable = "Enable";
            public readonly string IsSuccess = "IsSuccess";
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
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ThirdKnowledgeController : MarketPlace.Web.Controllers.ThirdKnowledgeController
    {
        public T4MVC_ThirdKnowledgeController() : base(Dummy.Instance) { }

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
        partial void TKSingleSearchOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult TKSingleSearch()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TKSingleSearch);
            TKSingleSearchOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void TKMasiveSearchOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult TKMasiveSearch()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TKMasiveSearch);
            TKMasiveSearchOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void TKDetailSingleSearchOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string QueryBasicPublicId, string ReturnUrl);

        [NonAction]
        public override System.Web.Mvc.ActionResult TKDetailSingleSearch(string QueryBasicPublicId, string ReturnUrl)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TKDetailSingleSearch);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "QueryBasicPublicId", QueryBasicPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ReturnUrl", ReturnUrl);
            TKDetailSingleSearchOverride(callInfo, QueryBasicPublicId, ReturnUrl);
            return callInfo;
        }

        [NonAction]
        partial void TKThirdKnowledgeSearchOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string SearchOrderType, string OrderOrientation, string PageNumber, string InitDate, string EndDate);

        [NonAction]
        public override System.Web.Mvc.ActionResult TKThirdKnowledgeSearch(string SearchOrderType, string OrderOrientation, string PageNumber, string InitDate, string EndDate)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TKThirdKnowledgeSearch);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "SearchOrderType", SearchOrderType);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "OrderOrientation", OrderOrientation);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "PageNumber", PageNumber);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "InitDate", InitDate);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "EndDate", EndDate);
            TKThirdKnowledgeSearchOverride(callInfo, SearchOrderType, OrderOrientation, PageNumber, InitDate, EndDate);
            return callInfo;
        }

        [NonAction]
        partial void TKThirdKnowledgeDetailOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string QueryPublicId, string InitDate, string EndDate, string Enable, string IsSuccess);

        [NonAction]
        public override System.Web.Mvc.ActionResult TKThirdKnowledgeDetail(string QueryPublicId, string InitDate, string EndDate, string Enable, string IsSuccess)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TKThirdKnowledgeDetail);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "QueryPublicId", QueryPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "InitDate", InitDate);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "EndDate", EndDate);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Enable", Enable);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "IsSuccess", IsSuccess);
            TKThirdKnowledgeDetailOverride(callInfo, QueryPublicId, InitDate, EndDate, Enable, IsSuccess);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
