
namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// Specifies how to login to mail server.
   /// </summary>
   public enum Pop3AuthenticateMode
   {
      /// <summary>Automático.</summary>
      Auto,
      /// <summary>With Pop authenticate to login server.</summary>
      Pop,
      /// <summary>With A-Pop authenticate to login server.</summary>
      APop
   }

}
