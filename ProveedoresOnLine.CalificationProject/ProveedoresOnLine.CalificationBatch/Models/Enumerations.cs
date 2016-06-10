namespace ProveedoresOnLine.CalificationBatch.Models
{
    public class Enumerations
    {
        public enum enumModuleType
        {
            CP_LegalModule = 203001,
            CP_FinancialModule = 203002,
            CP_CommercialModule = 203003,
            CP_HSEQModule = 203004,
            CP_BalanceModule = 203005,
        }

        public enum enumOperatorType
        {
            Positivo = 2001001,
            Negativo = 2001002,
            MenorQue = 2001003,
            MayorQue = 2001004,
            IgualQue = 2001005,
            MenorOIgual = 2001006,
            MayorOIgual = 2001007,
            Entre = 2001008,
        }

        public enum enumValueType
        {
            Boolean = 2002001,
            Numeric = 2002002,
            Percent = 2002003,
            Text = 2002004,
        }
    }
}
