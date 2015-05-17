
namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Enumera los tipos de autenticación soportados por el servicio Smtp.
   /// </summary>
   public enum SmtpAuthenticateMode
   {
      /// <summary>Automático</summary>
      Auto,
      /// <summary>Sin autenticación</summary>
      None,
      /// <summary>Plain</summary>
      Plain,
      /// <summary>Login</summary>
      Login,
      /// <summary>Cram_MD5</summary>
      Cram_MD5
   }

}
