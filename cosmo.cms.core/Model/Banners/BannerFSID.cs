using Cosmo.Utils;

namespace Cosmo.Cms.Model.Banners
{
   /// <summary>
   /// Implements a file system service unique ID for banners.
   /// </summary>
   public class BannerFSID : Cosmo.FileSystem.IFileSystemID
   {
      /// <summary>Prefix for the folder names corresponding to a container objects.</summary>
      private const string SERVICE_FOLDER = "ads";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FileSystemID"/>.
      /// </summary>
      /// <param name="objectId">Content object ID.</param>
      public BannerFSID(int objectId)
      {
         this.ObjectID = objectId;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the content object unique identifier.
      /// </summary>
      public int ObjectID { get; private set; }

      #endregion

      #region Methods

      /// <summary>
      /// Converts a file system object to a string.
      /// </summary>
      /// <returns>A string representing the file system object ID.</returns>
      public string ToFolderName()
      {
         return Cosmo.Net.Url.Combine(BannerFSID.SERVICE_FOLDER, this.ObjectID.ToString());
      }

      #endregion

   }
}
