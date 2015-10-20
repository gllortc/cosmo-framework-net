using System;

namespace Cosmo.Cms.Common
{
   /// <summary>
   /// Interface to implement persistable Cosmo.CMS objects.
   /// </summary>
   public interface IPublishable
   {

      #region Properties

      /// <summary>
      /// Gets or sets el identificador del objeto.
      /// </summary>
      int ID { get; set; }

      /// <summary>
      /// Gets or sets el estado de publicación del objeto.
      /// </summary>
      CmsPublishStatus.PublishStatus Status { get; set; }

      /// <summary>
      /// Gets or sets la fecha de creación del objeto.
      /// </summary>
      DateTime Created { get; }

      /// <summary>
      /// Gets or sets la fecha de la última modificación del objeto.
      /// </summary>
      DateTime Updated { get; }

      /// <summary>
      /// Gets or sets el login del propietario del objeto.
      /// </summary>
      /// <remarks>
      /// Por defecto, el propietario del objeto es el usuario creador del mismo.
      /// </remarks>
      string Owner { get; }

      #endregion

      #region Methods

      /// <summary>
      /// Serializa el objeto a un archivo XML.
      /// </summary>
      /// <param name="filename">Archivo de salida.</param>
      void Save(string filename);

      /// <summary>
      /// Desserializa un objeto serializado en un archivo XML y carga los datos en la instancia actual.
      /// </summary>
      /// <param name="filename">Archivo a cargar.</param>
      void Load(string filename);

      /// <summary>
      /// Valida los datos de las propiedades del objeto.
      /// </summary>
      /// <returns><c>true</c> si los datos son correctos o <c>false</c> en cualquier otro caso.</returns>
      bool Validate();

      #endregion

   }
}
