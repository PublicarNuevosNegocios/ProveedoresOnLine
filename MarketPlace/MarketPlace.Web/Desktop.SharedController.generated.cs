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
namespace T4MVC.Desktop
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
                public readonly string _CM_CompareMenu = "_CM_CompareMenu";
                public readonly string _L_Footer = "_L_Footer";
                public readonly string _L_Header = "_L_Header";
                public readonly string _Layout = "_Layout";
                public readonly string _P_FI_Balance = "_P_FI_Balance";
                public readonly string _P_FI_BalanceItem = "_P_FI_BalanceItem";
                public readonly string _P_FI_Indicators = "_P_FI_Indicators";
                public readonly string _P_FI_IndicatorsItem = "_P_FI_IndicatorsItem";
                public readonly string _P_ProviderLite = "_P_ProviderLite";
                public readonly string _P_ProviderMenu = "_P_ProviderMenu";
                public readonly string _P_Search_Comparison = "_P_Search_Comparison";
                public readonly string _P_Search_Filter = "_P_Search_Filter";
                public readonly string _P_Search_Order = "_P_Search_Order";
                public readonly string _P_Search_Project = "_P_Search_Project";
                public readonly string _P_Search_Result = "_P_Search_Result";
                public readonly string _P_Search_Result_Item = "_P_Search_Result_Item";
                public readonly string _P_Search_Result_Pager = "_P_Search_Result_Pager";
                public readonly string _PJ_ProjectDetail_Header = "_PJ_ProjectDetail_Header";
                public readonly string _PJ_ProjectDetail_Provider_Item = "_PJ_ProjectDetail_Provider_Item";
                public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404001 = "_PJ_ProjectDetail_Provider_ItemCriteria_1404001";
                public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404002 = "_PJ_ProjectDetail_Provider_ItemCriteria_1404002";
                public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404003 = "_PJ_ProjectDetail_Provider_ItemCriteria_1404003";
                public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404004 = "_PJ_ProjectDetail_Provider_ItemCriteria_1404004";
                public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404005 = "_PJ_ProjectDetail_Provider_ItemCriteria_1404005";
                public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404006 = "_PJ_ProjectDetail_Provider_ItemCriteria_1404006";
                public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404007 = "_PJ_ProjectDetail_Provider_ItemCriteria_1404007";
                public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404008 = "_PJ_ProjectDetail_Provider_ItemCriteria_1404008";
                public readonly string _PJ_ProjectDetail_Provider_Menu = "_PJ_ProjectDetail_Provider_Menu";
                public readonly string _PJ_ProjectDetail_Provider_Result = "_PJ_ProjectDetail_Provider_Result";
                public readonly string _SV_ProgramSurvey = "_SV_ProgramSurvey";
                public readonly string _SV_SurveySearch_Result_Item = "_SV_SurveySearch_Result_Item";
                public readonly string _SV_SurveySearch_Result_Pager = "_SV_SurveySearch_Result_Pager";
            }
            public readonly string _CM_CompareMenu = "~/Areas/Desktop/Views/Shared/_CM_CompareMenu.cshtml";
            public readonly string _L_Footer = "~/Areas/Desktop/Views/Shared/_L_Footer.cshtml";
            public readonly string _L_Header = "~/Areas/Desktop/Views/Shared/_L_Header.cshtml";
            public readonly string _Layout = "~/Areas/Desktop/Views/Shared/_Layout.cshtml";
            public readonly string _P_FI_Balance = "~/Areas/Desktop/Views/Shared/_P_FI_Balance.cshtml";
            public readonly string _P_FI_BalanceItem = "~/Areas/Desktop/Views/Shared/_P_FI_BalanceItem.cshtml";
            public readonly string _P_FI_Indicators = "~/Areas/Desktop/Views/Shared/_P_FI_Indicators.cshtml";
            public readonly string _P_FI_IndicatorsItem = "~/Areas/Desktop/Views/Shared/_P_FI_IndicatorsItem.cshtml";
            public readonly string _P_ProviderLite = "~/Areas/Desktop/Views/Shared/_P_ProviderLite.cshtml";
            public readonly string _P_ProviderMenu = "~/Areas/Desktop/Views/Shared/_P_ProviderMenu.cshtml";
            public readonly string _P_Search_Comparison = "~/Areas/Desktop/Views/Shared/_P_Search_Comparison.cshtml";
            public readonly string _P_Search_Filter = "~/Areas/Desktop/Views/Shared/_P_Search_Filter.cshtml";
            public readonly string _P_Search_Order = "~/Areas/Desktop/Views/Shared/_P_Search_Order.cshtml";
            public readonly string _P_Search_Project = "~/Areas/Desktop/Views/Shared/_P_Search_Project.cshtml";
            public readonly string _P_Search_Result = "~/Areas/Desktop/Views/Shared/_P_Search_Result.cshtml";
            public readonly string _P_Search_Result_Item = "~/Areas/Desktop/Views/Shared/_P_Search_Result_Item.cshtml";
            public readonly string _P_Search_Result_Pager = "~/Areas/Desktop/Views/Shared/_P_Search_Result_Pager.cshtml";
            public readonly string _PJ_ProjectDetail_Header = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Header.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_Item = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_Item.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404001 = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_ItemCriteria_1404001.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404002 = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_ItemCriteria_1404002.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404003 = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_ItemCriteria_1404003.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404004 = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_ItemCriteria_1404004.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404005 = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_ItemCriteria_1404005.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404006 = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_ItemCriteria_1404006.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404007 = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_ItemCriteria_1404007.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_ItemCriteria_1404008 = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_ItemCriteria_1404008.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_Menu = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_Menu.cshtml";
            public readonly string _PJ_ProjectDetail_Provider_Result = "~/Areas/Desktop/Views/Shared/_PJ_ProjectDetail_Provider_Result.cshtml";
            public readonly string _SV_ProgramSurvey = "~/Areas/Desktop/Views/Shared/_SV_ProgramSurvey.cshtml";
            public readonly string _SV_SurveySearch_Result_Item = "~/Areas/Desktop/Views/Shared/_SV_SurveySearch_Result_Item.cshtml";
            public readonly string _SV_SurveySearch_Result_Pager = "~/Areas/Desktop/Views/Shared/_SV_SurveySearch_Result_Pager.cshtml";
        }
    }

}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
