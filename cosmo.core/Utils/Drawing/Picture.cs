using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Cosmo.Utils.Drawing
{
   /// <summary>
   /// Representa una imagen y implementa las operaciones más habituales.
   /// </summary>
   public class Picture
   {

      #region Enumerations

      /// <summary>
      /// Enumera los tipos de imagen soportados por la clase <see cref="Picture"/>.
      /// </summary>
      public enum PictureFormats
      {
         /// <summary>Formato JPEG.</summary>
         JPEG,
         /// <summary>Formato GIF.</summary>
         GIF,
         /// <summary>Formato PNG.</summary>
         PNG
      }

      #endregion

      private string _filename;

      /// <summary>
      /// Gets a new instance of <see cref="Picture"/>.
      /// </summary>
      public Picture()
      {
         _filename = string.Empty;
      }

      /// <summary>
      /// Gets a new instance of <see cref="Picture"/>.
      /// </summary>
      /// <param name="filename">Nombre (y ruta) del archivo de imagen.</param>
      public Picture(string filename) : this()
      {
         _filename = filename;
      }

      /// <summary>
      /// Genera una imagen miniatura de la imagen.
      /// </summary>
      /// <param name="maxWidth">Anchura máxima de la imagen miniatura.</param>
      /// <param name="maxHeight">Alura máxima de la imagen miniatura.</param>
      /// <param name="save">Indica si se debe guardar la imagen resultante a un archivo.</param>
      /// <returns>Una instancia de <see cref="System.Drawing.Bitmap"/> que contiene la imagen miniatura.</returns>
      public Bitmap CreateThumbnail(int maxWidth, int maxHeight, bool save)
      {
         Bitmap image = null;
         Bitmap thumbnail = null;

         try
         {
            image = new Bitmap(_filename);
            ImageFormat imageformat = image.RawFormat;

            decimal lnRatio;
            int lnNewWidth = 0;
            int lnNewHeight = 0;

            //*** If the image is smaller than a thumbnail just return it
            if (image.Width < maxWidth && image.Height < maxHeight)
               return image;

            if (image.Width > image.Height)
            {
               lnRatio = (decimal)maxWidth / image.Width;
               lnNewWidth = maxWidth;
               decimal lnTemp = image.Height * lnRatio;
               lnNewHeight = (int)lnTemp;
            }
            else
            {
               lnRatio = (decimal)maxHeight / image.Height;
               lnNewHeight = maxHeight;
               decimal lnTemp = image.Width * lnRatio;
               lnNewWidth = (int)lnTemp;
            }

            thumbnail = new Bitmap(lnNewWidth, lnNewHeight);
            Graphics g = Graphics.FromImage(thumbnail);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
            g.DrawImage(image, 0, 0, lnNewWidth, lnNewHeight);

            // Guarda el thumnail añadiendo el prefijo "TH_" delante del nombre
            if (save)
            {
               string path = _filename.Substring(0, _filename.LastIndexOf("\\") + 1);
               string name = _filename.Substring(_filename.LastIndexOf("\\") + 1);
               thumbnail.Save(path + "th_" + name);
            }

            return thumbnail;
         }
         catch
         {
            return null;
         }
         finally
         {
            image.Dispose();
         }
      }

      /// <summary>
      /// Transforma la imagen a un formato distinto. 
      /// </summary>
      /// <param name="format">Formato de destino.</param>
      /// <param name="filename">Archivo de destino.</param>
      /// <param name="jpegQuality">Calidad de la imagen para el formato de destino JPEG.</param>
      public void Transform(PictureFormats format, string filename, float jpegQuality)
      {
         switch (format)
         {
            case PictureFormats.JPEG:
               {
                  DrawingUtils.ProgressUpdater progressObj = new DrawingUtils.ProgressUpdater();
                  DrawingUtils.CurrentOperationUpdater currentOperationObj = new DrawingUtils.CurrentOperationUpdater();

                  Bitmap bmp = this.ToBitmap();
                  byte[, ,] image_array = DrawingUtils.FillImageBuffer(bmp, progressObj, currentOperationObj);

                  Point originalDimension = new Point(bmp.Width, bmp.Height);
                  Point actualDimension = DrawingUtils.GetActualDimension(originalDimension);

                  FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                  BinaryWriter bw = new BinaryWriter(fs);

                  JpegBaseEncoder encoder = new JpegBaseEncoder();

                  encoder.EncodeImageBufferToJpg(image_array, originalDimension, actualDimension, bw, jpegQuality, progressObj, currentOperationObj);
               }
               break;
         }
      }

      /// <summary>
      /// Transforma la imagen a un formato distinto. 
      /// </summary>
      /// <param name="format">Formato de destino.</param>
      /// <param name="filename">Archivo de destino.</param>
      public void Transform(PictureFormats format, string filename)
      {
         this.Transform(format, filename, 0);
      }

      /// <summary>
      /// Convierte la imagen en una instancia de <see cref="System.Drawing.Bitmap"/>.
      /// </summary>
      public Bitmap ToBitmap()
      {
         return new Bitmap(_filename);
      }

      /// <summary>
      /// Convierte la imagen en una instancia de <see cref="System.Drawing.Image"/>.
      /// </summary>
      public Image ToImage()
      {
         return (Image)this.ToBitmap();
      }

      #region Static Members

      /// <summary>
      /// Genera una imagen miniatura a partir de una imagen original
      /// </summary>
      /// <param name="filename">Nombre del archivo que contiene la imagen</param>
      /// <param name="maxWidth">Ancho máximo de la imagen resultante</param>
      /// <param name="maxHeight">Altura máxima de la imagen resultante</param>
      /// <param name="save">Indica si se debe guardar la imagen resultante</param>
      /// <returns>Una instancia Bitmap que contiene la imagen miniatura</returns>
      /// <remarks>
      /// Al guardar la imagen miniatura se renombra añadiendo el prefijo "th_" delante del nombre 
      /// y se mantiene la misma ruta.
      /// </remarks>
      public static Bitmap CreateThumbnail(string filename, int maxWidth, int maxHeight, bool save)
      {
         Bitmap image = null;
         Bitmap thumbnail = null;

         try
         {
            image = new Bitmap(filename);
            ImageFormat imageformat = image.RawFormat;

            decimal lnRatio;
            int lnNewWidth = 0;
            int lnNewHeight = 0;

            //*** If the image is smaller than a thumbnail just return it
            if (image.Width < maxWidth && image.Height < maxHeight)
               return image;

            if (image.Width > image.Height)
            {
               lnRatio = (decimal)maxWidth / image.Width;
               lnNewWidth = maxWidth;
               decimal lnTemp = image.Height * lnRatio;
               lnNewHeight = (int)lnTemp;
            }
            else
            {
               lnRatio = (decimal)maxHeight / image.Height;
               lnNewHeight = maxHeight;
               decimal lnTemp = image.Width * lnRatio;
               lnNewWidth = (int)lnTemp;
            }

            thumbnail = new Bitmap(lnNewWidth, lnNewHeight);
            Graphics g = Graphics.FromImage(thumbnail);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
            g.DrawImage(image, 0, 0, lnNewWidth, lnNewHeight);

            // Guarda el thumnail añadiendo el prefijo "TH_" delante del nombre
            if (save)
            {
               string path = filename.Substring(0, filename.LastIndexOf("\\") + 1);
               string name = filename.Substring(filename.LastIndexOf("\\") + 1);
               thumbnail.Save(path + "th_" + name);
            }

            return thumbnail;
         }
         catch
         {
            return null;
         }
         finally
         {
            image.Dispose();
            // thumbnail.Dispose();
         }
      }

      #endregion
   }
}
