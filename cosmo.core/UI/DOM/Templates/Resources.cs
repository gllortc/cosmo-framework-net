using Cosmo.Utils.IO;
using System.Drawing;

namespace Cosmo
{

   #region Class PictureResource

   /// <summary>
   /// Implementa un recurso de tipo imagen.
   /// </summary>
   public class PictureResource
   {
      private string _filename;
      private int _width = 0;
      private int _height = 0;

      /// <summary>
      /// Devuelve una instáncia de PictureResource.
      /// </summary>
      /// <param name="filename">Nombre del archivo (sin ruta ni path).</param>
      public PictureResource(string filename)
      {
         _filename = filename;
      }

      /// <summary>
      /// Devuelve una instáncia de PictureResource.
      /// </summary>
      /// <param name="filename">Nombre del archivo (sin ruta ni path).</param>
      /// <param name="width">Ancho (en píxels) del recurso.</param>
      /// <param name="height">Altura (en píxels) del recurso.</param>
      public PictureResource(string filename, int width, int height)
      {
         _filename = filename;
         _width = width;
         _height = height;
      }

      /// <summary>
      /// Devuelve la URL relativa a la raíz del site correspondiente al recurso.
      /// </summary>
      public string RelativeUrl
      {
         get { return "templates/shared/" + _filename; }
      }

      /// <summary>
      /// Devuelve el nombre del archivo.
      /// </summary>
      public string Filename
      {
         get { return _filename; }
      }

      /// <summary>
      /// Devuelve una instancia de Image correspondiente al recurso solicitado.
      /// </summary>
      /// <param name="workspacePath">Ruta física del workspace.</param>
      public Image GetImage(string workspacePath)
      {
         return Image.FromFile(PathInfo.Combine(workspacePath, "templates", "shared", _filename));
      }

      /// <summary>
      /// Devuelve el código XHTML para mostrar el recurso en una página web.
      /// </summary>
      /// <param name="alternateText">Texto alternativo.</param>
      /// <param name="useTitle">Indica si se debe mostrar el texto alternativo como título del recurso.</param>
      /// <returns>Una cadena en formato XHTML.</returns>
      public string ToXhtml(string alternateText, bool useTitle)
      {
         return "<img src=\"templates/shared/" + _filename + "\" " + 
                      (_width > 0 ? " width=\"" + _width + "\" " : "") +
                      (_height > 0 ? " height=\"" + _height + "\" " : "") +
                      (alternateText.Length > 0 ? " alt=\"" + alternateText + "\" " : "") +
                      (useTitle ? " title=\"" + alternateText + "\" " : "") + " />";
      }

      /// <summary>
      /// Devuelve el código XHTML para mostrar el recurso en una página web.
      /// </summary>
      /// <param name="alternateText">Texto alternativo.</param>
      /// <returns>Una cadena en formato XHTML.</returns>
      public string ToXhtml(string alternateText)
      {
         return ToXhtml(alternateText, false);
      }
   }

   #endregion

   /// <summary>
   /// Contiene todos los recursos compartidos del workspace.
   /// </summary>
   public class Resources
   {

      #region MessageBoxIcons

      /// <summary>
      /// Contiene las referencias a recursos destinados a mensajes.
      /// </summary>
      public static class MessageBoxIcons
      {
         /// <summary>
         /// Icono de información.
         /// </summary>
         public static PictureResource Information
         {
            get { return new PictureResource("msgbox_info.png", 32, 32); }
         }

         /// <summary>
         /// Icono de aviso.
         /// </summary>
         public static PictureResource Warning
         {
            get { return new PictureResource("msgbox_warning.png", 32, 32); }
         }

         /// <summary>
         /// Icono de error.
         /// </summary>
         public static PictureResource Error
         {
            get { return new PictureResource("msgbox_error.png", 32, 32); }
         }
      }

      #endregion

      #region Buttons

      /// <summary>
      /// Contiene las referencias a recursos destinados a botones de comando.
      /// </summary>
      public static class Buttons
      {
         /// <summary>
         /// Botón Aceptar.
         /// </summary>
         public static PictureResource Accept
         {
            get { return new PictureResource("cmd_accept.gif", 66, 20); }
         }

         /// <summary>
         /// Botón Cancelar.
         /// </summary>
         public static PictureResource Cancel
         {
            get { return new PictureResource("cmd_cancel.gif", 66, 20); }
         }

         /// <summary>
         /// Botón Eliminar.
         /// </summary>
         public static PictureResource Delete
         {
            get { return new PictureResource("cmd_delete.gif", 66, 20); }
         }

         /// <summary>
         /// Botón Descargar.
         /// </summary>
         public static PictureResource Download
         {
            get { return new PictureResource("cmd_download.gif", 66, 20); }
         }

         /// <summary>
         /// Botón Ir.
         /// </summary>
         public static PictureResource Go
         {
            get { return new PictureResource("cmd_go.gif", 66, 20); }
         }

         /// <summary>
         /// Botón Menú.
         /// </summary>
         public static PictureResource Menu
         {
            get { return new PictureResource("cmd_menu.gif", 66, 20); }
         }

         /// <summary>
         /// Botón Imprimir.
         /// </summary>
         public static PictureResource Print
         {
            get { return new PictureResource("cmd_print.gif", 66, 20); }
         }

         /// <summary>
         /// Botón Reset.
         /// </summary>
         public static PictureResource Reset
         {
            get { return new PictureResource("cmd_reset.gif", 66, 20); }
         }

         /// <summary>
         /// Botón Volver.
         /// </summary>
         public static PictureResource Return
         {
            get { return new PictureResource("cmd_return.gif", 66, 20); }
         }

         /// <summary>
         /// Botón Enviar.
         /// </summary>
         public static PictureResource Send
         {
            get { return new PictureResource("cmd_send.gif", 66, 20); }
         }

         /// <summary>
         /// Botón Ver.
         /// </summary>
         public static PictureResource View
         {
            get { return new PictureResource("cmd_View.gif", 66, 20); }
         }
      }

      #endregion

      #region ToolButtons

      /// <summary>
      /// Contiene las referencias a recursos destinados a botones de herramientas (imágenes de 16x16).
      /// </summary>
      public static class ToolButtons
      {
         /// <summary>
         /// Imagen Agregar.
         /// </summary>
         public static PictureResource Add
         {
            get { return new PictureResource("ico_add.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen Eliminar.
         /// </summary>
         public static PictureResource Delete
         {
            get { return new PictureResource("ico_del.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen Documento.
         /// </summary>
         public static PictureResource Document
         {
            get { return new PictureResource("ico_doc.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Editar.
         /// </summary>
         public static PictureResource Edit
         {
            get { return new PictureResource("ico_edit.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen Carpeta.
         /// </summary>
         public static PictureResource Folder
         {
            get { return new PictureResource("ico_folder.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Ayuda.
         /// </summary>
         public static PictureResource Help
         {
            get { return new PictureResource("ico_help.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Home.
         /// </summary>
         public static PictureResource Home
         {
            get { return new PictureResource("ico_home.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Destacado.
         /// </summary>
         public static PictureResource Highlight
         {
            get { return new PictureResource("ico_light.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Mensaje.
         /// </summary>
         public static PictureResource Message
         {
            get { return new PictureResource("ico_mail.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Mensaje leído.
         /// </summary>
         public static PictureResource MessageReaded
         {
            get { return new PictureResource("ico_mailread.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Mensaje no leído.
         /// </summary>
         public static PictureResource MessageNew
         {
            get { return new PictureResource("ico_mailnew.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Nota.
         /// </summary>
         public static PictureResource Note
         {
            get { return new PictureResource("ico_note.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Refrescar.
         /// </summary>
         public static PictureResource Refresh
         {
            get { return new PictureResource("ico_refresh.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen Seguridad.
         /// </summary>
         public static PictureResource Security
         {
            get { return new PictureResource("ico_security.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Basura.
         /// </summary>
         public static PictureResource Trash
         {
            get { return new PictureResource("ico_trash.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen Agregar usuario.
         /// </summary>
         public static PictureResource UserAdd
         {
            get { return new PictureResource("ico_usradd.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen Editar usuario.
         /// </summary>
         public static PictureResource UserEdit
         {
            get { return new PictureResource("ico_usrdata.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen Eliminar usuario.
         /// </summary>
         public static PictureResource UserDelete
         {
            get { return new PictureResource("ico_usrdel.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen Información de usuario.
         /// </summary>
         public static PictureResource UserInfo
         {
            get { return new PictureResource("ico_usrinfo.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen Mensaje privado entre usuarios.
         /// </summary>
         public static PictureResource UserMessage
         {
            get { return new PictureResource("ico_usrmsg.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen Buscar usuario.
         /// </summary>
         public static PictureResource UserSearch
         {
            get { return new PictureResource("ico_usrsch.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen ver.
         /// </summary>
         public static PictureResource View
         {
            get { return new PictureResource("ico_view.gif", 16, 16); }
         }

         /// <summary>
         /// Imagen TAG.
         /// </summary>
         public static PictureResource Tag
         {
            get { return new PictureResource("ico_tag.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Información.
         /// </summary>
         public static PictureResource Information
         {
            get { return new PictureResource("ico_info.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Error.
         /// </summary>
         public static PictureResource Error
         {
            get { return new PictureResource("ico_error.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Alerta.
         /// </summary>
         public static PictureResource Warning
         {
            get { return new PictureResource("ico_wrng.png", 16, 16); }
         }

         /// <summary>
         /// Imagen RSS.
         /// </summary>
         public static PictureResource Rss
         {
            get { return new PictureResource("ico_rss.png", 16, 16); }
         }

         /// <summary>
         /// Imagen Legal/Creative Commons.
         /// </summary>
         public static PictureResource LegalCreativeCommons
         {
            get { return new PictureResource("ico_legal-cc.png", 16, 16); }
         }
      }

      #endregion

      #region BulletIcons

      /// <summary>
      /// Contiene las referencias a iconos para decorar listas o enlaces (bullets)
      /// </summary>
      public static class BulletIcons
      {
         /// <summary>
         /// Imagen Agregar.
         /// </summary>
         public static PictureResource Add
         {
            get { return new PictureResource("mini_ico_add.png", 10, 10); }
         }

         /// <summary>
         /// Imagen Eliminar.
         /// </summary>
         public static PictureResource Delete
         {
            get { return new PictureResource("mini_ico_del.png", 10, 10); }
         }

         /// <summary>
         /// Imagen Editar.
         /// </summary>
         public static PictureResource Edit
         {
            get { return new PictureResource("mini_ico_edit.png", 10, 10); }
         }

         /// <summary>
         /// Imagen enlace externo.
         /// </summary>
         public static PictureResource ExternalLink
         {
            get { return new PictureResource("mini_ico_extlink.png", 10, 10); }
         }

         /// <summary>
         /// Imagen Correo/Mensaje.
         /// </summary>
         public static PictureResource Mail
         {
            get { return new PictureResource("mini_ico_mail.png", 10, 10); }
         }

         /// <summary>
         /// Imagen Nuevo.
         /// </summary>
         public static PictureResource New
         {
            get { return new PictureResource("mini_ico_new.png", 10, 10); }
         }

         /// <summary>
         /// Imagen canal RSS.
         /// </summary>
         public static PictureResource Rss
         {
            get { return new PictureResource("mini_ico_rss.png", 10, 10); }
         }

         /// <summary>
         /// Imagen Seguridad.
         /// </summary>
         public static PictureResource Security
         {
            get { return new PictureResource("mini_ico_security.gif", 10, 10); }
         }

         /// <summary>
         /// Imagen TAG / Keyword.
         /// </summary>
         public static PictureResource Tag
         {
            get { return new PictureResource("mini_ico_tag.png", 10, 10); }
         }

         /// <summary>
         /// Imagen Usuario.
         /// </summary>
         public static PictureResource User
         {
            get { return new PictureResource("mini_ico_usr.gif", 10, 10); }
         }
      }

      #endregion

      #region TreeViewFolders

      /// <summary>
      /// Contiene los recursos destinados a generar listas gerárquicas (ListView)
      /// </summary>
      public class TreeViewFolders
      {
         /// <summary>
         /// Carpeta nivel 0.
         /// </summary>
         public static PictureResource FolderLevel0
         {
            get { return new PictureResource("tree_folder_0.gif"); }
         }

         /// <summary>
         /// Carpeta nivel 1.
         /// </summary>
         public static PictureResource FolderLevel1
         {
            get { return new PictureResource("tree_folder_1.gif"); }
         }

         /// <summary>
         /// Carpeta nivel 2.
         /// </summary>
         public static PictureResource FolderLevel2
         {
            get { return new PictureResource("tree_folder_2.gif"); }
         }

         /// <summary>
         /// Carpeta nivel 3.
         /// </summary>
         public static PictureResource FolderLevel3
         {
            get { return new PictureResource("tree_folder_3.gif"); }
         }

         /// <summary>
         /// Carpeta nivel 4.
         /// </summary>
         public static PictureResource FolderLevel4
         {
            get { return new PictureResource("tree_folder_4.gif"); }
         }

         /// <summary>
         /// Carpeta nivel 5.
         /// </summary>
         public static PictureResource FolderLevel5
         {
            get { return new PictureResource("tree_folder_5.gif"); }
         }
      }

      #endregion

   }
}
