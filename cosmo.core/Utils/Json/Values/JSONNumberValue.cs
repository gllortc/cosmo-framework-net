using System;
using System.Globalization;

namespace Cosmo.Utils.Json
{
    /// <summary>
    /// JSONNumberValue is very much like a C# number, except that octal and hexadecimal formats 
    /// are not used.
    /// </summary>
    public class MWJsonNumberValue : MWJsonValue
    {
        private string _value;

        /// <summary>
        /// Number formatting object for handling globalization differences with decimal point separators
        /// </summary>
        protected static NumberFormatInfo JavaScriptNumberFormatInfo;

        static MWJsonNumberValue()
        {
            JavaScriptNumberFormatInfo = new NumberFormatInfo();
            JavaScriptNumberFormatInfo.NumberDecimalSeparator = ".";
        }

        internal MWJsonNumberValue(string value) : base()
        {
            this._value = value;
        }

        /// <summary>
        /// Public constructor that accepts a value of type int
        /// </summary>
        /// <param name="value">int (System.Int32) value</param>
        public MWJsonNumberValue(int value) : this(value.ToString())
        {
        }

        /// <summary>
        /// Public constructor that accepts a value of type double
        /// </summary>
        /// <param name="value">double (System.Double) value</param>
        public MWJsonNumberValue(double value) : this(value.ToString(MWJsonNumberValue.JavaScriptNumberFormatInfo))
        {
        }

        /// <summary>
        /// Public constructor that accepts a value of type decimal
        /// </summary>
        /// <param name="value">decimal (System.Decimal) value</param>
        public MWJsonNumberValue(decimal value) : this(value.ToString(MWJsonNumberValue.JavaScriptNumberFormatInfo))
        {
        }

        /// <summary>
        /// Public constructor that accepts a value of type single
        /// </summary>
        /// <param name="value">single (System.Single) value</param>
        public MWJsonNumberValue(Single value) : this(value.ToString("E", MWJsonNumberValue.JavaScriptNumberFormatInfo))
        {
        }

        /// <summary>
        /// Public constructor that accepts a value of type byte
        /// </summary>
        /// <param name="value">byte (System.Byte) value</param>
        public MWJsonNumberValue(byte value) : this(value.ToString())
        {
        }

        /// <summary>
        /// Required override of ToString() method.
        /// </summary>
        /// <returns>contained numeric value, rendered as a string</returns>
        public override string ToString()
        {
            return this._value;
        }

        /// <summary>
        /// Required override of the PrettyPrint() method.
        /// </summary>
        /// <returns>this.ToString()</returns>
        public override string PrettyPrint()
        {
            return this.ToString();
        }
    }

}
