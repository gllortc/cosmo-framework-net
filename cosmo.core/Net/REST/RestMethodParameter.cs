using System;

namespace Cosmo.Net.REST
{
   /// <summary>
   /// Implements a definition of a REST method parameter for Cosmo REST handlers.
   /// </summary>
   [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
   public class RestMethodParameter : System.Attribute
   {

      #region Properties

      /// <summary>
      /// The name of the parameter that is used in URL.
      /// </summary>
      public string UrlParameterName { get; set; }

      /// <summary>
      /// The name of the parameter that is used in REST class method.
      /// </summary>
      public string MethodParameterName { get; set; }

      /// <summary>
      /// The type of the parameter.
      /// </summary>
      public Type DataType { get; set; }

      #endregion

   }
}
