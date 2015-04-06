using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Interfaces.ViewModel
{
    public class AsociateProviderViewModel
    {
        public string AP_AsociateProviderId { get; set; }

        public ProveedoresOnLine.AsociateProvider.Interfaces.Models.AsociateProvider.RelatedProviderModel BO_RelatedProvider { get; set; }

        public ProveedoresOnLine.AsociateProvider.Interfaces.Models.AsociateProvider.RelatedProviderModel DM_RelatedProvider { get; set; }

        #region BO Provider

        public string AP_BO_ProviderId { get { return BO_RelatedProvider.ProviderId.ToString(); } }

        public string AP_BO_ProviderPublicId { get { return BO_RelatedProvider.ProviderPublicId; } }

        public string AP_BO_ProviderName { get { return BO_RelatedProvider.ProviderName; } }

        public string AP_BO_IdentificationType { get { return BO_RelatedProvider.IdentificationType; } }

        public string AP_BO_IdentificationNumber { get { return BO_RelatedProvider.IdentificationNumber; } }

        #endregion

        #region DM Provider

        public string AP_DM_ProviderId { get { return DM_RelatedProvider.ProviderId.ToString(); } }

        public string AP_DM_ProviderPublicId { get { return DM_RelatedProvider.ProviderPublicId; } }

        public string AP_DM_ProviderName { get { return DM_RelatedProvider.ProviderName; } }

        public string AP_DM_IdentificationType { get { return DM_RelatedProvider.IdentificationType; } }

        public string AP_DM_IdentificationNumber { get { return DM_RelatedProvider.IdentificationNumber; } }

        #endregion

        public string AP_Email { get; set; }

        public string AP_LastModify { get; set; }

        public string AP_CreateDate { get; set; }

        public AsociateProviderViewModel() { }

        public AsociateProviderViewModel(ProveedoresOnLine.AsociateProvider.Interfaces.Models.AsociateProvider.AsociateProviderModel oAsociateProviderModel)
        {
            AP_AsociateProviderId = oAsociateProviderModel.AsociateProviderId.ToString();
            BO_RelatedProvider = oAsociateProviderModel.RelatedProviderBO;
            DM_RelatedProvider = oAsociateProviderModel.RelatedProviderDM;
            AP_Email = oAsociateProviderModel.Email;
            AP_CreateDate = oAsociateProviderModel.CreateDate.ToString();
            AP_LastModify = oAsociateProviderModel.LastModify.ToString();
        }
    }
}
