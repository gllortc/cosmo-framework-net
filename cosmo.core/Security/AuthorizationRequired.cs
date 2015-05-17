namespace Cosmo.Security
{
   /// <summary>
   /// Implementa un atributo para indicar a las páginas Cosmo que se reguiere cierto rol para acceder a ellas.
   /// </summary>
   /// <remarks>
   /// Añadir este atributo a una página conlleva implícitamente  que el usuario debe estar autenticado.
   /// </remarks>
   [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method, AllowMultiple = true)]
   public class AuthorizationRequired : System.Attribute
   {
      // Declaración de variables internas
      private string[] _roles;

      /// <summary>
      /// Devuelve una instancia de <see cref="AuthorizationRequired"/>.
      /// </summary>
      /// <param name="args">Los roles que dan acceso a la página o recurso.</param>
      public AuthorizationRequired(params string[] args)
      {
         _roles = args;
      }

      /// <summary>
      /// Nombre del rol requerido para acceder a las páginas que contengan este atributo.
      /// </summary>
      public string[] RequiredRole
      {
         get { return _roles; }
         set { _roles = value; }
      }
   }
}
