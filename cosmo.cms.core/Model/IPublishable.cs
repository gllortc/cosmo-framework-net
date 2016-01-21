using System;

namespace Cosmo.Cms.Model
{
   /// <summary>
   /// Interface for all publishable (persistent) objects in <c>Cosmo.Cms</c> namespace.
   /// </summary>
   public interface IPublishable
   {

      #region Properties

      /// <summary>
      /// Gets or sets the object unique identifier.
      /// </summary>
      /// <remarks>
      /// This identifier is refered to the object scope, not to all <see cref="IPublishable"/> objects.
      /// In future versions, this ID must be unique for all <see cref="IPublishable"/> objects.
      /// </remarks>
      int ID { get; set; }

      /// <summary>
      /// Gets or sets the publishing status of the object.
      /// </summary>
      CmsPublishStatus.PublishStatus Status { get; set; }

      /// <summary>
      /// Gets or sets the creation timestamp.
      /// </summary>
      DateTime Created { get; }

      /// <summary>
      /// Gets or sets the last modification timestamp.
      /// </summary>
      DateTime Updated { get; }

      /// <summary>
      /// Gets or sets the owner login.
      /// </summary>
      /// <remarks>
      /// By default, this login is the same of the creator account.
      /// </remarks>
      string Owner { get; }

      #endregion

      #region Methods

      /// <summary>
      /// Save the object to XML a file.
      /// </summary>
      /// <param name="filename">File (and path) to output file.</param>
      void Save(string filename);

      /// <summary>
      /// Loads an object from XML a file (created previously by the method <c>Save()</c>).
      /// </summary>
      /// <param name="filename">File (and path) to file to load.</param>
      void Load(string filename);

      /// <summary>
      /// Check instance data for validate its content.
      /// </summary>
      /// <returns><c>true</c> if data is correct or <c>false</c> in all other cases.</returns>
      bool Validate();

      #endregion

   }
}
