using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Utils.Json
{
    /// <summary>
    /// JSONReflector provides a convenient way to convert value and reference type objects
    /// to JSON format through reflection.
    /// 
    /// This implementation build JSON around reflected public properties of type int, float,
    /// double, decimal, byte, string, bool, enum or array.  (Generics and other types may be
    /// supported at a later time.)
    /// </summary>
    public class MWJsonReflector : MWJsonValue
    {
        private MWJsonObjectCollection _jsonObjectCollection;

        private MWJsonValue GetJSONValue(object objValue)
        {
            Type thisType = objValue.GetType();
            MWJsonValue jsonValue = null;

            if (thisType == typeof(System.Int32))
            {
                jsonValue = new MWJsonNumberValue(Convert.ToInt32(objValue));
            }
            else if (thisType == typeof(System.Single))
            {
                jsonValue = new MWJsonNumberValue(Convert.ToSingle(objValue));
            }
            else if (thisType == typeof(System.Double))
            {
                jsonValue = new MWJsonNumberValue(Convert.ToDouble(objValue));
            }
            else if (thisType == typeof(System.Decimal))
            {
                jsonValue = new MWJsonNumberValue(Convert.ToDecimal(objValue));
            }
            else if (thisType == typeof(System.Byte))
            {
                jsonValue = new MWJsonNumberValue(Convert.ToByte(objValue));
            }
            else if (thisType == typeof(System.String))
            {
                jsonValue = new MWJsonStringValue(Convert.ToString(objValue));
            }
            else if (thisType == typeof(System.Boolean))
            {
                jsonValue = new MWJsonBoolValue(Convert.ToBoolean(objValue));
            }
            else if (thisType.BaseType == typeof(System.Enum))
            {
                jsonValue = new MWJsonStringValue(Enum.GetName(thisType, objValue));
            }
            else if (thisType.IsArray)
            {
                List<MWJsonValue> jsonValues = new List<MWJsonValue>();
                Array arrValue = (Array)objValue;
                for (int x = 0; x < arrValue.Length; x++)
                {
                    MWJsonValue jsValue = this.GetJSONValue(arrValue.GetValue(x));
                    jsonValues.Add(jsValue);
                }
                jsonValue = new MWJsonArrayCollection(jsonValues);
            }
            return jsonValue;
        }

        /// <summary>
        /// Public constructor that accepts any object
        /// </summary>
        /// <param name="objValue">object to be reflected/evaluated for JSON conversion</param>
        public MWJsonReflector(object objValue)
        {
            Dictionary<MWJsonStringValue, MWJsonValue> jsonNameValuePairs = new Dictionary<MWJsonStringValue, MWJsonValue>();

            Type type = objValue.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo pi in properties)
            {
                MWJsonStringValue jsonParameterName = new MWJsonStringValue(pi.Name);
                MWJsonValue jsonParameterValue = this.GetJSONValue(pi.GetValue(objValue, null));
                if (jsonParameterValue != null)
                {
                    jsonNameValuePairs.Add(jsonParameterName, jsonParameterValue);
                }
            }

            this._jsonObjectCollection = new MWJsonObjectCollection(jsonNameValuePairs);
        }


        /// <summary>
        /// Required override of the ToString() method.
        /// </summary>
        /// <returns>returns the internal JSONObjectCollection ToString() method</returns>
        public override string ToString()
        {
            return this._jsonObjectCollection.ToString();
        }

        /// <summary>
        /// Required override of the PrettyPrint() method.
        /// </summary>
        /// <returns>returns the internal JSONObjectCollection PrettyPrint() method</returns>
        public override string PrettyPrint()
        {
            return this._jsonObjectCollection.PrettyPrint();
        }
    }
}
