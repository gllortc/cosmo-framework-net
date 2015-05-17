using System.IO;
using System.Security.Permissions;
using Microsoft.Win32;

namespace Cosmo.Utils
{

   /// <summary>
   /// Implementa una clase con utilidades MIME.
   /// </summary>
   public class Mime
   {
      /// <summary>
      /// Obtiene el tipo MIME associado a un archivo.
      /// </summary>
      /// <param name="filename">Nombre y path del archivo.</param>
      /// <returns>Una cadena que describe el tipo MIME asociado.</returns>
      public static string GetType(string filename)
      {
         // Obtiene el archivo a evaluar
         FileInfo file = new FileInfo(filename);
         string extension = file.Extension.ToLower().Replace(".", "");

         // Intenta determinar el tipo sin acceder al registro de Windows
         switch (extension)
         {
            // Texto
            case "asc":
            case "c":
            case "cc":
            case "formatter":
            case "f90":
            case "h":
            case "hh":
            case "m":
            case "txt": return "text/plain";
            case "css": return "text/css";
            case "etx": return "text/x-setext";
            case "htm":
            case "html":
            case "xhtml": return "text/html";
            case "rtf": return "text/rtf";
            case "rtx": return "text/richtext";
            case "sgm":
            case "sgml": return "text/sgml";
            case "tsv": return "text/tab-separated-values";
            case "xml": return "text/xml";

            // Imágen
            case "bm":
            case "bmp": return "image/bmp";
            case "gif": return "image/gif";
            case "ief": return "image/ief";
            case "jpe":
            case "jpeg":
            case "jpg": return "image/jpeg";
            case "pbm": return "image/x-portable-bitmap";
            case "pgm": return "image/x-portable-graymap";
            case "png": return "image/png";
            case "pnm": return "image/x-portable-anymap";
            case "ppm": return "image/x-portable-pixmap";
            case "ras": return "image/cmu-raster";
            case "rgb": return "image/x-rgb";
            case "tif":
            case "tiff": return "image/tiff";
            case "xbm":
            case "xpm": return "image/x-xpixmap";
            case "xwd": return "image/x-xwindowdump";

            // Audio
            case "aif":
            case "aifc":
            case "aiff": return "audio/x-aiff";
            case "snd":
            case "au": return "audio/basic";
            case "kar":
            case "mid":
            case "midi": return "audio/midi";
            case "mp2":
            case "mp3":
            case "mpe":
            case "mpeg":
            case "mpg":
            case "mpga": return "audio/mpeg";
            case "ra": return "audio/x-realaudio";
            case "ram": return "audio/x-pn-realaudio";
            case "rm": return "audio/x-pn-realaudio";
            case "tsi": return "audio/TSP-audio";
            case "wav": return "audio/x-wav";
            case "rpm": return "audio/x-pn-realaudio-plugin";

            // Video
            case "avi": return "video/x-msvideo";
            case "fli": return "video/x-fli";
            case "qt":
            case "mov": return "video/quicktime";
            case "movie": return "video/x-sgi-movie";
            case "viv":
            case "vivo": return "video/vnd.vivo";

            // Aplicaciones
            case "bcpio": return "application/x-bcpio";
            case "ccad": return "application/clariscad";
            case "cdf": return "application/x-netcdf";
            case "cpio": return "application/x-cpio";
            case "cpt": return "application/mac-compactpro";
            case "csh": return "application/x-csh";
            case "dcr":
            case "dir": return "application/x-director";
            case "dot":
            case "doc": return "application/msword";
            case "drw": return "application/drafting";
            case "dvi": return "application/x-dvi";
            case "dwg": return "application/acad";
            case "dxf": return "application/dxf";
            case "dxr": return "application/x-director";
            case "ez": return "application/andrew-inset";
            case "gtar": return "application/x-gtar";
            case "gz": return "application/x-gzip";
            case "hdf": return "application/x-hdf";
            case "hqx": return "application/mac-binhex40";
            case "ips": return "application/x-ipscript";
            case "ipx": return "application/x-ipix";
            case "jar": return "application/x-java-archive";
            case "js": return "application/x-javascript";
            case "latex": return "application/x-latex";
            case "lsp": return "application/x-lisp";
            case "man": return "application/x-troff-man";
            case "me": return "application/x-troff-me";
            case "mif": return "application/vnd.mif";
            case "ms": return "application/x-troff-ms";
            case "nc": return "application/x-netcdf";
            case "oda": return "application/oda";
            case "pdf": return "application/pdf";
            case "pgn": return "application/x-chess-pgn";
            case "pot":
            case "pps":
            case "ppt":
            case "ppz": return "application/mspowerpoint";
            case "pre": return "application/x-freelance";
            case "prt": return "application/pro_eng";
            case "ai":
            case "eps":
            case "ps": return "application/postscript";
            case "roff": return "application/x-troff";
            case "scm": return "application/x-lotusscreencam";
            case "set": return "application/set";
            case "sh": return "application/x-sh";
            case "shar": return "application/x-shar";
            case "sit": return "application/x-stuffit";
            case "skd":
            case "skm":
            case "skp":
            case "skt": return "application/x-koan";
            case "smi":
            case "smil": return "application/smil";
            case "sol": return "application/solids";
            case "spl": return "application/x-futuresplash";
            case "src": return "application/x-wais-source";
            case "step": return "application/STEP";
            case "stl": return "application/SLA";
            case "stp": return "application/STEP";
            case "sv4cpio": return "application/x-sv4cpio";
            case "sv4crc": return "application/x-sv4crc";
            case "swf": return "application/x-shockwave-flash";
            case "t": return "application/x-troff";
            case "tar": return "application/x-tar";
            case "tcl": return "application/x-tcl";
            case "tex": return "application/x-tex";
            case "texi":
            case "texinfo": return "application/x-texinfo";
            case "tr": return "application/x-troff";
            case "tsp": return "application/dsptype";
            case "unv": return "application/i-deas";
            case "ustar": return "application/x-ustar";
            case "vcd": return "application/x-cdlink";
            case "vda": return "application/vda";
            case "xlc":
            case "xll":
            case "xlm":
            case "xls":
            case "xlw": return "application/vnd.ms-excel";
            case "zip": return "application/zip";
            case "bin":
            case "class":
            case "dms":
            case "exe":
            case "lha":
            case "lzh": return "application/octet-stream";

            // Otros
            case "mesh":
            case "msh":
            case "silo": return "model/mesh";
            case "mime": return "www/mime";
            case "iges":
            case "igs": return "model/iges";
            case "vrml":
            case "wrl": return "model/vrml";
         }

         // Obtiene los permisos de acceso al registro de Windows
         RegistryPermission permission = new RegistryPermission(RegistryPermissionAccess.Read, "\\HKEY_CLASSES_ROOT");

         // Realiza la consulta al registro
         RegistryKey root = Registry.ClassesRoot;
         RegistryKey type = root.OpenSubKey("MIME\\Database\\Content Type");
         foreach (string key in type.GetSubKeyNames())
         {
            RegistryKey curKey = root.OpenSubKey("MIME\\Database\\Content Type\\" + key);
            if (curKey.GetValue("Extension").ToString().ToLower().Equals(extension))
               return key;
         }

         // Si no encuentra el tipo, se devuelve como un archivo binario
         return "application/octet-stream";
      }
   }
}
