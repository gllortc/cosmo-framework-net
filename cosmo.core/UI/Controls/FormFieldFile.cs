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
      // Internal data declarations
      private FileInfo _value;
      private FileInfo _thumbnail;
      private List<string> _allowedExt;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldFile"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public FormFieldFile(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldFile"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      public FormFieldFile(View parentView, string domId, string label)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldFile"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      /// <param name="value"></param>
      public FormFieldFile(View parentView, string domId, string label, string value)
         : base(parentView, domId)
      {
         Initialize();

         this.Label = label;
         this.Value = value;
      }

      /// <summary>
      /// Gets a new instance of <see cref="FormFieldFile"/>.
      /// </summary>
      /// <param name="parentView">Parent <see cref="View"/> which acts as a container of the control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      /// <param name="label"></param>
      /// <param name="description"></param>
      /// <param name="value"></param>
      public FormFieldFile(View parentView, string domId, string label, string description, string value)
         : base(parentView, domId)
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
      /// Gets or sets a boolean value indicating if the field is required to validate the form.
      /// </summary>
      public bool Required { get; set; }

      /// <summary>
      /// Gets or sets el texto que mostrará la etiqueta del campo.
      /// </summary>
      public string Label { get; set; }

      /// <summary>
      /// Gets or sets un texto en el control que desaparece cuando contiene algún valor.
      /// </summary>
      public string Placeholder { get; set; }

      /// <summary>
      /// Gets or sets una descripción que aparecerá en pequeño junto al campo (dependiendo de la plantilla de renderización).
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets el nombre que recibirá el archivo al ser guardado en el servidor.
      /// Si esta propiedad se deja en blanco, se usará el nombre original.
      /// </summary>
      public string FileName { get; set; }

      /// <summary>
      /// Gets or sets la ruta a la carpeta dónde se debe almacenar el archivo recibido.
      /// </summary>
      public string DowloadPath { get; set; }

      /// <summary>
      /// Indica si se debe crear una imagen miniatura del archivo original.
      /// Sólo funciona con archivos JPEG, GIF y PNG.
      /// </summary>
      public bool CreateThumbnail { get; set; }

      /// <summary>
      /// Gets or sets la anchura máxima de la imagen miniatura a crear si la propiedad <c>CreateThumbnail = true</c>.
      /// </summary>
      public int ThumbnailMaxWith { get; set; }

      /// <summary>
      /// Gets or sets la altura máxima de la imagen miniatura a crear si la propiedad <c>CreateThumbnail = true</c>.
      /// </summary>
      public int ThumbnailMaxHeight { get; set; }

      /// <summary>
      /// Gets or sets el tamaño máximo que puede tener el archivo transferido.
      /// Si se especifica <c>MaxLength <= 0</c> se interpreta como tamaño ilimitado.
      /// </summary>
      public int MaxLength { get; set; }

      /// <summary>
      /// Gets or sets la lista de extensiones de archivo permitidas.
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
      /// Gets the field value from the request.
      /// </summary>
      public override bool LoadValueFromRequest()
      {
         try
         {
            bool valid = Validate();

            if (valid)
            {
               // Get reveived file via HTTP
               HttpPostedFile file = ParentView.Workspace.Context.Request.Files[this.DomID];
               if (!string.IsNullOrWhiteSpace(file.FileName))
               {
                  // Generate the filename and path
                  string filePath = string.IsNullOrWhiteSpace(this.FileName) ? GetFileName(file.FileName) : this.FileName;
                  filePath = PathInfo.Combine(this.DowloadPath, filePath);

                  // Save the file to filesystem
                  file.SaveAs(filePath);

                  // Store the FileInfo instance as a field value to use later
                  _value = new FileInfo(filePath);

                  // Run additional field actions
                  if (this.CreateThumbnail)
                  {
                     _thumbnail = CreateImageThumbnail(_value.FullName);
                  }
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
      /// Validate the field value.
      /// </summary>
      /// <c>true</c> if the field value is valid or <c>false</c> if the value is not valid.
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
      /// Initializes the instance data.
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
      /// Gets the filename form a path.
      /// </summary>
      private string GetFileName(string path)
      {
         FileInfo file = new FileInfo(path);
         return file.Name;
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
