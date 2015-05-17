using System;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa los argumentos que passan los eventos de <see cref="DomPage"/>.
   /// </summary>
   public class DomFormEventArgs : EventArgs
   {
      private DomForm _form = null;

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFormEventArgs"/>.
      /// </summary>
      public DomFormEventArgs() : base() { }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomFormEventArgs"/>.
      /// </summary>
      public DomFormEventArgs(DomForm form) : base()
      {
         _form = form;
      }

      /// <summary>
      /// Contiene los parámetros de la llamada.
      /// </summary>
      public DomForm Form
      {
         get { return _form; }
      }
   }

}
