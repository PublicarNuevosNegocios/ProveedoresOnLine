using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.Interfaces
{
    internal interface ICompanyCustomerData
    {
        int UpsertCustomerProvider(string CustomerPublicId, string ProviderPublicId, int StatusId, bool Enable);

        int UpsertCustomerProviderInfo(int CustomerProviderId, int? CustomerProviderInfoId, int CustomerProviderInfoTypeId, string Value, string LargeValue, bool Enable);

        int UpsertSurveyConfig(string CompanyPublicId, int? SurveyConfigId, string SurveyName, bool Enable);

        int UpsertSurveyItem(int SurveyConfigId, int? SurveyItemId, string SurveyItemName, int SurveyItemTypeId, int? ParentSurveyItemId, bool Enable);

        int UpsertSurveyItemInfo(int SurveyItemId, int? SurveyItemInfoId, int SurveyItemInfoTypeId, string Value, string LargeValue, bool Enable);

        int UpsertProjectConfig(string CompanyPublicId, int? ProjectConfigId, string ProjectConfigName, int StatusId, bool Enable);

        int UpsertEvaluationItem(int ProjectConfigId, int? EvaluationItemId, string EvaluationItemName, int EvaluationItemTypeId, int? ParentEvaluationItemId, bool Enable);

        int UpsertEvaluationItemInfo(int EvaluationItemId, int? EvaluationItemInfoId, int EvaluationItemInfoTypeId, string Value, string LargeValue, bool Enable);
    }
}
