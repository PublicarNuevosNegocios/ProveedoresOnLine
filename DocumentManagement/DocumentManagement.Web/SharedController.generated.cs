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
                public readonly string _C_CustomerMenu = "_C_CustomerMenu";
                public readonly string _L_Footer = "_L_Footer";
                public readonly string _L_Header = "_L_Header";
                public readonly string _L_PrincipalMenu = "_L_PrincipalMenu";
                public readonly string _L_UserInfo = "_L_UserInfo";
                public readonly string _Layout = "_Layout";
                public readonly string _LayoutProviderForm = "_LayoutProviderForm";
                public readonly string _P_Menu = "_P_Menu";
                public readonly string _P_StepGeneric = "_P_StepGeneric";
                public readonly string _P_StepLogin = "_P_StepLogin";
            }
            public readonly string _C_CustomerMenu = "~/Views/Shared/_C_CustomerMenu.cshtml";
            public readonly string _L_Footer = "~/Views/Shared/_L_Footer.cshtml";
            public readonly string _L_Header = "~/Views/Shared/_L_Header.cshtml";
            public readonly string _L_PrincipalMenu = "~/Views/Shared/_L_PrincipalMenu.cshtml";
            public readonly string _L_UserInfo = "~/Views/Shared/_L_UserInfo.cshtml";
            public readonly string _Layout = "~/Views/Shared/_Layout.cshtml";
            public readonly string _LayoutProviderForm = "~/Views/Shared/_LayoutProviderForm.cshtml";
            public readonly string _P_Menu = "~/Views/Shared/_P_Menu.cshtml";
            public readonly string _P_StepGeneric = "~/Views/Shared/_P_StepGeneric.cshtml";
            public readonly string _P_StepLogin = "~/Views/Shared/_P_StepLogin.cshtml";
        }
    }

}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
