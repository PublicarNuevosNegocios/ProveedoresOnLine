using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Models;
using ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess;
using ProveedoresOnLine.RestrictiveListProcess.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.Controller
{
    public class RestrictiveListProcessModule
    {
        #region Provider Functions
            public static List<ProviderModel> GetProviderByStatus(int Status, string CustomerPublicId)
            {
                List<ProviderModel> oProviderList = new List<ProviderModel>();
                List<CompanyModel> oCompanyModeResult = DAL.Controller.RestrictiveListProcessDataController.Instance.GetProviderByStatus(Status, CustomerPublicId);

                //Set Related Company to ProviderModel
                oCompanyModeResult.All(x =>
                {
                    oProviderList.Add(new ProviderModel() { RelatedCompany = x });                
                    return true;
                });
                //Set Related Legal to ProviderModel
                oProviderList.All(prv =>
                {
                    prv.RelatedLegal = new List<GenericItemModel>();  
                    prv.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo(prv.RelatedCompany.CompanyPublicId,(int)enumLegalType.Designations, true);
                    return true;
                });

                return oProviderList;
            }
        #endregion

        #region RestrictiveList Functions
            public static List<RestrictiveListProcessModel> GetAllProvidersInProcess()
            {
                return DAL.Controller.RestrictiveListProcessDataController.Instance.GetAllProvidersInProcess();
            }
            public static string BlackListProcessUpsert(BlackListProcessModel oBlackListProcessModel)
            {
                return DAL.Controller.RestrictiveListProcessDataController.Instance.BlackListProcessUpsert(oBlackListProcessModel.BlackListProcessId, oBlackListProcessModel.FilePath, oBlackListProcessModel.ProcessStatus, oBlackListProcessModel.IsSuccess, oBlackListProcessModel.ProviderStatus, oBlackListProcessModel.Enable, oBlackListProcessModel.LastModify, oBlackListProcessModel.CreateDate);
            }
        #endregion
    }
}
