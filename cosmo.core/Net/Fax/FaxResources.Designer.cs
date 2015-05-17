namespace Cosmo.Net.Fax
{

   /// <summary>
   /// Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
   /// </summary>
   /// <remarks>
   /// Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder à l'aide d'un outil, tel que ResGen ou Visual Studio.
   /// Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen avec l'option /str ou régénérez votre projet VS.
   /// </remarks>
   [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
   [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
   [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
   internal class FaxResources
   {
      private static global::System.Resources.ResourceManager resourceMan;
      private static global::System.Globalization.CultureInfo resourceCulture;

      [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
      internal FaxResources() { }

      /// <summary>
      ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
      /// </summary>
      [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
      internal static global::System.Resources.ResourceManager ResourceManager
      {
         get
         {
            if (object.ReferenceEquals(resourceMan, null))
            {
               global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Tourreau.Gilles.FaxDotNet.FaxResources", typeof(FaxResources).Assembly);
               resourceMan = temp;
            }
            return resourceMan;
         }
      }

      /// <summary>
      ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
      ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
      /// </summary>
      [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
      internal static global::System.Globalization.CultureInfo Culture
      {
         get { return resourceCulture; }
         set { resourceCulture = value; }
      }

      /// <summary>
      ///   Recherche une chaîne localisée semblable à This job is not owned by this server..
      /// </summary>
      internal static string ExceptionBadOwnerServerForJob
      {
         get { return ResourceManager.GetString("ExceptionBadOwnerServerForJob", resourceCulture); }
      }

      /// <summary>
      ///   Recherche une chaîne localisée semblable à The fax server object has been disposed..
      /// </summary>
      internal static string ExceptionFaxServerDisposed
      {
         get { return ResourceManager.GetString("ExceptionFaxServerDisposed", resourceCulture); }
      }

      /// <summary>
      ///   Recherche une chaîne localisée semblable à Windows Fax Service is not installed on the current system..
      /// </summary>
      internal static string ExceptionFaxServiceNotInstalled
      {
         get { return ResourceManager.GetString("ExceptionFaxServiceNotInstalled", resourceCulture); }
      }

      /// <summary>
      ///   Recherche une chaîne localisée semblable à There is no recipient number.
      /// </summary>
      internal static string ExceptionNoRecipientNumber
      {
         get { return ResourceManager.GetString("ExceptionNoRecipientNumber", resourceCulture); }
      }

      /// <summary>
      ///   Recherche une chaîne localisée semblable à The _printer name &apos;{0}&apos; is incorrect.
      /// </summary>
      internal static string ExceptionPrinterIncorrect
      {
         get { return ResourceManager.GetString("ExceptionPrinterIncorrect", resourceCulture); }
      }

      /// <summary>
      ///   Recherche une chaîne localisée semblable à Unable to sending fax. Printing to device context has failed..
      /// </summary>
      internal static string ExceptionPrintingFail
      {
         get { return ResourceManager.GetString("ExceptionPrintingFail", resourceCulture); }
      }

      /// <summary>
      ///   Recherche une chaîne localisée semblable à Unable to connect fax server &apos;{0}&apos;..
      /// </summary>
      internal static string ExceptionUnableConnect
      {
         get { return ResourceManager.GetString("ExceptionUnableConnect", resourceCulture); }
      }

      /// <summary>
      ///   Recherche une chaîne localisée semblable à Unable to open the registry..
      /// </summary>
      internal static string ExceptionUnableOpenRegistryKey
      {
         get { return ResourceManager.GetString("ExceptionUnableOpenRegistryKey", resourceCulture); }
      }
   }

}
