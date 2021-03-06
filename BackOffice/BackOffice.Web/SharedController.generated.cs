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
namespace T4MVC
{
    public class SharedController
    {

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
                public readonly string _C_CustomerActions = "_C_CustomerActions";
                public readonly string _C_CustomerMenu = "_C_CustomerMenu";
                public readonly string _CD_CustomerForm = "_CD_CustomerForm";
                public readonly string _CD_CustomField_101001 = "_CD_CustomField_101001";
                public readonly string _CD_CustomField_101002 = "_CD_CustomField_101002";
                public readonly string _CD_CustomField_101003 = "_CD_CustomField_101003";
                public readonly string _CD_CustomField_101004 = "_CD_CustomField_101004";
                public readonly string _F_FileUpload = "_F_FileUpload";
                public readonly string _L_Footer = "_L_Footer";
                public readonly string _L_Header = "_L_Header";
                public readonly string _Layout = "_Layout";
                public readonly string _P_ProviderActions = "_P_ProviderActions";
                public readonly string _P_ProviderFilter = "_P_ProviderFilter";
                public readonly string _P_ProviderMenu = "_P_ProviderMenu";
                public readonly string _PJ_EvaluationCriteria_1404001 = "_PJ_EvaluationCriteria_1404001";
                public readonly string _PJ_EvaluationCriteria_1404002 = "_PJ_EvaluationCriteria_1404002";
                public readonly string _PJ_EvaluationCriteria_1404003 = "_PJ_EvaluationCriteria_1404003";
                public readonly string _PJ_EvaluationCriteria_1404004 = "_PJ_EvaluationCriteria_1404004";
                public readonly string _PJ_EvaluationCriteria_1404005 = "_PJ_EvaluationCriteria_1404005";
                public readonly string _PJ_EvaluationCriteria_1404006 = "_PJ_EvaluationCriteria_1404006";
                public readonly string _PJ_EvaluationCriteria_1404007 = "_PJ_EvaluationCriteria_1404007";
                public readonly string _PJ_EvaluationCriteria_1404008 = "_PJ_EvaluationCriteria_1404008";
            }
            public readonly string _C_CustomerActions = "~/Views/Shared/_C_CustomerActions.cshtml";
            public readonly string _C_CustomerMenu = "~/Views/Shared/_C_CustomerMenu.cshtml";
            public readonly string _CD_CustomerForm = "~/Views/Shared/_CD_CustomerForm.cshtml";
            public readonly string _CD_CustomField_101001 = "~/Views/Shared/_CD_CustomField_101001.cshtml";
            public readonly string _CD_CustomField_101002 = "~/Views/Shared/_CD_CustomField_101002.cshtml";
            public readonly string _CD_CustomField_101003 = "~/Views/Shared/_CD_CustomField_101003.cshtml";
            public readonly string _CD_CustomField_101004 = "~/Views/Shared/_CD_CustomField_101004.cshtml";
            public readonly string _F_FileUpload = "~/Views/Shared/_F_FileUpload.cshtml";
            public readonly string _L_Footer = "~/Views/Shared/_L_Footer.cshtml";
            public readonly string _L_Header = "~/Views/Shared/_L_Header.cshtml";
            public readonly string _Layout = "~/Views/Shared/_Layout.cshtml";
            public readonly string _P_ProviderActions = "~/Views/Shared/_P_ProviderActions.cshtml";
            public readonly string _P_ProviderFilter = "~/Views/Shared/_P_ProviderFilter.cshtml";
            public readonly string _P_ProviderMenu = "~/Views/Shared/_P_ProviderMenu.cshtml";
            public readonly string _PJ_EvaluationCriteria_1404001 = "~/Views/Shared/_PJ_EvaluationCriteria_1404001.cshtml";
            public readonly string _PJ_EvaluationCriteria_1404002 = "~/Views/Shared/_PJ_EvaluationCriteria_1404002.cshtml";
            public readonly string _PJ_EvaluationCriteria_1404003 = "~/Views/Shared/_PJ_EvaluationCriteria_1404003.cshtml";
            public readonly string _PJ_EvaluationCriteria_1404004 = "~/Views/Shared/_PJ_EvaluationCriteria_1404004.cshtml";
            public readonly string _PJ_EvaluationCriteria_1404005 = "~/Views/Shared/_PJ_EvaluationCriteria_1404005.cshtml";
            public readonly string _PJ_EvaluationCriteria_1404006 = "~/Views/Shared/_PJ_EvaluationCriteria_1404006.cshtml";
            public readonly string _PJ_EvaluationCriteria_1404007 = "~/Views/Shared/_PJ_EvaluationCriteria_1404007.cshtml";
            public readonly string _PJ_EvaluationCriteria_1404008 = "~/Views/Shared/_PJ_EvaluationCriteria_1404008.cshtml";
        }
    }

}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
