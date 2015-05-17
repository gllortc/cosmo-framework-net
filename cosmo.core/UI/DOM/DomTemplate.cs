using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Cosmo.UI.DOM.Templates;

namespace Cosmo.UI.DOM
{
   /// <summary>
   /// Implementa una plantilla de
   /// </summary>
   public class DomTemplate
   {
      private int _id;
      private string _filename;
      private string _name;
      private string _desc;
      private string _version;
      private string _author;
      private PresentationRule.DeviceTypes _platform;
      private string _renderer;
      private string _firstItemCssClass = string.Empty;
      private Hashtable _layoutPages;
      private Hashtable _layoutComponents;
      private Hashtable _contentComponents;
      private List<DomPageScript> _scripts;

      #region Constants

      /// <summary>Nombre de los archivos de definició de plantilla..</summary>
      public const string FILE_NAME = "template.xml";
      /// <summary>Extensión de los archivos que contienen una plantilla (Cosmo DOM Template).</summary>
      public const string FILE_EXTENSION = "cdt";

      /// <summary>Versión de la definición de plantillas soportada.</summary>
      public const string TEMPLATE_DEFINITION_VER = "1.0";

      /// <summary>Sección raíz.</summary>
      internal const string KEY_ROOT = "template";
      /// <summary>Sección de elementos HEAD.</summary>
      internal const string KEY_HEAD = "head";
      /// <summary>Sección de elementos DOM.</summary>
      internal const string KEY_DOM = "dom";
      /// <summary>Clave que engloba las estructuras de página.</summary>
      internal const string KEY_PAGE_LAYOUTS = "page-layouts";
      /// <summary>Clave que engloba los componentes de página.</summary>
      internal const string KEY_PAGE_LAYOUT_COMPONENTS = "layout-components";
      /// <summary>Clave que engloba los componentes de contenido.</summary>
      internal const string KEY_CONTENT_COMPONENTS = "content-components";
      /// <summary>Clave para el grupo Widgets.</summary>
      internal const string KEY_WIDGETS = "widgets";
      /// <summary>Nombre de la plantilla.</summary>
      internal const string KEY_TEMPLATE_NAME = "name";
      /// <summary>Descripción de la plantilla.</summary>
      internal const string KEY_TEMPLATE_DESCR = "description";
      /// <summary>Versión de la plantilla.</summary>
      internal const string KEY_TEMPLATE_VER = "version";
      /// <summary>Autor de la plantilla.</summary>
      internal const string KEY_TEMPLATE_AUTH = "author";
      /// <summary>Plataforma de la plantilla.</summary>
      internal const string KEY_TEMPLATE_PLATFORM = "platform";
      /// <summary>Renderer de la plantilla.</summary>
      internal const string KEY_TEMPLATE_RENDERER = "renderer";
      /// <summary>Licencia de la plantilla.</summary>
      internal const string KEY_TEMPLATE_LICENCE = "licence";
      /// <summary>Versión de la definición de plantilla con la que está basada.</summary>
      internal const string ATTR_DEFINITION_VER = "cdt-ver";
      /// <summary>Clase a aplicar en el primer elemento de la lista.</summary>
      internal const string ATTR_CSS_FIRSTCLASS = "first-cssclass";
      /// <summary>Indicador de visualización del fragmento.</summary>
      internal const string ATTR_ELEMENT_SHOW = "show";
      /// <summary>Indicador de cacheabilidad del fragmento.</summary>
      internal const string ATTR_ELEMENT_CACHEABLE = "cacheable";
      /// <summary>Tiempo (segundos) de validez del fragmento en cache.</summary>
      internal const string ATTR_ELEMENT_CACHETIMEOUT = "cache-timeout";
      /// <summary>Atributo que contiene el identificador del componente equivalente para representación lateral (opcional).</summary>
      internal const string ATTR_ELEMENT_LATERAL = "lat-equiv";

      #endregion

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTemplate"/> vacía.
      /// </summary>
      public DomTemplate() 
      {
         Clear();
         
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTemplate"/> que representa la plantilla cargada desde un archivo.
      /// </summary>
      /// <param name="filename">Nombre y ruta del archivo de plantilla a cargar.</param>
      public DomTemplate(string filename)
      {
         Load(filename);
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el código identificativo de la plantilla en la instalación Cosmo.
      /// </summary>
      /// <remarks>
      /// Esta propiedad la establece Cosmo al cargar la plantilla y el valor no se guarda internamente en el archivo XML de plantilla.
      /// </remarks>
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Devuelve el nombre de archivo dónde se encuentra la plantilla cargada actualmente.
      /// </summary>
      public string Filename
      {
         get { return _filename; }
         internal set { _filename = value; }
      }

      /// <summary>
      /// Devuelve o establece el nombre de la plantilla.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Devuelve o establece la descripción de la plantilla.
      /// </summary>
      public string Description
      {
         get { return _desc; }
         set { _desc = value; }
      }

      /// <summary>
      /// Devuelve o establece la versión de la plantilla.
      /// </summary>
      public string Version
      {
         get { return _version; }
         set { _version = value; }
      }

      /// <summary>
      /// Devuelve o establece el autor de la plantilla.
      /// </summary>
      public string Author
      {
         get { return _author; }
         set { _author = value; }
      }

      /// <summary>
      /// Devuelve o establece el tipo de dispositivo al que va orientada.
      /// </summary>
      public PresentationRule.DeviceTypes DeviceType
      {
         get { return _platform; }
         set { _platform = value; }
      }

      /// <summary>
      /// Devuelve o establece la clase que se usa para el renderizado de las páginas.
      /// </summary>
      public string Renderer
      {
         get { return _renderer; }
         set { _renderer = value; }
      }

      /// <summary>
      /// Devuelve o establece la clase CSS que se aplica al primer elemento en las listas.
      /// </summary>
      public string FirstItemCssClass
      {
         get { return _firstItemCssClass; }
         set { _firstItemCssClass = value; }
      }

      /// <summary>
      /// Contiene los <em>layout components</em> de la plantilla.
      /// </summary>
      public Hashtable LayoutComponents
      {
         get { return _layoutComponents; }
         set { _layoutComponents = value; }
      }

      /// <summary>
      /// Contiene los <em>content components</em> de la plantilla.
      /// </summary>
      public Hashtable ContentComponents
      {
         get { return _contentComponents; }
         set { _contentComponents = value; }
      }

      /// <summary>
      /// Lista de scripts (código y/o referencias externas) que usa la página creada a partir de la plantilla.
      /// </summary>
      public List<DomPageScript> Scripts
      {
         get { return _scripts; }
         set { _scripts = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve una clave para la cache a partir de un identificador de componente.
      /// </summary>
      public string GetCacheKey(string xmlComponentKey)
      {
         return "cosmo.template." + _id + "." + xmlComponentKey.Trim().ToLower();
      }

      /// <summary>
      /// Obtiene el código XHTML correspondiente a la estructura de una página.
      /// </summary>
      /// <param name="type">Tipo de página solicitada.</param>
      /// <returns>Uná cadena de texto que contiene el código XHTML solicitado.</returns>
      public string GetPage(DomPage.PageType type)
      {
         string key = DomPage.SECTION_HOME;

         switch (type)
         {
            case DomPage.PageType.HomePage:
               key = DomPage.SECTION_HOME;
               break;

            case DomPage.PageType.TwoColumnsLeft:
               key = DomPage.SECTION_2COLUMNSLEFT;
               break;

            case DomPage.PageType.TwoColumnsRight:
               key = DomPage.SECTION_2COLUMNSRIGHT;
               break;

            case DomPage.PageType.ThreeColumns:
               key = DomPage.SECTION_3COLUMNS;
               break;
         }

         if (_layoutPages[key] == null)
            return string.Empty;
         else
            return (string)_layoutPages[key];
      }

      /// <summary>
      /// Establece el código  XHTML correspondiente a la estructura de una página.
      /// </summary>
      /// <param name="key">Identificador de la página a agregar/actualizar.</param>
      /// <param name="xhtml">Uná cadena de texto que contiene el código XHTML.</param>
      public void SetPage(string key, string xhtml)
      {
         _layoutPages.Add(key, xhtml);
      }

      /// <summary>
      /// Obtiene el código XHTML correspondiente a un componente de estructura.
      /// </summary>
      /// <param name="key">Identificador del componente solicitado.</param>
      /// <returns>Una instancia de <see cref="DomTemplateComponent"/> que contiene todos los elementos y propiedades de la plantilla del componente.</returns>
      public DomTemplateComponent GetLayoutComponent(string key)
      {
         if (!_layoutComponents.ContainsKey(key))
            return null;
         else
            return (DomTemplateComponent)_layoutComponents[key];
      }

      /// <summary>
      /// Obtiene el código XHTML correspondiente a un elemento de la página.
      /// </summary>
      /// <param name="key">Identificador del elemento solicitado.</param>
      /// <param name="container">La zona para la que se desea renderizar el componente.</param>
      /// <returns>Una instancia de <see cref="DomTemplateComponent"/> que contiene todos los elementos y propiedades de la plantilla del componente.</returns>
      public DomTemplateComponent GetContentComponent(string key, DomPage.ContentContainer container)
      {
         if (string.IsNullOrEmpty(key) || !_contentComponents.ContainsKey(key))
               return null;

         if (container == DomPage.ContentContainer.CenterColumn)
         {
            return (DomTemplateComponent)_contentComponents[key];
         }
         else
         {
            // Si lo tiene, devuelve la clave del componente lateral
            DomTemplateComponent component = (DomTemplateComponent)_contentComponents[key];
            string lateralKey = component.GetAttribute(DomTemplate.ATTR_ELEMENT_LATERAL);

            if (_contentComponents.ContainsKey(lateralKey))
               return (DomTemplateComponent)_contentComponents[lateralKey];
            else
               return (DomTemplateComponent)_contentComponents[key];
         }
      }

      /// <summary>
      /// Carga una plantilla desde un archivo.
      /// </summary>
      /// <param name="filename">Nombre y ruta del archivo de plantilla a cargar.</param>
      public void Load(string filename)
      {
         Clear();

         try
         {
            // Lee el archivo
            XmlTextReader reader = new XmlTextReader(filename);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            reader.Close();

            // Comprueba que el paquete corresponda a una definición correcta
            XmlNode xnod = xmlDoc.DocumentElement;
            if (!xnod.Name.ToLower().Equals(DomTemplate.KEY_ROOT))
               throw new Exception("El archivo XML proporcionado no corresponde a una definición de plantilla de presentación de Cosmo.");
            if (xnod.Attributes[DomTemplate.ATTR_DEFINITION_VER].Value.ToString().CompareTo(DomTemplate.TEMPLATE_DEFINITION_VER) > 0)
               throw new Exception("La versión del archivo es más reciente que la versión soportada (" + DomTemplate.TEMPLATE_DEFINITION_VER  + "). Consiga una versión anterior de la plantilla.");

            // Lee las propiedades principales
            foreach (XmlNode node in xnod.ChildNodes)
            {
               switch (node.Name.ToLower())
               {
                  case DomTemplate.KEY_TEMPLATE_NAME:
                     this.Name = node.FirstChild == null ? "Unknown" : node.FirstChild.Value.ToString();
                     break;

                  case DomTemplate.KEY_TEMPLATE_DESCR:
                     this.Description = node.FirstChild == null ? string.Empty : node.FirstChild.Value.ToString();
                     break;

                  case DomTemplate.KEY_TEMPLATE_VER:
                     this.Version = node.FirstChild == null ? string.Empty : node.FirstChild.Value.ToString();
                     break;

                  case DomTemplate.KEY_TEMPLATE_AUTH:
                     this.Author = node.FirstChild == null ? string.Empty : node.FirstChild.Value.ToString();
                     break;

                  case DomTemplate.KEY_TEMPLATE_RENDERER:
                     this.Renderer = node.FirstChild == null ? string.Empty : node.FirstChild.Value.ToString();
                     break;

                  case DomTemplate.KEY_TEMPLATE_PLATFORM:
                     this.DeviceType = node.FirstChild == null ? PresentationRule.DeviceTypes.Unknown : (PresentationRule.DeviceTypes)int.Parse(node.FirstChild.Value.ToString());
                     break;

                  case DomTemplate.ATTR_CSS_FIRSTCLASS:
                     this.FirstItemCssClass = node.FirstChild == null ? string.Empty : node.FirstChild.Value.ToString();
                     break;

                  case DomTemplate.KEY_HEAD:
                     foreach (XmlNode headnode in node.ChildNodes)
                     {
                        switch (headnode.Name.ToLower())
                        {
                           case DomPageScript.KEY_SCRIPT:
                           {
                              _scripts.Add(DomPageScript.ReadFromXml(headnode));
                              break;
                           }
                        }
                     }
                     break;

                  case DomTemplate.KEY_DOM:
                     foreach (XmlNode domnode in node.ChildNodes)
                     {
                        switch (domnode.Name.ToLower())
                        {
                           case DomTemplate.KEY_PAGE_LAYOUTS:

                              foreach (XmlNode pageNode in domnode.ChildNodes)
                              {
                                 SetPage(pageNode.Name, pageNode.FirstChild.Value.ToString());
                              }
                              break;

                           case DomTemplate.KEY_PAGE_LAYOUT_COMPONENTS:

                              DomTemplateComponent layoutComponent = null;

                              foreach (XmlNode layoutComponentNode in domnode.ChildNodes)
                              {
                                 if (!(layoutComponentNode is XmlComment))
                                 {
                                    layoutComponent = new DomTemplateComponent();
                                    layoutComponent.Key = layoutComponentNode.Name;

                                    // Obtiene los atributos del widget
                                    if (layoutComponentNode.Attributes != null)
                                    {
                                       foreach (XmlAttribute attr in layoutComponentNode.Attributes)
                                       {
                                          switch (attr.Name)
                                          {
                                             case DomTemplate.ATTR_ELEMENT_SHOW: layoutComponent.Show = bool.Parse(attr.Value); break;
                                             default: layoutComponent.Attributes.Add(attr.Name, attr.Value); break;
                                          }
                                       }
                                    }

                                    // Obtiene los fragmentos del widget
                                    foreach (XmlNode componentPart in layoutComponentNode.ChildNodes)
                                    {
                                       switch (componentPart.Name.ToLower())
                                       {
                                          case DomPageScript.KEY_SCRIPT:
                                             layoutComponent.Scripts.Add(DomPageScript.ReadFromXml(componentPart));
                                             break;

                                          default:
                                             if (componentPart.FirstChild != null)
                                                layoutComponent.Fragments.Add(componentPart.Name, componentPart.FirstChild.Value.ToString());
                                             break;
                                       }

                                       // if (componentPart.FirstChild != null)
                                       //   layoutComponent.Fragments.Add(componentPart.Name, componentPart.FirstChild.Value.ToString());
                                    }

                                    _layoutComponents.Add(layoutComponentNode.Name, layoutComponent);
                                 }
                              }
                              break;

                           case DomTemplate.KEY_CONTENT_COMPONENTS:

                              DomTemplateComponent contentComponent = null;

                              foreach (XmlNode contentComponentNode in domnode.ChildNodes)
                              {
                                 if (!(contentComponentNode is XmlComment))
                                 {
                                    contentComponent = new DomTemplateComponent();
                                    contentComponent.Key = contentComponentNode.Name;

                                    // Obtiene los atributos del widget
                                    if (contentComponentNode.Attributes != null)
                                    {
                                       foreach (XmlAttribute attr in contentComponentNode.Attributes)
                                       {
                                          switch (attr.Name)
                                          {
                                             case DomTemplate.ATTR_ELEMENT_SHOW: contentComponent.Show = bool.Parse(attr.Value); break;
                                             default: contentComponent.Attributes.Add(attr.Name, attr.Value); break;
                                          }
                                       }
                                    }

                                    // Obtiene los fragmentos del widget
                                    foreach (XmlNode componentPart in contentComponentNode.ChildNodes)
                                    {
                                       switch (componentPart.Name.ToLower())
                                       {
                                          case DomPageScript.KEY_SCRIPT:
                                             contentComponent.Scripts.Add(DomPageScript.ReadFromXml(componentPart));
                                             break;

                                          default:
                                             if (componentPart.FirstChild != null)
                                                contentComponent.Fragments.Add(componentPart.Name, componentPart.FirstChild.Value.ToString());
                                             break;
                                       }
                                    }

                                    _contentComponents.Add(contentComponentNode.Name, contentComponent);
                                 }
                              }
                              break;
                        }
                     }
                     break;
               }
            }
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Guarda (actualiza) la plantilla la plantilla a un archivo CDT cargado previamente.
      /// </summary>
      public void Save()
      {
         if (string.IsNullOrEmpty(_filename))
            throw new Exception("No se puede guardar la plantilla. No hay ningún nombre de archivo.");

         Save(_filename);
      }

      /// <summary>
      /// Guarda la plantilla a un archivo CDT.
      /// </summary>
      /// <param name="filename">Nombre y ruta del archivo de plantilla a guardar.</param>
      public void Save(string filename)
      {
         try
         {
            // Abre el documento
            XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            writer.WriteStartDocument();

            // Abre una cláusula OBJECT y agrega sus atributos
            writer.WriteStartElement(DomTemplate.KEY_ROOT);
            writer.WriteAttributeString(DomTemplate.ATTR_DEFINITION_VER, DomTemplate.TEMPLATE_DEFINITION_VER);
            {

               // Características de la aplicación
               writer.WriteElementString(DomTemplate.KEY_TEMPLATE_NAME, _name);
               writer.WriteElementString(DomTemplate.KEY_TEMPLATE_VER, _version);
               writer.WriteElementString(DomTemplate.KEY_TEMPLATE_DESCR, _desc);
               writer.WriteElementString(DomTemplate.KEY_TEMPLATE_AUTH, _author);
               writer.WriteElementString(DomTemplate.KEY_TEMPLATE_PLATFORM, ((int)_platform).ToString());
               writer.WriteElementString(DomTemplate.KEY_TEMPLATE_RENDERER, _renderer);
               writer.WriteElementString(DomTemplate.ATTR_CSS_FIRSTCLASS, _firstItemCssClass);

               // Datos de la licencia
               // TODO: De momento no se usa esta entrada (para usos futuros)
               writer.WriteStartElement(DomTemplate.KEY_TEMPLATE_LICENCE);
               writer.WriteEndElement();

               // Fragmentos XHTML de la página
               writer.WriteStartElement(DomTemplate.KEY_DOM);
               {

                  // Estructura de las páginas
                  writer.WriteStartElement(DomTemplate.KEY_PAGE_LAYOUTS);
                  {
                     writer.WriteElementString(DomPage.SECTION_HOME, GetPage(DomPage.PageType.HomePage));
                     writer.WriteElementString(DomPage.SECTION_2COLUMNSLEFT, GetPage(DomPage.PageType.TwoColumnsLeft));
                     writer.WriteElementString(DomPage.SECTION_2COLUMNSRIGHT, GetPage(DomPage.PageType.TwoColumnsRight));
                     writer.WriteElementString(DomPage.SECTION_3COLUMNS, GetPage(DomPage.PageType.ThreeColumns));
                  }
                  writer.WriteEndElement();

                  // Componentes
                  writer.WriteStartElement(DomTemplate.KEY_CONTENT_COMPONENTS);
                  {
                     foreach (DomTemplateComponent component in _contentComponents)
                     {
                        writer.WriteStartElement(component.Key);

                        // Añade los atributos
                        foreach (DictionaryEntry attribute in component.Attributes)
                        {
                           writer.WriteAttributeString(attribute.Key.ToString(), attribute.Value.ToString());
                        }

                        // Añade los fragmentos
                        foreach (DictionaryEntry fragment in component.Fragments)
                        {
                           writer.WriteAttributeString(fragment.Key.ToString(), fragment.Value.ToString());
                        }

                        writer.WriteEndElement();
                     }
                  }
                  writer.WriteEndElement();

               }
               writer.WriteEndElement();

            }
            writer.WriteEndElement();

            // Cierra el documento
            writer.WriteEndDocument();
            writer.Close();
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      public void Clear()
      {
         _id = 0;
         _filename = string.Empty;
         _name = string.Empty;
         _desc = string.Empty;
         _version = string.Empty;
         _author = string.Empty;
         _platform = PresentationRule.DeviceTypes.Unknown;
         _renderer = string.Empty;
         _firstItemCssClass = string.Empty;
         _layoutPages = new Hashtable();
         _layoutComponents = new Hashtable();
         _contentComponents = new Hashtable();
         _scripts = new List<DomPageScript>();
      }

      #endregion

   }
}
