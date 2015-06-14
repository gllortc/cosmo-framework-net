using Cosmo.Data.ORM;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System;

namespace Cosmo.WebApp.UserServices
{
   /// <summary>
   /// Formulario de alta para nuevos usuarios.
   /// </summary>
   /// <remarks>
   /// Author : Gerard Llort
   /// Version: 1.0.0
   /// Copyright (c) InforGEST
   /// </remarks>
   public class UserJoin : PageView
   {
      private FormControl form;

      public override void InitPage()
      {
         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = "Crear nueva cuenta de usuario";
         ActiveMenuId = string.Empty;

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = "Crear nueva cuenta de usuario";
         header.Icon = IconControl.ICON_USER;
         MainContent.Add(header);

         // Genera el formulario para objetos del tipo User
         OrmEngine orm = new OrmEngine();
         form = orm.CreateForm(this, "frmJoin", typeof(User), true);
         
         MainContent.Add(form);
      }

      public override void FormDataReceived(FormControl receivedForm)
      {
         OrmEngine orm = new OrmEngine();

         try
         {
            // Rellena una instancia con los datos
            User user = new User();
            if (orm.ProcessForm(user, form, Parameters))
            {
               // Genera la nueva cuenta de usuario
               Workspace.AuthenticationService.Create(user);

               // Redirige al usuario a su página persnal
               Redirect("Home");
            }
         }
         catch (Exception ex)
         {
            ShowError(ex);
         }
      }

      /// <summary>
      /// Método invocado antes de renderizar todo forumario (excepto cuando se reciben datos invalidos).
      /// </summary>
      /// <param name="formDomID">Identificador (DOM) del formulario a renderizar.</param>
      public override void FormDataLoad(string formDomID)
      {
         // Nothing todo
      }

      public override void LoadPage()
      {
         // ProcessData();
      }
   }
}
