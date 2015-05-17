using System;
using System.ComponentModel;

namespace Cosmo.UI.AspControls
{
   /// <summary>
   /// Implementa un control TAB.
   /// </summary>
   [TypeConverter(typeof(ExpandableObjectConverter))]
   public class Tab
   {
      private string _title;
      private string _link;
      private bool _selected;

      /// <summary>
      /// Devuelve una instancia de CSTab
      /// </summary>
      public Tab() : this(String.Empty, String.Empty, false) { }

      /// <summary>
      /// Devuelve una instancia de CSTab
      /// </summary>
      public Tab(string title, string link, bool selected)
      {
         _title = title;
         _link = link;
         _selected = selected;
      }

      /// <summary>
      /// Devuelve una instancia de CSTab
      /// </summary>
      public Tab(string title, string link)
      {
         _title = title;
         _link = link;
         _selected = false;
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el título vosoble de la pestaña.
      /// </summary>
      [Category("Behavior"),
       DefaultValue(""),
       Description("Tab title"),
       NotifyParentProperty(true)]
      public string Title
      {
         get { return _title; }
         set { _title = value; }
      }

      /// <summary>
      /// Devuelve o establece el enlace asociado a la pestaña.
      /// </summary>
      [Category("Behavior"),
       DefaultValue(""),
       Description("Tab link"),
       NotifyParentProperty(true)]
      public string Link
      {
         get { return _link; }
         set { _link = value; }
      }

      /// <summary>
      /// Indica si la pestaña está seleccionada.
      /// </summary>
      /// <remarks>
      /// En un control CTabs siempre aparecerá como seleccionada la primera pestaña seleccionada si hay más de una.
      /// </remarks>
      [Category("Behavior"),
       DefaultValue(""),
       Description("Indicate if the tab is selected"),
       NotifyParentProperty(true)]
      public bool Selected
      {
         get { return _selected; }
         set { _selected = value; }
      }

      #endregion

   }
}

