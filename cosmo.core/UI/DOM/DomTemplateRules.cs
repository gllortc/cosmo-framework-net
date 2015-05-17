using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Cosmo.UI.DOM
{

   /// <summary>
   /// Implementa la clase que permite gestionar las reglas de presentación de Cosmo.
   /// </summary>
   public class DomTemplateRules
   {
      int _defaultTemplate;
      string _templatesPath;
      List<DomTemplateRule> _rules;

      /// <summary>Nombre del archivo que contiene las reglas de presentación.</summary>
      public const string FILE_NAME = "rules.xml";

      /// <summary>Versión de la definición de plantilla con la que está basada.</summary>
      internal const string ATTR_DEFINITION_VER = "cdt-ver";
      /// <summary>Versión de la definición de plantillas soportada.</summary>
      public const string RULES_DEFINITION_VER = "1.0";

      /// <summary>Sección raíz.</summary>
      internal const string KEY_ROOT = "rules";
      /// <summary>Clave que engloba la definición de una regla.</summary>
      internal const string KEY_RULE = "rule";

      /// <summary>Atributo que indica la plantilla a aplicar cuando no se cumple ninguna regla.</summary>
      internal const string ATTR_DEFAULT = "default";
      /// <summary>Atributo que indica la plantilla a aplicar cuando no se cumple ninguna regla.</summary>
      internal const string ATTR_CONTAINS = "contains";
      /// <summary>Atributo que indica la plantilla a aplicar cuando no se cumple ninguna regla.</summary>
      internal const string ATTR_REGEXP = "regexp";
      /// <summary>Atributo que indica la plantilla a aplicar cuando no se cumple ninguna regla.</summary>
      internal const string ATTR_TEMPLATE = "template";

      /// <summary>
      /// Devuelve una instancia de <see cref="DomTemplateRules"/>.
      /// </summary>
      /// <param name="templatesPath">Ruta raíz a la carpeta de plantillas.</param>
      /// <param name="loadRules">Indica si se deben cargar las reglas desde el archivo <c>rules.xml</c>.</param>
      public DomTemplateRules(string templatesPath, bool loadRules) 
      {
         DirectoryInfo path = new DirectoryInfo(templatesPath);
         if (!path.Exists)
            throw new DomTemplateRulesException("No se encuentra la ruta raíz para el sistema de plantillas.");

         _defaultTemplate = 0;
         _rules = new List<DomTemplateRule>();
         _templatesPath = path.FullName;

         if (loadRules)
            Load(GetRulesFilename());
      }

      #region Properties

      /// <summary>
      /// Devuelve o establece el identificador de la plantilla a aplicar cuando no se cumple ninguna regla.
      /// </summary>
      public int DefaultTemplate
      {
         get { return _defaultTemplate; }
         set { _defaultTemplate = value; }
      }

      /// <summary>
      /// Devuelve o establece la ruta raíz dónde reside el sistema de plantillas.
      /// </summary>
      public string TemplatesPath
      {
         get { return _templatesPath; }
         set { _templatesPath = value; }
      }

      /// <summary>
      /// Contiene la lista de reglas a aplicar.
      /// </summary>
      public List<DomTemplateRule> Rules
      {
         get { return _rules; }
         set { _rules = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Devuelve el nombre y la ruta del archivo que contiene las reglas de presentación.
      /// </summary>
      /// <returns></returns>
      public string GetRulesFilename()
      {
         return Path.Combine(_templatesPath, DomTemplateRules.FILE_NAME);
      }

      /// <summary>
      /// Obtiene la plantilla de presentación según las reglas establecidas y el dispositivo de acceso.
      /// </summary>
      /// <param name="UserAgent">Cadena de texto proporcionada por el navegador.</param>
      /// <returns>Una instancia de <see cref="DomTemplate"/> que representa la plantilla a aplicar.</returns>
      public DomTemplate GetTemplate(string UserAgent)
      {
         DomTemplate template = null;

         foreach (DomTemplateRule rule in _rules)
         {
            if (rule.Check(UserAgent))
            {
               template = new DomTemplate(Path.Combine(Path.Combine(_templatesPath, rule.Template.ToString()), DomTemplate.FILE_NAME));
               template.ID = rule.Template;
               return template;
            }
         }

         if (this.DefaultTemplate <= 0)
            throw new DomTemplateRulesException("No es posible determinar la plantilla de presentación a aplicar.");

         template = new DomTemplate(Path.Combine(Path.Combine(_templatesPath, this.DefaultTemplate.ToString()), DomTemplate.FILE_NAME));
         template.ID = this.DefaultTemplate;
         return template;
      }

      /// <summary>
      /// Carga las reglas desde el archivo especificado.
      /// </summary>
      /// <param name="filename">Nombre y ruta del archivo a cargar.</param>
      public void Load(string filename)
      {
         DomTemplateRule rule = null;

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
            if (!xnod.Name.ToLower().Equals(DomTemplateRules.KEY_ROOT))
               throw new Exception("El archivo XML proporcionado no corresponde a una definición de reglas de presentación de Cosmo.");
            if (xnod.Attributes[DomTemplateRules.ATTR_DEFINITION_VER].Value.ToString().CompareTo(DomTemplateRules.RULES_DEFINITION_VER) > 0)
               throw new Exception("La versión del archivo es más reciente que la versión soportada (" + DomTemplate.TEMPLATE_DEFINITION_VER + "). Consiga una versión anterior de la definición de reglas de presentación.");

            // Lee las propiedades principales
            foreach (XmlAttribute attr in xnod.Attributes)
            {
               switch (attr.Name.ToLower())
               {
                  case DomTemplateRules.ATTR_DEFAULT:
                     this.DefaultTemplate = int.Parse(attr.Value.ToString());
                     break;
               }
            }

            // Lee las reglas
            foreach (XmlNode node in xnod.ChildNodes)
            {
               switch (node.Name.ToLower())
               {
                  case DomTemplateRules.KEY_RULE:

                     rule = new DomTemplateRule();

                     foreach (XmlAttribute attr in node.Attributes)
                     {
                        switch (attr.Name.ToLower())
                        {
                           case DomTemplateRules.ATTR_TEMPLATE:
                              rule.Template = int.Parse(attr.Value.ToString());
                              break;

                           case DomTemplateRules.ATTR_CONTAINS:
                              rule.ContainText = attr.Value.ToString();
                              break;

                           case DomTemplateRules.ATTR_REGEXP:
                              rule.RegularExpression = attr.Value.ToString();
                              break;
                        }
                     }

                     _rules.Add(rule);

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
      /// Guarda las reglas al archivo <c>rules.xml</c>.
      /// </summary>
      public void Save()
      {
         Save(GetRulesFilename());
      }

      /// <summary>
      /// Guarda las reglas al archivo especificado.
      /// </summary>
      /// <param name="filename">Nombre y ruta del archivo dónde se guardarán las reglas de presentación.</param>
      public void Save(string filename)
      {
         try
         {
            // Abre el documento
            XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            writer.WriteStartDocument();

            // Abre una cláusula OBJECT y agrega sus atributos
            writer.WriteStartElement(DomTemplateRules.KEY_ROOT);
            writer.WriteAttributeString(DomTemplateRules.ATTR_DEFINITION_VER, DomTemplateRules.RULES_DEFINITION_VER);
            writer.WriteAttributeString(DomTemplateRules.ATTR_DEFAULT, _defaultTemplate.ToString());
            {

               foreach (DomTemplateRule rule in _rules)
               {
                  writer.WriteStartElement(DomTemplateRules.KEY_RULE);
                  writer.WriteAttributeString(DomTemplateRules.ATTR_CONTAINS, rule.ContainText);
                  writer.WriteAttributeString(DomTemplateRules.ATTR_REGEXP, rule.RegularExpression);
                  writer.WriteAttributeString(DomTemplateRules.ATTR_TEMPLATE, rule.Template.ToString());
                  writer.WriteEndElement();
               }

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

      #endregion

   }

}
