
namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Enumera los tipos de autenticaci�n soportados por el servicio Smtp.
   /// </summary>
   public enum SmtpAuthenticateMode
   {
      /// <summary>Autom�tico</summary>
      Auto,
      /// <summary>Sin autenticaci�n</summary>
      None,
      /// <summary>Plain</summary>
      Plain,
      /// <summary>Login</summary>
      Login,
      /// <summary>Cram_MD5</summary>
      Cram_MD5
   }

}
