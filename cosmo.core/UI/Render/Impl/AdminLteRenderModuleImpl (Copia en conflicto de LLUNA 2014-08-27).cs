using Cosmo.Communications.PrivateMessages;
using Cosmo.Handlers;
using Cosmo.UI.Controls;
using Cosmo.Utils;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Cosmo.UI.Render.Impl
{
   /// <summary>
   /// Implementa un módulo de renderizado para Bootstrap 3.
   /// </summary>
   public class AdminLteRenderModuleImpl : IRenderModule
   {
      /// <summary>
      /// Devuelve una instancia de <see cref="AdminLteRenderModuleImpl"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      public AdminLteRenderModuleImpl(Workspace workspace, Plugin plugin)
         : base(workspace, plugin) 
      { }

      /// <summary>
      /// Renderiza un control.
      /// </summary>
      /// <param name="control">Instancia del control a renderizar.</param>
      /// <param name="postback">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string Render(Controls.IControl control, bool postback)
      {
         if (control.GetType() == typeof(LayoutContainer))
         {
            return RenderLayout((LayoutContainer)control, postback);
         }
         else if (control.GetType() == typeof(Alert))
         {
            return RenderAlert((Alert)control);
         }
         else if (control.GetType() == typeof(Breadcrumb))
         {
            return RenderBreadcrumb((Breadcrumb)control);
         }
         else if (control.GetType() == typeof(ButtonBar))
         {
            return RenderButtonBar((ButtonBar)control);
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
         else if (control.GetType() == typeof(Sidebar))
         {
            return RenderSidebar((Sidebar)control);
         }
         else if (control.GetType() == typeof(SidebarButton))
         {
            return RenderSidebarButton((SidebarButton)control);
         }
         else if (control.GetType() == typeof(PageHeader))
         {
            return RenderPageHeader((PageHeader)control);
         }
         else if (control.GetType() == typeof(DocumentHeader))
         {
            return RenderDocumentHeader((DocumentHeader)control);
         }
         else if (control.GetType() == typeof(TreeView))
         {
            return RenderTreeView((TreeView)control);
         }
         else if (control.GetType() == typeof(HtmlContent))
         {
            return RenderHtmlContent((HtmlContent)control);
         }
         else if (control.GetType() == typeof(Form))
         {
            return RenderForm((Form)control, postback);
         }
         else if (control.GetType() == typeof(FormFieldText))
         {
            return RenderFormFieldText((FormFieldText)control, postback);
         }
         else if (control.GetType() == typeof(FormFieldEditor))
         {
            return RenderFormFieldTextArea((FormFieldEditor)control, postback);
         }
         else if (control.GetType() == typeof(FormFieldList))
         {
            return RenderFormFieldList((FormFieldList)control, postback);
         }
         else if (control.GetType() == typeof(FormFieldBoolean))
         {
            return RenderFormFieldBoolean((FormFieldBoolean)control, postback);
         }
         else if (control.GetType() == typeof(FormFieldImage))
         {
            return RenderFormFieldImage((FormFieldImage)control, postback);
         }
         else if (control.GetType() == typeof(FormFieldFile))
         {
            return RenderFormFieldFile((FormFieldFile)control, postback);
         }
         else if (control.GetType() == typeof(LoginForm))
         {
            return RenderLoginForm((LoginForm)control);
         }
         else if (control.GetType() == typeof(TabbedContainer))
         {
            return RenderTabbedContainer((TabbedContainer)control);
         }
         else if (control.GetType() == typeof(Error))
         {
            return RenderError((Error)control);
         }
         else if (typeof(ModalForm).IsAssignableFrom(control.GetType()))
         {
            return RenderModalForm((ModalForm)control);
         }
         else if (typeof(Badge).IsAssignableFrom(control.GetType()))
         {
            return RenderBadge((Badge)control);
         }
         else if (typeof(ProgressBar).IsAssignableFrom(control.GetType()))
         {
            return RenderProgressBar((ProgressBar)control);
         }
         else if (typeof(Timeline).IsAssignableFrom(control.GetType()))
         {
            return RenderTimeline((Timeline)control);
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
      /// <param name="page">Una instancia de <see cref="CosmoPage"/> que representa la instancia renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string RenderPage(CosmoPage page)
      {
         return RenderPage(page, false);
      }

      /// <summary>
      /// Renderiza una página.
      /// </summary>
      /// <param name="page">Una instancia de <see cref="CosmoPage"/> que representa la instancia renderizar.</param>
      /// <param name="postback">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string RenderPage(CosmoPage page, bool postback)
      {
         StringBuilder xhtml = new StringBuilder();
         StringBuilder js = new StringBuilder();

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

         foreach (string css in this.CssResources)
         {
            xhtml.AppendLine("  <link href=\"" + css + "\" rel=\"stylesheet\" type=\"text/css\" />");
         }

         foreach (string jsr in this.JavascriptResources)
         {
            xhtml.AppendLine("  <script type=\"text/javascript\" src=\"" + jsr + "\"></script>");
         }

         foreach (CosmoPageResource resource in page.Resources)
         {
            if (resource.Type == CosmoPageResource.ResourceType.CSS)
            {
               xhtml.AppendLine("  <link href=\"" + resource.FilePath + "\" rel=\"stylesheet\" type=\"text/css\" />");
            }
            else if (resource.Type == CosmoPageResource.ResourceType.JavaScript)
            {
               xhtml.AppendLine("  <script type=\"text/javascript\" src=\"" + resource.FilePath + "\"></script>");
            }
         }

         xhtml.AppendLine("</head>");
         xhtml.AppendLine("<body class=\"" + (page.Layout.FadeBackground ? "bg-black" : "skin-blue fixed") + "\">");

         // Renderiza controles de página
         xhtml.AppendLine(Render(page.Layout, postback));

         // Renderiza formularios modales
         foreach (ModalForm modal in page.ModalForms)
         {
            xhtml.AppendLine(Render(modal, postback));
         }

         // Renderiza scripts
         xhtml.AppendLine(RenderScripts(page));

         xhtml.AppendLine("</body>");
         xhtml.AppendLine("</html>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza una plantilla de visualización.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="CosmoTemplate"/> que representa la instancia renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public string RenderTemplate(CosmoTemplate template)
      {
         StringBuilder xhtml = new StringBuilder();
         StringBuilder js = new StringBuilder();

         // Renderiza controles de página
         xhtml.AppendLine(Render(template.Content));

         // Renderiza formularios modales
         foreach (ModalForm modal in template.ModalForms)
         {
            xhtml.AppendLine(Render(modal));
         }

         // Renderiza scripts
         xhtml.AppendLine(RenderScripts(template));

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza los scripts de la página.
      /// </summary>
      public string RenderScripts(ICosmoViewport viewport)
      {
         // Obtiene todos los scripts de la página
         if (viewport.Scripts.Count <= 0)
         {
            return string.Empty;
         }

         // Renderiza los scripts
         StringBuilder js = new StringBuilder();
         js.AppendLine("<script type=\"text/javascript\">");

         // Scripts no dependientes de la carga de la página
         foreach (Script scr in viewport.Scripts)
         {
            if (!scr.RunOnDocumentReady)
            {
               js.AppendLine(scr.Source.ToString());
            }
         }

         // Scripts dependientes de la carga de la página
         js.AppendLine("  $( document ).ready(function() {");
         foreach (Script scr in viewport.Scripts)
         {
            if (scr.RunOnDocumentReady)
            {
               js.AppendLine(scr.Source.ToString());
            }
         }
         js.AppendLine("  });");

         js.AppendLine("</script>");

         return js.ToString();
      }

      #region LayoutContainer

      /// <summary>
      /// Renderiza un control <see cref="LayoutContainer"/>.
      /// </summary>
      private string RenderLayout(LayoutContainer layout, bool postback)
      {
         StringBuilder xhtml = new StringBuilder();

         // Si es un layout de control simple llama al método especializado
         if (layout.IsSingleControlLayout)
         {
            return RenderSingleControlLayout(layout, postback);
         }

         // Header
         foreach (IControl ctrl in layout.Header)
         {
            xhtml.AppendLine(Render(ctrl, postback));
         }

         xhtml.AppendLine("<div class=\"wrapper row-offcanvas row-offcanvas-left\">");

         // Left column
         if (layout.LeftContent.Count > 0)
         {
            xhtml.AppendLine("  <aside class=\"left-side sidebar-offcanvas\">");
            foreach (IControl ctrl in layout.LeftContent)
            {
               xhtml.AppendLine(Render(ctrl, postback));
            }
            xhtml.AppendLine("  </aside>");
         }

         // Main content
         if (layout.LeftContent.Count > 0)
         {
            xhtml.AppendLine("  <aside class=\"right-side\">");
         }

         // Header
         if ((layout.MainContent.Count > 0) && (layout.MainContent.Get(0).GetType() == typeof(PageHeader)))
         {
            xhtml.AppendLine("  <section class=\"content-header\">");
            xhtml.AppendLine(Render(layout.MainContent.Get(0), postback));
            xhtml.AppendLine("  </section>");

            layout.MainContent.Remove(layout.MainContent.Get(0));
         }

         xhtml.AppendLine("  <section class=\"content\">");

         // Layout de tres columnas
         if (layout.RightContent.Count > 0)
         {
            xhtml.AppendLine("    <div class=\"row\">");
            xhtml.AppendLine("      <div class=\"col-md-9\">");
            foreach (IControl ctrl in layout.MainContent)
            {
               xhtml.AppendLine(Render(ctrl, postback));
            }
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("      <div class=\"col-md-3\">");
            foreach (IControl ctrl in layout.RightContent)
            {
               xhtml.AppendLine(Render(ctrl, postback));
            }
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("    </div>");
         }
         else
         {
            foreach (IControl ctrl in layout.MainContent)
            {
               xhtml.AppendLine(Render(ctrl, postback));
            }
         }

         xhtml.AppendLine("    </section>");

         if (layout.LeftContent.Count > 0)
         {
            xhtml.AppendLine("  </aside>");
         }

         // Footer
         foreach (IControl ctrl in layout.Footer)
         {
            xhtml.AppendLine(Render(ctrl, postback));
         }

         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un layout que contiene sólo un control
      /// </summary>
      private string RenderSingleControlLayout(LayoutContainer layout, bool postback)
      {
         StringBuilder xhtml = new StringBuilder();

         if (layout.Header.Count > 0) xhtml.AppendLine(Render(layout.Header.Get(0), postback));
         if (layout.LeftContent.Count > 0) xhtml.AppendLine(Render(layout.LeftContent.Get(0), postback));
         if (layout.MainContent.Count > 0) xhtml.AppendLine(Render(layout.MainContent.Get(0), postback));
         if (layout.RightContent.Count > 0) xhtml.AppendLine(Render(layout.RightContent.Get(0), postback));
         if (layout.Footer.Count > 0) xhtml.AppendLine(Render(layout.Footer.Get(0), postback));

         return xhtml.ToString();
      }

      #endregion

      #region Alert Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="Alert"/>.
      /// </summary>
      private string RenderAlert(Alert alert)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + alert.GetIdParameter() + "class=\"alert alert-" + IControl.ColorSchemeToString(alert.Type) + " " + (alert.Closeable ? string.Empty : "alert-dismissable") + "\">");
         xhtml.AppendLine("  <i class=\"fa fa-" + IControl.ColorSchemeToString(alert.Type) + "\"></i>");
         if (alert.Closeable) xhtml.AppendLine("  <button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>");
         xhtml.AppendLine("  " + alert.Text);
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region Breadcrumb Control

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
            if (!string.IsNullOrEmpty(item.Href)) xhtml.AppendLine("<a href=\"" + item.Href + "\">");
            if (!string.IsNullOrEmpty(item.Icon)) xhtml.AppendLine(Glyphicon.GetIcon(item.Icon)); 
            xhtml.AppendLine(HttpUtility.HtmlDecode(item.Caption));
            if (!string.IsNullOrEmpty(item.Href)) xhtml.AppendLine("</a>");
            xhtml.AppendLine("  </li>");
         }
         xhtml.AppendLine("</ol>");

         return xhtml.ToString();
      }

      #endregion

      #region Button / ButtonBar Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="ButtonBar"/>.
      /// </summary>
      private string RenderButtonBar(ButtonBar control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"" + (control.Vertical ? "btn-group-vertical" : "btn-group") + "\">");
         foreach (Button btn in control.Buttons)
         {
            btn.Size = control.Size;
            xhtml.AppendLine(Render(btn));
         }
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="Button"/>.
      /// </summary>
      private string RenderButton(Button button)
      {
         StringBuilder xhtml = new StringBuilder();

         string color = string.Empty;
         color = IControl.ColorSchemeToString(button.Color);
         color = string.IsNullOrWhiteSpace(color) ? "default" : color;

         xhtml.Append("<" + (string.IsNullOrWhiteSpace(button.Href) ? "button" : "a") + " ");
         if (button.Type == Button.ButtonTypes.Submit)
         {
            xhtml.Append("type=\"submit\" ");
         }
         else
         {
            xhtml.Append("type=\"button\" ");
         }
         xhtml.Append("class=\"btn btn-" + color + "" + (button.Block ? " btn-block" : string.Empty) + (button.Enabled ? string.Empty : " disabled") + " " + GetButtonSizeClass(button.Size) + "\" ");
         xhtml.Append(string.IsNullOrWhiteSpace(button.Href) ? string.Empty : "href=\"" + button.Href + "\" ");
         xhtml.Append(string.IsNullOrWhiteSpace(button.JavaScriptAction) ? string.Empty : "onclick=\"javascript:" + button.JavaScriptAction + "\"");
         xhtml.Append(!string.IsNullOrWhiteSpace(button.ModalDomId) && button.Type == Button.ButtonTypes.OpenModalForm ? "data-toggle=\"modal\" data-target=\"#" + button.ModalDomId.Trim() + "\" " : string.Empty);
         xhtml.Append(">");
         xhtml.Append((string.IsNullOrWhiteSpace(button.Icon) ? string.Empty : Glyphicon.GetIcon(button.Icon) + "&nbsp;&nbsp;") + HttpUtility.HtmlDecode(button.Caption));
         xhtml.Append("</" + (string.IsNullOrWhiteSpace(button.Href) ? "button" : "a") + ">\n");

         return xhtml.ToString();
      }

      private string GetButtonSizeClass(Button.ButtonSizes size)
      {
         switch (size)
         {
            case Button.ButtonSizes.Large: return "btn-lg";
            case Button.ButtonSizes.Small: return "btn-sm";
            case Button.ButtonSizes.ExtraSmall: return "btn-xs";
         }

         return string.Empty;
      }

      #endregion

      #region Callout Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="Callout"/>.
      /// </summary>
      private string RenderCallout(Callout callout)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + callout.GetIdParameter() + "class=\"callout callout-" + IControl.ColorSchemeToString(callout.Type) + "\">");
         xhtml.AppendLine("  <h4>" + HttpUtility.HtmlDecode(callout.Title) + "</h4>");
         xhtml.AppendLine("  <p>" + HttpUtility.HtmlDecode(callout.Text) + "</p>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region Carrousel Control

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

      #endregion

      #region Chat Control

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
            xhtml.AppendLine("<div id=\"" + message.DomID + "\" class=\"item\">");
            xhtml.AppendLine("   <img src=\"" + Workspace.CurrentUser.User.GetAvatarImage() + "\" alt=\"user image\" class=\"online\" />");
            xhtml.AppendLine("   <p class=\"message\">");
            xhtml.AppendLine("      <a href=\"#\" class=\"name\">");
            xhtml.AppendLine("         <small class=\"text-muted pull-right\"><i class=\"fa fa-clock-o\"></i> 2:15</small>");
            xhtml.AppendLine("         Mike Doe");
            xhtml.AppendLine("      </a>");
            xhtml.AppendLine(HttpUtility.HtmlDecode(message.Content).Replace("\n", "<br />\n"));
            xhtml.AppendLine("   </p>");
            xhtml.AppendLine("</div>");

            /*
            xhtml.AppendLine("<div id=\"" + message.DomID + "\" class=\"popover " + ConvertPopoverDirectionToString(message.Direction) + " chat-message-" + ConvertPopoverDirectionToString(message.Direction) + "\">");
            xhtml.AppendLine("  <div class=\"arrow\"></div>");
            xhtml.AppendLine("  <div class=\"chat-msg-toolbar clearfix\">");
            xhtml.AppendLine("    <div class=\"chat-msg-title\">" + HttpUtility.HtmlDecode(message.Caption) + "</div>");
            if (message.ToolbarButtons.Count > 0)
            {
               xhtml.AppendLine("    <div class=\"btn-group pull-right\">");
               foreach (ChatMessageToolbarButton button in message.ToolbarButtons)
               {
                  xhtml.AppendLine("      <a href=\"" + button.Href + "\" class=\"btn btn-default btn-xs\">" + (string.IsNullOrWhiteSpace(button.Icon) ? string.Empty : Glyphicon.GetIcon(button.Icon)) + " " + HttpUtility.HtmlDecode(button.Caption) + "</a>");
               }
               xhtml.AppendLine("    </div>");
            }
            xhtml.AppendLine("  </div>");
            xhtml.AppendLine("  <div class=\"popover-content\">");
            xhtml.AppendLine(HttpUtility.HtmlDecode(message.Content).Replace("\n", "<br />\n"));
            xhtml.AppendLine("  </div>");
            xhtml.AppendLine("</div>");
            */
         }
         xhtml.AppendLine(chat.Messages.Count <= 0 ? "<h2><small>No hay mensajes entre vosotros</small></h2>" : string.Empty);
         panel.ContentXhtml = xhtml.ToString();
         panel.ContentDomId = "pmChatMsgs";

         // Representa el formulario de envio
         if (chat.FormShow)
         {
            xhtml.Clear();
            xhtml.AppendLine("    <form id=\"" + chat.FormDomID + "\" role=\"form\">");
            xhtml.AppendLine("      <input type=\"hidden\" name=\"" + Workspace.PARAM_COMMAND + "\" value=\"" + PrivateMessageDAO.COMMAND_SEND + "\">");
            xhtml.AppendLine("      <input type=\"hidden\" name=\"" + Workspace.PARAM_USER_ID + "\" value=\"" + chat.FormToUserID + "\">");
            xhtml.AppendLine("      <div class=\"form-group\">");
            xhtml.AppendLine("        <textarea id=\"" + PrivateMessageDAO.PARAM_BODY + "\" name=\"" + PrivateMessageDAO.PARAM_BODY + "\" class=\"form-control\" rows=\"1\" placeholder=\"Escribe el texto a enviar...\"></textarea>");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("      <button id=\"" + chat.FormSubmitButtonID + "\" type=\"button\" class=\"btn btn-sm btn-primary\" data-loading-text=\"Enviando...\">" + Glyphicon.GetIcon(Glyphicon.ICON_SEND) + "&nbsp;&nbsp;Enviar</button>");
            xhtml.AppendLine("    </form>");
            panel.Footer = xhtml.ToString();

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

      #endregion

      #region Jumbotron Control

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

      #endregion

      #region ListGroup Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="ListGroup"/>.
      /// </summary>
      private string RenderListGroup(ListGroup listGroup)
      {
         StringBuilder xhtml = new StringBuilder();

         if (listGroup.ListItems.Count > 0)
         {
            xhtml.AppendLine("<ul " + listGroup.GetIdParameter() + "class=\"nav nav-pills nav-stacked\">");
            foreach (ListItem item in listGroup.ListItems)
            {
               item.Style = listGroup.Style;
               item.AlignDescription = listGroup.AlignDescription;

               xhtml.AppendLine(RenderListItem(item));
            }
            xhtml.AppendLine("</ul>");
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

         xhtml.AppendLine("<li" + (listItem.IsActive ? " class=\"active\"" : string.Empty) + ">");
         xhtml.AppendLine("  <a href=\"" + listItem.Href + "\">");
         if (!string.IsNullOrWhiteSpace(listItem.Icon))
         {
            xhtml.AppendLine("    " + Glyphicon.GetIcon(listItem.Icon) + " ");
         }
         xhtml.AppendLine("    " + HttpUtility.HtmlDecode(listItem.Caption) + "<br />");
         if (!string.IsNullOrWhiteSpace(listItem.Description))
         {
            if (!string.IsNullOrWhiteSpace(listItem.Icon))
            {
               xhtml.AppendLine("    <span style=\"padding-left:20px;\"><small>" + HttpUtility.HtmlDecode(listItem.Description) + "</small></span>");
            }
            else
            {
               xhtml.AppendLine("    <small>" + HttpUtility.HtmlDecode(listItem.Description) + "</small>");
            }
         }
         xhtml.AppendLine("  </a>");
         xhtml.AppendLine("</li>");

         return xhtml.ToString();
      }

      #endregion

      #region MediaList Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="MediaList"/>.
      /// </summary>
      private string RenderMediaList(MediaList mediaList)
      {
         StringBuilder xhtml = new StringBuilder();

         if (mediaList.Style == MediaListStyle.Thumbnail)
         {
            xhtml.AppendLine("<div " + mediaList.GetIdParameter() + "class=\"row\">");
         }

         foreach (MediaItem thumb in mediaList.Items)
         {
            if (mediaList.Style == MediaListStyle.Thumbnail)
            {
               xhtml.AppendLine("<div class=\"col-sm-6 col-md-4\">");
               xhtml.AppendLine("  <div class=\"thumbnail\">");
               xhtml.AppendLine("    <img class=\"img-thumbnail\" alt=\"" + HttpUtility.HtmlDecode(thumb.Title) + "\" src=\"" + thumb.Image + "\" style=\"" + (thumb.ImageWidth > 0 ? "width:" + thumb.ImageWidth + "px;" : string.Empty) + (thumb.ImageHeight > 0 ? "height:" + thumb.ImageHeight + "px;" : string.Empty) + "\">");
               xhtml.AppendLine("    <div class=\"caption\">");
               xhtml.AppendLine("      <h3>" + (!string.IsNullOrEmpty(thumb.Icon) ? Glyphicon.GetIcon(thumb.Icon) + "&nbsp;&nbsp;" : string.Empty) + HttpUtility.HtmlDecode(thumb.Title) + "</h3>");
               xhtml.AppendLine("      <p>" + HttpUtility.HtmlDecode(thumb.Description) + "</p>");
               xhtml.AppendLine("      <p><a href=\"" + thumb.LinkHref + "\" class=\"btn btn-primary\" role=\"button\">" + HttpUtility.HtmlDecode(thumb.LinkCaption) + "</a></p>");
               xhtml.AppendLine("    </div>");
               xhtml.AppendLine("  </div>");
               xhtml.AppendLine("</div>");
            }
            else
            {
               xhtml.AppendLine("<div class=\"media\">");
               if (!string.IsNullOrWhiteSpace(thumb.Image))
               {
                  xhtml.AppendLine("  <a class=\"pull-left\" href=\"" + thumb.LinkHref + "\">");
                  xhtml.AppendLine("    <img class=\"img-thumbnail\" alt=\"" + HttpUtility.HtmlDecode(thumb.Title) + "\" src=\"" + thumb.Image + "\" style=\"" + (thumb.ImageWidth > 0 ? "width:" + thumb.ImageWidth + "px;" : string.Empty) + (thumb.ImageHeight > 0 ? "height:" + thumb.ImageHeight + "px;" : string.Empty) + "\">");
                  xhtml.AppendLine("  </a>");
               }
               xhtml.AppendLine("  <div class=\"media-body\">");
               xhtml.AppendLine("    <h4 class=\"media-heading\">" + (!string.IsNullOrEmpty(thumb.Icon) ? Glyphicon.GetIcon(thumb.Icon) + "&nbsp;&nbsp;" : string.Empty) + "<a href=\"" + thumb.LinkHref + "\">" + HttpUtility.HtmlDecode(thumb.Title) + "</a></h4>");
               xhtml.AppendLine("    <p>" + HttpUtility.HtmlDecode(thumb.Description) + "</p>");
               xhtml.AppendLine("  </div>");
               xhtml.AppendLine("</div>");
            }

            if (mediaList.UseItemSeparator) xhtml.AppendLine("<hr />");
         }

         if (mediaList.Style == MediaListStyle.Thumbnail)
         {
            xhtml.AppendLine("</div>");
         }

         return xhtml.ToString();
      }

      #endregion

      #region Navbar Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="Navbar"/>.
      /// </summary>
      private string RenderNavbar(Navbar navbar)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<header class=\"header\">");
         if (navbar.Header != null)
         {
            xhtml.AppendLine("  <a href=\"" + navbar.Header.Href + "\" class=\"logo\">");
            if (string.IsNullOrWhiteSpace(navbar.Header.LogoImageUrl))
            {
               xhtml.AppendLine(HttpUtility.HtmlDecode(navbar.Header.Caption));
            }
            else
            {
               xhtml.AppendLine("    <img src=\"" + navbar.Header.LogoImageUrl + "\" alt=\"" + HttpUtility.HtmlDecode(navbar.Header.Caption) + "\" class=\"icon\" />");
            }
            xhtml.AppendLine("  </a>");
         }

         xhtml.AppendLine("  <nav class=\"navbar navbar-static-top\" role=\"navigation\">");

         xhtml.AppendLine("    <a href=\"#\" class=\"navbar-btn sidebar-toggle\" data-toggle=\"offcanvas\" role=\"button\">");
         xhtml.AppendLine("      <span class=\"sr-only\">" + HttpUtility.HtmlDecode(navbar.Header.ToggleNavigationText) + "</span>");
         xhtml.AppendLine("      <span class=\"icon-bar\"></span>");
         xhtml.AppendLine("      <span class=\"icon-bar\"></span>");
         xhtml.AppendLine("      <span class=\"icon-bar\"></span>");
         xhtml.AppendLine("    </a>");

         if (navbar.Items.Count > 0)
         {
            // Para que salgan en el mismo órden que en la configuración
            navbar.Items.Reverse();

            xhtml.AppendLine("    <div class=\"navbar-right\">");
            xhtml.AppendLine("      <ul class=\"nav navbar-nav\">");
            foreach (NavbarIButton item in navbar.Items)
            {
               if (item.GetType() == typeof(NavbarLoginItem))
               {
                  xhtml.AppendLine(RenderNavbarLoginItem((NavbarLoginItem)item));
               }
               else if (item.GetType() == typeof(NavbarPrivateMessagesItem))
               {
                  xhtml.AppendLine(RenderNavbarPrivateMessagesItem((NavbarPrivateMessagesItem)item));
               }
               else
               {
                  // TODO: Falta incorporar la posibilidad de subelementos mediante menú desplegable

                  xhtml.AppendLine("        <li>");
                  xhtml.AppendLine("          <a href=\"" + item.Href + "\">");
                  if (!string.IsNullOrWhiteSpace(item.Icon)) xhtml.AppendLine("            " + Glyphicon.GetIcon(item.Icon) + " ");
                  xhtml.AppendLine("            " + HttpUtility.HtmlDecode(item.Caption));
                  xhtml.AppendLine("          </a>");
                  xhtml.AppendLine("        </li>");
               }
            }
            xhtml.AppendLine("      </ul>");
            xhtml.AppendLine("    </div>");
         }

         xhtml.AppendLine("  </nav>");

         xhtml.AppendLine("</header>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Representa un elemento de menú que muestra el perfil de usuario y las opciones.
      /// </summary>
      private string RenderNavbarLoginItem(NavbarLoginItem item)
      {
         StringBuilder xhtml = new StringBuilder();

         if (Workspace.CurrentUser.IsAuthenticated)
         {
            xhtml.AppendLine("<li class=\"dropdown user user-menu\">");
            xhtml.AppendLine("  <a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">");
            xhtml.AppendLine("    <span class=\"glyphicon glyphicon-user\"></span>");
            xhtml.AppendLine("    <span>" + Workspace.CurrentUser.User.Login + " <i class=\"caret\"></i></span>");
            xhtml.AppendLine("  </a>");
            xhtml.AppendLine("  <ul class=\"dropdown-menu\">");
            xhtml.AppendLine("    <li class=\"user-header bg-light-blue\">");
            xhtml.AppendLine("      <img src=\"" + Workspace.CurrentUser.User.GetAvatarImage() + "\" class=\"img-circle\" alt=\"User Image\" />");
            xhtml.AppendLine("      <p>");
            xhtml.AppendLine("        " + Workspace.CurrentUser.User.GetDisplayName() + " (<em>" + Workspace.CurrentUser.User.Login + "</em>)");
            xhtml.AppendLine("        <small>Miembro desde " + Workspace.CurrentUser.User.Created.ToString("dd/MM/yyyy") + "</small>");
            xhtml.AppendLine("      </p>");
            xhtml.AppendLine("    </li>");
            xhtml.AppendLine("    <li class=\"user-body\">");
            xhtml.AppendLine("      <div class=\"col-xs-4 text-center\">");
            xhtml.AppendLine("        <a href=\"#\">Followers</a>");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("      <div class=\"col-xs-4 text-center\">");
            xhtml.AppendLine("        <a href=\"#\">Sales</a>");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("      <div class=\"col-xs-4 text-center\">");
            xhtml.AppendLine("        <a href=\"#\">Friends</a>");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("    </li>");
            xhtml.AppendLine("    <li class=\"user-footer\">");
            xhtml.AppendLine("      <div class=\"pull-left\">");
            xhtml.AppendLine("        <a href=\"#\" class=\"btn btn-default btn-flat\">Perfil</a>");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("      <div class=\"pull-right\">");
            xhtml.AppendLine("        <a href=\"#\" class=\"btn btn-default btn-flat\">Cerrar sesión</a>");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("    </li>");
            xhtml.AppendLine("  </ul>");
            xhtml.AppendLine("</li>");
         }
         else
         {
            xhtml.AppendLine("<li class=\"user\">");
            xhtml.AppendLine("  <a href=\"" + Cosmo.Workspace.COSMO_URL_LOGIN + "\">");
            xhtml.AppendLine("    <i class=\"glyphicon glyphicon-user\"></i>");
            xhtml.AppendLine("    <span>Iniciar sesión</span>");
            xhtml.AppendLine("  </a>");
            xhtml.AppendLine("</li>");
         }

         return xhtml.ToString();
      }

      /// <summary>
      /// Representa un elemento de menú que muestra el número de mensajes privados nuevos.
      /// </summary>
      private string RenderNavbarPrivateMessagesItem(NavbarPrivateMessagesItem item)
      {
         int unreadMsgs = 0;
         StringBuilder xhtml = new StringBuilder();

         if (Workspace.CurrentUser.IsAuthenticated)
         {
            unreadMsgs = Workspace.Communications.PrivateMessages.CountUnreadMessages();

            xhtml.AppendLine("      <li>");
            xhtml.AppendLine("        <a href=\"" + Cosmo.Workspace.COSMO_URL_USER_MESSAGES + "\">");
            xhtml.AppendLine("          <i class=\"fa fa-envelope\"></i>");
            if (unreadMsgs > 0)
            {
               xhtml.AppendLine("          &nbsp;");
               xhtml.AppendLine("          <span class=\"label label-danger\">" + unreadMsgs + "</span>");
            }
            xhtml.AppendLine("        </a>");
            xhtml.AppendLine("      </li>");
         }
         else
         {
            // No representa el elemento
         }

         return xhtml.ToString();
      }

      #endregion

      #region Panel Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="Panel"/>.
      /// </summary>
      private string RenderPanel(Panel panel)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + panel.GetIdParameter() + " class=\"box box-primary\">");
         if (!string.IsNullOrWhiteSpace(panel.Caption))
         {
            xhtml.AppendLine("  <div class=\"box-header\" title=\"\">");
            xhtml.AppendLine("    <h3 class=\"box-title\">" + panel.Caption + "</h3>");
            // xhtml.AppendLine("    <div class=\"box-tools pull-right\">");
            // xhtml.AppendLine("      <button class=\"btn btn-primary btn-xs\" data-widget=\"collapse\"><i class=\"fa fa-minus\"></i></button>");
            // xhtml.AppendLine("      <button class=\"btn btn-primary btn-xs\" data-widget=\"remove\"><i class=\"fa fa-times\"></i></button>");
            // xhtml.AppendLine("    </div>");
            xhtml.AppendLine("  </div>");
         }
         xhtml.AppendLine("  <div class=\"box-body\">");

         if (!string.IsNullOrWhiteSpace(panel.ContentXhtml))
         {
            xhtml.AppendLine(panel.ContentXhtml);
         }

         foreach (IControl component in panel.Content)
         {
            xhtml.AppendLine(Render(component));
         }

         xhtml.AppendLine("  </div>");
         if (!string.IsNullOrWhiteSpace(panel.Footer))
         {
            xhtml.AppendLine("  <div class=\"box-footer\">");
            xhtml.AppendLine(panel.Footer);
            xhtml.AppendLine("  </div>");
         }
         xhtml.AppendLine("</div>");

         /*
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

         foreach (IControl component in panel.ContentComponents)
         {
            // xhtml.AppendLine(component.ToXhtml());
         }

         if (!string.IsNullOrWhiteSpace(panel.Footer))
         {
            xhtml.AppendLine("  <div class=\"panel-footer\">" + panel.Footer + "</div>");
         }
         xhtml.AppendLine("</div>");
         */

         return xhtml.ToString();
      }

      #endregion

      #region Popover Control

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

      #endregion

      #region Table Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="Table"/>.
      /// </summary>
      private string RenderTable(Table table)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<table " + table.GetIdParameter() + "class=\"table" + (table.Striped ? " table-striped" : string.Empty) +
                         (table.Bordered ? " table-bordered" : string.Empty) +
                         (table.Hover ? " table-hover" : string.Empty) +
                         (table.Condensed ? " table-condensed" : string.Empty) + "\">");
         if (table.Header != null)
         {
            xhtml.AppendLine("  " + RenderTableRow(table.Header));
         }
         foreach (TableRow row in table.Rows)
         {
            xhtml.AppendLine("  " + RenderTableRow(row));
         }

         xhtml.AppendLine("</table>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="TableRow"/>.
      /// </summary>
      public string RenderTableRow(TableRow control)
      {
         string content;
         StringBuilder xhtml = new StringBuilder();

         if (control.Cells.Length > 0)
         {
            xhtml.AppendLine("  <tr " + control.GetIdParameter() + ">");
         }

         foreach (TableCell cell in control.Cells)
         {
            // Determina si debe renderizar el contenido de la celda
            if (typeof(IControl).IsAssignableFrom(cell.Value.GetType()))
            {
               content = Render((IControl)cell.Value);
            }
            else
            {
               content = cell.Value.ToString();
            }

            // Determina si es una celda normal o de cabecera
            if (control.IsHeader)
            {
               xhtml.AppendLine("<th>" + content + "</th>");
            }
            else
            {
               xhtml.AppendLine("<td>" + content + "</td>");
            }
         }

         if (control.Cells.Length > 0)
         {
            xhtml.AppendLine("  </tr>");
         }

         return xhtml.ToString();
      }

      #endregion

      #region Sidebar Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="Sidebar"/>.
      /// </summary>
      private string RenderSidebar(Sidebar sidebar)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<section class=\"sidebar\">");
         /*
                    <!-- search form -->
                    <form action="#" method="get" class="sidebar-form">
                        <div class="input-group">
                            <input type="text" name="q" class="form-control" placeholder="Buscar..."/>
                            <span class="input-group-btn">
                                <button type='submit' name='seach' id='search-btn' class="btn btn-flat"><i class="fa fa-search"></i></button>
                            </span>
                        </div>
                    </form>
                    <!-- /.search form -->
          */
         xhtml.AppendLine("  <ul class=\"sidebar-menu\">");
         foreach (SidebarButton btn in sidebar.Buttons)
         {
            xhtml.AppendLine(RenderSidebarButton(btn));
         }
/*                        <li class="active">
                            <a href="home.html">
                                <i class="fa fa-home"></i> <span>Inicio</span>
                            </a>
                        </li>
                        <li>
                            <a href="docfolder.html">
                                <i class="fa fa-flag"></i> <span>Novedades</span> <small class="badge pull-right bg-green">12</small>
                            </a>
                        </li>
                        <li class="treeview">
                            <a href="#">
                                <i class="fa fa-book"></i>
                                <span>Artículos</span>
                                <i class="fa fa-angle-left pull-right"></i>
                            </a>
                            <ul class="treeview-menu">
                                <li><a href="docfolder.html"><i class="fa fa-angle-double-right"></i> Tren real</a></li>
                                <li><a href="docfolder.html"><i class="fa fa-angle-double-right"></i> Modelismo</a></li>
                                <li><a href="docfolder.html"><i class="fa fa-angle-double-right"></i> Software</a></li>
                            </ul>
                        </li>
                        <li class="treeview">
                            <a href="#">
                                <i class="fa fa-camera"></i>
                                <span>Fotos</span>
                                <i class="fa fa-angle-left pull-right"></i>
                            </a>
                            <ul class="treeview-menu">
                                <li><a href="pages/UI/general.html"><i class="fa fa-angle-double-right"></i> General</a></li>
                                <li><a href="pages/UI/icons.html"><i class="fa fa-angle-double-right"></i> Icons</a></li>
                                <li><a href="pages/UI/buttons.html"><i class="fa fa-angle-double-right"></i> Buttons</a></li>
                                <li><a href="pages/UI/sliders.html"><i class="fa fa-angle-double-right"></i> Sliders</a></li>
                                <li><a href="pages/UI/timeline.html"><i class="fa fa-angle-double-right"></i> Timeline</a></li>
                            </ul>
                        </li>
                        <li class="treeview">
                            <a href="#">
                                <i class="fa fa-comments"></i> <span>Foros</span>
                                <i class="fa fa-angle-left pull-right"></i>
                            </a>
                            <ul class="treeview-menu">
                                <li><a href="pages/forms/general.html"><i class="fa fa-angle-double-right"></i> General</a></li>
                                <li><a href="pages/forms/advanced.html"><i class="fa fa-angle-double-right"></i> Tren real</a></li>
                                <li><a href="pages/forms/editors.html"><i class="fa fa-angle-double-right"></i> Modelismo</a></li>
								<li><a href="pages/forms/editors.html"><i class="fa fa-angle-double-right"></i> Digital / Electrónica</a></li>
                            </ul>
                        </li>
                        <li class="treeview">
                            <a href="#">
                                <i class="ion ion-bag"></i> <span>Clasificados</span>
                                <i class="fa fa-angle-left pull-right"></i>
                            </a>
                            <ul class="treeview-menu">
                                <li><a href="pages/tables/simple.html"><i class="fa fa-angle-double-right"></i> Compro H0</a></li>
                                <li><a href="pages/tables/data.html"><i class="fa fa-angle-double-right"></i> Compro H0 (Märklin)</a></li>
								<li><a href="pages/tables/data.html"><i class="fa fa-angle-double-right"></i> Compro TT/N/Z</a></li>
								<li><a href="pages/tables/data.html"><i class="fa fa-angle-double-right"></i> Compro 0/I/II</a></li>
								<li><a href="pages/tables/data.html"><i class="fa fa-angle-double-right"></i> Vendo</a></li>
                            </ul>
                        </li>*/
         xhtml.AppendLine("  </ul>");
         xhtml.AppendLine("</section>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="SidebarButton"/>.
      /// </summary>
      private string RenderSidebarButton(SidebarButton sidebarBtn)
      {
         StringBuilder xhtml = new StringBuilder();
         string css = string.Empty;

         if (sidebarBtn.SubItems.Count > 0) css += "treeview ";
         if (sidebarBtn.Active) css += "active";

         xhtml.AppendLine("    <li" + (!string.IsNullOrEmpty(css) ? " class=\"" + css + "\"" : string.Empty) + ">");
         xhtml.AppendLine("      <a href=\"" + sidebarBtn.Href + "\">");
         if (!string.IsNullOrWhiteSpace(sidebarBtn.Icon)) xhtml.AppendLine("<i class=\"fa " + sidebarBtn.Icon + "\"></i>");
         xhtml.AppendLine("        <span>" + sidebarBtn.Caption + "</span>");
         if (!string.IsNullOrWhiteSpace(sidebarBtn.BadgeText)) xhtml.AppendLine("<small class=\"badge pull-right bg-green\">" + sidebarBtn.BadgeText + "</small>");
         if (sidebarBtn.SubItems.Count > 0) xhtml.AppendLine("<i class=\"fa fa-angle-left pull-right\"></i>");
         xhtml.AppendLine("      </a>");
         if (sidebarBtn.SubItems.Count > 0)
         {
            xhtml.AppendLine("      <ul class=\"treeview-menu\">");
            foreach (SidebarButton btn in sidebarBtn.SubItems)
            {
               xhtml.AppendLine(RenderSidebarButton(btn));
            }
            xhtml.AppendLine("      </ul>");
         }
         xhtml.AppendLine("    </li>");

         return xhtml.ToString();
      }

      #endregion

      #region PageHeader

      private string RenderPageHeader(PageHeader pageHeader)
      {
         StringBuilder xhtml = new StringBuilder();

         // xhtml.AppendLine("<section class=\"content-header\">");
         xhtml.AppendLine("  <h1>");
         xhtml.AppendLine(((!string.IsNullOrWhiteSpace(pageHeader.Icon) ? Glyphicon.GetIcon(pageHeader.Icon) : string.Empty) + "&nbsp;"));
         xhtml.AppendLine(HttpUtility.HtmlDecode(pageHeader.Title));
         if (!string.IsNullOrWhiteSpace(pageHeader.SubTitle))
         {
            xhtml.AppendLine("<small>" + HttpUtility.HtmlDecode(pageHeader.SubTitle) + "</small>");
         }
         xhtml.AppendLine("  </h1>");

         if (pageHeader.Breadcrumb != null)
         {
            xhtml.AppendLine(Render(pageHeader.Breadcrumb));
         }

         // xhtml.AppendLine("</section>");

         /*
                    <ol class="breadcrumb">
                        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
                        <li class="active">Dashboard</li>
                    </ol>
                </section>
         */

         return xhtml.ToString();
      }

      #endregion

      #region DocumentHeader Control

      /// <summary>
      /// Renderiza un control del tipo <see cref="DocumentHeader"/>.
      /// </summary>
      private string RenderDocumentHeader(DocumentHeader docHead)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<h4 class=\"page-header\">");
         xhtml.AppendLine(HttpUtility.HtmlDecode(docHead.Title));
         xhtml.AppendLine("<small>" + HttpUtility.HtmlDecode(docHead.SubTitle) + "</small>");
         xhtml.AppendLine("</h4>");

         return xhtml.ToString();
      }

      #endregion

      #region TreeView Control

      /// <summary>
      /// Renderiza controles de tipo <see cref="TreeView"/>.
      /// </summary>
      private string RenderTreeView(TreeView treeView)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"box box-danger\">");
         xhtml.AppendLine("  <div class=\"box-header\">");
         if (!string.IsNullOrWhiteSpace(treeView.Icon)) xhtml.AppendLine("    " + Glyphicon.GetIcon(treeView.Icon));
         if (!string.IsNullOrWhiteSpace(treeView.Caption)) xhtml.AppendLine("    <h3 class=\"box-title\">" + HttpUtility.HtmlDecode(treeView.Caption) + "</h3>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("  <div class=\"box-body\">");

         if (!string.IsNullOrWhiteSpace(treeView.Description)) xhtml.AppendLine("    <p>" + treeView.Description + "</p>");

         xhtml.AppendLine("    <div class=\"tree\">");
         xhtml.AppendLine("      <ul role=\"tree\">");

         foreach (TreeViewChildItem child in treeView.ChildItems)
         {
            xhtml.AppendLine(RenderTreeViewChils(child, treeView.Collapsed));
         }

         xhtml.AppendLine("      </ul>");
         xhtml.AppendLine("    </div>");

         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un nodo de un control <see cref="TreeView"/>.
      /// </summary>
      private string RenderTreeViewChils(TreeViewChildItem childItem, bool collapsed)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<li class=\"parent_li\" role=\"treeitem\">");
         xhtml.AppendLine("  <span class=\"clickable" + (childItem.Type != ComponentColorScheme.Normal ? " badge badge-" + IControl.ColorSchemeToString(childItem.Type) : string.Empty) + "\">");

         if (childItem.ChildItems.Count > 0)
         {
            if (!string.IsNullOrWhiteSpace(childItem.Href)) xhtml.AppendLine("<a onlick=\"\">");
         }
         else
         {
            if (!string.IsNullOrWhiteSpace(childItem.Href)) xhtml.AppendLine("<a href=\"" + childItem.Href + "\">");
         }
         
         if (!string.IsNullOrWhiteSpace(childItem.Icon)) xhtml.AppendLine(Glyphicon.GetIcon(childItem.Icon));
         xhtml.AppendLine(childItem.Caption);

         /*if (childItem.ChildItems.Count > 0)
         {
            // if (!string.IsNullOrWhiteSpace(childItem.Href)) xhtml.AppendLine("<a href=\"#\">");
         }
         else
         {*/
            if (!string.IsNullOrWhiteSpace(childItem.Href)) xhtml.AppendLine("</a>");
         //}
         
         xhtml.AppendLine("  </span>"); 

         if (childItem.ChildItems.Count > 0)
         {
            xhtml.AppendLine("    <ul role=\"group\"" + (collapsed ? " style=\"display:none;\"" : string.Empty) + ">");
            foreach (TreeViewChildItem child in childItem.ChildItems)
            {
               xhtml.AppendLine(RenderTreeViewChils(child, collapsed));
            }
            xhtml.AppendLine("    </ul>");
         }

         xhtml.AppendLine("</li>");

         return xhtml.ToString();
      }

      #endregion

      #region LoginFormControl

      /// <summary>
      /// Renderiza un formulario de login.
      /// </summary>
      private string RenderLoginForm(LoginForm loginForm)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div id=\"" + loginForm.DomID + "\" class=\"form-box\">");
         xhtml.AppendLine("  <div class=\"header\">Sign In</div>");
         xhtml.AppendLine("    <form id=\"" + loginForm.DomID + "-form\" role=\"form\">");
         xhtml.AppendLine("      <input type=\"hidden\" id=\"" + Workspace.PARAM_COMMAND + "\" name=\"" + Workspace.PARAM_COMMAND + "\" value=\"" + UserHandler.COMMAND_USER_AUTHENTICATION + "\" />");
         xhtml.AppendLine("      <input type=\"hidden\" id=\"" + Workspace.PARAM_LOGIN_REDIRECT + "\" name=\"" + Workspace.PARAM_LOGIN_REDIRECT + "\" value=\"" + (string.IsNullOrEmpty(loginForm.RedirectionUrl) ? Workspace.COSMO_URL_DEFAULT : loginForm.RedirectionUrl) + "\" />");
         xhtml.AppendLine("      <div id=\"" + loginForm.DomID + "-msg\" class=\"body\"></div>");
         xhtml.AppendLine("      <div class=\"body bg-gray\">");
         xhtml.AppendLine("        <div class=\"form-group\">");
         xhtml.AppendLine("          <input type=\"text\" name=\"txtLogin\" class=\"form-control\" placeholder=\"User ID\"/>");
         xhtml.AppendLine("        </div>");
         xhtml.AppendLine("        <div class=\"form-group\">");
         xhtml.AppendLine("          <input type=\"password\" name=\"txtPwd\" class=\"form-control\" placeholder=\"Password\"/>");
         xhtml.AppendLine("        </div>");
         xhtml.AppendLine("      </div>");
         xhtml.AppendLine("      <div class=\"footer\">");
         xhtml.AppendLine("        <button type=\"button\" class=\"btn bg-olive\" id=\"btn-login\">Sign me in</button>");
         xhtml.AppendLine("        <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Cerrar</button>");
         if (!string.IsNullOrWhiteSpace(loginForm.UserRememberPasswordUrl)) xhtml.AppendLine("        <p><a href=\"" + loginForm.UserRememberPasswordUrl + "\">¿Has olvidado tu contraseña?</a></p>");
         if (!string.IsNullOrWhiteSpace(loginForm.UserJoinUrl)) xhtml.AppendLine("        <p><a href=\"" + loginForm.UserJoinUrl + "\" class=\"text-center\">Crear una nueva cuenta</a></p>");
         xhtml.AppendLine("      </div>");
         xhtml.AppendLine("    </form>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         xhtml.AppendLine("<script type=\"text/javascript\">");
         xhtml.AppendLine("  $(\"#btn-login\").click(function() {");
         xhtml.AppendLine("    $(\"#" + loginForm.DomID + "-form\").submit(function(e) {");
         xhtml.AppendLine("      var postData = $(this).serializeArray();");
         xhtml.AppendLine("      $.ajax({");
         xhtml.AppendLine("        url     : \"users.do\",");
         xhtml.AppendLine("        type    : \"POST\",");
         xhtml.AppendLine("        data    : postData,");
         xhtml.AppendLine("        success : function(data, textStatus, jqXHR) {");
         xhtml.AppendLine("                    if (data.response == 'ok') {");
         xhtml.AppendLine("                      $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-success\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-check\"></i>Autenticación correcta</div>');");
         xhtml.AppendLine("                      window.location = data.tourl;");
         xhtml.AppendLine("                    }");
         xhtml.AppendLine("                    else if (data.response == 'err') {");
         xhtml.AppendLine("                      if (data.code == '1001') {");
         xhtml.AppendLine("                        $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-danger\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-ban\"></i>Esta cuenta actualmente no tiene acceso.</div>');");
         xhtml.AppendLine("                      } else if (data.code == '1002') {");
         xhtml.AppendLine("                        $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-warning\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-warning\"></i>Esta cuenta está pendiente de verificación y aun no tiene acceso. Revise su correo, debe tener un correo con las instrucciones para verificar esta cuenta.</div>');");
         xhtml.AppendLine("                      } else {");
         xhtml.AppendLine("                        $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-danger\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-ban\"></i>El usuario y/o la contraseña son incorrectos.</div>');");
         xhtml.AppendLine("                      }");
         xhtml.AppendLine("                    }");
         xhtml.AppendLine("                  },");
         xhtml.AppendLine("        error   : function(jqXHR, textStatus, errorThrown) {");
         xhtml.AppendLine("                     $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-danger\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-ban\"></i><strong>Ooooops!</strong> No se ha podido realizar la autenticación a causa de un error.</div>');");
         xhtml.AppendLine("                  }");
         xhtml.AppendLine("      });");
         xhtml.AppendLine("      e.preventDefault();");
         xhtml.AppendLine("      e.unbind();");
         xhtml.AppendLine("    });");
         xhtml.AppendLine("    $(\"#" + loginForm.DomID + "-form\").submit();");
         xhtml.AppendLine("  });");
         xhtml.AppendLine("</script>");

         return xhtml.ToString();
      }

      #endregion

      #region HtmlContent Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="HtmlContent"/>.
      /// </summary>
      private string RenderHtmlContent(HtmlContent control)
      {
         return control.html.ToString();
      }

      #endregion

      #region Form Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="Form"/>.
      /// </summary>
      private string RenderForm(Form form, bool postback)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"box box-primary\">");
         xhtml.AppendLine("  <div class=\"box-header\">");
         if (!string.IsNullOrWhiteSpace(form.Caption)) xhtml.AppendLine("    <h3 class=\"box-title\">" + (string.IsNullOrWhiteSpace(form.Icon) ? string.Empty : Glyphicon.GetIcon(form.Icon) + " ") + HttpUtility.HtmlDecode(form.Caption) + "</h3>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("  <form role=\"form\" method=\"" + form.Method + "\"" + (string.IsNullOrWhiteSpace(form.Action) ? string.Empty : " action=\"" + form.Action + "\"") + (form.Multipart ? " enctype=\"multipart/form-data\"" : string.Empty) + ">");

         foreach (KeyValue setting in form.Settings)
         {
            // id=\"" + setting.Label + "\" 
            xhtml.AppendLine("    <input type=\"hidden\" name=\"" + setting.Label + "\" value=\"" + setting.Value + "\" />");
         }

         xhtml.AppendLine("    <div class=\"box-body\">");
         
         foreach (IControl field in form.Content)
         {
            // Se pasa el parámetro postback para evitar que en las cargas iniciales 
            // de los formularios aparezcan los campos con error de validación
            xhtml.AppendLine(Render(field, postback));
         }
         
         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("    <div class=\"box-footer\">");

         foreach (Button button in form.FormButtons)
         {
            xhtml.AppendLine(Render(button));
         }

         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("  </form>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldText"/>.
      /// </summary>
      private string RenderFormFieldText(FormFieldText control, bool postback)
      {
         bool isValid = postback ? control.Validate() : true;
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"form-group" + (isValid ? string.Empty : " has-error") + "\">");
         xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + (isValid ? string.Empty : Glyphicon.GetIcon(Glyphicon.ICON_WARNING) + " ") + HttpUtility.HtmlDecode(control.Label) + "</label>");

         if (control.Type == FormFieldText.FieldType.Text)
         {
            xhtml.AppendLine("  <input " + control.GetIdParameter() + control.GetNameParameter() + " type=\"text\" class=\"form-control\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" value=\"" + HttpUtility.HtmlDecode(control.Value.ToString()) + "\" />");
         }
         else if (control.Type == FormFieldText.FieldType.Password)
         {
            xhtml.AppendLine("  <input " + control.GetIdParameter() + control.GetNameParameter() + " type=\"password\" class=\"form-control\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" value=\"" + HttpUtility.HtmlDecode(control.Value.ToString()) + "\" />");
         }
         else if (control.Type == FormFieldText.FieldType.Email)
         {
            xhtml.AppendLine("  <div class=\"input-group\">");
            xhtml.AppendLine("    <span class=\"input-group-addon\"><i class=\"fa fa-envelope\"></i></span>");
            xhtml.AppendLine("    <input " + control.GetIdParameter() + control.GetNameParameter() + " type=\"email\" class=\"form-control\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" value=\"" + HttpUtility.HtmlDecode(control.Value.ToString()) + "\" />");
            xhtml.AppendLine("  </div>");
         }
         else if (control.Type == FormFieldText.FieldType.Tel)
         {
            xhtml.AppendLine("  <div class=\"input-group\">");
            xhtml.AppendLine("    <span class=\"input-group-addon\"><i class=\"fa fa-phone\"></i></span>");
            xhtml.AppendLine("    <input " + control.GetIdParameter() + control.GetNameParameter() + " type=\"tel\" class=\"form-control\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" value=\"" + HttpUtility.HtmlDecode(control.Value.ToString()) + "\" />");
            xhtml.AppendLine("  </div>");
         }

         if (!string.IsNullOrWhiteSpace(control.Description))
         {
            xhtml.AppendLine("  <p class=\"help-block\">" + HttpUtility.HtmlDecode(control.Description) + "</p>");
         }

         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldEditor"/>.
      /// </summary>
      private string RenderFormFieldTextArea(FormFieldEditor control, bool postback)
      {
         bool isValid = postback ? control.Validate() : true;
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"form-group" + (isValid ? string.Empty : " has-error") + "\">");
         xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + (isValid ? string.Empty : Glyphicon.GetIcon(Glyphicon.ICON_WARNING) + " ") + HttpUtility.HtmlDecode(control.Label) + "</label>");
         xhtml.AppendLine("  <textarea " + control.GetIdParameter() + control.GetNameParameter() + " class=\"form-control\" rows=\"3\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\">" + control.Value + "</textarea>");

         if (!string.IsNullOrWhiteSpace(control.Description))
         {
            xhtml.AppendLine("  <p class=\"help-block\">" + HttpUtility.HtmlDecode(control.Description) + "</p>");
         }

         xhtml.AppendLine("</div>");

         if (control.Type == FormFieldEditor.FieldEditorType.HTML)
         {
            xhtml.AppendLine("<script type=\"text/javascript\">");
            xhtml.AppendLine("$(function() {");
            xhtml.AppendLine("  CKEDITOR.replace('" + control.DomID + "');");
            xhtml.AppendLine("});");
            xhtml.AppendLine("</script>");
         }

         return xhtml.ToString();
      }

      private const string NO_IMAGE_URL = "http://placehold.it/70x70";

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldImage"/>.
      /// </summary>
      private string RenderFormFieldImage(FormFieldImage control, bool postback)
      {
         StringBuilder xhtml = new StringBuilder();

         // TODO: Aplicar http://www.abeautifulsite.net/blog/2013/08/whipping-file-inputs-into-shape-with-bootstrap-3/
         // TODO: Evitar encolumnar la imagen y hacer el DIV dependiente de la anchura de la misma

         xhtml.AppendLine("<div class=\"form-group\">");
         xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + HttpUtility.HtmlDecode(control.Label) + "</label>");
         xhtml.AppendLine("  <div class=\"row no-margin form-control-image\">");
         xhtml.AppendLine("    <div class=\"col-md-1 no-margin no-padding\">");
         xhtml.AppendLine("      <img src=\"" + (string.IsNullOrWhiteSpace(control.PreviewUrl) ? NO_IMAGE_URL : control.PreviewUrl) + "\" alt=\"\" />");
         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("    <div class=\"col-md-11 no-margin no-padding\">");
         xhtml.AppendLine("      <input type=\"file\" " + control.GetIdParameter() + control.GetNameParameter() + " />");
         if (!string.IsNullOrWhiteSpace(control.Description)) xhtml.AppendLine("      <p class=\"help-block\">" + HttpUtility.HtmlDecode(control.Description) + "</p>");
         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         /*
         <span class="btn btn-default btn-file">
            <input type="file" class="form-control">
         </span>
         */

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldFile"/>.
      /// </summary>
      private string RenderFormFieldFile(FormFieldFile control, bool postback)
      {
         StringBuilder xhtml = new StringBuilder();

         //xhtml.AppendLine("<div class=\"form-group\">");
         //xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + HttpUtility.HtmlDecode(control.Label) + "</label>");
         //xhtml.AppendLine("  <div class=\"fileinput fileinput-new input-group\" data-provides=\"fileinput\" />");
         //xhtml.AppendLine("    <div class=\"form-control\" data-trigger=\"fileinput\"><i class=\"glyphicon glyphicon-file fileinput-exists\"></i> <span class=\"fileinput-filename\"></span></div>");
         //xhtml.AppendLine("    <span class=\"input-group-addon btn btn-default btn-file\">");
         //xhtml.AppendLine("      <span class=\"fileinput-new\">Select file</span>");
         //xhtml.AppendLine("      <span class=\"fileinput-exists\">Change</span>");
         //xhtml.AppendLine("      <input type=\"file\" " + control.GetIdParameter() + control.GetNameParameter() + " />");
         //xhtml.AppendLine("    </span>");
         //xhtml.AppendLine("    <a href=\"#\" class=\"input-group-addon btn btn-default fileinput-exists\" data-dismiss=\"fileinput\">Remove</a>");
         //if (!string.IsNullOrWhiteSpace(control.Description)) xhtml.AppendLine("      <p class=\"help-block\">" + HttpUtility.HtmlDecode(control.Description) + "</p>");
         //xhtml.AppendLine("  </div>");
         //xhtml.AppendLine("</div>");

         xhtml.AppendLine("<div class=\"form-group\">");
         xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + HttpUtility.HtmlDecode(control.Label) + "</label>");
         xhtml.AppendLine("  <input type=\"file\" " + control.GetIdParameter() + "\" />");
         if (!string.IsNullOrWhiteSpace(control.Description)) xhtml.AppendLine("      <p class=\"help-block\">" + HttpUtility.HtmlDecode(control.Description) + "</p>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldList"/>.
      /// </summary>
      private string RenderFormFieldList(FormFieldList control, bool postback)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"form-group\">");
         xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + HttpUtility.HtmlDecode(control.Label) + "</label>");
         xhtml.AppendLine("  <select " + control.GetIdParameter() + control.GetNameParameter() + " class=\"form-control\"" + (control.Type == FormFieldList.ListType.Multiple ? " multiple=\"multiple\"" : string.Empty) + ">");

         foreach (KeyValue value in control.Values)
         {
            xhtml.AppendLine("    <option" + (value.Value.Equals(control.Value) ? " selected=\"selected\"" : string.Empty) + " value=\"" + value.Value + "\">" + HttpUtility.HtmlDecode(value.Label) + "</option>");
         }

         xhtml.AppendLine("  </select>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldBoolean"/>.
      /// </summary>
      private string RenderFormFieldBoolean(FormFieldBoolean control, bool postback)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"form-group\">");
         xhtml.AppendLine("  <div class=\"checkbox\">");
         xhtml.AppendLine("    <label>");
         xhtml.AppendLine("      <input " + control.GetIdParameter() + control.GetNameParameter() + " type=\"checkbox\" value=\"1\" />&nbsp; ");
         xhtml.AppendLine("      " + HttpUtility.HtmlDecode(control.Label));
         xhtml.AppendLine("    </label>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region TabbedContainer Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="TabbedContainer"/>.
      /// </summary>
      private string RenderTabbedContainer(TabbedContainer control)
      {
         bool first;
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"nav-tabs-custom\">");
         xhtml.AppendLine("  <ul class=\"nav nav-tabs\">");

         first = true;
         foreach (TabItem tab in control.Tabs)
         {
            xhtml.AppendLine("    <li" + (first ? " class=\"active\"" : string.Empty) + "><a href=\"#" + tab.DomID + "\" data-toggle=\"tab\">" + HttpUtility.HtmlDecode(tab.Caption) + "</a></li>");
            first = false;
         }
         
         // <li class="pull-right"><a href="#" class="text-muted"><i class="fa fa-gear"></i></a></li>
         xhtml.AppendLine("  </ul>");
         xhtml.AppendLine("  <div class=\"tab-content\">");

         first = true;
         foreach (TabItem tab in control.Tabs)
         {
            xhtml.AppendLine("    <div class=\"tab-pane" + (first ? " active" : string.Empty) + "\" " + tab.GetIdParameter() + ">");
            foreach (IControl ctrl in tab.Content)
            {
               xhtml.AppendLine(Render(ctrl));
            }
            xhtml.AppendLine("    </div>");
            first = false;
         }

         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region ModalForm Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="ModalForm"/>.
      /// </summary>
      private string RenderModalForm(ModalForm control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"modal fade\" " + control.GetIdParameter() + " tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"" + control.DomID + "-label\" aria-hidden=\"true\">");
         xhtml.AppendLine("  <div class=\"modal-dialog\">");
         xhtml.AppendLine("    <div class=\"modal-content\">");
         xhtml.AppendLine("      <div class=\"modal-header\">");
         if (control.Closeable)
         {
            xhtml.AppendLine("        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>");
         }
         xhtml.AppendLine("        <h4 class=\"modal-title\" id=\"" + control.DomID + "-label\">" + HttpUtility.HtmlDecode(control.Title) + "</h4>");
         xhtml.AppendLine("      </div>");
         xhtml.AppendLine("      <div class=\"modal-body\">");

         foreach (IControl ctrl in control.Content)
         {
            xhtml.AppendLine(Render(ctrl));
         }

         xhtml.AppendLine("      </div>");
         xhtml.AppendLine("      <div class=\"modal-footer\">");
         xhtml.AppendLine("        <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Close</button>");
         xhtml.AppendLine("        <button type=\"button\" class=\"btn btn-primary\">Save changes</button>");
         xhtml.AppendLine("      </div>");
         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region Badge Control

      /// <summary>
      /// Renderiza controles de tipo <see cref="Badge"/>.
      /// </summary>
      private string RenderBadge(Badge control)
      {
         StringBuilder xhtml = new StringBuilder();

         if (control.RoundedBorders)
         {
            xhtml.AppendLine("<span class=\"badge bg-" + IControl.ColorSchemeToString(control.Type) + "\">" + control.Text + "</span>");
         }
         else
         {
            xhtml.AppendLine("<span class=\"label label-" + IControl.ColorSchemeToString(control.Type) + "\">" + control.Text + "</span>");
         }

         return xhtml.ToString();
      }

      #endregion

      #region ProgressBar Control

      /// <summary>
      /// Renderiza controles de tipo <see cref="ProgressBar"/>.
      /// </summary>
      private string RenderProgressBar(ProgressBar control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"progress xs\">");
         xhtml.AppendLine("  <div class=\"progress-bar progress-bar-" + IControl.ColorSchemeToString(control.Color) + "\" style=\"width: " + control.Percentage + "%\"></div>");
         xhtml.AppendLine("</div>");

         if (!string.IsNullOrWhiteSpace(control.Description))
         {
            xhtml.AppendLine("<small>" + HttpUtility.HtmlDecode(control.Description) + "</small>");
         }

         return xhtml.ToString();
      }

      #endregion

      #region Timeline Control

      /// <summary>
      /// Renderiza controles de tipo <see cref="Timeline"/>.
      /// </summary>
      private string RenderTimeline(Timeline control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"box box-primary\">");
         xhtml.AppendLine("  <div class=\"box-header\">");
         xhtml.AppendLine("    <h3 class=\"box-title\">" + HttpUtility.HtmlDecode(control.Title) + "</h3>");
         // xhtml.AppendLine("       <div class=\"box-tools pull-right\">");
         // xhtml.AppendLine("           <button class=\"btn btn-primary btn-xs\" data-widget=\"collapse\"><i class=\"fa fa-minus\"></i></button>
         // xhtml.AppendLine("           <button class=\"btn btn-primary btn-xs\" data-widget=\"remove\"><i class=\"fa fa-times\"></i></button>
         // xhtml.AppendLine("       </div>");
         xhtml.AppendLine("   </div>");
         xhtml.AppendLine("   <div class=\"box-body\">");
         xhtml.AppendLine("    <ul class=\"timeline\">");
         xhtml.AppendLine("      <li class=\"time-label\"><span class=\"bg-red\">10 Feb. 2014</span></li>");

         foreach (TimelineItem item in control.Items)
         {
            xhtml.AppendLine("   <li>");
            xhtml.AppendLine("      <i class=\"fa " + item.Icon + " bg-blue\"></i>");
            xhtml.AppendLine("      <div class=\"timeline-item\">");
            xhtml.AppendLine("         <span class=\"time\"><i class=\"fa fa-clock-o\"></i> " + item.Time + "</span>");
            xhtml.AppendLine("         <h3 class=\"timeline-header\">" + item.Title + "</h3>");
            xhtml.AppendLine("         <div class=\"timeline-body\">");
            xhtml.AppendLine(item.Body);
            xhtml.AppendLine("         </div>");
            // xhtml.AppendLine("         <div class='timeline-footer'>");
            // xhtml.AppendLine("            <a class=\"btn btn-primary btn-xs\">...</a>");
            // xhtml.AppendLine("         </div>");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("   </li>");
         }

         xhtml.AppendLine("    </ul>");
         xhtml.AppendLine("   </div>");
         xhtml.AppendLine("   <div class=\"box-footer\">");
         xhtml.AppendLine("       <code>.box-footer</code>");
         xhtml.AppendLine("   </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region Error Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="Error"/>.
      /// </summary>
      private string RenderError(Error control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"box box-solid bg-red\">");
         xhtml.AppendLine("  <div class=\"box-header\">");
         xhtml.AppendLine("    <h3 class=\"box-title\">ERROR 500 | Alguna cosa ha ido mal...</h3>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("  <div class=\"box-body\">");
         xhtml.AppendLine("    <p>Al procesar su petición al servidor se ha producido un error y no ha sido posible completarla. El error ha sido registrado para su revisión.</p>");
         xhtml.AppendLine("    <p>Puede intentar lo siguiente:</p>");
         xhtml.AppendLine("    <ul>");
         xhtml.AppendLine("      <li>Si es una página de visualización, vuelva a intentar recargando la página.</li>");
         xhtml.AppendLine("      <li>Si se ha producido al enviar datos de un formulario, vuelva atras y revise que todos los campos sean correctos.</li>");
         xhtml.AppendLine("    </ul>");
         xhtml.AppendLine("    <hr />");
         xhtml.AppendLine("    <p>A continuación se muestran detalles técnicos del error.</p>");
         xhtml.AppendLine("    <pre style=\"font-weight: 600;\">");
         xhtml.AppendLine("Mensaje de error: " + control.Exception.Message + "<br /><br />");
         xhtml.AppendLine(control.Exception.StackTrace);
         xhtml.AppendLine("    </pre>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

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
