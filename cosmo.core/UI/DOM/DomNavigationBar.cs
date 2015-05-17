using System;
using System.Collections.Generic;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa una barra de navegación.
   /// </summary>
   public class DomNavigationBar : DomLayoutComponentBase
   {

      #region Enumerations

      /// <summary>
      /// Define las zonas de la barra de navegación.
      /// </summary>
      public enum NavigationBarZone
      {
         /// <summary>Zona izquierda.</summary>
         Left,
         /// <summary>Zona derecha.</summary>
         Right
      }

      #endregion

      string _separator = " | ";
      List<DomNavigationBarItem> _left;
      List<DomNavigationBarItem> _right;
      System.Web.Caching.Cache _cache;

      #region Constants

      /// <summary>Identificador de la cabecera de la barra de navegación.</summary>
      internal const string SECTION_HEAD = "navbar-top-header";
      /// <summary>Identificador de la cabecera de la sección izquierda de la barra de navegación.</summary>
      internal const string SECTION_LEFT_HEAD = "navbar-top-left-header";
      /// <summary>Identificador del pié de la sección izquierda de la barra de navegación.</summary>
      internal const string SECTION_LEFT_FOOT = "navbar-top-left-footer";
      /// <summary>Identificador de la cabecera de la sección derecha de la barra de navegación.</summary>
      internal const string SECTION_RIGHT_HEAD = "navbar-top-right-header";
      /// <summary>Identificador del pié de la sección derecha de la barra de navegación.</summary>
      internal const string SECTION_RIGHT_FOOT = "navbar-top-right-footer";
      /// <summary>Identificador del pié de la barra de navegación.</summary>
      internal const string SECTION_FOOT = "navbar-top-footer";

      /// <summary>Tag: ID de plantilla</summary>
      public const string TAG_TEMPLATE_ID = "tid";

      #endregion

      /// <summary>
      /// Devuelve una instnacia de <see cref="DomNavigationBar"/>.
      /// </summary>
      public DomNavigationBar() : base() 
      {
         _cache = null;
         _left = new List<DomNavigationBarItem>();
         _right = new List<DomNavigationBarItem>();
      }

      /// <summary>
      /// Devuelve una instnacia de <see cref="DomNavigationBar"/>.
      /// </summary>
      /// <param name="cache">Una instancia de <see cref="System.Web.Caching.Cache"/> que permite cachear el componente si este es cacheable.</param>
      public DomNavigationBar(System.Web.Caching.Cache cache) : base()
      {
         _cache = cache;
         _left = new List<DomNavigationBarItem>();
         _right = new List<DomNavigationBarItem>();
      }

      #region Properties

      /// <summary>
      /// Devuelve el identificador del componente HTML de la plantilla que implementa.
      /// </summary>
      public override string ELEMENT_ROOT
      {
         get { return "navbar-top"; }
      }

      /// <summary>
      /// Colección de elementos de la parte izquierda de la barra de navegación.
      /// </summary>
      public List<DomNavigationBarItem> LeftItems
      {
         get { return _left; }
         set { _left = value; }
      }

      /// <summary>
      /// Colección de elementos de la parte derecha de la barra de navegación.
      /// </summary>
      public List<DomNavigationBarItem> RightItems
      {
         get { return _right; }
         set { _right = value; }
      }

      /// <summary>
      /// Devuelve o establece el carácter (o código XHTML) que separa los elementos de la barra de navegación.
      /// </summary>
      public string ItemSeparator
      {
         get { return _separator; }
         set { _separator = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Agrega un elemento a la barra de navegación.
      /// </summary>
      /// <param name="name">Texto visible que aparecerá en el elemento de navegación.</param>
      /// <param name="align">Posición del elemento en la barra de navegación.</param>
      public void AddItem(string name, NavigationBarZone align)
      {
         AddItem(name, string.Empty, align);
      }

      /// <summary>
      /// Agrega un elemento a la barra de navegación.
      /// </summary>
      /// <param name="name">Texto visible que aparecerá en el elemento de navegación.</param>
      /// <param name="href">URL del elemento de navegación.</param>
      /// <param name="align">Posición del elemento en la barra de navegación.</param>
      public void AddItem(string name, string href, NavigationBarZone align)
      {
         switch (align)
         {
            case NavigationBarZone.Left:
               _left.Add(new DomNavigationBarItem(name, href));
               break;

            case NavigationBarZone.Right:
               _right.Add(new DomNavigationBarItem(name, href));
               break;

            default:
               break;
         }
      }

      /*
      /// <summary>
      /// Inicializa la barra de navegación para el nodo seleccionado.
      /// </summary>
      /// <param name="nodes">Una lista de nodos, ordenados del nodo superior al inferior.</param>
      /// <param name="side">Indica la zona dónde se ubicará la barra de navegación.</param>
      public void LoadNavigationBar(List<TreeNodeBase> nodes, DomNavigationBar.NavigationBarZone side)
      {
         int count = 0;

         // DomNavigationBar navbar = new DomNavigationBar();

         // Obtiene la gerarquía
         // List<TreeNodeBase> route = GetNodeHierarchy(nodeId);

         if (side == NavigationBarZone.Left)
            this.LeftItems.Clear();
         else
            this.RightItems.Clear();

         // Genera la barra de navegación
         this.AddItem("Inicio", "cs_default.aspx", Cosmo.Web.DOM.DomNavigationBar.NavigationBarZone.Left);
         foreach (TreeNodeBase node in nodes)
         {
            this.AddItem(node.Name, (count + 1 >= nodes.Count ? node.GetUrl() : ""), side);
            count++;
         }

         // return navbar;
      }
      */

      /// <summary>
      /// Renderiza el elemento.
      /// </summary>
      /// <param name="template">Una instancia de <see cref="DomTemplate"/> que representa la plantilla de presentación a aplicar.</param>
      /// <returns>Una cadena de texto que contiene el código XHTML que permite representar el elemento.</returns>
      public override string Render(DomTemplate template)
      {
         string xhtml = string.Empty;
         string part = string.Empty;

         try
         {
            // Si el elemento está en caché lo devuelve, sin comprobar si es cacheable o no
            if (_cache != null && _cache[template.GetCacheKey(ELEMENT_ROOT)] != null)
               return (string)_cache[template.GetCacheKey(ELEMENT_ROOT)];

            // Obtiene la plantilla del componente
            DomTemplateComponent component = template.GetLayoutComponent(ELEMENT_ROOT);
            if (component == null) return string.Empty;

            // Cabecera del menú
            xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomNavigationBar.SECTION_HEAD));

            // Añade la zona izquierda
            if (_left.Count > 0)
            {
               // Representa la cabecera de la zona izquierda
               xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomNavigationBar.SECTION_LEFT_HEAD));

               part = string.Empty;
               for (int i=0; i<_left.Count; i++)
               {
                  part += _left[i].ToXhtml();
                  if (i < _left.Count - 1) part += this.ItemSeparator;
               }
               xhtml = string.Format("{0}{1}", xhtml, part);

               // Representa el pie de la zona izquierda
               xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomNavigationBar.SECTION_LEFT_FOOT));
            }

            // Añade la zona derecha
            if (_right.Count > 0)
            {
               // Representa la cabecera de la zona izquierda
               xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomNavigationBar.SECTION_RIGHT_HEAD));

               part = string.Empty;
               for (int i = 0; i < _right.Count; i++)
               {
                  part += _right[i].ToXhtml();
                  if (i < _right.Count - 1) part += this.ItemSeparator;
               }
               xhtml = string.Format("{0}{1}", xhtml, part);

               // Representa el pie de la zona izquierda
               xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomNavigationBar.SECTION_RIGHT_FOOT));
            }

            // Pie del menú
            xhtml = string.Format("{0}{1}", xhtml, component.GetFragment(DomNavigationBar.SECTION_FOOT));

            // Reemplaza los TAGs comunesa todas las secciones
            part = DomContentComponentBase.ReplaceTag(part, DomNavigationBar.TAG_TEMPLATE_ID, template.ID.ToString());

            // Cachea el componente si es necesario
            if (_cache != null && this.CacheEnabled)
            {
               if (_cache[template.GetCacheKey(ELEMENT_ROOT)] == null)
               {
                  _cache.Insert(template.GetCacheKey(ELEMENT_ROOT), xhtml, null, DateTime.Now.AddSeconds(this.CacheExpiration), TimeSpan.Zero);
               }
            }

            return xhtml;
         }
         catch
         {
            throw;
         }
      }

      #endregion

   }

}
