using System.Drawing.Printing;
using System.Reflection;
using System.Collections.Generic;

namespace Cosmo.Utils.Drawing
{

   /// <summary>
   /// Implementa un documento de impresión formado por uno o más documerntos de impresión.
   /// </summary>
   public class PrintDocuments : PrintDocument
   {
      // Declaración de variables internas
      private List<PrintDocument> _documents;
      private int _docIndex;
      private PrintEventArgs _args;

      /// <summary>
      /// Devuelve una instancia de <see cref="PrintDocuments"/>.
      /// </summary>
      public PrintDocuments()
      {
         _documents = new List<PrintDocument>();
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="PrintDocuments"/>.
      /// </summary>
      public PrintDocuments(PrintDocument document1, PrintDocument document2)
      {
         _documents = new List<PrintDocument>();
         _documents.Add(document1);
         _documents.Add(document2);
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="PrintDocuments"/>.
      /// </summary>
      public PrintDocuments(PrintDocument[] documents)
      {
         _documents = new List<PrintDocument>();
         _documents.AddRange(documents);
      }

      /// <summary>
      /// Anexa un documento de impresión al documento maestro.
      /// </summary>
      /// <param name="document">Una instancia de <see cref="PrintDocument"/>.</param>
      public void Add(PrintDocument document)
      {
         _documents.Add(document);
      }

      /// <summary>
      /// Evento que se llama al inicio de la impresión.
      /// </summary>
      protected override void OnBeginPrint(PrintEventArgs e)
      {
         base.OnBeginPrint(e);
         if (_documents.Count <= 0)
            e.Cancel = true;

         if (e.Cancel) return;

         _args = e;
         _docIndex = 0;  // reset current document index
         CallMethod(_documents[_docIndex], "OnBeginPrint", e);
      }

      /// <summary>
      /// Evento que se llama durante la consulta de la configuración de impresora.
      /// </summary>
      protected override void OnQueryPageSettings(QueryPageSettingsEventArgs e)
      {
         e.PageSettings = _documents[_docIndex].DefaultPageSettings;
         CallMethod(_documents[_docIndex], "OnQueryPageSettings", e);
         base.OnQueryPageSettings(e);
      }

      /// <summary>
      /// Evento que se llama al imprimir la página.
      /// </summary>
      protected override void OnPrintPage(PrintPageEventArgs e)
      {
         CallMethod(_documents[_docIndex], "OnPrintPage", e);
         base.OnPrintPage(e);
         if (e.Cancel) return;
         if (!e.HasMorePages)
         {
            CallMethod(_documents[_docIndex], "OnEndPrint", _args);
            if (_args.Cancel) return;
            _docIndex++;  // increments the current document index

            if (_docIndex < _documents.Count)
            {
               // says that it has more pages (in others documents)
               e.HasMorePages = true;
               CallMethod(_documents[_docIndex], "OnBeginPrint", _args);
            }
         }
      }

      // use reflection to call protected methods of child documents
      private void CallMethod(PrintDocument document, string methodName, object args)
      {
         typeof(PrintDocument).InvokeMember(methodName, 
                                            BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
                                            null, 
                                            document, 
                                            new object[] { args });
      }
   }
}
