using System.Reflection;

namespace Cosmo.Cms
{
   public class Cms
   {
      private const string PRODUCT_NAME = "Cosmo.CMS";

      /// <summary>
      /// Devuelve el nombre del producto.
      /// </summary>
      public static string ProductName
      {
         get { return Cms.PRODUCT_NAME; }
      }

      /// <summary>
      /// Devuelve la versión de la libreria que define el objeto WSWorkspace
      /// </summary>
      public static string Version
      {
         get
         {
            return Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                   Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                   Assembly.GetExecutingAssembly().GetName().Version.Revision + "." +
                   Assembly.GetExecutingAssembly().GetName().Version.Build; ;
         }
      }
   }
}
