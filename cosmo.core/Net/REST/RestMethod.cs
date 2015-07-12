namespace Cosmo.Net.REST
{
   /// <summary>
   /// Implements a definition of a REST method for Cosmo REST handlers.
   /// </summary>
   [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
   public class RestMethod : System.Attribute
   {

      #region Enumerations

      /// <summary>
      /// Enumerate the type of data returned by method.
      /// </summary>
      public enum RestMethodReturnType
      {
         StandardRestResponse,
         JsonDataResponse,
         HtmlDataResponse,
         Redirect
      }

      #endregion

      #region Properties

      /// <summary>
      /// Returns or sets the command name associated to a class method.
      /// </summary>
      /// <remarks>
      /// This name is used to create URL: <c>myServer.com\RestHandler?cmd=[CommandName]</c>.
      /// </remarks>
      public string CommandName { get; set; }

      /// <summary>
      /// Returns or sets the type of data returned by method.
      /// </summary>
      public RestMethodReturnType DataType { get; set; }

      #endregion

   }
}
