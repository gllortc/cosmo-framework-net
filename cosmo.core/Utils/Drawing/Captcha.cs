using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Cosmo.Utils.Drawing
{
   /// <summary>
	/// Implementa un sistema de imágenes CAPTCHA para formularios.
	/// </summary>
	public class Captcha
	{
      private int _width = 200;
      private int _height = 50;
      private int _length = 6;
      private string _text = "";
      private Color _color = Color.Empty;
      private Bitmap _bitmap;
      private Graphics _graphics;

      private string[] charset = {  "I","6","a","R","8","W","b","c","B","9","d","X"
                                    ,"e","L","f","E","5","g","H","8","Y","h","V"
                                    ,"i","j","C","M","k","T","l","D","m","U","2"
                                    ,"Q","n","J","o","3","A","7","p","q","Z","r"
                                    ,"s","F","t","S","u","4","P","v","K","w","G"
                                    ,"x","O","y","z","0","N","1","f","E","5","g"
                                    ,"1","J","o","3","A","9"};
      private string[] fontset = {  "Georgia","Verdana","Tahoma"
                                    ,"Arial","Times New Roman","Garamond","Agency FB" };
      
      /// <summary>Clave por defecto dónde se guarda el código captcha en el objeto Session.</summary>
      public static string SESSION_CAPTCHA = "cosmo.web.session.captcha";

      /// <summary>
      /// Gets a new instance of Captcha.
      /// </summary>
      /// <remarks>
      /// Usa los siguientes valores por defecto:
      /// - ImageWidth    = 200
      /// - ImageHeight   = 50 
      /// - CaptchaLength = 6
      /// </remarks>
      public Captcha()
      {
         _bitmap = new Bitmap(_width, _height);
         _graphics = Graphics.FromImage(_bitmap);
      }

      /// <summary>
      /// Gets a new instance of Captcha.
      /// </summary>
      /// <param name="ImageWidth">Width of the GIF image produced.</param>
      /// <param name="Imageheight">Height of the GIF image produced.</param>
      /// <param name="Length">Número de carácteres que se muestran.</param>
      public Captcha(int ImageWidth, int Imageheight, int Length)
      {
         // TODO: Check if CaptchaLength > ImageWidth
         _width = ImageWidth;
         _height = Imageheight;
         _length = Length;
         _text = string.Empty;

         _bitmap = new Bitmap(_width, _height);
         _graphics = Graphics.FromImage(_bitmap);
      }

      /// <summary>
      /// Gets a new instance of Captcha.
      /// </summary>
      /// <param name="ImageWidth">Width of the GIF image produced.</param>
      /// <param name="Imageheight">Height of the GIF image produced.</param>
      /// <param name="Length">Número de carácteres que se muestran.</param>
      /// <param name="FilePath">Nombre y ruta del archivo que se generará al finalizar la generación.</param>
      public Captcha(int ImageWidth, int Imageheight, int Length, string FilePath)
      {
         // TODO: Check if CaptchaLength > ImageWidth
         _width = ImageWidth;
         _height = Imageheight;
         _length = Length;
         _text = string.Empty;

         _bitmap = new Bitmap(_width, _height);
         _graphics = Graphics.FromImage(_bitmap);
      }

      #region Settings

      /// <summary>
      /// Devuelve el código del captcha generado.
      /// </summary>
      public string Text
      {
         get { return _text; }
      }

      /// <summary>
      /// Color único con el que se generará la imagen Captcha.
      /// </summary>
      public Color Color
      {
         set { _color = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Genera una coloración aleatória (por defecto).
      /// </summary>
      public void ResetColor()
      {
         _color = Color.Empty;
      }

      /// <summary>
      /// Genera una imagen Captcha en un archivo GIF.
      /// </summary>
      /// <param name="FilePath">Nombre de archivo (más la ruta) dónde se almacenará el archivo GIF.</param>
      /// <returns>El código que contiene la imagen Captcha.</returns>
      public string GenerateCaptcha(string FilePath)
      {
         Generate();
         _bitmap.Save(FilePath, ImageFormat.Gif);

         return _text;
      }

      /// <summary>
      /// Genera una imagen Captcha para su uso en un script ASPX.
      /// </summary>
      /// <returns>Un objeto Bitmap que contiene la imagen a mostrar.</returns>
      public Bitmap GenerateCaptcha()
      {
         Generate();

         return _bitmap;
      }

      /// <summary>
      /// Genera una imagen Captcha para su uso en un script ASPX.
      /// </summary>
      /// <param name="Session">La instancia de Page.Session</param>
      /// <returns>Un objeto Bitmap que contiene la imagen a mostrar.</returns>
      public Bitmap GenerateCaptcha(System.Web.SessionState.HttpSessionState Session)
      {
         Generate();
         Session[Captcha.SESSION_CAPTCHA] = _text;

         return _bitmap;
      }

      /// <summary>
      /// Genera una imagen Captcha para su uso en un script ASPX.
      /// </summary>
      /// <param name="Session">La instancia de Page.Session</param>
      /// <param name="SessionKey">Clave del objeto Session en la que desea guardar el código representado en la imagen captcha.</param>
      /// <returns>Un objeto Bitmap que contiene la imagen a mostrar.</returns>
      public Bitmap GenerateCaptcha(System.Web.SessionState.HttpSessionState Session, string SessionKey)
      {
         Generate();
         Session[SessionKey] = _text;

         return _bitmap;
      }

      #endregion

      #region Static members

      /// <summary>
      /// Valida un código captcha leido por un usuario.
      /// </summary>
      /// <param name="Session">La instancia de Page.Session</param>
      /// <param name="value">Valor proporcionado por el usuario.</param>
      /// <returns>Un valor booleano que indica si la validación es correcta o no.</returns>
      /// <remarks>
      /// Este método se debe usar únicamente en conjunto con el método GenerateCaptcha(System.Web.SessionState.HttpSessionState).
      /// </remarks>
      public static bool ValidateCaptcha(System.Web.SessionState.HttpSessionState Session, string value)
      {
         try
         {
            return value.Equals(Session[Captcha.SESSION_CAPTCHA]);
         }
         catch
         {
            return false;
         }
      }

      #endregion

      #region Private members

      private string GetRandomString()
      {
         Random rand = new Random();
         StringBuilder sb = new StringBuilder(_length);

         for (int i = 0; i < _length; i++)
         {
            sb.Append(charset[rand.Next(charset.Length - 1)]);
         }
         return sb.ToString();
      }

      private void Generate()
      {
         _graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, _width, _height));

         Random rand = new Random();
         Random Colorrand = new Random();
         Random anglerand = new Random();
         Random sizerand = new Random();

         // Generate random arcs and lines to distort image
         Random numlines = new Random();
         Random coordinates = new Random();
         Random brushsize = new Random();
         for (int i = 0; i < numlines.Next(20, 50); i++)
         {
            Pen blackPen;

            if (_color != Color.Empty)
            {
               Color bgcolor = Color.FromArgb(80, _color);
               blackPen = new Pen(bgcolor, (float)brushsize.Next(4));
            }
            else
            {
               blackPen = new Pen(Color.FromArgb(Colorrand.Next(150, 200), Colorrand.Next(120, 240), Colorrand.Next(150, 200)), (float)brushsize.Next(4));
            }

            Rectangle rect = new Rectangle(coordinates.Next(0, _width), coordinates.Next(0, _height), coordinates.Next(50, _width), coordinates.Next(30, _height));
            float startAngle = (float)coordinates.Next(20, 45);
            float sweepAngle = (float)coordinates.Next(40, 170);
            _graphics.DrawArc(blackPen, rect, startAngle, sweepAngle);
            _graphics.DrawLine(blackPen, new Point(coordinates.Next(0, _width), coordinates.Next(0, _height)), new Point(coordinates.Next(0, _width), coordinates.Next(0, _height)));
         }

         _text = GetRandomString();

         // Initial margin from left and subsequent distance between characters
         int xpos = 10;

         for (int i = 0; i < _length; i++)
         {
            if (_color != Color.Empty)
            {
               _graphics.DrawString(_text[i].ToString(),
                                    new Font(fontset[rand.Next(fontset.Length - 1)], (float)sizerand.Next(20, 30)),
                                    new SolidBrush(_color),
                                    new PointF((float)xpos, 0.0F));
            }
            else
            {
               _graphics.DrawString(_text[i].ToString(),
                                    new Font(fontset[rand.Next(fontset.Length - 1)], (float)sizerand.Next(20, 30)),
                                    new SolidBrush(Color.FromArgb(Colorrand.Next(240), Colorrand.Next(100), Colorrand.Next(220))),
                                    new PointF((float)xpos, 0.0F));
            }

            xpos += sizerand.Next(20, 30);
         }
      }

      #endregion

   }
}
