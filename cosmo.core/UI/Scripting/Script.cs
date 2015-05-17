using Cosmo.UI.Controls;
using System.IO;
using System.Text;

namespace Cosmo.UI.Scripting
{
   /// <summary>
   /// Interface que deben implementar todos los scripts.
   /// </summary>
   public abstract class Script
   {

      #region Enumerations

      /// <summary>
      /// Enumera los distintos tipos de llamada al código.
      /// </summary>
      public enum ScriptExecutionMethod
      {
         /// <summary>Se ejecuta cuando el documento está completamente cargado.</summary>
         OnDocumentReady,
         /// <summary>Se ejecuta cuando el documento está completamente cargado.</summary>
         OnEvent,
         OnFunctionCall,
         Standalone
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="Script"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      protected Script(ViewContainer container)
      {
         Initialize();

         this.ViewPort = container;
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="IScript"/>.
      /// </summary>
      /// <param name="viewport">Página o contenedor dónde se representará el control.</param>
      /// <param name="source"´>Una cadena que contiene código JavaScript.</param>
      protected Script(ViewContainer container, string source)
      {
         Initialize();

         this.ViewPort = container;
         this.Source.AppendLine(source);
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="IScript"/>.
      /// </summary>
      /// <param name="container">Página o contenedor dónde se representará el control.</param>
      /// <param name="source">Una instancia de <see cref="StringBuilder"/> que contiene código JavaScript.</param>
      protected Script(ViewContainer container, StringBuilder source)
      {
         Initialize();

         this.ViewPort = container;
         this.Source = source;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Indica si el script se debe ejecutar en el evento <c>$( document ).ready()</c> 
      /// de <em>jQuery</em>.
      /// </summary>
      public ScriptExecutionMethod ExecutionType { get; set; }

      /// <summary>
      /// Devuelve la pPágina o contenedor dónde se representará el control.
      /// </summary>
      public ViewContainer ViewPort { get; internal set; }

      /// <summary>
      /// Devuelve o establece el código fuente del script.
      /// </summary>
      internal StringBuilder Source { get; set; }

      #endregion 

      #region Methods

      /// <summary>
      /// Añade un comentario al código.
      /// </summary>
      /// <param name="comment"></param>
      public Script AppendComment(string comment)
      {
         Source.AppendLine("<!-- " + comment + " -->");
         return this;
      }

      /// <summary>
      /// Añade una línea de código fuente.
      /// </summary>
      /// <param name="code"></param>
      public Script AppendSourceLine(string code)
      {
         Source.AppendLine(code);
         return this;
      }

      /// <summary>
      /// Añade código fuente.
      /// </summary>
      /// <param name="code"></param>
      public Script AppendSource(string code)
      {
         Source.Append(code);
         return this;
      }

      /// <summary>
      /// Carga el código del script desde un archivo.
      /// </summary>
      /// <param name="filename">Nombre (con ruta) del archivo a cargar.</param>
      public void LoadFromFile(string filename)
      {
         string line;
         StringBuilder sb = new StringBuilder();

         StreamReader file = new StreamReader(filename);
         while ((line = file.ReadLine()) != null)
         {
            sb.AppendLine(line);
         }

         Source = sb;
      }

      #endregion

      #region Abstract Members

      /// <summary>
      /// Genera el código fuente (JavaScript).
      /// </summary>
      /// <returns>Una cadena de texto que contiene el código JavaScript generado.</returns>
      public abstract string GetSource();

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve el valor de un campo en el formulario via <c>scripting</c>.
      /// </summary>
      /// <param name="fieldDomId">Nombre del campo (DOM ID) para el que se desea obtener el valor.</param>
      /// <returns>Una secuéncia JavaScript (JQuery) que obtiene el valor del campo.</returns>
      public static string GetFieldValue(string fieldDomId)
      {
         return "$('#" + fieldDomId + "').val()";
      }

      /// <summary>
      /// Devuelve el valor de un campo en el formulario via <c>scripting</c>.
      /// </summary>
      /// <param name="formName">Nombre del formulario al que pertenece el campo.</param>
      /// <param name="field">Campo para el que se desea obtener el valor (actualmente sólo funciona para campos de tipo INPUT y SELECT).</param>
      /// <returns>Una secuéncia JavaScript (JQuery) que obtiene el valor del campo.</returns>
      public static string GetFieldValue(string formName, FormField field)
      {
         if (field is FormFieldList)
         {
            return "$('#" + formName + " select[name=" + field.DomID + "]').val()";
         }
         else
         {
            return "$('#" + formName + " input[name=" + field.DomID + "]').val()";
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.ExecutionType = ScriptExecutionMethod.Standalone;
         this.ViewPort = null;
         this.Source = new StringBuilder();
      }

      #endregion

   }
}
