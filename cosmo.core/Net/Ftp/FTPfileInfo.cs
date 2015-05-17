using System;
using System.Text.RegularExpressions;

namespace Cosmo.Net.Ftp
{

   /// <summary>
   /// Represents a file or directory entry from an FTP listing
   /// </summary>
   /// <remarks>
   /// This class is used to parse the results from a detailed
   /// directory list from FTP. It supports most formats of
   /// 
   /// v1.1 fixed bug in Fullname/path
   /// </remarks>
   public class FTPfileInfo
   {
      private string _filename;
      private string _path;
      private DirectoryEntryTypes _fileType;
      private long _size;
      private DateTime _fileDateTime;
      private string _permission;

      /// <summary>
      /// List of REGEX formats for different FTP server listing formats
      /// </summary>
      /// <remarks>
      /// The first three are various UNIX/LINUX formats, fourth is for MS FTP
      /// in detailed mode and the last for MS FTP in 'DOS' mode.
      /// I wish VB.NET had support for Const arrays like C# but there you go
      /// </remarks>
      private static string[] _ParseFormats = new string[] { 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})(\\s+)(?<size>(\\d+))(\\s+)(?<ctbit>(\\w+\\s\\w+))(\\s+)(?<size2>(\\d+))\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{2}:\\d{2})\\s+(?<name>.+)", 
            "(?<timestamp>\\d{2}\\-\\d{2}\\-\\d{2}\\s+\\d{2}:\\d{2}[Aa|Pp][mM])\\s+(?<dir>\\<\\w+\\>){0,1}(?<size>\\d+){0,1}\\s+(?<name>.+)" };

      /// <summary>
      /// Devuelve una instancia de FTPfileInfo
      /// </summary>
      /// <param name="line">The line returned from the detailed directory list</param>
      /// <param name="path">Path of the directory</param>
      /// <remarks></remarks>
      public FTPfileInfo(string line, string path)
      {
         // parse line
         Match m = GetMatchingRegex(line);
         if (m == null)
         {
            // failed
            throw (new ApplicationException("Unable to parse line: " + line));
         }
         else
         {
            _filename = m.Groups["name"].Value;
            _path = path;

            Int64.TryParse(m.Groups["size"].Value, out _size);
            //_size = System.Convert.ToInt32(m.Groups["size"].Value);

            _permission = m.Groups["permission"].Value;
            string _dir = m.Groups["dir"].Value;
            if (_dir != string.Empty && _dir != "-")
            {
               _fileType = DirectoryEntryTypes.Directory;
            }
            else
            {
               _fileType = DirectoryEntryTypes.File;
            }

            try
            {
               _fileDateTime = DateTime.Parse(m.Groups["timestamp"].Value);
            }
            catch (Exception)
            {
               _fileDateTime = Convert.ToDateTime(null);
            }
         }
      }

      #region Properties

      /// <summary>
      /// Devuelve el nombre del archivo y la ruta.
      /// </summary>
      public string FullName
      {
         get { return Path + Filename; }
      }

      /// <summary>
      /// Devuelve el nombre del archivo sin la ruta.
      /// </summary>
      public string Filename
      {
         get { return _filename; }
      }

      /// <summary>
      /// Path of file (always ending in /)
      /// </summary>
      /// <remarks>
      /// 1.1: Modifed to ensure always ends in / - with thanks to jfransella for pointing this out
      /// </remarks>
      public string Path
      {
         get { return _path + (_path.EndsWith("/") ? string.Empty : "/"); }
      }

      /// <summary>
      /// Devuelve el tipo de archivo.
      /// </summary>
      public DirectoryEntryTypes FileType
      {
         get { return _fileType; }
      }

      /// <summary>
      /// Devuelve el tamaño en bytes del archivo.
      /// </summary>
      public long Size
      {
         get { return _size; }
      }

      /// <summary>
      /// Devuelve la fecha de la útlima actualización del archivo.
      /// </summary>
      public DateTime FileDateTime
      {
         get { return _fileDateTime; }
         internal set { _fileDateTime = value; }
      }

      /// <summary>
      /// Devuelve una cadena descriptiva de los permisos del archivo.
      /// </summary>
      public string Permission
      {
         get { return _permission; }
      }

      /// <summary>
      /// Devuelve la extensión del archivo.
      /// </summary>
      public string Extension
      {
         get
         {
            int i = this.Filename.LastIndexOf(".");
            if (i >= 0 && i < (this.Filename.Length - 1))
            {
               return this.Filename.Substring(i + 1);
            }
            else
            {
               return string.Empty;
            }
         }
      }

      /// <summary>
      /// Devuelve el nombre (sin extensión ni ruta) del archivo.
      /// </summary>
      public string NameOnly
      {
         get
         {
            int i = this.Filename.LastIndexOf(".");
            if (i > 0)
            {
               return this.Filename.Substring(0, i);
            }
            else
            {
               return this.Filename;
            }
         }
      }

      #endregion

      #region Private Members

      private Match GetMatchingRegex(string line)
      {
         Regex rx;
         Match m;

         for (int i = 0; i <= _ParseFormats.Length - 1; i++)
         {
            rx = new Regex(_ParseFormats[i]);
            m = rx.Match(line);
            if (m.Success)
            {
               return m;
            }
         }
         return null;
      }

      #endregion

   }
}

