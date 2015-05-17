using Cosmo.Communications.PrivateMessages;
using Cosmo.REST;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using System.Text;
using System.Web;

namespace Cosmo.UI.Render.Impl
{
   /// <summary>
   /// Implementa un módulo de renderizado para Bootstrap 3.
   /// </summary>
   public class BootstrapRenderModuleImpl : RenderModule
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="BootstrapRenderModuleImpl"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      public BootstrapRenderModuleImpl(Workspace workspace, Plugin plugin)
         : base(workspace, plugin)
      { }

      /// <summary>
      /// Convierte un color de control en una clase CSS.
      /// </summary>
      /// <param name="ctrlColor">Color para el que se desea obtener la clase CSS.</param>
      /// <returns>Una cadena que representa la clase CSS a aplicar al elemento.</returns>
      public override string GetCssClassFromControlColor(ComponentColorScheme ctrlColor)
      {
         switch (ctrlColor)
         {
            case ComponentColorScheme.Information: return "info";
            case ComponentColorScheme.Warning: return "warning";
            case ComponentColorScheme.Error: return "danger";
            case ComponentColorScheme.Success: return "success";
            case ComponentColorScheme.Primary: return "primary";
            default: return string.Empty;
         }
      }

      /// <summary>
      /// Convierte un color de fondo para controles en una clase CSS.
      /// </summary>
      /// <param name="bgColor">Color para el que se desea obtener la clase CSS.</param>
      /// <returns>Una cadena que representa la clase CSS a aplicar al elemento.</returns>
      public override string GetCssClassFromBackgroundColor(ComponentBackgroundColor bgColor)
      {
         switch (bgColor)
         {
            case ComponentBackgroundColor.Red: return "bg-red";
            case ComponentBackgroundColor.Yellow: return "bg-yellow";
            case ComponentBackgroundColor.Aqua: return "bg-aqua";
            case ComponentBackgroundColor.Blue: return "bg-blue";
            case ComponentBackgroundColor.LightBlue: return "bg-light-blue";
            case ComponentBackgroundColor.Green: return "bg-green";
            case ComponentBackgroundColor.Navy: return "bg-navy";
            case ComponentBackgroundColor.Teal: return "bg-teal";
            case ComponentBackgroundColor.Olive: return "bg-olive";
            case ComponentBackgroundColor.Lime: return "bg-lime";
            case ComponentBackgroundColor.Orange: return "bg-orange";
            case ComponentBackgroundColor.Fuchsia: return "bg-fuchsia";
            case ComponentBackgroundColor.Purple: return "bg-purple";
            case ComponentBackgroundColor.Maroon: return "bg-maroon";
            case ComponentBackgroundColor.Black: return "bg-black";
            case ComponentBackgroundColor.Gray: return "bg-gray";
            default: return string.Empty;
         }
      }

      /// <summary>
      /// Renderiza un control.
      /// </summary>
      /// <param name="control">Instancia del control a renderizar.</param>
      /// <param name="postback">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string Render(Controls.IControl control, bool postback)
      {
         if (control.GetType() == typeof(Alert))
         {
            return RenderAlert((Alert)control);
         }
         else if (control.GetType() == typeof(Icon))
         {
            return RenderIcon((Icon)control);
         }
         else if (control.GetType() == typeof(Breadcrumb))
         {
            return RenderBreadcrumb((Breadcrumb)control);
         }
         else if (control.GetType() == typeof(Button))
         {
            return RenderButton((Button)control);
         }
         else if (control.GetType() == typeof(Callout))
         {
            return RenderCallout((Callout)control);
         }
         else if (control.GetType() == typeof(Carrousel))
         {
            return RenderCarrousel((Carrousel)control);
         }
         else if (control.GetType() == typeof(Chat))
         {
            return RenderChat((Chat)control);
         }
         else if (control.GetType() == typeof(Jumbotron))
         {
            return RenderJumbotron((Jumbotron)control);
         }
         else if (control.GetType() == typeof(ListGroup))
         {
            return RenderListGroup((ListGroup)control);
         }
         else if (control.GetType() == typeof(MediaList))
         {
            return RenderMediaList((MediaList)control);
         }
         else if (control.GetType() == typeof(Navbar))
         {
            return RenderNavbar((Navbar)control);
         }
         else if (control.GetType() == typeof(Panel))
         {
            return RenderPanel((Panel)control);
         }
         else if (control.GetType() == typeof(Popover))
         {
            return RenderPopover((Popover)control);
         }
         else if (control.GetType() == typeof(Table))
         {
            return RenderTable((Table)control);
         }

         throw new ControlNotSuportedException("No se puede renderizar el control de tipo " + control.GetType().AssemblyQualifiedName);
      }

      /// <summary>
      /// Renderiza un control.
      /// </summary>
      /// <param name="control">Instancia del control a renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string Render(Controls.IControl control)
      {
         return Render(control, false);
      }

      /// <summary>
      /// Renderiza una página.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="PageViewContainer"/> que representa la instancia renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string RenderPage(PageViewContainer page)
      {
         return RenderPage(page, false);
      }

      /// <summary>
      /// Renderiza una página.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="PageViewContainer"/> que representa la instancia renderizar.</param>
      /// <param name="postback">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string RenderPage(PageViewContainer page, bool postback)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
         xhtml.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
         xhtml.AppendLine("<head>");
         xhtml.AppendLine("  <title>" + (string.IsNullOrEmpty(page.Title) ? Workspace.Name : Workspace.Name + " - " + page.Title) + "</title>");
         xhtml.AppendLine("  <meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />");
         xhtml.AppendLine("  <meta http-equiv=\"cache-control\" content=\"no-cache\" />");
         xhtml.AppendLine("  <meta http-equiv=\"expires\" content=\"3600\" />");
         xhtml.AppendLine("  <meta http-equiv=\"revisit-after\" content=\"2 days\" />");
         xhtml.AppendLine("  <meta http-equiv=\"robots\" content=\"index,follow\" />");
         xhtml.AppendLine("  <meta http-equiv=\"publisher\" content=\"" + Workspace.Name + "\" />");
         xhtml.AppendLine("  <meta http-equiv=\"copyright\" content=\"" + Workspace.Name + "\" />");
         xhtml.AppendLine("  <meta http-equiv=\"author\" content=\"" + Workspace.Name + "\" />");
         xhtml.AppendLine("  <meta http-equiv=\"generator\" content=\"" + Cosmo.Workspace.ProductName + " (" + Cosmo.Workspace.Version + ")" + "\" />");
         xhtml.AppendLine("  <meta http-equiv=\"distribution\" content=\"global\" />");
         xhtml.AppendLine("  <meta http-equiv=\"description\" content=\"" + (string.IsNullOrEmpty(page.Description) ? Workspace.PageDescription : page.Description) + "\" />");
         xhtml.AppendLine("  <meta http-equiv=\"keywords\" content=\"" + (string.IsNullOrEmpty(page.Keywords) ? Workspace.PageKeywords : page.Keywords) + "\" />");
         xhtml.AppendLine("  <meta http-equiv=\"device-type\" content=\"" + page.DeviceType.ToString() + "\" />");

         foreach (string css in this.CssResources)
         {
            xhtml.AppendLine("  <link href=\"" + css + "\" rel=\"stylesheet\" type=\"text/css\" />");
         }

         foreach (string js in this.JavascriptResources)
         {
            xhtml.AppendLine("  <script type=\"text/javascript\" src=\"" + js + "\"></script>");
         }

         xhtml.AppendLine("</head>");
         xhtml.AppendLine("<body class=\"" + (page.Layout.FadeBackground ? "bg-black" : "skin-blue fixed") + "\">");

         xhtml.AppendLine(Workspace.UIService.Render(page.Layout, postback));

         xhtml.AppendLine("</body>");
         xhtml.AppendLine("</html>");

         return xhtml.ToString();
      }

      #region Icon Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="Icon"/>.
      /// </summary>
      private string RenderIcon(Icon control)
      {
         if (control.Code.StartsWith("glyphicon"))
         {
            return "<i class=\"glyphicon " + control.Code + " " + GetCssClassFromBackgroundColor(control.BackgroundColor) + "\"></i>";
         }
         else if (control.Code.StartsWith("fa"))
         {
            return "<i class=\"fa " + control.Code + " " + GetCssClassFromBackgroundColor(control.BackgroundColor) + "\"></i>";
         }
         else
         {
            return "<i class=\"" + control.Code + " " + GetCssClassFromBackgroundColor(control.BackgroundColor) + "\"></i>";
         }
      }

      #endregion

      /// <summary>
      /// Renderiza un control de tipo <see cref="Alert"/>.
      /// </summary>
      private string RenderAlert(Alert alert)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + alert.GetIdParameter() + "class=\"alert alert-" + GetCssClassFromControlColor(alert.Type) + " " + (alert.Closeable ? string.Empty : "alert-dismissable") + "\">");
         if (alert.Closeable) xhtml.AppendLine("  <button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>");
         xhtml.AppendLine("  " + alert.Text);
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Breadcrumb"/>.
      /// </summary>
      private string RenderBreadcrumb(Breadcrumb breadcrumb)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<ol " + breadcrumb.GetIdParameter() + "class=\"breadcrumb\">");
         foreach (BreadcrumbItem item in breadcrumb.Items)
         {
            xhtml.AppendLine("  <li" + (item.IsActive ? " class=\"active\"" : string.Empty) + ">");
            if (!string.IsNullOrEmpty(item.Href)) xhtml.AppendLine("<a href=\"" + item.Href + "\">" + HttpUtility.HtmlDecode(item.Caption) + "</a>");
            xhtml.AppendLine("  </li>");
         }
         xhtml.AppendLine("</ol>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Button"/>.
      /// </summary>
      private string RenderButton(Button button)
      {
         StringBuilder xhtml = new StringBuilder();

         string color = string.Empty;
         color = GetCssClassFromControlColor(button.Color);
         color = string.IsNullOrWhiteSpace(color) ? "default" : color;

         xhtml.Append("<button type=\"button\" class=\"btn btn-" + color + "\"" + (string.IsNullOrWhiteSpace(button.JavaScriptAction) ? string.Empty : "onclick=\"javascript:" + button.JavaScriptAction + "\"") + ">");
         xhtml.Append((string.IsNullOrWhiteSpace(button.Icon) ? string.Empty : Icon.GetIcon(button.ParentViewport, button.Icon) + "&nbsp;&nbsp;") + HttpUtility.HtmlDecode(button.Caption));
         xhtml.Append("</button>\n");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Callout"/>.
      /// </summary>
      private string RenderCallout(Callout callout)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + callout.GetIdParameter() + "class=\"bs-callout bs-callout-" + GetCssClassFromControlColor(callout.Type) + "\">");
         xhtml.AppendLine("  <h4>" + HttpUtility.HtmlDecode(callout.Title) + "</h4>");
         xhtml.AppendLine("  <p>" + HttpUtility.HtmlDecode(callout.Text) + "</p>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Carrousel"/>.
      /// </summary>
      private string RenderCarrousel(Carrousel carrousel)
      {
         int count;
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("");

         // Obtiene la altura máxima del control
         int height = 338;
         foreach (CarrouselSlide slide in carrousel.Slides)
         {
            // TODO: Sólo funciona si se guardan las medidas en la BBDD
            if (slide.Height > height) height = slide.Height;
         }

         xhtml.Append("<div " + carrousel.GetIdParameter() + "class=\"carousel slide\" data-ride=\"carousel\">\n");
         xhtml.Append("  <ol class=\"carousel-indicators\">\n");

         count = 0;
         foreach (CarrouselSlide slide in carrousel.Slides)
         {
            xhtml.Append("    <li data-target=\"#" + carrousel.DomID + "\" data-slide-to=\"" + count + "\" class=\"" + (count == 0 ? "active" : "") + "\"></li>\n");
            count++;
         }

         xhtml.Append("  </ol>\n");
         xhtml.Append("  <div class=\"carousel-inner\">\n");

         count = 0;
         foreach (CarrouselSlide slide in carrousel.Slides)
         {
            xhtml.Append("    <div class=\"item " + (count == 0 ? "active" : "") + "\">\n");
            xhtml.Append("       <img alt=\"" + slide.Name.Replace("\"", "") + "\" src=\"" + slide.FileName + "\">\n");
            xhtml.Append("    </div>\n");
            count++;
         }

         xhtml.Append("  </div>\n");
         xhtml.Append("  <a class=\"left carousel-control\" href=\"#" + carrousel.DomID + "\" data-slide=\"prev\">\n");
         xhtml.Append("    <span class=\"glyphicon glyphicon-chevron-left\"></span>\n");
         xhtml.Append("  </a>\n");
         xhtml.Append("  <a class=\"right carousel-control\" href=\"#" + carrousel.DomID + "\" data-slide=\"next\">\n");
         xhtml.Append("    <span class=\"glyphicon glyphicon-chevron-right\"></span>\n");
         xhtml.Append("  </a>\n");
         xhtml.Append("</div>\n");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Chat"/>.
      /// </summary>
      private string RenderChat(Chat chat)
      {
         Panel panel = new Panel(null);
         StringBuilder script = new StringBuilder();
         StringBuilder xhtml = new StringBuilder();

         if (chat.AutoSize)
         {
            script.AppendLine("var chHeight = $(window).height();");
            script.AppendLine("chHeight = chHeight - 200;");
            // script.AppendLine("console.log('Posición: ' + $('#pmChatMsgs').position());");
         }
         else
         {
            script.AppendLine("var chHeight = " + chat.Height + ";");
         }

         script.AppendLine("$('#pmChatMsgs').slimScroll({");
         script.AppendLine("  height: chHeight + 'px',");
         script.AppendLine("  railVisible: true,");
         script.AppendLine("  start: 'bottom'");
         script.AppendLine("});");

         panel.DomID = chat.DomID;
         panel.Caption = chat.Caption;

         // Representa los mensajes
         xhtml.Clear();
         foreach (ChatMessage message in chat.Messages)
         {
            xhtml.AppendLine("<div id=\"" + message.DomID + "\" class=\"popover " + ConvertPopoverDirectionToString(message.Direction) + " chat-message-" + ConvertPopoverDirectionToString(message.Direction) + "\">");
            xhtml.AppendLine("  <div class=\"arrow\"></div>");
            xhtml.AppendLine("  <div class=\"chat-msg-toolbar clearfix\">");
            xhtml.AppendLine("    <div class=\"chat-msg-title\">" + HttpUtility.HtmlDecode(message.Caption) + "</div>");
            if (message.ToolbarButtons.Count > 0)
            {
               xhtml.AppendLine("    <div class=\"btn-group pull-right\">");
               foreach (ChatMessageToolbarButton button in message.ToolbarButtons)
               {
                  xhtml.AppendLine("      <a href=\"" + button.Href + "\" class=\"btn btn-default btn-xs\">" + (string.IsNullOrWhiteSpace(button.Icon) ? string.Empty : Icon.GetIcon(button.ParentViewport, button.Icon)) + " " + HttpUtility.HtmlDecode(button.Caption) + "</a>");
               }
               xhtml.AppendLine("    </div>");
            }
            xhtml.AppendLine("  </div>");
            xhtml.AppendLine("  <div class=\"popover-content\">");
            xhtml.AppendLine(HttpUtility.HtmlDecode(message.Content).Replace("\n", "<br />\n"));
            xhtml.AppendLine("  </div>");
            xhtml.AppendLine("</div>");
         }
         xhtml.AppendLine(chat.Messages.Count <= 0 ? "<h2><small>No hay mensajes entre vosotros</small></h2>" : string.Empty);
         panel.ContentXhtml = xhtml.ToString();
         panel.ContentDomId = "pmChatMsgs";

         // Representa el formulario de envio
         if (chat.FormShow)
         {
            xhtml.Clear();
            xhtml.AppendLine("    <form id=\"" + chat.FormDomID + "\" role=\"form\">");
            xhtml.AppendLine("      <input type=\"hidden\" name=\"" + Workspace.PARAM_COMMAND + "\" value=\"" + CommunicationsRestHandler.COMMAND_SEND + "\">");
            xhtml.AppendLine("      <input type=\"hidden\" name=\"" + Workspace.PARAM_USER_ID + "\" value=\"" + chat.FormToUserID + "\">");
            xhtml.AppendLine("      <div class=\"form-group\">");
            xhtml.AppendLine("        <textarea id=\"" + PrivateMessageDAO.PARAM_BODY + "\" name=\"" + PrivateMessageDAO.PARAM_BODY + "\" class=\"form-control\" rows=\"1\" placeholder=\"Escribe el texto a enviar...\"></textarea>");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("      <button id=\"" + chat.FormSubmitButtonID + "\" type=\"button\" class=\"btn btn-sm btn-primary\" data-loading-text=\"Enviando...\">" + Icon.GetIcon(chat.ParentViewport, Icon.ICON_SEND) + "&nbsp;&nbsp;Enviar</button>");
            xhtml.AppendLine("    </form>");
            panel.Footer.Add(new HtmlContent(chat.ParentViewport, xhtml.ToString()));

            script.AppendLine("cosmoCommServices.sendPMsg('" + chat.FormDomID + "','" + chat.FormSubmitButtonID + "'," + chat.FormToUserID + ");");
            script.AppendLine("$('#" + PrivateMessageDAO.PARAM_BODY + "').autosize({append: \"\\n\"});");
            script.AppendLine("$('#" + PrivateMessageDAO.PARAM_BODY + "').inputlimiter({");
            script.AppendLine("	remText: '%n character%s remaining...',");
            script.AppendLine("	limitText: 'max allowed : %n.'");
            script.AppendLine("});");
         }

         // Conjunta todo el código XHTML y el JavaScript
         xhtml.Clear();
         xhtml.AppendLine(Render(panel));
         xhtml.AppendLine("  <script type=\"text/javascript\">");
         xhtml.AppendLine("  $( document ).ready(function() {");
         xhtml.AppendLine(script.ToString());
         xhtml.AppendLine("  });");
         xhtml.AppendLine("  </script>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Jumbotron"/>.
      /// </summary>
      private string RenderJumbotron(Jumbotron jumbotron)
      {
         StringBuilder style = new StringBuilder();
         StringBuilder xhtml = new StringBuilder();

         if (!string.IsNullOrEmpty(jumbotron.BackgroundImage)) style.Append("background-image:url(" + jumbotron.BackgroundImage + ");");
         if (!string.IsNullOrEmpty(jumbotron.Color)) style.Append("color:" + jumbotron.Color + ";");

         xhtml.AppendLine("<div " + jumbotron.GetIdParameter() + "class=\"jumbotron\"" + (style.Length > 0 ? " style=\"" + style.ToString() + "\"" : string.Empty) + ">");
         xhtml.AppendLine("  <h1>" + HttpUtility.HtmlDecode(jumbotron.Title) + "</h1>");
         xhtml.AppendLine("  <p>" + HttpUtility.HtmlDecode(jumbotron.Description) + "</p>");
         xhtml.AppendLine("  <p><a class=\"btn btn-primary btn-lg\" role=\"button\">Learn more</a></p>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="ListGroup"/>.
      /// </summary>
      private string RenderListGroup(ListGroup listGroup)
      {
         string cssStyle = string.Empty;
         StringBuilder xhtml = new StringBuilder();

         if (listGroup.Style == ListGroupStyle.Simple)
         {
            xhtml.AppendLine("<ul " + listGroup.GetIdParameter() + "class=\"list-group\">");
            foreach (ListItem item in listGroup.ListItems)
            {
               item.Style = listGroup.Style;
               item.AlignDescription = listGroup.AlignDescription;

               xhtml.AppendLine(RenderListItem(item));
            }
            xhtml.AppendLine("</ul>");
         }
         else
         {
            xhtml.AppendLine("<div " + listGroup.GetIdParameter() + "class=\"list-group\">");
            foreach (ListItem item in listGroup.ListItems)
            {
               item.Style = listGroup.Style;
               item.AlignDescription = listGroup.AlignDescription;

               xhtml.AppendLine(RenderListItem(item));
            }
            xhtml.AppendLine("</div>");
         }

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="ListItem"/>.
      /// </summary>
      private string RenderListItem(ListItem listItem)
      {
         string cssStyle = string.Empty;
         StringBuilder xhtml = new StringBuilder();

         if (string.IsNullOrEmpty(listItem.Href))
         {
            if (listItem.Style == ListGroupStyle.Simple)
            {
               xhtml.AppendLine("  <li class=\"list-group-item" +
                                   (listItem.IsActive ? " active" : string.Empty) +
                                   (listItem.Type != ComponentColorScheme.Normal ? " list-group-item-" + GetCssClassFromControlColor(listItem.Type) : string.Empty) + "\">");
               if (!string.IsNullOrEmpty(listItem.BadgeText)) xhtml.AppendLine("    <span class=\"badge\">" + HttpUtility.HtmlDecode(listItem.BadgeText) + "</span>");
               if (!string.IsNullOrEmpty(listItem.Icon)) xhtml.AppendLine("    " + Icon.GetIcon(null, listItem.Icon) + "&nbsp;&nbsp;");
               xhtml.AppendLine("    " + HttpUtility.HtmlDecode(listItem.Caption));
               xhtml.AppendLine("  </li>");
            }
            else
            {
               if (!string.IsNullOrEmpty(listItem.Icon) && listItem.AlignDescription)
               {
                  cssStyle = " style=\"padding-left:30px;\"";
               }

               xhtml.AppendLine("  <li class=\"list-group-item" +
                                   (listItem.IsActive ? " active" : string.Empty) +
                                   (listItem.Type != ComponentColorScheme.Normal ? " list-group-item-" + GetCssClassFromControlColor(listItem.Type) : string.Empty) + "\">");
               xhtml.AppendLine("    <h4 class=\"list-group-item-heading\">" + (!string.IsNullOrEmpty(listItem.Icon) ? Icon.GetIcon(null, listItem.Icon) + "&nbsp;&nbsp;" : string.Empty) + HttpUtility.HtmlDecode(listItem.Caption) +
                                     (!string.IsNullOrEmpty(listItem.BadgeText) ? "<span class=\"badge pull-right\">" + HttpUtility.HtmlDecode(listItem.BadgeText) + "</span>" : string.Empty) + "</h4>");
               if (!string.IsNullOrEmpty(listItem.Description)) xhtml.AppendLine("    <p class=\"list-group-item-text\"" + cssStyle + ">" + HttpUtility.HtmlDecode(listItem.Description) + "</p>");
               xhtml.AppendLine("  </li>");
            }
         }
         else
         {
            if (listItem.Style == ListGroupStyle.Simple)
            {
               xhtml.AppendLine("  <a href=\"" + listItem.Href + "\" class=\"list-group-item" +
                               (listItem.IsActive ? " active" : string.Empty) +
                               (listItem.Type != ComponentColorScheme.Normal ? " list-group-item-" + GetCssClassFromControlColor(listItem.Type) : string.Empty) + "\">");
               if (!string.IsNullOrEmpty(listItem.BadgeText)) xhtml.AppendLine("    <span class=\"badge\">" + HttpUtility.HtmlDecode(listItem.BadgeText) + "</span>");
               if (!string.IsNullOrEmpty(listItem.Icon)) xhtml.AppendLine(Icon.GetIcon(null, listItem.Icon) + "&nbsp;&nbsp;");
               xhtml.AppendLine("    " + HttpUtility.HtmlDecode(listItem.Caption));
               xhtml.AppendLine("  </a>");
            }
            else
            {
               if (!string.IsNullOrEmpty(listItem.Icon) && listItem.AlignDescription)
               {
                  cssStyle = " style=\"padding-left:30px;\"";
               }

               xhtml.AppendLine("  <a href=\"" + listItem.Href + "\" class=\"list-group-item" +
                                   (listItem.IsActive ? " active" : string.Empty) +
                                   (listItem.Type != ComponentColorScheme.Normal ? " list-group-item-" + GetCssClassFromControlColor(listItem.Type) : string.Empty) + "\">");
               xhtml.AppendLine("    <h4 class=\"list-group-item-heading\">" + (!string.IsNullOrEmpty(listItem.Icon) ? Icon.GetIcon(null, listItem.Icon) + "&nbsp;&nbsp;" : string.Empty) + HttpUtility.HtmlDecode(listItem.Caption) +
                                     (!string.IsNullOrEmpty(listItem.BadgeText) ? "<span class=\"badge pull-right\">" + HttpUtility.HtmlDecode(listItem.BadgeText) + "</span>" : string.Empty) + "</h4>");
               if (!string.IsNullOrEmpty(listItem.Description)) xhtml.AppendLine("    <p class=\"list-group-item-text\"" + cssStyle + ">" + HttpUtility.HtmlDecode(listItem.Description) + "</p>");
               xhtml.AppendLine("  </a>");
            }
         }

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="MediaList"/>.
      /// </summary>
      private string RenderMediaList(MediaList mediaList)
      {
         StringBuilder xhtml = new StringBuilder();

         if (mediaList.Style == MediaList.MediaListStyle.Thumbnail)
         {
            xhtml.AppendLine("<div " + mediaList.GetIdParameter() + "class=\"row\">");
         }

         foreach (MediaItem thumb in mediaList.Items)
         {
            if (mediaList.Style == MediaList.MediaListStyle.Thumbnail)
            {
               xhtml.AppendLine("<div class=\"col-sm-6 col-md-4\">");
               xhtml.AppendLine("  <div class=\"thumbnail\">");
               xhtml.AppendLine("    <img class=\"img-thumbnail\" alt=\"" + HttpUtility.HtmlDecode(thumb.Title) + "\" src=\"" + thumb.Image + "\" style=\"" + (thumb.ImageWidth > 0 ? "width:" + thumb.ImageWidth + "px;" : string.Empty) + (thumb.ImageHeight > 0 ? "height:" + thumb.ImageHeight + "px;" : string.Empty) + "\">");
               xhtml.AppendLine("    <div class=\"caption\">");
               xhtml.AppendLine("      <h3>" + (!string.IsNullOrEmpty(thumb.Icon) ? Icon.GetIcon(mediaList.ParentViewport, thumb.Icon) + "&nbsp;&nbsp;" : string.Empty) + HttpUtility.HtmlDecode(thumb.Title) + "</h3>");
               xhtml.AppendLine("      <p>" + HttpUtility.HtmlDecode(thumb.Description) + "</p>");
               xhtml.AppendLine("      <p><a href=\"" + thumb.LinkHref + "\" class=\"btn btn-primary\" role=\"button\">" + HttpUtility.HtmlDecode(thumb.LinkCaption) + "</a></p>");
               xhtml.AppendLine("    </div>");
               xhtml.AppendLine("  </div>");
               xhtml.AppendLine("</div>");
            }
            else
            {
               xhtml.AppendLine("<div class=\"media\">");
               xhtml.AppendLine("  <a class=\"pull-left\" href=\"" + thumb.LinkHref + "\">");
               xhtml.AppendLine("    <img class=\"img-thumbnail\" alt=\"" + HttpUtility.HtmlDecode(thumb.Title) + "\" src=\"" + thumb.Image + "\" style=\"" + (thumb.ImageWidth > 0 ? "width:" + thumb.ImageWidth + "px;" : string.Empty) + (thumb.ImageHeight > 0 ? "height:" + thumb.ImageHeight + "px;" : string.Empty) + "\">");
               xhtml.AppendLine("  </a>");
               xhtml.AppendLine("  <div class=\"media-body\">");
               xhtml.AppendLine("    <h4 class=\"media-heading\">" + (!string.IsNullOrEmpty(thumb.Icon) ? Icon.GetIcon(mediaList.ParentViewport, thumb.Icon) + "&nbsp;&nbsp;" : string.Empty) + HttpUtility.HtmlDecode(thumb.Title) + "</h4>");
               xhtml.AppendLine("    <p>" + HttpUtility.HtmlDecode(thumb.Description) + "</p>");
               xhtml.AppendLine("  </div>");
               xhtml.AppendLine("</div>");
            }

            if (mediaList.UseItemSeparator) xhtml.AppendLine("<hr />");
         }

         if (mediaList.Style == MediaList.MediaListStyle.Thumbnail)
         {
            xhtml.AppendLine("</div>");
         }

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Navbar"/>.
      /// </summary>
      private string RenderNavbar(Navbar navbar)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<nav " + navbar.GetIdParameter() + "class=\"navbar navbar-default" + (navbar.IsFixedTop ? "  navbar-fixed-top" : string.Empty) + "\" role=\"navigation\">");
         xhtml.AppendLine("  <div class=\"container-fluid\">");
         if (navbar.Header != null)
         {
            xhtml.AppendLine("    <div class=\"navbar-header\">");
            xhtml.AppendLine("      <button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\"#" + navbar.Header.DomID + "\">");
            xhtml.AppendLine("        <span class=\"sr-only\">" + HttpUtility.HtmlDecode(navbar.Header.ToggleNavigationText) + "</span>");
            xhtml.AppendLine("        <span class=\"icon-bar\"></span>");
            xhtml.AppendLine("        <span class=\"icon-bar\"></span>");
            xhtml.AppendLine("        <span class=\"icon-bar\"></span>");
            xhtml.AppendLine("      </button>");
            xhtml.AppendLine("      <a class=\"navbar-brand\" href=\"" + navbar.Header.Href + "\">");

            if (string.IsNullOrWhiteSpace(navbar.Header.LogoImageUrl))
            {
               xhtml.AppendLine(HttpUtility.HtmlDecode(navbar.Header.Caption));
            }
            else
            {
               xhtml.AppendLine("<img src=\"" + navbar.Header.LogoImageUrl + "\" alt=\"" + HttpUtility.HtmlDecode(navbar.Header.Caption) + "\" />");
            }

            xhtml.AppendLine("      </a>");
            xhtml.AppendLine("    </div>");
         }

         if (navbar.Items.Count > 0)
         {
            xhtml.AppendLine("    <div class=\"navbar-collapse collapse\">");

            // Elementos izquierda
            xhtml.AppendLine("      <ul class=\"nav navbar-nav\">");
            foreach (NavbarIButton item in navbar.Items)
            {
               if (item.Position == NavbarItemPosition.Left)
               {
                  xhtml.AppendLine(item.ToXhtml());
               }
            }
            xhtml.AppendLine("      </ul>");

            // Elementos derecha
            xhtml.AppendLine("      <ul class=\"nav navbar-nav navbar-right\">");
            foreach (NavbarIButton item in navbar.Items)
            {
               if (item.Position == NavbarItemPosition.Right)
               {
                  xhtml.AppendLine(item.ToXhtml());
               }
            }
            xhtml.AppendLine("      </ul>");
            xhtml.AppendLine("    </div>");
         }

         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</nav>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Panel"/>.
      /// </summary>
      private string RenderPanel(Panel panel)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + panel.GetIdParameter() + "class=\"panel panel-default\">");
         if (!string.IsNullOrWhiteSpace(panel.Caption))
         {
            xhtml.AppendLine("  <div class=\"panel-heading\">" + panel.Caption + "</div>");
         }

         if (!string.IsNullOrWhiteSpace(panel.ContentXhtml))
         {
            xhtml.AppendLine("  <div " + (!string.IsNullOrWhiteSpace(panel.ContentDomId) ? "id=\"" + panel.ContentDomId + "\"" : string.Empty) + "class=\"panel-body\">");
            xhtml.AppendLine(panel.ContentXhtml);
            xhtml.AppendLine("  </div>");
         }

         foreach (IControl component in panel.Content)
         {
            xhtml.AppendLine(component.ToXhtml());
         }

         if (panel.Footer.Count > 0)
         {
            foreach (IControl component in panel.Footer)
            {
               xhtml.AppendLine(component.ToXhtml());
            }
         }
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Popover"/>.
      /// </summary>
      private string RenderPopover(Popover popover)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + popover.GetIdParameter() + "class=\"popover " + ConvertPopoverDirectionToString(popover.Direction) + " chat-message-" + ConvertPopoverDirectionToString(popover.Direction) + "\">");
         xhtml.AppendLine("  <div class=\"arrow\"></div>");
         xhtml.AppendLine("  <h3 class=\"popover-title\">" + HttpUtility.HtmlDecode(popover.Caption) + "</h3>");
         xhtml.AppendLine("  <div class=\"popover-content\">");
         xhtml.AppendLine(HttpUtility.HtmlDecode(popover.Text));
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Table"/>.
      /// </summary>
      private string RenderTable(Table table)
      {
         StringBuilder xhtml = new StringBuilder();

         if (table.Responsive) xhtml.AppendLine("<div class=\"table-responsive\">");
         xhtml.AppendLine("  <table " + table.GetIdParameter() + "class=\"table" + (table.Striped ? " table-striped" : string.Empty) +
                         (table.Bordered ? " table-bordered" : string.Empty) +
                         (table.Hover ? " table-hover" : string.Empty) +
                         (table.Condensed ? " table-condensed" : string.Empty) + "\">");
         if (table.Header != null)
         {
            // xhtml.AppendLine("    " + table.Header.ToXhtml());
         }
         foreach (TableRow row in table.Rows)
         {
            // xhtml.AppendLine("    " + row.ToXhtml());
         }
         xhtml.AppendLine("  </table>");
         if (table.Responsive) xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #region Auxiliar Methods

      private string ConvertPopoverDirectionToString(Cosmo.UI.Controls.ChatMessage.ChatMessageDirections direction)
      {
         switch (direction)
         {
            case Cosmo.UI.Controls.ChatMessage.ChatMessageDirections.Left:
               return "left";

            case Cosmo.UI.Controls.ChatMessage.ChatMessageDirections.Right:
               return "right";

            default:
               return string.Empty;
         }
      }

      private string ConvertPopoverDirectionToString(Cosmo.UI.Controls.Popover.PopoverDirections direction)
      {
         switch (direction)
         {
            case Cosmo.UI.Controls.Popover.PopoverDirections.Left:
               return "left";

            case Cosmo.UI.Controls.Popover.PopoverDirections.Right:
               return "right";

            default:
               return string.Empty;
         }
      }

      #endregion
      
   }
}
