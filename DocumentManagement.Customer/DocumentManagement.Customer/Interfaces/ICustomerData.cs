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

        string FormUpsert(string FormPublicId, string CustomerPublicId, string Name, string TermsAndConditions, string Logo);

        int StepCreate(string FormPublicId, string Name, int Position);

        void StepDelete(int StepId);

        int FieldCreate(int StepId, string Name, int ProviderInfoType, int FieldType, bool IsRequired, int Position);

        void FieldDelete(int FieldId);
    }
}
