using System;

namespace Cosmo.Cms.Common
{
   /// <summary>
   /// Enumera los estados posibles de publicación de un objeto Cosmo.
   /// </summary>
   public enum PublishStatus : int
   {
      /// <summary>
      /// Despublicado. 
      /// El objeto no es visible.
      /// </summary>
      Unpublished = 0,
      /// <summary>
      /// Borrador. 
      /// El objeto no es visible externamente y sólo es accesible por el propietario.
      /// </summary>
      Draft = 1,
      /// <summary>
      /// Aprovación pendiente. 
      /// El objeto no es visible externamente y está a la espera de recibir aprovación por un usuario gerárquicamente por encima del propietario.
      /// </summary>
      ApprovalPending = 2,
      /// <summary>
      /// Publicado.
      /// El objeto es visible externamente y puede ser accedido por todos los usuarios o por los que tienen suficientes privilegios.
      /// </summary>
      Published = 3
   }

   /// <summary>
   /// Interface para los objetos estándars que maneja Cosmo.
   /// </summary>
   public interface IPublishable
   {
      /// <summary>
      /// Devuelve o establece el identificador del objeto.
      /// </summary>
      int ID { get; set; }

      /// <summary>
      /// Devuelve o establece el identificador de la rama del repositorio Cosmo al que pertenece.
      /// </summary>
      int FolderId { get; set; }

      /// <summary>
      /// Devuelve o establece el estado de publicación del objeto.
      /// </summary>
      PublishStatus Status { get; set; }

      /// <summary>
      /// Devuelve o establece la fecha de creación del objeto.
      /// </summary>
      DateTime Created { get; set; }

      /// <summary>
      /// Devuelve o establece la fecha de la última modificación del objeto.
      /// </summary>
      DateTime Updated { get; set; }

      /// <summary>
      /// Devuelve o establece el login del propietario del objeto.
      /// </summary>
      /// <remarks>
      /// Por defecto, el propietario del objeto es el usuario creador del mismo.
      /// </remarks>
      string Owner { get; set; }

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
   }
}
