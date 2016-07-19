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
            return (Value != "" && Value != null) ? int.Parse(Value) : 0;            
        }

        public static string ValueTypeText(string Value)
        {
            return (Value != "" && Value != null) ? Value : "";             
        }

        public static bool ValueTypeBoolean(string Value)
        {
            return Value == "true" || Value == "1" ? true : false;
        }

        public static double ValueTypePercent(string Value)
        {
            return (Value != "" && Value != null) ? Convert.ToDouble(Value) : 0;            
        }

        public static DateTime ValueTypeDate(string Value)
        {
            return (Value != "" && Value != null) ? Convert.ToDateTime(Value) : new DateTime(1,1,1);            
        }

        public static float ValueTypeFloat(string value)
        {
            return (value != "" && value != null) ? float.Parse(value) : 0;            
        }

        public static decimal ValueTypeDecimal(string value)
        {
            return (value != "" && value != null) ? decimal.Parse(value) : 0;            
        }
        #endregion
    }
}
