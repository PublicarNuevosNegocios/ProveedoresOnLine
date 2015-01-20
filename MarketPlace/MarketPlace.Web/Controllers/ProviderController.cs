using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ProviderController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        #region General Info

        public virtual ActionResult GIProviderInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult GICompanyContactInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult GIPersonContactInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult GIBranchInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult GIDistributorInfo(string ProviderPublicId)
        {
            return View();
        }

        #endregion

        #region Commercial Info

        public virtual ActionResult CIExperiencesInfo(string ProviderPublicId)
        {
            return View();
        }

        #endregion

        #region HSEQ Info

        public virtual ActionResult HICertificationsInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult HIHealtyPoliticInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult HIRiskPoliciesInfo(string ProviderPublicId)
        {
            return View();
        }

        #endregion

        #region Financial Info

        public virtual ActionResult FIBalanceSheetInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult FITaxInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult FIIncomeStatementInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult FIBankInfo(string ProviderPublicId)
        {
            return View();
        }

        #endregion

        #region Legal Info

        public virtual ActionResult LIChaimberOfCommerceInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult LIRutInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult LICIFINInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult LISARLAFTInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult LIResolutionInfo(string ProviderPublicId)
        {
            return View();
        }

        #endregion
    }
}