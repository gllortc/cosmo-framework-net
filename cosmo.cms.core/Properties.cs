using System.Diagnostics;
using System.Reflection;

namespace Cosmo.Cms
{
   public static class Properties
   {
      // Internal data declarations
      private static FileVersionInfo fvi;

      /// <summary>
      /// Devuelve el nombre del producto.
      /// </summary>
      public static string ProductName
      {
         get 
         {
            if (Properties.fvi == null)
            {
               Properties.fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            }

            return fvi.ProductName;
         }
      }

      /// <summary>
      /// Devuelve la versión de la libreria que define el objeto WSWorkspace
      /// </summary>
      public static string ProductVersion
      {
         get
         {
            if (Properties.fvi == null)
            {
               Properties.fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            }

            return fvi.ProductVersion;
         }
      }
   }
}
