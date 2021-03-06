﻿using Cosmo.Security.Auth;
using System;

namespace Cosmo.Cms.Forums.Model.Obsolete
{
   /// <summary>
   /// Implementa una clase para gestionar un canal de los foros.
   /// </summary>
   public class CSForum
   {
      private int _id;
      private string _name;
      private string _desc;
      private DateTime _date;
      private bool _enabled;
      private string _owner;
      private int _msgCount;

      /// <summary>
      /// Devuelve una instancia de <see cref="Cosmo.Cms.Forums.CSForum"/>.
      /// </summary>
      public CSForum()
      {
         _id = 0;
         _name = string.Empty;
         _desc = string.Empty;
         _date = DateTime.Now;
         _enabled = false;
         _owner = AuthenticationService.ACCOUNT_SUPER;
         _msgCount = 0;
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador único del objeto.
      /// </summary>
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Devuelve o establece el nombre del objeto.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Devuelve o establece la descripción del objeto.
      /// </summary>
      public string Description
      {
         get { return _desc; }
         set { _desc = value; }
      }

      /// <summary>
      /// Devuelve o establece la fecha de creación del objeto.
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
      /// Devuelve o establece el login del usuario creador del objeto.
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
