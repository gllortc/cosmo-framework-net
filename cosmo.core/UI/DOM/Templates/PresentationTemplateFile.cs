using Cosmo.FileSystem;
using Cosmo.Utils.IO;
using System.IO;

namespace Cosmo.UI.DOM.Templates
{

   /// <summary>
   /// Implementa un archivo adjunto de la plantilla.
   /// </summary>
   public class PresentationTemplateFile
   {
      private string _name = "";
      private string _filename = "";
      private WorkspaceFolders _destination = WorkspaceFolders.Unknown;
      private int _len = 0;
      private byte[] _buffer = null;

      /// <summary>
      /// Devuelve una instancia de PresentationTemplateFile
      /// </summary>
      public PresentationTemplateFile()
      {
         _name = "";
         _filename = "";
         _destination = WorkspaceFolders.Unknown;
         _len = 0;
         _buffer = null;
      }

      /// <summary>
      /// Devuelve una instancia de PresentationTemplateFile
      /// </summary>
      /// <param name="filename">Nombre+ruta del archivo</param>
      /// <param name="destination">Identificador de la carpeta de destino</param>
      public PresentationTemplateFile(string filename, WorkspaceFolders destination)
      {
         _destination = destination;

         // Lee el archivo
         FileInfo file = new FileInfo(filename);
         _name = file.Name;
         _filename = file.FullName;

         /*FileStream stream = file.OpenRead();
         _len = Convert.ToInt32(stream.Length);

         byte[] buffer = new byte[_len];
         stream.Read(_buffer, 0, _len);
         stream.Close();*/

         _buffer = FileReader.LoadTextFileToByteArray(file.FullName);
      }

      #region Properties

      /// <summary>
      /// Contiene de forma temporal el nombre y ruta del archivo.
      /// </summary>
      public string FileName
      {
         get { return _filename; }
         set { _filename = value; }
      }

      /// <summary>
      /// Nombre del archivo.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Carpeta de destino dónde se debe almacenar el archivo.
      /// </summary>
      public WorkspaceFolders Destination
      {
         get { return _destination; }
         set { _destination = value; }
      }

      /// <summary>
      /// Contiene la longitud (en bytes) del archivo (en el buffer).
      /// </summary>
      public int Length
      {
         get { return _len; }
         set { _len = value; }
      }

      /// <summary>
      /// Buffer que contiene el archivo.
      /// </summary>
      public byte[] Buffer
      {
         get { return _buffer; }
         set { _buffer = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Guarda el archivo en una carpeta específica.
      /// </summary>
      /// <param name="path">Ruta dónde se debe almacenar el archivo.</param>
      /// <remarks>Si el archivo existe, lo sobreescribe.</remarks>
      public void Save(string path)
      {
         FileInfo file = new FileInfo(Path.Combine(path, _name));

         // Si existe el archivo, lo elimina
         if (file.Exists) file.Delete();

         // Guarda el archivo
         FileStream fstream = file.Create();
         fstream.Write(_buffer, 0, _len);
         fstream.Close();
      }

      #endregion

   }
}
