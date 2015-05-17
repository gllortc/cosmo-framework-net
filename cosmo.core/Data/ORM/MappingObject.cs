using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmo.Data.ORM
{
   /// <summary>
   /// Devuelve una instancia de <see cref="MappingObject"/>.
   /// </summary>
   [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
   public class MappingObject : System.Attribute
   {
      // Declaración de variables internas
      private string _caption;
      private string _captionIcon;
      private string _description;
      private string _dbTableName;
      private OrmEngine.OrmFormStyle _style;

      /// <summary>
      /// Devuelve una instancia de <see cref="MappingObject"/>.
      /// </summary>
      public MappingObject()
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve o establece el título del formulario.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Devuelve o establece el icono que aparece junto al título del formulario.
      /// </summary>
      public string CaptionIcon
      {
         get { return _captionIcon; }
         set { _captionIcon = value; }
      }

      /// <summary>
      /// Devuelve o establece la descripción del formulario.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Devuelve o establece el nombre de la tabla de datos dónde se almacenan los objetos.
      /// </summary>
      public string DatabaseTableName
      {
         get { return _dbTableName; }
         set { _dbTableName = value; }
      }

      /// <summary>
      /// Devuelve o establece el estilo de formulario que se generará.
      /// </summary>
      public OrmEngine.OrmFormStyle FormStyle
      {
         get { return _style; }
         set { _style = value; }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _caption = string.Empty;
         _captionIcon = string.Empty;
         _description = string.Empty;
         _dbTableName = string.Empty;
         _style = OrmEngine.OrmFormStyle.Standard;
      }
   }
}
