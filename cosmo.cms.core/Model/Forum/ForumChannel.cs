using Cosmo.Security.Auth;
using System;

namespace Cosmo.Cms.Model.Forum
{

   /// <summary>
   /// Implementa una clase para gestionar un canal de los foros.
   /// </summary>
   public class ForumChannel
   {
      // Internal data declarations
      private int _id;
      private string _name;
      private string _desc;
      private DateTime _date;
      private bool _enabled;
      private string _owner;
      private int _msgCount;

      /// <summary>
      /// Gets a new instance of <see cref="Cosmo.Cms.Forums.CSForum"/>.
      /// </summary>
      public ForumChannel()
      {
         _id = 0;
         _name = string.Empty;
         _desc = string.Empty;
         _date = DateTime.Now;
         _enabled = false;
         _owner = SecurityService.ACCOUNT_SUPER;
         _msgCount = 0;
      }

      #region Settings

      /// <summary>
      /// Gets or sets el identificador único del objeto.
      /// </summary>
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Gets or sets el nombre del objeto.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Gets or sets la descripción del objeto.
      /// </summary>
      public string Description
      {
         get { return _desc; }
         set { _desc = value; }
      }

      /// <summary>
      /// Gets or sets la fecha de creación del objeto.
      /// </summary>
      public DateTime Date
      {
         get { return _date; }
         set { _date = value; }
      }

      /// <summary>
      /// Indica si el objeto se encuentra publicado.
      /// </summary>
      public bool Published
      {
         get { return _enabled; }
         set { _enabled = value; }
      }

      /// <summary>
      /// Gets or sets el login del usuario creador del objeto.
      /// </summary>
      public string Owner
      {
         get { return _owner; }
         set { _owner = value; }
      }

      /// <summary>
      /// Devuelve el número de mensajes que contiene.
      /// </summary>
      public int MessageCount
      {
         get { return _msgCount; }
         internal set { _msgCount = value; }
      }

      #endregion

   }
}
