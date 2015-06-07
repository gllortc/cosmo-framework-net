using Cosmo.Utils.Drawing;
using Cosmo.Utils.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa un campo que permite incorporar archivos.
   /// </summary>
   public class FormFieldFile : FormField
   {
      // Declaración de variables internas
      private FileInfo _value;
      private FileInfo _thumbnail;
      private List<string> _allowedExt;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldFile"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      public FormFieldFile(View parentViewport, string domId)
         : base(parentViewport, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldFile"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      /// <param name="label"></param>
      public FormFieldFile(View parentViewport, string domId, string label)
         : base(parentViewport, domId)
      {
         Initialize();

         this.Label = label;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldFile"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      /// <param name="label"></param>
      /// <param name="value"></param>
      public FormFieldFile(View parentViewport, string domId, string label, string value)
         : base(parentViewport, domId)
      {
         Initialize();

         this.Label = label;
         this.Value = value;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="FormFieldFile"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador único del componente dentro de una vista.</param>
      /// <param name="label"></param>
      /// <param name="description"></param>
      /// <param name="value"></param>
      public FormFieldFile(View parentViewport, string domId, string label, string description, string value)
         : base(parentViewport, domId)
      {
         Initialize();

         this.Label = label;
         this.Value = value;
         this.Description = description;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el tipo de campo implementado.
      /// </summary>
      public override FieldTypes FieldType
      {
         get { return FieldTypes.Upload; }
      }

      /// <summary>
      /// Indica si el campo es obligatorio.
      /// </summary>
      public bool Required { get; set; }

      /// <summary>
      /// Devuelve o establece el texto que mostrará la etiqueta del campo.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Devuelve o establece un texto en el control que desaparece cuando contiene algún valor.
      /// </summary>
      public string Placeholder { get; set; }

      /// <summary>
      /// Devuelve o establece una descripción que aparecerá en pequeño junto al campo (dependiendo de la plantilla de renderización).
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Devuelve o establece el nombre que recibirá el archivo al ser guardado en el servidor.
      /// Si esta propiedad se deja en blanco, se usará el nombre original.
      /// </summary>
      public string FileName { get; set; }

      /// <summary>
      /// Devuelve o establece la ruta a la carpeta dónde se debe almacenar el archivo recibido.
      /// </summary>
      public string DowloadPath { get; set; }

      /// <summary>
      /// Indica si se debe crear una imagen miniatura del archivo original.
      /// Sólo funciona con archivos JPEG, GIF y PNG.
      /// </summary>
      public bool CreateThumbnail { get; set; }

      /// <summary>
      /// Devuelve o establece la anchura máxima de la imagen miniatura a crear si la propiedad <c>CreateThumbnail = true</c>.
      /// </summary>
      public int ThumbnailMaxWith { get; set; }

      /// <summary>
      /// Devuelve o establece la altura máxima de la imagen miniatura a crear si la propiedad <c>CreateThumbnail = true</c>.
      /// </summary>
      public int ThumbnailMaxHeight { get; set; }

      /// <summary>
      /// Devuelve o establece el tamaño máximo que puede tener el archivo transferido.
      /// Si se especifica <c>MaxLength <= 0</c> se interpreta como tamaño ilimitado.
      /// </summary>
      public int MaxLength { get; set; }

      /// <summary>
      /// Devuelve o establece la lista de extensiones de archivo permitidas.
      /// </summary>
      public List<string> AllowedExtensions
      {
         get { return _allowedExt; }
         set 
         { 
            _allowedExt = value; 

            // Formatea el contenido: minúsculas y sin punto inicial
            int index = 0;
            foreach (string ext in _allowedExt)
            {
               _allowedExt[index] = ext.Trim().ToLower().Replace(".", string.Empty);
               index++;
            }
         }
      }

      /// <summary>
      /// Devuelve el archivo correspondiente a la imagen miniatura generada al subir el archivo.
      /// </summary>
      public FileInfo Thumbnail
      {
         get { return _thumbnail; }
      }

      /// <summary>
      /// Devuelve el valor del campo.
      /// </summary>
      /// <remarks>
      /// Aunque elsta propiedad se puede establecer, omite cualquier operación. Esta propiedad es de
      /// solo lectura puesto que los campos archivo en un formulario no permiten pre-establecerlos 
      /// a un valor.
      /// </remarks>
      public override object Value
      {
         get { return _value; }
         set { }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Obtiene el valor del campo a partir de los datos recibidos mediante GET o POST.
      /// </summary>
      public override bool LoadValueFromRequest()
      {
         try
         {
            bool valid = Validate();

            if (valid)
            {
               // Obtiene el archivo
               HttpPostedFile file = ParentView.Workspace.Context.Request.Files[this.DomID];

               // Genera el nombre del archivo
               string filePath = string.IsNullOrWhiteSpace(this.FileName) ? file.FileName : this.FileName;
               filePath = PathInfo.Combine(this.DowloadPath, filePath);

               // Guarda el archivo
               file.SaveAs(filePath);

               // Guarda como valor una instancia de FileInfo que permite el acceso al archivo
               _value = new FileInfo(filePath);

               // Ejecuta las acciones
               if (this.CreateThumbnail)
               {
                  _thumbnail = CreateImageThumbnail(_value.FullName);
               }
            }

            return true;
         }
         catch
         {
            _value = null;
            return false;
         }
      }

      /// <summary>
      /// Valida el valor del campo.
      /// </summary>
      /// <returns><c>true</c> si el valor és aceptable o <c>false</c> si el valor no es válido.</returns>
      public override bool Validate()
      {
         bool valid = true;

         if (this.Required)
         {
            // Verifica el envio del archivo
            if (ParentView.Workspace.Context.Request.Files.Count <= 0)
               return false;

            if (ParentView.Workspace.Context.Request.Files[this.DomID] == null)
               return false;

            // Verifica que el archivo contenga datos
            if (ParentView.Workspace.Context.Request.Files[this.DomID].ContentLength <= 0)
               return false;

            // Verifica el tamaño máximo del archivo
            if (this.MaxLength > 0)
            {
               if (ParentView.Workspace.Context.Request.Files[this.DomID].ContentLength > this.MaxLength)
                  return false;
            }

            // Verifica las extensiones de archivo
            bool isAllowed = false;
            FileInfo file = null;
            if (AllowedExtensions.Count > 0)
            {
               foreach (string ext in _allowedExt)
               {
                  file = new FileInfo(ParentView.Workspace.Context.Request.Files[this.DomID].FileName);
                  isAllowed = isAllowed | file.Extension.ToLower().Equals("." + ext.ToLower());
               }

               valid = valid && isAllowed;
            }

            return valid;
         }

         return true;
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Required = false;
         this.Label = string.Empty;
         this.Placeholder = string.Empty;
         this.Description = string.Empty;
         this.Value = null;
         this.DowloadPath = string.Empty;
         this.MaxLength = 0;
         this.AllowedExtensions = new List<string>();
         this.CreateThumbnail = false;
         this.ThumbnailMaxHeight = 0;
         this.ThumbnailMaxWith = 0;

         _thumbnail = null;
      }

      /// <summary>
      /// Genera una imagen miniatura del archivo indicado.
      /// </summary>
      /// <param name="filename">Nombre (con ruta) del archivo para el que se desea obtener la miniatura.</param>
      private FileInfo CreateImageThumbnail(string filename)
      {
         string originalFilename = string.Empty;
         string thumbFilename = string.Empty;
         FileInfo imgfile = null;

         try
         {
            // Se asegura de la existencia y accesibilidad del archivo
            imgfile = new FileInfo(filename);
            if (!imgfile.Exists)
            {
               throw new Exception("No se encuentra o no está disponible el archivo de imagen.");
            }

            if (DrawingUtils.IsImageFile(filename))
            { 
               // Obtiene la imagen
               Image image = Image.FromFile(imgfile.FullName);

               // Obtiene las medidas exactas de la imagen miniatura
               SizeF thumbSize = GetThumbnailSize(image.Width, image.Height, ThumbnailMaxWith, ThumbnailMaxHeight);

               // Genera la imagen miniatura
               Image thumb = image.GetThumbnailImage((int)thumbSize.Width,
                                                     (int)thumbSize.Height,
                                                     new Image.GetThumbnailImageAbort(ThumbnailCallback),
                                                     IntPtr.Zero);

               // Guarda la imagen miniatura
               thumbFilename = GenerateThumbnailFilename(imgfile.FullName);
               thumb.Save(thumbFilename);

               return new FileInfo(thumbFilename);
            }
            else
            {
               return null;
            }
         }
         catch (Exception ex)
         {
            throw ex;
         }
      }

      /// <summary>
      /// Required, but not used
      /// </summary>
      /// <returns>true</returns>
      private bool ThumbnailCallback()
      {
         return true;
      }

      /// <summary>
      /// Devuelve un nombre válido para una imagen miniatura
      /// </summary>
      /// <param name="filename">Nombre de la imagen original (con path).</param>
      /// <returns>El nombre a usar para la imagen miniatura.</returns>
      private string GenerateThumbnailFilename(string filename)
      {
         FileInfo imgfile = new FileInfo(filename);
         if (!imgfile.Exists)
         {
            throw new Exception("El archivo " + imgfile.Name + " no se encuentra o no está disponible.");
         }

         return imgfile.FullName.ToLower().Replace(imgfile.Extension.ToLower(), string.Empty) + "_th" + imgfile.Extension.ToLower();
      }

      /// <summary>
      /// Calcula las dimensiones reales del thumbnail.
      /// http://stackoverflow.com/questions/5222711/image-resize-in-c-sharp-algorith-to-determine-resize-dimensions-height-and-wi
      /// </summary>
      private SizeF GetThumbnailSize(float imageWidth, float imageHeight, float thumbMaxWidth, float thumbMaxHeight)
      {
         // Caso 1: Foto cuadrada
         if (imageWidth == imageHeight)
         {
            return new SizeF(thumbMaxWidth, thumbMaxWidth);
         }

         // Caso 2: Foto apaisada
         else if (imageWidth > imageHeight)
         {
            float ratio = imageHeight / imageWidth;
            return new SizeF(thumbMaxWidth, thumbMaxWidth * ratio);
         }

         // Caso 3: Foto vertical
         else
         {
            float ratio = imageHeight / imageWidth;
            return new SizeF(thumbMaxWidth, thumbMaxWidth * ratio);
         }
      }

      #endregion
   }
}
