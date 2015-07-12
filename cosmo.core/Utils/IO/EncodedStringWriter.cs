using System.IO;
using System.Text;

namespace Cosmo.Utils.IO
{

   /// <summary>
   /// Implementa una clase StringBuilder que permite especificar la codificación de sus carácteres
   /// </summary>
   public class EncodedStringWriter : StringWriter
   {
      Encoding encoding;

      /// <summary>
      /// Gets a new instance of EncodedStringWriter.
      /// </summary>
      /// <param name="builder"></param>
      /// <param name="encoding"></param>
      public EncodedStringWriter(StringBuilder builder, Encoding encoding) : base(builder)
      {
         this.encoding = encoding;
      }

      /// <summary>
      /// Devuelve la codificación.
      /// </summary>
      public override Encoding Encoding
      {
         get { return encoding; }
      }
   }
}
