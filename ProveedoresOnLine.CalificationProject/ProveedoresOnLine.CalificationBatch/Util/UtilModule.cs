using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.Util
{
    public class UtilModule
    {
        #region ValueType

        public static int ValueTypeNumeric(string Value)
        {
            return int.Parse(Value);
        }

        public static string ValueTypeText(string Value)
        {
            return Value;
        }

        public static bool ValueTypeBoolean(string Value)
        {
            return Value == "true" || Value == "1" ? true : false;
        }

        public static double ValueTypePercent(string Value)
        {
            return Convert.ToDouble(Value);
        }

        public static DateTime ValueTypeDate(string Value)
        {
            return Convert.ToDateTime(Value);
        }

        #endregion
    }
}
