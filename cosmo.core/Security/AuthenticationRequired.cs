namespace Cosmo.Security
{
   /// <summary>
   /// Implementa un atributo para indicar a las páginas Cosmo que se reguiere cierto rol para acceder a ellas.
   /// </summary>
   /// <remarks>
   /// Añadir este atributo a una página conlleva implícitamente  que el usuario debe estar autenticado.
   /// </remarks>
   [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method, AllowMultiple = false)]
   public class AuthenticationRequired : System.Attribute
   {
      // No contiene ninguna propiedad
   }
}
