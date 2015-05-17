using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.Net.Mail
{

   /// <summary>
   /// Enumera los tipos de codificaci�n para la transferencia de datos
   /// </summary>
   public enum TransferEncoding
   {
      /// <summary>Codificaci�n mediante 7 bits</summary>
      SevenBit,
      /// <summary>Codificaci�n mediante Base64</summary>
      Base64, 
      /// <summary>Codificaci�n mediante QuoetPrintable</summary>
      QuotedPrintable
   }

}
