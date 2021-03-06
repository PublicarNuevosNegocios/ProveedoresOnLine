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
    public partial class ProviderFormController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ProviderFormController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ProviderFormController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult LoginProvider()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LoginProvider);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpsertGenericStep()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpsertGenericStep);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult AdminProvider()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AdminProvider);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult AdminLogProvider()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AdminLogProvider);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpsertAdminProvider()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpsertAdminProvider);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult DuplicateForm()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DuplicateForm);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.FileResult GetPdfFileBytes()
        {
            return new T4MVC_System_Web_Mvc_FileResult(Area, Name, ActionNames.GetPdfFileBytes);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult LoginProviderChangesControl()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LoginProviderChangesControl);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SyncPartnersGrid()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SyncPartnersGrid);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SyncMultipleFileGrid()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SyncMultipleFileGrid);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ProviderFormController Actions { get { return MVC.ProviderForm; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "ProviderForm";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "ProviderForm";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string LoginProvider = "LoginProvider";
            public readonly string UpsertGenericStep = "UpsertGenericStep";
            public readonly string AdminProvider = "AdminProvider";
            public readonly string AdminLogProvider = "AdminLogProvider";
            public readonly string UpsertAdminProvider = "UpsertAdminProvider";
            public readonly string DuplicateForm = "DuplicateForm";
            public readonly string GetPdfFileBytes = "GetPdfFileBytes";
            public readonly string LoginProviderChangesControl = "LoginProviderChangesControl";
            public readonly string SyncPartnersGrid = "SyncPartnersGrid";
            public readonly string SyncMultipleFileGrid = "SyncMultipleFileGrid";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string LoginProvider = "LoginProvider";
            public const string UpsertGenericStep = "UpsertGenericStep";
            public const string AdminProvider = "AdminProvider";
            public const string AdminLogProvider = "AdminLogProvider";
            public const string UpsertAdminProvider = "UpsertAdminProvider";
            public const string DuplicateForm = "DuplicateForm";
            public const string GetPdfFileBytes = "GetPdfFileBytes";
            public const string LoginProviderChangesControl = "LoginProviderChangesControl";
            public const string SyncPartnersGrid = "SyncPartnersGrid";
            public const string SyncMultipleFileGrid = "SyncMultipleFileGrid";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
            public readonly string FormPublicId = "FormPublicId";
            public readonly string StepId = "StepId";
            public readonly string msg = "msg";
        }
        static readonly ActionParamsClass_LoginProvider s_params_LoginProvider = new ActionParamsClass_LoginProvider();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_LoginProvider LoginProviderParams { get { return s_params_LoginProvider; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_LoginProvider
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
            public readonly string FormPublicId = "FormPublicId";
            public readonly string CustomerPublicId = "CustomerPublicId";
        }
        static readonly ActionParamsClass_UpsertGenericStep s_params_UpsertGenericStep = new ActionParamsClass_UpsertGenericStep();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpsertGenericStep UpsertGenericStepParams { get { return s_params_UpsertGenericStep; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpsertGenericStep
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
            public readonly string FormPublicId = "FormPublicId";
            public readonly string StepId = "StepId";
            public readonly string NewStepId = "NewStepId";
            public readonly string IsSync = "IsSync";
        }
        static readonly ActionParamsClass_AdminProvider s_params_AdminProvider = new ActionParamsClass_AdminProvider();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AdminProvider AdminProviderParams { get { return s_params_AdminProvider; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AdminProvider
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
            public readonly string FormPublicId = "FormPublicId";
        }
        static readonly ActionParamsClass_AdminLogProvider s_params_AdminLogProvider = new ActionParamsClass_AdminLogProvider();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AdminLogProvider AdminLogProviderParams { get { return s_params_AdminLogProvider; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AdminLogProvider
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
            public readonly string FormPublicId = "FormPublicId";
        }
        static readonly ActionParamsClass_UpsertAdminProvider s_params_UpsertAdminProvider = new ActionParamsClass_UpsertAdminProvider();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpsertAdminProvider UpsertAdminProviderParams { get { return s_params_UpsertAdminProvider; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpsertAdminProvider
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
            public readonly string FormPublicId = "FormPublicId";
        }
        static readonly ActionParamsClass_DuplicateForm s_params_DuplicateForm = new ActionParamsClass_DuplicateForm();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DuplicateForm DuplicateFormParams { get { return s_params_DuplicateForm; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DuplicateForm
        {
            public readonly string CustomerPublicId = "CustomerPublicId";
        }
        static readonly ActionParamsClass_GetPdfFileBytes s_params_GetPdfFileBytes = new ActionParamsClass_GetPdfFileBytes();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetPdfFileBytes GetPdfFileBytesParams { get { return s_params_GetPdfFileBytes; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetPdfFileBytes
        {
            public readonly string FilePath = "FilePath";
        }
        static readonly ActionParamsClass_LoginProviderChangesControl s_params_LoginProviderChangesControl = new ActionParamsClass_LoginProviderChangesControl();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_LoginProviderChangesControl LoginProviderChangesControlParams { get { return s_params_LoginProviderChangesControl; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_LoginProviderChangesControl
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
            public readonly string FormPublicId = "FormPublicId";
            public readonly string CustomerPublicId = "CustomerPublicId";
            public readonly string IdentificationType = "IdentificationType";
            public readonly string IdentificationNumber = "IdentificationNumber";
        }
        static readonly ActionParamsClass_SyncPartnersGrid s_params_SyncPartnersGrid = new ActionParamsClass_SyncPartnersGrid();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SyncPartnersGrid SyncPartnersGridParams { get { return s_params_SyncPartnersGrid; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SyncPartnersGrid
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
            public readonly string IdentificationNumber = "IdentificationNumber";
            public readonly string FullName = "FullName";
            public readonly string ProviderInfoId = "ProviderInfoId";
        }
        static readonly ActionParamsClass_SyncMultipleFileGrid s_params_SyncMultipleFileGrid = new ActionParamsClass_SyncMultipleFileGrid();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SyncMultipleFileGrid SyncMultipleFileGridParams { get { return s_params_SyncMultipleFileGrid; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SyncMultipleFileGrid
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
            public readonly string ProviderInfoUrl = "ProviderInfoUrl";
            public readonly string Name = "Name";
            public readonly string ProviderInfoId = "ProviderInfoId";
            public readonly string ItemType = "ItemType";
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
                public readonly string AdminLogProvider = "AdminLogProvider";
                public readonly string AdminProvider = "AdminProvider";
                public readonly string Index = "Index";
            }
            public readonly string AdminLogProvider = "~/Views/ProviderForm/AdminLogProvider.cshtml";
            public readonly string AdminProvider = "~/Views/ProviderForm/AdminProvider.cshtml";
            public readonly string Index = "~/Views/ProviderForm/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ProviderFormController : DocumentManagement.Web.Controllers.ProviderFormController
    {
        public T4MVC_ProviderFormController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId, string FormPublicId, string StepId, string msg);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index(string ProviderPublicId, string FormPublicId, string StepId, string msg)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FormPublicId", FormPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "StepId", StepId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "msg", msg);
            IndexOverride(callInfo, ProviderPublicId, FormPublicId, StepId, msg);
            return callInfo;
        }

        [NonAction]
        partial void LoginProviderOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId, string FormPublicId, string CustomerPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult LoginProvider(string ProviderPublicId, string FormPublicId, string CustomerPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LoginProvider);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FormPublicId", FormPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "CustomerPublicId", CustomerPublicId);
            LoginProviderOverride(callInfo, ProviderPublicId, FormPublicId, CustomerPublicId);
            return callInfo;
        }

        [NonAction]
        partial void UpsertGenericStepOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId, string FormPublicId, string StepId, string NewStepId, string IsSync);

        [NonAction]
        public override System.Web.Mvc.ActionResult UpsertGenericStep(string ProviderPublicId, string FormPublicId, string StepId, string NewStepId, string IsSync)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpsertGenericStep);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FormPublicId", FormPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "StepId", StepId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "NewStepId", NewStepId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "IsSync", IsSync);
            UpsertGenericStepOverride(callInfo, ProviderPublicId, FormPublicId, StepId, NewStepId, IsSync);
            return callInfo;
        }

        [NonAction]
        partial void AdminProviderOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId, string FormPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult AdminProvider(string ProviderPublicId, string FormPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AdminProvider);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FormPublicId", FormPublicId);
            AdminProviderOverride(callInfo, ProviderPublicId, FormPublicId);
            return callInfo;
        }

        [NonAction]
        partial void AdminLogProviderOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId, string FormPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult AdminLogProvider(string ProviderPublicId, string FormPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AdminLogProvider);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FormPublicId", FormPublicId);
            AdminLogProviderOverride(callInfo, ProviderPublicId, FormPublicId);
            return callInfo;
        }

        [NonAction]
        partial void UpsertAdminProviderOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId, string FormPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult UpsertAdminProvider(string ProviderPublicId, string FormPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpsertAdminProvider);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FormPublicId", FormPublicId);
            UpsertAdminProviderOverride(callInfo, ProviderPublicId, FormPublicId);
            return callInfo;
        }

        [NonAction]
        partial void DuplicateFormOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string CustomerPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult DuplicateForm(string CustomerPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DuplicateForm);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "CustomerPublicId", CustomerPublicId);
            DuplicateFormOverride(callInfo, CustomerPublicId);
            return callInfo;
        }

        [NonAction]
        partial void GetPdfFileBytesOverride(T4MVC_System_Web_Mvc_FileResult callInfo, string FilePath);

        [NonAction]
        public override System.Web.Mvc.FileResult GetPdfFileBytes(string FilePath)
        {
            var callInfo = new T4MVC_System_Web_Mvc_FileResult(Area, Name, ActionNames.GetPdfFileBytes);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FilePath", FilePath);
            GetPdfFileBytesOverride(callInfo, FilePath);
            return callInfo;
        }

        [NonAction]
        partial void LoginProviderChangesControlOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId, string FormPublicId, string CustomerPublicId, string IdentificationType, string IdentificationNumber);

        [NonAction]
        public override System.Web.Mvc.ActionResult LoginProviderChangesControl(string ProviderPublicId, string FormPublicId, string CustomerPublicId, string IdentificationType, string IdentificationNumber)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LoginProviderChangesControl);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FormPublicId", FormPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "CustomerPublicId", CustomerPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "IdentificationType", IdentificationType);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "IdentificationNumber", IdentificationNumber);
            LoginProviderChangesControlOverride(callInfo, ProviderPublicId, FormPublicId, CustomerPublicId, IdentificationType, IdentificationNumber);
            return callInfo;
        }

        [NonAction]
        partial void SyncPartnersGridOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId, string IdentificationNumber, string FullName, string ProviderInfoId);

        [NonAction]
        public override System.Web.Mvc.ActionResult SyncPartnersGrid(string ProviderPublicId, string IdentificationNumber, string FullName, string ProviderInfoId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SyncPartnersGrid);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "IdentificationNumber", IdentificationNumber);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FullName", FullName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderInfoId", ProviderInfoId);
            SyncPartnersGridOverride(callInfo, ProviderPublicId, IdentificationNumber, FullName, ProviderInfoId);
            return callInfo;
        }

        [NonAction]
        partial void SyncMultipleFileGridOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId, string ProviderInfoUrl, string Name, string ProviderInfoId, string ItemType);

        [NonAction]
        public override System.Web.Mvc.ActionResult SyncMultipleFileGrid(string ProviderPublicId, string ProviderInfoUrl, string Name, string ProviderInfoId, string ItemType)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SyncMultipleFileGrid);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderInfoUrl", ProviderInfoUrl);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Name", Name);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderInfoId", ProviderInfoId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ItemType", ItemType);
            SyncMultipleFileGridOverride(callInfo, ProviderPublicId, ProviderInfoUrl, Name, ProviderInfoId, ItemType);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
