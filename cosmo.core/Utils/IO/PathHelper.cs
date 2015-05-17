using System;
using System.IO;

namespace Cosmo.Utils.IO
{

   /// <summary>
   /// Implementa una clase para el manejo de rutas físicas de archivos.
   /// </summary>
   public class PathInfo
   {
      /// <summary>
      /// Compina dos fragmentos de archivo/ruta
      /// </summary>
      /// <returns>Los fragmentos combinados.</returns>
      public static string Combine(string path1, string path2)
      {
         return System.IO.Path.Combine(path1, path2);
      }

      /// <summary>
      /// Compina dos o más fragmentos de archivo/ruta
      /// </summary>
      /// <returns>Los fragmentos combinados.</returns>
      public static string Combine(params string[] paths)
      {
         if (paths.Length == 0) return string.Empty;
         if (paths.Length == 1) return paths[0].Trim();

         string path = paths[0].Trim();

         for (int i = 1; i < paths.Length; i++)
         {
            path = Path.Combine(path, paths[i]);
         }

         return path;
      }

   }
}
