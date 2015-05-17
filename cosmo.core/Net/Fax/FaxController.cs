using System;
using System.Drawing;
using System.Drawing.Printing;

namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Controller for print processing to fax _printer.
   /// </summary>
   public class FaxController : PrintController
   {
      private IntPtr dc;

      internal FaxController(IntPtr dc) : base()
      {
         this.dc = dc;
      }

      /// <overloads></overloads>
      public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
      {
         if (e.Cancel == true)
         {
            NativeMethods.AbortDoc(this.dc);
            return null;
         }

         return Graphics.FromHdc(this.dc);
      }

      /// <overloads></overloads>
      public override void OnStartPrint(PrintDocument document, PrintEventArgs e)
      {
         // Dont call the StartDoc function, because FaxStartPrintJob function do this automaticly !
         // But, we must override this method, for preventing the default behavior of the OnStartPrint method.
      }

      /// <overloads></overloads>
      public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
      {
         if (e.HasMorePages == true)
            NativeMethods.EndPage(this.dc);

         base.OnEndPage(document, e);
      }

      /// <overloads></overloads>
      public override void OnEndPrint(PrintDocument document, PrintEventArgs e)
      {
         if (e.Cancel == false)
            NativeMethods.EndDoc(this.dc);

         base.OnEndPrint(document, e);
      }
   }

}
