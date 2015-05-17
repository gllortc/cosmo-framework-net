using System.IO;
using System.Text;

namespace Cosmo.Utils.IO
{

   /// <summary>
   /// Implementa una classe con utilidades relacionadas con el sistema de archivos
   /// </summary>
   public class Shell
   {

      #region Static Members

      /// <summary>
      /// Averigua si una carpeta existe.
      /// </summary>
      /// <param name="path">Ruta a la carpeta.</param>
      /// <returns>Un valor que indica si la carpeta existe o no.</returns>
      public static bool FolderExists(string path)
      {
         if (path.Trim().Equals(string.Empty)) return false;

         try
         {
            DirectoryInfo folder = new DirectoryInfo(path);
            return folder.Exists;
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      /// Averigua si un archivo existe.
      /// </summary>
      /// <param name="path">Ruta del archivo.</param>
      /// <returns>Un valor que indica si el archivo existe o no.</returns>
      public static bool FileExists(string path)
      {
         if (path.Trim().Equals(string.Empty)) return false;

         try
         {
            FileInfo file = new FileInfo(path);
            return file.Exists;
         }
         catch
         {
            return false;
         }
      }

      #endregion

   }
}
