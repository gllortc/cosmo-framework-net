using Cosmo.Utils;
using System;

namespace Cosmo.Communications
{
   /// <summary>
   /// Interface que deben implementar todos los módulos de comunicaciones.
   /// </summary>
   public abstract class ICommunicationsModule
   {
      // Internal data declarations
      private Plugin _plugin;
      private Workspace _ws;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ICommunicationsModule"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      protected ICommunicationsModule(Workspace workspace, Plugin plugin)
      {
         Initialize();

         _plugin = plugin;
         _ws = workspace;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve el nombre (ID) del módulo de comunicaciones.
      /// </summary>
      public string ID
      {
         get { return _plugin.ID; }
      }

      /// <summary>
      /// Gets the plugin instance.
      /// </summary>
      public Plugin Plugin
      {
         get { return _plugin; }
      }

      #endregion

      #region Abstrat Members

      /// <summary>
      /// Devuelve el nombre qualificado de la classe que implementa los mensajes que permite enviar el módulo.
      /// </summary>
      public abstract string MessageQualifiedName { get; }

      /// <summary>
      /// Envia el mensaje del tipo indicado por el parámetro <c>MessageQualifiedName</c>.
      /// </summary>
      /// <param name="message"></param>
      public abstract void Send(Object message);

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _plugin = null;
         _ws = null;
      }

      #endregion

   }
}
