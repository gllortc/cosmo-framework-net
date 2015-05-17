using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.Net.Mail
{

   /// <summary>
   /// Enumera los tipos de codificación para la transferencia de datos
   /// </summary>
   public enum TransferEncoding
   {
      /// <summary>Codificación mediante 7 bits</summary>
      SevenBit,
      /// <summary>Codificación mediante Base64</summary>
      Base64, 
      /// <summary>Codificación mediante QuoetPrintable</summary>
      QuotedPrintable
   }

}
