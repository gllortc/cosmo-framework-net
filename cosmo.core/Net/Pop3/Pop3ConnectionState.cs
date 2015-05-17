
namespace Cosmo.Net.Pop3
{

   /// <summary>
   /// Specifies whether server and client is connected,or disconnected or authenticated.
   /// </summary>
   public enum Pop3ConnectionState
   {
      /// <summary>Server and client does not connected.</summary>
      Disconnected,
      /// <summary>Server and client communicate with tcp/ip.</summary>
      Connected,
      /// <summary>Server and client authenticate success.</summary>
      Authenticated
   }

}
