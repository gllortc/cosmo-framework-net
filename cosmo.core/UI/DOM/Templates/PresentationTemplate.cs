using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;

namespace Cosmo.UI.DOM.Templates
{
   /// <summary>
   /// Implementa una plantilla de presentación
   /// </summary>
   public class PresentationTemplate
   {
      private int _id = 0;
      private string _name = string.Empty;
      private string _description = string.Empty;
      private string _version = string.Empty;
      private string _author = string.Empty;
      private PresentationRule.DeviceTypes _type = PresentationRule.DeviceTypes.Unknown;
      private DateTime _created = DateTime.Now;
      private DateTime _updated = DateTime.Now;
      private string _owner = AuthenticationService.ACCOUNT_SUPER;
      private List<PresentationTemplatePart> _parts;
      private List<PresentationTemplateFile> _files;
      private bool _pageheaders = true;
      private string _renderer = string.Empty;

      /// <summary>
      /// Identificador del tipo de objeto
      /// </summary>
      public const string OBJECT_ID = "Cosmo.Template";

      /// <summary>
      /// Versión del formato de empaquetado de este tipo de objeto
      /// </summary>
      public const string OBJECT_VER = "3.0";

      /// <summary>
      /// Extensión de los archivo de empaquetado (serialización) de este tipo de objeto
      /// </summary>
      public const string OBJECT_FILEEXTENSION = "mwt";

      /// <summary>
      /// Devuelve una instancia de PresentationTemplate
      /// </summary>
      public PresentationTemplate()
      {
         // Reserva espacio para todas las partes
         int parts = Enum.GetValues(typeof(PresentationTemplatePart.DOMTemplateParts)).Length;
         _parts = new List<PresentationTemplatePart>(parts);
         _files = new List<PresentationTemplateFile>();
      }

      #region Properties

      /// <summary>
      /// Establece o devuelve el identificador de la plantilla de presentación
      /// </summary>
      public int ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Establece o devuelve el nombre de la plantilla de presentación
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Establece o devuelve la descripción de la plantilla de presentación
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Establece o devuelve el autor (o información de copyright) de la plantilla de presentación
      /// </summary>
      public string Author
      {
         get { return _author; }
         set { _author = value; }
      }

      /// <summary>
      /// Establece o devuelve la versión de la plantilla de presentación
      /// </summary>
      public string Version
      {
         get { return _version; }
         set { _version = value; }
      }

      /// <summary>
      /// Establece o devuelve el tipo de plataforma de la plantilla de presentación
      /// </summary>
      public PresentationRule.DeviceTypes Type
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// Establece o devuelve si se debe mostrar la estructura de la página o sólo el contenido
      /// generado (para integrar aplicaciones en otros portales o páginas)
      /// </summary>
      public bool UsePageHeaders
      {
         get { return _pageheaders; }
         set { _pageheaders = value; }
      }

      /// <summary>
      /// Establece o devuelve el nombre del archivo DLL (libreria de clases) que se usará para
      /// renderizar la página.
      /// </summary>
      /// <remarks>
      /// Si este campo se deja en blanco se usará el renderizado estándar de MobilerWhere, usando
      /// la información de la plantilla.
      /// </remarks>
      public string Renderer
      {
         get { return _renderer; }
         set { _renderer = value; }
      }

      /// <summary>
      /// Establece o devuelve la fecha de instalación de la plantilla de presentación
      /// </summary>
      public DateTime Created
      {
         get { return _created; }
         set { _created = value; }
      }

      /// <summary>
      /// Establece o devuelve la fecha de la última modificación de la plantilla de presentación
      /// </summary>
      public DateTime Updated
      {
         get { return _updated; }
         set { _updated = value; }
      }

      /// <summary>
      /// Establece o devuelve el login del propietario (instalador) de la plantilla de presentación
      /// </summary>
      public string Owner
      {
         get { return _owner; }
         set { _owner = value; }
      }

      /// <summary>
      /// Establece o devuelve el array de partes (fragmentos) de la plantilla de presentación
      /// </summary>
      public List<PresentationTemplatePart> Parts
      {
         get { return _parts; }
         set { _parts = value; }
      }

      /// <summary>
      /// Establece o devuelve el array de archivos adjuntos a la plantilla de presentación
      /// </summary>
      /// <remarks>
      /// Este array sólo se usa para tareas de instalación de plantillas empaquetadas en archivos
      /// MWT (XML).
      /// </remarks>
      public List<PresentationTemplateFile> Files
      {
         get { return _files; }
         set { _files = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Obtiene un fragmento de la plantilla.
      /// </summary>
      /// <param name="partid">Identificador del fragmento de plantilla.</param>
      /// <returns>Una instáncia de MWTemplatePart.</returns>
      public PresentationTemplatePart GetPart(PresentationTemplatePart.DOMTemplateParts partid)
      {
         try
         {
            foreach (PresentationTemplatePart part in _parts)
               if (part.ID == partid)
                  return part;

            return null;
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// Permite establecer el contenido de un fragmento de plantilla.
      /// </summary>
      /// <param name="partid">Identificador del fragmento de plantilla.</param>
      /// <param name="html">Una cadena con el código xHTML.</param>
      public void SetPart(PresentationTemplatePart.DOMTemplateParts partid, string html)
      {
         try
         {
            foreach (PresentationTemplatePart part in _parts)
            {
               if (part.ID == partid)
               {
                  part.Html = html;
                  return;
               }
            }
         }
         catch
         {
            throw;
         }
      }

      #endregion

   }
}
