namespace Cosmo.Communications.PrivateMessages
{
   /// <summary>
   /// Implementa las funciones de mensajería privada como servicio.
   /// </summary>
   public class PrivateMessagesService
   {
      // Internal data declarations
      private Workspace _ws;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="PrivateMessagesService"/>.
      /// </summary>
      /// <param name="workspace">An instance of current Cosmo workspace</param>
      public PrivateMessagesService(Workspace workspace)
      {
         _ws = workspace;
      }

      #endregion

      #region Methods

      /// <summary>
      /// Obtiene el número de mensajes pendientes de leer.
      /// </summary>
      /// <returns>Un entero que indica el número de mensajes no leídos.</returns>
      public int CountUnreadMessages()
      {
         if (_ws.CurrentUser.IsAuthenticated)
         {
            PrivateMessageDAO pmdao = new PrivateMessageDAO(_ws);
            return pmdao.Count(_ws.CurrentUser.User.ID, PrivateMessage.UserMessageStatus.Unreaded);
         }
         else
         {
            return 0;
         }
      }

      #endregion

   }
}
