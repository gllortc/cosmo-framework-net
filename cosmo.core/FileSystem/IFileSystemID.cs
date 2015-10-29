using Cosmo.Utils;

namespace Cosmo.FileSystem
{
   /// <summary>
   /// Implements a file system service unique ID for all types of content.
   /// </summary>
   public interface IFileSystemID
   {

      #region Methods

      /// <summary>
      /// Converts the instance data into a unique ID in string format.
      /// </summary>
      /// <returns>A unique string corresponding to a folder name.</returns>
      string ToFolderName();

      #endregion

   }
}
