using Cosmo.Communications.PrivateMessages;
using Cosmo.Utils;
using System;
using System.Collections.Generic;

namespace Cosmo.Communications
{
   /// <summary>
   /// Implementa el servicio de comunicaciones del workspace.
   /// </summary>
   public class CommunicationsService
   {
      // Internal data declarations
      private Workspace _ws;
      private PrivateMessagesService _upm;
      private Dictionary<string, ICommunicationsModule> _modules;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="CommunicationsService"/>.
      /// </summary>
      /// <param name="workspace">An instance of current Cosmo workspace</param>
      public CommunicationsService(Workspace workspace)
      {
         Initialize();

         _ws = workspace;

         LoadModules();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Permite acceder al servicio de mensajería privada entre usuarios del workspace.
      /// </summary>
      public PrivateMessagesService PrivateMessages
      {
         get 
         {
            if (_upm == null)
            {
               _upm = new PrivateMessagesService(_ws);
            }

            return _upm; 
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Envia un mensaje.
      /// </summary>
      /// <param name="message">Una instancia que representa el mensaje a enviar.</param>
      public void Send(Object message)
      {
         string moduleName = message.GetType().Namespace + "." + message.GetType().Name;

         if (_modules.ContainsKey(moduleName))
         {
            _modules[moduleName].Send(message);
         }
         else
         {
            throw new CommunicationsException("No se encuentra ningún modulo de comunicaciones que permita enviar mensajes del tipo #" + moduleName);
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _ws = null;
         _modules = new Dictionary<string, ICommunicationsModule>();
      }

      /// <summary>
      /// Carga los módulos de comunicaciones.
      /// </summary>
      private void LoadModules()
      {
         Type type = null;
         ICommunicationsModule _module;

         foreach (Plugin plugin in _ws.Settings.CommunicationModules.Plugins)
         {
            Object[] args = new Object[2];
            args[0] = _ws;
            args[1] = plugin;

            type = Type.GetType(plugin.Class, true, true);
            _module = (ICommunicationsModule)Activator.CreateInstance(type, args);

            if (_module != null)
            {
               _modules.Add(_module.MessageQualifiedName, _module);
            }
         }
      }

      #endregion

   }
}
