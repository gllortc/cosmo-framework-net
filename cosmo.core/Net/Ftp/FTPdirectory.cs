using System;
using System.Collections.Generic;

namespace Cosmo.Net.Ftp
{

   #region Enumerations

   /// <summary>
   /// Identifies entry as either File or Directory
   /// </summary>
   public enum DirectoryEntryTypes
   {
      /// <summary>Archivo</summary>
      File,
      /// <summary>Directorio</summary>
      Directory
   }

   #endregion

   /// <summary>
   /// Stores a list of files and directories from an FTP result
   /// </summary>
   public class FTPdirectory : List<FTPfileInfo>
   {
      private const char slash = '/';

      /// <summary>
      /// Devuelve una instancia de FTPdirectory
      /// </summary>
      /// <remarks>
      /// Este constructor crea un listado de directorio en blanco
      /// </remarks>
      public FTPdirectory() { }

      /// <summary>
      /// Constructor: create list from a (detailed) directory string
      /// </summary>
      /// <param name="dir">directory listing string</param>
      /// <param name="path"></param>
      /// <remarks></remarks>
      public FTPdirectory(string dir, string path)
      {
         foreach (string line in dir.Replace("\n", string.Empty).Split(System.Convert.ToChar('\r')))
         {
            //parse
            if (!string.IsNullOrEmpty(line))
            {
               this.Add(new FTPfileInfo(line, path));
            }
         }
      }

      #region Methods

      /// <summary>
      /// Filter out only files from directory listing
      /// </summary>
      /// <param name="ext">optional file extension filter</param>
      /// <returns>FTPdirectory listing</returns>
      public FTPdirectory GetFiles(string ext)
      {
         return this.GetFileOrDir(DirectoryEntryTypes.File, ext);
      }

      /// <summary>
      /// Returns a list of only subdirectories
      /// </summary>
      /// <returns>FTPDirectory list</returns>
      /// <remarks></remarks>
      public FTPdirectory GetDirectories()
      {
         return this.GetFileOrDir(DirectoryEntryTypes.Directory, string.Empty);
      }

      /// <summary>
      /// Averigua si un fichero existe.
      /// </summary>
      /// <param name="filename">Nombre y ruta del archivo.</param>
      /// <returns>Un valor booleano indicando la existencia o no del archivo.</returns>
      public bool FileExists(string filename)
      {
         foreach (FTPfileInfo ftpfile in this)
         {
            if (ftpfile.Filename == filename)
            {
               return true;
            }
         }
         return false;
      }

      /// <summary>
      /// Devuelve el directorio superior al indicado.
      /// </summary>
      /// <param name="dir">Directorio actual.</param>
      public static string GetParentDirectory(string dir)
      {
         string tmp = dir.TrimEnd(slash);
         int i = tmp.LastIndexOf(slash);
         if (i > 0)
         {
            return tmp.Substring(0, i - 1);
         }
         else
         {
            throw (new ApplicationException("No parent for root"));
         }
      }

      //internal: share use function for GetDirectories/Files
      private FTPdirectory GetFileOrDir(DirectoryEntryTypes type, string ext)
      {
         FTPdirectory result = new FTPdirectory();

         foreach (FTPfileInfo fi in this)
         {
            if (fi.FileType == type)
            {
               if (string.IsNullOrEmpty(ext))
               {
                  result.Add(fi);
               }
               else if (ext == fi.Extension)
               {
                  result.Add(fi);
               }
            }
         }

         return result;
      }

      #endregion

   }
}

