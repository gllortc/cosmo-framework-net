using Cosmo.Diagnostics;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace Cosmo.Data
{
   /// <summary>
   /// <see cref="SerializationBinder"/> class that supports backwards compatible serialization.
   /// </summary>
   internal sealed class RedirectDeserializationBinder : SerializationBinder
   {
      private readonly Dictionary<string, RedirectTypeAttribute> _redirectAttributes = new Dictionary<string, RedirectTypeAttribute>();
      private static List<string> _msPublicKeyTokens;

      /// <summary>
      /// Creates a custom binder that redirects all the types to new types if required. All properties
      /// decorated with the <see cref="RedirectTypeAttribute"/> will be redirected.
      /// </summary>
      /// <remarks>
      /// This constructor searches for attributes of the current application domain.
      /// </remarks>
      public RedirectDeserializationBinder() : this(AppDomain.CurrentDomain) { }

      /// <summary>
      /// Creates a custom binder that redirects all the types to new types if required. All properties
      /// decorated with the <see cref="RedirectTypeAttribute"/> will be redirected.
      /// </summary>
      /// <param name="appDomain"><see cref="AppDomain"/> to search in.</param>
      /// <remarks>
      /// This constructor searches for attributes in a specific application domain.
      /// </remarks>
      public RedirectDeserializationBinder(AppDomain appDomain)
      {
         // Declare variables
         List<string> assemblies = new List<string>();

         // Loop all assemblies in the application domain
         foreach (Assembly assembly in appDomain.GetAssemblies())
         {
            // Skip Microsoft assemblies
            string company = AssemblyInfo.Company(assembly.Location);
            if (!company.Contains("Microsoft"))
            {
               assemblies.Add(assembly.Location);
            }
         }

         // Initialize
         Initialize(assemblies.ToArray());
      }

      /// <summary>
      /// Creates a custom binder that redirects all the types to new types if required. All properties
      /// decorated with the <see cref="RedirectTypeAttribute"/> will be redirected.
      /// </summary>
      /// <param name="assemblies">Array of assembly locations that should be searched.</param>
      /// <remarks>
      /// This constructor searches for attributes in specific assemblies.
      /// </remarks>
      public RedirectDeserializationBinder(IEnumerable<string> assemblies)
      {
         // Initialize
         Initialize(assemblies);
      }

      #region Properties

      /// <summary>
      /// Gets the Microsoft public key tokens.
      /// </summary>
      /// <value>The Microsoft public key tokens.</value>
      private static IEnumerable<string> MicrosoftPublicKeyTokens
      {
         get
         {
            // Check instance
            if (_msPublicKeyTokens == null)
            {
               // Create object
               _msPublicKeyTokens = new List<string>();

               // Create all known public key tokens
               _msPublicKeyTokens.Add("b77a5c561934e089");
               _msPublicKeyTokens.Add("b03f5f7f11d50a3a");
               _msPublicKeyTokens.Add("31bf3856ad364e35");
            }

            // Return list
            return _msPublicKeyTokens;
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Initializes the binder by searching for all <see cref="RedirectTypeAttribute"/> in the
      /// assemblies passed to this method.
      /// </summary>
      /// <param name="assemblies">Array of assembly locations that should be searched.</param>
      private void Initialize(IEnumerable<string> assemblies)
      {
         // Get attribute type
         Type attributeType = typeof(RedirectTypeAttribute);

         // Loop all assemblies
         foreach (string assemblyLocation in assemblies)
         {
            // Make sure the assembly location is valid
            if (File.Exists(assemblyLocation))
            {
               // Load assembly
               Assembly assembly = Assembly.LoadFile(assemblyLocation);
               if (assembly != null)
               {
                  // Get all types
                  foreach (Type type in assembly.GetTypes())
                  {
                     // Get attributes defined to the type directly
                     InitializeAttributes(type, (RedirectTypeAttribute[])type.GetCustomAttributes(attributeType, true));

                     // Loop all members
                     foreach (MemberInfo member in type.GetMembers())
                     {
                        // Get member attributes
                        InitializeAttributes(member, (RedirectTypeAttribute[])member.GetCustomAttributes(attributeType, true));
                     }
                  }
               }
            }
         }
      }

      /// <summary>
      /// Initializes the binder by searching for all <see cref="RedirectTypeAttribute"/> in the
      /// attributes passed to this method.
      /// </summary>
      /// <param name="decoratedObject">Object that was decorated with the attribute.</param>
      /// <param name="attributes">Array of attributes to search for.</param>
      private void InitializeAttributes(object decoratedObject, RedirectTypeAttribute[] attributes)
      {
         // Declare variables
         Type type = null;

         // Return if array is empty
         if (attributes.Length == 0) return;

         // Check object
         if (decoratedObject is Type)
         {
            type = decoratedObject as Type;
         }

         // Loop all attributes
         foreach (RedirectTypeAttribute attribute in attributes)
         {
            // Check if we should automatically improve the attribute
            if (type != null)
            {
               // Format type
               string typeName = TypeHelper.FormatType(type.Assembly.FullName, type.FullName);
               typeName = ConvertTypeToVersionIndependentType(typeName);
               string finalTypeName, finalAssemblyName;
               SplitType(typeName, out finalAssemblyName, out finalTypeName);

               // Yes, do it
               attribute.NewTypeName = finalTypeName;
               attribute.NewAssemblyName = finalAssemblyName;
            }

            // Check if the attribute does not already exist
            if (_redirectAttributes.ContainsKey(attribute.OriginalType))
            {
               // Trace
               Trace.TraceWarning("A redirect for type '{0}' is already added to '{1}'. The redirect to '{2}' will not be added.",
                                  attribute.OriginalType, _redirectAttributes[attribute.OriginalType].TypeToLoad, attribute.TypeToLoad);
            }
            else
            {
               // Store attribute
               _redirectAttributes.Add(attribute.OriginalType, attribute);
            }
         }
      }

      /// <summary>
      /// Binds an assembly and typename to a specific type.
      /// </summary>
      /// <param name="assemblyName">Original assembly name.</param>
      /// <param name="typeName">Original type name.</param>
      /// <returns><see cref="Type"/> that the serialization should actually use.</returns>
      public override Type BindToType(string assemblyName, string typeName)
      {
         // Get current type
         string currentType = TypeHelper.FormatType(assemblyName, typeName);
         string currentTypeVersionIndependent = ConvertTypeToVersionIndependentType(currentType);

         // Convert the type to a redirected type
         string newType = ConvertTypeToNewType(currentTypeVersionIndependent);

         // Load right type (first new type, than version independent type, and finally try current type)
         Type typeToDeserialize = LoadType(newType) ?? (LoadType(currentTypeVersionIndependent) ?? LoadType(currentType));

         // Return the type
         return typeToDeserialize;
      }

      /// <summary>
      /// Tries to load a type on a safe way.
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      /// <remarks>
      /// In case this method fails to load the type, a warning will be traced with additional information.
      /// </remarks>
      private static Type LoadType(string type)
      {
         // Declare variables
         Type loadedType = null;

         try
         {
            // Try to load type
            loadedType = Type.GetType(type);
         }
         catch (Exception ex)
         {
            // Trace
            Trace.TraceWarning("Failed to load type '{1}'.{0}{0}Additional details:{0}{2}", Environment.NewLine, type, ex.ToString());
         }

         // Return result
         return loadedType;
      }

      /// <summary>
      /// Splits the type into a type name and assembly name.
      /// </summary>
      /// <param name="type">Type to split.</param>
      /// <param name="assemblyName">Assemby name.</param>
      /// <param name="typeName">Type name.</param>
      private static void SplitType(string type, out string assemblyName, out string typeName)
      {
         // Find splitter
         int splitterPos = type.IndexOf(", ");

         // Split
         typeName = (splitterPos != -1) ? type.Substring(0, splitterPos).Trim() : type;
         assemblyName = (splitterPos != -1) ? type.Substring(splitterPos + 1).Trim() : type;
      }

      /// <summary>
      /// Converts a string representation of a type to a version independent type by removing the assembly version information.
      /// </summary>
      /// <param name="type">Type to convert.</param>
      /// <returns>String representing the type without version information.</returns>
      private static string ConvertTypeToVersionIndependentType(string type)
      {
         // Declare constants
         const string InnerTypesEnd = ",";

         // Declare variables
         string newType = type;
         string[] innerTypes = GetInnerTypes(newType);

         // Check if there is at least one inner type
         if (innerTypes.Length > 0)
         {
            // Remove inner types
            newType = newType.Replace(string.Format(CultureInfo.InvariantCulture, "[{0}]", TypeHelper.FormatInnerTypes(innerTypes)), string.Empty);

            // Convert inner types as well
            for (int i = 0; i < innerTypes.Length; i++)
            {
               innerTypes[i] = ConvertTypeToVersionIndependentType(innerTypes[i]);
            }
         }

         // Split
         string typeName, assemblyName;
         SplitType(newType, out assemblyName, out typeName);

         // Is this a Microsoft assembly?
         bool isMicrosoftAssembly = false;
         foreach (string t in MicrosoftPublicKeyTokens)
         {
            if (assemblyName.Contains(t))
            {
               isMicrosoftAssembly = true;
               break;
            }
         }

         // Remove version info from assembly (if not signed by Microsoft)
         if (!isMicrosoftAssembly)
         {
            int splitterPos = assemblyName.IndexOf(", ");
            if (splitterPos != -1) assemblyName = assemblyName.Substring(0, splitterPos);
         }

         // Format type
         newType = TypeHelper.FormatType(assemblyName, typeName);

         // If there is at least one inner type, add it again
         if (innerTypes.Length > 0)
         {
            // Get index
            int innerTypesIndex = newType.IndexOf(InnerTypesEnd);
            if (innerTypesIndex >= 0)
            {
               newType = newType.Insert(innerTypesIndex, string.Format(CultureInfo.InvariantCulture, "[{0}]", TypeHelper.FormatInnerTypes(innerTypes)));
            }
         }

         // Return new type
         return newType;
      }

      /// <summary>
      /// Converts a string representation of a type to a redirected type.
      /// </summary>
      /// <param name="type">Type to convert.</param>
      /// <returns>String representing the type that represents the redirected type.</returns>
      private string ConvertTypeToNewType(string type)
      {
         // Declare constants
         const string InnerTypesEnd = ",";

         // Declare variables
         string newType = type;
         string[] innerTypes = GetInnerTypes(newType);

         // Check if there is at least one inner type
         if (innerTypes.Length > 0)
         {
            // Remove inner types
            newType = newType.Replace(string.Format(CultureInfo.InvariantCulture, "[{0}]", TypeHelper.FormatInnerTypes(innerTypes)), string.Empty);

            // Convert inner types as well
            for (int i = 0; i < innerTypes.Length; i++)
            {
               innerTypes[i] = ConvertTypeToNewType(innerTypes[i]);
            }
         }

         // Get redirect
         if (_redirectAttributes.ContainsKey(newType))
         {
            newType = _redirectAttributes[newType].TypeToLoad;
         }

         // If there is at least one inner type, add it again
         if (innerTypes.Length > 0)
         {
            // Get index
            int innerTypesIndex = newType.IndexOf(InnerTypesEnd);
            if (innerTypesIndex >= 0)
            {
               newType = newType.Insert(innerTypesIndex, string.Format(CultureInfo.InvariantCulture, "[{0}]", TypeHelper.FormatInnerTypes(innerTypes)));
            }
         }

         // Return new type
         return newType;
      }

      /// <summary>
      /// Returns the inner type of a type, for example, a generic array type.
      /// </summary>
      /// <param name="type">Full type which might contain an inner type.</param>
      /// <returns>Array of inner types.</returns>
      private static string[] GetInnerTypes(string type)
      {
         // Declare constants
         const char InnerTypeCountStart = '`';
         char[] InnerTypeCountEnd = new[] { '[', '+' };
         const char InternalTypeStart = '+';
         const char InternalTypeEnd = '[';
         const string AllTypesStart = "[[";
         const char SingleTypeStart = '[';
         const char SingleTypeEnd = ']';

         // Declare variables
         List<string> innerTypes = new List<string>();

         try
         {
            // Get count
            int countIndex = type.IndexOf(InnerTypeCountStart);
            if (countIndex == -1) return innerTypes.ToArray();

            // This is a generic, but does the type definition also contain the inner types?
            if (!type.Contains(AllTypesStart)) return innerTypes.ToArray();

            // Get the number of inner types
            int innerTypeCountEnd = -1;
            foreach (char t in InnerTypeCountEnd)
            {
               int index = type.IndexOf(t);
               if ((index != -1) && ((innerTypeCountEnd == -1) || (index < innerTypeCountEnd)))
               {
                  // This value is more likely to be the one
                  innerTypeCountEnd = index;
               }
            }
            int innerTypeCount = int.Parse(type.Substring(countIndex + 1, innerTypeCountEnd - countIndex - 1));

            // Remove all info until the first inner type
            if (!type.Contains(InternalTypeStart.ToString()))
            {
               // Just remove the info
               type = type.Substring(innerTypeCountEnd + 1);
            }
            else
            {
               // Remove the index, but not the numbers
               int internalTypeEnd = type.IndexOf(InternalTypeEnd);
               type = type.Substring(internalTypeEnd + 1);
            }

            // Get all the inner types
            for (int i = 0; i < innerTypeCount; i++)
            {
               // Get the start & end of this inner type
               int innerTypeStart = type.IndexOf(SingleTypeStart);
               int innerTypeEnd = innerTypeStart + 1;
               int openings = 1;

               // Loop until we find the end
               while (openings > 0)
               {
                  // Is this the closing one?
                  if (type[innerTypeEnd] == SingleTypeStart)
                  {
                     // Increase openings
                     openings++;
                  }
                  else if (type[innerTypeEnd] == SingleTypeEnd)
                  {
                     // Decrease openings
                     openings--;
                  }

                  // Increase current pos if we still have openings left
                  if (openings > 0) innerTypeEnd++;
               }

               // Get inner type
               innerTypes.Add(type.Substring(innerTypeStart + 1, innerTypeEnd - innerTypeStart - 1));

               // Remove this inner type
               type = type.Substring(innerTypeEnd + 1);
            }
         }
         catch (Exception ex)
         {
            // Trace
            ExceptionHelper.TraceExceptionAsWarning(ex, "damn!");
         }

         // Return result
         return innerTypes.ToArray();
      }

      #endregion

   }
}
