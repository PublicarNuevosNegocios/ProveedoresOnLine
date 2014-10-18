using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Customer.Interfaces
{
    internal interface ICustomerData
    {
        string CustomerUpsert(string CustomerPublicId, string Name, int IdentificationType, string IdentificationNumber);

        string FormUpsert(string FormPublicId, string CustomerPublicId, string Name, string TermsAndConditions);

        void FormUpsertLogo(string FormPublicId, string Logo);

        int StepCreate(string FormPublicId, string Name, int Position);

        void StepDelete(int StepId);

        int FieldCreate(int StepId, string Name, int ProviderInfoType, bool IsRequired, int Position);

        void FieldDelete(int FieldId);

        List<DocumentManagement.Customer.Models.Customer.CustomerModel> CustomerSearch(string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        List<DocumentManagement.Customer.Models.Form.FormModel> FormSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        DocumentManagement.Customer.Models.Customer.CustomerModel CustomerGetByFormId(string FormPublicId);

        DocumentManagement.Customer.Models.Customer.CustomerModel CustomerGetById(string CustomerPublicId);

        List<DocumentManagement.Customer.Models.Util.CatalogModel> CatalogGetCustomerOptions();

        List<DocumentManagement.Customer.Models.Form.StepModel> StepGetByFormId(string FormPublicId);

        List<DocumentManagement.Customer.Models.Form.FieldModel> FieldGetByStepId(int StepId);

    }
}
