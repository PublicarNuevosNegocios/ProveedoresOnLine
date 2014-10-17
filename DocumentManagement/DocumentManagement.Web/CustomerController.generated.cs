// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
#pragma warning disable 1591, 3008, 3009
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
namespace DocumentManagement.Web.Controllers
{
    public partial class CustomerController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public CustomerController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected CustomerController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult UpsertCustomer()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpsertCustomer);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ListForm()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListForm);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UploadProvider()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UploadProvider);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public CustomerController Actions { get { return MVC.Customer; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Customer";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Customer";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string UpsertCustomer = "UpsertCustomer";
            public readonly string ListForm = "ListForm";
            public readonly string UploadProvider = "UploadProvider";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string UpsertCustomer = "UpsertCustomer";
            public const string ListForm = "ListForm";
            public const string UploadProvider = "UploadProvider";
        }


        static readonly ActionParamsClass_UpsertCustomer s_params_UpsertCustomer = new ActionParamsClass_UpsertCustomer();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpsertCustomer UpsertCustomerParams { get { return s_params_UpsertCustomer; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpsertCustomer
        {
            public readonly string CustomerPublicId = "CustomerPublicId";
        }
        static readonly ActionParamsClass_ListForm s_params_ListForm = new ActionParamsClass_ListForm();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListForm ListFormParams { get { return s_params_ListForm; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListForm
        {
            public readonly string CustomerPublicId = "CustomerPublicId";
        }
        static readonly ActionParamsClass_UploadProvider s_params_UploadProvider = new ActionParamsClass_UploadProvider();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UploadProvider UploadProviderParams { get { return s_params_UploadProvider; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UploadProvider
        {
            public readonly string CustomerPublicId = "CustomerPublicId";
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
                public readonly string ListForm = "ListForm";
                public readonly string UploadProvider = "UploadProvider";
                public readonly string UpsertCustomer = "UpsertCustomer";
            }
            public readonly string Index = "~/Views/Customer/Index.cshtml";
            public readonly string ListForm = "~/Views/Customer/ListForm.cshtml";
            public readonly string UploadProvider = "~/Views/Customer/UploadProvider.cshtml";
            public readonly string UpsertCustomer = "~/Views/Customer/UpsertCustomer.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_CustomerController : DocumentManagement.Web.Controllers.CustomerController
    {
        public T4MVC_CustomerController() : base(Dummy.Instance) { }

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
        partial void UpsertCustomerOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string CustomerPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult UpsertCustomer(string CustomerPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpsertCustomer);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "CustomerPublicId", CustomerPublicId);
            UpsertCustomerOverride(callInfo, CustomerPublicId);
            return callInfo;
        }

        [NonAction]
        partial void ListFormOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string CustomerPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult ListForm(string CustomerPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListForm);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "CustomerPublicId", CustomerPublicId);
            ListFormOverride(callInfo, CustomerPublicId);
            return callInfo;
        }

        [NonAction]
        partial void UploadProviderOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string CustomerPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult UploadProvider(string CustomerPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UploadProvider);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "CustomerPublicId", CustomerPublicId);
            UploadProviderOverride(callInfo, CustomerPublicId);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
