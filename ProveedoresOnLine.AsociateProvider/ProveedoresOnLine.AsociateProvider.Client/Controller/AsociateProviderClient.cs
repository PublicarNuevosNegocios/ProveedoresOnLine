using ProveedoresOnLine.AsociateProvider.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.Controller
{
    public class AsociateProviderClient
    {
        public static void AsociateProvider(AsociateProviderModel AsociateProviderToUpsert)
        {
            try
            {
                int BOProviderUpsert = ProviderUpsertBO(AsociateProviderToUpsert);

                int DMProviderUpsert = ProviderUpsertDM(AsociateProviderToUpsert);

                DAL.Controller.AsociateProviderClientController.Instance.AP_AsociateProviderUpsert(
                    AsociateProviderToUpsert.RelatedProviderBO.ProviderPublicId,
                    AsociateProviderToUpsert.RelatedProviderDM.ProviderPublicId,
                    AsociateProviderToUpsert.Email);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        
        public static int ProviderUpsertBO(AsociateProviderModel AsociateProviderToUpsert)
        {
            int oReturn = 0;
            
            if (AsociateProviderToUpsert != null &&
                AsociateProviderToUpsert.RelatedProviderBO != null)
            {
                try
                {
                    oReturn = DAL.Controller.AsociateProviderClientController.Instance.BOProviderUpsert(
                    AsociateProviderToUpsert.RelatedProviderBO.ProviderId,
                    AsociateProviderToUpsert.RelatedProviderBO.ProviderPublicId,
                    AsociateProviderToUpsert.RelatedProviderBO.ProviderName,
                    AsociateProviderToUpsert.RelatedProviderBO.IdentificationType,
                    AsociateProviderToUpsert.RelatedProviderBO.IdentificationNumber);
                }
                catch (Exception)
                {
                    
                    throw;
                }                
            }

            return oReturn;
        }

        public static int ProviderUpsertDM(AsociateProviderModel AsociateProviderToUpsert)
        {
            int oReturn = 0;

            if (AsociateProviderToUpsert != null &&
                AsociateProviderToUpsert.RelatedProviderDM != null)
            {
                try
                {
                    oReturn = DAL.Controller.AsociateProviderClientController.Instance.DMProviderUpsert(
                    AsociateProviderToUpsert.RelatedProviderDM.ProviderId,
                    AsociateProviderToUpsert.RelatedProviderDM.ProviderPublicId,
                    AsociateProviderToUpsert.RelatedProviderDM.ProviderName,
                    AsociateProviderToUpsert.RelatedProviderDM.IdentificationType,
                    AsociateProviderToUpsert.RelatedProviderDM.IdentificationNumber);
                }
                catch (Exception)
                {
                    
                    throw;
                }                
            }

            return oReturn;
        }
    }
}
