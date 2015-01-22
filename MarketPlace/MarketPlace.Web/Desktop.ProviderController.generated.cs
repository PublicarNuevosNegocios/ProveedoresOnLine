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
namespace MarketPlace.Web.Areas.Desktop.Controllers
{
    public partial class ProviderController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ProviderController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ProviderController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult Search()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Search);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GIProviderInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GIProviderInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GICompanyContactInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GICompanyContactInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GIPersonContactInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GIPersonContactInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GIBranchInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GIBranchInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GIDistributorInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GIDistributorInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult CIExperiencesInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CIExperiencesInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult HICertificationsInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.HICertificationsInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult HIHealtyPoliticInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.HIHealtyPoliticInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult HIRiskPoliciesInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.HIRiskPoliciesInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult FIBalanceSheetInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FIBalanceSheetInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult FITaxInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FITaxInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult FIIncomeStatementInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FIIncomeStatementInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult FIBankInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FIBankInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult LIChaimberOfCommerceInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LIChaimberOfCommerceInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult LIRutInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LIRutInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult LICIFINInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LICIFINInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult LISARLAFTInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LISARLAFTInfo);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult LIResolutionInfo()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LIResolutionInfo);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ProviderController Actions { get { return MVC.Desktop.Provider; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Desktop";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Provider";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Provider";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Search = "Search";
            public readonly string GIProviderInfo = "GIProviderInfo";
            public readonly string GICompanyContactInfo = "GICompanyContactInfo";
            public readonly string GIPersonContactInfo = "GIPersonContactInfo";
            public readonly string GIBranchInfo = "GIBranchInfo";
            public readonly string GIDistributorInfo = "GIDistributorInfo";
            public readonly string CIExperiencesInfo = "CIExperiencesInfo";
            public readonly string HICertificationsInfo = "HICertificationsInfo";
            public readonly string HIHealtyPoliticInfo = "HIHealtyPoliticInfo";
            public readonly string HIRiskPoliciesInfo = "HIRiskPoliciesInfo";
            public readonly string FIBalanceSheetInfo = "FIBalanceSheetInfo";
            public readonly string FITaxInfo = "FITaxInfo";
            public readonly string FIIncomeStatementInfo = "FIIncomeStatementInfo";
            public readonly string FIBankInfo = "FIBankInfo";
            public readonly string LIChaimberOfCommerceInfo = "LIChaimberOfCommerceInfo";
            public readonly string LIRutInfo = "LIRutInfo";
            public readonly string LICIFINInfo = "LICIFINInfo";
            public readonly string LISARLAFTInfo = "LISARLAFTInfo";
            public readonly string LIResolutionInfo = "LIResolutionInfo";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Search = "Search";
            public const string GIProviderInfo = "GIProviderInfo";
            public const string GICompanyContactInfo = "GICompanyContactInfo";
            public const string GIPersonContactInfo = "GIPersonContactInfo";
            public const string GIBranchInfo = "GIBranchInfo";
            public const string GIDistributorInfo = "GIDistributorInfo";
            public const string CIExperiencesInfo = "CIExperiencesInfo";
            public const string HICertificationsInfo = "HICertificationsInfo";
            public const string HIHealtyPoliticInfo = "HIHealtyPoliticInfo";
            public const string HIRiskPoliciesInfo = "HIRiskPoliciesInfo";
            public const string FIBalanceSheetInfo = "FIBalanceSheetInfo";
            public const string FITaxInfo = "FITaxInfo";
            public const string FIIncomeStatementInfo = "FIIncomeStatementInfo";
            public const string FIBankInfo = "FIBankInfo";
            public const string LIChaimberOfCommerceInfo = "LIChaimberOfCommerceInfo";
            public const string LIRutInfo = "LIRutInfo";
            public const string LICIFINInfo = "LICIFINInfo";
            public const string LISARLAFTInfo = "LISARLAFTInfo";
            public const string LIResolutionInfo = "LIResolutionInfo";
        }


        static readonly ActionParamsClass_Search s_params_Search = new ActionParamsClass_Search();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Search SearchParams { get { return s_params_Search; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Search
        {
            public readonly string SearchParam = "SearchParam";
            public readonly string SearchFilter = "SearchFilter";
            public readonly string SearchOrderType = "SearchOrderType";
            public readonly string OrderOrientation = "OrderOrientation";
            public readonly string PageNumber = "PageNumber";
        }
        static readonly ActionParamsClass_GIProviderInfo s_params_GIProviderInfo = new ActionParamsClass_GIProviderInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GIProviderInfo GIProviderInfoParams { get { return s_params_GIProviderInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GIProviderInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_GICompanyContactInfo s_params_GICompanyContactInfo = new ActionParamsClass_GICompanyContactInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GICompanyContactInfo GICompanyContactInfoParams { get { return s_params_GICompanyContactInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GICompanyContactInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_GIPersonContactInfo s_params_GIPersonContactInfo = new ActionParamsClass_GIPersonContactInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GIPersonContactInfo GIPersonContactInfoParams { get { return s_params_GIPersonContactInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GIPersonContactInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_GIBranchInfo s_params_GIBranchInfo = new ActionParamsClass_GIBranchInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GIBranchInfo GIBranchInfoParams { get { return s_params_GIBranchInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GIBranchInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_GIDistributorInfo s_params_GIDistributorInfo = new ActionParamsClass_GIDistributorInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GIDistributorInfo GIDistributorInfoParams { get { return s_params_GIDistributorInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GIDistributorInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_CIExperiencesInfo s_params_CIExperiencesInfo = new ActionParamsClass_CIExperiencesInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CIExperiencesInfo CIExperiencesInfoParams { get { return s_params_CIExperiencesInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CIExperiencesInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_HICertificationsInfo s_params_HICertificationsInfo = new ActionParamsClass_HICertificationsInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_HICertificationsInfo HICertificationsInfoParams { get { return s_params_HICertificationsInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_HICertificationsInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_HIHealtyPoliticInfo s_params_HIHealtyPoliticInfo = new ActionParamsClass_HIHealtyPoliticInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_HIHealtyPoliticInfo HIHealtyPoliticInfoParams { get { return s_params_HIHealtyPoliticInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_HIHealtyPoliticInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_HIRiskPoliciesInfo s_params_HIRiskPoliciesInfo = new ActionParamsClass_HIRiskPoliciesInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_HIRiskPoliciesInfo HIRiskPoliciesInfoParams { get { return s_params_HIRiskPoliciesInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_HIRiskPoliciesInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_FIBalanceSheetInfo s_params_FIBalanceSheetInfo = new ActionParamsClass_FIBalanceSheetInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_FIBalanceSheetInfo FIBalanceSheetInfoParams { get { return s_params_FIBalanceSheetInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_FIBalanceSheetInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_FITaxInfo s_params_FITaxInfo = new ActionParamsClass_FITaxInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_FITaxInfo FITaxInfoParams { get { return s_params_FITaxInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_FITaxInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_FIIncomeStatementInfo s_params_FIIncomeStatementInfo = new ActionParamsClass_FIIncomeStatementInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_FIIncomeStatementInfo FIIncomeStatementInfoParams { get { return s_params_FIIncomeStatementInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_FIIncomeStatementInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_FIBankInfo s_params_FIBankInfo = new ActionParamsClass_FIBankInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_FIBankInfo FIBankInfoParams { get { return s_params_FIBankInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_FIBankInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_LIChaimberOfCommerceInfo s_params_LIChaimberOfCommerceInfo = new ActionParamsClass_LIChaimberOfCommerceInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_LIChaimberOfCommerceInfo LIChaimberOfCommerceInfoParams { get { return s_params_LIChaimberOfCommerceInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_LIChaimberOfCommerceInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_LIRutInfo s_params_LIRutInfo = new ActionParamsClass_LIRutInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_LIRutInfo LIRutInfoParams { get { return s_params_LIRutInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_LIRutInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_LICIFINInfo s_params_LICIFINInfo = new ActionParamsClass_LICIFINInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_LICIFINInfo LICIFINInfoParams { get { return s_params_LICIFINInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_LICIFINInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_LISARLAFTInfo s_params_LISARLAFTInfo = new ActionParamsClass_LISARLAFTInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_LISARLAFTInfo LISARLAFTInfoParams { get { return s_params_LISARLAFTInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_LISARLAFTInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
        }
        static readonly ActionParamsClass_LIResolutionInfo s_params_LIResolutionInfo = new ActionParamsClass_LIResolutionInfo();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_LIResolutionInfo LIResolutionInfoParams { get { return s_params_LIResolutionInfo; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_LIResolutionInfo
        {
            public readonly string ProviderPublicId = "ProviderPublicId";
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
                public readonly string CIExperiencesInfo = "CIExperiencesInfo";
                public readonly string FIBalanceSheetInfo = "FIBalanceSheetInfo";
                public readonly string FIBankInfo = "FIBankInfo";
                public readonly string FIIncomeStatementInfo = "FIIncomeStatementInfo";
                public readonly string FITaxInfo = "FITaxInfo";
                public readonly string GIBranchInfo = "GIBranchInfo";
                public readonly string GICompanyContactInfo = "GICompanyContactInfo";
                public readonly string GIDistributorInfo = "GIDistributorInfo";
                public readonly string GIPersonContactInfo = "GIPersonContactInfo";
                public readonly string GIProviderInfo = "GIProviderInfo";
                public readonly string HICertificationsInfo = "HICertificationsInfo";
                public readonly string HIHealtyPoliticInfo = "HIHealtyPoliticInfo";
                public readonly string HIRiskPoliciesInfo = "HIRiskPoliciesInfo";
                public readonly string Index = "Index";
                public readonly string LIChaimberOfCommerceInfo = "LIChaimberOfCommerceInfo";
                public readonly string LICIFINInfo = "LICIFINInfo";
                public readonly string LIResolutionInfo = "LIResolutionInfo";
                public readonly string LIRutInfo = "LIRutInfo";
                public readonly string LISARLAFTInfo = "LISARLAFTInfo";
                public readonly string Search = "Search";
            }
            public readonly string CIExperiencesInfo = "~/Areas/Desktop/Views/Provider/CIExperiencesInfo.cshtml";
            public readonly string FIBalanceSheetInfo = "~/Areas/Desktop/Views/Provider/FIBalanceSheetInfo.cshtml";
            public readonly string FIBankInfo = "~/Areas/Desktop/Views/Provider/FIBankInfo.cshtml";
            public readonly string FIIncomeStatementInfo = "~/Areas/Desktop/Views/Provider/FIIncomeStatementInfo.cshtml";
            public readonly string FITaxInfo = "~/Areas/Desktop/Views/Provider/FITaxInfo.cshtml";
            public readonly string GIBranchInfo = "~/Areas/Desktop/Views/Provider/GIBranchInfo.cshtml";
            public readonly string GICompanyContactInfo = "~/Areas/Desktop/Views/Provider/GICompanyContactInfo.cshtml";
            public readonly string GIDistributorInfo = "~/Areas/Desktop/Views/Provider/GIDistributorInfo.cshtml";
            public readonly string GIPersonContactInfo = "~/Areas/Desktop/Views/Provider/GIPersonContactInfo.cshtml";
            public readonly string GIProviderInfo = "~/Areas/Desktop/Views/Provider/GIProviderInfo.cshtml";
            public readonly string HICertificationsInfo = "~/Areas/Desktop/Views/Provider/HICertificationsInfo.cshtml";
            public readonly string HIHealtyPoliticInfo = "~/Areas/Desktop/Views/Provider/HIHealtyPoliticInfo.cshtml";
            public readonly string HIRiskPoliciesInfo = "~/Areas/Desktop/Views/Provider/HIRiskPoliciesInfo.cshtml";
            public readonly string Index = "~/Areas/Desktop/Views/Provider/Index.cshtml";
            public readonly string LIChaimberOfCommerceInfo = "~/Areas/Desktop/Views/Provider/LIChaimberOfCommerceInfo.cshtml";
            public readonly string LICIFINInfo = "~/Areas/Desktop/Views/Provider/LICIFINInfo.cshtml";
            public readonly string LIResolutionInfo = "~/Areas/Desktop/Views/Provider/LIResolutionInfo.cshtml";
            public readonly string LIRutInfo = "~/Areas/Desktop/Views/Provider/LIRutInfo.cshtml";
            public readonly string LISARLAFTInfo = "~/Areas/Desktop/Views/Provider/LISARLAFTInfo.cshtml";
            public readonly string Search = "~/Areas/Desktop/Views/Provider/Search.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ProviderController : MarketPlace.Web.Areas.Desktop.Controllers.ProviderController
    {
        public T4MVC_ProviderController() : base(Dummy.Instance) { }

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
        partial void SearchOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string SearchParam, string SearchFilter, string SearchOrderType, string OrderOrientation, string PageNumber);

        [NonAction]
        public override System.Web.Mvc.ActionResult Search(string SearchParam, string SearchFilter, string SearchOrderType, string OrderOrientation, string PageNumber)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Search);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "SearchParam", SearchParam);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "SearchFilter", SearchFilter);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "SearchOrderType", SearchOrderType);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "OrderOrientation", OrderOrientation);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "PageNumber", PageNumber);
            SearchOverride(callInfo, SearchParam, SearchFilter, SearchOrderType, OrderOrientation, PageNumber);
            return callInfo;
        }

        [NonAction]
        partial void GIProviderInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult GIProviderInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GIProviderInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            GIProviderInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void GICompanyContactInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult GICompanyContactInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GICompanyContactInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            GICompanyContactInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void GIPersonContactInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult GIPersonContactInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GIPersonContactInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            GIPersonContactInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void GIBranchInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult GIBranchInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GIBranchInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            GIBranchInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void GIDistributorInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult GIDistributorInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GIDistributorInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            GIDistributorInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void CIExperiencesInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult CIExperiencesInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CIExperiencesInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            CIExperiencesInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void HICertificationsInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult HICertificationsInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.HICertificationsInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            HICertificationsInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void HIHealtyPoliticInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult HIHealtyPoliticInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.HIHealtyPoliticInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            HIHealtyPoliticInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void HIRiskPoliciesInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult HIRiskPoliciesInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.HIRiskPoliciesInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            HIRiskPoliciesInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void FIBalanceSheetInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult FIBalanceSheetInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FIBalanceSheetInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            FIBalanceSheetInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void FITaxInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult FITaxInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FITaxInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            FITaxInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void FIIncomeStatementInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult FIIncomeStatementInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FIIncomeStatementInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            FIIncomeStatementInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void FIBankInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult FIBankInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FIBankInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            FIBankInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void LIChaimberOfCommerceInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult LIChaimberOfCommerceInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LIChaimberOfCommerceInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            LIChaimberOfCommerceInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void LIRutInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult LIRutInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LIRutInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            LIRutInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void LICIFINInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult LICIFINInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LICIFINInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            LICIFINInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void LISARLAFTInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult LISARLAFTInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LISARLAFTInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            LISARLAFTInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

        [NonAction]
        partial void LIResolutionInfoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string ProviderPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult LIResolutionInfo(string ProviderPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LIResolutionInfo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ProviderPublicId", ProviderPublicId);
            LIResolutionInfoOverride(callInfo, ProviderPublicId);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
