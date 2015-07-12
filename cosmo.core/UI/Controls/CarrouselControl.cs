using System.Collections.Generic;
using System.Text;

namespace Cosmo.UI.Controls
{
   /// <summary>
   /// Implementa el carrousel (slider) de Bootstrap.
   /// </summary>
   /// <remarks>
   /// http://getbootstrap.com/javascript/#carousel
   /// </remarks>
   public class CarrouselControl : Control
   {

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="CarrouselControl"/>.
      /// </summary>
      /// <param name="parentView">Página o contenedor dónde se representará el control.</param>
      /// <param name="domId">Control unique identifier in view (HTML DOM).</param>
      public CarrouselControl(View parentView, string domId)
         : base(parentView, domId)
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Contiene los slides del carrousel.
      /// </summary>
      public List<CarrouselSlide> Slides { get; set; }

      /// <summary>
      /// Gets or sets el ancho del control.
      /// </summary>
      public int Width { get; set; }

      /// <summary>
      /// Gets or sets la altura del control.
      /// </summary>
      public int Height { get; set; }

      /// <summary>
      /// Gets or sets el tiempo entre slide y slide.
      /// </summary>
      public int Timeout { get; set; }

      /// <summary>
      /// Gets or sets el porcentaje de opacidad del fondo contenedor de texto del slide (0 a 100).
      /// </summary>
      public int CaptionOpacity { get; set; }

      /// <summary>
      /// Gets or sets el color del texto del slide.
      /// </summary>
      public string CaptionFontColor
      {
         get { return CaptionFontColor; }
         set
         {
            CaptionFontColor = value.Replace("#", string.Empty).ToLower();
         }
      }

      /// <summary>
      /// Gets or sets el color de fondo del contenedor de texto del slide.
      /// </summary>
      public string CaptionBackgroundColor
      {
         get { return CaptionBackgroundColor; }
         set
         {
            CaptionBackgroundColor = value.Replace("#", string.Empty).ToLower();
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Inicializa el control.
      /// </summary>
      /// <param name="container">Una instancia que describa la página que debe contener el control.</param>
      public void Initialize(System.Web.UI.Page page)
      {
         // Nothing to do
      }
      /*
      /// <summary>
      /// Convierte el componente en código XHTML.
      /// </summary>
      /// <returns>Una cadena que contiene el código XHTML necesario para representar el componente en un navegador.</returns>
      public string ToXhtml()
      {
         int count;
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("");

         // Obtiene la altura máxima del control
         int height = 338;
         foreach (CarrouselSlide slide in _slides)
         {
            // TODO: Sólo funciona si se guardan las medidas en la BBDD
            if (slide.Height > height) height = slide.Height;
         }

         xhtml.Append("<div " + GetIdParameter() + "class=\"carousel slide\" data-ride=\"carousel\">\n");
         xhtml.Append("  <ol class=\"carousel-indicators\">\n");

         count = 0;
         foreach (CarrouselSlide slide in _slides)
         {
            xhtml.Append("    <li data-target=\"#" + this.DomID + "\" data-slide-to=\"" + count + "\" class=\"" + (count == 0 ? "active" : "") + "\"></li>\n");
            count++;
         }
         
         xhtml.Append("  </ol>\n");
         xhtml.Append("  <div class=\"carousel-inner\">\n");

         count = 0;
         foreach (CarrouselSlide slide in _slides)
         {
            xhtml.Append("    <div class=\"item " + (count == 0 ? "active" : "") + "\">\n");
            xhtml.Append("       <img alt=\"" + slide.PropertyName.Replace("\"", "") + "\" src=\"" + slide.FileName + "\">\n");
            xhtml.Append("    </div>\n");
            count++;
         }

         xhtml.Append("  </div>\n");
         xhtml.Append("  <a class=\"left carousel-control\" href=\"#" + this.DomID + "\" data-slide=\"prev\">\n");
         xhtml.Append("    <span class=\"glyphicon glyphicon-chevron-left\"></span>\n");
         xhtml.Append("  </a>\n");
         xhtml.Append("  <a class=\"right carousel-control\" href=\"#" + this.DomID + "\" data-slide=\"next\">\n");
         xhtml.Append("    <span class=\"glyphicon glyphicon-chevron-right\"></span>\n");
         xhtml.Append("  </a>\n");
         xhtml.Append("</div>\n");

         return xhtml.ToString();
      }
      */
      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         this.Width = 400;
         this.Height = 300;
         this.CaptionOpacity = 70;
         this.Timeout = 3000;
         this.CaptionFontColor = "fff";
         this.CaptionBackgroundColor = "000";
         this.Slides = new List<CarrouselSlide>();
      }

      private string ConvertPositionToClass(SlideCaptionPositions position)
      {
         switch (position)
         {
            case SlideCaptionPositions.Bottom: return "cs_slide_bottom";
            case SlideCaptionPositions.Left: return "cs_slide_left";
            case SlideCaptionPositions.Right: return "cs_slide_right";
            case SlideCaptionPositions.Top: return "cs_slide_top";
         }

         return "";
      }

      /// <summary>
      /// Genera el código JavaScript que activa el control.
      /// </summary>
      private string GenerateJs()
      {
         StringBuilder js = new StringBuilder();

         // Genera el código javaScript
         js.AppendLine("<script type=\"text/javascript\">");
         js.AppendLine("  $(document).ready(function() {");
         js.AppendLine("    $('.carousel').carousel({");
         js.AppendLine("      interval: 4000");
         js.AppendLine("    });");
         js.AppendLine("  });");
         js.AppendLine("</script>");

         return js.ToString();
      }

      #endregion

   }
}