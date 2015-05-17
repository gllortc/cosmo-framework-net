namespace Cosmo.Cms.Classified.Model
{

   /// <summary>
   /// Implementa un objeto AdFolder (carpeta contenedora de anuncios)
   /// </summary>
   public class CSAdsFolder
   {

      private int pID;
      private string pName;
      private string pDescription;
      private bool pEnabled;
      private bool pIsListDefault;
      private bool pIsNotSelectable;
      private int pObjects;

      /// <summary>
      /// Devuelve una instancia de CSAdsFolder
      /// </summary>
      public CSAdsFolder()
      {
         pID = 0;
         pName = "";
         pDescription = "";
         pEnabled = true;
         pIsListDefault = false;
         pIsNotSelectable = false;
         pObjects = 0;
      }

      #region Properties

      public int ID
      {
         get { return pID; }
         set { pID = value; }
      }

      public string Name
      {
         get { return pName; }
         set { pName = value; }
      }

      public string Description
      {
         get { return pDescription; }
         set { pDescription = value; }
      }

      public bool IsListDefault
      {
         get { return pIsListDefault; }
         set { pIsListDefault = value; }
      }

      public bool IsNotSelectable
      {
         get { return pIsNotSelectable; }
         set { pIsNotSelectable = value; }
      }

      public bool Enabled
      {
         get { return pEnabled; }
         set { pEnabled = value; }
      }

      public int Objects
      {
         get { return pObjects; }
         set { pObjects = value; }
      }

      #endregion

   }
}