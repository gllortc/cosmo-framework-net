using Cosmo.REST;
using Cosmo.UI.Controls;
using Cosmo.UI.Scripting;
using Cosmo.Utils;
using Cosmo.Utils.Html;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Cosmo.UI.Render.Impl
{
   /// <summary>
   /// Implementa un módulo de renderizado para AdminLTE versión 1.2.1
   /// </summary>
   /// <remarks>
   /// https://github.com/almasaeed2010/AdminLTE/releases/tag/1.2.1
   /// </remarks>
   public class AdminLteRenderModuleImpl : RenderModule
   {

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="AdminLteRenderModuleImpl"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      public AdminLteRenderModuleImpl(Workspace workspace, Plugin plugin)
         : base(workspace, plugin) 
      { }

      #endregion

      #region RenderModule Implementation

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
      /// <param name="receivedFormID">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string Render(Controls.Control control, string receivedFormID)
      {
         if (control.GetType() == typeof(LayoutContainerControl))
         {
            return RenderLayout((LayoutContainerControl)control, receivedFormID);
         }
         else if (control.GetType() == typeof(IconControl))
         {
            return RenderIcon((IconControl)control);
         }
         else if (control.GetType() == typeof(AlertControl))
         {
            return RenderAlert((AlertControl)control);
         }
         else if (control.GetType() == typeof(BreadcrumbControl))
         {
            return RenderBreadcrumb((BreadcrumbControl)control);
         }
         else if (control.GetType() == typeof(ButtonGroupControl))
         {
            return RenderButtonBar((ButtonGroupControl)control);
         }
         else if (control.GetType() == typeof(ButtonControl))
         {
            return RenderButton((ButtonControl)control);
         }
         else if (control.GetType() == typeof(ButtonSplit))
         {
            return RenderButtonSplit((ButtonSplit)control);
         }
         else if (control.GetType() == typeof(CalloutControl))
         {
            return RenderCallout((CalloutControl)control);
         }
         else if (control.GetType() == typeof(CarrouselControl))
         {
            return RenderCarrousel((CarrouselControl)control);
         }
         else if (control.GetType() == typeof(ChatControl))
         {
            return RenderChat((ChatControl)control);
         }
         else if (control.GetType() == typeof(JumbotronControl))
         {
            return RenderJumbotron((JumbotronControl)control);
         }
         else if (control.GetType() == typeof(ListGroupControl))
         {
            return RenderListGroup((ListGroupControl)control);
         }
         else if (control.GetType() == typeof(MediaListControl))
         {
            return RenderMediaList((MediaListControl)control);
         }
         else if (control.GetType() == typeof(NavbarControl))
         {
            return RenderNavbar((NavbarControl)control);
         }
         else if (control.GetType() == typeof(PanelControl))
         {
            return RenderPanel((PanelControl)control);
         }
         else if (control.GetType() == typeof(PopoverControl))
         {
            return RenderPopover((PopoverControl)control);
         }
         else if (control.GetType() == typeof(TableControl))
         {
            return RenderTable((TableControl)control);
         }
         else if (control.GetType() == typeof(SidebarControl))
         {
            return RenderSidebar((SidebarControl)control);
         }
         else if (control.GetType() == typeof(SidebarButton))
         {
            return RenderSidebarButton((SidebarButton)control);
         }
         else if (control.GetType() == typeof(PageHeaderControl))
         {
            return RenderPageHeader((PageHeaderControl)control);
         }
         else if (control.GetType() == typeof(DocumentHeaderControl))
         {
            return RenderDocumentHeader((DocumentHeaderControl)control);
         }
         else if (control.GetType() == typeof(TreeViewControl))
         {
            return RenderTreeView((TreeViewControl)control);
         }
         else if (control.GetType() == typeof(HtmlContentControl))
         {
            return RenderHtmlContent((HtmlContentControl)control);
         }
         else if (control.GetType() == typeof(FormControl))
         {
            string recvFrm = (((FormControl)control).DomID == receivedFormID) ? receivedFormID : string.Empty;
            return RenderForm((FormControl)control, recvFrm);
         }
         else if (control.GetType() == typeof(FormFieldHidden))
         {
            return RenderFormFieldHidden((FormFieldHidden)control, receivedFormID);
         }
         else if (control.GetType() == typeof(FormFieldText))
         {
            return RenderFormFieldText((FormFieldText)control, receivedFormID);
         }
         else if (control.GetType() == typeof(FormFieldPassword))
         {
            return RenderFormFieldPassword((FormFieldPassword)control, receivedFormID);
         }
         else if (control.GetType() == typeof(FormFieldEditor))
         {
            return RenderFormFieldEditor((FormFieldEditor)control, receivedFormID);
         }
         else if (control.GetType() == typeof(FormFieldList))
         {
            return RenderFormFieldList((FormFieldList)control, receivedFormID);
         }
         else if (control.GetType() == typeof(FormFieldBoolean))
         {
            return RenderFormFieldBoolean((FormFieldBoolean)control, receivedFormID);
         }
         else if (control.GetType() == typeof(FormFieldDate))
         {
            return RenderFormFieldDate((FormFieldDate)control, receivedFormID);
         }
         else if (control.GetType() == typeof(FormFieldImage))
         {
            return RenderFormFieldImage((FormFieldImage)control, receivedFormID);
         }
         else if (control.GetType() == typeof(FormFieldFile))
         {
            return RenderFormFieldFile((FormFieldFile)control, receivedFormID);
         }
         else if (control.GetType() == typeof(FormFieldCaptcha))
         {
            return RenderFormFieldCaptcha((FormFieldCaptcha)control, receivedFormID);
         }
         else if (control.GetType() == typeof(LoginFormControl))
         {
            return RenderLoginForm((LoginFormControl)control);
         }
         else if (control.GetType() == typeof(TabbedContainerControl))
         {
            return RenderTabbedContainer((TabbedContainerControl)control, receivedFormID);
         }
         else if (control.GetType() == typeof(ErrorControl))
         {
            return RenderError((ErrorControl)control);
         }
         else if (typeof(BadgeControl).IsAssignableFrom(control.GetType()))
         {
            return RenderBadge((BadgeControl)control);
         }
         else if (typeof(PaginationControl).IsAssignableFrom(control.GetType()))
         {
            return RenderPagination((PaginationControl)control);
         }
         else if (typeof(ProgressBarControl).IsAssignableFrom(control.GetType()))
         {
            return RenderProgressBar((ProgressBarControl)control);
         }
         else if (typeof(TimelineControl).IsAssignableFrom(control.GetType()))
         {
            return RenderTimeline((TimelineControl)control);
         }
         else if (typeof(PictureControl).IsAssignableFrom(control.GetType()))
         {
            return RenderPicture((PictureControl)control);
         }
         else if (typeof(PictureGalleryControl).IsAssignableFrom(control.GetType()))
         {
            return RenderPictureGallery((PictureGalleryControl)control);
         }
         else if (typeof(CookiesAdvisorControl).IsAssignableFrom(control.GetType()))
         {
            return RenderCookiesAdvisor((CookiesAdvisorControl)control);
         }

         // throw new ControlNotSuportedException("No se puede renderizar el control de tipo " + control.GetType().AssemblyQualifiedName);

         StringBuilder xhtml = new StringBuilder();
         xhtml.AppendLine("<div class=\"alert alert-warning alert-dismissable\">");
         xhtml.AppendLine("  <i class=\"fa fa-warning\"></i>");
         xhtml.AppendLine("  <b>Render Error</b>: No ha sido posible renderizar el control de tipo <code>" + control.GetType().FullName + "</code>.");
         xhtml.AppendLine("</div>");
         return xhtml.ToString();

      }

      /// <summary>
      /// Renderiza un control.
      /// </summary>
      /// <param name="control">Instancia del control a renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string Render(Control control)
      {
         return Render(control, string.Empty);
      }

      /// <summary>
      /// Renderiza una página.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="PageViewContainer"/> que representa la instancia renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string RenderPage(PageViewContainer container)
      {
         return RenderPage(container, string.Empty);
      }

      /// <summary>
      /// Renderiza una página.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="PageViewContainer"/> que representa la instancia renderizar.</param>
      /// <param name="receivedFormID">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string RenderPage(PageViewContainer container, string receivedFormID)
      {
         StringBuilder xhtml = new StringBuilder();
         StringBuilder js = new StringBuilder();

         xhtml.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
         xhtml.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
         xhtml.AppendLine("<head>");
         xhtml.AppendLine("  <title>" + (string.IsNullOrEmpty(container.Title) ? Workspace.Name : Workspace.Name + " - " + container.Title) + "</title>");
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
         xhtml.AppendLine("  <meta http-equiv=\"description\" content=\"" + (string.IsNullOrEmpty(container.Description) ? Workspace.PageDescription : container.Description) + "\" />");
         xhtml.AppendLine("  <meta http-equiv=\"keywords\" content=\"" + (string.IsNullOrEmpty(container.Keywords) ? Workspace.PageKeywords : container.Keywords) + "\" />");
         xhtml.AppendLine("  <meta http-equiv=\"device-type\" content=\"" + container.DeviceType.ToString() + "\" />");

         foreach (string css in this.CssResources)
         {
            xhtml.AppendLine("  <link href=\"" + css + "\" rel=\"stylesheet\" type=\"text/css\" />");
         }

         foreach (string jsr in this.JavascriptResources)
         {
            xhtml.AppendLine("  <script type=\"text/javascript\" src=\"" + jsr + "\"></script>");
         }

         foreach (ViewResource resource in container.Resources)
         {
            if (resource.Type == ViewResource.ResourceType.CSS)
            {
               xhtml.AppendLine("  <link href=\"" + resource.FilePath + "\" rel=\"stylesheet\" type=\"text/css\" />");
            }
            else if (resource.Type == ViewResource.ResourceType.JavaScript)
            {
               xhtml.AppendLine("  <script type=\"text/javascript\" src=\"" + resource.FilePath + "\"></script>");
            }
         }

         xhtml.AppendLine("</head>");
         xhtml.AppendLine("<body class=\"" + (container.Layout.FadeBackground ? "bg-black" : (string.IsNullOrWhiteSpace(Skin) ? "skin-blue" : Skin) + " fixed") + "\">");

         // Renderiza controles de página
         xhtml.AppendLine(Render(container.Layout, receivedFormID));

         // Renderiza las ventanas modales
         xhtml.AppendLine(RenderModalViews(container, container.Modals));

         // Renderiza scripts
         xhtml.AppendLine(RenderScripts(container));

         xhtml.AppendLine("</body>");
         xhtml.AppendLine("</html>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza una vista parcial.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="PartialViewContainer"/> que representa la instancia renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string RenderPage(PartialViewContainer container)
      {
         return RenderPage(container, string.Empty);
      }

      /// <summary>
      /// Renderiza una vista parcial.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="PartialViewContainer"/> que representa la instancia renderizar.</param>
      /// <param name="receivedFormID">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string RenderPage(PartialViewContainer container, string receivedFormID)
      {
         StringBuilder xhtml = new StringBuilder();
         StringBuilder js = new StringBuilder();

         foreach (ViewResource resource in container.Resources)
         {
            if (resource.Type == ViewResource.ResourceType.JavaScript)
            {
               xhtml.AppendLine("  <script type=\"text/javascript\" src=\"" + resource.FilePath + "\"></script>");
            }
         }

         // Renderiza controles de página
         xhtml.AppendLine(Render(container.Content, receivedFormID));

         // Renderiza las ventanas modales
         xhtml.AppendLine(RenderModalViews(container, container.Modals));

         // Renderiza scripts
         xhtml.AppendLine(RenderScripts(container));

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza una vista modal.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="ModalViewContainer"/> que representa la instancia renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string RenderPage(ModalViewContainer container)
      {
         return RenderPage(container, string.Empty);
      }

      /// <summary>
      /// Renderiza una vista modal.
      /// </summary>
      /// <param name="container">Una instancia de <see cref="ModalViewContainer"/> que representa la instancia renderizar.</param>
      /// <param name="receivedFormID">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public override string RenderPage(ModalViewContainer container, string receivedFormID)
      {
         StringBuilder xhtml = new StringBuilder();
         StringBuilder js = new StringBuilder();

         foreach (ViewResource resource in container.Resources)
         {
            if (resource.Type == ViewResource.ResourceType.JavaScript)
            {
               xhtml.AppendLine("  <script type=\"text/javascript\" src=\"" + resource.FilePath + "\"></script>");
            }
         }

         // Genera la cabecera de la ventana modal
         xhtml.AppendLine("<div class=\"modal-content\">");
         xhtml.AppendLine("  <div class=\"modal-header\">");
         if (container.Closeable)
         {
            xhtml.AppendLine("    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>");
         }
         xhtml.AppendLine("    <h4 class=\"modal-title\">");
         if (!string.IsNullOrWhiteSpace(container.Icon)) xhtml.AppendLine(IconControl.GetIcon(container, container.Icon) + "&nbsp;&nbsp;");
         xhtml.AppendLine(HttpUtility.HtmlDecode(container.Title) + "&nbsp;");
         xhtml.AppendLine("    </h4>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("  <div class=\"modal-body\">");

         // Renderiza controles de página
         xhtml.AppendLine(Render(container.Content, receivedFormID));

         // Genera el pie de la ventana modal
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         // Renderiza scripts
         xhtml.AppendLine(RenderScripts(container));

         return xhtml.ToString();
      }

      #endregion

      #region Methods

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
         /*
         // Renderiza formularios modales
         foreach (IModalForm modal in template.ModalForms)
         {
            xhtml.AppendLine(Render(modal));
         }
         */
         // Renderiza scripts
         xhtml.AppendLine(RenderScripts(template));

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza los scripts de la página.
      /// </summary>
      public string RenderScripts(ViewContainer viewport)
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
            if (scr.ExecutionType != Script.ScriptExecutionMethod.OnDocumentReady)
            {
               js.AppendLine(scr.GetSource());
            }
         }

         // Scripts dependientes de la carga de la página
         js.AppendLine("  $( document ).ready(function() {");
         foreach (Script scr in viewport.Scripts)
         {
            if (scr.ExecutionType == Script.ScriptExecutionMethod.OnDocumentReady)
            {
               js.AppendLine(scr.GetSource());
            }
         }
         js.AppendLine("  });");

         js.AppendLine("</script>");

         return js.ToString();
      }

      #endregion

      #region Render Functions

      #region Alert Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="AlertControl"/>.
      /// </summary>
      private string RenderAlert(AlertControl alert)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + alert.GetIdParameter() + "class=\"alert alert-" + GetCssClassFromControlColor(alert.Type) + " " + (alert.Closeable ? string.Empty : "alert-dismissable") + "\">");
         xhtml.AppendLine("  <i class=\"fa fa-" + GetCssClassFromControlColor(alert.Type) + "\"></i>");
         if (alert.Closeable) xhtml.AppendLine("  <button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>");
         xhtml.AppendLine("  " + alert.Text);
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region Badge Control

      /// <summary>
      /// Renderiza controles de tipo <see cref="BadgeControl"/>.
      /// </summary>
      private string RenderBadge(BadgeControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         if (control.RoundedBorders)
         {
            xhtml.AppendLine("<span class=\"badge bg-" + GetCssClassFromControlColor(control.Type) + "\">" + control.Text + "</span>");
         }
         else
         {
            xhtml.AppendLine("<span class=\"label label-" + GetCssClassFromControlColor(control.Type) + "\">" + control.Text + "</span>");
         }

         return xhtml.ToString();
      }

      #endregion

      #region Breadcrumb Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="BreadcrumbControl"/>.
      /// </summary>
      private string RenderBreadcrumb(BreadcrumbControl breadcrumb)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<ol " + breadcrumb.GetIdParameter() + "class=\"breadcrumb\">");
         foreach (BreadcrumbItem item in breadcrumb.Items)
         {
            xhtml.AppendLine("  <li" + (item.IsActive ? " class=\"active\"" : string.Empty) + ">");
            if (!string.IsNullOrEmpty(item.Href)) xhtml.AppendLine("<a href=\"" + item.Href + "\">");
            if (!string.IsNullOrEmpty(item.Icon)) xhtml.AppendLine(IconControl.GetIcon(breadcrumb.Container, item.Icon));
            xhtml.AppendLine(HttpUtility.HtmlDecode(item.Caption));
            if (!string.IsNullOrEmpty(item.Href)) xhtml.AppendLine("</a>");
            xhtml.AppendLine("  </li>");
         }
         xhtml.AppendLine("</ol>");

         return xhtml.ToString();
      }

      #endregion

      #region Button / ButtonGroup Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="ButtonGroupControl"/>.
      /// </summary>
      private string RenderButtonBar(ButtonGroupControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"" + (control.Vertical ? "btn-group-vertical" : "btn-group") + "\">");
         foreach (ButtonControl btn in control.Buttons)
         {
            btn.Size = control.Size;
            xhtml.AppendLine(Render(btn));
         }
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="ButtonControl"/>.
      /// </summary>
      private string RenderButton(ButtonControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         string color = string.Empty;
         color = GetCssClassFromControlColor(control.Color);
         color = string.IsNullOrWhiteSpace(color) ? "default" : color;

         /*string cssClass = "btn btn-" + color;
         cssClass += (control.IsBlock ? " btn-block" : string.Empty);
         cssClass += (control.Enabled ? string.Empty : " disabled") + " ";
         cssClass += GetButtonSizeClass(control.Size);

         string jsAction = string.Empty;

         switch (control.Type)
         {
            case ButtonControl.ButtonTypes.Submit:
               
               // jsAction is discarded
               // HREF     is discarded --> Not link

               xhtml.AppendLine("<button " + control.GetIdParameter() + " " +
                                             control.GetNameParameter() + " " +
                                             "class=\"" + cssClass  + "\"" +
                                             ">");
               break;

            case ButtonControl.ButtonTypes.SubmitJS:

               // HREF     is discarded --> Not link

               xhtml.AppendLine("<button " + control.GetIdParameter() + " " +
                                             control.GetNameParameter() + " " +
                                             "class=\"" + cssClass  + "\" " +
                                             "onclick=\"javascript:" + control.JavaScriptAction + "\" " +
                                             ">");
               break;
         }
         */

         xhtml.Append("<" + (string.IsNullOrWhiteSpace(control.Href) ? "button" : "a") + " ");
         xhtml.Append(control.GetIdParameter());
         if (control.Type == ButtonControl.ButtonTypes.Submit)
         {
            xhtml.Append("type=\"submit\" ");
         }
         else
         {
            xhtml.Append("type=\"button\" ");
         }
         xhtml.Append("class=\"btn btn-" + color + (control.IsBlock ? " btn-block" : string.Empty) + (control.Enabled ? string.Empty : " disabled") + " " + GetButtonSizeClass(control.Size) + "\" ");
         xhtml.Append(string.IsNullOrWhiteSpace(control.Href) ? string.Empty : "href=\"" + control.Href + "\" ");
         xhtml.Append(string.IsNullOrWhiteSpace(control.JavaScriptAction) ? string.Empty : "onclick=\"javascript:" + control.JavaScriptAction + "\"");
         xhtml.Append(!string.IsNullOrWhiteSpace(control.ModalDomId) && control.Type == ButtonControl.ButtonTypes.OpenModalForm ? "data-toggle=\"modal\" data-target=\"#" + control.ModalDomId.Trim() + "\" " : string.Empty);
         xhtml.Append(!string.IsNullOrWhiteSpace(control.ModalDomId) && control.Type == ButtonControl.ButtonTypes.OpenModalView ? "onclick=\"javascript:open" + Script.ConvertToFunctionName(control.DomID) + "();\"" : string.Empty);
         xhtml.Append(control.Type == ButtonControl.ButtonTypes.CloseModalForm ? "data-dismiss=\"modal\" " : string.Empty);
         xhtml.Append(">");
         xhtml.Append((string.IsNullOrWhiteSpace(control.Icon) ? string.Empty : IconControl.GetIcon(control.Container, control.Icon) + "&nbsp;&nbsp;") + HttpUtility.HtmlDecode(control.Caption));
         xhtml.Append("</" + (string.IsNullOrWhiteSpace(control.Href) ? "button" : "a") + ">\n");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="ButtonSplit"/>.
      /// </summary>
      private string RenderButtonSplit(ButtonSplit control)
      {
         StringBuilder xhtml = new StringBuilder();

         // Si no tiene opciones de menú, trata el control como un simple Button
         if (control.MenuOptions.Count <= 0)
         {
            return RenderButton((ButtonControl)control);
         }

         string color = string.Empty;
         color = GetCssClassFromControlColor(control.Color);
         color = string.IsNullOrWhiteSpace(color) ? "default" : color;

         xhtml.Append("<div class=\"btn-group\">");
         xhtml.Append("   <button type=\"button\" class=\"btn btn-" + color + " " + GetButtonSizeClass(control.Size) + " dropdown-toggle\" data-toggle=\"dropdown\">");
         if (!string.IsNullOrWhiteSpace(control.Icon)) xhtml.AppendLine(IconControl.GetIcon(control.Container, control.Icon) + "&nbsp;&nbsp;");
         xhtml.Append(control.Caption + "&nbsp;&nbsp;<span class=\"caret\"></span>");
         xhtml.Append("<span class=\"sr-only\">Toggle Dropdown</span>");
         xhtml.Append("   </button>");
         xhtml.Append("   <ul class=\"dropdown-menu\" role=\"menu\">");

         foreach (ButtonControl button in control.MenuOptions)
         {
            xhtml.Append("       <li><a ");
            xhtml.Append("href=\"" + (string.IsNullOrWhiteSpace(button.Href) ? "#" : button.Href) + "\" ");
            xhtml.Append(!string.IsNullOrWhiteSpace(button.ModalDomId) && button.Type == ButtonControl.ButtonTypes.OpenModalForm ? "data-toggle=\"modal\" data-target=\"#" + button.ModalDomId.Trim() + "\" " : string.Empty);
            xhtml.Append(!string.IsNullOrWhiteSpace(button.ModalDomId) && button.Type == ButtonControl.ButtonTypes.OpenModalView ? "onclick=\"javascript:" + button.JavaScriptAction + "\"" : string.Empty);
            xhtml.Append(">" + button.Caption + "</a></li>");
         }

         xhtml.Append("   </ul>");
         xhtml.Append("</div>");

         // TODO: Los elementos de menú necesitan revisión

         return xhtml.ToString();
      }

      private string GetButtonSizeClass(ButtonControl.ButtonSizes size)
      {
         switch (size)
         {
            case ButtonControl.ButtonSizes.Large: return "btn-lg";
            case ButtonControl.ButtonSizes.Small: return "btn-sm";
            case ButtonControl.ButtonSizes.ExtraSmall: return "btn-xs";
         }

         return string.Empty;
      }

      #endregion

      #region Callout Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="CalloutControl"/>.
      /// </summary>
      private string RenderCallout(CalloutControl callout)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + callout.GetIdParameter() + "class=\"callout callout-" + GetCssClassFromControlColor(callout.Type) + "\">");
         xhtml.AppendLine("  <h4>" + (!string.IsNullOrWhiteSpace(callout.Icon) ? IconControl.GetIcon(callout.Container, callout.Icon) + "&nbsp;" : string.Empty) + HttpUtility.HtmlDecode(callout.Title) + "</h4>");
         xhtml.AppendLine("  <p>" + HttpUtility.HtmlDecode(callout.Text) + "</p>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region Carrousel Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="CarrouselControl"/>.
      /// </summary>
      private string RenderCarrousel(CarrouselControl carrousel)
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
      /// Renderiza un control de tipo <see cref="ChatControl"/>.
      /// </summary>
      private string RenderChat(ChatControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"box\">");
         xhtml.AppendLine("  <div class=\"box-header\">");
         xhtml.AppendLine("    <i class=\"fa fa-comments-o\"></i>");
         xhtml.AppendLine("    <h3 class=\"box-title\">" + control.Caption + "</h3>");
         xhtml.AppendLine("    <div class=\"box-tools pull-right\" data-toggle=\"tooltip\" title=\"Status\">");
         xhtml.AppendLine("      <div class=\"btn-group\" data-toggle=\"btn-toggle\">");
         xhtml.AppendLine("        <button type=\"button\" class=\"btn btn-default btn-sm active\"><i class=\"fa fa-square text-green\"></i></button>");
         xhtml.AppendLine("        <button type=\"button\" class=\"btn btn-default btn-sm\"><i class=\"fa fa-square text-red\"></i></button>");
         xhtml.AppendLine("      </div>");
         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("  <div class=\"box-body chat\" id=\"chat-box\">");

         foreach (ChatMessage msg in control.Messages)
         {
            xhtml.AppendLine(RenderChatMessage(msg));
         }

         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("  <div class=\"box-footer\">");
         xhtml.AppendLine("    <div class=\"input-group\">");
         xhtml.AppendLine("      <input class=\"form-control\" placeholder=\"Type message...\"/>");
         xhtml.AppendLine("      <div class=\"input-group-btn\">");
         xhtml.AppendLine("        <button class=\"btn btn-success\"><i class=\"fa fa-plus\"></i></button>");
         xhtml.AppendLine("      </div>");
         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      public string RenderChatMessage(ChatMessage control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"item\">");
         xhtml.AppendLine("  <img src=\"img/avatar.png\" alt=\"user image\" class=\"online\"/>");
         xhtml.AppendLine("  <p class=\"message\">");
         xhtml.AppendLine("    <a href=\"#\" class=\"name\">");
         xhtml.AppendLine("      <small class=\"text-muted pull-right\"><i class=\"fa fa-clock-o\"></i> 2:15</small>");
         xhtml.AppendLine(control.Author);
         xhtml.AppendLine("    </a>");
         xhtml.AppendLine(control.Content);
         xhtml.AppendLine("  </p>");
         xhtml.AppendLine("  <div class=\"attachment\">");
         xhtml.AppendLine("    <h4>Attachments:</h4>");
         xhtml.AppendLine("    <p class=\"filename\">Theme-thumbnail-image.jpg</p>");
         xhtml.AppendLine("    <div class=\"pull-right\">");
         xhtml.AppendLine("      <button class=\"btn btn-primary btn-sm btn-flat\">Open</button>");
         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region CookiesAdvisor Control

      private const string COOKIESADVISORCTRL_COOKIE_NAME = "cosmo.UI.cookiesadvisorcontrol.status";

      /// <summary>
      /// Renderiza un control de tipo <see cref="CookiesAdvisorControl"/>.
      /// </summary>
      private string RenderCookiesAdvisor(CookiesAdvisorControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         // If cookie exists (cookies accepted) quit
         HttpCookie cookie = Workspace.Context.Request.Cookies.Get(COOKIESADVISORCTRL_COOKIE_NAME);
         if (cookie != null)
         {
            if (cookie.Value == "accept")
            {
               return string.Empty;
            }
         }

         if (string.IsNullOrWhiteSpace(control.Message))
         {
            control.Message = "Utilizamos cookies propias y de terceros para mejorar nuestros servicios. Al utilizar nuestros servicios, aceptas el uso que hacemos de las cookies.";
         }

         HtmlContentControl message = new HtmlContentControl(control.Container);
         message.AppendParagraph(control.Message);

         PanelControl cookiesPanel = new PanelControl(control.Container, control.DomID);
         cookiesPanel.CaptionIcon = IconControl.ICON_LEGAL;
         cookiesPanel.Caption = "Uso de <em>cookies</em>";
         cookiesPanel.Content.Add(message);

         ButtonControl btnAccept = new ButtonControl(control.Container, "cmdAcceptCookies", "Aceptar cookies", string.Empty, "acceptCookies();");
         btnAccept.Size = ButtonControl.ButtonSizes.ExtraSmall;
         cookiesPanel.ButtonBar.Buttons.Add(btnAccept);

         if (!string.IsNullOrWhiteSpace(control.InformationHref))
         {
            ButtonControl btnInfo = new ButtonControl(control.Container, "cmdInfoCookies", "Más información", control.InformationHref, string.Empty);
            btnInfo.Size = ButtonControl.ButtonSizes.ExtraSmall;
            cookiesPanel.ButtonBar.Buttons.Add(btnInfo);
         }

         SimpleScript script = new SimpleScript(control.Container);
         script.ExecutionType = Script.ScriptExecutionMethod.Standalone;
         script.AppendSourceLine("function acceptCookies() {");
         script.AppendSourceLine("  $.cookie('" + COOKIESADVISORCTRL_COOKIE_NAME + "', 'accept');");
         script.AppendSourceLine("  $('#" + control.DomID + "').hide();");
         script.AppendSourceLine("}");
         control.Container.Scripts.Add(script);

         return Render(cookiesPanel);
      }

      #endregion

      #region DocumentHeader Control

      /// <summary>
      /// Renderiza un control del tipo <see cref="DocumentHeaderControl"/>.
      /// </summary>
      private string RenderDocumentHeader(DocumentHeaderControl docHead)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<h4 class=\"page-header\">");
         xhtml.AppendLine(HttpUtility.HtmlDecode(docHead.Title));
         xhtml.AppendLine("<small>" + HttpUtility.HtmlDecode(docHead.SubTitle) + "</small>");
         xhtml.AppendLine("</h4>");

         return xhtml.ToString();
      }

      #endregion

      #region Error Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="ErrorControl"/>.
      /// </summary>
      private string RenderError(ErrorControl control)
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

      #region Form Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormControl"/>.
      /// </summary>
      private string RenderForm(FormControl control, string receivedFormID)
      {
         StringBuilder xhtml = new StringBuilder();

         if (control.UsePanel)
         {
            xhtml.AppendLine("<div class=\"box box-primary\">");
            xhtml.AppendLine("  <div class=\"box-header\">");
            if (!string.IsNullOrWhiteSpace(control.Caption)) xhtml.AppendLine("    <h3 class=\"box-title\">" + (string.IsNullOrWhiteSpace(control.Icon) ? string.Empty : IconControl.GetIcon(control.Container, control.Icon) + " ") + HttpUtility.HtmlDecode(control.Caption) + "</h3>");
            xhtml.AppendLine("  </div>");
         }
         xhtml.AppendLine("  <form " + control.GetIdParameter() + " role=\"form\" method=\"" + control.Method + "\"" + (string.IsNullOrWhiteSpace(control.Action) ? string.Empty : " action=\"" + control.Action + "\"") + (control.IsMultipart ? " enctype=\"multipart/form-data\"" : string.Empty) + ">");
         xhtml.AppendLine("    <input type=\"hidden\" name=\"" + Cosmo.Workspace.PARAM_ACTION + "\" value=\"" + FormControl.FORM_ACTION_SEND + "\" />");
         xhtml.AppendLine("    <input type=\"hidden\" name=\"" + FormControl.FORM_ID + "\" value=\"" + control.DomID + "\" />");
         xhtml.AppendLine("    <div class=\"box-body\">");

         foreach (Control field in control.Content)
         {
            // Se pasa el parámetro postback para evitar que en las cargas iniciales 
            // de los formularios aparezcan los campos con error de validación
            xhtml.AppendLine(Render(field, receivedFormID));
         }

         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("    <div class=\"box-footer\">");

         foreach (ButtonControl button in control.FormButtons)
         {
            xhtml.AppendLine(Render(button));
         }

         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("  </form>");
         if (control.UsePanel)
         {
            xhtml.AppendLine("</div>");
         }

         // Para formulario con envio AJAX, genera el script necesario
         if (control.SendDataMethod == FormControl.FormSendDataMethod.JSSubmit)
         {
            ModalViewSendFormScript script = new ModalViewSendFormScript((ModalViewContainer)control.Container, control);
            control.Container.Scripts.Add(script);
         }

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldHidden"/>.
      /// </summary>
      private string RenderFormFieldHidden(FormFieldHidden control, string receivedFormID)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("  <input type=\"hidden\" " + control.GetIdParameter() + control.GetNameParameter() + " value=\"" + HttpUtility.HtmlDecode(control.Value.ToString()) + "\" />");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldCaptcha"/>.
      /// </summary>
      private string RenderFormFieldCaptcha(FormFieldCaptcha control, string receivedFormID)
      {
         bool isValid = !string.IsNullOrWhiteSpace(receivedFormID) ? control.Validate() : true;
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"form-group" + (isValid ? string.Empty : " has-error") + "\">");
         xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + (isValid ? string.Empty : IconControl.GetIcon(control.Container, IconControl.ICON_WARNING) + " ") + HttpUtility.HtmlDecode(control.Label) + "</label>");
         xhtml.AppendLine("  <div class=\"input-group\" />");
         xhtml.AppendLine("    <div class=\"input-group-addon\" style=\"padding:0;\">");
         xhtml.AppendLine("      <img src=\"" + UIRestHandler.GetCaptchaUrl().ToString() + "\" alt=\"Human Verification Image\" style=\"height:40px;\">");
         xhtml.AppendLine("    </div>");
         xhtml.AppendLine("    <input type=\"text\" " + control.GetIdParameter() + " " + control.GetNameParameter() + " placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" class=\"form-control input-lg\" />");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldText"/>.
      /// </summary>
      private string RenderFormFieldPassword(FormFieldPassword control, string receivedFormID)
      {
         bool isValid = !string.IsNullOrWhiteSpace(receivedFormID) ? control.Validate() : true;
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"form-group" + (isValid ? string.Empty : " has-error") + "\">");
         xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + (isValid ? string.Empty : IconControl.GetIcon(control.Container, IconControl.ICON_WARNING) + " ") + HttpUtility.HtmlDecode(control.Label) + "</label>");

         if (control.RewriteRequired)
         {
            xhtml.AppendLine("  <div class=\"row\">");
            xhtml.AppendLine("    <div class=\"col-xs-6\">");
            xhtml.AppendLine("      <div class=\"input-group\">");
            xhtml.AppendLine("        <span class=\"input-group-addon\"><i class=\"fa fa-lock\"></i></span>");
            xhtml.AppendLine("        <input " + control.GetIdParameter() + control.GetNameParameter() + " type=\"password\" class=\"form-control\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" value=\"\" />");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("    </div>");
            xhtml.AppendLine("    <div class=\"col-xs-6\">");
            xhtml.AppendLine("      <div class=\"input-group\">");
            xhtml.AppendLine("        <span class=\"input-group-addon\"><i class=\"fa fa-unlock-alt\"></i></span>");
            xhtml.AppendLine("        <input id=\"" + control.DomID + FormField.FIELD_CHECK_POST_DOMID + "\" name=\"" + control.DomID + FormField.FIELD_CHECK_POST_DOMID + "\" type=\"password\" class=\"form-control\" placeholder=\"" + HttpUtility.HtmlDecode(control.RewriteFieldPlaceholder) + "\" value=\"\" />");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("    </div>");
            xhtml.AppendLine("  </div>");
         }
         else
         {
            xhtml.AppendLine("  <div class=\"input-group\">");
            xhtml.AppendLine("    <span class=\"input-group-addon\"><i class=\"fa fa-lock\"></i></span>");
            xhtml.AppendLine("    <input " + control.GetIdParameter() + control.GetNameParameter() + " type=\"password\" class=\"form-control\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" value=\"\" />");
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
      /// Renderiza un control de tipo <see cref="FormFieldText"/>.
      /// </summary>
      private string RenderFormFieldText(FormFieldText control, string receivedFormID)
      {
         bool isValid = !string.IsNullOrWhiteSpace(receivedFormID) ? control.Validate() : true;
         StringBuilder xhtml = new StringBuilder();

         if (string.IsNullOrWhiteSpace(control.AddonIcon))
         {
            switch (control.Type)
            {
               case FormFieldText.FieldDataType.Color:
                  control.AddonIcon = IconControl.ICON_EYEDROPER;
                  break;
               case FormFieldText.FieldDataType.Email:
                  control.AddonIcon = IconControl.ICON_ENVELOPE;
                  break;
               case FormFieldText.FieldDataType.Phone:
                  control.AddonIcon = IconControl.ICON_PHONE;
                  break;
               case FormFieldText.FieldDataType.Url:
                  control.AddonIcon = IconControl.ICON_GLOBE;
                  break;
            }
         }

         xhtml.AppendLine("<div class=\"form-group" + (isValid ? string.Empty : " has-error") + "\">");
         xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + (isValid ? string.Empty : IconControl.GetIcon(control.Container, IconControl.ICON_WARNING) + " ") + HttpUtility.HtmlDecode(control.Label) + "</label>");

         if (control.Type == FormFieldText.FieldDataType.Text)
         {
            xhtml.AppendLine("  " + RenderInputTag(control, "text"));
         }
         else if (control.Type == FormFieldText.FieldDataType.Email)
         {
            xhtml.AppendLine("  <div class=\"input-group\">");
            xhtml.AppendLine("    <span class=\"input-group-addon\"><i class=\"fa fa-envelope\"></i></span>");
            xhtml.AppendLine("  " + RenderInputTag(control, "email"));
            xhtml.AppendLine("  </div>");
         }
         else if (control.Type == FormFieldText.FieldDataType.Phone)
         {
            xhtml.AppendLine("  <div class=\"input-group\">");
            xhtml.AppendLine("    <span class=\"input-group-addon\"><i class=\"fa fa-phone\"></i></span>");
            xhtml.AppendLine("  " + RenderInputTag(control, "tel"));
            xhtml.AppendLine("  </div>");
         }
         else if (control.Type == FormFieldText.FieldDataType.Url)
         {
            xhtml.AppendLine("  <div class=\"input-group\">");
            xhtml.AppendLine("    <span class=\"input-group-addon\"><i class=\"fa fa-globe\"></i></span>");
            xhtml.AppendLine("  " + RenderInputTag(control, "url"));
            xhtml.AppendLine("  </div>");
         }
         else if (control.Type == FormFieldText.FieldDataType.Number)
         {
            xhtml.AppendLine("  " + RenderInputTag(control, "number"));
         }

         if (!string.IsNullOrWhiteSpace(control.Description))
         {
            xhtml.AppendLine("  <p class=\"help-block\">" + HttpUtility.HtmlDecode(control.Description) + "</p>");
         }

         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      private string RenderInputTag(FormFieldText control, string type)
      {
         return "  <input " + control.GetIdParameter() + 
                              control.GetNameParameter() + " " +
                              (!string.IsNullOrWhiteSpace(type) ? "type=\"" + type + "\"" : string.Empty) + " " +
                              "class=\"form-control\" " +
                              "placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" " +
                              "value=\"" + HttpUtility.HtmlDecode(control.Value.ToString()) + "\" " +
                              (control.ReadOnly ? "disabled" : string.Empty) + "/>";
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldDate"/>.
      /// </summary>
      private string RenderFormFieldDate(FormFieldDate control, string receivedFormID)
      {
         bool isValid = !string.IsNullOrWhiteSpace(receivedFormID) ? control.Validate() : true;
         StringBuilder xhtml = new StringBuilder();

         SimpleScript jscalendar = new SimpleScript(control.Container);
         jscalendar.ExecutionType = Script.ScriptExecutionMethod.OnDocumentReady;

         xhtml.AppendLine("<div class=\"form-group" + (isValid ? string.Empty : " has-error") + "\">");
         xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + (isValid ? string.Empty : IconControl.GetIcon(control.Container, IconControl.ICON_WARNING) + " ") + HttpUtility.HtmlDecode(control.Label) + "</label>");

         if (control.Type == FormFieldDate.FieldDateType.Date)
         {
            xhtml.AppendLine("  <div class=\"input-group\">");
            xhtml.AppendLine("    <span class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></span>");
            xhtml.AppendLine("    <input " + control.GetIdParameter() + control.GetNameParameter() + " class=\"form-control\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" value=\"" + HttpUtility.HtmlDecode(((DateTime)control.Value).ToString(Formatter.FORMAT_DATE)) + "\" />");
            xhtml.AppendLine("  </div>");

            jscalendar.AppendSourceLine("$('#" + control.DomID + "').daterangepicker({");
            jscalendar.AppendSourceLine("  singleDatePicker: true,");
            jscalendar.AppendSourceLine("  format: 'DD/MM/YYYY'");
            jscalendar.AppendSourceLine("});");
            control.Container.Scripts.Add(jscalendar);
         }
         else if (control.Type == FormFieldDate.FieldDateType.Datetime)
         {
            xhtml.AppendLine("  <div class=\"input-group\">");
            xhtml.AppendLine("    <span class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></span>");
            xhtml.AppendLine("    <input " + control.GetIdParameter() + control.GetNameParameter() + " class=\"form-control\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" value=\"" + HttpUtility.HtmlDecode(((DateTime)control.Value).ToString(Formatter.FORMAT_DATETIME)) + "\" />");
            xhtml.AppendLine("  </div>");

            jscalendar.AppendSourceLine("$('#" + control.DomID + "').daterangepicker({");
            jscalendar.AppendSourceLine("  singleDatePicker: true,");
            jscalendar.AppendSourceLine("  timePicker: true,");
            jscalendar.AppendSourceLine("  timePickerIncrement: 30,");
            jscalendar.AppendSourceLine("  format: 'DD/MM/YYYY hh:mm'");
            jscalendar.AppendSourceLine("});");
            control.Container.Scripts.Add(jscalendar);
         }
         else if (control.Type == FormFieldDate.FieldDateType.Time)
         {
            xhtml.AppendLine("  <div class=\"input-group\">");
            xhtml.AppendLine("    <span class=\"input-group-addon\"><i class=\"fa fa-clock-o\"></i></span>");
            xhtml.AppendLine("    <input " + control.GetIdParameter() + control.GetNameParameter() + " class=\"form-control\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\" value=\"" + HttpUtility.HtmlDecode(((DateTime)control.Value).ToString(Formatter.FORMAT_SHORTTIME)) + "\" />");
            xhtml.AppendLine("  </div>");

            jscalendar.AppendSourceLine("$(\"#" + control.DomID + "\").timepicker({");
            jscalendar.AppendSourceLine("  showInputs: false,");
            jscalendar.AppendSourceLine("  format: 'hh:mm'");
            jscalendar.AppendSourceLine("});");
            control.Container.Scripts.Add(jscalendar);
         }

         if (!string.IsNullOrWhiteSpace(control.Description))
         {
            xhtml.AppendLine("  <p class=\"help-block\">" + HttpUtility.HtmlDecode(control.Description) + "</p>");
         }

         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      private string RenderInputTag(string domId, string name, string type, string cssClass, bool required)
      {
         return "<input id    = \"" + domId + "\" " +
                       "name  = \"" + name + "\" " +
                       "type  = \"" + type + "\" " +
                       "class = \"" + cssClass + "\" " +
                       (required ? "required=\"required\"" : string.Empty) +
                "/>";
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldEditor"/>.
      /// </summary>
      private string RenderFormFieldEditor(FormFieldEditor control, string receivedFormID)
      {
         bool isValid = !string.IsNullOrWhiteSpace(receivedFormID) ? control.Validate() : true;
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"form-group" + (isValid ? string.Empty : " has-error") + "\">");
         xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + (isValid ? string.Empty : IconControl.GetIcon(control.Container, IconControl.ICON_WARNING) + " ") + HttpUtility.HtmlDecode(control.Label) + "</label>");
         xhtml.AppendLine("  <textarea " + control.GetIdParameter() + control.GetNameParameter() + " class=\"form-control\" rows=\"3\" placeholder=\"" + HttpUtility.HtmlDecode(control.Placeholder) + "\">" + control.Value + "</textarea>");

         if (!string.IsNullOrWhiteSpace(control.Description))
         {
            xhtml.AppendLine("  <p class=\"help-block\">" + HttpUtility.HtmlDecode(control.Description) + "</p>");
         }

         xhtml.AppendLine("</div>");

         if (control.Type == FormFieldEditor.FieldEditorType.HTML)
         {
            SimpleScript script = new SimpleScript(control.Container);
            script.ExecutionType = Script.ScriptExecutionMethod.OnDocumentReady;
            script.AppendSourceLine("CKEDITOR.replace('" + control.DomID + "');");

            control.Container.Scripts.Add(script);
         }
         else if (control.Type == FormFieldEditor.FieldEditorType.BBCode)
         {
            SimpleScript script = new SimpleScript(control.Container);
            script.ExecutionType = Script.ScriptExecutionMethod.OnDocumentReady;
            script.AppendSourceLine("CKEDITOR.replace('" + control.DomID + "', {");
            script.AppendSourceLine("  extraPlugins: 'bbcode',");
            script.AppendSourceLine("  toolbar: [");
            script.AppendSourceLine("    [ 'Source', '-', 'Save', 'NewPage', '-', 'Undo', 'Redo' ],");
            script.AppendSourceLine("    [ 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat' ],");
            script.AppendSourceLine("    [ 'Link', 'Unlink', 'Image' ],");
            script.AppendSourceLine("    '/',");
            script.AppendSourceLine("    [ 'FontSize', 'Bold', 'Italic', 'Underline' ],");
            script.AppendSourceLine("    [ 'NumberedList', 'BulletedList', '-', 'Blockquote' ],");
            script.AppendSourceLine("    [ 'TextColor', '-', 'Smiley', 'SpecialChar', '-', 'Maximize' ]");
            script.AppendSourceLine("  ]");
            script.AppendSourceLine("});");

            control.Container.Scripts.Add(script);
         }

         return xhtml.ToString();
      }

      // URL de una imágen cuando no se tiene imágen que mostrar
      private const string NO_IMAGE_URL = "http://placehold.it/70x70";

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldImage"/>.
      /// </summary>
      private string RenderFormFieldImage(FormFieldImage control, string receivedFormID)
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
            <input type="file" class="modal-control">
         </span>
         */

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldFile"/>.
      /// </summary>
      private string RenderFormFieldFile(FormFieldFile control, string receivedFormID)
      {
         StringBuilder xhtml = new StringBuilder();

         //xhtml.AppendLine("<div class=\"modal-group\">");
         //xhtml.AppendLine("  <label for=\"" + control.DomID + "\">" + HttpUtility.HtmlDecode(control.Label) + "</label>");
         //xhtml.AppendLine("  <div class=\"fileinput fileinput-new input-group\" data-provides=\"fileinput\" />");
         //xhtml.AppendLine("    <div class=\"modal-control\" data-trigger=\"fileinput\"><i class=\"glyphicon glyphicon-file fileinput-exists\"></i> <span class=\"fileinput-filename\"></span></div>");
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
         xhtml.AppendLine("  <input type=\"file\" " + control.GetIdParameter() + control.GetNameParameter() + "/>");
         if (!string.IsNullOrWhiteSpace(control.Description)) xhtml.AppendLine("      <p class=\"help-block\">" + HttpUtility.HtmlDecode(control.Description) + "</p>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="FormFieldList"/>.
      /// </summary>
      private string RenderFormFieldList(FormFieldList control, string receivedFormID)
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
      private string RenderFormFieldBoolean(FormFieldBoolean control, string receivedFormID)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"form-group\">");
         xhtml.AppendLine("  <div class=\"checkbox\">");
         xhtml.AppendLine("    <label>");
         xhtml.AppendLine("      <input " + control.GetIdParameter() + control.GetNameParameter() + " type=\"checkbox\" value=\"1\" " + ((bool)control.Value ? "checked=\"checked\"" : string.Empty) + " />&nbsp; ");
         xhtml.AppendLine("      " + HttpUtility.HtmlDecode(control.Label));
         xhtml.AppendLine("    </label>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region HtmlContent Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="HtmlContentControl"/>.
      /// </summary>
      private string RenderHtmlContent(HtmlContentControl control)
      {
         return control.Html.ToString();
      }

      #endregion

      #region Icon Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="IconControl"/>.
      /// </summary>
      private string RenderIcon(IconControl control)
      {
         if (control.Code.StartsWith("glyphicon"))
         {
            return "<i class=\"glyphicon " + control.Code + " " + GetCssClassFromBackgroundColor(control.BackgroundColor) + "\"></i>";
         }
         else if (control.Code.StartsWith("fa"))
         {
            if (!control.Banned)
            {
               return "<i class=\"fa " + control.Code + " " + control.Action + " " + GetCssClassFromBackgroundColor(control.BackgroundColor) + "\"></i>";
            }
            else
            {
               StringBuilder xhtml = new StringBuilder();
               xhtml.Append("<span class=\"fa-stack\">");
               xhtml.Append("<i class=\"fa " + control.Code + " fa-stack-1x\"></i>");
               xhtml.Append("<i class=\"fa fa-ban fa-stack-2x text-danger\"></i>");
               xhtml.Append("</span>");
               return xhtml.ToString();
            }
         }
         else
         {
            return "<i class=\"" + control.Code + " " + GetCssClassFromBackgroundColor(control.BackgroundColor) + "\"></i>";
         }
      }

      #endregion

      #region Jumbotron Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="JumbotronControl"/>.
      /// </summary>
      private string RenderJumbotron(JumbotronControl jumbotron)
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

      #region LayoutContainer

      /// <summary>
      /// Renderiza un control <see cref="LayoutContainerControl"/>.
      /// </summary>
      private string RenderLayout(LayoutContainerControl layout, string receivedFormID)
      {
         StringBuilder xhtml = new StringBuilder();

         // Si es un layout de control simple llama al método especializado
         if (layout.IsSingleControlLayout)
         {
            return RenderSingleControlLayout(layout, receivedFormID);
         }

         // Header
         foreach (Control ctrl in layout.Header)
         {
            xhtml.AppendLine(Render(ctrl, receivedFormID));
         }

         xhtml.AppendLine("<div class=\"wrapper row-offcanvas row-offcanvas-left\">");

         // Left column
         if (layout.LeftContent.Count > 0)
         {
            xhtml.AppendLine("  <aside class=\"left-side sidebar-offcanvas\">");
            foreach (Control ctrl in layout.LeftContent)
            {
               xhtml.AppendLine(Render(ctrl, receivedFormID));
            }
            xhtml.AppendLine("  </aside>");
         }

         // Main content
         if (layout.LeftContent.Count > 0)
         {
            xhtml.AppendLine("  <aside class=\"right-side\">");
         }

         // Header
         if ((layout.MainContent.Count > 0) && (layout.MainContent.Get(0).GetType() == typeof(PageHeaderControl)))
         {

            xhtml.AppendLine("  <section class=\"content-header\">");
            xhtml.AppendLine(Render(layout.MainContent.Get(0), receivedFormID));
            xhtml.AppendLine("  </section>");

            layout.MainContent.Remove(layout.MainContent.Get(0));
         }

         xhtml.AppendLine("  <section class=\"content\">");

         // Layout de tres columnas
         if (layout.RightContent.Count > 0)
         {
            xhtml.AppendLine("    <div class=\"row\">");
            xhtml.AppendLine("      <div class=\"col-md-9\">");
            foreach (Control ctrl in layout.MainContent)
            {
               xhtml.AppendLine(Render(ctrl, receivedFormID));
            }
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("      <div class=\"col-md-3\">");
            foreach (Control ctrl in layout.RightContent)
            {
               xhtml.AppendLine(Render(ctrl, receivedFormID));
            }
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("    </div>");
         }
         else
         {
            foreach (Control ctrl in layout.MainContent)
            {
               xhtml.AppendLine(Render(ctrl, receivedFormID));
            }
         }

         xhtml.AppendLine("    </section>");

         if (layout.LeftContent.Count > 0)
         {
            xhtml.AppendLine("  </aside>");
         }

         // Footer
         foreach (Control ctrl in layout.Footer)
         {
            xhtml.AppendLine(Render(ctrl, receivedFormID));
         }

         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un layout que contiene sólo un control
      /// </summary>
      private string RenderSingleControlLayout(LayoutContainerControl layout, string receivedFormID)
      {
         StringBuilder xhtml = new StringBuilder();

         if (layout.Header.Count > 0) xhtml.AppendLine(Render(layout.Header.Get(0), receivedFormID));
         if (layout.LeftContent.Count > 0) xhtml.AppendLine(Render(layout.LeftContent.Get(0), receivedFormID));
         if (layout.MainContent.Count > 0) xhtml.AppendLine(Render(layout.MainContent.Get(0), receivedFormID));
         if (layout.RightContent.Count > 0) xhtml.AppendLine(Render(layout.RightContent.Get(0), receivedFormID));
         if (layout.Footer.Count > 0) xhtml.AppendLine(Render(layout.Footer.Get(0), receivedFormID));

         return xhtml.ToString();
      }

      #endregion

      #region ListGroup Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="ListGroupControl"/>.
      /// </summary>
      private string RenderListGroup(ListGroupControl listGroup)
      {
         StringBuilder xhtml = new StringBuilder();

         if (listGroup.ListItems.Count > 0)
         {
            xhtml.AppendLine("<ul " + listGroup.GetIdParameter() + "class=\"nav nav-pills nav-stacked\">");
            foreach (ListItem item in listGroup.ListItems)
            {
               item.Style = listGroup.Style;
               item.AlignDescription = listGroup.AlignDescription;

               xhtml.AppendLine(RenderListItem(item, listGroup));
            }
            xhtml.AppendLine("</ul>");
         }

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="ListItem"/>.
      /// </summary>
      private string RenderListItem(ListItem listItem, ListGroupControl list)
      {
         string cssStyle = string.Empty;
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<li" + (listItem.IsActive ? " class=\"active\"" : string.Empty) + ">");
         xhtml.AppendLine("  <a href=\"" + listItem.Href + "\">");
         if (!string.IsNullOrWhiteSpace(listItem.Icon))
         {
            xhtml.AppendLine("    " + IconControl.GetIcon(list.Container, listItem.Icon) + " ");
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

      #region LoginFormControl

      /// <summary>
      /// Renderiza un formulario de login.
      /// </summary>
      private string RenderLoginForm(LoginFormControl loginForm)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div id=\"" + loginForm.DomID + "\" class=\"form-box\">");
         xhtml.AppendLine("  <div class=\"header\">Sign In</div>");
         xhtml.AppendLine("    <form id=\"" + loginForm.DomID + "-form\" role=\"form\">");
         xhtml.AppendLine("      <input type=\"hidden\" id=\"" + Workspace.PARAM_COMMAND + "\" name=\"" + Workspace.PARAM_COMMAND + "\" value=\"" + SecurityRestHandler.COMMAND_USER_AUTHENTICATION + "\" />");
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

         // Genera un script para enviar la petición al servidor
         SimpleScript js = new SimpleScript(loginForm.Container);
         js.ExecutionType = Script.ScriptExecutionMethod.Standalone;
         js.AppendSourceLine("$(\"#btn-login\").click(function() {");
         js.AppendSourceLine("  $(\"#" + loginForm.DomID + "-form\").submit(function(e) {");
         js.AppendSourceLine("    var postData = $(this).serializeArray();");
         js.AppendSourceLine("    $.ajax({");
         js.AppendSourceLine("      url: \"" + SecurityRestHandler.ServiceUrl + "\",");
         js.AppendSourceLine("      type: \"POST\",");
         js.AppendSourceLine("      data: postData,");
         js.AppendSourceLine("      success: function(data, textStatus, jqXHR) {");
         js.AppendSourceLine("        if (data.Result == " + (int)AjaxResponse.JsonResponse.Successful + ") {");
         js.AppendSourceLine("          $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-success\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-check\"></i>Autenticación correcta</div>');");
         js.AppendSourceLine("          window.location = data.ToURL;");
         js.AppendSourceLine("        }");
         js.AppendSourceLine("        else {");
         js.AppendSourceLine("          if (data.ErrorCode == '1001') {");
         js.AppendSourceLine("            $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-danger\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-ban\"></i>Esta cuenta actualmente no tiene acceso.</div>');");
         js.AppendSourceLine("          } else if (data.ErrorCode == '1002') {");
         js.AppendSourceLine("            $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-warning\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-warning\"></i>Esta cuenta está pendiente de verificación y aun no tiene acceso. Revise su correo, debe tener un correo con las instrucciones para verificar esta cuenta.</div>');");
         js.AppendSourceLine("          } else {");
         js.AppendSourceLine("            $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-danger\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-ban\"></i>El usuario y/o la contraseña son incorrectos.</div>');");
         js.AppendSourceLine("          }");
         js.AppendSourceLine("        }");
         js.AppendSourceLine("      },");
         js.AppendSourceLine("      error: function(jqXHR, textStatus, errorThrown) {");
         js.AppendSourceLine("        $('#" + loginForm.DomID + "-msg').html('<div class=\"alert alert-danger\" style=\"margin-bottom:0;margin-top:10px;\"><i class=\"fa fa-ban\"></i><strong>Ooooops!</strong> No se ha podido realizar la autenticación a causa de un error.</div>');");
         js.AppendSourceLine("      }");
         js.AppendSourceLine("    });");
         js.AppendSourceLine("    e.preventDefault();");
         js.AppendSourceLine("  });");
         js.AppendSourceLine("  $(\"#" + loginForm.DomID + "-form\").submit();");
         js.AppendSourceLine("});");
         loginForm.Container.Scripts.Add(js);

         return xhtml.ToString();
      }

      #endregion

      #region MediaList Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="MediaListControl"/>.
      /// </summary>
      private string RenderMediaList(MediaListControl mediaList)
      {
         StringBuilder xhtml = new StringBuilder();

         if (mediaList.Style == MediaListControl.MediaListStyle.Thumbnail)
         {
            xhtml.AppendLine("<div " + mediaList.GetIdParameter() + "class=\"row\">");
         }

         foreach (MediaItem thumb in mediaList.Items)
         {
            if (mediaList.Style == MediaListControl.MediaListStyle.Thumbnail)
            {
               xhtml.AppendLine("<div class=\"col-sm-6 col-md-4\">");
               xhtml.AppendLine("  <div class=\"thumbnail\">");
               xhtml.AppendLine("    <img class=\"img-thumbnail\" alt=\"\" src=\"" + thumb.Image + "\" style=\"" + (thumb.ImageWidth > 0 ? "width:" + thumb.ImageWidth + "px;" : string.Empty) + (thumb.ImageHeight > 0 ? "height:" + thumb.ImageHeight + "px;" : string.Empty) + "\">");
               xhtml.AppendLine("    <div class=\"caption\">");
               xhtml.AppendLine("      <h3>" + (!string.IsNullOrEmpty(thumb.Icon) ? IconControl.GetIcon(mediaList.Container, thumb.Icon) + "&nbsp;&nbsp;" : string.Empty) + HttpUtility.HtmlDecode(thumb.Title) + "</h3>");
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
                  xhtml.AppendLine("    <img class=\"img-thumbnail\" alt=\"\" src=\"" + thumb.Image + "\" style=\"" + (thumb.ImageWidth > 0 ? "width:" + thumb.ImageWidth + "px;" : string.Empty) + (thumb.ImageHeight > 0 ? "height:" + thumb.ImageHeight + "px;" : string.Empty) + "\">");
                  xhtml.AppendLine("  </a>");
               }
               xhtml.AppendLine("  <div class=\"media-body\">");
               xhtml.AppendLine("    <h4 class=\"media-heading\">" + (!string.IsNullOrEmpty(thumb.Icon) ? IconControl.GetIcon(mediaList.Container, thumb.Icon) + "&nbsp;&nbsp;" : string.Empty) + "<a href=\"" + thumb.LinkHref + "\">" + HttpUtility.HtmlDecode(thumb.Title) + "</a></h4>");
               xhtml.AppendLine("    <p>" + HttpUtility.HtmlDecode(thumb.Description) + "</p>");
               xhtml.AppendLine("  </div>");
               xhtml.AppendLine("</div>");
            }

            if (mediaList.UseItemSeparator) xhtml.AppendLine("<hr />");
         }

         if (mediaList.Style == MediaListControl.MediaListStyle.Thumbnail)
         {
            xhtml.AppendLine("</div>");
         }

         return xhtml.ToString();
      }

      #endregion

      #region Navbar Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="NavbarControl"/>.
      /// </summary>
      private string RenderNavbar(NavbarControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<header class=\"header\">");
         if (control.Header != null)
         {
            xhtml.AppendLine("  <a href=\"" + control.Header.Href + "\" class=\"logo\">");
            if (string.IsNullOrWhiteSpace(control.Header.LogoImageUrl))
            {
               xhtml.AppendLine(HttpUtility.HtmlDecode(control.Header.Caption));
            }
            else
            {
               xhtml.AppendLine("    <img src=\"" + control.Header.LogoImageUrl + "\" alt=\"" + HttpUtility.HtmlDecode(control.Header.Caption) + "\" class=\"icon\" />");
            }
            xhtml.AppendLine("  </a>");
         }

         xhtml.AppendLine("  <nav class=\"navbar navbar-static-top\" role=\"navigation\">");

         xhtml.AppendLine("    <a href=\"#\" class=\"navbar-btn sidebar-toggle\" data-toggle=\"offcanvas\" role=\"button\">");
         xhtml.AppendLine("      <span class=\"sr-only\">" + HttpUtility.HtmlDecode(control.Header.ToggleNavigationText) + "</span>");
         xhtml.AppendLine("      <span class=\"icon-bar\"></span>");
         xhtml.AppendLine("      <span class=\"icon-bar\"></span>");
         xhtml.AppendLine("      <span class=\"icon-bar\"></span>");
         xhtml.AppendLine("    </a>");

         if (control.Items.Count > 0)
         {
            // Para que salgan en el mismo órden que en la configuración
            control.Items.Reverse();

            xhtml.AppendLine("    <div class=\"navbar-right\">");
            xhtml.AppendLine("      <ul class=\"nav navbar-nav\">");
            foreach (NavbarIButtonControl item in control.Items)
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
                  if (!string.IsNullOrWhiteSpace(item.Icon)) xhtml.AppendLine("            " + IconControl.GetIcon(control.Container, item.Icon) + " ");
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
            xhtml.AppendLine("        <a href=\"" + Cosmo.Workspace.COSMO_URL_USER_DATA + "\" class=\"btn btn-default btn-flat\">Perfil</a>");
            xhtml.AppendLine("      </div>");
            xhtml.AppendLine("      <div class=\"pull-right\">");
            xhtml.AppendLine("        <a href=\"" + SecurityRestHandler.GetUserLogOffUrl() + "\" class=\"btn btn-default btn-flat\">Cerrar sesión</a>");
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

      #region PageHeader

      private string RenderPageHeader(PageHeaderControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         // xhtml.AppendLine("<section class=\"content-header\">");
         xhtml.AppendLine("  <h1><a name=\"" + PageViewContainer.LINK_TOP_PAGE + "\"></a>");
         xhtml.AppendLine(((!string.IsNullOrWhiteSpace(control.Icon) ? IconControl.GetIcon(control.Container, control.Icon) : string.Empty) + "&nbsp;"));
         xhtml.AppendLine(HttpUtility.HtmlDecode(control.Title));
         if (!string.IsNullOrWhiteSpace(control.SubTitle))
         {
            xhtml.AppendLine("<small>" + HttpUtility.HtmlDecode(control.SubTitle) + "</small>");
         }
         xhtml.AppendLine("  </h1>");

         if (control.Breadcrumb != null)
         {
            xhtml.AppendLine(Render(control.Breadcrumb));
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

      #region Pagination Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="PaginationControl"/>.
      /// </summary>
      private string RenderPagination(PaginationControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"dataTables_paginate paging_bootstrap\">");
         xhtml.AppendLine("  <ul class=\"pagination\">");
         xhtml.AppendLine("    <li class=\"prev disabled\"><a href=\"#\">← Previous</a></li>");

         for (int i = control.Min; i < control.Max; i++)
         {
            xhtml.AppendLine("    <li" + (control.Current == i ? " class=\"active\"" : string.Empty) + ">");
            xhtml.AppendLine("      <a href=\"" + control.UrlPattern.Replace(PaginationControl.URL_PAGEID_TAG, i.ToString()) + "\">" + i + "</a>");
            xhtml.AppendLine("    </li>");
         }

         xhtml.AppendLine("    <li class=\"next\"><a href=\"#\">Next → </a></li>");
         xhtml.AppendLine("  </ul>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region Panel Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="PanelControl"/>.
      /// </summary>
      private string RenderPanel(PanelControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div " + control.GetIdParameter() + " class=\"box box-primary\">");
         if (!string.IsNullOrWhiteSpace(control.Caption))
         {
            xhtml.AppendLine("  <div class=\"box-header\" title=\"\">");
            xhtml.AppendLine("    <h3 class=\"box-title\">" + (string.IsNullOrWhiteSpace(control.CaptionIcon) ? string.Empty : IconControl.GetIcon(control.Container, control.CaptionIcon) + "&nbsp;&nbsp;") + control.Caption + "</h3>");
            if (control.ButtonBar.Buttons.Count > 0)
            {
               xhtml.AppendLine("    <div class=\"box-tools pull-right\">");

               control.ButtonBar.Size = ButtonControl.ButtonSizes.Small;
               xhtml.AppendLine(Render(control.ButtonBar));

               /*foreach (Button button in control.ButtonGroup.Buttons)
               {
                  button.Size = Button.ButtonSizes.Small;
                  xhtml.AppendLine(Render(button));
               }*/
               xhtml.AppendLine("    </div>");
            }
            xhtml.AppendLine("  </div>");
         }
         xhtml.AppendLine("  <div class=\"box-body\">");

         if (!string.IsNullOrWhiteSpace(control.ContentXhtml))
         {
            xhtml.AppendLine(control.ContentXhtml);
         }

         foreach (Control component in control.Content)
         {
            xhtml.AppendLine(Render(component));
         }

         xhtml.AppendLine("  </div>");
         if (control.Footer.Count > 0)
         {
            xhtml.AppendLine("  <div class=\"box-footer\">");
            foreach (Control component in control.Footer)
            {
               xhtml.AppendLine(Render(component));
            }
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

      #region PictureGallery Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="PictureGalleryControl"/>.
      /// </summary>
      public string RenderPictureGallery(PictureGalleryControl control)
      {
         int col = 0;
         List<List<PictureControl>> picCols = new List<List<PictureControl>>();
         StringBuilder xhtml = new StringBuilder();

         // Distribuye las fotografias por columnas
         for (int i = 0; i < control.Columns; i++)
         {
            picCols.Add(new List<PictureControl>());
         }
         foreach (PictureControl pic in control.Pictures)
         {
            picCols[col].Add(pic);

            // Control de la columna
            if (++col >= control.Columns) col = 0;
         }

         // Genera el código HTML
         xhtml.AppendLine("<div class=\"row\">");
         for (int i = 0; i < control.Columns; i++)
         {
            xhtml.AppendLine("  <div class=\"col-lg-3 col-xs-6\">");
            foreach (PictureControl pic in picCols[i])
            {
               xhtml.AppendLine(RenderPicture(pic));
            }
            xhtml.AppendLine("  </div>");
         }
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un control de tipo <see cref="PictureControl"/>.
      /// </summary>
      public string RenderPicture(PictureControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"box picture-box\">");
         xhtml.AppendLine("  <div class=\"box-header picture-box-header\">");
         xhtml.AppendLine("    <a href=\"" + control.ImageHref + " title=\"Ampliar imagen\"\" target=\"_blank\">");
         xhtml.AppendLine("    <img src=\"" + control.ImageUrl + "\" alt=\"" + control.ImageAlternativeText + "\" style=\"width:100%;\">");
         xhtml.AppendLine("    </a>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("  <div class=\"box-body picture-box-body\">");
         xhtml.AppendLine("    <p style=\"font-size:0.85em;\">" + control.Text + "</p>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("  <div class=\"box-footer picture-box-footer clearfix\">");
         xhtml.AppendLine("    <p class=\"pull-left\">" + control.Footer + "</p>");
         xhtml.AppendLine("    <a href=\"#\" class=\"btn btn-default btn-xs pull-right\">" + IconControl.GetIcon(control.Container, IconControl.ICON_WRENCH) + "</a>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region Popover Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="PopoverControl"/>.
      /// </summary>
      private string RenderPopover(PopoverControl popover)
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

      #region ProgressBar Control

      /// <summary>
      /// Renderiza controles de tipo <see cref="ProgressBarControl"/>.
      /// </summary>
      private string RenderProgressBar(ProgressBarControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"progress xs\">");
         xhtml.AppendLine("  <div class=\"progress-bar progress-bar-" + GetCssClassFromControlColor(control.Color) + "\" style=\"width: " + control.Percentage + "%\"></div>");
         xhtml.AppendLine("</div>");

         if (!string.IsNullOrWhiteSpace(control.Description))
         {
            xhtml.AppendLine("<small>" + HttpUtility.HtmlDecode(control.Description) + "</small>");
         }

         return xhtml.ToString();
      }

      #endregion

      #region Sidebar Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="SidebarControl"/>.
      /// </summary>
      private string RenderSidebar(SidebarControl sidebar)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<section class=\"sidebar\">");
         xhtml.AppendLine("  <ul class=\"sidebar-menu\">");

         foreach (SidebarButton btn in sidebar.Buttons)
         {
            xhtml.AppendLine(RenderSidebarButton(btn));
         }
         
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

         if (!Workspace.CurrentUser.CheckAuthorization(sidebarBtn.Roles))
         {
            return string.Empty;
         }

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

      #region TabbedContainer Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="TabbedContainerControl"/>.
      /// </summary>
      private string RenderTabbedContainer(TabbedContainerControl control, string receivedFormID)
      {
         bool first;
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"nav-tabs-custom\">");
         xhtml.AppendLine("  <ul class=\"nav nav-tabs\">");

         first = true;
         foreach (TabItemControl tab in control.TabItems)
         {
            xhtml.AppendLine("    <li" + (first ? " class=\"active\"" : string.Empty) + "><a href=\"#" + tab.DomID + "\" data-toggle=\"tab\">" + (string.IsNullOrWhiteSpace(tab.Icon) ? string.Empty : IconControl.GetIcon(control.Container, tab.Icon) + "&nbsp;") + HttpUtility.HtmlDecode(tab.Caption) + "</a></li>");
            first = false;
         }

         // <li class="pull-right"><a href="#" class="text-muted"><i class="fa fa-gear"></i></a></li>
         xhtml.AppendLine("  </ul>");
         xhtml.AppendLine("  <div class=\"tab-content\">");

         first = true;
         foreach (TabItemControl tab in control.TabItems)
         {
            xhtml.AppendLine("    <div class=\"tab-pane" + (first ? " active" : string.Empty) + "\" " + tab.GetIdParameter() + ">");
            foreach (Control ctrl in tab.Content)
            {
               xhtml.AppendLine(Render(ctrl, receivedFormID));
            }
            xhtml.AppendLine("    </div>");
            first = false;
         }

         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region Table Control

      /// <summary>
      /// Renderiza un control de tipo <see cref="TableControl"/>.
      /// </summary>
      private string RenderTable(TableControl table)
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
            if (typeof(Control).IsAssignableFrom(cell.Value.GetType()))
            {
               content = Render((Control)cell.Value);
            }
            else
            {
               if (string.IsNullOrWhiteSpace(cell.Href))
               {
                  content = cell.Value.ToString();
               }
               else
               {
                  content = "<a href=\"" + cell.Href + "\">" + cell.Value.ToString() + "</a>";
               }
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

      #region Timeline Control

      /// <summary>
      /// Renderiza controles de tipo <see cref="TimelineControl"/>.
      /// </summary>
      private string RenderTimeline(TimelineControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<div class=\"row\">");
         xhtml.AppendLine("  <div class=\"col-md-12\">");
         xhtml.AppendLine("    <ul class=\"timeline\">");

         foreach (TimelineItem item in control.Items)
         {
            if (item.Type == TimelineItem.TimelineItemType.Entry)
            {
               xhtml.AppendLine("   <li>");
               if (!string.IsNullOrWhiteSpace(item.ID)) xhtml.AppendLine("     <a name=\"" + item.ID + "\"></a>");
               xhtml.AppendLine("      " + IconControl.GetIcon(control.Container, item.Icon, ComponentBackgroundColor.Blue));
               xhtml.AppendLine("      <div class=\"timeline-item\">");
               xhtml.AppendLine("         <span class=\"time\">");
               xhtml.AppendLine("            <i class=\"fa fa-clock-o\"></i> " + item.Time);

               if (item.Buttons.Count > 0)
               {
                  xhtml.AppendLine("            &nbsp;|<span class=\"tools\">");
                  foreach (ButtonControl button in item.Buttons)
                  {
                     button.Size = ButtonControl.ButtonSizes.ExtraSmall;
                     // xhtml.AppendLine("            " + RenderButton(button));
                     xhtml.AppendLine("&nbsp;<a href=\"" + button.Href + "\" title=\"" + HttpUtility.HtmlDecode(button.Caption) + "\">" + IconControl.GetIcon(control.Container, button.Icon) + "</a>");
                  }
                  xhtml.AppendLine("            </span>");
               }

               xhtml.AppendLine("         </span>");
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
            else if (item.Type == TimelineItem.TimelineItemType.Label)
            {
               xhtml.AppendLine("      <li class=\"time-label\"><span class=\"" + GetCssClassFromBackgroundColor(item.BackgroundColor) + "\">" + HttpUtility.HtmlDecode(item.Title) + "</span></li>");
            }
         }

         xhtml.AppendLine("    </ul>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      #endregion

      #region TreeView Control

      /// <summary>
      /// Renderiza controles de tipo <see cref="TreeViewControl"/>.
      /// </summary>
      private string RenderTreeView(TreeViewControl control)
      {
         StringBuilder xhtml = new StringBuilder();

         /*xhtml.AppendLine("<div class=\"box box-danger\">");
         xhtml.AppendLine("  <div class=\"box-header\">");
         if (!string.IsNullOrWhiteSpace(control.Icon)) xhtml.AppendLine("    " + Icon.GetIcon(control.ParentViewport, control.Icon));
         if (!string.IsNullOrWhiteSpace(control.Caption)) xhtml.AppendLine("    <h3 class=\"box-title\">" + HttpUtility.HtmlDecode(control.Caption) + "</h3>");
         xhtml.AppendLine("  </div>");
         xhtml.AppendLine("  <div class=\"box-body\">");

         if (!string.IsNullOrWhiteSpace(control.Description)) xhtml.AppendLine("    <p>" + control.Description + "</p>");*/

         xhtml.AppendLine("    <div class=\"tree\">");
         xhtml.AppendLine("      <ul role=\"tree\">");

         foreach (TreeViewChildItemControl child in control.ChildItems)
         {
            xhtml.AppendLine(RenderTreeViewChils(child, control.Collapsed));
         }

         xhtml.AppendLine("      </ul>");
         xhtml.AppendLine("    </div>");
         
         // xhtml.AppendLine("  </div>");
         // xhtml.AppendLine("</div>");

         return xhtml.ToString();
      }

      /// <summary>
      /// Renderiza un nodo de un control <see cref="TreeViewControl"/>.
      /// </summary>
      private string RenderTreeViewChils(TreeViewChildItemControl childItem, bool collapsed)
      {
         StringBuilder xhtml = new StringBuilder();

         xhtml.AppendLine("<li class=\"parent_li\" role=\"treeitem\">");
         xhtml.AppendLine("  <span class=\"clickable" + (childItem.Type != ComponentColorScheme.Normal ? " badge badge-" + GetCssClassFromControlColor(childItem.Type) : string.Empty) + "\">");

         if (childItem.ChildItems.Count > 0)
         {
            if (!string.IsNullOrWhiteSpace(childItem.Href)) xhtml.AppendLine("<a onlick=\"\">");
         }
         else
         {
            if (!string.IsNullOrWhiteSpace(childItem.Href)) xhtml.AppendLine("<a href=\"" + childItem.Href + "\">");
         }

         if (!string.IsNullOrWhiteSpace(childItem.Icon)) xhtml.AppendLine(IconControl.GetIcon(childItem.Container, childItem.Icon));
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
            foreach (TreeViewChildItemControl child in childItem.ChildItems)
            {
               xhtml.AppendLine(RenderTreeViewChils(child, collapsed));
            }
            xhtml.AppendLine("    </ul>");
         }

         xhtml.AppendLine("</li>");

         return xhtml.ToString();
      }

      #endregion

      #endregion

      #region Private Members

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

      private string ConvertPopoverDirectionToString(Cosmo.UI.Controls.PopoverControl.PopoverDirections direction)
      {
         switch (direction)
         {
            case Cosmo.UI.Controls.PopoverControl.PopoverDirections.Left:
               return "left";

            case Cosmo.UI.Controls.PopoverControl.PopoverDirections.Right:
               return "right";

            default:
               return string.Empty;
         }
      }

      /// <summary>
      /// Render modal forms.
      /// </summary>
      private string RenderModalViews(ViewContainer container, List<ModalViewContainer> modalList)
      {
         StringBuilder xhtml = new StringBuilder();

         // Renderiza las ventanas modales
         foreach (ModalViewContainer modal in modalList)
         {
            // Genera la cabecera de la ventana modal
            xhtml.AppendLine("<!-- Modal view: " + modal.DomID + " -->");
            xhtml.AppendLine("<div id=\"" + modal.DomID + "\" class=\"modal fade\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"" + modal.DomID + "-label\" aria-hidden=\"true\">");
            xhtml.AppendLine("  <div class=\"modal-dialog\"></div>");
            xhtml.AppendLine("</div>");

            container.Scripts.Add(modal.GetOpenModalScript());
         }

         return xhtml.ToString();
      }

      #endregion

   }
}
