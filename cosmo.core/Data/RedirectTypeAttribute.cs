using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.Data
{
   /// <summary>
   /// Attribute that can be used to redirect types to other types to be able
   /// to rename / move property types.
   /// </summary>
   /// <remarks>
   /// This attribute should be appended to the property definition.
   /// 
   /// In case this attribute is used on a field or property, the <see cref="NewAssemblyName"/> and 
   /// <see cref="NewTypeName"/> are mandatory. In all other cases, the type and assembly will be
   /// loaded automatically.
   /// </remarks>
   [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
   public class RedirectTypeAttribute : Attribute
   {
      /// <summary>
      /// Initializes a new object from scratch.
      /// </summary>
      /// <param name="originalAssemblyName">Original assembly location..</param>
      /// <param name="originalTypeName">Original type name.</param>
      public RedirectTypeAttribute(string originalAssemblyName, string originalTypeName)
      {
         // Store values
         OriginalAssemblyName = originalAssemblyName;
         OriginalTypeName = originalTypeName;
      }

      #region Properties

      /// <summary>
      /// Gets or sets the original assembly name.
      /// </summary>
      public string OriginalAssemblyName { get; private set; }

      /// <summary>
      /// Gets or sets the new assembly name.
      /// </summary>
      /// <remarks>
      /// Leave empty if the assembly name is unchanged.
      /// </remarks>
      public string NewAssemblyName { get; set; }

      /// <summary>
      /// Gets or sets the original type name.
      /// </summary>
      /// <remarks>
      /// List or Array types should be postfixed with a [[]].
      /// </remarks>
      public string OriginalTypeName { get; private set; }

      /// <summary>
      /// Gets or sets the new type name.
      /// </summary>
      /// <remarks>
      /// Leave empty if the type name is unchanged.
      /// 
      /// List or Array types should be postfixed with a [[]].
      /// </remarks>
      public string NewTypeName { get; set; }

      /// <summary>
      /// Gets the original type.
      /// </summary>
      public string OriginalType
      {
         get { return TypeHelper.FormatType(OriginalAssemblyName, OriginalTypeName); }
      }

      /// <summary>
      /// Gets the new type that should be loaded.
      /// </summary>
      public string TypeToLoad
      {
         get
         {
            // Determine values to load
            string assemblyToLoad = string.IsNullOrEmpty(NewAssemblyName) ? OriginalAssemblyName : NewAssemblyName;
            string typeToLoad = string.IsNullOrEmpty(NewTypeName) ? OriginalTypeName : NewTypeName;

            // Return type to load
            return TypeHelper.FormatType(assemblyToLoad, typeToLoad);
         }
      }

      #endregion

   }
}
