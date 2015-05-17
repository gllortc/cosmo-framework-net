using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cosmo.Utils.Drawing
{

   /// <summary>
   /// Implementa una clase que permite el manejo básico de imágenes.
   /// </summary>
   [Obsolete()]
   public class Thumbnail
   {

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
      public static Bitmap Create(string filename, int maxWidth, int maxHeight, bool save)
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
