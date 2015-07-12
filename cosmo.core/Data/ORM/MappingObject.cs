using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmo.Data.ORM
{
   /// <summary>
   /// Gets a new instance of <see cref="MappingObject"/>.
   /// </summary>
   [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
   public class MappingObject : System.Attribute
   {
      // Internal data declarations
      private string _caption;
      private string _captionIcon;
      private string _description;
      private string _dbTableName;
      private OrmEngine.OrmFormStyle _style;

      /// <summary>
      /// Gets a new instance of <see cref="MappingObject"/>.
      /// </summary>
      public MappingObject()
      {
         Initialize();
      }

      /// <summary>
      /// Gets or sets el título del formulario.
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }

      /// <summary>
      /// Gets or sets el icono que aparece junto al título del formulario.
      /// </summary>
      public string CaptionIcon
      {
         get { return _captionIcon; }
         set { _captionIcon = value; }
      }

      /// <summary>
      /// Gets or sets la descripción del formulario.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Gets or sets el nombre de la tabla de datos dónde se almacenan los objetos.
      /// </summary>
      public string DatabaseTableName
      {
         get { return _dbTableName; }
         set { _dbTableName = value; }
      }

      /// <summary>
      /// Gets or sets el estilo de formulario que se generará.
      /// </summary>
      public OrmEngine.OrmFormStyle FormStyle
      {
         get { return _style; }
         set { _style = value; }
      }

      /// <summary>
      /// Initializes the instance data.
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
