namespace Cosmo.Security
{
   /// <summary>
   /// Implementa una clase que representa un rol de usuario.
   /// </summary>
   public class Role
   {
      // Internal data declaration
      private int _id;
      private string _name;
      private string _description;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="Role"/>.
      /// </summary>
      public Role()
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="Role"/>.
      /// </summary>
      public Role(int id, string name)
      {
         Initialize();

         _id = id;
         _name = name;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el identificador único del rol.
      /// </summary>
      public int ID
      {
         get { return _id; }
         internal set { _id = value; }
      }

      /// <summary>
      /// Devuelve o establece el nombre del rol.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Devuelve o establece la descripción del rol.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _id = 0;
         _name = string.Empty;
         _description = string.Empty;
      }

      #endregion

   }
}
