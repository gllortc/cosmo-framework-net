using System;
using System.IO;
using System.Reflection;

namespace Cosmo.Utils
{
   /// <summary>
   /// Assembly info helper class.
   /// </summary>
   public class AssemblyInfo
   {
      /// <summary>
      /// Gets the title of the current assembly.
      /// </summary>
      /// <returns>Title.</returns>
      public static string Title() 
      { 
         return Title(null); 
      }

      /// <summary>
      /// Gets the title of a specific assembly.
      /// </summary>
      /// <param name="path">Path of the assembly.</param>
      /// <returns>Title.</returns>
      public static string Title(string path)
      {
         // Get assembly
         Assembly assembly = GetAssembly(path);

         // Get title
         string title = GetAssemblyAttributeValue(assembly, typeof(AssemblyTitleAttribute), "Title");

         // If it is not an empty string, return it
         if (!string.IsNullOrEmpty(title)) return title;

         // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
         return System.IO.Path.GetFileNameWithoutExtension(assembly.CodeBase);
      }

      /// <summary>
      /// Gets the version of the current assembly.
      /// </summary>
      /// <returns>Version.</returns>
      public static string Version() 
      { 
         return Version(3); 
      }

      /// <summary>
      /// Gets the version of the current assembly with a separator count.
      /// </summary>
      /// <param name="separatorCount">Number that determines how many version numbers should be returned.</param>
      /// <returns>Version.</returns>
      public static string Version(int separatorCount) 
      { 
         return Version(null, separatorCount); 
      }

      /// <summary>
      /// Gets the version of a specific assembly.
      /// </summary>
      /// <param name="path">Path of the assembly.</param>
      /// <returns>Version.</returns>
      public static string Version(string path) 
      { 
         return Version(path, 3); 
      }

      /// <summary>
      /// Gets the version of a specific assembly with a separator count.
      /// </summary>
      /// <param name="path">Path of the assembly.</param>
      /// <param name="separatorCount">Number that determines how many version numbers should be returned.</param>
      /// <returns>Version.</returns>
      public static string Version(string path, int separatorCount)
      {
         // Get assembly
         Assembly assembly = GetAssembly(path);

         // Always increase separator count (since we need 1 part extra)
         separatorCount++;

         // Get version
         string version = assembly.GetName().Version.ToString();

         // Split version
         string[] versionSplit = version.Split('.');

         // Make sure we have a maximum of separators
         version = versionSplit[0];
         for (int i = 1; i < separatorCount; i++)
         {
            // Check if we still have parts to handle
            if (i >= versionSplit.Length)
            {
               // Exit loop
               break;
            }

            // Add this part
            version += string.Format(".{0}", versionSplit[i]);
         }

         // Return version
         return version;
      }

      /// <summary>
      /// Gets the description of the current assembly.
      /// </summary>
      /// <returns>Description.</returns>
      public static string Description() 
      { 
         return Description(null); 
      }

      /// <summary>
      /// Gets the description of a specific assembly.
      /// </summary>
      /// <param name="path">Path of the assembly.</param>
      /// <returns>Description.</returns>
      public static string Description(string path)
      {
         return GetAssemblyAttributeValue(GetAssembly(path), typeof(AssemblyDescriptionAttribute), "Description");
      }

      /// <summary>
      /// Gets the product of the current assembly.
      /// </summary>
      /// <returns>Product.</returns>
      public static string Product() 
      { 
         return Product(null); 
      }

      /// <summary>
      /// Gets the product of a specific assembly.
      /// </summary>
      /// <param name="path">Path of the assembly.</param>
      /// <returns>Product.</returns>
      public static string Product(string path)
      {
         return GetAssemblyAttributeValue(GetAssembly(path), typeof(AssemblyProductAttribute), "Product");
      }

      /// <summary>
      /// Gets the copyright of the current assembly.
      /// </summary>
      /// <returns>Copyright.</returns>
      public static string Copyright() 
      { 
         return Copyright(null); 
      }

      /// <summary>
      /// Gets the copyright of a specific assembly.
      /// </summary>
      /// <param name="path">Path of the assembly.</param>
      /// <returns>Copyright.</returns>
      public static string Copyright(string path)
      {
         return GetAssemblyAttributeValue(GetAssembly(path), typeof(AssemblyCopyrightAttribute), "Copyright");
      }

      /// <summary>
      /// Gets the company of a specific assembly.
      /// </summary>
      /// <returns>Company.</returns>
      public static string Company() 
      { 
         return Company(null); 
      }

      /// <summary>
      /// Gets the company of a specific assembly
      /// </summary>
      /// <param name="path">Path of the assembly</param>
      /// <returns>Company</returns>
      public static string Company(string path)
      {
         return GetAssemblyAttributeValue(GetAssembly(path), typeof(AssemblyCompanyAttribute), "Company");
      }

      /// <summary>
      /// Gets a path of the current assembly.
      /// </summary>
      /// <returns>Path.</returns>
      public static string Path() 
      { 
         return Path(null); 
      }

      /// <summary>
      /// Gets the path of a specific assembly.
      /// </summary>
      /// <param name="path">Path of the assembly.</param>
      /// <returns>Path.</returns>
      public static string Path(string path)
      {
         // Get assembly
         Assembly assembly = GetAssembly(path);

         // Get location
         string location = assembly.Location;

         // Strip last part
         return location.Substring(0, location.LastIndexOf('\\'));
      }

      /// <summary>
      /// Loads a specific assembly. If the failing of a specific assembly fails, the entry assembly is returned.
      /// </summary>
      /// <param name="path">Path of the assembly to load or null if the entry assembly should be loaded.</param>
      /// <returns>Assembly.</returns>
      private static Assembly GetAssembly(string path)
      {
         // Declare variable
         Assembly result = null;

         // Do we have a valid path?
         if ((path == null) || (!File.Exists(path)))
         {
            result = Assembly.GetEntryAssembly();
         }
         else
         {
            // Try to load assembly
            try
            {
               result = Assembly.LoadFile(path);
            }
            catch (Exception)
            {
               result = null;
            }
         }

         // Check if there is a valid assembly
         if (result == null)
         {
            // No, load entry assembly first
            result = Assembly.GetEntryAssembly();

            // Check if there is a valid assembly
            if (result == null)
            {
               // No, load calling assembly
               result = Assembly.GetCallingAssembly();
            }
         }

         // Return result
         return result;
      }

      /// <summary>
      /// Gets the specific <see cref="Attribute"/> value of the attribute type in the specified assembly.
      /// </summary>
      /// <param name="assembly">Assembly to read the information from.</param>
      /// <param name="attribute">Attribute to read.</param>
      /// <param name="property">Property to read from the attribute.</param>
      /// <returns>Value of the attribute or empty if the attribute is not found.</returns>
      private static string GetAssemblyAttributeValue(Assembly assembly, Type attribute, string property)
      {
         // Get the specified attribute
         object[] attributes = assembly.GetCustomAttributes(attribute, false);

         // Return empty string if attribute is not found
         if (attributes.Length == 0) return string.Empty;

         // Get attribute value
         object attributeValue = attributes[0];
         if (attributeValue == null) return string.Empty;

         // Get the property
         Type attributeType = attributeValue.GetType();
         PropertyInfo propertyInfo = attributeType.GetProperty(property);
         if (propertyInfo == null) return string.Empty;

         // Get property value
         object propertyValue = propertyInfo.GetValue(attributeValue, null);
         if (propertyValue == null) return string.Empty;

         // Return value
         return propertyValue.ToString();
      }
   }
}
