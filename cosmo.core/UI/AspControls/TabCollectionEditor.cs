using System;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace Cosmo.UI.AspControls
{
   /// <summary>
   /// To Do
   /// </summary>
   [EditorAttribute(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
   public class TabCollectionEditor : CollectionEditor
   {
      public TabCollectionEditor(Type type) : base(type) { }

      protected override bool CanSelectMultipleInstances()
      {
         return false;
      }

      protected override Type CreateCollectionItemType()
      {
         return typeof(Tab);
      }
   }
}

