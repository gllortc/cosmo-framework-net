namespace Cosmo.Cms.Model.Photos
{
   public class PhotosFSID : Cosmo.FileSystem.IFileSystemID
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PhotosFSID"/>.
      /// </summary>
      public PhotosFSID() { } 

      #endregion

      #region Methods

      /// <summary>
      /// Converts the instance data into a unique ID in string format.
      /// </summary>
      /// <returns>A unique string corresponding to a folder name.</returns>
      public string ToFolderName()
      {
         return PhotoDAO.SERVICE_FOLDER;
      }

      #endregion 

   }
}
