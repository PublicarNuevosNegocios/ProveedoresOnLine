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
            NotValidated =7002
        }
    }
}
