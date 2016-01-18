namespace DocumentManagement.Provider.Models
{
    public class Enumerations
    {
        public enum enumProcessStatus
        {
            New = 201,
            FullRegister = 202,
            AgentDocumentaryReviewer = 203,
            CertificationStart = 204,
            CertificationEnds = 205
        }

        public enum enumChangesStatus
        {
            IsValidated = 7001,
            NotValidated = 7002
        }

        public enum enumProviderInfoType
        {
            Company = 2,
            Commercial = 3,
            HSEQ = 7,
            Finantial = 5,
            Lega = 6
        }
    }
}
