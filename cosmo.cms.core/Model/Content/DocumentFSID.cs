using Cosmo.Utils;

namespace Cosmo.Cms.Model.Content
{
   /// <summary>
   /// Implements a file system service unique ID for all types of content.
   /// </summary>
   public class DocumentFSID : Cosmo.FileSystem.IFileSystemID
   {
      /// <summary>Prefix for the folder names corresponding to a container objects.</summary>
      private const string CONTAINER_NAME_PREFIX = "_";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FileSystemID"/>.
      /// </summary>
      /// <param name="objectId">Content object ID.</param>
      public DocumentFSID(int objectId)
      {
         this.ObjectID = objectId;
         this.IsContainer = false;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FileSystemID"/>.
      /// </summary>
      /// <param name="objectId">Content object ID.</param>
      /// <param name="isContainer">Indicates if the object can contains other objects.</param>
      public DocumentFSID(int objectId, bool isContainer)
      {
         this.ObjectID = objectId;
         this.IsContainer = isContainer;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FileSystemID"/>.
      /// </summary>
      /// <param name="fileSystemObjectID">A file system OID.</param>
      public DocumentFSID(string fileSystemObjectID)
      {
         this.IsContainer = (fileSystemObjectID.Trim().StartsWith(DocumentFSID.CONTAINER_NAME_PREFIX));
         this.ObjectID = Number.ToInteger(fileSystemObjectID.Trim().Replace(DocumentFSID.CONTAINER_NAME_PREFIX, string.Empty));
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the content object unique identifier.
      /// </summary>
      public int ObjectID { get; private set; }

      /// <summary>
      /// Gets a boolean indicating if the original content object is an object container.
      /// </summary>
      public bool IsContainer { get; private set; }

      #endregion

      #region Methods

      /// <summary>
      /// Converts a file system object to a string.
      /// </summary>
      /// <returns>A string representing the file system object ID.</returns>
      public string ToFolderName()
      {
         return (this.IsContainer ? DocumentFSID.CONTAINER_NAME_PREFIX : string.Empty) + this.ObjectID;
      }

      #endregion

   }
}
