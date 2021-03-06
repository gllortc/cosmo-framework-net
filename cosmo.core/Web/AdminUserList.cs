﻿using Cosmo.Net;
using Cosmo.Security;
using Cosmo.UI;
using Cosmo.UI.Controls;
using System.Collections.Generic;
using System.Reflection;

namespace Cosmo.Web
{
   /// <summary>
   /// Implements a page that allows admins to manage user accounts.
   /// </summary>
   [AuthorizationRequired(Cosmo.Workspace.ROLE_ADMINISTRATOR)]
   public class AdminUserList : PageView
   {

      #region PageView Implementation

      public override void InitPage()
      {
         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         Title = "Administrar Usuarios";
         ActiveMenuId = "adminUsers";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         //-------------------------------
         // Configuración de la vista
         //-------------------------------

         // Header
         PageHeaderControl header = new PageHeaderControl(this);
         header.Title = "Administrar Usuarios";
         header.Icon = IconControl.ICON_USER;
         MainContent.Add(header);

         // Find users form
         FormControl findForm = new FormControl(this, "frmFindUsers");
         findForm.UsePanel = false;
         findForm.Content.Add(new FormFieldText(this, "txtFind", "Buscar", FormFieldText.FieldDataType.Text));
         findForm.FormButtons.Add(new ButtonControl(this, "cmdFind", "Buscar", ButtonControl.ButtonTypes.Submit));

         TabItemControl tabFindForm = new TabItemControl(this);
         tabFindForm.DomID = "tabFindForm";
         tabFindForm.Caption = "Buscar usuario(s)";
         tabFindForm.Icon = IconControl.ICON_USER;
         tabFindForm.Content.Add(findForm);

         TabItemControl tabSearchesForm = new TabItemControl(this);
         tabSearchesForm.DomID = "tabSearchesForm";
         tabSearchesForm.Caption = "Búsquedas habituales";
         tabSearchesForm.Icon = IconControl.ICON_USER;
         // tabSearchesForm.Content.Add(findForm);

         // Coloca los formularios en pestañas
         TabbedContainerControl tabs = new TabbedContainerControl(this);
         tabs.TabItems.Add(tabFindForm);
         tabs.TabItems.Add(tabSearchesForm);
         MainContent.Add(tabs);
      }

      public override void FormDataReceived(UI.Controls.FormControl receivedForm)
      {
         string icon = string.Empty;
         string toFindText = receivedForm.GetStringFieldValue("txtFind");

         List<User> users = Workspace.SecurityService.Find(toFindText, string.Empty, 0, false);
         if (users != null && users.Count > 0)
         {
            TableRow row;

            TableControl table = new TableControl(this);
            table.Condensed = true;
            table.Striped = true;

            row = new TableRow(6);
            row.Cells[0] = new TableCell("Login");
            row.Cells[1] = new TableCell("Nombre");
            row.Cells[2] = new TableCell("Mail");
            row.Cells[3] = new TableCell("Alta");
            row.Cells[4] = new TableCell("Último acceso");
            row.Cells[5] = new TableCell("Accesos");
            table.Header = row;

            foreach (User user in users)
            {
               switch (user.Status)
               {
                  case User.UserStatus.NotVerified:
                     icon = IconControl.GetIcon(this, IconControl.ICON_QUESTION);
                     break;
                  case User.UserStatus.Disabled:
                     icon = IconControl.GetIcon(this, IconControl.ICON_PROHIBITED);
                     break;
                  case User.UserStatus.Enabled:
                     icon = IconControl.GetIcon(this, IconControl.ICON_USER);
                     break;
                  case User.UserStatus.SecurityBloqued:
                     icon = IconControl.GetIcon(this, IconControl.ICON_PROHIBITED_CIRCLE);
                     break;
               }

               row = new TableRow(6);
               row.Cells[0] = new TableCell(icon + " " + user.Login, AdminUserData.GetURL(user.ID));
               row.Cells[1] = new TableCell(user.GetDisplayName());
               row.Cells[2] = new TableCell(user.Mail);
               row.Cells[3] = new TableCell(user.Created.ToString(Cosmo.Utils.Calendar.FORMAT_SHORTDATE));
               row.Cells[4] = new TableCell(user.LastLogon.ToString(Cosmo.Utils.Calendar.FORMAT_SHORTDATE));
               row.Cells[5] = new TableCell(user.LogonCount);

               table.Rows.Add(row);
            }

            PanelControl resultsPanel = new PanelControl(this);
            resultsPanel.Text = "Resultados de la búsqueda";
            resultsPanel.Content.Add(table);

            MainContent.Add(resultsPanel);
         }
         else
         {
            CalloutControl alert = new CalloutControl(this);
            alert.Title = "Ninguna coincidencia";
            alert.Text = "El texto \"" + toFindText + "\" no ha devuelto ningun usuario.";
            MainContent.Add(alert);
         }
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL()
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         return url.ToString();
      }

      #endregion

   }
}
