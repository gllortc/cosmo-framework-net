using Cosmo.Diagnostics;
using Cosmo.UI.Controls;
using Cosmo.UI.Menu;
using Cosmo.UI.Render;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace Cosmo.UI
{
   /// <summary>
   /// Implementa el servicio de renderizadio (UI).
   /// </summary>
   public class UIService
   {
      private const string SETTING_BROWSER_AGENT = "browser-agent-regexp";

      // Declaración de variables internas
      private string _activeRenderer;
      private Workspace _ws;
      private Dictionary<string, RenderModule> _renderers;
      private Dictionary<string, IMenuProvider> _menus;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="UIService"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Workspace"/> que representa el espacio de trabajo actual.</param>
      /// <param name="context">Una instancia del contexto actual que permite acceder al tipo de navegador y elegir el módulo de renderizado.</param>
      public UIService(Workspace workspace, HttpContext context)
      {
         Initialize();

         _ws = workspace;

         // Inicializa el servicio de renderizado de controles
         LoadRenderModules();
         SelectRenderer(context);

         // Inicial el servicio de menús
         LoadMenuModules();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el nombre (ID) del módulo de renderizado activo.
      /// </summary>
      public string ActiveRenderModuleId
      {
         get { return _activeRenderer; }
      }

      /// <summary>
      /// Devuelve una lista de recursos JavaScript necesarios para representar el contenido renderizado.
      /// </summary>
      public List<string> JavascriptResources
      {
         get { return _renderers[_activeRenderer].JavascriptResources; }
      }

      /// <summary>
      /// Devuelve una lista de recursos CSS necesarios para representar el contenido renderizado.
      /// </summary>
      public List<string> CssResources
      {
         get { return _renderers[_activeRenderer].CssResources; }
      }

      /// <summary>
      /// Devuelve la referencia al icono de tipo <c>X-Icon</c> que debe decorar la pestaña/página actual.
      /// </summary>
      public string XIcon
      {
         get { return _renderers[_activeRenderer].XIcon; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Renderiza un control.
      /// </summary>
      /// <param name="control">Control a renderizar.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML para representar el control en el navegador del usuario.</returns>
      public string Render(Control control)
      {
         return Render(control, string.Empty);
      }

      /// <summary>
      /// Renderiza un control.
      /// </summary>
      /// <param name="control">Control a renderizar.</param>
      /// <param name="postback">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena de texto que contiene el código XHTML para representar el control en el navegador del usuario.</returns>
      public string Render(Control control, string receivedFormID)
      {
         return _renderers[_activeRenderer].Render(control, receivedFormID);
      }

      /// <summary>
      /// Renderiza un conjunto de controles.
      /// </summary>
      /// <param name="controls">Colección de controles a renderizar.</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public string Render(ControlCollection controls)
      {
         return Render(controls, string.Empty);
      }

      /// <summary>
      /// Renderiza un conjunto de controles.
      /// </summary>
      /// <param name="controls">Colección de controles a renderizar.</param>
      /// <param name="postback">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>Una cadena que contiene XHTML.</returns>
      public string Render(ControlCollection controls, string receivedFormID)
      {
         return _renderers[_activeRenderer].Render(controls, receivedFormID);
      }

      /// <summary>
      /// Render a view.
      /// </summary>
      /// <param name="container">A page view container.</param>
      /// <returns>A string containing the XHTML markup to represent the view in a browser.</returns>
      public string RenderPage(PageViewContainer container)
      {
         return RenderPage(container, string.Empty);
      }

      /// <summary>
      /// Render a view.
      /// </summary>
      /// <param name="container">A page view container.</param>
      /// <param name="postback">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>A string containing the XHTML markup to represent the view in a browser.</returns>
      public string RenderPage(PageViewContainer container, string receivedFormID)
      {
         return _renderers[_activeRenderer].RenderPage(container, receivedFormID);
      }

      /// <summary>
      /// Render a view.
      /// </summary>
      /// <param name="container">A modal view container.</param>
      /// <returns>A string containing the XHTML markup to represent the view in a browser.</returns>
      public string RenderPage(ModalViewContainer container)
      {
         return RenderPage(container, string.Empty);
      }

      /// <summary>
      /// Render a view.
      /// </summary>
      /// <param name="container">A modal view container.</param>
      /// <param name="receivedFormID">Indica si la carga obedece a una llamada de <em>postback</em> (respuesta a un formulario).</param>
      /// <returns>A string containing the XHTML markup to represent the view in a browser.</returns>
      public string RenderPage(ModalViewContainer container, string receivedFormID)
      {
         return _renderers[_activeRenderer].RenderPage(container, receivedFormID);
      }

      /// <summary>
      /// Selecciona un módulo de renderizado según la configuración del servicio.
      /// </summary>
      /// <param name="context"></param>
      public void SelectRenderer(HttpContext context)
      {
         string applyRule = string.Empty;
         Match match = null;

         foreach (Plugin plugin in _ws.Settings.RenderModules.Plugins)
         {
            applyRule = plugin.GetString(SETTING_BROWSER_AGENT);

            if (applyRule.Equals("*"))
            {
               _activeRenderer = plugin.ID;
               return;
            }
            else if (!string.IsNullOrWhiteSpace(applyRule))
            {
               match = Regex.Match(context.Request.Browser.Browser, applyRule, RegexOptions.IgnoreCase);
               if (match.Success)
               {
                  _activeRenderer = plugin.ID;
                  return;
               }
            }
         }

         throw new NoRenderModuleAvailableException("No se ha podido establecer ningún módulo de renderizado para el workspace.");
      }

      /// <summary>
      /// Obtiene el menú lateral a partir de un proveedor determinado.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador del menú a obtener.</param>
      /// <param name="activeId">Identificador del elemento de menú que debe marcarse como activo.</param>
      /// <returns>Una instancia de <see cref="SidebarControl"/> configurada convenientemente con los elementos especificados en la configuración.</returns>
      public SidebarControl GetSidebarMenu(ViewContainer parentViewport, string id, string activeId)
      {
         if (_menus.ContainsKey(id))
         {
            return ((IMenuProvider)_menus[id]).GenerateSidebar(parentViewport, activeId);
         }
         else
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".GetSidebarMenu(string)",
                                        "No se puede obtener el control Sidebar a partir del menú [" + id + "] porqué no existe en la configuración del workspace.",
                                        LogEntry.LogEntryType.EV_WARNING));
            return new SidebarControl(parentViewport);
         }
      }

      /// <summary>
      /// Obtiene la barra de navegación a partir de un proveedor determinado.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="id">Identificador del menú a obtener.</param>
      /// <param name="activeId">Identificador del elemento de menú que debe marcarse como activo.</param>
      /// <returns>Una instancia de <see cref="NavbarControl"/> configurada convenientemente con los elementos especificados en la configuración.</returns>
      public NavbarControl GetNavbarMenu(ViewContainer parentViewport, string id, string activeId)
      {
         if (_menus.ContainsKey(id))
         {
            return ((IMenuProvider)_menus[id]).GenerateNavbar(parentViewport, activeId);
         }
         else
         {
            _ws.Logger.Add(new LogEntry(GetType().Name + ".GetNavbarMenu(string)",
                                        "No se puede obtener el control Navbar a partir del menú [" + id + "] porqué no existe en la configuración del workspace.",
                                        LogEntry.LogEntryType.EV_WARNING));

            return new NavbarControl(parentViewport);
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Carga el módulo que se usará para renderizar los controles del workspace.
      /// </summary>
      private void LoadRenderModules()
      {
         string applyRule = string.Empty;
         Type type = null;
         RenderModule _module;

         foreach (Plugin plugin in _ws.Settings.RenderModules.Plugins)
         {
            try
            {
               Object[] args = new Object[2];
               args[0] = _ws;
               args[1] = plugin;

               type = Type.GetType(plugin.Class, true, true);
               _module = (RenderModule)Activator.CreateInstance(type, args);

               if (_module != null)
               {
                  _renderers.Add(_module.ID, _module);
               }
            }
            catch
            {
               // TODO: Registrar error de carga y seguir adelante
            }
         }
      }

      /// <summary>
      /// Carga el módulo que se usará para renderizar los controles del workspace.
      /// </summary>
      private void LoadMenuModules()
      {
         string applyRule = string.Empty;
         Type type = null;
         IMenuProvider _module;

         foreach (Plugin plugin in _ws.Settings.MenuModules.Plugins)
         {
            Object[] args = new Object[2];
            args[0] = _ws;
            args[1] = plugin;

            type = Type.GetType(plugin.Class, true, true);
            _module = (IMenuProvider)Activator.CreateInstance(type, args);

            if (_module != null)
            {
               _menus.Add(_module.ID, _module);
            }
         }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _activeRenderer = string.Empty;
         _ws = null;
         _renderers = new Dictionary<string, RenderModule>();
         _menus = new Dictionary<string, IMenuProvider>();
      }

      #endregion

   }
}
