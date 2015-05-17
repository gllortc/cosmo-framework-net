using System.IO;
using System.Text;

namespace Cosmo.Utils.IO
{

   /// <summary>
   /// Implementa una classe con utilidades relacionadas con el sistema de archivos
   /// </summary>
   public class FileReader
   {

      #region Static Members

      /// <summary>
      /// Obtiene el contenido (texto) de un archivo.
      /// </summary>
      /// <param name="filename">Nombre y ruta del archivo.</param>
      /// <returns>El contenido (texto) del archivo.</returns>
      public static byte[] LoadTextFileToByteArray(string filename)
      {
         FileInfo file = new FileInfo(filename);
         if (!file.Exists)
            throw new FileNotFoundException("No se encuentra o no está disponible el archivo " + file.Name + ".");

         FileStream stream = file.OpenRead();
         byte[] buffer = new byte[stream.Length];
         stream.Read(buffer, 0, buffer.Length);
         stream.Close();

         return buffer;
      }

      /// <summary>
      /// Obtiene el contenido (texto) de un archivo.
      /// </summary>
      /// <param name="filename">Nombre y ruta del archivo.</param>
      /// <returns>El contenido (texto) del archivo.</returns>
      public static string LoadTextFileToString(string filename)
      {
         return Encoding.ASCII.GetString(FileReader.LoadTextFileToByteArray(filename));
      }

      #endregion

   }
}
